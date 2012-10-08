
Option Strict Off
Option Explicit On

Imports System
Imports System.Text
Imports System.Collections.Generic


Public Class Fanfic
    Private _id As Integer
    Private _title As String
    Private _author As String
    Private _folder As String
    Private _chapter As String
    Private _count As System.Nullable(Of Integer)
    Private _matchup As String
    Private _crossover As String
    Private _description As String
    Private _internet As String
    Private _storyID As String
    Private _abandoned As System.Nullable(Of Boolean)
    Private _complete As System.Nullable(Of Boolean)
    Private _publish_Date As System.Nullable(Of Date)
    Private _update_Date As System.Nullable(Of Date)
    Private _last_Checked As System.Nullable(Of Date)
    Private _category_Id As Integer

    Public Sub New()
        MyBase.New()
    End Sub
    Public Overridable Property Id() As Integer
        Get
            Return Me._id
        End Get
        Set(value As Integer)
            Me._id = value
        End Set
    End Property
    Public Overridable Property Title() As String
        Get
            Return Me._title
        End Get
        Set(value As String)
            Me._title = value
        End Set
    End Property
    Public Overridable Property Author() As String
        Get
            Return Me._author
        End Get
        Set(value As String)
            Me._author = value
        End Set
    End Property
    Public Overridable Property Folder() As String
        Get
            Return Me._folder
        End Get
        Set(value As String)
            Me._folder = value
        End Set
    End Property
    Public Overridable Property Chapter() As String
        Get
            Return Me._chapter
        End Get
        Set(value As String)
            Me._chapter = value
        End Set
    End Property
    Public Overridable Property Count() As System.Nullable(Of Integer)
        Get
            Return Me._count
        End Get
        Set(value As System.Nullable(Of Integer))
            Me._count = value
        End Set
    End Property
    Public Overridable Property Matchup() As String
        Get
            Return Me._matchup
        End Get
        Set(value As String)
            Me._matchup = value
        End Set
    End Property
    Public Overridable Property Crossover() As String
        Get
            Return Me._crossover
        End Get
        Set(value As String)
            Me._crossover = value
        End Set
    End Property
    Public Overridable Property Description() As String
        Get
            Return Me._description
        End Get
        Set(value As String)
            Me._description = value
        End Set
    End Property
    Public Overridable Property Internet() As String
        Get
            Return Me._internet
        End Get
        Set(value As String)
            Me._internet = value
        End Set
    End Property
    Public Overridable Property StoryID() As String
        Get
            Return Me._storyID
        End Get
        Set(value As String)
            Me._storyID = value
        End Set
    End Property
    Public Overridable Property Abandoned() As System.Nullable(Of Boolean)
        Get
            Return Me._abandoned
        End Get
        Set(value As System.Nullable(Of Boolean))
            Me._abandoned = value
        End Set
    End Property
    Public Overridable Property Complete() As System.Nullable(Of Boolean)
        Get
            Return Me._complete
        End Get
        Set(value As System.Nullable(Of Boolean))
            Me._complete = value
        End Set
    End Property
    Public Overridable Property Publish_Date() As System.Nullable(Of Date)
        Get
            Return Me._publish_Date
        End Get
        Set(value As System.Nullable(Of Date))
            Me._publish_Date = value
        End Set
    End Property
    Public Overridable Property Update_Date() As System.Nullable(Of Date)
        Get
            Return Me._update_Date
        End Get
        Set(value As System.Nullable(Of Date))
            Me._update_Date = value
        End Set
    End Property
    Public Overridable Property Last_Checked() As System.Nullable(Of Date)
        Get
            Return Me._last_Checked
        End Get
        Set(value As System.Nullable(Of Date))
            Me._last_Checked = value
        End Set
    End Property
    Public Overridable Property Category_Id() As Integer
        Get
            Return Me._category_Id
        End Get
        Set(value As Integer)
            Me._category_Id = value
        End Set
    End Property
End Class
