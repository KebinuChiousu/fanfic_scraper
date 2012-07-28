Imports System.Text
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO

Public Class WinInet
    Implements IDisposable

    ' WinInet constants.
    Private Const INTERNET_ACCESS_TYPE_DIRECT = 1

    ' Member constants.
    Private Const USER_AGENT = "IE"
    Private Const HEADER As String = "Accept: */*" & Cr & Cr
    Private Const CONTEXT As Integer = 0
    Private Const FLAGS As Integer = 0

    ' Member variables.
    Private _handle As IntPtr

    ' Import WinInet.dll functions.
    <DllImport("WinInet.dll", _
    EntryPoint:="InternetOpenA", _
    CharSet:=CharSet.Ansi, ExactSpelling:=True, SetLastError:=True)> _
    Private Shared Function InternetOpen( _
    ByVal agent As String, _
    ByVal accessType As Int32, _
    ByVal proxyName As String, _
    ByVal proxyBypass As String, _
    ByVal flags As Int32) As IntPtr
    End Function

    <DllImport("WinInet.dll", _
    EntryPoint:="InternetOpenUrlA", _
    CharSet:=CharSet.Ansi, ExactSpelling:=True, SetLastError:=True)> _
    Private Shared Function InternetOpenUrl( _
    ByVal session As IntPtr, _
    ByVal url As String, _
    ByVal header As String, _
    ByVal headerLength As Int32, _
    ByVal flags As Int32, _
    ByVal context As Int32) As Int32
    End Function

    <DllImport("WinInet.dll", _
    EntryPoint:="InternetReadFile", _
    CharSet:=CharSet.Auto, SetLastError:=True)> _
    Private Shared Function InternetReadFile( _
    ByVal handle As Int32, _
    <MarshalAs(UnmanagedType.LPArray)> _
    ByVal newBuffer() As Byte, _
    ByVal bufferLength As Int32, _
    ByRef bytesRead As Int32) As Int32
    End Function

    <DllImport("WinInet.dll", _
    EntryPoint:="InternetCloseHandle", _
    CharSet:=CharSet.Ansi, ExactSpelling:=True, SetLastError:=True)> _
    Private Shared Function InternetCloseHandle( _
    ByVal hInternet As Int32) As Int32
    End Function

    Sub New()
        ' Open an internet session.
        _handle = Me.InternetOpen(USER_AGENT, _
                                    INTERNET_ACCESS_TYPE_DIRECT, _
                                    vbNullString, vbNullString, FLAGS)
    End Sub

    ' Read a file from a url.
    Public Function GetHttpFile(ByVal url As String) As Byte()
        Dim bufferLength As Int32 = 1024 ' Size of buffer to read into.
        Dim bufferStream As New MemoryStream ' Stream to read buffer into.
        Dim bufferStreamWriter As New BinaryWriter(bufferStream)

        Dim session As Int32 ' Http request session handle.

        ' Open http request.
        session = Me.InternetOpenUrl(Me._handle, url, _
                                        HEADER, HEADER.Length, _
                                        0, CONTEXT)

        If session <= 0 Then
            ' Open session failed.
            Throw New Net.WebException(Marshal.GetLastWin32Error())
        Else
            Dim newBuffer() As Byte
            Dim bytesRead As Int32
            Dim response As Boolean

            Do ' Read bytes from http stream until all are read.
                ReDim newBuffer(bufferLength - 1)
                response = Me.InternetReadFile(session, newBuffer, _
                                                    bufferLength, bytesRead)
                If Not response Then
                    ' Read failed.
                    Throw New Net.WebException(Marshal.GetLastWin32Error())
                Else
                    ' Write bytes read to response stream.
                    bufferStreamWriter.Write(newBuffer, 0, bytesRead)
                End If
            Loop While response And bytesRead > 0
        End If
        Me.InternetCloseHandle(session)

        ' Get buffer bytes.
        Dim bufferBytes As Byte() = bufferStream.GetBuffer

        ' Close streams.
        bufferStreamWriter.Close()
        bufferStream.Close()

        ' Return return bytes read.
        Return bufferBytes
    End Function

    Private _disposed As Boolean = False

    Public Sub Dispose() Implements System.IDisposable.Dispose
        ' Close the internet session.
        If Me._disposed Then Exit Sub ' Abort if already disposed.
        Me.InternetCloseHandle(_handle.ToInt32)
        Me._disposed = True
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        ' Dispose, only if not done already.
        Me.Dispose()
    End Sub
End Class

