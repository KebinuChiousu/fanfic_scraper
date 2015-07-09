Imports System.IO

Module modMain

    Public cls As clsFanfic
    Public BL As New clsBL
    Public Browser As clsWeb

    Sub ExtractResources()

        Dim path As String = ""

        path = My.Application.Info.DirectoryPath

        If Not File.Exists(path & "\" & "XMLtoINI.xslt") Then
            GetEmbeddedFile("XMLtoINI.xslt")
        End If

        If Not File.Exists(path & "\" & "phantomjs.exe") Then
            GetEmbeddedFile("phantomjs.exe")
        End If

    End Sub

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
        Browser = New clsWeb

        InitIniFile()

        frmMain.ShowDialog()

        Browser.Dispose()

    End Sub


End Module
