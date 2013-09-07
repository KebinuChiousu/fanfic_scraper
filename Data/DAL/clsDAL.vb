Imports System.Configuration

Public MustInherit Class DAL

    ''' <summary>
    ''' Obatains Story Metadata
    ''' </summary>
    ''' <param name="Category_ID">Category Id of Metadata to Retrieve</param>
    ''' <param name="ALL">If set to true obtains all Stories regardless of Status.</param>
    ''' <returns>Returns DataTable of Story MetaData</returns>
    ''' <remarks></remarks>
    Public MustOverride _
    Function GetData( _
                      ByVal Category_ID As Integer, _
                      Optional ByVal ALL As Boolean = False _
                    ) As DataTable



    ''' <summary>
    ''' Checks to see if the record already exists in the database.
    ''' </summary>
    ''' <param name="category_id">Cateory Id to check for folder</param>
    ''' <returns>Retuurns true if record exists</returns>
    ''' <remarks></remarks>
    Public MustOverride _
    Function RecordExists(category_id As Integer) As Boolean

    ''' <summary>
    ''' Checks to see if the record already exists in the database.
    ''' </summary>
    ''' <param name="folder">Folder Nmae to check for.</param>
    ''' <param name="category_id">Cateory Id to check for folder</param>
    ''' <returns>Retuurns true if record exists</returns>
    ''' <remarks></remarks>
    Public MustOverride _
    Function RecordExists(folder As String, category_id As Integer) As Boolean

    ''' <summary>
    ''' Checks to see if the record already exists in the database.
    ''' </summary>
    ''' <param name="category">Category Name to check for.</param>
    ''' <returns>Returns true if record exists</returns>
    ''' <remarks></remarks>
    Public MustOverride _
    Function CategoryExists(category As String) As Boolean

    ''' <summary>
    ''' Update Categories in Database based on Metadata from DataTable
    ''' </summary>
    ''' <param name="dt">DataTable of Category MetaData</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride _
    Function UpdateCategories(ByRef dt As DataTable) As Integer

    ''' <summary>
    ''' Update Database based on Metadata from DataTable
    ''' </summary>
    ''' <param name="dt">DataTable of Story MetaData</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride _
    Function UpdateData(ByRef dt As DataTable) As Integer

    ''' <summary>
    ''' Retrieves Category Name and ID of Story Data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride _
    Function GetCategories() As DataTable

    ''' <summary>
    ''' Obtain Path to Database from ConnectionString
    ''' </summary>
    ''' <param name="ConnStr">Database Connection String</param>
    ''' <returns>Path to Database</returns>
    ''' <remarks></remarks>
    Public MustOverride _
    Function GetPath(ConnStr As String) As String

    ''' <summary>
    ''' Build Connection String
    ''' </summary>
    ''' <param name="csName">Name of Connection String Key</param>
    ''' <param name="csValue">Path to Database</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected MustOverride _
    Function SetConnectionString( _
                                  csName As String, _
                                  csValue As String _
                                ) As ConnectionStringSettings

#Region ".NET Configuration Management"

    ''' <summary>
    ''' Retrieve full Connection String Key Path from XML File
    ''' </summary>
    ''' <param name="csName">Connection String Key Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' Retrieve Connection String Name from App.Config
    ''' </summary>
    ''' <param name="csName">Name of Connection String Key</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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

        Return ConnStr

    End Function

    ''' <summary>
    ''' Update Connection String in App.Config
    ''' </summary>
    ''' <param name="csName">Name of Connection String Key</param>
    ''' <param name="csValue">Connection String</param>
    ''' <remarks></remarks>
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

        Try
            csSection.ConnectionStrings.Remove(csName)
        Catch
        End Try

        csSection.ConnectionStrings.Add(csSettings)

        ' Save the configuration file.
        conf.Save(ConfigurationSaveMode.Modified, True)

        ConfigurationManager.RefreshSection("connectionStrings")

    End Sub

#End Region

End Class
