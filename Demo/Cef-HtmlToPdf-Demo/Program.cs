using System;
using System.IO;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;

namespace Cef_HtmlToPdf_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Cef);
            CefConverterSettings settings = new CefConverterSettings();

            //Assign CEF settings to HTML converter
            htmlConverter.ConverterSettings = settings;

            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert("https://www.google.com");

            FileStream stream = new FileStream("HtmlToPdf.pdf", FileMode.CreateNew);

            //Save and close the PDF document 
            document.Save(stream);
            document.Close(true);
            stream.Close();
        }
    }
}
