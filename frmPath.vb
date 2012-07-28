Imports System.IO
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Text

Public Class frmPath
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents btnPath As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnPath = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Tag = "FanFic"
        Me.Label1.Text = "FanFic Database Path"
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(8, 24)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(336, 20)
        Me.txtPath.TabIndex = 1
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(125, 50)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(136, 32)
        Me.btnUpdate.TabIndex = 4
        Me.btnUpdate.Text = "Update INI File"
        '
        'btnPath
        '
        Me.btnPath.Location = New System.Drawing.Point(352, 24)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(24, 20)
        Me.btnPath.TabIndex = 5
        Me.btnPath.Text = "..."
        '
        'frmPath
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(384, 89)
        Me.Controls.Add(Me.btnPath)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmPath"
        Me.Text = "Path to Databases"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub btnBrowse_Click( _
                                 ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs _
                                ) Handles btnPath.Click
        Dim dlg As OpenFileDialog = New OpenFileDialog
        dlg.DefaultExt = "mdb"
        dlg.Filter = "Access Database|*.mdb"
        dlg.CheckFileExists = True
        dlg.CheckPathExists = True
        dlg.Title = "Select Path to Database."
        Try
            If dlg.ShowDialog() = DialogResult.OK Then
                txtPath.Text = dlg.FileName
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub frmPath_Load( _
                              ByVal sender As Object, _
                              ByVal e As System.EventArgs _
                            ) Handles MyBase.Load
        'Legacy Ini Code
        InitIniFile()

        'App.Config Code
        InitConfigFile()

    End Sub

    Private Sub btnUpdate_Click( _
                                 ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs _
                               ) Handles btnUpdate.Click

        'App.Config Code
        UpdateConfigFile()

        'Legacy Ini Code
        UpdateIniFile()

        My.Settings.Reload()

        Me.Dispose()

    End Sub

#Region "config.ini Manipulation Routines"

    Dim ifr As IniFileReader

    Sub InitIniFile()

        Dim fi As FileInfo
        Dim sc As StringCollection
        fi = New FileInfo(Application.StartupPath & "\\" & "config.ini")
        If fi.Exists Then
            ifr = New IniFileReader(Application.StartupPath & "\config.ini", True)
            txtPath.Text = ifr.GetIniValue("FanFic", "Path")
        Else
            ifr = New IniFileReader(Application.StartupPath & "\config.ini", True)
            sc = ifr.GetIniComments(Nothing)
            sc.Add("Fanfiction Downloader DB Configuration")
            ifr.SetIniComments(Nothing, sc)
            ifr.SetIniValue("FanFic", "Path", "No Path Set")
            ifr.OutputFilename = Application.StartupPath & "\config.ini"
            ifr.SaveAsIniFile()
        End If

    End Sub

    Sub UpdateIniFile()

        If txtPath.Text = "" Then
            ifr.SetIniValue("FanFic", "Path", "No Path Set")
        Else
            ifr.SetIniValue("FanFic", "Path", txtPath.Text)
        End If

        ifr.OutputFilename = Application.StartupPath & "\config.ini"
        ifr.SaveAsIniFile()

    End Sub

#End Region

#Region "app.config Manipulation Routines"

    Dim conf As clsConfig

    Sub InitConfigFile()

        conf = New clsConfig

        txtPath.Text = conf.GetConnStr("FanFic")

    End Sub

    Sub UpdateConfigFile()

        If txtPath.Text <> "" Then
            conf.UpdateConnStr("FanFic", txtPath.Text)
        End If

    End Sub

#End Region

End Class

