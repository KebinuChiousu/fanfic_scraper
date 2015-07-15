Imports System.Reflection
Imports System.Reflection.Assembly
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports System.Text.Encoding
Imports System.Text.RegularExpressions

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

    Function FormatLineEndings(ByVal str As String) As String
        ' this function converts all line endings to Windows CrLf line endings
        Dim prevChar As String
        Dim nextChar As String
        Dim curChar As String

        Dim strRet As String

        Dim X As Long

        prevChar = ""
        nextChar = ""
        curChar = ""
        strRet = ""

        For X = 1 To Len(str)
            prevChar = curChar
            curChar = Mid$(str, X, 1)

            If nextChar <> vbNullString And curChar <> nextChar Then
                curChar = curChar & nextChar
                nextChar = ""
            ElseIf curChar = vbLf Then
                If prevChar <> vbCr Then
                    curChar = vbCrLf
                End If

                nextChar = ""
            ElseIf curChar = vbCr Then
                nextChar = vbLf
            End If

            strRet = strRet & curChar
        Next X

        FormatLineEndings = strRet
    End Function

    Function LastPos(ByVal source As String, ByVal search As String) As Integer

        Dim idx As Integer = 1
        Dim last_idx As Integer = 0

        Do Until idx = 0
            idx = InStr(last_idx + 1, source, search)
            If idx > 0 Then
                last_idx = idx
            End If
        Loop

        Return last_idx

    End Function

    Function ConvertToAscii(ByVal str As String) As String

        Dim ecp1252 As Encoding = Encoding.GetEncoding(1252)

        Dim ascii As ASCIIEncoding = New ASCIIEncoding
        Dim bytearray As Byte()
        Dim asciiarray As Byte()
        Dim ret As String

        bytearray = Encoding.UTF8.GetBytes(str)
        asciiarray = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, bytearray)
        ret = ascii.GetString(asciiarray)

        Return ret

    End Function

    Public Function CleanString(ByVal s As String) As String

        s = Regex.Replace(s, "[^A-Za-z0-9]", String.Empty)

        Return s

    End Function

    Sub Increment_FileNumber(ByVal Folder As String)

        Dim base As String = Folder

        Dim strFiles As String() = Directory.GetFiles(base, "*.htm", SearchOption.TopDirectoryOnly)
        Dim filename As String = ""
        Dim newfilename As String = ""
        Dim idx As Integer
        Dim fi As FileInfo

        Dim num As Integer

        If strFiles.Length > 0 Then
            For idx = UBound(strFiles) To 0 Step -1
                filename = strFiles(idx)
                fi = New FileInfo(filename)
                newfilename = Split(fi.Name, ".")(0)

                num = CInt(Mid(newfilename, Len(newfilename) - 1, 2))
                num += 1

                newfilename = Mid(newfilename, 1, Len(newfilename) - 2)
                newfilename += Format(num, "0#")
                newfilename += ".htm"

                fi.MoveTo(base & "\" & newfilename)

            Next
        End If

    End Sub

    Sub Decrement_FileNumber(ByVal Folder As String)

        Dim base As String = Folder

        Dim strFiles As String() = Directory.GetFiles(base, "*.htm", SearchOption.TopDirectoryOnly)
        Dim filename As String = ""
        Dim newfilename As String = ""
        Dim idx As Integer
        Dim fi As FileInfo

        Dim num As Integer

        If strFiles.Length > 0 Then
            For idx = UBound(strFiles) To 0 Step -1
                filename = strFiles(idx)
                fi = New FileInfo(filename)
                newfilename = Split(fi.Name, ".")(0)

                num = CInt(Mid(newfilename, Len(newfilename) - 1, 2))
                num -= 1

                newfilename = Mid(newfilename, 1, Len(newfilename) - 2)
                newfilename += Format(num, "0#")
                newfilename += ".htm"

                fi.MoveTo(base & "\" & newfilename)

            Next
        End If

    End Sub

End Module
