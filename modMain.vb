Imports System.IO

Module modMain

    Public cls As clsFanfic
    Public BL As New clsBL

    Sub InitIniFile()

        Dim Desktop As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

        Dim fi As FileInfo

        Dim ifr As IniFileReader

        Dim val As String = ""

        fi = New FileInfo(Application.StartupPath & "\\" & "config.ini")

        If fi.Exists Then
            ifr = New IniFileReader(Application.StartupPath & "\config.ini", True)

            val = ifr.GetIniValue("Output", "Path")

        End If

        If val = "" Then
            val = Desktop
        End If

        BL.OutputPath = val

    End Sub

    Sub Main()

        Dim frmMain As New HtmlGrabber

        InitIniFile()

        frmMain.ShowDialog()

    End Sub


End Module
