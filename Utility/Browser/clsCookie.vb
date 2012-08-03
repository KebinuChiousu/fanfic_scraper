Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization.Formatters.Binary


Class clsCookie

    Public Shared Sub WriteCookiesToDisk(ByVal fileName As String, ByVal cookieJar As CookieContainer)
        Using stream As Stream = File.Create(fileName)
            Try
                'MsgBox("Writing cookies to disk... ")
                Dim formatter As New BinaryFormatter()
                formatter.Serialize(stream, cookieJar)
                'MsgBox("Done.")
            Catch e As Exception
                'MsgBox("Problem writing cookies to disk: " & e.Message)
            End Try
        End Using
    End Sub

    Public Shared Function ReadCookiesFromDisk(ByVal fileName As String) As CookieContainer

        Try
            Using stream As Stream = File.Open(fileName, FileMode.Open)
                'MsgBox("Reading cookies from disk... ")
                Dim formatter As New BinaryFormatter()
                'MsgBox("Done.")
                Return DirectCast(formatter.Deserialize(stream), CookieContainer)
            End Using
        Catch e As Exception
            'MsgBox("Problem reading cookies from disk: " & e.Message())
            Return New CookieContainer()
        End Try
    End Function
End Class
