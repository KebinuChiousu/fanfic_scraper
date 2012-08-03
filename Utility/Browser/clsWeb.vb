Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Net

Class clsWeb

    Private wsk As New Collection
    Private txtResponse As String
    Public blnConnected As Boolean

    'Public UserAgent As String = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)"
    Private UserAgent As String = "Chrome/21.0.1180.60"
    Private Accept As String = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"

    Private Sub WriteFile(ByVal file As String, ByVal data As String)
        Dim path As String = file
        Dim fs As FileStream

        ' Delete the file if it exists.
        If IO.File.Exists(path) = False Then
            ' Create the file.
            fs = IO.File.Create(path)
            Dim info As Byte() = New UTF8Encoding(True).GetBytes(data)

            ' Add some information to the file.
            fs.Write(info, 0, info.Length)
            fs.Close()
        End If
    End Sub

    Public Function DownloadCookies(ByVal URL As String, ByVal postData As String, ByVal cookie As String) As Boolean

        Dim ret As Boolean = True

Retry:
        Try
            Dim oHttp As HttpWebRequest = System.Net.HttpWebRequest.Create(URL)
            Dim objResponse As HttpWebResponse

            Dim cc As CookieContainer
            Dim c As Cookie
            Dim fi As FileInfo

            Dim encoding As New System.Text.UTF8Encoding
            Dim data() As Byte
            Dim postStream As System.IO.Stream = Nothing

            Dim hl As URL


            'oHttp.UserAgent = "Mozilla/5.0 " & _
            '                  "(Windows NT 6.1; WOW64) " & _
            '                  "AppleWebKit/536.11 (KHTML, like Gecko) " & _
            '                  "Chrome/20.0.1132.47 " & _
            '                  "Safari/536.11"

            oHttp.UserAgent = UserAgent

            If postData = "" Then
                oHttp.Method = "GET"
            Else
                oHttp.Method = "POST"
            End If

            oHttp.Timeout = (180 * 1000) '3 Minutes
            oHttp.AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate
            oHttp.Accept = Accept

            data = encoding.GetBytes(postData)

            cc = New CookieContainer

            oHttp.CookieContainer = cc

            If postData <> "" Then
                oHttp.ContentType = "application/x-www-form-urlencoded"
                oHttp.ContentLength = data.Length
                postStream = oHttp.GetRequestStream
                postStream.Write(data, 0, data.Length)
            End If


            objResponse = oHttp.GetResponse

            hl = ExtractUrl(URL)



            c = cc.GetCookies(New Uri("http://" & hl.Host))(0)
            c.Expires = Date.Now.AddYears(1)

            fi = New FileInfo(Application.StartupPath & "\\" & cookie)

            clsCookie.WriteCookiesToDisk(fi.FullName, cc)

            fi = Nothing

            If postData <> "" Then
                postStream.Close()
            End If

        Catch ex As System.Exception
            Select Case ex.Message
                Case "The remote server returned an error: (404) Not Found."
                    Return ""
                Case "Invalid URI: The format of the URI could not be determined."
                    MsgBox("Please enter a valid URL")
                    Return Nothing
                Case "The underlying connection was closed: " & _
                     "The server committed an HTTP protocol violation."
                    GoTo retry
                Case "Unable to read data from the transport connection: " & _
                     "The connection was closed."
                    GoTo retry

                Case Else
                    If InStr(ex.Message, "500") Then GoTo retry
                    If InStr(ex.Message, "503") Then GoTo retry
                    If InStr(ex.Message, "502") Then GoTo retry
                    Throw New System.Exception(ex.Message, ex)
            End Select

            ret = False


        End Try

        Return ret


    End Function

    Public Function DownloadPage(ByVal URL As String, Optional ByVal Cookie As String = "") As String

        Dim objResponse As HttpWebResponse

Retry:
        Try
            Dim sResult As String
            Dim oHttp As HttpWebRequest = System.Net.HttpWebRequest.Create(URL)


            Dim cc As CookieContainer

            'oHttp.UserAgent = "Mozilla/5.0 " & _
            '                  "(Windows NT 6.1; WOW64) " & _
            '                  "AppleWebKit/536.11 (KHTML, like Gecko) " & _
            '                  "Chrome/20.0.1132.47 " & _
            '                  "Safari/536.11"

            oHttp.UserAgent = UserAgent

            oHttp.Method = "GET"
            oHttp.Timeout = (180 * 1000) '3 Minutes
            oHttp.AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate
            oHttp.Accept = Accept

            If Cookie <> "" Then
                If File.Exists(Cookie) Then

                    cc = clsCookie.ReadCookiesFromDisk(Cookie)
                    oHttp.CookieContainer = cc

                Else

                    DownloadCookies(URL, "", Cookie)
                    cc = clsCookie.ReadCookiesFromDisk(Cookie)
                    oHttp.CookieContainer = cc

                End If
            End If

            objResponse = oHttp.GetResponse

ErrorDetail:

            Dim sr As StreamReader
            sr = New StreamReader( _
                                   objResponse.GetResponseStream(), _
                                   System.Text.Encoding.UTF8 _
                                 )


            sResult = sr.ReadToEnd()
            ' Close and clean up the StreamReader
            sr.Close()

            Return sResult

        Catch ex As System.Net.WebException
            Select Case ex.Message
                Case "The remote server returned an error: (404) Not Found."
                    Return ""
                Case "Invalid URI: The format of the URI could not be determined."
                    MsgBox("Please enter a valid URL")
                    Return Nothing
                Case "The underlying connection was closed: " & _
                     "The server committed an HTTP protocol violation."
                    GoTo retry
                Case "Unable to read data from the transport connection: " & _
                     "The connection was closed."
                    GoTo retry

                Case Else
                    If InStr(ex.Message, "500") Then GoTo retry
                    If InStr(ex.Message, "503") Then GoTo retry
                    If InStr(ex.Message, "502") Then GoTo retry
                    If InStr(ex.Message, "403") Then
                        objResponse = ex.Response
                        GoTo errordetail
                    End If
                    Throw New System.Exception(ex.Message, ex)
            End Select
        End Try

    End Function

End Class
