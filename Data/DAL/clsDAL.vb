Imports System.Configuration

Public MustInherit Class DAL

    Public MustOverride _
    Function GetData( _
                      ByVal Category_ID As Integer, _
                      Optional ByVal ALL As Boolean = False _
                    ) As DataTable

    Public MustOverride _
    Function UpdateData(ByVal dt As DataTable) As Integer

    Public MustOverride _
    Function GetCategories() As DataTable

    Protected MustOverride _
    Function GetConnectionString(ConnStr As String) As String

    Protected MustOverride _
    Function SetConnectionString( _
                                  csName As String, _
                                  csValue As String _
                                ) As ConnectionStringSettings

#Region ".NET Configuration Management"

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

        path = Me.GetConnectionString(ConnStr)

        'Select Case Type		
        '          Case DBConnType.Access
        '          	path = GetAccessString(ConnStr)
        '          Case DBConnType.SQLLite
        '          	path = GetSQLLiteString(ConnStr)
        '      End Select

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

        csSettings = Me.SetConnectionString(csName, csValue)

        csSection.ConnectionStrings.Remove(csName)
        csSection.ConnectionStrings.Add(csSettings)

        ' Save the configuration file.
        conf.Save(ConfigurationSaveMode.Modified)

        ConfigurationManager.RefreshSection("connectionStrings")

    End Sub

#End Region

End Class
