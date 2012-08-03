Imports System.IO
Imports System.Xml
Imports XMLFilter

Module modXML

    Function DownloadXML(ByVal URL As String) As XmlDocument

        Dim Browser As New clsWeb

        Dim xml As String
        Dim xmldoc As New XmlDocument

        xml = Browser.DownloadPage(URL)

        Try
            xmldoc.LoadXml(xml)
        Catch
            xmldoc = Nothing
        End Try

        DownloadXML = xmldoc

        xmldoc = Nothing
        xml = Nothing
        Browser = Nothing

    End Function

    Function CleanXML( _
                        ByVal source As XmlDocument, _
                        Optional ByVal filter As Boolean = False _
                      ) As XmlDocument

        Dim xml As String
        Dim xmlDoc As New XmlDocument

        xml = ConvertXMLtoFeed(source.OuterXml, filter)

        StripTags(xml, "dd")

        xmlDoc.LoadXml(xml)

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

End Module
