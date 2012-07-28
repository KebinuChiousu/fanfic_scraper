Module FormManagement

    Public frmDebug As Debug
    Public frmMain As HtmlGrabber

    Public Enum forms
        frmMain = 0
        frmDebug = 1
    End Enum

    Sub Initialize( _
                    ByVal cls As forms _
                  )

        Dim frm As Object

        frm = Nothing

        Select Case cls
            Case forms.frmDebug
                frm = frmDebug
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

        End Select

    End Sub

    Sub PlaceDebugWindow()
        frmDebug.Top = frmMain.Top + 168 + 35
        frmDebug.Left = frmMain.Left + 28
    End Sub

End Module
