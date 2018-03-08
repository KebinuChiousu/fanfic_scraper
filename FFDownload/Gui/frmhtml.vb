Imports System.xml
Imports System.io

Public Class Html

    Dim BL As New clsBL

#Region "Drag and Drop / File Browsing"

    Private Sub txtFile_DragDrop( _
                                   ByVal sender As Object, _
                                   ByVal e As System.Windows. _
                                              Forms.DragEventArgs _
                                 ) Handles txtFile.DragDrop

        Dim a As Array = CType(e.Data.GetData(DataFormats.FileDrop), Array)

        txtFile.Text = a(0).ToString


    End Sub

    Private Sub txtFile_DragEnter( _
                                     ByVal sender As Object, _
                                     ByVal e As System.Windows. _
                                                Forms.DragEventArgs _
                                   ) Handles txtFile.DragEnter

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If

    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click

        OpenFile.FileName = ""
        OpenFile.Filter = "HTML (*.htm;*.html)|*.htm;*.html"
        OpenFile.ShowDialog()

        txtFile.Text = OpenFile.FileName

    End Sub

#End Region

    Private Sub btnClean_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClean.Click

        Dim fs As FileStream
        Dim html As String

        fs = File.OpenRead(txtFile.Text)

        Dim sr As StreamReader

        sr = New StreamReader( _
                               fs, _
                               System.Text.Encoding.UTF8 _
                             )

        html = sr.ReadToEnd

        sr.Close()
        sr.Dispose()

        CleanHTML(html)

        BL.ProcessChapter(html, txtPrefix.Text, txtChapter.Text)

    End Sub

    
End Class