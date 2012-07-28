Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Net

Module FileManipulation

    Public wsk As New Collection
    Public txtResponse As String
    Public blnConnected As Boolean

    Public Sub WriteFile(ByVal file As String, ByVal data As String)
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

    Function DownloadCookies(ByVal URL As String, ByVal postData As String) As Boolean

        Dim ret As Boolean = True

Retry:
        Try
            Dim sResult As String
            Dim oHttp As HttpWebRequest = System.Net.HttpWebRequest.Create(URL)
            Dim objResponse As HttpWebResponse

            Dim cc As CookieContainer
            Dim c As Cookie
            Dim fi As FileInfo

            Dim encoding As New System.Text.UTF8Encoding
            Dim data() As Byte
            Dim postStream As System.IO.Stream


            oHttp.UserAgent = "Mozilla/5.0 " & _
                              "(Windows NT 6.1; WOW64) " & _
                              "AppleWebKit/536.11 (KHTML, like Gecko) " & _
                              "Chrome/20.0.1132.47 " & _
                              "Safari/536.11"

            oHttp.Method = "POST"
            oHttp.Timeout = (180 * 1000) '3 Minutes
            oHttp.AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate

            data = encoding.GetBytes(postData)


            cc = New CookieContainer

            oHttp.CookieContainer = cc
            oHttp.ContentType = "application/x-www-form-urlencoded"
            oHttp.ContentLength = data.Length


            postStream = oHttp.GetRequestStream
            postStream.Write(data, 0, data.Length)

            objResponse = oHttp.GetResponse

            c = cc.GetCookies(New Uri("http://members.adultfanfiction.net"))(0)
            c.Expires = Date.Now.AddYears(1)

            fi = New FileInfo(Application.StartupPath & "\\" & Replace(Mid(c.Domain, 2), ".", "_") & "_cookie.txt")

            clsCookie.WriteCookiesToDisk(fi.FullName, cc)

            fi = Nothing

            postStream.Close()


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

    Function DownloadPage(ByVal URL As String, Optional ByVal Cookie As String = "") As String

Retry:
        Try
            Dim sResult As String
            Dim oHttp As HttpWebRequest = System.Net.HttpWebRequest.Create(URL)
            Dim objResponse As HttpWebResponse

            Dim cc As CookieContainer

            oHttp.UserAgent = "Mozilla/5.0 " & _
                              "(Windows NT 6.1; WOW64) " & _
                              "AppleWebKit/536.11 (KHTML, like Gecko) " & _
                              "Chrome/20.0.1132.47 " & _
                              "Safari/536.11"

            oHttp.Method = "GET"
            oHttp.Timeout = (180 * 1000) '3 Minutes
            oHttp.AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate

            If Cookie <> "" Then
                If File.Exists(Cookie) Then

                    cc = clsCookie.ReadCookiesFromDisk(Cookie)
                    oHttp.CookieContainer = cc

                End If
            End If

            objResponse = oHttp.GetResponse

            Dim sr As StreamReader
            sr = New StreamReader( _
                                   objResponse.GetResponseStream(), _
                                   System.Text.Encoding.UTF8 _
                                 )


            sResult = sr.ReadToEnd()
            ' Close and clean up the StreamReader
            sr.Close()

            Return sResult

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
        End Try

    End Function

End Module
