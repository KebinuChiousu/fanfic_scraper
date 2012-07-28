Module FormManagement

    Public frmDebug As Debug
    Public frmMain As HtmlGrabber

    Public Enum forms
        frmMain = 0
        frmDebug = 1
    End Enum

    Sub Initialize( _
                    ByRef frm As System.Windows.Forms.Form, _
                    ByVal cls As forms _
                  )

        If Not IsNothing(frm) Then
            If frm.IsDisposed Then
                frm = CreateForm(cls)
            End If
        Else
            frm = CreateForm(cls)
        End If

    End Sub

    Function CreateForm( _
                         ByVal cls As forms _
                       ) As Form

        CreateForm = Nothing

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

                CreateForm = New Debug



        End Select

    End Function

    Sub PlaceDebugWindow()
        frmDebug.Top = frmMain.Top + 168 + 35
        frmDebug.Left = frmMain.Left + 28
    End Sub

End Module
