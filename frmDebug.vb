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
    Friend WithEvents grdDB As System.Windows.Forms.DataGrid
    Friend WithEvents btnUpdateDB As System.Windows.Forms.Button
    Friend WithEvents btnUpdateStoryID As System.Windows.Forms.Button
    Friend WithEvents cmbSearch As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBatch As System.Windows.Forms.Button
    Friend WithEvents btnOpenDB As System.Windows.Forms.Button
    Friend WithEvents btnPath As System.Windows.Forms.Button
    Friend WithEvents tmrDownload As System.Windows.Forms.Timer
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cmbChooseDB As System.Windows.Forms.ComboBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.grdRSS = New System.Windows.Forms.DataGrid
        Me.grdDB = New System.Windows.Forms.DataGrid
        Me.btnUpdateDB = New System.Windows.Forms.Button
        Me.btnUpdateStoryID = New System.Windows.Forms.Button
        Me.cmbSearch = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnBatch = New System.Windows.Forms.Button
        Me.btnOpenDB = New System.Windows.Forms.Button
        Me.btnPath = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.tmrDownload = New System.Windows.Forms.Timer(Me.components)
        Me.btnSearch = New System.Windows.Forms.Button
        Me.cmbChooseDB = New System.Windows.Forms.ComboBox
        Me.lblStatus = New System.Windows.Forms.Label
        CType(Me.grdRSS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdDB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdRSS
        '
        Me.grdRSS.DataMember = ""
        Me.grdRSS.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdRSS.Location = New System.Drawing.Point(7, 7)
        Me.grdRSS.Name = "grdRSS"
        Me.grdRSS.Size = New System.Drawing.Size(712, 127)
        Me.grdRSS.TabIndex = 27
        '
        'grdDB
        '
        Me.grdDB.DataMember = ""
        Me.grdDB.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdDB.Location = New System.Drawing.Point(7, 140)
        Me.grdDB.Name = "grdDB"
        Me.grdDB.Size = New System.Drawing.Size(712, 176)
        Me.grdDB.TabIndex = 29
        '
        'btnUpdateDB
        '
        Me.btnUpdateDB.Enabled = False
        Me.btnUpdateDB.Location = New System.Drawing.Point(619, 318)
        Me.btnUpdateDB.Name = "btnUpdateDB"
        Me.btnUpdateDB.Size = New System.Drawing.Size(100, 21)
        Me.btnUpdateDB.TabIndex = 31
        Me.btnUpdateDB.Text = "Update Database"
        '
        'btnUpdateStoryID
        '
        Me.btnUpdateStoryID.Enabled = False
        Me.btnUpdateStoryID.Location = New System.Drawing.Point(619, 344)
        Me.btnUpdateStoryID.Name = "btnUpdateStoryID"
        Me.btnUpdateStoryID.Size = New System.Drawing.Size(100, 21)
        Me.btnUpdateStoryID.TabIndex = 32
        Me.btnUpdateStoryID.Text = "Update StoryID"
        '
        'cmbSearch
        '
        Me.cmbSearch.Items.AddRange(New Object() {"Title", "Author", "Folder"})
        Me.cmbSearch.Location = New System.Drawing.Point(355, 344)
        Me.cmbSearch.Name = "cmbSearch"
        Me.cmbSearch.Size = New System.Drawing.Size(84, 21)
        Me.cmbSearch.TabIndex = 33
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(352, 322)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 14)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "Search By"
        '
        'btnBatch
        '
        Me.btnBatch.Enabled = False
        Me.btnBatch.Location = New System.Drawing.Point(445, 344)
        Me.btnBatch.Name = "btnBatch"
        Me.btnBatch.Size = New System.Drawing.Size(168, 21)
        Me.btnBatch.TabIndex = 35
        Me.btnBatch.Text = "Batch Download Updates"
        '
        'btnOpenDB
        '
        Me.btnOpenDB.Location = New System.Drawing.Point(61, 343)
        Me.btnOpenDB.Name = "btnOpenDB"
        Me.btnOpenDB.Size = New System.Drawing.Size(139, 21)
        Me.btnOpenDB.TabIndex = 38
        Me.btnOpenDB.Text = "Open DB"
        '
        'btnPath
        '
        Me.btnPath.Location = New System.Drawing.Point(7, 323)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(48, 40)
        Me.btnPath.TabIndex = 39
        Me.btnPath.Text = "Set Paths"
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(206, 325)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(72, 40)
        Me.btnSave.TabIndex = 40
        Me.btnSave.Text = "Save Story to DB"
        '
        'tmrDownload
        '
        Me.tmrDownload.Interval = 1000
        '
        'btnSearch
        '
        Me.btnSearch.Enabled = False
        Me.btnSearch.Location = New System.Drawing.Point(284, 325)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(62, 40)
        Me.btnSearch.TabIndex = 41
        Me.btnSearch.Text = "Search DB"
        '
        'cmbChooseDB
        '
        Me.cmbChooseDB.FormattingEnabled = True
        Me.cmbChooseDB.Location = New System.Drawing.Point(61, 323)
        Me.cmbChooseDB.Name = "cmbChooseDB"
        Me.cmbChooseDB.Size = New System.Drawing.Size(139, 21)
        Me.cmbChooseDB.TabIndex = 42
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(442, 324)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(25, 13)
        Me.lblStatus.TabIndex = 43
        Me.lblStatus.Text = "DB:"
        '
        'Debug
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(726, 376)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.cmbChooseDB)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnPath)
        Me.Controls.Add(Me.btnOpenDB)
        Me.Controls.Add(Me.btnBatch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbSearch)
        Me.Controls.Add(Me.btnUpdateStoryID)
        Me.Controls.Add(Me.btnUpdateDB)
        Me.Controls.Add(Me.grdDB)
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

    Public myCaller As HtmlGrabber
    Public Navigate As Process

#Region "Database Routines"

    Public DB As String

    Function GetData( _
                      ByVal Category_ID As Integer, _
                      Optional ByVal ALL As Boolean = False _
                    ) As DataTable

        Dim dt As DataTable
        Dim taFF As New dsFanFicTableAdapters.FanficTableAdapter

        DB = cmbChooseDB.Text

        Try
            If ALL Then
                dt = taFF.GetDataByCat(Category_ID)
            Else
                dt = taFF.GetDataByStatus(False, False, Category_ID)
            End If

        Catch
            dt = Nothing
        Finally
            taFF.Dispose()
        End Try

        GetData = dt

        dt.Dispose()

    End Function

    Private Function UpdateData(ByVal dt As DataTable) As Integer

        Dim result As Integer = 0

        Dim taFF As New dsFanFicTableAdapters.FanficTableAdapter

        Try
            result = taFF.Update(dt)
        Catch
            result = -1
        Finally
            taFF.Dispose()
        End Try

        Return result

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
    
    Private Function GetCategories() As DataTable
    	
    	Dim dt As DataTable
    	
    	Dim taCat As New dsFanFicTableAdapters.CategoryTableAdapter

		dt = taCat.GetData()

		Return dt
    	
    End Function

    Private Sub UpdateDateBase( _
                                  ByVal sender As System.Object, _
                                  ByVal e As System.EventArgs _
                              ) Handles btnUpdateDB.Click
        Dim dt As DataTable

        dt = grdDB.DataSource

        UpdateData(dt)

    End Sub

#End Region

#Region "Interface Code"



    Private Sub SetStoryID( _
                            ByVal sender As System.Object, _
                            ByVal e As System.EventArgs _
                          ) Handles btnUpdateStoryID.Click
        StoryID(True)
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
        
        Dim conf As New clsConfig

        Dim val As String

        fi = New FileInfo(Application.StartupPath & "\\" & "config.ini")

        If fi.Exists Then
            ifr = New IniFileReader(Application.StartupPath & "\config.ini", True)

            val = ifr.GetIniValue("FanFic", "Path")

            If val <> empty Then
                conf.UpdateConnStr("FanFic", val)
            End If

        End If

        ifr = Nothing
        conf = Nothing

        My.Settings.Reload()

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

    Private Sub FillCategories()

        Dim dt As DataTable

		dt = GetCategories()

        cmbChooseDB.DataSource = dt
        cmbChooseDB.ValueMember = "Index"
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

        publish_date = CDate(dt.Rows(pos).Item("Publish_Date").ToString)

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
        Else
            dt.Rows(pos).Item("Last_Checked") = CDate(Date.Today)
            UpdateData(dt)
        End If

        Return ret

    End Function

    Function isValid( _
                      ByRef dt As DataTable, _
                      ByVal pos As Long _
                    ) As Boolean

        Dim ret As Boolean = False
        Dim link As String

        If dt.Rows(grdDB.CurrentRowIndex). _
           Item("Internet").GetType Is GetType(DBNull) _
        Then
            ret = False
        Else

            link = dt.Rows(grdDB.CurrentRowIndex).Item("Internet")
            link = Replace(link, "#", "")

            ret = myCaller.CheckUrl(link)

        End If

        Return ret

    End Function

#End Region


    Sub ProcessStory( _
                      ByVal dt As DataTable, _
                      ByVal pos As Long _
                    )

        frmMain.lblTitle.Text = dt.Rows(pos). _
                                Item("Title")

        frmMain.txtStart.Text = dt.Rows(pos). _
                                Item("Count") + 1

        frmMain.txtFileMask.Text = dt.Rows(pos). _
                                   Item("Folder") & "-"

        If InStr(LCase(frmMain.txtSource.Text), frmMain.cls.ErrorMessage) <> 0 Then
            frmMain.lblChapterCount.Text = _
                 CInt(dt.Rows(pos).Item("Count")) + 1
            frmMain.DownloadData()
            Exit Sub
        End If

        If dt.Rows(pos).Item("Publish_Date").ToString = "" Then
            dt.Rows(pos).Item("Publish_Date") = CDate(frmMain.lblPublish.Text)
            UpdateData(dt)
        End If

        If frmMain.lblChapterCount.Text > _
           dt.Rows(pos).Item("Count") _
        Then

            dt.Rows(pos).Item("Count") = CInt(frmMain.lblChapterCount.Text)

            If frmMain.lblUpdate.Text = "" Then
                frmMain.lblUpdate.Text = frmMain.lblPublish.Text
            End If

            If dt.Rows(pos).Item("Publish_Date").ToString = "" Then
                dt.Rows(pos).Item("Update_Date") = CDate(frmMain.lblPublish.Text)
            End If

            dt.Rows(pos).Item("Update_Date") = CDate(frmMain.lblUpdate.Text)

            frmMain.DownloadData()

            UpdateData(dt)

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

        If initial Then
            grdDB.CurrentRowIndex = 0
        End If

bypass:

        pos = grdDB.CurrentRowIndex

        If (pos <= (dt.Rows.Count - 1)) Then
            If initial = False Then
                grdDB.CurrentRowIndex = grdDB.CurrentRowIndex + 1
                If pos = grdDB.CurrentRowIndex Then
                    Return -1
                End If
                pos = grdDB.CurrentRowIndex
                If pos = 0 Then Return -1
            End If
        Else
            Return -1
        End If

        abort = False


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

        If abort Then
            initial = False
            GoTo bypass
        Else
            Select Case Navigate
                Case Process.AuthorPage
                    UpdateAtom(url)
                Case Process.StoryPage
                    StoryID(False)
                    UpdateURL(url)
                    Application.DoEvents()
                    ProcessStory(dt, pos)
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
        btnUpdateStoryID.Enabled = True

        grdDB.DataSource = dt
        grdDB.CurrentRowIndex = 0

        If dt.Rows.Count > 0 Then

            lblStatus.Text = "(" & _
                             (grdDB.CurrentRowIndex + 1) & _
                             " of " & dt.Rows.Count & ")"
        Else
            lblStatus.Text = "(0 of 0)"
        End If

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
        Dim dt As DataTable

        dt = grdDB.DataSource

        Dim NewRow As Integer = dt.Rows.Count

        Dim iYesNo As Integer

        Dim link As String

        iYesNo = MsgBox("Add New Record?", MsgBoxStyle.YesNo)

        If iYesNo = vbYes Then
            Dim dr As DataRow = dt.NewRow

            If myCaller.urlAtom.Text = "" Then
                link = myCaller.txtUrl.Text
            Else
                link = myCaller.urlAtom.Text
            End If

            dr("Title") = myCaller.lblTitle.Text
            dr("Author") = myCaller.lblAuthor.Text
            dr("Folder") = InputBox("Enter File Name") 'Folder Name
            'dr("Chapter") = "" 'Current Chapter
            dr("Count") = 0 'Chapter Count
            'dr("Matchup") = "" ' Matchup
            dr("Description") = Trim(myCaller.txtSource.Text)
            dr("Internet") = myCaller.lblAuthor.Text & _
                             "#" & myCaller.cls.GetAuthorURL(link) & "#"
            dr("StoryId") = myCaller.cls.GetStoryID(myCaller.txtUrl.Text)
            dr("Complete") = False
            dr("Publish_Date") = CDate( _
                                        Replace( _
                                                 frmMain.lblPublish.Text, _
                                                 "Published:", _
                                                 "" _
                                               ) _
                                      )

            dr("Category_Index") = CInt(cmbChooseDB.SelectedValue)

            dt.Rows.Add(dr)

            grdDB.DataSource = dt
            grdDB.CurrentRowIndex = NewRow

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

        start = grdDB.CurrentRowIndex

        Dim dt As DataTable
        dt = grdDB.DataSource

        If start < (dt.Rows.Count - 1) Then
            start = start + 1
        Else
            start = 0
        End If

        For count = start To (dt.Rows.Count - 1)

            grdDB.CurrentRowIndex = count
            Application.DoEvents()

            folder = grdDB.Item(grdDB.CurrentRowIndex, 2).ToString
            title = grdDB.Item(grdDB.CurrentRowIndex, 0).ToString
            author = grdDB.Item(grdDB.CurrentRowIndex, 1).ToString

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

                grdDB.CurrentRowIndex = count
                Application.DoEvents()

                title = grdDB.Item(grdDB.CurrentRowIndex, 0).ToString
                author = grdDB.Item(grdDB.CurrentRowIndex, 1).ToString
                folder = grdDB.Item(grdDB.CurrentRowIndex, 2).ToString

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
            grdDB.CurrentRowIndex = count
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

        Dim grdDB As DataGrid = CType(sender, DataGrid)
        Dim dt As DataTable

        dt = grdDB.DataSource

        If dt.Rows.Count > 0 Then
            lblStatus.Text = "(" & _
                             (grdDB.CurrentRowIndex + 1) & _
                             " of " & dt.Rows.Count & ")"
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

        If dt.Rows(grdDB.CurrentRowIndex). _
           Item("StoryId").GetType Is GetType(DBNull) _
        Then
            link = ""
        Else

            StoryID = dt.Rows(grdDB.CurrentRowIndex). _
                      Item("StoryID")

            If StoryID = "" Then
                link = ""
            Else
                link = myCaller.cls.GetStoryURL(StoryID)
            End If

        End If

        Return link

    End Function

    Function GetAtom( _
                         ByVal dt As DataTable _
                       ) As String

        If dt.Rows(grdDB.CurrentRowIndex). _
           Item("Internet").GetType Is GetType(DBNull) _
        Then
            GetAtom = ""
        Else
            GetAtom = Replace( _
                               dt.Rows(grdDB.CurrentRowIndex). _
                               Item("Internet") _
                               , _
                               "#", _
                               "" _
                             )
        End If

    End Function

#End Region

#Region "Utility Routines"

    Private Sub StoryID(Optional ByVal Update As Boolean = True)

        Dim dt As DataTable
        dt = grdDB.DataSource

        If Update Then

            dt.Rows(grdDB.CurrentRowIndex). _
            Item("StoryID") _
            = _
            frmMain.lblStoryID.Text

        Else

            frmMain.lblStoryID.Text = _
            dt.Rows(grdDB.CurrentRowIndex). _
            Item("StoryID")
        End If

    End Sub

    Sub UpdateAtom(ByVal url As String)
        ' update urlAtom with AuthorPage
        frmMain.urlAtom.Text = url
        ' Obtain Feed From Site
        frmMain.ObtainFeed(url)
    End Sub

    Sub UpdateURL(ByVal url As String)
        'Update txtUrl with story Page
        frmMain.txtUrl.Text = url
        'Reset frmMain
        ResetInfo()
        'Update btnURL Caption for Correct Processing
        frmMain.btnURL.Text = "Get Chapters"
        'Obtain Story Info From Story Page
        frmMain.DownloadData()
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

End Class
