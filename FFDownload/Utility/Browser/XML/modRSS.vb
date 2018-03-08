Imports System.Xml

Module modRSS

#Region "RSS Feed Conversion"

    Function RDFtoRSS(ByVal xmlDoc As XmlDocument) As XmlDocument

        Dim xml_Doc As New XmlDocument

        xml_Doc = xmlDoc

        Dim xmlList As XmlNodeList = xml_Doc.DocumentElement.ChildNodes

        Dim count As Long
        Dim max As Long = xmlList.Count - 1

        Dim name As String


        For count = max To 0 Step -1
            name = xmlList.Item(count).Name
            Select Case name
                Case "item"
                    'Do Nothing
                Case Else
                    xml_Doc.DocumentElement.RemoveChild(xmlList.Item(count))
            End Select
        Next

        xmlDoc = xml_Doc

        Dim xml As String
        xmlList = xmlDoc.SelectNodes("//item")

        If xmlList.Count = 0 Then
            Return Nothing
        End If

        Dim count2 As Integer

        xml = "<rss>"

        For count2 = 0 To xmlList.Count - 1

            xml += xmlList(count2).OuterXml

        Next

        xml += "</rss>"

        xml_Doc.LoadXml(xml)

        Return xml_Doc

    End Function

    Function ATOMtoRSS(ByVal xmlDoc As XmlDocument) As XmlDocument

        Dim xml_Doc As New XmlDocument

        xml_Doc = xmlDoc

        Dim xmlList As XmlNodeList = xml_Doc.DocumentElement.ChildNodes

        Dim count As Long
        Dim max As Long = xmlList.Count - 1

        Dim name As String

        For count = max To 0 Step -1
            name = xmlList.Item(count).Name
            Select Case name
                Case "entry"
                    'Do Nothing
                Case Else
                    xml_Doc.DocumentElement.RemoveChild(xmlList.Item(count))
            End Select
        Next

        xmlDoc = xml_Doc

        Dim xml As String
        xmlList = xmlDoc.SelectNodes("//entry")

        If xmlList.Count = 0 Then
            Return Nothing
        End If

        Dim count2 As Integer

        xml = "<rss>"

        For count2 = 0 To xmlList.Count - 1

            xml += xmlList(count2).OuterXml

        Next

        xml += "</rss>"

        xml_Doc.LoadXml(xml)


        Return xml_Doc

    End Function

    Function CleanRSS(ByVal xmlDoc As XmlDocument) As XmlDocument

        Dim xml_Doc As New XmlDocument

        xml_Doc = xmlDoc

        Dim node As XmlNode

        Dim xmlList As XmlNodeList = xml_Doc.DocumentElement.ChildNodes

        node = xmlList(0)

        xmlList = node.ChildNodes

        Dim count As Long
        Dim max As Long = xmlList.Count - 1

        Dim name As String

        For count = max To 0 Step -1
            name = xmlList.Item(count).Name.ToLower
            Select Case name
                Case "item"
                    'Do Nothing
                Case Else
                    node.RemoveChild(xmlList.Item(count))
            End Select
        Next


        xmlDoc = xml_Doc

        Dim xml As String
        xmlList = xmlDoc.SelectNodes("//item")

        If xmlList.Count = 0 Then
            Return Nothing
        End If

        Dim count2 As Integer

        xml = "<rss>"

        For count2 = 0 To xmlList.Count - 1

            xml += xmlList(count2).OuterXml

        Next

        xml += "</rss>"

        xml_Doc.LoadXml(xml)


        Return xml_Doc

    End Function

#End Region

    Function CleanFeed(ByVal xmlDoc As XmlDocument) As XmlDocument

        Dim rss As XmlDocument

        rss = xmlDoc

        Dim type As String

        type = rss.DocumentElement.LocalName.ToLower

        Select Case type
            Case "rdf"
                rss = RDFtoRSS(rss)
            Case "feed"
                rss = ATOMtoRSS(rss)
            Case "rss"
                rss = CleanRSS(rss)
            Case Else
                Return Nothing
        End Select

        Return rss

    End Function

End Module
