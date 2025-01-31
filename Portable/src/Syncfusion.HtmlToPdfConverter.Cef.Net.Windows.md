### Syncfusion&reg; .NET HTML to PDF converter

This NuGet package supports to convert HTML to PDF in Azure App Service for Windows. It internally uses the [CEFSharp](https://www.nuget.org/packages/CefSharp.OffScreen.NETCore/119.4.30) open source library that comes under a BSD license.

> #### Starting with v20.1.0.x, if you reference Syncfusion&reg; HTML converter assemblies from trial setup or from the NuGet feed, include a license key in your projects. Refer to link to learn about generating and registering Syncfusion&reg; license key in your application to use the components without trail message.

![NET HTML to PDF converter](https://cdn.syncfusion.com/nuget-readme/fileformats/net-html-to-pdf.png)
[Features overview](https://www.syncfusion.com/pdf-framework/net/html-to-pdf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget) | [Documentation](https://help.syncfusion.com/file-formats/pdf/converting-html-to-pdf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget) | [API Reference](https://help.syncfusion.com/cr/file-formats/Syncfusion.Pdf.HtmlToPdf.html?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget) | [Online Demo](https://ej2.syncfusion.com/aspnetcore/PDF/HtmltoPDF#/bootstrap5) | [Blogs](https://www.syncfusion.com/blogs/?s=html+to+pdf) | [Support](https://support.syncfusion.com/support/tickets/create?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget) | [Forums](https://www.syncfusion.com/forums?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget) | [Feedback](https://www.syncfusion.com/feedback/wpf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget)

### Key Features

* Converts any [webpage to PDF.](https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/features#url-to-pdf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget)
* Converts any raw [HTML string to PDF.](https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/features#html-string-to-pdf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget)
* Converts [HTML form to fillable PDF form](https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/features#html-form-to-pdf-form?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget).
* Automatically [creates Table of Contents](https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/features#table-of-contents?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget).
* Automatically creates [bookmark hierarchy](https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/features#bookmarks?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget).
* Supports header and footer.
* Converts any [HTML to image](https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/features#url-to-image?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget).
* Supports HTTP cookies.
* Supports cookies-based [form authentication](https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/features#form-authentication?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget).
* Supports internal and external [hyperlinks](https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/features#hyperlinks?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget).

> #### Note: We ensured the working of HTML to PDF converter by using the CefSharp.OffScreen.NetCore package version 119.4.30 as dependent package. So kindly use the same version and using other version is not recommended.

### System Requirements

* [System Requirements](https://help.syncfusion.com/file-formats/installation-and-upgrade/system-requirements?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget).

### Getting Started

Install the [Syncfusion.HtmlToPdfConverter.Cef.Net.Windows](https://www.nuget.org/packages/Syncfusion.HtmlToPdfConverter.Cef.Net.Windows?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget) NuGet package as reference to your .NET Core application from [NuGet.org](https://www.nuget.org/).

### Convert HTML to PDF document programmatically using C#

```csharp
//Initialize HTML to PDF converter.
HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Cef);
//Convert URL to PDF document.
PdfDocument document = htmlConverter.Convert("https://www.google.com");
FileStream fileStream = new FileStream("Sample.pdf", FileMode.CreateNew, FileAccess.ReadWrite);
//Save and close the PDF document.
document.Save(fileStream);
document.Close(true);
```

### License

This NuGet package includes code from the CefSharp project. This code is subject to the terms of the CefSharp Project license available [here](https://github.com/cefsharp/CefSharp/blob/master/README.md). Syncfusion&reg; does not provide any warranty or any indemnity with regard to the use of code from the CefSharp Project. If you do not agree to these terms, please do not install, or use this NuGet package.
 
### About Syncfusion&reg;

Founded in 2001 and headquartered in Research Triangle Park, N.C., Syncfusion&reg; has more than 27,000+ customers and more than 1 million users, including large financial institutions, Fortune 500 companies, and global IT consultancies.
 
Today, we provide 1700+ components and frameworks for web ([Blazor](https://www.syncfusion.com/blazor-components?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [ASP.NET Core](https://www.syncfusion.com/aspnet-core-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [ASP.NET MVC](https://www.syncfusion.com/aspnet-mvc-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [ASP.NET Web Forms](https://www.syncfusion.com/jquery/aspnet-webforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [Angular](https://www.syncfusion.com/angular-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [React](https://www.syncfusion.com/react-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [Vue](https://www.syncfusion.com/vue-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), and [jQuery](https://www.syncfusion.com/jquery-ui-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget)), mobile ([.NET MAUI (Preview)](https://www.syncfusion.com/maui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), and [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget)), and desktop development ([WinForms](https://www.syncfusion.com/winforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [WPF](https://www.syncfusion.com/wpf-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [WinUI](https://www.syncfusion.com/winui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [.NET MAUI (Preview)](https://www.syncfusion.com/maui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), [Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget), and [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget)). We provide ready-to-deploy enterprise software for dashboards, reports, data integration, and big data processing. Many customers have saved millions in licensing fees by deploying our software.

[sales@syncfusion.com](mailto:sales@syncfusion.com?Subject=Syncfusion%20HTMLConverter%20-%20NuGet) | [www.syncfusion.com](https://www.syncfusion.com?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-htmltopdfconverter-nuget) | Toll Free: 1-888-9 DOTNET
