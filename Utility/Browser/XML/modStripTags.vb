Imports System.Xml

Module modStripTags

    Public Enum paramType
        Tag = 1
        Attribute = 2
        Value = 3
    End Enum

    Public Enum partialM
        Yes = 1
        No = 2
    End Enum

    Sub StripTags( _
                   ByRef xml As String, _
                   ByVal param As String, _
                   Optional ByVal type As paramType = paramType.Tag, _
                   Optional ByVal partialMatch As partialM = partialM.No _
                 )

        Dim xmldoc As New XmlDocument

        xml = CleanString(xml)

        xmldoc.LoadXml(xml)

        StripChildTags( _
                        xmldoc.DocumentElement, _
                        param, _
                        type, _
                        partialMatch _
              )

        xml = xmldoc.OuterXml

        xmldoc = Nothing

    End Sub

    Sub StripChildTags( _
                        ByRef xml_node As XmlNode, _
                        ByVal param As String, _
                        ByVal type As paramType, _
                        ByVal partialMatch As partialM _
                      )

        Dim name As String
        Dim count As Integer
        Dim attr_count As Integer

        Dim xmllist As XmlNodeList = xml_node.ChildNodes

        Dim child_node As XmlNode

        For count = (xmllist.Count - 1) To 0 Step -1
            child_node = xmllist.Item(count)

            Select Case type
                Case paramType.Tag

                    name = child_node.LocalName
                    Select Case partialMatch
                        Case partialM.No
                            If name = param Then
                                child_node.RemoveAll()
                            End If
                        Case partialM.Yes
                            If InStr(name, param) > 0 Then
                                child_node.RemoveAll()
                            End If
                    End Select


                Case paramType.Attribute
                    If Not IsNothing(child_node.Attributes) Then

                        Dim match As Boolean = False

                        For attr_count = (child_node.Attributes.Count - 1) To 0 Step -1
                            If child_node.Attributes.Count > 0 Then
                                name = child_node.Attributes(attr_count).Value
                                Select Case partialMatch
                                    Case partialM.No
                                        match = (name = param)
                                    Case partialM.Yes
                                        match = (InStr(name, param) > 0)
                                End Select

                                If match Then
                                    child_node.RemoveAll()
                                End If
                            End If
                        Next
                    End If

            End Select

            If child_node.ChildNodes.Count > 0 Then
                StripChildTags(child_node, param, type, partialMatch)
            End If

        Next count

    End Sub


End Module
