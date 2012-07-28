Imports System
Imports System.text
Imports System.net
Imports System.IO

Class FFNet

    Public Function GetPageHTML(ByVal URL As String) As String
        Dim result As String
        ' Retrieves the HTML from the specified URL
        Dim objWC As New System.Net.WebClient
        objWC.Headers.Item("User-Agent") = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.7.5) Gecko/20060111"
        result = New System.Text.UTF8Encoding(True).GetString(objWC.DownloadData(URL))
        objWC = Nothing
        GetPageHTML = result
    End Function

    Public Function GrabTitle(ByVal page As String) As String

        Dim sstart As Integer
        Dim sstop As Integer

        sstart = InStr(LCase(page), "<title>") + 7
        sstop = InStr(LCase(page), "</title>")

        GrabTitle = Replace(Mid(page, sstart, (sstop - sstart)), " - FanFiction.Net", "")
        GrabTitle = Replace(GrabTitle, "", "")

    End Function

    Public Function ParseOptionBox(ByVal page As String, ByVal title As String) As String

        Dim sstart As Integer
        Dim sstop As Integer
        Dim test As String = ""
        Dim found As Boolean = True

        Do Until (title = test)

            sstart = InStr(LCase(page), "<select")

            If sstart = 0 Then
                found = False
                Exit Do
            End If

            page = Mid(page, sstart)

            sstart = InStr(LCase(page), "title='") + 7
            page = Mid(page, sstart)
            sstop = InStr(LCase(page), "'")

            test = Mid(page, 1, sstop - 1)


        Loop

        sstart = InStr(LCase(page), ">") + 1
        page = Mid(page, sstart)
        sstart = InStr(LCase(page), ">") + 1
        page = Mid(page, sstart)
        sstop = InStr(LCase(page), "</select>")
        page = Mid(page, 1, sstop)

        If found Then
            ParseOptionBox = page
        Else
            ParseOptionBox = ""
        End If

    End Function

    Public Function GrabDate(ByVal page As String, ByVal title As String) As String

        Dim sstart As Integer

        sstart = InStr(page, title) + Len(title)

        GrabDate = Mid(page, sstart, 8)

    End Function

    Public Function GrabAuthor(ByVal page As String) As String

        Dim sstart As Integer
        Dim sstop As Integer

        sstart = InStr(page, "Author: ")
        page = Mid(page, sstart)

        sstart = InStr(page, ">") + 1
        page = Mid(page, sstart)

        sstop = InStr(page, "<") - 1

        GrabAuthor = Mid(page, 1, sstop)

    End Function

    Public Function GetBody(ByVal page As String) As String

        Dim sstart As String
        Dim sstop As String

        sstart = InStr(page, "<!-- start story -->")
        If sstart < 0 Then Return "retry"
        page = Mid(page, sstart)

        'sstart = InStr(page, "</script>") + Len("</script>")
        'If sstart < 0 Then Return "retry"
        'page = Mid(page, sstart)

        sstop = InStr(LCase(page), "<!-- end story -->") - 1
        If sstop < 0 Then Return "retry"
        page = Mid(page, 1, sstop)

        GetBody = page

    End Function


    Public Sub GetChapters(ByVal lst As ListBox, ByVal page As String)

        Dim data As String
        Dim sstop As String

        data = ParseOptionBox(page, "chapter navigation")

        If data <> "" Then

            Do Until Len(data) = 1

                sstop = InStr(LCase(data), "<") - 1

                lst.Items.Add(Mid(data, 1, sstop))

                sstop = InStr(LCase(data), ">") + 1

                If sstop = 1 Then sstop = Len(data)

                data = Mid(data, sstop)

            Loop

        Else
            lst.Items.Add("Chapter 1")
        End If

    End Sub

    Public Function InitialDownload(ByRef url As String) As String

        Dim host As String = "http://www.fanfiction.net/s/"
        Dim txtResult As String

        txtResult = Replace(url, host, "")
        host = host & Mid(txtResult, 1, InStr(txtResult, "/") - 1)

        url = host & "/"
        txtResult = DownloadPage(url & "1/")

        InitialDownload = txtResult

    End Function

    Public Function ProcessChapters(ByVal URL As String, ByVal index As Integer, ByRef Publish As String, ByRef Update As String, Optional ByVal lst As ListBox = Nothing) As String

        

        Dim txtResult As String

retry:
        txtResult = DownloadPage(URL & (index + 1) & "/")
        If txtResult = "" Then GoTo retry

        If InStr(LCase(txtResult), "id:") <> 0 Then
            txtResult = Mid(txtResult, InStr(LCase(txtResult), "id:"))
        End If

        ProcessChapters = txtResult

    End Function

    Public Function WriteDate(ByVal publish As String, ByVal update As String, ByVal index As Integer, ByVal lstop As Integer) As String

        WriteDate = ""

        If index = 1 Then
            WriteDate = "<p>" & publish & "</p>"
        ElseIf index = lstop Then
            WriteDate = "<p>" & update & "</p>"
        End If

    End Function

End Class
