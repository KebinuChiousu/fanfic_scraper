Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports NHibernate
Imports NHibernate.Tool.hbm2ddl
Imports FluentNHibernate
Imports FluentNHibernate.Cfg
Imports FluentNHibernate.Cfg.Db

Public Class nHibernateHelper

    Public Enum Database
        SQLite = 0
    End Enum

    Private _database As Database
    Private _sessionFactory As ISessionFactory
    Private _connstr As String

    ''' <summary>
    ''' Initialize nHibernate Helper Class
    ''' </summary>
    ''' <param name="connstr">Connection String to Database</param>
    ''' <remarks></remarks>
    Sub New(connstr As String, Optional ByVal db As Database = Database.SQLite)
        _connstr = connstr
        _database = db
    End Sub

    ''' <summary>
    ''' Obtain instance of a nHibernate SessionFactory
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SessionFactory As ISessionFactory
        Get
            If IsNothing(_sessionFactory) Then
                InitializeSessionFactory()
            End If

            Return _sessionFactory

        End Get

    End Property

    ''' <summary>
    ''' Initialize nHibernate SessionFactory
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeSessionFactory()

        Select Case _database
            Case Database.SQLite
                InitializeSQLiteSessionFactory()
        End Select

    End Sub


    ''' <summary>
    ''' Initialize nHibernate SQLite SessionFactory
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeSQLiteSessionFactory()

        _sessionFactory = Fluently.Configure() _
         .Database( _
                    SQLiteConfiguration.Standard _
                    .ConnectionString(_connstr) _
                  ) _
         .Mappings(Function(x) x.FluentMappings.AddFromAssemblyOf(Of Fanfic)()) _
         .BuildSessionFactory()

    End Sub

    ''' <summary>
    ''' Obtain nHibernate Session
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OpenSession() As ISession

        Return SessionFactory.OpenSession

    End Function


End Class
