Imports HtmlAgilityPack
Imports System.IO
Imports System.Xml

Module modHTML

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

    Function FindNodesByValue( _
                               ByVal node As HtmlNode, _
                               ByVal NodeName As String, _
                               ByVal NodeValue As String _
                             ) As HtmlNodeCollection

        Dim ret As HtmlNodeCollection

        ret = node.SelectNodes("//" & NodeName & "[contains(text(), '" & NodeValue & "')]")

        Return ret

    End Function

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

    Function FindNodesByAttribute( _
                                   ByVal node As HtmlNode, _
                                   ByVal NodeName As String, _
                                   ByVal Attr As String _
                                 ) As HtmlNodeCollection

        Dim ret As HtmlNodeCollection

        ret = node.SelectNodes("//" & NodeName & "[@" & Attr & "]")

        Return ret

    End Function

    Function FindLinksByHref( _
                             ByVal node As HtmlNode, _
                             ByVal href As String _
                           ) As HtmlNodeCollection

        Dim ret As HtmlNodeCollection

        ret = node.SelectNodes("//a[contains(@href, '" & href & "')]")

        Return ret



    End Function

    Function GetHTMLNodes( _
                           ByVal htmldoc As HtmlDocument, _
                           ByVal xpath As String _
                         ) As HtmlNodeCollection

        Dim XmlList As HtmlNodeCollection

        XmlList = htmldoc.DocumentNode.SelectNodes(xpath)

        Return XmlList

    End Function



End Module

