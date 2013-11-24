Imports HtmlAgilityPack
Imports System.Linq
Imports System.IO
Imports System.Collections.Generic
Imports System.Xml

Module modStripTags

    

    Public Enum partialM
        Yes = 1
        No = 2
    End Enum

    Sub StripTag( _
                  ByRef html As String, _
                  ByVal attribute As String, _
                  ByVal value As String, _
                  Optional ByVal partialMatch As partialM = partialM.No _
                )

        Dim doc As New HtmlDocument
        Dim outputFile As StringWriter

        outputFile = New StringWriter()

        html = CleanString(html)

        doc.LoadHtml(html)

        Dim nodesToRemove As List(Of HtmlNode) = Nothing

        nodesToRemove = doc.DocumentNode.Descendants() _
                                   .Where(Function(n) n.Attributes.Contains(attribute)) _
                                   .ToList()

        For Each node In nodesToRemove
            If node.Attributes(attribute).Value.Contains(value) Then
                node.Remove()
            End If
        Next

        doc.OptionOutputAsXml = True

        doc.Save(outputFile)

        html = outputFile.ToString

        outputFile.Close()

        doc = Nothing

    End Sub

    Sub StripTag( _
                  ByRef html As String, _
                  ByVal tag As String, _
                  Optional ByVal partialMatch As partialM = partialM.No _
                 )

        Dim doc As New HtmlDocument
        Dim outputFile As StringWriter

        outputFile = New StringWriter()

        html = CleanString(html)

        doc.LoadHtml(html)

        Dim nodesToRemove As List(Of HtmlNode) = Nothing

        Select Case partialMatch
            Case partialM.Yes
                nodesToRemove = doc.DocumentNode.Descendants() _
                                   .Where(Function(n) n.Name.Contains(tag)) _
                                   .ToList()
            Case partialM.No
                nodesToRemove = doc.DocumentNode.Descendants() _
                                   .Where(Function(n) n.Name = tag) _
                                   .ToList()
        End Select

        For Each node In nodesToRemove
            node.Remove()
        Next

        doc.OptionOutputAsXml = True

        doc.Save(outputFile)

        html = outputFile.ToString

        outputFile.Close()

        doc = Nothing

    End Sub

End Module
