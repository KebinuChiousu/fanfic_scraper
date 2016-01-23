Imports System.IO

Public Class Debug
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents grdRSS As System.Windows.Forms.DataGrid
    Friend WithEvents btnUpdateDB As System.Windows.Forms.Button
    Friend WithEvents btnUpdateRecord As System.Windows.Forms.Button
    Friend WithEvents cmbSearch As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBatch As System.Windows.Forms.Button
    Friend WithEvents btnOpenDB As System.Windows.Forms.Button
    Friend WithEvents btnPath As System.Windows.Forms.Button
    Friend WithEvents tmrDownload As System.Windows.Forms.Timer
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cmbChooseDB As System.Windows.Forms.ComboBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents grdDB As System.Windows.Forms.DataGridView
    Friend WithEvents btnCategory As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.grdRSS = New System.Windows.Forms.DataGrid()
        Me.btnUpdateDB = New System.Windows.Forms.Button()
        Me.btnUpdateRecord = New System.Windows.Forms.Button()
        Me.cmbSearch = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnBatch = New System.Windows.Forms.Button()
        Me.btnOpenDB = New System.Windows.Forms.Button()
        Me.btnPath = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.tmrDownload = New System.Windows.Forms.Timer(Me.components)
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.cmbChooseDB = New System.Windows.Forms.ComboBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.grdDB = New System.Windows.Forms.DataGridView()
        Me.btnCategory = New System.Windows.Forms.Button()
        CType(Me.grdRSS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdDB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdRSS
        '
        Me.grdRSS.DataMember = ""
        Me.grdRSS.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdRSS.Location = New System.Drawing.Point(8, 8)
        Me.grdRSS.Name = "grdRSS"
        Me.grdRSS.Size = New System.Drawing.Size(855, 147)
        Me.grdRSS.TabIndex = 27
        '
        'btnUpdateDB
        '
        Me.btnUpdateDB.Enabled = False
        Me.btnUpdateDB.Location = New System.Drawing.Point(743, 367)
        Me.btnUpdateDB.Name = "btnUpdateDB"
        Me.btnUpdateDB.Size = New System.Drawing.Size(120, 24)
        Me.btnUpdateDB.TabIndex = 31
        Me.btnUpdateDB.Text = "Update Database"
        '
        'btnUpdateRecord
        '
        Me.btnUpdateRecord.Enabled = False
        Me.btnUpdateRecord.Location = New System.Drawing.Point(743, 397)
        Me.btnUpdateRecord.Name = "btnUpdateRecord"
        Me.btnUpdateRecord.Size = New System.Drawing.Size(120, 24)
        Me.btnUpdateRecord.TabIndex = 32
        Me.btnUpdateRecord.Text = "Update Record"
        '
        'cmbSearch
        '
        Me.cmbSearch.Items.AddRange(New Object() {"Title", "Author", "Folder"})
        Me.cmbSearch.Location = New System.Drawing.Point(426, 397)
        Me.cmbSearch.Name = "cmbSearch"
        Me.cmbSearch.Size = New System.Drawing.Size(101, 24)
        Me.cmbSearch.TabIndex = 33
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(422, 372)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 16)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "Search By"
        '
        'btnBatch
        '
        Me.btnBatch.Enabled = False
        Me.btnBatch.Location = New System.Drawing.Point(534, 397)
        Me.btnBatch.Name = "btnBatch"
        Me.btnBatch.Size = New System.Drawing.Size(202, 24)
        Me.btnBatch.TabIndex = 35
        Me.btnBatch.Text = "Batch Download Updates"
        '
        'btnOpenDB
        '
        Me.btnOpenDB.Location = New System.Drawing.Point(178, 396)
        Me.btnOpenDB.Name = "btnOpenDB"
        Me.btnOpenDB.Size = New System.Drawing.Size(62, 24)
        Me.btnOpenDB.TabIndex = 38
        Me.btnOpenDB.Text = "Open"
        '
        'btnPath
        '
        Me.btnPath.Location = New System.Drawing.Point(8, 373)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(58, 47)
        Me.btnPath.TabIndex = 39
        Me.btnPath.Text = "Set Paths"
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(247, 375)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(87, 46)
        Me.btnSave.TabIndex = 40
        Me.btnSave.Text = "Save Story to DB"
        '
        'tmrDownload
        '
        Me.tmrDownload.Interval = 1500
        '
        'btnSearch
        '
        Me.btnSearch.Enabled = False
        Me.btnSearch.Location = New System.Drawing.Point(341, 375)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(74, 46)
        Me.btnSearch.TabIndex = 41
        Me.btnSearch.Text = "Search DB"
        '
        'cmbChooseDB
        '
        Me.cmbChooseDB.FormattingEnabled = True
        Me.cmbChooseDB.Location = New System.Drawing.Point(73, 373)
        Me.cmbChooseDB.Name = "cmbChooseDB"
        Me.cmbChooseDB.Size = New System.Drawing.Size(167, 24)
        Me.cmbChooseDB.TabIndex = 42
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(530, 374)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(31, 17)
        Me.lblStatus.TabIndex = 43
        Me.lblStatus.Text = "DB:"
        '
        'grdDB
        '
        Me.grdDB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdDB.Location = New System.Drawing.Point(8, 162)
        Me.grdDB.Name = "grdDB"
        Me.grdDB.Size = New System.Drawing.Size(855, 198)
        Me.grdDB.TabIndex = 44
        '
        'btnCategory
        '
        Me.btnCategory.Location = New System.Drawing.Point(73, 397)
        Me.btnCategory.Name = "btnCategory"
        Me.btnCategory.Size = New System.Drawing.Size(99, 23)
        Me.btnCategory.TabIndex = 46
        Me.btnCategory.Text = "Categories..."
        Me.btnCategory.UseVisualStyleBackColor = True
        '
        'Debug
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(870, 430)
        Me.Controls.Add(Me.btnCategory)
        Me.Controls.Add(Me.grdDB)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.cmbChooseDB)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnPath)
        Me.Controls.Add(Me.btnOpenDB)
        Me.Controls.Add(Me.btnBatch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbSearch)
        Me.Controls.Add(Me.btnUpdateRecord)
        Me.Controls.Add(Me.btnUpdateDB)
        Me.Controls.Add(Me.grdRSS)
        Me.Name = "Debug"
        Me.Text = "Download Automation"
        CType(Me.grdRSS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdDB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Enum Process
        AuthorPage = 0
        StoryPage = 1
    End Enum

    Public DAL As DAL
    Public myCaller As HtmlGrabber
    Public Navigate As Process

    ' Declare a new DataGridTableStyle in the
    ' declarations area of your form.
    Dim ts As DataGridTableStyle = New DataGridTableStyle()

#Region "Database Routines"

    Private DB As String

    Function GetData( _
                      ByVal Category_ID As Integer, _
                      Optional ByVal ALL As Boolean = False _
                    ) As DataTable

        Dim dt As DataTable

        DB = cmbChooseDB.Text

        dt = DAL.GetData(Category_ID, ALL)

        Return dt

    End Function

    Private Function UpdateData(ByVal dt As DataTable) As Integer

        Dim result As Integer = 0

        result = DAL.UpdateData(dt)

        Return result

    End Function

    Private Function GetCategories() As DataTable

        Dim dt As DataTable

        Try
            dt = DAL.GetCategories
        Catch
            dt = Nothing
        End Try

        Return dt

    End Function

    Public Sub UpdateRSS(ByVal ds As DataSet)

        'Initialize Debug Console
        Initialize( _
                    forms.frmDebug _
                  )

        'Update Data in Debug Console
        Try
            grdRSS.DataMember = ds.Tables(0).TableName
            grdRSS.DataSource = ds
            grdRSS.Visible = True
        Catch
        End Try
    End Sub

    Private Sub UpdateDateBase( _
                                  ByVal sender As System.Object, _
                                  ByVal e As System.EventArgs _
                              ) Handles btnUpdateDB.Click

        Dim ret As Integer

        Dim dt As DataTable

        dt = grdDB.DataSource

        Dim cat_id As String

        Try
            cat_id = dt.Rows(grdDB.CurrentRow.Index). _
            Item("Category_Id").ToString

            If cat_id = "" Or cat_id = "0" Then
                cat_id = cmbChooseDB.SelectedValue
                dt.Rows(grdDB.CurrentRow.Index). _
                Item("Category_Id") = cat_id
            End If
        Catch
        End Try

        If Not IsNothing(dt.GetChanges) Then
            ret = UpdateData(dt)
            If ret > 0 Then
                MsgBox("Database updated sucessfully!")
            Else
                MsgBox("No changes detected...")
            End If
        End If

    End Sub

#End Region

#Region "Interface Code"

    Private Sub UpdateRec( _
                            ByVal sender As System.Object, _
                            ByVal e As System.EventArgs _
                          ) Handles btnUpdateRecord.Click
        UpdateRecord()
    End Sub

    Private Sub btnPath_Click( _
                                  ByVal sender As System.Object, _
                                  ByVal e As System.EventArgs _
                                ) Handles btnPath.Click
        Dim frmpath As New frmPath
        frmpath.Show()
    End Sub

    Private Sub GotoNextRecord( _
                                   ByVal sender As System.Object, _
                                   ByVal e As System.EventArgs _
                                 )
        MoveNext()
    End Sub

    Private Sub cmbProcess_SelectedIndexChanged( _
                                                    ByVal sender As System.Object, _
                                                    ByVal e As System.EventArgs _
                                                  ) Handles cmbSearch.SelectedIndexChanged

        'Select Case cmbSearch.Text
        '    Case "StoryID"
        Navigate = Process.StoryPage
        '    Case "Author"
        'Navigate = Process.AuthorPage
        'End Select

        ResetInfo()

    End Sub

#End Region

#Region "Form Handling Routines"

    Sub InitConfig()

        Const empty As String = "No Path Set"

        Dim fi As FileInfo

        Dim ifr As IniFileReader

        Dim val As String

        fi = New FileInfo(Application.StartupPath & "\\" & "config.ini")

        If fi.Exists Then
            ifr = New IniFileReader(Application.StartupPath & "\config.ini", True)

            val = ifr.GetIniValue("FanFic", "Path")

            If val <> empty Then

                ReloadDAL("Fanfic", val, False)
                DAL.UpdateConnStr("FanFic", val)
                ReloadDAL("Fanfic", val)

            Else
                ReloadDAL("Fanfic", "", False)
            End If

        End If

        ifr = Nothing

        My.Settings.Reload()

    End Sub

    Public Sub ReloadDAL(csname As String, path As String, Optional ByVal Init As Boolean = True)

        frmDebug.DAL = Nothing

        'Dim fi As FileInfo
        'fi = New FileInfo(path)

        DAL = New SQLite("Fanfic", Init)

        'Select Case fi.Extension
        '    Case ".mdb"
        '      DAL = New Access
        '    Case ".db"
        '      DAL = New SQLite("FanFic", Init)
        'End Select

    End Sub

    Private Sub frmDebug_Load( _
                               ByVal sender As System.Object, _
                               ByVal e As System.EventArgs _
                             ) Handles MyBase.Load



        PlaceDebugWindow()
        cmbSearch.SelectedIndex = 0
        InitConfig()

        FillCategories()

    End Sub

    Public Sub FillCategories()

        Dim dt As DataTable

        dt = GetCategories()

        cmbChooseDB.DataSource = dt
        cmbChooseDB.ValueMember = "Id"
        cmbChooseDB.DisplayMember = "Name"

    End Sub


    Private Sub frmDebug_Closing( _
                                  ByVal sender As Object, _
                                  ByVal e As System. _
                                             ComponentModel. _
                                             CancelEventArgs _
                                ) Handles MyBase.Closing

        e.Cancel = True
        Me.Hide()

    End Sub

#End Region

#Region "Validation Routines"

    Function isStale( _
                      ByRef dt As DataTable, _
                      ByVal pos As Long _
                    ) As Boolean

        Dim ret As Boolean = False

        Dim publish_date As Date
        Dim update_date As Date
        Dim last_checked As Date
        Dim tsDelta As TimeSpan

        Dim wait As Integer


        If dt.Rows(pos).Item("Last_Checked").ToString = "" Then
            dt.Rows(pos).Item("Last_checked") = CDate(Date.Today)
            UpdateData(dt)
            ret = False
            Return ret
        End If


        If dt.Rows(pos).Item("Publish_Date").ToString = "" Then
            ret = False
            Return ret
        Else
            publish_date = CDate(dt.Rows(pos).Item("Publish_Date").ToString)
        End If

        If dt.Rows(pos).Item("Update_Date").ToString = "" Then
            update_date = publish_date
        Else
            update_date = CDate(dt.Rows(pos).Item("Update_Date").ToString)
        End If

        last_checked = CDate(dt.Rows(pos).Item("Last_Checked").ToString)

        tsDelta = last_checked.Subtract(update_date)

        Select Case tsDelta.Days
            Case Is > 360
                wait = 180
            Case Is > 180
                wait = 90
            Case Is > 90
                wait = 60
            Case Is > 60
                wait = 30
            Case Is > 30
                wait = 7
            Case Else
                wait = 0
        End Select

        tsDelta = Date.Today.Subtract(last_checked)
        If tsDelta.Days < wait Then
            ret = True
        End If

        Return ret

    End Function

    Function isValid( _
                      ByRef dt As DataTable, _
                      ByVal pos As Long _
                    ) As Boolean

        Dim ret As Boolean = False
        Dim link As String

        Dim params() As String

        If dt.Rows(grdDB.CurrentRow.Index). _
           Item("Internet").GetType Is GetType(DBNull) _
        Then
            ret = False
        Else

            link = dt.Rows(grdDB.CurrentRow.Index).Item("Internet")

            params = Split(link, "#")

            If UBound(params) > 0 Then
                link = params(1)
            Else
                link = params(0)
            End If

            ret = BL.CheckUrl(link)

        End If

        Return ret

    End Function

#End Region

    Sub ProcessStory( _
                      ByVal dt As DataTable, _
                      ByVal pos As Long, _
                      ByVal link As String _
                    )

        Dim fic As clsFanfic.Story
        Dim folder As String
        Dim check As Boolean
        Dim current As Integer
        Dim start As Integer

        check = BL.GetChapters(link)

        fic = BL.FanFic


        start = CInt(dt.Rows(pos).Item("Count") + 1)

        folder = dt.Rows(pos).Item("Folder")

        If check Then

            frmMain.UpdateUI(fic, BL.Result, start)

            Application.DoEvents()

            frmMain.txtFileMask.Text = folder & "-"

            If dt.Rows(pos).Item("Publish_Date").ToString = "" Then
                dt.Rows(pos).Item("Publish_Date") = CDate(fic.PublishDate)
                UpdateData(dt)
            End If

            current = dt.Rows(pos).Item("Count")

            If CInt(fic.ChapterCount) > current Then

                dt.Rows(pos).Item("Count") = CInt(fic.ChapterCount)

                BL.ProcessChapters(link, start, fic.ChapterCount, folder, cmbChooseDB.Text)

                fic = BL.FanFic

                dt.Rows(pos).Item("Update_Date") = CDate(fic.UpdateDate)

            Else
                If CInt(fic.ChapterCount) < current Then
                    dt.Rows(pos).Item("Count") = CInt(fic.ChapterCount)
                End If
            End If

            dt.Rows(pos).Item("Last_Checked") = CDate(Date.Today)

            UpdateData(dt)

        Else

            BL.ProcessError(link, start, folder, cmbChooseDB.Text)

        End If

    End Sub

    Function MoveNext( _
                       Optional ByVal initial As Boolean = False _
                     ) As Integer

        Dim pos As Long
        Dim dt As DataTable
        Dim url As String = ""
        Dim data As String = ""
        Dim abort As Boolean

        dt = grdDB.DataSource

bypass:

        If initial Then
            Application.DoEvents()
            SetCurrentRow(0)
            pos = 0
        Else
            pos = grdDB.CurrentRow.Index
            pos += 1
        End If



        If (pos <= (dt.Rows.Count - 1)) Then
            If initial = False Then
                SetCurrentRow(pos)
                Application.DoEvents()
            End If
        Else
            Return -1
        End If

        abort = False

        If GetStoryID() = "" Then
            abort = True
        Else
            If Not isValid(dt, pos) Then
                abort = True
            Else
                If Not isStale(dt, pos) Then

                    Select Case Navigate
                        Case Process.AuthorPage
                            url = GetAtom(dt)
                        Case Process.StoryPage
                            url = GetStory(dt)
                    End Select
                Else
                    abort = True
                End If
            End If
        End If



        If abort Then
            initial = False
            GoTo bypass
        Else
            Select Case Navigate
                Case Process.AuthorPage
                    UpdateAtom(url)
                Case Process.StoryPage
                    ProcessStory(dt, pos, url)
            End Select
        End If

        Return pos

    End Function

    Private Sub ProcessBatch( _
                              ByVal sender As System.Object, _
                              ByVal e As System.EventArgs _
                            ) Handles btnBatch.Click

        Dim dt As New DataTable
        Dim start As Integer

        dt = GetData(CInt(cmbChooseDB.SelectedValue))

        If dt.Rows.Count = 0 Then
            MsgBox("No new stories to download!", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        grdDB.DataSource = dt

        Application.DoEvents()

        start = MoveNext(True)

        tmrDownload.Enabled = True

    End Sub

    Private Sub RefreshDB()

        Dim dt As DataTable

        Dim cat_id As Integer

        cat_id = CInt(cmbChooseDB.SelectedValue.ToString)

        dt = GetData(cat_id, True)

        btnSave.Enabled = True
        btnSearch.Enabled = True

        btnBatch.Enabled = True

        btnUpdateDB.Enabled = True
        btnUpdateRecord.Enabled = True

        grdDB.DataSource = dt
        grdDB.Columns("Id").Visible = False
        grdDB.Columns("Category_Id").Visible = False
        SetCurrentRow(0)

        If dt.Rows.Count > 0 Then

            lblStatus.Text = "(" & _
                             (grdDB.CurrentRow.Index + 1) & _
                             " of " & dt.Rows.Count & ")"
        Else
            lblStatus.Text = "(0 of 0)"
        End If

    End Sub

    Sub SetCurrentRow(Index As Integer)

        grdDB.CurrentCell = grdDB.Rows(Index).Cells(1)

    End Sub

#Region "Interface Code"

    Private Sub btnOpenDB_Click( _
                        ByVal sender As System.Object, _
                        ByVal e As System.EventArgs _
                      ) Handles btnOpenDB.Click

        RefreshDB()

    End Sub

    Private Sub btnSave_Click( _
                               ByVal sender As System.Object, _
                               ByVal e As System.EventArgs _
                             ) Handles btnSave.Click

        Dim fic As clsFanfic.Story
        Dim id As String
        Dim dt As DataTable
        Dim NewRow As Integer
        Dim iYesNo As Integer
        Dim link As String
        Dim folder As String
        Dim cat_id As Integer

        dt = grdDB.DataSource

        NewRow = dt.Rows.Count

        iYesNo = MsgBox("Add New Record?", MsgBoxStyle.YesNo)
        cat_id = CInt(cmbChooseDB.SelectedValue)

        If iYesNo = vbYes Then

            folder = InputBox("Enter File Name")

            If Not Me.DAL.RecordExists(folder, cat_id) Then

                Dim dr As DataRow = dt.NewRow

                link = myCaller.txtUrl.Text
                id = BL.GetStoryID(link)
                fic = BL.GetStoryInfoByID(id)

                dr("Title") = fic.Title
                dr("Author") = fic.Author
                dr("Folder") = folder 'Folder Name

                'dr("Chapter") = "" 'Current Chapter

                dr("Count") = 0 'Chapter Count

                'dr("Matchup") = "" ' Matchup

                dr("Description") = fic.Summary
                dr("Internet") = fic.Author & "#" & fic.AuthorURL & "#"
                dr("StoryId") = fic.ID
                dr("Complete") = False
                dr("Publish_Date") = fic.PublishDate
                dr("Category_Id") = cat_id

                dt.Rows.Add(dr)

                grdDB.DataSource = dt

                SetCurrentRow(NewRow)

            Else
                MsgBox("Record already exists!", vbExclamation)
            End If
        End If

        dt = Nothing

    End Sub

    Private Sub tmrDownload_Tick( _
                                  ByVal sender As Object, _
                                  ByVal e As System.EventArgs _
                                ) Handles tmrDownload.Tick

        Dim pos As Long

        tmrDownload.Enabled = False
        pos = MoveNext()

        If pos <> -1 Then
            tmrDownload.Enabled = True
        Else
            RefreshDB()
        End If

    End Sub

    Private Sub btnSearch_Click( _
                                 ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs _
                               ) Handles btnSearch.Click

        If cmbSearch.Text = "" Then
            MsgBox("Please select field to search by!", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Dim title As String = ""
        Dim folder As String = ""
        Dim author As String = ""

        Dim search As String = ""
        Dim result As String = ""

        Dim value As String = ""


        search = InputBox("Enter Value to Search For.", "Fanfiction DB")
        search = UCase(search)

        If search = "" Then Exit Sub

        Dim count As Integer
        Dim start As Integer

        start = grdDB.CurrentRow.Index

        Dim dt As DataTable
        dt = grdDB.DataSource

        If start < (dt.Rows.Count - 1) Then
            start = start + 1
        Else
            start = 0
        End If

        For count = start To (dt.Rows.Count - 1)

            SetCurrentRow(count)

            Application.DoEvents()

            folder = grdDB.Item(3, grdDB.CurrentRow.Index).Value
            title = grdDB.Item(1, grdDB.CurrentRow.Index).Value
            author = grdDB.Item(2, grdDB.CurrentRow.Index).Value

            Select Case cmbSearch.Text
                Case "Title"
                    value = UCase(title)
                Case "Author"
                    value = UCase(author)
                Case "Folder"
                    value = UCase(folder)
            End Select

            Select Case cmbSearch.Text
                Case "Folder"
                    If InStr(value, search) = 1 Then
                        result = value
                    End If
                Case Else
                    If InStr(value, search) <> 0 Then
                        result = value
                    End If
            End Select


            If result <> "" Then
                Exit For
            End If

        Next

        If result = "" Then

            For count = 0 To start

                SetCurrentRow(count)
                Application.DoEvents()

                folder = grdDB.Item(3, grdDB.CurrentRow.Index).Value
                title = grdDB.Item(1, grdDB.CurrentRow.Index).Value
                author = grdDB.Item(2, grdDB.CurrentRow.Index).Value

                Select Case cmbSearch.Text
                    Case "Title"
                        value = UCase(title)
                    Case "Author"
                        value = UCase(author)
                    Case "Folder"
                        value = UCase(folder)
                End Select

                Select Case cmbSearch.Text
                    Case "Folder"
                        If InStr(value, search) = 1 Then
                            result = value
                        End If
                    Case Else
                        If InStr(value, search) <> 0 Then
                            result = value
                        End If
                End Select

                If result <> "" Then
                    Exit For
                End If

            Next

        End If

        If result = "" Then
            MsgBox("Story Does Not Exist in Database")
        Else
            SetCurrentRow(count)
            MsgBox( _
                    "Folder: " & folder & vbCrLf & _
                    "Author: " & author & vbCrLf & _
                    "Title: " & title & vbCrLf _
                  )

        End If

    End Sub

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

    Private Sub grdDB_DoubleClick( _
                                   ByVal sender As Object, _
                                   ByVal e As System.EventArgs _
                                 ) Handles grdDB.DoubleClick

        Initialize( _
                    forms.frmStory _
                  )

        frmStory.myCaller = Me

        frmStory.Show()

        frmStory.TopMost = True
        frmStory.RefreshData()
        frmStory.TopMost = False

    End Sub

#End Region

#Region "Retrieval Functions"

    Function GetStory( _
                       ByVal dt As DataTable _
                     ) As String

        Dim link As String
        Dim StoryID As String

        If dt.Rows(grdDB.CurrentRow.Index). _
           Item("StoryId").GetType Is GetType(DBNull) _
        Then
            link = ""
        Else

            StoryID = dt.Rows(grdDB.CurrentRow.Index). _
                      Item("StoryID")

            If StoryID = "" Then
                link = ""
            Else
                link = BL.GetStoryURL(StoryID)
            End If

        End If

        Return link

    End Function

    Function GetAtom( _
                         ByVal dt As DataTable _
                       ) As String

        If dt.Rows(grdDB.CurrentRow.Index). _
           Item("Internet").GetType Is GetType(DBNull) _
        Then
            GetAtom = ""
        Else
            GetAtom = Replace( _
                               dt.Rows(grdDB.CurrentRow.Index). _
                               Item("Internet") _
                               , _
                               "#", _
                               "" _
                             )
        End If

    End Function

#End Region

#Region "Utility Routines"

    Private Function GetStoryID() As String

        Dim ret As String

        Dim dt As DataTable
        dt = grdDB.DataSource

        ret = _
        dt.Rows(grdDB.CurrentRow.Index). _
        Item("StoryID").ToString

        frmMain.lblStoryID.Text = ret

        Return ret

    End Function

    Private Sub UpdateRecord()

        Dim fic As clsFanfic.Story
        Dim id As String
        Dim link As String
        Dim dt As DataTable
        Dim dte As Date

        dt = grdDB.DataSource

        link = myCaller.txtUrl.Text
        id = BL.GetStoryID(link)
        fic = BL.GetStoryInfoByID(id)

        dt.Rows(grdDB.CurrentRow.Index). _
        Item("Title") = fic.Title

        dt.Rows(grdDB.CurrentRow.Index). _
        Item("Author") = fic.Author

        dt.Rows(grdDB.CurrentRow.Index). _
        Item("Description") = fic.Summary

        dt.Rows(grdDB.CurrentRow.Index). _
        Item("Internet") = fic.Author & "#" & fic.AuthorURL & "#"

        dt.Rows(grdDB.CurrentRow.Index). _
        Item("StoryId") = id

        dt.Rows(grdDB.CurrentRow.Index). _
        Item("Complete") = False
        dt.Rows(grdDB.CurrentRow.Index). _
        Item("Abandoned") = False

        dte = fic.PublishDate

        dt.Rows(grdDB.CurrentRow.Index). _
        Item("Publish_Date") = dte

        dt.Rows(grdDB.CurrentRow.Index). _
        Item("Last_Checked") = dte

        dt.Rows(grdDB.CurrentRow.Index). _
        Item("Category_Id") = CInt(cmbChooseDB.SelectedValue)

    End Sub

    Sub UpdateAtom(ByVal url As String)
        ' update urlAtom with AuthorPage
        frmMain.urlAtom.Text = url
        ' Obtain Feed From Site
        frmMain.ObtainFeed(url)
    End Sub

   

    Sub ResetInfo()
        'Clear Information from source
        frmMain.ListChapters.Items.Clear()
        frmMain.lblChapterCount.Text = ""
        frmMain.lblProgress.Text = ""
        frmMain.lblStart.Visible = False
        frmMain.txtStart.Text = "1"
        frmMain.txtStart.Visible = False
        frmMain.txtSource.Text = ""
    End Sub

#End Region

    Private Sub btnCategory_Click( _
                                   sender As System.Object, _
                                   e As System.EventArgs _
                                 ) Handles btnCategory.Click

        Dim frmCat As New frmCategory
        frmCat.ShowDialog()

    End Sub

    
End Class
