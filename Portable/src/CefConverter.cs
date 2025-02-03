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
using Newtonsoft.Json;
using Syncfusion.Pdf.HtmlToPdf;
using System.Net;
using CefSharp.DevTools.Network;
using System.Linq;
using static Syncfusion.HtmlConverter.CefConverterSettings;
using CookieCollection = Syncfusion.HtmlConverter.CefConverterSettings.CookieCollection;

namespace Syncfusion.HtmlConverter
{
    internal class CefConverter
    {
        #region Field

        public string log = string.Empty;
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
        private bool m_enableHyperlink = true;
        private bool m_enableJavaScript = true;
        private MediaType mediaType;
        private PdfMargins m_pdfMargins;
        private PdfPageTemplateElement m_pdfHeader;
        private PdfPageTemplateElement m_pdfFooter;
        private SizeF m_pdfPageSize;
        private PdfPageOrientation m_orientation;
        private PdfPageRotateAngle rotateAngle;
        private static string m_username = string.Empty;
        private static string m_password = string.Empty;
        private Size viewportSize;
        private HttpRequestHeaderCollection m_httpRequestHeaders = new HttpRequestHeaderCollection();
        private static HttpPostFieldCollection m_httpPostFields = new HttpPostFieldCollection();
        private CefConverterSettings.CookieCollection m_cookies;
        private string m_windowStatus;
        private CefProxySettings m_proxySettings = new CefProxySettings();
        private string m_htmlElementID;
        private bool isLayoutResult;
        private PdfLayoutResult m_layoutResult;
        private string m_layoutLink = string.Empty;
        private SinglePageLayout m_singlePageLayout;
        private bool m_enableLocalFileAccess;
        private bool m_imageLoading;
        private CefCommandLineArguments m_commandLineArguments;
        private double m_scale;
        private bool m_enableForm;
        private bool m_enableBookmarks;
        private bool m_enableToc;
        private HtmlToPdfToc m_toc = new HtmlToPdfToc();
        private List<CefForm> m_cefFormCollection = new List<CefForm>();
        private string m_formLink = string.Empty;
        private string m_formQuery = string.Empty;
        private bool m_enableOfflineMode;
        //constant fields for form fields
        private string TEXTBOX = "text";
        private string TEXTAREA = "textarea";
        private string SUBMIT = "submit";
        private string BUTTON = "button";
        private string CHECKBOX = "checkbox";
        private string RADIOBUTTON = "radio";
        private string SELECTBOX = "select";
        private string PASSWORD = "password";
        private string NUMBER = "number";
        private string EMAIL = "email";
        private string LISTBOX = "select-multiple";
        private string LINKCOLOR = "#FFFFFF00";
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
        internal bool EnableOfflineMode
        {
            get
            {
                return m_enableOfflineMode;
            }
            set
            {
                m_enableOfflineMode = value;
            }
        }
        internal CefCommandLineArguments CommandLineArguments
        {
            get
            {
                return m_commandLineArguments;
            }
            set
            {
                m_commandLineArguments = value;
            }
        }
        internal CefProxySettings ProxySettings
        {
            get
            {
                return m_proxySettings;
            }
            set
            {
                m_proxySettings = value;
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
        internal bool EnableForm
        {
            get
            {
                return m_enableForm;
            }
            set
            {
                m_enableForm = value;
            }
        }
        internal bool EnableBookmarks
        {
            get
            {
                return m_enableBookmarks;
            }
            set
            {
                m_enableBookmarks = value;
            }
        }
        internal bool EnableToc
        {
            get
            {
                return m_enableToc;
            }
            set
            {
                m_enableToc = value;
            }
        }
        /// <summary>
        /// Gets or sets TOC styles
        /// /// <para>This property is used internally and should not be used directly.</para>
        /// </summary>
        internal HtmlToPdfToc Toc
        {
            get
            {
                return m_toc;

            }
            set
            {
                m_toc = value;
            }
        }
        internal string HtmlElementID
        {
            get
            {
                return m_htmlElementID;
            }
            set
            {
                m_htmlElementID = value;
            }
        }
        internal HttpPostFieldCollection HttpPostFields
        {
            get
            {
                return m_httpPostFields;
            }
            set
            {
                m_httpPostFields = value;
            }
        }
        internal HttpRequestHeaderCollection HttpRequestHeaders
        {
            get
            {
                return m_httpRequestHeaders;
            }
            set
            {
                m_httpRequestHeaders = value;
            }
        }
        internal CookieCollection Cookies
        {
            get
            {
                return m_cookies;
            }
            set
            {
                m_cookies = value;
            }
        }
        internal PdfLayoutResult CefLayoutResult
        {
            get
            {
                return m_layoutResult;
            }
            set
            {
                m_layoutResult = value;
            }
        }
        internal bool IsLayoutResult
        {
            get
            {
                return isLayoutResult;
            }
            set
            {
                isLayoutResult = value;
            }
        }
        internal string WindowStatus
        {
            get
            {
                return m_windowStatus;
            }
            set
            {
                m_windowStatus = value;
            }
        }
        internal SinglePageLayout SinglePageLayout
        {
            get
            {
                return m_singlePageLayout;
            }
            set
            {
                m_singlePageLayout = value;
            }
        }
        internal bool EnableLocalFileAccess
        {
            get
            {
                return m_enableLocalFileAccess;
            }
            set
            {
                m_enableLocalFileAccess = value;
            }
        }
        internal bool LoadLazyImages
        {
            get
            {
                return m_imageLoading;
            }
            set
            {
                m_imageLoading = value;
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
                    System.Text.RegularExpressions.Group group = match.Groups["HEAD_TAG_GROUP"];
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
                CreateTempUserDir();
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
            finally
            {
                DeleteTempUserDir();
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
                    System.Text.RegularExpressions.Group group = match.Groups["HEAD_TAG_GROUP"];
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
        public class CustomRequestHandler : CefSharp.Handler.RequestHandler
        {
            protected override bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
            {
                //Cast IWebBrowser to ChormiumWebBrowser if you need to access the UI control
                //You can Invoke onto the UI Thread if you need to show a dialog
                var b = (ChromiumWebBrowser)chromiumWebBrowser;
                if (!isProxy)
                {
                    using (callback)
                    {
                        callback.Continue(username: m_username, password: m_password);
                    }
                    b.LoadUrlAsync(originUrl);
                    return true;
                }

                //Return false to cancel the request
                return false;
            }
            protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
            {
                //see the post data
                if (request.Url != "about:blank" && m_httpPostFields.Count != 0)
                {
                    return new CustomResourceRequestHandler();
                }
                return null;
            }
        }
        public class CustomResourceRequestHandler : CefSharp.Handler.ResourceRequestHandler
        {
            protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
            {
                string httpPostData = string.Empty;
                foreach (KeyValuePair<string, string> data in m_httpPostFields)
                {
                    httpPostData += data.Key + "=" + data.Value + "&";
                }
                httpPostData = httpPostData.Substring(0, httpPostData.Length - 1);
                var postData = new PostData();

                postData.AddData(httpPostData);
                request.Method = System.Net.Http.HttpMethod.Post.ToString();
                request.PostData = postData;
                //Set the Content-Type header to whatever suites your requirement
                request.SetHeaderByName("Content-Type", "application/x-www-form-urlencoded", true);
                //Set additional Request headers as required.

                return CefReturnValue.Continue;
            }
        }
        internal async Task ConvertAsync(string url)
        {
            try
            {
                DevToolsClient client;
                IBrowserSettings browserSettings = null;
                IWindowInfo windowInfo = null;
                int webpageheight = 0;
                var settings = new CefSettings();
                settings.LogSeverity = LogSeverity.Disable;
                foreach (string args in CommandLineArguments)
                {
                    settings.CefCommandLineArgs.Add(args, "0");
                }
                if (ProxySettings.Type != CefProxySettings.CefProxyType.None)
                {
                    var proxyUrl = ProxySettings.Type.ToString().ToLower() + "://" + ProxySettings.HostName + ":" + ProxySettings.PortNumber;
                    settings.CefCommandLineArgs.Add("proxy-server", proxyUrl);
                }
                if (!Cef.IsInitialized)
                {
                    var success = Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
                }
                using (var webBrowser = new ChromiumWebBrowser())
                {
                    await WaitForBrowserInitialization(webBrowser);
                    await webBrowser.LoadUrlAsync(url);
                    var onUi = Cef.CurrentlyOnThread(CefThreadIds.TID_UI);

                    if (browserSettings == null)
                    {
                        browserSettings = CefSharp.Core.ObjectFactory.CreateBrowserSettings(autoDispose: true);
                    }
                    if (!EnableJavaScript)
                    {
                        browserSettings.Javascript = CefState.Disabled;

                    }
                    if ((!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password)) || (!string.IsNullOrEmpty(ProxySettings.Username) && !string.IsNullOrEmpty(ProxySettings.Password)) || (HttpPostFields != null && HttpPostFields.Count != 0))
                    {
                        webBrowser.RequestHandler = new CustomRequestHandler();
                    }
                    await webBrowser.WaitForInitialLoadAsync();
                    if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                    {
                        await webBrowser.WaitForRenderIdleAsync();
                    }
                    var mainFrame = webBrowser.GetMainFrame();

                    client = webBrowser.GetDevToolsClient();

                    await client.Emulation.SetEmulatedMediaAsync(MediaType.ToString());
                    if (EnableOfflineMode)
                    {
                        await client.Network.EmulateNetworkConditionsAsync(EnableOfflineMode, 0, -1, -1).ConfigureAwait(false);
                        await webBrowser.LoadUrlAsync(url);
                    }
                    if (HttpRequestHeaders.Count != 0)
                    {
                        var extraHeaders = new Headers();
                        foreach (KeyValuePair<string, string> keyValue in HttpRequestHeaders)
                        {
                            extraHeaders.SetCommaSeparatedValues(keyValue.Key, keyValue.Value);
                        }
                        await client.Network.SetExtraHTTPHeadersAsync(extraHeaders);
                        // enable events
                        await client.Network.EnableAsync();
                        await webBrowser.LoadUrlAsync(url);
                    }

                    if (Cookies.Count != 0)
                    {
                        var cookieManager = webBrowser.GetCookieManager();
                        foreach (KeyValuePair<string, string> keyValue in Cookies)
                        {
                            CefSharp.Cookie param = new CefSharp.Cookie();
                            param.Name = keyValue.Key;
                            param.Value = keyValue.Value;
                            await cookieManager.SetCookieAsync(webBrowser.Address, param);
                        }
                        await webBrowser.EvaluateScriptAsync<string>("document.cookie").ConfigureAwait(false);
                        await webBrowser.LoadUrlAsync(url).ConfigureAwait(false);
                    }
                    if (EnableForm)
                    {
                        string[] formList = { "input", "textarea", "select" };
                        for (int i = 0; i < formList.Length; i++)
                        {
                            int htmlFormCollection = await webBrowser.EvaluateScriptAsync<int>("document.getElementsByTagName('" + formList[i] + @"').length").ConfigureAwait(false);
                            for (int j = 0; j < htmlFormCollection; j++)
                            {
                                await GetFormFieldValue(webBrowser, formList[i], j).ConfigureAwait(false);
                            }
                        }
                    }
                  
                    //Replace the HtmlElementID content to body content for Partial HTML Conversion
                    if (!string.IsNullOrEmpty(HtmlElementID) && webBrowser.Address != "about:blank")
                    {
                        if (!HtmlElementID.StartsWith("#"))
                        {
                            HtmlElementID = "#" + HtmlElementID;
                        }
                        var element = await webBrowser.GetMainFrame().EvaluateScriptAsync(HtmlElementID).ConfigureAwait(false);

                        if (element != null)
                        {
                            string header = await webBrowser.GetMainFrame().EvaluateScriptAsync<string>("document.head.outerHTML").ConfigureAwait(false);
                            string baseurl = string.Empty;
                            if (!header.Contains("BASE HREF") && !header.Contains("base href"))
                            {
                                UriBuilder uri = new UriBuilder(url);
                                string portNumber = uri.Port.ToString();
                                if (portNumber.Length > 0 && portNumber != "-1")
                                {
                                    if (!string.IsNullOrEmpty(portNumber))
                                        baseurl = uri.Scheme + "://" + uri.Host + ":" + portNumber;
                                    else
                                        baseurl = uri.Uri.GetLeftPart(UriPartial.Authority);
                                }

                                baseurl = "<BASE HREF=\"" + baseurl + "\" />";
                            }
                            string htmlElement = await webBrowser.GetMainFrame().EvaluateScriptAsync<string>("document.querySelector('" + HtmlElementID + "').outerHTML").ConfigureAwait(false);

                            if (htmlElement.Contains("e-js"))
                            {
                                int canvasLength = await webBrowser.GetMainFrame().EvaluateScriptAsync<int>("document.getElementsByTagName('canvas').length").ConfigureAwait(false);

                                for (int i = 0; i < canvasLength; i++)
                                {
                                    string canvas = await webBrowser.GetMainFrame().EvaluateScriptAsync<string>("document.getElementsByTagName('canvas')[" + i + "].outerHTML").ConfigureAwait(false);

                                    if (htmlElement.Contains(canvas))
                                        htmlElement = htmlElement.Replace(canvas, string.Empty);
                                }

                            }
                            int scriptLength = await webBrowser.GetMainFrame().EvaluateScriptAsync<int>("document.body.getElementsByTagName('script').length").ConfigureAwait(false);
                            string scriptCollection = string.Empty;
                            for (int i = 0; i < scriptLength; i++)
                            {
                                string script = await webBrowser.GetMainFrame().EvaluateScriptAsync<string>("document.body.getElementsByTagName('script')[" + i + "].outerHTML").ConfigureAwait(false);
                                scriptCollection += script;
                            }
                            string content = ("<!DOCTYPE html><html>" + baseurl + header + "<body>" + htmlElement + scriptCollection + "</body></html>");

                            string htmlTempPath = string.Empty;
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

                            if (!File.Exists(htmlTempPath))
                                File.WriteAllText(htmlTempPath, content, HtmlEncoding);

                            await webBrowser.LoadUrlAsync(htmlTempPath).ConfigureAwait(false);

                            if (File.Exists(htmlTempPath))
                                File.Delete(htmlTempPath);

                        }
                    }
                    if (!EnableLocalFileAccess)
                    {
                        string header = await webBrowser.GetMainFrame().EvaluateScriptAsync<string>("document.head.outerHTML").ConfigureAwait(false);
                        string body = await webBrowser.GetMainFrame().EvaluateScriptAsync<string>("document.body.outerHTML").ConfigureAwait(false);
                        var localFilePath = body.Split(' ');
                        foreach (var localFile in localFilePath)
                        {
                            if (localFile.Contains("file:///"))
                            {
                                var filePath = localFile.Split('"');
                                foreach (var file in filePath)
                                {
                                    if (file.Contains("file:///"))
                                    {
                                        if (body.Contains(file))
                                            body = body.Replace(file, "about:blank");
                                    }
                                }
                            }
                        }
                        string content = ("<html>" + header + body + "</html>");
                        await webBrowser.SetMainFrameDocumentContentAsync(content).ConfigureAwait(false);
                    }
                    //Adding dummy hyperlink for LayoutResult and SinglePageLayout
                    if (IsLayoutResult)
                    {
                        m_layoutLink = "http://" + Guid.NewGuid().ToString() + ".com/";
                        await webBrowser.GetMainFrame().EvaluateScriptAsync<string>("document.body.innerHTML += '<div><a style=\"color: #FFFFFF00\" href=" + m_layoutLink + ">&nbsp;dummylink</a></div>'").ConfigureAwait(false);

                        string htmlString = await webBrowser.GetMainFrame().EvaluateScriptAsync<string>("document.documentElement.outerHTML").ConfigureAwait(false);

                        htmlString = "<!DOCTYPE html>" + htmlString;

                        await webBrowser.SetMainFrameDocumentContentAsync(htmlString).ConfigureAwait(false);

                    }
                    if (m_singlePageLayout != SinglePageLayout.None)
                    {
                        webpageheight = await webBrowser.GetMainFrame().EvaluateScriptAsync<int>("document.body.scrollHeight").ConfigureAwait(false);
                    }

                    //Additional delay and windows status
                    if (!string.IsNullOrEmpty(WindowStatus))
                    {
                        while (true)
                        {
                            string windowStatus = await webBrowser.GetMainFrame().EvaluateScriptAsync<string>("window.status").ConfigureAwait(false);
                            if (windowStatus == WindowStatus)
                            {
                                break;
                            }
                            else
                            {
                                await webBrowser.WaitForRenderIdleAsync(DEF_TIMEOUT).ConfigureAwait(false);
                            }
                        }
                    }
                    if (LoadLazyImages)
                    {
                        try
                        {
                            webpageheight = await webBrowser.GetMainFrame().EvaluateScriptAsync<int>("document.body.scrollHeight").ConfigureAwait(false);
                            for (int k = 0; k < webpageheight; k = k + 500)
                            {
                                await webBrowser.GetMainFrame().EvaluateScriptAsync("window.scrollTo(" + k.ToString() + ", " + (k + 500).ToString() + ")").ConfigureAwait(false);
                                await webBrowser.WaitForRenderIdleAsync(DEF_TIMEOUT).ConfigureAwait(false);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }

                    if (AdditionalDelay > 0)
                        await Task.Delay(AdditionalDelay);
                    if (format == PDF)
                    {
                        var paperWidth = 8.5m;
                        var paperHeight = 11m;

                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.Width = PdfPageSize.Width;

                        //Set Maximum PDF page height for single page layout
                        if (m_singlePageLayout != SinglePageLayout.None && webpageheight > 0)
                        {
                            pdfOptions.fullPage = true;
                        }
                        else
                            pdfOptions.Height = PdfPageSize.Height;

                        if (webpageheight < 600)
                        {
                            pdfOptions.WebPageHeight = webpageheight;
                        }
                        PdfPrintSettings pdfPrintSettings = new PdfPrintSettings();
                        pdfPrintSettings.PrintBackground = true;


                        if (Scale == 0)
                        {
                            double scaleFactor = DEF_CEFVIEWPORT / double.Parse(ViewPortSize.Width.ToString());
                            if (scaleFactor <= 1 && scaleFactor > 0)
                                Scale = scaleFactor;
                        }
                        else
                        {
                            Scale = Scale;
                        }
                        pdfOptions.Scale = (decimal)Scale;
                        pdfPrintSettings.Scale = Scale;

                        if (Orientation == PdfPageOrientation.Landscape)
                            pdfPrintSettings.Landscape = true;

                        pdfPrintSettings.MarginType = CefPdfPrintMarginType.Custom;
                        pdfPrintSettings.MarginTop = (double)ConvertPrintParameterToInches(PdfMargins.Top.ToString());
                        pdfPrintSettings.MarginBottom = (double)ConvertPrintParameterToInches(PdfMargins.Bottom.ToString());
                        pdfPrintSettings.MarginLeft = (double)ConvertPrintParameterToInches(PdfMargins.Left.ToString());
                        pdfPrintSettings.MarginRight = (double)ConvertPrintParameterToInches(PdfMargins.Right.ToString());


                        if (pdfOptions.Width != null)
                        {
                            paperWidth = ConvertPrintParameterToInches(pdfOptions.Width.ToString());
                        }
                        if (pdfOptions.Height != null)
                        {
                            paperHeight = ConvertPrintParameterToInches(pdfOptions.Height.ToString());
                        }


                        var marginTop = ConvertPrintParameterToInches(pdfOptions.MarginOptions.Top);
                        var marginLeft = ConvertPrintParameterToInches(pdfOptions.MarginOptions.Left);
                        var marginBottom = ConvertPrintParameterToInches(pdfOptions.MarginOptions.Bottom);
                        var marginRight = ConvertPrintParameterToInches(pdfOptions.MarginOptions.Right);

                        if (pdfOptions.fullPage)
                        {
                            if (pdfOptions != null && pdfOptions.fullPage)
                            {
                                var metrics = await client.Page.GetLayoutMetricsAsync().ConfigureAwait(false);
                                var contentSize = metrics.ContentSize;
                                var width = System.Convert.ToInt32(Math.Ceiling(contentSize.Width));
                                var height = System.Convert.ToInt32(Math.Ceiling(contentSize.Height));

                                var isMobile = false;
                                var deviceScaleFactor = 1.0;

                                await client.Emulation.SetDeviceMetricsOverrideAsync(width, height, deviceScaleFactor, isMobile).ConfigureAwait(false);

                                PdfUnitConvertor converter = new PdfUnitConvertor();
                                int webPageHeight;
                                if (pdfOptions.WebPageHeight != null && (int)pdfOptions.WebPageHeight < height)
                                {
                                    webPageHeight = (int)pdfOptions.WebPageHeight;
                                }
                                else
                                {
                                    webPageHeight = (int)converter.ConvertFromPixels(height, PdfGraphicsUnit.Point);
                                }
                                webPageHeight = (int)(webPageHeight * pdfOptions.Scale);
                                paperHeight = ConvertPrintParameterToInches(webPageHeight.ToString());

                            }

                        }

                        pdfPrintSettings.PaperWidth = (double)paperWidth;
                        //pdfPrintSettings.MarginType = CefPdfPrintMarginType.Custom;
                        pdfPrintSettings.PaperHeight = (double)paperHeight;
                        string fileName = Guid.NewGuid().ToString();
                        string tempFilePath = Path.Combine(Path.GetTempPath(), fileName + ".pdf");

                        await webBrowser.PrintToPdfAsync(tempFilePath, pdfPrintSettings);

                        byte[] tempPdfData = File.ReadAllBytes(tempFilePath);
                        OutputStream = new MemoryStream(tempPdfData);

                        DeleteFile(tempFilePath);
                    }
                    else
                    {
                        Viewport viewport = new Viewport();
                        viewport.Width = viewportSize.Width;
                        if (viewportSize.Height == 0)
                        {
                            viewport.Height = webBrowser.Size.Height;
                        }
                        else
                        {
                            viewport.Height = viewportSize.Height;
                        }
                        viewport.Scale = 1.0;

                        string getBackgroundColorScript = @"
    (function() {
        return window.getComputedStyle(document.body).backgroundColor;
    })();
";
                        var result = await webBrowser.EvaluateScriptAsync(getBackgroundColorScript);
                        if (result.Result.ToString() == "rgba(0, 0, 0, 0)")
                        {
                            string setBackgroundScript = @"
                (function() {
                    document.body.style.backgroundColor = 'white';
                })();
            "; 
                            webBrowser.ExecuteScriptAsync(setBackgroundScript);
                        }
                        byte[] byteData = await webBrowser.CaptureScreenshotAsync(CaptureScreenshotFormat.Jpeg, 100, viewport);
                        OutputStream = new MemoryStream(byteData);
                    }
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

        private async Task WaitForBrowserInitialization(ChromiumWebBrowser browser)
        {
            while (!browser.IsBrowserInitialized)
            {
                await Task.Delay(10); // Wait for 100 milliseconds
            }
        }

        private PdfDocument CefResult(Stream outputStream)
        {
            PdfDocument document = new PdfDocument();
            document.PageSettings.Rotate = PageRotateAngle;

            if (OutputStream != null)
            {
                PdfLoadedDocument ldDoc = new PdfLoadedDocument(outputStream);
                RectangleF layoutRectangle = RectangleF.Empty;
                int pageCount = ldDoc.PageCount;

                for (int i = pageCount - 1; i >= 0; i--)
                {
                    //Getting dummy hyperlink bound values for layoutresult and single page PDF
                    if (IsLayoutResult)
                    {
                        PdfAnnotationCollection annotColl = ldDoc.Pages[i].Annotations as PdfLoadedAnnotationCollection;
                        if (annotColl != null)
                        {
                            for (int j = 0; j <= annotColl.Count - 1; j++)
                            {
                                PdfLoadedAnnotation layoutAnnot = annotColl[j] as PdfLoadedAnnotation;
                                if (layoutAnnot != null)
                                {
                                    PdfLoadedTextWebLinkAnnotation dummyLinkAnnot = layoutAnnot as PdfLoadedTextWebLinkAnnotation;
                                    if (dummyLinkAnnot != null && dummyLinkAnnot.Url == m_layoutLink)
                                    {
                                        layoutRectangle = dummyLinkAnnot.Bounds;
                                        //Remove the dummy link annotation in last page.
                                        annotColl.RemoveAt(j);
                                        if (i != pageCount - 1)
                                        {
                                            ldDoc.Pages.RemoveAt(pageCount - 1);
                                            pageCount = ldDoc.PageCount;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                   
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

                if (SinglePageLayout != SinglePageLayout.None)
                {
                    if (pageCount == 1)
                    {
                        document.ImportPageRange(ldDoc, 0, pageCount - 1);
                    }
                    else
                    {
                        //Skip extra pages and import
                        document.ImportPageRange(ldDoc, 0, 0);
                    }
                    layoutRectangle.Y = document.Pages[document.PageCount - 1].GetClientSize().Height;
                }
                else if (pageCount > 1 && IsLayoutResult)
                {
                    //Skip the extra Pdf Page if the dummy hyperlink added in beginning new page
                    document = SkipExtraPage(document, ldDoc, pageCount, layoutRectangle.Y);
                    if (layoutRectangle.Y == 0)
                    {
                        layoutRectangle.Y = document.Pages[document.PageCount - 1].GetClientSize().Height;
                    }
                }
                else
                    document.ImportPageRange(ldDoc, 0, pageCount - 1);
                if (IsLayoutResult)
                {
                    pageCount = document.PageCount;
                    SizeF clientSize = document.Pages[pageCount - 1].GetClientSize();
                    CefLayoutResult = new PdfLayoutResult(document.Pages[pageCount - 1], new RectangleF(0, 0, clientSize.Width, layoutRectangle.Y));
                }
               
                
                //Creating the PdfForms from cefFormCollection
                if (EnableForm && m_cefFormCollection.Count > 0)
                {
                    CreatePdfForms(document);
                }
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
            }
            else
            {
                throw new PdfException("Failed to convert webpage" + "\t" + log.ToString());
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
        private async Task GetFormFieldValue(ChromiumWebBrowser page, string fieldType, int i)
        {
            string FieldValueQuery = @"(function ()
                {
                    const values = document.getElementsByTagName('" + fieldType + "').item(" + i + @");
                    return {value : values.value,
                            outerHTML : values.outerHTML,
                            type : values.type,
                            id  : values.name,
                            title : values.title,
                            isReadOnly : values.readOnly,
                            isSelected : values.checked,
                            isDisabled : values.disabled,
                            selectedIndex : values.selectedIndex
                    };
                })();";
            m_formLink = "http://" + Guid.NewGuid().ToString() + ".com/";

            CefForm cefForm = new CefForm();
            if (fieldType == SELECTBOX)
            {
                cefForm.OptionField = new List<CefForm>();
                int optionCount = await page.EvaluateScriptAsync<int>("document.getElementsByTagName('" + fieldType + "').item(" + i + @").length").ConfigureAwait(false);
                for (int k = 0; k < optionCount; k++)
                {
                    string OptionValueQuery = @"(function ()
                    {
                        const options = document.getElementsByTagName('" + fieldType + "').item(" + i + @").item(" + k + @");
                        return {value : options.value,
                                selectedValue : options.text
                                };
                    })();";
                    CefForm selectField = new CefForm();
                    var response = await page.GetMainFrame().EvaluateScriptAsync<OptionValue>(OptionValueQuery).ConfigureAwait(false);
                    selectField.OptionValue = response;
                    cefForm.OptionField.Add(selectField);
                }
            }

            cefForm.FormLink = m_formLink;

            cefForm.FieldValue = await page.GetMainFrame().EvaluateScriptAsync<FieldValue>(FieldValueQuery).ConfigureAwait(false);

            if (cefForm.FieldValue != null && cefForm.FieldValue.type != "hidden")
            {
                m_formQuery = ReplaceInvalidQuery(cefForm.FieldValue.outerHTML);
                await page.EvaluateScriptAsync<string>("document.getElementsByTagName('" + fieldType + "').item(" + i + @").outerHTML = '<a style=" + "color:" + LINKCOLOR + @" href=" + m_formLink + @">" + m_formQuery + "</a>'").ConfigureAwait(false);
                m_cefFormCollection.Add(cefForm);
            }
        }
        //Replacing the invalid terms of Form fields Query selector
        private string ReplaceInvalidQuery(string formValue)
        {
            string[] list = { "'", "\n\t", "\n", "\t" };
            for (int i = 0; i < list.Length; i++)
            {
                if (formValue.Contains(list[i]))
                {
                    formValue = formValue.Replace(list[i], "");
                }
            }
            return formValue;
        }
        #endregion
        #region Form Fields conversion
      
        private void CreatePdfForms(PdfDocument doc)
        {
            for (int i = 0; i < m_cefFormCollection.Count; i++)
            {
                CefForm formField = m_cefFormCollection[i];
                string formType = formField.FieldValue.type;
                bool formReadOnly = formField.FieldValue.isDisabled || formField.FieldValue.isReadOnly;
                if (formType == TEXTBOX || formType == TEXTAREA || formType == PASSWORD || formType == NUMBER || formType == EMAIL)
                {
                    if (string.IsNullOrEmpty(formField.FieldValue.id))
                        formField.FieldValue.id = formType + "_" + Guid.NewGuid();
                    PdfTextBoxField pdfTextBoxField = new PdfTextBoxField(doc.Pages[formField.PageNumber], formField.FieldValue.id);
                    pdfTextBoxField.Bounds = formField.Bounds;
                    pdfTextBoxField.ReadOnly = formReadOnly;
                    pdfTextBoxField.Text = formField.FieldValue.value;
                    if (!string.IsNullOrEmpty(formField.FieldValue.title))
                        pdfTextBoxField.ToolTip = formField.FieldValue.title;
                    if (formType == TEXTAREA)
                    {
                        pdfTextBoxField.Multiline = true;
                        pdfTextBoxField.Scrollable = true;
                    }
                    else if (formType == PASSWORD)
                    {
                        pdfTextBoxField.Password = true;
                    }
                    doc.Form.Fields.Add(pdfTextBoxField);
                }
                else if (formType == CHECKBOX)
                {
                    if (string.IsNullOrEmpty(formField.FieldValue.id))
                        formField.FieldValue.id = CHECKBOX + "_" + Guid.NewGuid();
                    PdfCheckBoxField checkBoxField = new PdfCheckBoxField(doc.Pages[formField.PageNumber], formField.FieldValue.id);
                    checkBoxField.Bounds = formField.Bounds;
                    checkBoxField.Checked = formField.FieldValue.isSelected;
                    checkBoxField.ReadOnly = formReadOnly;
                    if (!string.IsNullOrEmpty(formField.FieldValue.title))
                        checkBoxField.ToolTip = formField.FieldValue.title;
                    doc.Form.Fields.Add(checkBoxField);
                }
                else if (formType == SUBMIT || formType == BUTTON)
                {
                    if (string.IsNullOrEmpty(formField.FieldValue.id))
                        formField.FieldValue.id = formType + "_" + Guid.NewGuid();
                    PdfButtonField buttonField = new PdfButtonField(doc.Pages[formField.PageNumber], formField.FieldValue.id);
                    buttonField.Bounds = formField.Bounds;
                    buttonField.ReadOnly = formReadOnly;
                    if (!string.IsNullOrEmpty(formField.FieldValue.title))
                        buttonField.ToolTip = formField.FieldValue.title;
                    if (string.IsNullOrEmpty(formField.FieldValue.value))
                        buttonField.Text = " ";
                    else
                        buttonField.Text = formField.FieldValue.value;
                    doc.Form.Fields.Add(buttonField);
                }
                else if (formType == RADIOBUTTON && !formField.IsAdded)
                {
                    int index = 0;
                    bool flag = true;
                    PdfRadioButtonListField RadioList = new PdfRadioButtonListField(doc.Pages[formField.PageNumber], formField.FieldValue.id);
                    foreach (CefForm radioElement in m_cefFormCollection)
                    {
                        if (radioElement.PageNumber >= 0)
                        {
                            if (formField.FieldValue.id == radioElement.FieldValue.id && formField.PageNumber == radioElement.PageNumber)
                            {
                                PdfRadioButtonListItem radioButton = new PdfRadioButtonListItem(radioElement.FieldValue.value);
                                radioButton.Bounds = radioElement.Bounds;
                                radioButton.ReadOnly = radioElement.FieldValue.isDisabled || radioElement.FieldValue.isReadOnly;
                                RadioList.ReadOnly = radioButton.ReadOnly;
                                if (!string.IsNullOrEmpty(radioElement.FieldValue.title))
                                    radioButton.ToolTip = radioElement.FieldValue.title;

                                if (!radioElement.FieldValue.isSelected && flag)
                                {
                                    index++;
                                }
                                else
                                {
                                    flag = false;
                                }
                                radioElement.IsAdded = true;
                                RadioList.Items.Add(radioButton);
                            }
                        }
                    }
                    if (!flag)
                    {
                        if (RadioList.Items.Count != 0)
                            RadioList.SelectedIndex = index;
                    }
                    doc.Form.Fields.Add(RadioList);
                }
                else if (formType.Contains(SELECTBOX))
                {
                    if (string.IsNullOrEmpty(formField.FieldValue.id))
                        formField.FieldValue.id = SELECTBOX + "_" + Guid.NewGuid();

                    if (formType == LISTBOX)
                    {
                        PdfListBoxField listBoxField = new PdfListBoxField(doc.Pages[formField.PageNumber], formField.FieldValue.id);
                        listBoxField.Bounds = formField.Bounds;
                        listBoxField.ReadOnly = formField.FieldValue.isDisabled;
                        foreach (CefForm selectBox in formField.OptionField)
                        {
                            PdfListFieldItem listItem = new PdfListFieldItem(selectBox.OptionValue.selectedValue, selectBox.OptionValue.value);
                            listBoxField.Items.Add(listItem);
                        }
                        if (listBoxField.Items.Count != 0 && formField.FieldValue.selectedIndex >= 0)
                            listBoxField.SelectedIndex = formField.FieldValue.selectedIndex;
                        if (!string.IsNullOrEmpty(formField.FieldValue.title))
                            listBoxField.ToolTip = formField.FieldValue.title;
                        listBoxField.MultiSelect = true;
                        doc.Form.Fields.Add(listBoxField);
                    }
                    else
                    {
                        PdfComboBoxField comboBoxField = new PdfComboBoxField(doc.Pages[formField.PageNumber], formField.FieldValue.id);
                        comboBoxField.Bounds = formField.Bounds;
                        comboBoxField.ReadOnly = formField.FieldValue.isDisabled;
                        foreach (CefForm selectBox in formField.OptionField)
                        {
                            PdfListFieldItem listItem = new PdfListFieldItem(selectBox.OptionValue.selectedValue, selectBox.OptionValue.value);
                            comboBoxField.Items.Add(listItem);
                        }
                        if (!string.IsNullOrEmpty(formField.FieldValue.title))
                            comboBoxField.ToolTip = formField.FieldValue.title;
                        if (comboBoxField.Items.Count != 0)
                            comboBoxField.SelectedIndex = formField.FieldValue.selectedIndex;
                        doc.Form.Fields.Add(comboBoxField);
                    }
                }
            }
        }
        #endregion
        #region Bookmark conversion
        //Sort bookmarks collection with destination y position and destination page number.
      
        //Skipped the extra page added due to dummy link annotation created at new page.
        private PdfDocument SkipExtraPage(PdfDocument document, PdfLoadedDocument ldDoc, int pageCount, float height)
        {
            float pageHeight = 0.0f;
            for (int i = 0; i < pageCount - 1; i++)
            {
                pageHeight += ldDoc.Pages[i].Size.Height;
            }
            float totalContentHeight = pageHeight + height;
            if (totalContentHeight == pageHeight)
                //Dummy link added at new page, so we skipped that. 
                document.ImportPageRange(ldDoc, 0, pageCount - 2);
            else
                document.ImportPageRange(ldDoc, 0, pageCount - 1);

            return document;
        }
        private decimal ConvertPrintParameterToInches(string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                return 0;
            }
            else
            {
                decimal val = decimal.Parse(parameter);
                val = val / (decimal)0.75;
                return val / 96;
            }
        }
        #endregion

        #region Invoke
        List<Task> m_deletionTasks = new List<Task>();

        private void CreateTempUserDir()
        {
            if (!string.IsNullOrEmpty(TempPath))
            {
                if (Directory.Exists(TempPath))
                {
                    DirectoryInfo info = Directory.CreateDirectory(Path.Combine(TempPath, Guid.NewGuid().ToString()));
                    TempUserDir = info.FullName;
                }
                else
                {
                    throw new DirectoryNotFoundException("Path does not exists");
                }
            }
            else
            {
                TempPath = Path.GetTempPath();
                if (Directory.Exists(TempPath))
                {
                    DirectoryInfo info = Directory.CreateDirectory(Path.Combine(TempPath, Guid.NewGuid().ToString()));
                    TempUserDir = info.FullName;
                }
                else
                {
                    throw new DirectoryNotFoundException("Path does not exists");
                }
            }
        }

        private void DeleteTempUserDir()
        {
            try
            {
                if (Directory.Exists(TempUserDir))
                {
                    DirectoryInfo info = new DirectoryInfo(TempUserDir);
                    Directory.Delete(info.FullName, true);
                }
            }
            catch (Exception)
            {
                TaskCreationOptions options = new TaskCreationOptions();

                Task task = Task.Factory.StartNew(() => RunUntillDelete(TempUserDir), CancellationToken.None, options, TaskScheduler.Default);
                m_deletionTasks.Add(task);
            }
        }

        private void RunUntillDelete(string tempUserDir)
        {
            bool continueDeletion = true;
            while (continueDeletion)
            {
                try
                {
                    if (Directory.Exists(TempUserDir))
                    {
                        DirectoryInfo info = new DirectoryInfo(tempUserDir);
                        Directory.Delete(info.FullName, true);
                    }
                    continueDeletion = false;
                }
                catch
                {
                    continueDeletion = true;
                }
            }
        }
        #endregion
    }
    internal class CefForm
    {
        internal int PageNumber { get; set; }
        internal string FormLink { get; set; }
        internal bool IsAdded { get; set; }
        internal List<CefForm> OptionField { get; set; }
        internal RectangleF Bounds { get; set; }
        internal FieldValue FieldValue { get; set; }
        internal OptionValue OptionValue { get; set; }
    }

    internal class FieldValue
    {
        [JsonProperty("value")]
        public string value { get; set; }

        [JsonProperty("outerHTML")]
        public string outerHTML { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("isReadOnly")]
        public bool isReadOnly { get; set; }

        [JsonProperty("isDisabled")]
        public bool isDisabled { get; set; }

        [JsonProperty("isSelected")]
        public bool isSelected { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("selectedIndex")]
        public int selectedIndex { get; set; }

    }

    internal class OptionValue
    {
        [JsonProperty("value")]
        public string value { get; set; }

        [JsonProperty("selectedValue")]
        public string selectedValue { get; set; }
    }
    internal class PdfOptions
    {
        public PdfOptions()
        {
        }

        public decimal Scale { get; set; } = 1;

        public bool DisplayHeaderFooter { get; set; }

        public string HeaderTemplate { get; set; } = string.Empty;

        public string FooterTemplate { get; set; } = string.Empty;

        public bool PrintBackground { get; set; }

        public bool Landscape { get; set; }
        public string PageRanges { get; set; } = string.Empty;
        public object Width { get; set; }

        public object Height { get; set; }

        public object WebPageHeight { get; set; }

        public MarginOptions MarginOptions { get; set; } = new MarginOptions();

        public bool PreferCSSPageSize { get; set; }

        public bool fullPage { get; set; }

    }
    internal class MarginOptions
    {
        public string Top { get; set; }
        public string Left { get; set; }
        public string Bottom { get; set; }
        public string Right { get; set; }

    }
    internal class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class CefProxySettings
    {
        #region Fields

        private int m_portNumber;
        private string m_hostname;
        private CefProxyType m_type;
        private string m_username;
        private string m_password;

        #endregion

        #region Properties
        public string HostName
        {
            get
            {
                return m_hostname;
            }
            set
            {
                m_hostname = value;
            }
        }
        public int PortNumber
        {
            get
            {
                return m_portNumber;
            }
            set
            {
                if (value > -1 && value < 65535)
                    m_portNumber = value;
                else
                    throw new Exception("Invalid port number. Port number must be range from 0 to 65535.");
            }
        }
        public CefProxyType Type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
            }
        }

        public string Username
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
        public string Password
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
        #endregion

        #region Constructor
        public CefProxySettings()
        {
            HostName = string.Empty;
            PortNumber = 0;
            Type = CefProxyType.None;
            Username = string.Empty;
            Password = string.Empty;
        }
        #endregion
        public enum CefProxyType
        {
            /// <summary>
            /// No proxy
            /// </summary>
            None,
            /// <summary>
            /// Http proxy type
            /// </summary>
            Http,
            /// <summary>
            /// Socks5 proxy type
            /// </summary>
            Socks5
        }
    }
}
