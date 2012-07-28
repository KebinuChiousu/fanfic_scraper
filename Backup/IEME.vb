Public Class IEME
    Dim oIE As SHDocVw.InternetExplorer
    Public oDOC As mshtml.IHTMLDocument2
    Public Sub New()
        oIE = New SHDocVw.InternetExplorer
        'oIE.Visible = True
    End Sub
    Public Sub New(ByVal strURL As String)
        On Error Resume Next
        Dim SWs As New SHDocVw.ShellWindows
        Dim IE As SHDocVw.InternetExplorer
        Dim DOC As mshtml.IHTMLDocument2
        For Each IE In SWs
            DOC = IE.Document
            If TypeOf DOC Is mshtml.HTMLDocument Then
                If InStr(DOC.url, strURL) > 0 Then
                    oIE = IE
                    oDOC = IE.Document
                End If
            End If
        Next
        If (oIE Is Nothing) Then
            oIE = New SHDocVw.InternetExplorer
            oIE.Navigate(strURL)
            Do
            Loop Until Not oIE.Busy
            oDOC = oIE.Document
        End If
        'oIE.Visible = True
    End Sub
    Public Function Nav(ByVal strURL As String)
        oIE.Navigate(strURL)
        Do
        Loop Until Not oIE.Busy
        oDOC = oIE.Document
    End Function

    Protected Overrides Sub Finalize()
        oIE = Nothing
        oDOC = Nothing
        MyBase.Finalize()
    End Sub
End Class
