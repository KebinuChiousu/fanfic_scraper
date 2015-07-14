Imports HtmlAgilityPack
Imports System.IO
Imports System.Xml
Imports System.Web.HttpUtility

Module modHTML

    <DebuggerStepThrough()> _
    Function CleanHTML(ByRef html As String) As HtmlAgilityPack.HtmlDocument

        Dim ret As HtmlDocument
        Dim doc As New HtmlDocument

        Dim outputFile As StringWriter

        outputFile = New StringWriter()

        doc.LoadHtml(html)

        doc.OptionFixNestedTags = True
        doc.OptionOutputAsXml = True

        doc.Save(outputFile)

        ret = doc

        html = outputFile.ToString

        outputFile.Close()

        doc = Nothing

        Return ret

    End Function

    <DebuggerStepThrough()> _
    Function FindNodesByValue( _
                               ByVal node As HtmlNode, _
                               ByVal NodeName As String, _
                               ByVal NodeValue As String _
                             ) As HtmlNodeCollection

        Dim ret As HtmlNodeCollection

        ret = node.SelectNodes("//" & NodeName & "[contains(text(), '" & NodeValue & "')]")

        Return ret

    End Function

    <DebuggerStepThrough()> _
    Function FindNodesByAttribute( _
                                   ByVal node As HtmlNode, _
                                   ByVal NodeName As String, _
                                   ByVal Attr As String, _
                                   ByVal AttrValue As String, _
                                   Optional ByVal PartialMatch As Boolean = True _
                                 ) As HtmlNodeCollection

        Dim ret As HtmlNodeCollection


        If PartialMatch Then
            ret = node.SelectNodes("//" & NodeName & "[contains(@" & Attr & ", '" & AttrValue & "')]")
        Else
            ret = node.SelectNodes("//" & NodeName & "[@" & Attr & "='" & AttrValue & "']")
        End If

        Return ret

    End Function

    <DebuggerStepThrough()> _
    Function FindNodesByAttribute( _
                                   ByVal node As HtmlNode, _
                                   ByVal NodeName As String, _
                                   ByVal Attr As String _
                                 ) As HtmlNodeCollection

        Dim ret As HtmlNodeCollection

        ret = node.SelectNodes("//" & NodeName & "[@" & Attr & "]")

        Return ret

    End Function

    <DebuggerStepThrough()> _
    Function FindLinksByHref( _
                             ByVal node As HtmlNode, _
                             ByVal href As String _
                           ) As HtmlNodeCollection

        Dim ret As HtmlNodeCollection

        ret = node.SelectNodes("//a[contains(@href, '" & href & "')]")

        Return ret



    End Function

    <DebuggerStepThrough()> _
    Function GetOptionValues( _
                              ByVal htmlDoc As String, _
                              Optional ByVal param As String = "" _
                            ) As String()

        Dim doc As HtmlDocument

        doc = CleanHTML(htmlDoc)

        Dim result As String = ""
        Dim values() As String
        Dim idx As Integer = 0

        ReDim values(0)

        Dim temp As HtmlNodeCollection

        If param = "" Then
            temp = doc.DocumentNode.SelectNodes("//select")
        Else
            temp = doc.DocumentNode.SelectNodes("//select[@title='" & param & "']")
        End If

        If IsNothing(temp) Then
            Return Nothing
        End If

        If temp.Count = 0 Then
            Return Nothing
        End If

        result = temp(0).InnerHtml

        result = "<select>" & result & "</select>"

        doc = CleanHTML(result)

        Dim count As Integer
        Dim node As HtmlNode

        temp = doc.DocumentNode.SelectNodes("//option")

        With temp

            For count = 0 To (.Count - 1)
                node = .Item(count)

                If node.NextSibling.InnerText <> "" Then
                    ReDim Preserve values(idx)
                    values(idx) = node.NextSibling.InnerText
                    idx = idx + 1
                End If
            Next

        End With

        doc = Nothing

        Return values

    End Function

    <DebuggerStepThrough()> _
    Function GetListNodes( _
                           ByVal node As HtmlNode, _
                           Optional ByVal Attr As String = "", _
                           Optional ByVal AttrValue As String = "", _
                           Optional ByVal PartialMatch As Boolean = True _
                         ) As HtmlNodeCollection

        Dim NodeName As String
        Dim result As String
        Dim ret As HtmlNodeCollection
        Dim temp As HtmlNodeCollection
        Dim doc As HtmlDocument

        If InStr(node.OuterHtml, "ul") > 0 Then
            NodeName = "ul"
        Else
            NodeName = "ol"
        End If

        If Attr = "" Then
            temp = node.SelectNodes("//" & NodeName)
        Else
            If PartialMatch Then
                temp = node.SelectNodes("//" & NodeName & "[contains(@" & Attr & ", '" & AttrValue & "')]")
            Else
                temp = node.SelectNodes("//" & NodeName & "[@" & Attr & "='" & AttrValue & "']")
            End If
        End If

        If IsNothing(temp) Then
            Return Nothing
        End If

        If temp.Count = 0 Then
            Return Nothing
        End If

        result = temp(0).OuterHtml

        doc = CleanHTML(result)

        ret = doc.DocumentNode.SelectNodes("//li")

        doc = Nothing
        temp = Nothing

        Return ret

    End Function

    <DebuggerStepThrough()> _
    Function GetHTMLNodes( _
                           ByVal htmldoc As HtmlDocument, _
                           ByVal xpath As String _
                         ) As HtmlNodeCollection

        Dim XmlList As HtmlNodeCollection

        XmlList = htmldoc.DocumentNode.SelectNodes(xpath)

        Return XmlList

    End Function

    <DebuggerStepThrough()> _
    Function IsHtmlEncoded(text As String) As Boolean

        Dim ret As Boolean = False
        Dim result As String

        result = HtmlDecode(text)

        If result <> text Then
            ret = True
        Else
            ret = False
        End If

        Return ret

    End Function

    <DebuggerStepThrough()> _
    Function DecodeHTML(html As String) As String

        Dim ret As String

        ret = html

        Do Until IsHtmlEncoded(ret) = False Or ret = ""
            ret = HtmlDecode(ret)
        Loop

        Return ret

    End Function


End Module

