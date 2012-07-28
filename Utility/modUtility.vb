Imports System.Reflection
Imports System.Reflection.Assembly
Imports System.Diagnostics
Imports System.IO

Module modUtility

    Public Function EmbeddedObj(ByVal Name As String) As Stream

        Dim assy As Assembly

        Dim obj As Stream

        Dim str As String = ""

        assy = GetExecutingAssembly()
        Dim resources() As String

        resources = assy.GetManifestResourceNames()

        For Each resourceName As String In resources
            If InStr(resourceName, Name) <> 0 Then
                str = resourceName
                Exit For
            End If
        Next

        obj = assy.GetManifestResourceStream(str)

        Return obj

    End Function

    Sub GetEmbeddedFile(ByVal filename As String)

        Dim UMS As UnmanagedMemoryStream
        Dim outfile As Stream
        Const sz As Integer = 4096
        Dim buf As Byte()
        Dim nRead As Integer

        ReDim buf(sz)

        UMS = EmbeddedObj(filename)

        File.Delete(filename)
        outfile = File.Create(filename)

        While True
            nRead = UMS.Read(buf, 0, sz)
            If nRead < 1 Then
                Exit While
            End If
            outfile.Write(buf, 0, nRead)
        End While

        outfile.Close()


    End Sub

    Function GetTempFileName() As String

        Return System.IO.Path.GetTempFileName

    End Function

    Function runAndWait( _
                         ByVal command As String, _
                         ByVal commandLine As String _
                       ) As Boolean

        Dim runProcess As Process
        Dim path As String

        path = My.Application.Info.DirectoryPath

        Try
            runProcess = New Process



            With runProcess.StartInfo
                .FileName = path & "\" & command
                .Arguments = commandLine
                '.WindowStyle = ProcessWindowStyle.Hidden
                .WindowStyle = ProcessWindowStyle.Minimized
            End With

            runProcess.Start()

            'Wait until the process passes back an exit code 
            runProcess.WaitForExit()

            'Free resources associated with this proces
            runProcess.Close()
            runAndWait = True
        Catch ex As Exception
            runAndWait = False
        End Try
    End Function

    Function StringToStream(ByVal data As String) As Stream

        Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(data)
        Dim ms As MemoryStream = New MemoryStream(bytes)

        Return CType(ms, Stream)

    End Function

End Module
