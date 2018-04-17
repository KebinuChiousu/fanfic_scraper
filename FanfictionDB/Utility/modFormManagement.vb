Module FormManagement

    Public frmDebug As Debug
    Public frmMain As HtmlGrabber
    Public frmHtml As Html
    Public frmStory As Story

    Public Enum forms
        frmMain = 0
        frmDebug = 1
        frmHtml = 2
        frmStory = 3
    End Enum

    Sub Initialize( _
                    ByVal cls As forms _
                  )

        Dim frm As Object

        frm = Nothing

        Select Case cls
            Case forms.frmDebug
                frm = frmDebug
            Case forms.frmHtml
                frm = frmHtml
            Case forms.frmStory
                frm = frmStory

        End Select

        If Not IsNothing(frm) Then
            If frm.IsDisposed Then
                CreateForm(cls)
            End If
        Else
            CreateForm(cls)
        End If

    End Sub

    Sub CreateForm( _
                    ByVal cls As forms _
                  )

        Select Case cls

            Case forms.frmMain
                ' #################################
                ' # Instance of Class HtmlGrabber #
                ' # Default instance created by   #
                ' # .Net Framework because        #
                ' # screen is loaded at start up  #
                ' #################################
            Case forms.frmDebug
                'Create an Instance of class frmDebug
                frmDebug = New Debug
            Case forms.frmHtml
                'Create an Instance of class frmHtml
                frmHtml = New Html
            Case forms.frmStory
                frmStory = New Story
                PlaceStoryWindow()
        End Select

    End Sub

    Sub PlaceDebugWindow()
        frmDebug.Top = frmMain.Top + 168 + 35
        frmDebug.Left = frmMain.Left + 28
    End Sub

    Sub PlaceHtmlWindow()
        frmHtml.Top = frmMain.Top + 168 + 35
        frmHtml.Left = frmMain.Left + 28
    End Sub

    Sub PlaceStoryWindow()

        frmStory.Top = frmDebug.Top - frmDebug.grdRSS.Height
        frmStory.Left = frmDebug.Left

    End Sub



End Module
