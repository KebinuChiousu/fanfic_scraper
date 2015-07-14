Imports System
Imports System.IO
Imports System.Net
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports OpenQA.Selenium.PhantomJS
Imports HtmlAgilityPack
Imports System.Runtime.Serialization.Formatters.Binary

Public Class clsWeb
    Implements IDisposable

    Private _driver As IWebDriver
    Private _options As PhantomJSOptions
    Private _service As PhantomJSDriverService
    Private _cookie As Boolean = False
    Private _submit As IWebElement

    'Private UserAgent As String = "Chrome/34.0.1847.131"

    Sub New()

        _service = PhantomJSDriverService. _
                   CreateDefaultService(Environment.CurrentDirectory)
        _service.HideCommandPromptWindow = True

        _options = New PhantomJSOptions
        '_options.AddAdditionalCapability("phantomjs.page.settings.userAgent", UserAgent)

        _driver = New PhantomJSDriver(_service, _options)
        _driver.Manage().Timeouts().ImplicitlyWait(New TimeSpan(0, 0, 30))

    End Sub

    Sub Dispose() Implements IDisposable.Dispose

        _driver.Quit()
        _driver = Nothing
        _options = Nothing
        _service = Nothing

    End Sub

    Private Function InnerElements(ctrl As IWebElement) As Boolean

        Dim ret As Boolean = False
        Dim html As String
        Dim doc As HtmlDocument = Nothing

        html = ctrl.GetAttribute("outerHTML")

        doc = CleanHTML(html)

        If doc.DocumentNode.FirstChild.HasChildNodes Then

            If doc.DocumentNode.FirstChild.ChildNodes.Count > 1 Then
                ret = True
            Else
                If doc.DocumentNode.FirstChild.FirstChild.Name = "#text" Then
                    ret = False
                Else
                    ret = True
                End If
            End If
        End If

        doc = Nothing

        Return ret

    End Function

    Private Function CheckTag(name As String) As Boolean

        Select Case name
            Case "input", "select"
                Return True
            Case Else
                Return False
        End Select


    End Function

    Private Sub ProcessFields( _
                               ByRef item As IWebElement, _
                               ByRef Fields As List(Of KeyValuePair(Of String, String)) _
                             )

        Dim idx As Integer
        Dim kvp As KeyValuePair(Of String, String)
        Dim ret As Boolean

        If CheckTag(item.TagName) Then

            For idx = 0 To (Fields.Count - 1)

                kvp = Fields(idx)
                ret = ProcessElement(item, kvp)

                If ret = True Then
                    Exit For
                End If

            Next

        End If

    End Sub

    Private Function ProcessElement( _
                                     ByRef item As IWebElement, _
                                     ByRef kvp As KeyValuePair(Of String, String) _
                                   ) As Boolean

        Dim sel As SelectElement
        Dim ret As Boolean = False
        Dim name As String
        Dim type As String

        name = item.GetAttribute("name")
        type = item.GetAttribute("type")

        If name = kvp.Key Then
            Select Case type
                Case "text", "password"
                    item.SendKeys(kvp.Value)
                Case "checkbox"
                    item.Click()
                Case "select-one"
                    sel = New SelectElement(item)
                    sel.SelectByValue(kvp.Value)
            End Select
            ret = True
        End If

        If Not ret Then
            If type = kvp.Key Then
                If item.Text = kvp.Value Or item.GetAttribute("value") = kvp.Value Then
                    _submit = item
                End If
            End If
        End If


        Return ret

    End Function

    Private Sub GetInnerElements( _
                                  ByRef ctrl As IWebElement, _
                                  ByRef Fields As List(Of KeyValuePair(Of String, String)) _
                                )

        Dim ChildElements As ReadOnlyCollection(Of IWebElement)

        ChildElements = ctrl.FindElements(By.XPath("*"))

        For Each item As IWebElement In ChildElements

            If InnerElements(item) Then
                GetInnerElements(item, Fields)
            End If

            ProcessFields(item, Fields)

        Next

    End Sub

    Public Sub FollowLink(ByVal URL As String, ByVal LinkText As String, cookieName As String)

        Dim cookies As ReadOnlyCollection(Of OpenQA.Selenium.Cookie)
        Dim cookies2 As New List(Of OpenQA.Selenium.Cookie)
        Dim link As IWebElement
        Dim ret As String

        _driver.Navigate.GoToUrl(URL)

        link = _driver.FindElement(By.LinkText(LinkText))

        link.Click()

        ret = _driver.PageSource

        cookies = _driver.Manage.Cookies.AllCookies

        For idx = 0 To cookies.Count - 1
            cookies2.Add(cookies(idx))
        Next

        WriteCookiesToDisk(cookieName, cookies2)

    End Sub

    Public Sub LogIn(ByVal URL As String, formName As String, Fields As List(Of KeyValuePair(Of String, String)), cookieName As String)

        Dim cookies As ReadOnlyCollection(Of OpenQA.Selenium.Cookie)
        Dim cookies2 As New List(Of OpenQA.Selenium.Cookie)

        Dim idx As Integer
        Dim formElement As IWebElement
        Dim forms As ReadOnlyCollection(Of IWebElement)
        Dim allFormChildElements As ReadOnlyCollection(Of IWebElement)

        Dim btn As IWebElement = Nothing

        _driver.Navigate.GoToUrl(URL)

        forms = _driver.FindElements(By.TagName("form"))

        formElement = forms(0)

        allFormChildElements = formElement.FindElements(By.XPath("*"))

        For Each item As IWebElement In allFormChildElements

            If InnerElements(item) Then
                GetInnerElements(item, Fields)
            End If

            ProcessFields(item, Fields)

        Next

        _submit.Submit()

        cookies = _driver.Manage.Cookies.AllCookies

        For idx = 0 To cookies.Count - 1
            cookies2.Add(cookies(idx))
        Next

        WriteCookiesToDisk(cookieName, cookies2)

    End Sub

    Public Function DownloadPage(ByVal URL As String, Optional ByVal Cookie As String = "") As String

        Dim cookies As List(Of OpenQA.Selenium.Cookie)
        Dim idx As Integer

        Dim ret As String = ""

        _driver.Navigate.GoToUrl(URL)

        If System.IO.File.Exists(Environment.CurrentDirectory & "\" & Cookie) Then

            cookies = ReadCookiesFromDisk(Cookie, URL)
            _driver.Manage.Cookies.DeleteAllCookies()

            For idx = 0 To cookies.Count - 1
                _driver.Manage.Cookies.AddCookie(cookies(idx))
            Next

            _driver.Navigate.GoToUrl(URL)

            _cookie = True

        End If

        ret = _driver.PageSource

        Return ret

    End Function

    Private Sub WriteCookiesToDisk(ByVal fileName As String, ByVal cookieJar As List(Of OpenQA.Selenium.Cookie))

        Dim cookies As New CookieContainer
        Dim cookie As System.Net.Cookie

        Dim idx As Integer

        For idx = 0 To cookieJar.Count - 1
            cookie = New System.Net.Cookie( _
                                            cookieJar(idx).Name, _
                                            cookieJar(idx).Value, _
                                            cookieJar(idx).Path, _
                                            cookieJar(idx).Domain _
                                          )

            cookie.Expires = cookieJar(idx).Expiry

            cookies.Add(cookie)

        Next

        Using stream As Stream = File.Create(fileName)
            Try
                'MsgBox("Writing cookies to disk... ")
                Dim formatter As New BinaryFormatter()
                formatter.Serialize(stream, cookies)
                'MsgBox("Done.")
            Catch e As Exception
                MsgBox("Problem writing cookies to disk: " & e.Message)
            End Try
        End Using
    End Sub

    Private Function ReadCookiesFromDisk(ByVal fileName As String, URL As String) As List(Of OpenQA.Selenium.Cookie)

        Dim cookies As CookieContainer
        Dim cookiec As CookieCollection

        Dim ts As New TimeSpan

        Dim dte As DateTime = DateTime.Now

        dte = dte.AddDays(365)

        Dim u As New System.Uri(URL)

        'URL = u.Scheme & Uri.SchemeDelimiter & GetDomain(u)

        'u = Nothing
        'u = New System.Uri(URL)

        Dim idx As Integer

        Dim cookiejar As New List(Of OpenQA.Selenium.Cookie)
        Dim cookie As OpenQA.Selenium.Cookie

        Try
            Using stream As Stream = File.Open(fileName, FileMode.Open)
                'MsgBox("Reading cookies from disk... ")
                Dim formatter As New BinaryFormatter()
                'MsgBox("Done.")
                cookies = CType(formatter.Deserialize(stream), CookieContainer)
                cookiec = cookies.GetCookies(u)

                For idx = 0 To cookiec.Count - 1
                    cookie = New OpenQA.Selenium.Cookie( _
                                                         cookiec(idx).Name, _
                                                         cookiec(idx).Value, _
                                                         cookiec(idx).Domain, _
                                                         cookiec(idx).Path, _
                                                         dte _
                                                       )

                    cookiejar.Add(cookie)

                Next


            End Using
        Catch e As Exception
            MsgBox("Problem reading cookies from disk: " & e.Message())
            cookiejar = Nothing
        End Try

        Return cookiejar

    End Function

    Private Function GetDomain(u As Uri) As String
        If u.HostNameType <> UriHostNameType.Dns Then Return String.Empty
        Dim parts = u.Host.Split("."c)
        If parts.Length > 1 Then
            Return String.Join(".", parts, parts.Length - 2, 2)
        Else
            Return parts(0)
        End If
    End Function

End Class
