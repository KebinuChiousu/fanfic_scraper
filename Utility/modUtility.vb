Imports System.IO

Module modUtility

    Function StringToStream(ByVal data As String) As Stream

        Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(data)
        Dim ms As MemoryStream = New MemoryStream(bytes)

        Return CType(ms, Stream)

    End Function

End Module
