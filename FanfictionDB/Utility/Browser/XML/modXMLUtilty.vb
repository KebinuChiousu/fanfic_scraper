Imports System.Xml

Module modXMLUtilty

    Public Enum paramType
        Tag = 1
        Attribute = 2
    End Enum

    Function GetNodes( _
                       ByVal xmldoc As XmlDocument, _
                       ByVal xpath As String _
                     ) As XmlNodeList

        Dim XmlList As XmlNodeList

        XmlList = xmldoc.SelectNodes(xpath)

        Return XmlList

    End Function

    Function ReturnNodes(ByVal xmlDoc As XmlDocument, ByVal XPath As String) As XmlDocument

        Dim xml_Doc As New XmlDocument

        Dim xmlList As XmlNodeList

        Dim xml As String
        xmlList = xmlDoc.SelectNodes(XPath)

        'If xmlList.Count = 0 Then
        '    Return Nothing
        'End If

        Dim count2 As Integer

        xml = "<html><body>"

        For count2 = 0 To xmlList.Count - 1

            xml += xmlList(count2).OuterXml

        Next

        xml += "</body></html>"

        xml_Doc.LoadXml(xml)


        Return xml_Doc

    End Function

    Function GetFirstNodeValue( _
                               ByVal xmldoc As XmlDocument, _
                               ByVal xpath As String _
                             ) As String

        Dim result As String = ""
        Dim xml_node As XmlNode
        Dim XmlList As XmlNodeList

        XmlList = xmldoc.SelectNodes(xpath)

        If XmlList.Count > 0 Then
            xml_node = XmlList(0)
            result = xml_node.InnerText
        End If
        

        Return result

    End Function

    Sub SearchForParameter( _
                            ByVal xml_node As XmlNode, _
                            ByVal param As String, _
                            ByRef result As String, _
                            ByVal type As paramType, _
                            ByVal partialMatch As partialM _
                          )

        Dim count As Long
        Dim attr_count As Integer
        Dim name As String
        Dim value As String

        Dim xmllist As XmlNodeList = xml_node.ChildNodes
        Dim child_node As XmlNode

        For count = (xmllist.Count - 1) To 0 Step -1
            child_node = xmllist.Item(count)

            Select Case type
                Case paramType.Tag

                    name = child_node.LocalName
                    If name = param Then
                        result = child_node.InnerXml
                        Exit Sub
                    End If

                Case paramType.Attribute
                    If Not IsNothing(child_node.Attributes) Then

                        Dim match As Boolean = False

                        For attr_count = (child_node.Attributes.Count - 1) To 0 Step -1
                            If child_node.Attributes.Count > 0 Then
                                name = child_node.Attributes(attr_count).Name
                                value = child_node.Attributes(attr_count).Value

                                Select Case partialMatch
                                    Case partialM.No
                                        match = (value = param)
                                    Case partialM.Yes
                                        match = (InStr(value, param) > 0)
                                End Select

                                If match Then
                                    result = child_node.InnerXml
                                    Exit Sub
                                End If

                            End If
                        Next attr_count
                    End If
            End Select

            If child_node.ChildNodes.Count > 0 Then
                SearchForParameter(child_node, param, result, type, partialMatch)
            End If

            If result <> "" Then
                Exit Sub
            End If

        Next

    End Sub

    Function GetAttrValue( _
                       ByVal xmldoc As XmlDocument, _
                       ByVal element As String, _
                       ByVal attribute As String, _
                       Optional ByVal search As String = "" _
                     ) As String

        Dim value As String = ""
        Dim temp As String = ""

        Dim xmlList As XmlNodeList

        Dim xpath As String = "//"
        xpath += element
        xpath += "[@" & attribute & "]"

        xmlList = xmldoc.SelectNodes(xpath)

        Dim count As Long
        Dim max As Long = xmlList.Count - 1

        For count = 0 To max
            temp = xmlList(count).Attributes(attribute).Value

            If search = "" Then
                value = temp
                Exit For
            End If

            If InStr(temp, search) <> 0 Then
                value = temp
                Exit For
            End If

        Next

        Return value

    End Function

End Module
