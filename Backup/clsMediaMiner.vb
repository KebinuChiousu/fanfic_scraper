Imports System
Imports System.text
Imports System.net
Imports System.IO
Imports System.Web

Class MediaMiner

    Public Function GetPageHTML(ByVal URL As String) As String
        Dim result As String
        ' Retrieves the HTML from the specified URL
        Dim objWC As New System.Net.WebClient
        'objWC.Headers.Item("User-Agent") = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.7.5) Gecko/20060111"
        result = New System.Text.UTF8Encoding(True).GetString(objWC.DownloadData(URL))
        objWC = Nothing
        GetPageHTML = result
    End Function

    Public Function GrabTitle(ByVal page As String)

        Dim sstart As Integer
        Dim sstop As Integer

        'sstart = InStr(LCase(page), "<title>") + 7
        'sstop = InStr(LCase(page), "</title>")

        'GrabTitle = Replace(Mid(page, sstart, (sstop - sstart)), "MediaMiner - Fan Fic: ", "")

        sstart = InStr(LCase(Replace(page, Chr(34), "'")), "class='ffh'>") + Len("class='ffh'>")
        page = Mid(page, sstart)

        If InStr(page, "<") <> 0 Then
            page = Mid(page, 1, InStr(page, "<") - 1)
        Else
            page = Mid(page, 1, InStr(page, ":") - 1)
        End If


        GrabTitle = page

    End Function

    Public Function ParseOptionBox(ByVal page As String) As String

        Dim sstart As Integer
        Dim sstop As Integer

        sstart = InStr(LCase(page), "<select")

        If sstart <> 0 Then

            page = Mid(page, sstart)
            sstart = InStr(LCase(page), ">") + 1
            page = Mid(page, sstart)
            sstop = InStr(LCase(page), "</select>")
            page = Mid(page, 1, sstop)

        Else
            frmMain.ListChapters.Items.Add(frmMain.txtUrl.Text)
            page = ""
        End If

        ParseOptionBox = page

    End Function

    Private Function GrabUpload(ByVal page As String) As String

        Dim sstart As Integer
        Dim title As String = "<b>Uploaded On: </b>"

        sstart = InStr(page, title) + Len(title)
        page = Mid(page, sstart)
        GrabUpload = title & Mid(page, 1, (InStr(page, ":") - 4))

    End Function

    Private Function GrabUpdate(ByVal page As String) As String

        Dim sstart As Integer
        Dim title As String = "<b>Updated On: </b>"

        If InStr(page, title) <> 0 Then
            sstart = InStr(page, title) + Len(title)
            page = Mid(page, sstart)
            GrabUpdate = title & Mid(page, 1, (InStr(page, "<") - 1))
        Else
            GrabUpdate = ""
        End If

    End Function

    Public Function GrabDate(ByVal page As String, ByVal title As String) As String

        Select Case title
            Case "Published: "
                GrabDate = GrabUpload(page)
            Case "Updated: "
                GrabDate = GrabUpdate(page)
        End Select

    End Function

    Public Function GrabAuthor(ByVal page As String) As String

        Dim sstart As Integer
        Dim sstop As Integer

        sstart = InStr(page, "<a href")
        page = Mid(page, sstart)

        sstart = InStr(page, ">") + 1
        page = Mid(page, sstart)

        sstop = InStr(page, "<") - 1

        GrabAuthor = Mid(page, 1, sstop)

    End Function

    Public Function GetBody(ByVal page As String) As String

        Dim sstart As String
        Dim sstop As String

        sstop = InStr(LCase(page), "<table") - 1
        page = Mid(page, 1, sstop)

        GetBody = page

    End Function

    Sub GetChapters(ByRef lst As ListBox, ByRef page As String)
        Dim host As String = "http://www.mediaminer.org/fanfic/view_ch.php?"
        Dim url As String

        Dim data As String
        Dim sstart As String
        Dim sstop As String

        Dim id As String
        Dim search As String = "<input type='hidden' name='id' value='"



        id = Replace(page, Chr(34), "'")
        sstart = InStr(id, search) + Len(search)
        id = Mid(id, sstart)
        sstop = InStr(id, "'") - 1
        id = Mid(id, 1, sstop)

        data = ParseOptionBox(page)
        data = Replace(data, Chr(34), "'")

        Do Until Len(data) = 1

            sstart = InStr(LCase(data), "value='") + 7

            If sstart = 7 Then Exit Do

            data = Mid(data, sstart)

            sstop = InStr(LCase(data), "'") - 1

            url = host & "cid=" & Mid(data, 1, sstop) & _
                  "&submit=View+Chapter&id=" & id

            lst.Items.Add(url)

            sstop = InStr(LCase(data), ">") + 1

            If sstop = 1 Then sstop = Len(data)

            data = Mid(data, sstop)

        Loop

        page = DownloadPage(lst.Items(0))
        page = Mid(page, InStr(page, "Genre(s):"))
        page = Mid(page, InStr(page, "Author:"))


    End Sub


    Public Function InitialDownload(ByVal url As String) As String
        InitialDownload = DownloadPage(url)
    End Function

    Public Function ProcessChapters(ByVal URL As String, _
                                    ByVal index As Integer, _
                                    ByRef Publish As String, _
                                    ByRef Update As String, _
                                    Optional ByVal lst As ListBox = Nothing _
                                   ) As String


        Dim txtResult As String

        txtResult = DownloadPage(lst.Items(index))
        txtResult = Mid(txtResult, InStr(txtResult, "Genre(s):"))
        txtResult = Mid(txtResult, InStr(txtResult, "Author:"))
        Publish = GrabDate(txtResult, "Published: ")
        Update = GrabDate(txtResult, "Updated: ")

        txtResult = Mid(txtResult, InStr(txtResult, "</div>"))

        If InStr(LCase(txtResult), "<select") <> 0 Then

            txtResult = Mid(txtResult, InStr(txtResult, "</table>") _
                                       + Len("</table>"))
        End If



        txtResult = Replace(txtResult, "</div>", "<p>")

        txtResult = HttpUtility.HtmlDecode(txtResult)

        ProcessChapters = txtResult

    End Function

    Public Function WriteDate(ByVal publish As String, ByVal update As String, ByVal index As Integer, ByVal lstop As Integer) As String

        WriteDate = "<p>" & publish & "</p>"
        WriteDate &= "<p>" & update & "</p>"

    End Function

    Public Function XMLBuilder(ByVal fldStoryUrl As String, _
    ByVal fldTitle As String, _
    ByVal fldAuthorUrl As String, _
    ByVal fldAuthor As String, _
    ByVal fldRevision As String, _
    ByVal fldSummary As String, _
    ByVal fldanime As String) As String

        Dim result As String

        result = "<entry>"
        result &= "<author>"
        result &= "<name>"
        result &= fldAuthor
        result &= "</name>"
        result &= "<uri>"
        result &= fldAuthorUrl
        result &= "</uri>"
        result &= "</author>"
        result &= "<category term=" & Chr(34) & "Anime" & Chr(34) & ">"
        result &= fldanime
        result &= "</category>"
        result &= "<published>"
        result &= fldRevision
        result &= "</published>"
        result &= "<updated>"
        result &= fldRevision
        result &= "</updated>"
        result &= "<title type=" & Chr(34) & "html" & Chr(34) & ">"
        result &= fldTitle
        result &= "</title>"
        result &= "<link rel=" & Chr(34) & "alternate" & Chr(34) & _
                  " href=" & Chr(34)
        result &= fldStoryUrl & Chr(34)
        result &= " />"
        result &= "<summary type=" & Chr(34) & "html" & Chr(34) & ">"
        result &= fldSummary
        result &= "</summary>"
        result &= "</entry>"

        Return result

    End Function

End Class
