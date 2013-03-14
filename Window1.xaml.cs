using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dynamsoft.DotNet.TWAIN;
using System.Windows.Forms;
using System.Windows.Controls;
using System.IO;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        
        public Window1()
        {
            
            InitializeComponent();
        }
        DynamicDotNetTwain objDynamicDotNetTwain = null;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();

            this.ddlResultFormat.Items.Add("Text");
            this.ddlResultFormat.Items.Add("PDF Plain Text");
            this.ddlResultFormat.Items.Add("PDF Image Over Text");
            this.ddlResultFormat.SelectedIndex = 0;

            objDynamicDotNetTwain = new DynamicDotNetTwain();
            objDynamicDotNetTwain.Width = 200;
            objDynamicDotNetTwain.Height = 300;
            host.Child = objDynamicDotNetTwain;
            this.grid1.Children.Add(host);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            objDynamicDotNetTwain.SelectSource();
        }

        private void btnAcquire_Click(object sender, RoutedEventArgs e)
        {          
            objDynamicDotNetTwain.IfDisableSourceAfterAcquire = true;
            objDynamicDotNetTwain.AcquireImage();
            if(!objDynamicDotNetTwain.SaveAsBMP(@"C:\Users\Victor\Desktop\Nueva carpeta\test.bmp", 1))
                System.Windows.MessageBox.Show(this.objDynamicDotNetTwain.ErrorString);

            
        }

        private void btnOCR_Click(object sender, RoutedEventArgs e)
        {
            string languageFolder = System.Windows.Forms.Application.StartupPath;

            this.objDynamicDotNetTwain.OCRTessDataPath = languageFolder;
            this.objDynamicDotNetTwain.OCRLanguage = "eng";
            this.objDynamicDotNetTwain.OCRDllPath = @"C:\Users\Victor\Desktop\Nueva carpeta\";
            //3) Choose the OCR result file format and save. Supported file format includes Text, PDF Plain Text and PDF Image over Text. By setting the format to PDF Image over Text, the detailed image/text position and format, such as font names, font sizes, line widths and more, will keep as original.

            // Collapse | Copy Code
            this.objDynamicDotNetTwain.OCRResultFormat = (Dynamsoft.DotNet.TWAIN.OCR.ResultFormat)this.ddlResultFormat.SelectedIndex;


            byte[] sbytes = this.objDynamicDotNetTwain.OCR(this.objDynamicDotNetTwain.CurrentSelectedImageIndicesInBuffer);

            if (sbytes != null)
            {
                SaveFileDialog filedlg = new SaveFileDialog();
                if (this.ddlResultFormat.SelectedIndex != 0)
                {
                    filedlg.Filter = "PDF File(*.pdf)| *.pdf";
                }
                else
                {
                    filedlg.Filter = "Text File(*.txt)| *.txt branch1123fsdasdf";
                }

                if (filedlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileStream fs = File.OpenWrite(filedlg.FileName);
                    fs.Write(sbytes, 0, sbytes.Length);
                    fs.Close();
                }
            }
            else
            {
                System.Windows.MessageBox.Show(this.objDynamicDotNetTwain.ErrorString);
            }
        }

    }
}
