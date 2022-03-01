using System;
using System.IO;
using System.Text;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

/// <summary>
/// The Syncfusion.HtmlConverter namespace contains classes to convert HTML to PDF.
/// </summary>
namespace Syncfusion.HtmlConverter
{
    /// <summary>
    /// Class which allows converting HTML to PDF.
    /// </summary>
    /// <example>
    /// //Initialize HTML to PDF converter 
    /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
    /// CefConverterSettings cefSettings = new CefConverterSettings();
    /// htmlConverter.ConverterSettings = cefSettings;
    /// //Convert url to pdf
    /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
    /// MemoryStream stream = new MemoryStream();
    /// document.Save(stream);
    /// </example>
    public class HtmlToPdfConverter
    {
        #region Fields
        private HtmlRenderingEngine m_renderingEngine;
        private IHtmlConverterSettings m_converterSettings;
        private CefConverter m_cefConverter;
        private const string DEF_REGEX_URL_PATTERN = @"(([a-zA-Z][0-9a-zA-Z+\\-\\.]*:)?/{0,2}[0-9a-zA-Z;/?:@&=+$\\.\\-_!~*'()%]+)?(#[0-9a-zA-Z;/?:@&=+$\\.\\-_!~*'()%]+)?";
        private const string DEF_REGEX_HEADTAG_PATTERN = @"(?<HEAD_TAG_GROUP>\<\s*HEAD\s*[^\>]*\>)";
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the rendering engine used for conversion.
        /// </summary>
        /// <value> Specifies rendering engine (By default, Cef rendering engine).</value>
        /// <example>
        /// //Initialize the HTML to PDF converter 
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Set rendering engine for conversion
        /// htmlConverter.RenderingEngine = HtmlRenderingEngine.Cef;
        /// //Initialize the Cef converter settings
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Assign Cef settings to the HTML converter
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
        /// //Save and close the PDF document 
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="HtmlRenderingEngine"/>
        /// <seealso cref="CefConverterSettings"/> class
        public HtmlRenderingEngine RenderingEngine
        {
            get
            {
                return m_renderingEngine;
            }
            set
            {
                m_renderingEngine = value;
                ConverterSettings = new CefConverterSettings();
                CefConverter = new CefConverter();
            }
        }
        /// <summary>
        /// Gets or sets the rendering engine settings.
        /// </summary>
        /// <value> Specifies rendering engine settings.</value>
        public IHtmlConverterSettings ConverterSettings
        {
            get
            {
                return m_converterSettings;
            }
            set
            {
                m_converterSettings = value;
            }
        }

        internal CefConverter CefConverter
        {
            get
            {
                return m_cefConverter;
            }
            set
            {
                m_cefConverter = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlToPdfConverter"/> class.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter 
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// CefConverterSettings settings = new CefConverterSettings();
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert url to pdf
        /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="HtmlRenderingEngine"/>
        /// <seealso cref="CefConverterSettings"/> class
        public HtmlToPdfConverter()
        {
            RenderingEngine = HtmlRenderingEngine.Cef;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlToPdfConverter"/> class with specified <see cref="RenderingEngine"/>.
        /// </summary>
        /// <param name="renderingEngine">Rendering Engine used for conversion</param>
        /// <remarks>
        /// To know more details about HTML to PDF converter, refer this 
        /// <see href="https://help.syncfusion.com/file-formats/pdf/converting-html-to-pdf"> link</see>. 
        /// </remarks>
        /// <example>
        /// //Initialize the HTML to PDF converter 
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Cef);
        /// //Initialize the Cef converter settings
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Assign Cef settings to the HTML converter
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
        /// //Save and close the PDF document 
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="HtmlRenderingEngine"/>
        /// <seealso cref="PdfDocument"/> class
        public HtmlToPdfConverter(HtmlRenderingEngine renderingEngine)
        {
            RenderingEngine = renderingEngine;
        }
        #endregion

        #region Private member
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

        #region PublicMethods

        /// <summary>
        /// Converts URL to PdfDocument.
        /// </summary>
        /// <param name="url">Path to the HTML resource.</param>
        /// <returns>The PDF document</returns>
        /// <example>
        /// //Initialize HtmlToPdfConverter with CefConverterSettings
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Set webkitSettings to the html converter
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="HtmlRenderingEngine"/>
        /// <seealso cref="PdfDocument"/> class
        public PdfDocument Convert(string url)
        {
            PdfDocument document = new PdfDocument();

            CefConverter = UpdateCefSettings(ConverterSettings as CefConverterSettings);

            string tempFile = string.Empty;

            if (string.IsNullOrEmpty(CefConverter.TempPath.Trim()))
            {
                tempFile = Path.GetTempPath() + Guid.NewGuid();
            }
            else
            {
                if (Directory.Exists(CefConverter.TempPath))
                {
                    tempFile = Path.Combine(CefConverter.TempPath, Guid.NewGuid().ToString());
                }
                else
                {
                    throw new DirectoryNotFoundException("Path does not exists");
                }
            }
            CefConverter.TempFileName = tempFile;
            document = CefConverter.Convert(url);

            return document;
        }
        /// <summary>
        /// Converts HTML string to PdfDocument
        /// </summary>
        /// <param name="html">html string</param>
        /// <param name="baseurl">Path of the resource used in the HTML</param>
        /// <returns>PDF document</returns>
        /// <example>
        /// //Input HTML string
        /// string htmlString = "<HTML><BODY><H1>Welcome to Syncfusion.!</H1><P>Simple HTML content</P></BODY></HTML>";
        /// //Initialize HtmlToPdfConverter with CefConverterSettings
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Set Cef settings to the converter
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert HTML string to pdf
        /// PdfDocument document = htmlConverter.Convert(htmlString, "");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="HtmlRenderingEngine"/>
        /// <seealso cref="CefConverterSettings"/> class
        /// <seealso cref="PdfDocument"/> class
        public PdfDocument Convert(string html, string baseurl)
        {
            PdfDocument document = new PdfDocument();

            document.PageSettings.Size = ConverterSettings.PdfPageSize;
            document.PageSettings.Margins = ConverterSettings.Margin;
            document.PageSettings.Orientation = ConverterSettings.Orientation;
            document.PageSettings.Rotate = ConverterSettings.PageRotateAngle;

            CefConverter = UpdateCefSettings(ConverterSettings as CefConverterSettings);
            document = CefConverter.Convert(html, baseurl);

            //Return the output PDF document
            return document;
        }

        /// <summary>
        /// Converts HTML string to Image
        /// </summary>
        /// <param name="html">html string</param>
        /// <param name="baseurl">Path of the resource used in the HTML</param>
        /// <returns>Image</returns>
        /// <example>
        /// //Initialize HTML converter 
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Cef);
        /// //HTML string and Base URL 
        /// string htmlText = "<html><body><div id="pic"><img src="syncfusion_logo.gif"></img></div><p> Hello World</p></body></html>";
        /// string baseUrl = @"C:/Temp/HTMLFiles/";
        /// // Cef converter settings
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Assign the Cef settings
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert HTML to Image
        /// Image image = htmlConverter.ConvertToImage(htmlText, baseUrl);
        /// //Save Image
        /// File.WriteAllBytes("output.jpg", image.ImageData);
        /// </example>
        /// <seealso cref="Image"/> class
        /// <seealso cref="HtmlRenderingEngine"/>
        /// <seealso cref="CefConverterSettings"/> class
        public Image ConvertToImage(string html, string baseurl)
        {
            CefConverter = UpdateCefSettings(ConverterSettings as CefConverterSettings);
            Image[] totalresult = CefConverter.ConvertToImage(html, baseurl);
            Image resultImage = totalresult[0];

            //Return the output Image
            return resultImage;
        }

        /// <summary>
        /// Converts URL to Image
        /// </summary>
        /// <param name="url">Path to the Html resource.</param>
        /// <returns>Image</returns>
        /// <example>
        /// //Initialize HTML converter 
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Cef);
        /// // Cef converter settings
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Assign the Cef settings
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert HTML to Image
        /// Image image = htmlConverter.ConvertToImage("http://www.google.com");
        /// //Save Image
        /// File.WriteAllBytes("output.jpg", image.ImageData);
        /// </example>
        /// <seealso cref="Image"/> class
        /// <seealso cref="HtmlRenderingEngine"/>
        /// <seealso cref="CefConverterSettings"/> class
        public Image ConvertToImage(string url)
        {
            CefConverter = UpdateCefSettings(ConverterSettings as CefConverterSettings);
            Image[] totalresult = CefConverter.ConvertToImage(url);
            Image resultImage = totalresult[0];

            return resultImage;

        }
        /// <summary>
        /// Update specified settings to CefConverter
        /// </summary>
        internal CefConverter UpdateCefSettings(CefConverterSettings settings)
        {
            if (settings == null)
            {
                throw new PdfException("Unsupported settings: Please use CefConverterSettings");
            }
            CefConverter.AdditionalDelay = settings.AdditionalDelay;
            CefConverter.EnableHyperLink = settings.EnableHyperLink;
            CefConverter.TempPath = settings.TempPath;
            CefConverter.MediaType = settings.MediaType;
            CefConverter.PdfHeader = settings.PdfHeader;
            CefConverter.PdfFooter = settings.PdfFooter;
            CefConverter.HtmlEncoding = settings.HtmlEncoding;
            CefConverter.PdfPageSize = settings.PdfPageSize;
            CefConverter.ViewPortSize = settings.ViewPortSize;
            CefConverter.PageRotateAngle = settings.PageRotateAngle;
            CefConverter.Orientation = settings.Orientation;
            CefConverter.PdfMargins = settings.Margin;
            return CefConverter;
        }
        #endregion
    }

    #region Helper classes
    /// <summary>
    /// Interface for converter settings
    /// </summary> 
    /// <example>
    /// //Initialize HTML to PDF converter
    /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
    /// //Initialize the converter settings
    /// IHtmlConverterSettings converterSettings = new CefConverterSettings();
    /// //Assign converter settings to HTML converter
    /// htmlConverter.ConverterSettings = converterSettings;
    /// //Convert URL to PDF
    /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
    /// //Save the output PDF document
    /// MemoryStream stream = new MemoryStream();
    /// document.Save(stream);
    /// </example>
    public interface IHtmlConverterSettings
    {
        /// <summary>
        /// Gets or sets the temporary folder path where the temporary operations are performed if any; By default, system temporary folder path.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the converter settings
        /// IHtmlConverterSettings converterSettings = new CefConverterSettings();
        /// //Set temporary path for conversion
        /// converterSettings.TempPath = @"C:/HtmlConversion/Temp/";
        /// //Assign converter settings to HTML converter
        /// htmlConverter.ConverterSettings = converterSettings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        string TempPath
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the additional delay to load JavaScript, unit is Milliseconds. By default, 0.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the converter settings
        /// IHtmlConverterSettings converterSettings = new CefConverterSettings();
        /// //Set AdditionalDelay for conversion
        /// converterSettings.AdditionalDelay = 2000;
        /// //Assign converter settings to HTML converter
        /// htmlConverter.ConverterSettings = converterSettings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        int AdditionalDelay
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether to preserve the live-links in the converted document or not; By default, true.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the converter settings
        /// IHtmlConverterSettings converterSettings = new CefConverterSettings();
        /// //Set EnableHyperLink
        /// converterSettings.EnableHyperLink = true;
        /// //Assign converter settings to HTML converter
        /// htmlConverter.ConverterSettings = converterSettings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        bool EnableHyperLink
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether to enable or disable JavaScripts in the webpage; By default, true.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the converter settings
        /// IHtmlConverterSettings converterSettings = new CefConverterSettings();
        /// //Set EnableJavaScript
        /// converterSettings.EnableJavaScript = true;
        /// //Assign converter settings to HTML converter
        /// htmlConverter.ConverterSettings = converterSettings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        bool EnableJavaScript
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the password; By default, empty.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the converter settings
        /// IHtmlConverterSettings converterSettings = new CefConverterSettings();
        /// //Set username and password for windows authentication
        /// converterSettings.Username = "username";
        /// converterSettings.Password = "password";
        /// //Assign converter settings to HTML converter
        /// htmlConverter.ConverterSettings = converterSettings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        string Password
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the username; By default, empty.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the converter settings
        /// IHtmlConverterSettings converterSettings = new CefConverterSettings();
        /// //Set username and password for windows authentication
        /// converterSettings.Username = "username";
        /// converterSettings.Password = "password";
        /// //Assign converter settings to HTML converter
        /// htmlConverter.ConverterSettings = converterSettings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        string Username
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the margins to a PDF document; By default, all margins are 0.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter.
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the Cef converter settings.
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Set margins
        /// settings.Margin.All = 20;
        /// //Assign Cef settings to HTML converter.
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert HTML string to PDF.
        /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="PdfMargins"/> class
        PdfMargins Margin
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the Header to a PDF document; By default, null.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the Cef converter settings
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Create PDF page template element for header with bounds.
        /// PdfPageTemplateElement header = new PdfPageTemplateElement(new RectangleF(0, 0, settings.PdfPageSize.Width, 50));
        /// //Create font and brush for header element.
        /// PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 7);
        /// PdfBrush brush = new PdfSolidBrush(Color.Black);
        /// //Draw the header string in header template element. 
        /// header.Graphics.DrawString("This is header", font, brush, PointF.Empty);
        /// //Assign the header element to PdfHeader of Cef settings.
        /// settings.PdfHeader = header;
        /// //Assign Cef settings to HTML converter
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="PdfPageTemplateElement"/> class
        PdfPageTemplateElement PdfHeader
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the Footer to a PDF document; By default, null.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the Cef converter settings
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Create PDF page template element for footer with bounds.
        /// PdfPageTemplateElement footer = new PdfPageTemplateElement(new RectangleF(0, 0, settings.PdfPageSize.Width, 50));
        /// //Create font and brush for header element.
        /// PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 7);
        /// PdfBrush brush = new PdfSolidBrush(Color.Black);
        /// //Draw the footer string in footer template element. 
        /// footer.Graphics.DrawString("This is footer", font, brush, PointF.Empty);
        /// //Assign the footer element to PdfFooter of Cef settings.
        /// settings.PdfFooter = footer;
        /// //Assign Cef settings to HTML converter
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="PdfPageTemplateElement"/> class
        PdfPageTemplateElement PdfFooter
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the orientation of the PDF document; By default, Portrait.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the Cef converter settings
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Set PDF page orientation
        /// settings.Orientation = PdfPageOrientation.Landscape;
        /// //Assign Cef settings to HTML converter
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="PdfPageOrientation"/> enum
        PdfPageOrientation Orientation
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the size of a PDF page; By default, A4 size.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the Cef converter settings
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Set PDF page size
        /// settings.PdfPageSize = new SizeF(595, 842);
        /// //Assign Cef settings to HTML converter
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="Syncfusion.Pdf.PdfPageSize"/>
        SizeF PdfPageSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of degrees by which the page should be rotated clockwise when displayed or printed.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the Cef converter settings
        /// CefConverterSettings settings = new CefConverterSettings();
        /// //Set PDF page rotation angle
        /// settings.PageRotateAngle = PdfPageRotateAngle.RotateAngle90;
        /// //Assign Cef settings to HTML converter
        /// htmlConverter.ConverterSettings = settings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="PdfPageRotateAngle"/>
        PdfPageRotateAngle PageRotateAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets HtmlEncoding for HTML string to PDF conversion.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the converter settings
        /// IHtmlConverterSettings converterSettings = new CefConverterSettings();
        /// //Set Html Encoding
        /// converterSettings.HtmlEncoding = Encoding.Default;
        /// //Assign converter settings to HTML converter
        /// htmlConverter.ConverterSettings = converterSettings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        Encoding HtmlEncoding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a ViewPortSize size.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
        /// //Initialize the converter settings
        /// IHtmlConverterSettings converterSettings = new CefConverterSettings();
        /// //Assign converter settings to HTML converter
        /// (converterSettings as CefConverterSettings).ViewPortSize = new Size(800, 0);
        /// htmlConverter.ConverterSettings = converterSettings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("http://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        Size ViewPortSize
        {
            get;
            set;
        }

    }

    #endregion
    
    #region Cef Helper classes
    public class CefConverterSettings : IHtmlConverterSettings
    {
        #region Fields
        private string m_tempPath;
        private int m_additionalDelay;
        private bool m_enableHyperLink;
        private PdfMargins m_margin;
        private PdfPageTemplateElement m_pdfHeader;
        private PdfPageTemplateElement m_pdfFooter;
        private PdfPageOrientation m_orientation;
        private SizeF m_pdfPageSize;
        private PdfPageRotateAngle m_pageRotateAngle;
        private MediaType m_mediaType;
        private string m_password;
        private string m_username;
        private Encoding m_htmlEncoding;
        private Size viewportSize;

        #endregion

        #region Properties

        public string TempPath
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
        public int AdditionalDelay
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
        public bool EnableHyperLink
        {
            get
            {
                return m_enableHyperLink;
            }
            set
            {
                m_enableHyperLink = value;
            }
        }

        public PdfMargins Margin
        {
            get
            {
                return m_margin;
            }
            set
            {
                m_margin = value;
            }
        }

        public PdfPageTemplateElement PdfHeader
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
        public PdfPageTemplateElement PdfFooter
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

        public PdfPageOrientation Orientation
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
        public SizeF PdfPageSize
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

        public PdfPageRotateAngle PageRotateAngle
        {
            get
            {
                return m_pageRotateAngle;
            }
            set
            {
                m_pageRotateAngle = value;
            }
        }

        public MediaType MediaType
        {
            get
            {
                return m_mediaType;
            }
            set
            {
                m_mediaType = value;
            }
        }

        public Encoding HtmlEncoding
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
        public Size ViewPortSize
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
        bool IHtmlConverterSettings.EnableJavaScript
        {
            get;
            set;
        }

        string IHtmlConverterSettings.Password
        {
            get;
            set;
        }
        string IHtmlConverterSettings.Username
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CefConverterSettings"/> class.
        /// </summary>
        /// <example>
        /// //Initialize HTML to PDF converter with Cef rendering engine
        /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Cef);
        /// CefConverterSettings cefConverterSettings = new CefConverterSettings();
        /// //Assign Cef converter settings to HTML converter
        /// htmlConverter.ConverterSettings = cefConverterSettings;
        /// //Convert URL to PDF
        /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
        /// //Save the output PDF document
        /// MemoryStream stream = new MemoryStream();
        /// document.Save(stream);
        /// </example>
        /// <seealso cref="PdfMargins"/> class
        /// <seealso cref="PdfPageTemplateElement"/> class
        /// <seealso cref="Syncfusion.HtmlConverter.MediaType"/>
        /// <seealso cref="PdfPageRotateAngle"/>
        public CefConverterSettings()
        {
            EnableHyperLink = true;
            Margin = new PdfMargins();
            Orientation = PdfPageOrientation.Portrait;
            PdfPageSize = Syncfusion.Pdf.PdfPageSize.A4;
            TempPath = Path.GetTempPath();
            PdfHeader = null;
            PdfFooter = null;
            ViewPortSize = new Size(1024, 0);
            MediaType = MediaType.Screen;
            HtmlEncoding = Encoding.UTF8;
        }
        #endregion
    }
    #endregion

    #region Enums
    /// <summary>
    /// Specifies the Rendering Engine.
    /// </summary>
    /// <remarks>
    /// To know more details about rendering engines, refer this 
    /// <see href="https://help.syncfusion.com/file-formats/pdf/converting-html-to-pdf">link</see>.
    /// </remarks>
    /// <example>
    /// //Initialize the HTML to PDF converter 
    /// HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Cef);
    /// //Initialize the Cef converter settings
    /// CefConverterSettings settings = new CefConverterSettings();
    /// //Assign Cef settings to the HTML converter
    /// htmlConverter.ConverterSettings = settings;
    /// //Convert URL to PDF
    /// PdfDocument document = htmlConverter.Convert("https://www.google.com");
    /// //Save the PDF document 
    /// MemoryStream stream = new MemoryStream();
    /// document.Save(stream);
    /// </example>
    /// <seealso cref="HtmlToPdfConverter"/>
    /// <seealso cref="PdfDocument"/>
    public enum HtmlRenderingEngine
    {
        /// <summary>
        /// Cef rendering engine.
        /// </summary>
        Cef
    }
    /// <summary>
    /// Specifies the media type for HTML conversion.
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// Print media type
        /// </summary>
        Print,
        /// <summary>
        /// Screen media type
        /// </summary>
        Screen
    }

    #endregion
}