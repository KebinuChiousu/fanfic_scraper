Public Class Story

    Public myCaller As Debug

    Public Sub RefreshData()

        Dim row_idx As Integer
        Dim dt As DataTable
        Dim temp As String

        Dim Title As String
        Dim Author As String
        Dim AuthorLink As String
        Dim Folder As String
        Dim Count As String
        Dim StoryID As String
        Dim Abandoned As Boolean
        Dim Complete As Boolean
        Dim PubDate As Date
        Dim UpdDate As Date
        Dim Matchup As String
        Dim Crossover As String
        Dim Description As String

        Dim link As LinkLabel.Link

        row_idx = myCaller.grdDB.CurrentRow.Index
        dt = myCaller.grdDB.DataSource

        Title = dt.Rows(row_idx).Item("Title").ToString
        Author = dt.Rows(row_idx).Item("Author").ToString
        Folder = dt.Rows(row_idx).Item("Folder").ToString
        Count = dt.Rows(row_idx).Item("Count").ToString
        StoryID = dt.Rows(row_idx).Item("StoryID").ToString

        temp = dt.Rows(row_idx).Item("Abandoned").ToString

        If temp = "" Then
            Abandoned = False
        Else
            Abandoned = CBool(dt.Rows(row_idx).Item("Abandoned"))
        End If

        temp = dt.Rows(row_idx).Item("Complete").ToString

        If temp = "" Then
            Complete = False
        Else
            Complete = CBool(dt.Rows(row_idx).Item("Complete"))
        End If

        temp = dt.Rows(row_idx).Item("Publish_Date").ToString

        If temp = "" Then
            PubDate = Nothing
        Else
            PubDate = CDate(dt.Rows(row_idx).Item("Publish_Date"))
        End If

        temp = dt.Rows(row_idx).Item("Update_Date").ToString

        If temp = "" Then
            UpdDate = Nothing
        Else
            UpdDate = CDate(dt.Rows(row_idx).Item("Update_Date"))
        End If

        Matchup = dt.Rows(row_idx).Item("Matchup").ToString

        Crossover = dt.Rows(row_idx).Item("Crossover").ToString

        Description = dt.Rows(row_idx).Item("Description").ToString

        AuthorLink = dt.Rows(row_idx).Item("Internet").ToString

        If AuthorLink <> "" Then
            AuthorLink = Split(AuthorLink, "#")(1)
        End If

        txtTitle.Text = Title

        link = New LinkLabel.Link

        lnkAuthor.Text = Author

        link.Start = 0
        link.Length = Author.Length
        link.LinkData = AuthorLink

        lnkAuthor.Links.Clear()
        lnkAuthor.Links.Add(link)

        txtFolder.Text = Folder
        txtCount.Text = Count
        txtStoryID.Text = StoryID
        chkAbandoned.Checked = Abandoned
        chkComplete.Checked = Complete
        txtPublish.Text = PubDate.ToString("MM/dd/yyyy")
        txtUpdate.Text = UpdDate.ToString("MM/dd/yyyy")
        txtMatchup.Text = Matchup
        txtCrossover.Text = Crossover
        txtdescription.text = Description



    End Sub

    Private Sub lnkAuthor_LinkClicked( _
                                       ByVal sender As Object, _
                                       ByVal e As System.Windows.Forms. _
                                                  LinkLabelLinkClickedEventArgs _
                                     ) Handles lnkAuthor.LinkClicked

        Dim target As String = e.Link.LinkData
        System.Diagnostics.Process.Start(target)

    End Sub

End Class