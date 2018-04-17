Public Class frmCategory

    Private DAL As DAL

    Private Sub grdDB_CurrentCellChanged( _
                                        ByVal sender As Object, _
                                        ByVal e As System.EventArgs _
                                      ) Handles grdDB.CurrentCellChanged

        Dim grdDB As DataGridView = CType(sender, DataGridView)
        Dim dt As DataTable

        dt = grdDB.DataSource

        If dt.Rows.Count > 0 Then

            If Not IsNothing(grdDB.CurrentRow) Then

                lblStatus.Text = "(" & _
                                 (grdDB.CurrentRow.Index + 1) & _
                                 " of " & dt.Rows.Count & ")"
            End If

        Else
            lblStatus.Text = "(0 of 0)"
        End If

    End Sub

    Private Sub btnAdd_Click( _
                              sender As System.Object, _
                              e As System.EventArgs _
                            ) Handles btnAdd.Click

        Dim Category As String
        Dim dt As DataTable = grdDB.DataSource
        Dim dr As DataRow
        Dim NewRow As Integer = dt.Rows.Count

        Category = InputBox("Enter Category Name")

        If Category <> "" Then
            If Not Me.DAL.CategoryExists(Category) Then

                dr = dt.NewRow

                dr("Name") = Category

                dt.Rows.Add(dr)

                grdDB.DataSource = dt

                SetCurrentRow(NewRow)

            Else
                MsgBox("Record already exists!", vbExclamation)
            End If
        End If

    End Sub

    Private Sub btnEdit_Click( _
                               sender As System.Object, _
                               e As System.EventArgs _
                             ) Handles btnEdit.Click

        Dim oldCategory As String
        Dim Category As String
        Dim dt As DataTable = grdDB.DataSource
        Dim idx As Integer = grdDB.CurrentRow.Index
        Dim dr As DataRow

        oldCategory = grdDB.CurrentCell.Value

        Category = InputBox("Enter Category Name", , oldCategory)

        If Category <> "" Then
            If Not Me.DAL.CategoryExists(Category) Then

                dr = dt.Rows(idx)

                dr("Name") = Category

                grdDB.DataSource = dt

                SetCurrentRow(idx)

            Else
                MsgBox("Record already exists!", vbExclamation)
            End If
        End If

    End Sub

    Private Sub btnDelete_Click( _
                                 sender As System.Object, _
                                 e As System.EventArgs _
                               ) Handles btnDelete.Click

        Dim Category As String
        Dim CategoryIdx As Integer
        Dim iYesNo As Integer
        Dim dt As DataTable = grdDB.DataSource
        Dim idx As Integer = grdDB.CurrentRow.Index
        Dim dr As DataRow

        Category = grdDB.CurrentCell.Value

        dr = dt.Rows(idx)

        CategoryIdx = dr("id")

        iYesNo = MsgBox("Delete Record: " & Category & "?", MsgBoxStyle.YesNo)

        If iYesNo = vbYes Then
            If Not Me.DAL.RecordExists(CategoryIdx) Then

                dt.Rows.Remove(dr)

                grdDB.DataSource = dt

            Else
                MsgBox("Unable to delete record. Stories exist under this category!", vbExclamation)
            End If
        End If


    End Sub

    Private Sub btnUpdate_Click( _
                                 sender As System.Object, _
                                 e As System.EventArgs _
                               ) Handles btnUpdate.Click

        Dim ret As Integer

        Dim dt As DataTable

        dt = grdDB.DataSource

        If Not IsNothing(dt.GetChanges) Then
            ret = UpdateData(dt)
            If ret > 0 Then
                MsgBox("Database updated sucessfully!")
            Else
                MsgBox("No changes detected...")
            End If
        End If

        If dt.Rows.Count > 0 Then
            frmDebug.FillCategories()
        End If

        RefreshDB()

    End Sub

#Region "Form Handling Routines"

    Private Sub frmCategory_Load( _
                                  sender As Object, _
                                  e As System.EventArgs _
                                ) Handles Me.Load

        DAL = frmDebug.DAL

        RefreshDB()

    End Sub

    Sub SetCurrentRow(Index As Integer)

        grdDB.CurrentCell = grdDB.Rows(Index).Cells(1)

    End Sub

#End Region

#Region "Data Routines"

    Private Function GetCategories() As DataTable

        Dim dt As DataTable

        If Not IsNothing(DAL) Then
            dt = DAL.GetCategories
        Else
            dt = Nothing
        End If

        Return dt

    End Function

    Private Function UpdateData(ByVal dt As DataTable) As Integer

        Dim result As Integer = 0

        result = DAL.UpdateCategories(dt)

        Return result

    End Function

#End Region

    Private Sub RefreshDB()

        Dim dt As DataTable

        dt = GetCategories()

        grdDB.DataSource = dt
        grdDB.Columns("Id").Visible = False

        If dt.Rows.Count = 0 Then
            lblStatus.Text = "(0 of 0)"
        Else
            SetCurrentRow(0)
        End If

    End Sub

    

End Class