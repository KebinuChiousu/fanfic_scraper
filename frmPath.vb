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
    Friend WithEvents txtHP As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtRanma As System.Windows.Forms.TextBox
    Friend WithEvents btnHP As System.Windows.Forms.Button
    Friend WithEvents btnRanma As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtHP = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtRanma = New System.Windows.Forms.TextBox
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnHP = New System.Windows.Forms.Button
        Me.btnRanma = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Harry Potter"
        '
        'txtHP
        '
        Me.txtHP.Location = New System.Drawing.Point(8, 24)
        Me.txtHP.Name = "txtHP"
        Me.txtHP.Size = New System.Drawing.Size(336, 20)
        Me.txtHP.TabIndex = 1
        Me.txtHP.Text = ""
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Ranma"
        '
        'txtRanma
        '
        Me.txtRanma.Location = New System.Drawing.Point(8, 64)
        Me.txtRanma.Name = "txtRanma"
        Me.txtRanma.Size = New System.Drawing.Size(336, 20)
        Me.txtRanma.TabIndex = 3
        Me.txtRanma.Text = ""
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(124, 120)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(136, 32)
        Me.btnUpdate.TabIndex = 4
        Me.btnUpdate.Text = "Update INI File"
        '
        'btnHP
        '
        Me.btnHP.Location = New System.Drawing.Point(352, 24)
        Me.btnHP.Name = "btnHP"
        Me.btnHP.Size = New System.Drawing.Size(24, 20)
        Me.btnHP.TabIndex = 5
        Me.btnHP.Text = "..."
        '
        'btnRanma
        '
        Me.btnRanma.Location = New System.Drawing.Point(352, 64)
        Me.btnRanma.Name = "btnRanma"
        Me.btnRanma.Size = New System.Drawing.Size(24, 20)
        Me.btnRanma.TabIndex = 6
        Me.btnRanma.Text = "..."
        '
        'frmPath
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(384, 173)
        Me.Controls.Add(Me.btnRanma)
        Me.Controls.Add(Me.btnHP)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.txtRanma)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtHP)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmPath"
        Me.Text = "Path to Databases"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnBrowse_Click( _
                                 ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs _
                                ) Handles _
                                  btnHP.Click, _
                                  btnRanma.Click
        Dim dlg As OpenFileDialog = New OpenFileDialog
        dlg.DefaultExt = "mdb"
        dlg.Filter = "Access Database|*.mdb"
        dlg.CheckFileExists = True
        dlg.CheckPathExists = True
        dlg.Title = "Select Path to Database."
        Try
            If dlg.ShowDialog() = DialogResult.OK Then
                Select Case CType(sender, Button).Name
                    Case "btnHP"
                        txtHP.Text = dlg.FileName
                    Case "btnRanma"
                        txtRanma.Text = dlg.FileName
                End Select
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
            txtHP.Text = ifr.GetIniValue("HP", "Path")
            txtRanma.Text = ifr.GetIniValue("Ranma", "Path")
        Else
            ifr = New IniFileReader(Application.StartupPath & "\config.ini", True)
            sc = ifr.GetIniComments(Nothing)
            sc.Add("Fanfiction Downloader DB Configuration")
            ifr.SetIniComments(Nothing, sc)
            ifr.SetIniValue("HP", "Path", "No Path Set")
            ifr.SetIniValue("Ranma", "Path", "No Path Set")
            ifr.OutputFilename = Application.StartupPath & "\config.ini"
            ifr.SaveAsIniFile()
        End If

    End Sub

    Sub UpdateIniFile()

        If txtHP.Text = "" Then
            ifr.SetIniValue("HP", "Path", "No Path Set")
        Else
            ifr.SetIniValue("HP", "Path", txtHP.Text)
        End If

        If txtRanma.Text = "" Then
            ifr.SetIniValue("Ranma", "Path", "No Path Set")
        Else
            ifr.SetIniValue("Ranma", "Path", txtRanma.Text)
        End If

        ifr.OutputFilename = Application.StartupPath & "\config.ini"
        ifr.SaveAsIniFile()

    End Sub

#End Region

#Region "app.config Manipulation Routines"

    Dim conf As clsConfig

    Sub InitConfigFile()

        conf = New clsConfig

        txtHP.Text = conf.GetConnStr("HP")
        txtRanma.Text = conf.GetConnStr("Ranma")

    End Sub

    Sub UpdateConfigFile()

        If txtHP.Text <> "" Then
            conf.UpdateConnStr("HP", txtHP.Text)
        End If

        If txtRanma.Text <> "" Then
            conf.UpdateConnStr("Ranma", txtRanma.Text)
        End If

    End Sub

#End Region

End Class

