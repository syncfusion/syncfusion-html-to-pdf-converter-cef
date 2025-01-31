
xcopy /q /y "nuget.exe"

xcopy /q /y "Src\Syncfusion.HtmlToPdfConverter.Cef.Net.Windows.nuspec" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\"
copy "Src\LICENSE.txt" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\LICENSE.txt"
copy "Src\syncfusion_logo.png" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\syncfusion_logo.png"
copy "Src\Syncfusion.HtmlToPdfConverter.Cef.Net.Windows.md" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\README.md"

IF EXIST "Src\bin\Release-Xml\net6.0" (

md "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net6.0"

IF EXIST "Src\bin\Release-Xml\net6.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.dll" (
xcopy /q /y "Src\bin\Release-Xml\net6.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.dll" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net6.0"
)

IF EXIST "Src\bin\Release-Xml\net6.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.xml" (
xcopy /q /y "Src\bin\Release-Xml\net6.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.xml" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net6.0"
)

IF EXIST "Src\bin\Release-Xml\net6.0\Syncfusion.HtmlConverter.Portable.dll" (
xcopy /q /y "Src\bin\Release-Xml\net6.0\Syncfusion.HtmlConverter.Portable.dll" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net6.0"
)

IF EXIST "Src\bin\Release-Xml\net6.0\Syncfusion.HtmlConverter.Portable.xml" (
xcopy /q /y "Src\bin\Release-Xml\net6.0\Syncfusion.HtmlConverter.Portable.xml" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net6.0"
)

IF EXIST "Src\bin\Release-Xml\net6.0\Syncfusion.HtmlConverter.Portable.xml" (
xcopy /q /y "Src\bin\Release-Xml\net6.0\Syncfusion.HtmlConverter.Portable.xml" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net6.0"
)

)
IF EXIST "Src\bin\Release-Xml\net8.0" (
md "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net8.0"

IF EXIST "Src\bin\Release-Xml\net8.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.dll" (
xcopy /q /y "Src\bin\Release-Xml\net8.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.dll" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net8.0"
)

IF EXIST "Src\bin\Release-Xml\net8.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.xml" (
xcopy /q /y "Src\bin\Release-Xml\net8.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.xml" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net8.0"
)

IF EXIST "Src\bin\Release-Xml\net8.0\Syncfusion.HtmlConverter.Portable.dll" (
xcopy /q /y "Src\bin\Release-Xml\net8.0\Syncfusion.HtmlConverter.Portable.dll" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net8.0"
)

IF EXIST "Src\bin\Release-Xml\net8.0\Syncfusion.HtmlConverter.Portable.xml" (
xcopy /q /y "Src\bin\Release-Xml\net8.0\Syncfusion.HtmlConverter.Portable.xml" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net8.0"
)

)

IF EXIST "Src\bin\Release-Xml\net9.0" (
md "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net9.0"

IF EXIST "Src\bin\Release-Xml\net9.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.dll" (
xcopy /q /y "Src\bin\Release-Xml\net9.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.dll" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net9.0"
)

IF EXIST "Src\bin\Release-Xml\net9.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.xml" (
xcopy /q /y "Src\bin\Release-Xml\net9.0\Syncfusion.HtmlConverter.Portable\Syncfusion.HtmlConverter.Portable.xml" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net9.0"
)

IF EXIST "Src\bin\Release-Xml\net9.0\Syncfusion.HtmlConverter.Portable.dll" (
xcopy /q /y "Src\bin\Release-Xml\net9.0\Syncfusion.HtmlConverter.Portable.dll" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net9.0"
)

IF EXIST "Src\bin\Release-Xml\net9.0\Syncfusion.HtmlConverter.Portable.xml" (
xcopy /q /y "Src\bin\Release-Xml\net9.0\Syncfusion.HtmlConverter.Portable.xml" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net9.0"
)

IF EXIST "Src\bin\Release-Xml\net9.0\Syncfusion.HtmlConverter.Portable.xml" (
xcopy /q /y "Src\bin\Release-Xml\net9.0\Syncfusion.HtmlConverter.Portable.xml" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\lib\net9.0"
)

)

md "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\BrowserSubProcess"

IF EXIST "Extensions\BrowserSubProcess" (
xcopy /s "Extensions\BrowserSubProcess" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\BrowserSubProcess"
)

md "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\build"

IF EXIST "Extensions\build" (
xcopy /s "Extensions\build" "Syncfusion.HtmlToPdfConverter.Cef.Net.Windows\build"
)




rmdir /S /Q "Src\bin\"

rmdir /S /Q "Src\obj\"

