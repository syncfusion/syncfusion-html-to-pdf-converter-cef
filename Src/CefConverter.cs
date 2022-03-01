using CefSharp;
using CefSharp.DevTools;
using CefSharp.DevTools.Page;
using CefSharp.OffScreen;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using Syncfusion.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Syncfusion.HtmlConverter
{
    internal class CefConverter
    {
        #region Field

        private const int DEF_CEFVIEWPORT = 800;
        private const string DEF_REGEX_URL_PATTERN = @"(([a-zA-Z][0-9a-zA-Z+\\-\\.]*:)?/{0,2}[0-9a-zA-Z;/?:@&=+$\\.\\-_!~*'()%]+)?(#[0-9a-zA-Z;/?:@&=+$\\.\\-_!~*'()%]+)?";
        private const string DEF_REGEX_HEADTAG_PATTERN = @"(?<HEAD_TAG_GROUP>\<\s*HEAD\s*[^\>]*\>)";
        private const string PDF = "pdf";
        private const string JPG = "jpg";
        private const int DEF_TIMEOUT = 50;
        private Exception exception = null;
        private string m_tempPath;
        private string m_tempUserDir = string.Empty;
        private string format;
        private Stream m_outputStream = null;
        private string m_tempFileName;
        private Encoding m_htmlEncoding;
        private int m_additionalDelay;
        private bool m_enableHyperlink;
        private bool m_enableJavaScript;
        private MediaType mediaType;
        private PdfMargins m_pdfMargins;
        private PdfPageTemplateElement m_pdfHeader;
        private PdfPageTemplateElement m_pdfFooter;
        private SizeF m_pdfPageSize;
        private PdfPageOrientation m_orientation;
        private PdfPageRotateAngle rotateAngle;
        private string m_username = string.Empty;
        private string m_password = string.Empty;
        private Size viewportSize;
        private double m_scale;

        #endregion

        #region properties
        internal string TempPath
        {
            get
            {
                return m_tempPath;
            }
            set
            {
                m_tempPath = value;
            }
        }
        internal string TempUserDir
        {
            get
            {
                return m_tempUserDir;
            }
            set
            {
                m_tempUserDir = value;
            }
        }
        internal Stream OutputStream
        {
            get
            {
                return m_outputStream;
            }
            set
            {
                m_outputStream = value;
            }
        }
        internal string TempFileName
        {
            get
            {
                return m_tempFileName;
            }
            set
            {
                m_tempFileName = value;
            }
        }
        internal Encoding HtmlEncoding
        {
            get
            {
                return m_htmlEncoding;
            }
            set
            {
                m_htmlEncoding = value;
            }
        }
        internal int AdditionalDelay
        {
            get
            {
                return m_additionalDelay;
            }
            set
            {
                m_additionalDelay = value;
            }
        }
        internal bool EnableHyperLink
        {
            get
            {
                return m_enableHyperlink;
            }
            set
            {
                m_enableHyperlink = value;
            }
        }
        internal bool EnableJavaScript
        {
            get
            {
                return m_enableJavaScript;
            }
            set
            {
                m_enableJavaScript = value;
            }
        }
        internal MediaType MediaType
        {
            get
            {
                return mediaType;
            }
            set
            {
                mediaType = value;
            }
        }
        internal PdfMargins PdfMargins
        {
            get
            {
                return m_pdfMargins;
            }
            set
            {
                m_pdfMargins = value;
            }
        }
        internal PdfPageTemplateElement PdfHeader
        {
            get
            {
                return m_pdfHeader;
            }
            set
            {
                m_pdfHeader = value;
            }
        }

        internal PdfPageTemplateElement PdfFooter
        {
            get
            {
                return m_pdfFooter;
            }
            set
            {
                m_pdfFooter = value;
            }
        }
        internal string Username
        {
            get
            {
                return m_username;
            }
            set
            {
                m_username = value;
            }
        }
        internal string Password
        {
            get
            {
                return m_password;
            }
            set
            {
                m_password = value;
            }
        }

        internal PdfPageRotateAngle PageRotateAngle
        {
            get
            {
                return rotateAngle;
            }
            set
            {
                rotateAngle = value;
            }
        }
        internal SizeF PdfPageSize
        {
            get
            {
                return m_pdfPageSize;
            }
            set
            {
                m_pdfPageSize = value;
            }
        }
        internal PdfPageOrientation Orientation
        {
            get
            {
                return m_orientation;
            }
            set
            {
                m_orientation = value;
            }
        }
        internal Size ViewPortSize
        {
            get
            {
                return viewportSize;
            }
            set
            {
                viewportSize = value;
            }
        }
        internal double Scale
        {
            get
            {
                return m_scale;
            }
            set
            {
                m_scale = value;
            }
        }
        #endregion

        #region Methods
        internal PdfDocument Convert(string url)
        {
            UriBuilder uri = new UriBuilder(url);
            url = uri.Uri.ToString();

            format = PDF;

            if (PdfHeader != null)
                PdfMargins.Top += PdfHeader.Height;
            if (PdfFooter != null)
                PdfMargins.Bottom += PdfFooter.Height;

            PdfDocument document = new PdfDocument();

            ConvertToPdf(url, ref document);

            if (exception != null)
                throw new PdfException(exception.Message, exception);
            return document;

        }
        internal PdfDocument Convert(string htmlString, string baseurl)
        {
            string htmlTempPath = string.Empty;
            if (string.IsNullOrEmpty(TempFileName))
            {
                if (!string.IsNullOrEmpty(TempPath.Trim()))
                {
                    if (Directory.Exists(TempPath))
                    {
                        htmlTempPath = Path.Combine(TempPath, Guid.NewGuid().ToString() + ".html");
                    }
                    else
                    {
                        throw new DirectoryNotFoundException("Path does not exists");
                    }
                }
                else
                {
                    htmlTempPath = Path.GetTempPath() + Guid.NewGuid() + ".html";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(TempPath.Trim()))
                {
                    if (Directory.Exists(TempPath))
                    {
                        DirectoryInfo info = Directory.CreateDirectory(Path.Combine(TempPath, Guid.NewGuid().ToString()));
                        htmlTempPath = Path.Combine(info.FullName, TempFileName + ".html");
                    }
                    else
                    {
                        throw new DirectoryNotFoundException("Path does not exists");
                    }
                }
                else
                {
                    TempPath = Path.GetTempPath();
                    DirectoryInfo info = Directory.CreateDirectory(Path.Combine(TempPath, Guid.NewGuid().ToString()));
                    htmlTempPath = Path.Combine(info.FullName, TempFileName + ".html");
                }
            }


            if (htmlString.Length != 0 && baseurl != null && baseurl != "")
            {
                if (!Regex.IsMatch(baseurl, DEF_REGEX_URL_PATTERN))
                    baseurl = "about:blank";

                if ((!baseurl.EndsWith("/")) && (!baseurl.EndsWith("\\")))
                    baseurl += "/";

                Match match = Regex.Match(htmlString, DEF_REGEX_HEADTAG_PATTERN, RegexOptions.IgnoreCase);
                if (match != null && !string.IsNullOrEmpty(baseurl.Trim()))
                {
                    Group group = match.Groups["HEAD_TAG_GROUP"];
                    htmlString = htmlString.Insert(group.Index + group.Length, string.Format("<BASE HREF=\"{0}\">", baseurl));
                }
            }
            if (!File.Exists(htmlTempPath))
                File.WriteAllText(htmlTempPath, htmlString, HtmlEncoding);

            PdfDocument document = Convert(htmlTempPath);

            DeleteFile(htmlTempPath);

            if (!string.IsNullOrEmpty(TempFileName))
            {
                DirectoryInfo info = new DirectoryInfo(htmlTempPath);
                Directory.Delete(info.Parent.FullName, true);
            }

            return document;
        }
        internal void ConvertToPdf(string url, ref PdfDocument document)
        {
            try
            {
                TaskAwaiter taskAwaiter = ConvertAsync(url).GetAwaiter();

                while (!taskAwaiter.IsCompleted)
                {

                }
                document = CefResult(OutputStream);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }
        internal Image[] ConvertToImage(string url)
        {
            UriBuilder uri = new UriBuilder(url);
            url = uri.Uri.ToString();

            format = JPG;

            Image[] outputImage = null;

            ConvertToImage(url, ref outputImage);

            return outputImage;
        }
        internal Image[] ConvertToImage(string htmlString, string baseurl)
        {
            string htmlTempPath = string.Empty;
            if (string.IsNullOrEmpty(TempFileName))
            {
                if (!string.IsNullOrEmpty(TempPath.Trim()))
                {
                    if (Directory.Exists(TempPath))
                    {
                        htmlTempPath = Path.Combine(TempPath, Guid.NewGuid().ToString() + ".html");
                    }
                    else
                    {
                        throw new DirectoryNotFoundException("Path does not exists");
                    }
                }
                else
                {
                    htmlTempPath = Path.GetTempPath() + Guid.NewGuid() + ".html";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(TempPath.Trim()))
                {
                    if (Directory.Exists(TempPath))
                    {
                        htmlTempPath = Path.Combine(TempPath, TempFileName + ".html");
                    }
                    else
                    {
                        throw new DirectoryNotFoundException("Path does not exists");
                    }
                }
                else
                {
                    htmlTempPath = Path.GetTempPath() + TempFileName + ".html";
                }
            }


            if (htmlString.Length != 0 && baseurl != null && baseurl != "")
            {
                if (!Regex.IsMatch(baseurl, DEF_REGEX_URL_PATTERN))
                    baseurl = "about:blank";

                if ((!baseurl.EndsWith("/")) && (!baseurl.EndsWith("\\")))
                    baseurl += "/";

                Match match = Regex.Match(htmlString, DEF_REGEX_HEADTAG_PATTERN, RegexOptions.IgnoreCase);
                if (match != null && !string.IsNullOrEmpty(baseurl.Trim()))
                {
                    Group group = match.Groups["HEAD_TAG_GROUP"];
                    htmlString = htmlString.Insert(group.Index + group.Length, string.Format("<BASE HREF=\"{0}\">", baseurl));
                }
            }
            if (!File.Exists(htmlTempPath))
                File.WriteAllText(htmlTempPath, htmlString, HtmlEncoding);

            Image[] outputImage = ConvertToImage(htmlTempPath);

            DeleteFile(htmlTempPath);

            if (!string.IsNullOrEmpty(TempFileName))
            {
                DirectoryInfo info = new DirectoryInfo(htmlTempPath);
                Directory.Delete(info.Parent.FullName, true);
            }

            return outputImage;
        }
        internal void ConvertToImage(string url, ref Image[] outputImage)
        {
            try
            {
                TaskAwaiter taskAwaiter = ConvertAsync(url).GetAwaiter();

                while (!taskAwaiter.IsCompleted)
                {
                }
                if (OutputStream != null)
                    outputImage = new Image[] { Image.FromStream(OutputStream) };
                else
                    throw new PdfException("Failed to convert webpage");
            }
            catch (Exception ex)
            {
                throw new PdfException(ex.Message);
            }
        }

        internal async Task ConvertAsync(string url)
        {
            try
            {
                ChromiumWebBrowser webBrowser = new ChromiumWebBrowser(url);

                await webBrowser.WaitForInitialLoadAsync();
                DevToolsClient client = webBrowser.GetDevToolsClient();

                await client.Emulation.SetEmulatedMediaAsync(MediaType.ToString());
                Viewport viewport = new Viewport();
                viewport.Width = viewportSize.Width;
                viewport.Height = viewportSize.Height;
                viewport.Scale = 1.0;

                await client.Emulation.SetDeviceMetricsOverrideAsync((int)viewport.Width, (int)viewport.Height, viewport.Scale, false);

                if (format == PDF)
                {
                    PdfPrintSettings pdfPrintSettings = new PdfPrintSettings();
                    pdfPrintSettings.BackgroundsEnabled = true;

                    if (Scale == 0)
                    {
                        double scaleFactor = DEF_CEFVIEWPORT / double.Parse(ViewPortSize.Width.ToString());
                        if (scaleFactor <= 1 && scaleFactor > 0)
                            Scale = scaleFactor * 100;
                    }
                    else
                    {
                        Scale = Scale * 100;
                    }
                    pdfPrintSettings.ScaleFactor = (int)Scale;

                    if (Orientation == PdfPageOrientation.Landscape)
                        pdfPrintSettings.Landscape = true;

                    pdfPrintSettings.MarginTop = (int)PdfMargins.Top;
                    pdfPrintSettings.MarginBottom = (int)PdfMargins.Bottom;
                    pdfPrintSettings.MarginLeft = (int)PdfMargins.Left;
                    pdfPrintSettings.MarginRight = (int)PdfMargins.Right;

                    pdfPrintSettings.MarginType = CefPdfPrintMarginType.Custom;

                    string fileName = Guid.NewGuid().ToString();
                    string tempFilePath = Path.Combine(Path.GetTempPath(), fileName + ".pdf");

                    await webBrowser.PrintToPdfAsync(tempFilePath, pdfPrintSettings);
                    
                    byte[] tempPdfData = File.ReadAllBytes(tempFilePath);
                    OutputStream = new MemoryStream(tempPdfData);

                    DeleteFile(tempFilePath);
                }
                else
                {
                    byte[] byteData = await webBrowser.CaptureScreenshotAsync(CaptureScreenshotFormat.Jpeg, 100, viewport);
                    OutputStream = new MemoryStream(byteData);
                }
            }
            catch (Exception exception)
            {
                throw new PdfException(exception.ToString(), exception.InnerException);
            }
            finally
            {
                DeleteFile(TempPath);
            }

        }

        private PdfDocument CefResult(Stream outputStream)
        {
            PdfDocument document = new PdfDocument();
            document.PageSettings.Rotate = PageRotateAngle;

            if (OutputStream != null)
            {
                PdfLoadedDocument ldDoc = new PdfLoadedDocument(outputStream);
                int pageCount = ldDoc.PageCount;

                for (int i = pageCount - 1; i >= 0; i--)
                {
                    if (!EnableHyperLink)
                    {
                        PdfAnnotationCollection annotColl = ldDoc.Pages[i].Annotations as PdfLoadedAnnotationCollection;
                        if (annotColl != null)
                        {
                            for (int j = annotColl.Count - 1; j >= 0; j--)
                            {
                                PdfLoadedAnnotation annot = annotColl[j] as PdfLoadedAnnotation;
                                if (annot != null && (annot is PdfLoadedTextWebLinkAnnotation || annot is PdfLoadedUriAnnotation || annot is PdfLoadedDocumentLinkAnnotation))
                                    annotColl.RemoveAt(j);
                            }
                        }
                    }
                }

                document.ImportPageRange(ldDoc, 0, pageCount - 1);

                if (PdfHeader != null)
                {
                    PdfHeader.Foreground = true;
                    PdfMargins.Top -= PdfHeader.Height;
                    document.Template.Top = PdfHeader;
                }
                if (PdfFooter != null)
                {
                    PdfFooter.Foreground = true;
                    PdfMargins.Bottom -= PdfFooter.Height;
                    document.Template.Bottom = PdfFooter;
                }
                document.Template.blinkMargin = PdfMargins;
            }
            else
            {
                throw new PdfException("Failed to convert webpage");
            }

            return document;
        }
        #endregion

        #region Helper 
        /// <summary>
        /// Delete the file
        /// </summary>
        private void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        #endregion
    }
}
