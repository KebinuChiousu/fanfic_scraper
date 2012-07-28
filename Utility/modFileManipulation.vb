Imports System
Imports System.IO.File
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
        If Exists(path) = False Then
            ' Create the file.
            fs = Create(path)
            Dim info As Byte() = New UTF8Encoding(True).GetBytes(data)

            ' Add some information to the file.
            fs.Write(info, 0, info.Length)
            fs.Close()
        End If
    End Sub

    Function DownloadPage(ByVal URL As String) As String

Retry:
        Try
            Dim sResult As String
            Dim oHttp As HttpWebRequest = System.Net.HttpWebRequest.Create(URL)
            Dim objResponse As HttpWebResponse

            oHttp.UserAgent = "Mozilla/5.0 " & _
                              "(Windows NT 6.1; WOW64) " & _
                              "AppleWebKit/536.11 (KHTML, like Gecko) " & _
                              "Chrome/20.0.1132.47 " & _
                              "Safari/536.11"

            oHttp.Method = "GET"
            oHttp.Timeout = (180 * 1000) '3 Minutes
            oHttp.AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate
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
