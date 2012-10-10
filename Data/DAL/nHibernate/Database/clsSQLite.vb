Imports System.Configuration
Imports System.Collections.Generic
Imports NHibernate
Imports NHibernate.Criterion
Imports NHibernate.Tool.hbm2ddl
Imports FluentNHibernate
Imports FluentNHibernate.Cfg
Imports FluentNHibernate.Cfg.Db


Public Class SQLite
    Inherits DAL

    Private orm As nHibernateHelper

    ''' <summary>
    ''' Initilizes SQLite Database connection settings
    ''' </summary>
    ''' <param name="ConnectionStringName">Name of ConnectionString Key in app.config</param>
    ''' <param name="Init">If set to true initialize nHibernate.</param>
    ''' <remarks></remarks>
    Public Sub New(ConnectionStringName As String, Optional ByVal Init As Boolean = True)

        Dim connstr As String

        If Init Then

            'Read Connection String from App Config XML File
            connstr = GetConnStr(ConnectionStringName)

            'Create an instance of the nHibernate Helper class using the connstr obtained from the config file.
            orm = New nHibernateHelper(connstr)
        End If

    End Sub

    ''' <summary>
    ''' Obtain List of Fanfiction Categories from SQLlite Database
    ''' </summary>
    ''' <returns>DataTable of ID and Names of Categories</returns>
    ''' <remarks></remarks>
    Public Overrides _
    Function GetCategories() As System.Data.DataTable

        Dim dt As DataTable
        Dim cat As New List(Of Category)

        Using s As ISession = orm.OpenSession

            s.CreateCriteria(Of Category).List(cat)

        End Using

        dt = cat.ToDataTable

        Return dt

    End Function

    ''' <summary>
    ''' Obtains DataTable of Metadata from SQLite Database
    ''' </summary>
    ''' <param name="Category_ID">ID number of Fiction Category.</param>
    ''' <param name="ALL">If set to true obtains all Stories regardless of Status.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides _
    Function GetData(Category_ID As Integer, Optional ALL As Boolean = False) As System.Data.DataTable

        Dim dt As DataTable
        Dim fic As New List(Of Fanfic)

        Using s As ISession = orm.OpenSession

            If ALL Then
                s.CreateCriteria(Of Fanfic) _
                    .Add(Restrictions.Eq("Category_Id", Category_ID)) _
                    .AddOrder(Order.Asc("Folder")) _
                    .List(fic)

            Else

                s.CreateCriteria(Of Fanfic) _
                    .Add(Restrictions.Eq("Abandoned", False)) _
                    .Add(Restrictions.Eq("Complete", False)) _
                    .Add(Restrictions.Eq("Category_Id", Category_ID)) _
                    .AddOrder(Order.Asc("Folder")) _
                    .List(fic)
            End If

        End Using

        dt = fic.ToDataTable

        Return dt


    End Function

    ''' <summary>
    ''' Updates Fanfiction Metadata using the provided DataTable as Input
    ''' </summary>
    ''' <param name="dt">DataTable containing fanfiction metadata.</param>
    ''' <returns>Number of rows affected.</returns>
    ''' <remarks></remarks>
    Public Overrides _
    Function UpdateData(ByRef dt As System.Data.DataTable) As Integer

        Dim fic As Fanfic
        Dim ret As Integer

        dt = dt.GetChanges()

        Using s As ISession = orm.OpenSession

            Using t As ITransaction = s.BeginTransaction

                For i = 0 To dt.Rows.Count - 1

                    fic = New Fanfic

                    fic.Id = dt.Rows(i).Item("Id")
                    fic.Title = dt.Rows(i).Item("Title")
                    fic.Author = dt.Rows(i).Item("Author")
                    fic.Folder = dt.Rows(i).Item("Folder")
                    fic.Chapter = dt.Rows(i).Item("Chapter").ToString
                    fic.Count = dt.Rows(i).Item("Count")
                    fic.Matchup = dt.Rows(i).Item("Matchup").ToString
                    fic.Crossover = dt.Rows(i).Item("Crossover").ToString
                    fic.Description = dt.Rows(i).Item("Description").ToString
                    fic.Internet = dt.Rows(i).Item("Internet").ToString
                    fic.StoryID = dt.Rows(i).Item("StoryID").ToString
                    fic.Abandoned = dt.Rows(i).Item("Abandoned")
                    fic.Complete = dt.Rows(i).Item("Complete")

                    If Not IsNothing(dt.Rows(i).Item("Publish_Date")) Then
                        fic.Publish_Date = dt.Rows(i).Item("Publish_Date")
                    End If

                    If IsDate(dt.Rows(i).Item("Update_Date")) Then
                        fic.Update_Date = dt.Rows(i).Item("Update_Date")
                    Else
                        If Not IsNothing(fic.Publish_Date) Then
                            fic.Update_Date = fic.Publish_Date
                        End If
                    End If

                    If Not IsNothing(dt.Rows(i).Item("Last_Checked")) Then
                        fic.Last_Checked = dt.Rows(i).Item("Last_Checked")
                    End If

                    fic.Category_Id = dt.Rows(i).Item("Category_Id")

                    s.SaveOrUpdate(fic)

                    fic = Nothing


                Next

                t.Commit()

            End Using
        End Using




        ret = dt.Rows.Count
        dt.AcceptChanges()

        fic = Nothing

        Return ret

    End Function

    ''' <summary>
    ''' Extracts path to SQLite Database from Connection String
    ''' </summary>
    ''' <param name="ConnStr">SQLite Connection String</param>
    ''' <returns>Path to SQLite Database</returns>
    ''' <remarks></remarks>
    Public Overrides _
    Function GetPath(ConnStr As String) As String

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

    ''' <summary>
    ''' Generates Connection String Settings from Key String and Path to SQLite Database
    ''' </summary>
    ''' <param name="csName">Name of Connection String Key</param>
    ''' <param name="Path">Path to SQLite Database</param>
    ''' <returns>ConnectionStringSettings for app.config file</returns>
    ''' <remarks></remarks>
    Protected Overrides _
    Function SetConnectionString( _
                                  csName As String, _
                                  Path As String _
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
        Dim csSettings As New  _
        ConnectionStringSettings( _
                                  csName, _
                                  connstr, _
                                  "System.Data.SQLite" _
                                )

        Return csSettings

    End Function

    
End Class
