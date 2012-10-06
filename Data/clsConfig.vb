Imports System.Configuration

Public Class clsConfig
	
	Private Type As DBConnType
	
	Sub New()
		Type = DBConnType.Access
	End Sub

    Private Enum DBConnType
    	Access = 0
    	SQLLite = 1
    End Enum

    Public Function GetConnStr( _
                                 ByVal csName As String _                                 
                                 ) As String
    	
    	
        Dim conf As System.Configuration.Configuration
        Dim ConnStr As String = ""

        conf = ConfigurationManager. _
               OpenExeConfiguration( _
                                     ConfigurationUserLevel.None _
                                   )

        Dim csSection _
                As ConnectionStringsSection = _
                conf.ConnectionStrings

        csName = GetConnStrName(csName)

        Dim path As String = ""

        Try
            ConnStr = csSection.ConnectionStrings(csName).ConnectionString
        Catch
            ConnStr = ""
        End Try

        If ConnStr = "" Then
            Return ""
        End If

		Select Case Type		
            Case DBConnType.Access
            	path = GetAccessString(ConnStr)
            Case DBConnType.SQLLite
            	path = GetSQLLiteString(ConnStr)
        End Select

        Return path

    End Function

    Sub UpdateConnStr( _
                       ByVal csName As String, _
                       ByVal csValue As String _                       
                     )

        Dim conf As System.Configuration.Configuration
        Dim csSettings As New ConnectionStringSettings


        conf = ConfigurationManager. _
               OpenExeConfiguration( _
                                     ConfigurationUserLevel.None _
                                   )


        ' Get the connection strings section.
        Dim csSection _
        As ConnectionStringsSection = _
        conf.ConnectionStrings

        ' Get the current connection strings count.
        Dim connStrCnt As Integer = _
        ConfigurationManager.ConnectionStrings.Count

        csName = GetConnStrName(csName)

        ' Create the connection string name. 
        'Dim csName As String = "ConnStr" + connStrCnt.ToString()
        Select Case Type
            Case DbconnType.Access
            	csSettings = SetAccessString(csName, csValue)
            Case DBConnType.SQLLite 
            	csSettings = SetSQLLiteString(csName,csValue)
        End Select

        csSection.ConnectionStrings.Remove(csName)
        csSection.ConnectionStrings.Add(csSettings)

        ' Save the configuration file.
        conf.Save(ConfigurationSaveMode.Modified)

        ConfigurationManager.RefreshSection("connectionStrings")


    End Sub

#Region "Access Routines"

    Private Function SetAccessString( _
                                      ByVal csName As String, _
                                      ByVal Path As String _
                                    ) As ConnectionStringSettings

        Dim connstr As String = ""

        connstr = "Provider=Microsoft.Jet.OLEDB.4.0"
        connstr += ";"
        connstr += "Data Source="
        connstr += """" & Path & """"
        connstr += ";"
        connstr += "Persist Security Info=True"

        ' Create a connection string element and
        ' save it to the configuration file.
        ' Create a connection string element.
        Dim csSettings As New _
        ConnectionStringSettings( _
                                  csName, _
                                  connstr, _
                                  "System.Data.OleDb" _
                                )

        Return csSettings

    End Function

    Private Function GetAccessString(ByVal ConnStr As String) As String

        Dim Conn() As String
        Dim path As String = ""
        Dim index As Integer

        Try
            Conn = Split(ConnStr, ";")

            For index = 0 To UBound(Conn)
                If InStr(Conn(index), "Data Source") Then
                    Conn = Split(Conn(index), "=")
                    path = Conn(1)
                    path = Replace(path, """", "")
                    Exit For
                End If
            Next

        Catch
            path = ""
        End Try

        Return path

    End Function

#End Region

#Region "SQLLite Routines"

	Private Function SetSQLLiteString( _
                                      ByVal csName As String, _
                                      ByVal Path As String _
                                    ) As ConnectionStringSettings

	Dim connstr As String = ""
		      
        connstr = "Data Source="
        connstr += """" & Path & """"
        connstr += ";"
        connstr += "Version=3"
        connstr += ";"
        connstr += "New=False"
        connstr += ";"
        connstr += "Compress=True"
        connstr += ";"
        
        ' Create a connection string element and
        ' save it to the configuration file.
        ' Create a connection string element.
        Dim csSettings As New _
        ConnectionStringSettings( _
                                  csName, _
                                  connstr, _
                                  "System.Data.SQLite" _
                                )

        Return csSettings

    End Function

    Private Function GetSQLLiteString(ByVal ConnStr As String) As String

        Dim Conn() As String
        Dim path As String = ""
        Dim index As Integer

        Try
            Conn = Split(ConnStr, ";")

            For index = 0 To UBound(Conn)
                If InStr(Conn(index), "Data Source") Then
                    Conn = Split(Conn(index), "=")
                    path = Conn(1)
                    path = Replace(path, """", "")
                    Exit For
                End If
            Next

        Catch
            path = ""
        End Try

        Return path

    End Function


#End Region

    Private Function GetConnStrName(ByVal csName As String) As String

        Dim ns As String = My.Application.GetType.Namespace
        Dim settings As String = "MySettings"
        Dim name As String = ""

        name = ns
        name += "."
        name += settings
        name += "."
        name += csName

        Return name

    End Function

End Class
