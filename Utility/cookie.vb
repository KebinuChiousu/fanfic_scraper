Imports System.Runtime.InteropServices
Imports System.Text
Imports System.net

Public Class cookie

    <DllImport("wininet.dll", SetLastError:=True)> _
    Public Shared Function InternetGetCookie( _
                                              ByVal url As String, _
                                              ByVal cookieName As String, _
                                              ByVal cookieData As StringBuilder, _
                                              ByRef size As Integer) As Boolean
    End Function

    Public Shared Function GetUriCookieContainer(ByVal uri As Uri) As CookieContainer
        Dim cookies As CookieContainer = Nothing

        ' Determine the size of the cookie
        Dim datasize As Integer = 256
        Dim cookieData As New StringBuilder(datasize)

        If Not InternetGetCookie(uri.ToString(), Nothing, cookieData, datasize) Then
            If datasize < 0 Then
                Return Nothing
            End If
            ' Allocate stringbuilder large enough to hold the cookie
            cookieData = New StringBuilder(datasize)
            If Not InternetGetCookie(uri.ToString(), Nothing, cookieData, datasize) Then
                Return Nothing
            End If
        End If
        If cookieData.Length > 0 Then
            cookies = New CookieContainer
            cookies.SetCookies(uri, cookieData.ToString().Replace(";"c, ","c))
        End If
        Return cookies
    End Function 'GetUriCookieContainer

End Class

'Sample Usage
'
'Dim ck As New cookie
'Dim siteUri As New Uri(URL)
'Dim sitecookie As CookieContainer = ck.GetUriCookieContainer(siteUri)

