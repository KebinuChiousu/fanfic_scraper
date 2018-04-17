Option Explicit On 

Structure URL
    Dim Scheme As String
    Dim Host As String
    Dim Port As Long
    Dim URI As String
    Dim Query As QueryString()
End Structure

Structure QueryString
    Dim Name As String
    Dim Value As String
End Structure

Module URLHelper

    ' returns as type URL from a string
    Function ExtractUrl(ByVal strUrl As String) As URL
        Dim intPos1 As Integer
        Dim intPos2 As Integer

        Dim retURL As New URL

        '1 look for a scheme it ends with ://
        intPos1 = InStr(strUrl, "://")

        If intPos1 > 0 Then
            retURL.Scheme = Mid(strUrl, 1, intPos1 - 1)
            strUrl = Mid(strUrl, intPos1 + 3)
        End If

        '2 look for a port
        intPos1 = InStr(strUrl, ":")
        intPos2 = InStr(strUrl, "/")

        If intPos1 > 0 And intPos1 < intPos2 Then
            ' a port is specified
            retURL.Host = Mid(strUrl, 1, intPos1 - 1)

            If (IsNumeric(Mid(strUrl, intPos1 + 1, intPos2 - intPos1 - 1))) Then
                retURL.Port = CInt(Mid(strUrl, intPos1 + 1, intPos2 - intPos1 - 1))
            End If
        ElseIf intPos2 > 0 Then
            retURL.Host = Mid(strUrl, 1, intPos2 - 1)
        Else
            retURL.Host = strUrl
            retURL.URI = "/"

            ExtractUrl = retURL
            Exit Function
        End If

        strUrl = Mid(strUrl, intPos2)

        ' find a question mark ?
        intPos1 = InStr(strUrl, "?")

        If intPos1 > 0 Then
            retURL.URI = Mid(strUrl, 1, intPos1 - 1)
            retURL.Query = SplitParms(Mid(strUrl, intPos1 + 1))
        Else
            retURL.URI = strUrl
        End If

        ExtractUrl = retURL
    End Function

    Function SplitParms(ByVal Query As String) As QueryString()

        Dim idx As Integer
        Dim parms As String()
        Dim value As String()
        Dim ret As QueryString()

        parms = Split(Query, "&")

        ReDim ret(UBound(parms))

        For idx = 0 To UBound(parms)

            value = Split(parms(idx), "=")

            ret(idx).Name = value(0)
            ret(idx).Value = value(1)

        Next

        Return ret

    End Function

    ' url encodes a string
    Function URLEncode(ByVal str As String) As String
        Dim intLen As Integer
        Dim X As Integer
        Dim curChar As Long
        Dim newStr As String
        intLen = Len(Str)
        newStr = ""
        For X = 1 To intLen
            curChar = Asc(Mid$(Str, X, 1))

            If (curChar < 48 Or curChar > 57) And _
                (curChar < 65 Or curChar > 90) And _
                (curChar < 97 Or curChar > 122) Then
                newStr = newStr & "%" & Hex(curChar)
            Else
                newStr = newStr & Chr(curChar)
            End If
        Next X

        URLEncode = newStr
    End Function

    ' decodes a url encoded string
    Function UrlDecode(ByVal str As String) As String
        Dim intLen As Integer
        Dim X As Integer
        Dim curChar As String
        Dim strCode As String

        Dim newStr As String

        intLen = Len(str)
        newStr = ""

        For X = 1 To intLen
            curChar = Mid$(str, X, 1)

            If curChar = "%" Then
                strCode = "&h" & Mid$(str, X + 1, 2)

                If IsNumeric(strCode) Then
                    curChar = Chr(Int(strCode))
                Else
                    curChar = ""
                End If
                X = X + 2
            End If

            newStr = newStr & curChar
        Next X

        UrlDecode = newStr
    End Function

End Module
