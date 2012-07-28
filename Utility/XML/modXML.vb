Imports System.IO
Imports System.Xml
Imports HtmlReaderWriter
Imports XMLFilter

Module modXML

#Region "HTML"

    Function CleanHTML(ByVal html As String) As XmlDocument

        Dim xml As String
        Dim xmlDoc As New XmlDocument

        html = Replace(html, "di v", "div")

        xml = ConvertHTMLtoXHTML(html)

        'Dim sgml As New SGMLReaderHelper
        'xml = sgml.ProcessString(xml)

        Dim htmlDoc As New XmlDocument

        htmlDoc.LoadXml(xml)

        xmlDoc = StripTags(htmlDoc, "dd")
        'xmlDoc = htmlDoc

        htmlDoc = Nothing

        CleanHTML = xmlDoc

        xmlDoc = Nothing

    End Function

    Function ConvertHTMLtoXHTML(ByVal html As String) As String

        Dim result As String

        Dim reader As HtmlReader
        Dim outputFile As StringWriter
        Dim writer As HtmlWriter

        reader = New HtmlReader(html)

        outputFile = New StringWriter()
        writer = New HtmlWriter(outputFile)

        writer.FilterOutput = True

        reader.Read()
        While Not reader.EOF
            writer.WriteNode(reader, True)
        End While

        writer.Flush()
        result = outputFile.ToString

        outputFile.Close()

        Return result

    End Function

#End Region

#Region "XML"

    Function DownloadXML(ByVal URL As String) As XmlDocument

        Dim xml As String
        Dim xmldoc As New XmlDocument

        xml = DownloadPage(URL)

        Try
            xmldoc.LoadXml(xml)
        Catch
            xmldoc = Nothing
        End Try

        DownloadXML = xmldoc

        xmldoc = Nothing
        xml = Nothing

    End Function

    Function CleanXML( _
                        ByVal source As XmlDocument, _
                        Optional ByVal filter As Boolean = False _
                      ) As XmlDocument

        Dim xml As String
        Dim xmlDoc As New XmlDocument

        xml = ConvertXMLtoFeed(source.OuterXml, filter)

        'Dim sgml As New SGMLReaderHelper
        'xml = sgml.ProcessString(xml)

        Dim htmlDoc As New XmlDocument

        htmlDoc.LoadXml(xml)

        xmlDoc = StripTags(htmlDoc, "dd")
        'xmlDoc = htmlDoc

        htmlDoc = Nothing

        CleanXML = xmlDoc

        xmlDoc = Nothing

    End Function

    Function ConvertXMLtoFeed( _
                                 ByVal xml As String, _
                                 Optional ByVal Filter As Boolean = False _
                               ) As String

        Dim result As String

        Dim reader As XMLFilteringReader
        Dim outputFile As StringWriter
        Dim writer As XMLFilteringWriter


        reader = New XMLFilteringReader(xml)
        reader.StripPrefix = False

        outputFile = New StringWriter()
        writer = New XMLFilteringWriter(outputFile)

        writer.FilterOutput = Filter
        writer.ConvertPrefixesToTags = True

        reader.Read()
        While Not reader.EOF
            writer.WriteNode(reader, True)
        End While

        writer.WriteEndDocument()
        writer.Flush()

        result = outputFile.ToString

        outputFile.Close()

        Return result

    End Function

#End Region

End Module
