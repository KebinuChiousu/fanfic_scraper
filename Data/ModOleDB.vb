Imports System.Data
Imports System.Data.OleDb

Module ModOleDB
    Dim ifr As IniFileReader
#Region "OLE DB Framework"

    Public Enum DatabaseType
        Access = 0
    End Enum

    Public SelectCmd As New OleDbCommand
    Public Builder As OleDbCommandBuilder
    Public conn1 As OleDbConnection

    Public DataSource As String
    Public Tbl As String

    Sub CreateConnection(ByVal type As DatabaseType)

        conn1 = Nothing

        Dim connStr As String = ""

        Select Case type

            Case DatabaseType.Access

                'Connection String to Access2000 Database
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                          "Data Source=" & DataSource & _
                          ";Persist Security Info=False"

        End Select

        'Declare and Open OLEDB CONNECTION FOR Access2k database
        conn1 = New OleDbConnection(connStr)
        conn1.Open()

    End Sub

    Public Function LoadData(ByVal selectcmd As OleDbCommand) As DataSet

        ' ###################################################
        ' # Declare a DataSet Object in Preparation for the #
        ' # DataAdapter(ResultSet)                          #
        ' ###################################################
        Dim ds As DataSet = New DataSet

        'Declare and Create Data Adapter
        Dim da As New OleDbDataAdapter

        selectcmd.Connection = conn1
        da.SelectCommand = selectcmd

        'Fill the DataSet with the Data
        da.Fill(ds)

        LoadData = ds
        da.Dispose()
        conn1.Dispose()

    End Function

    Public Sub SynchData(ByVal ds As DataSet)

        ' ###################################################
        ' # Declare a DataSet Object in Preparation for the #
        ' # DataAdapter(ResultSet)                          #
        ' ###################################################
        Dim oldDS As DataSet = New DataSet

        'Declare and Create Data Adapter
        Dim daSynch As New OleDbDataAdapter

        'AddHandler daSynch.RowUpdated, AddressOf daSynch_OnRowUpdate


        SelectCmd.Connection = conn1
        daSynch.SelectCommand = SelectCmd


        'Fill the DataSet with the Old Data
        daSynch.Fill(oldDS)

        Builder = New OleDbCommandBuilder(daSynch)
        Builder.QuotePrefix = "["
        Builder.QuoteSuffix = "]"

        daSynch.InsertCommand = Builder.GetInsertCommand

        daSynch.UpdateCommand = Builder.GetUpdateCommand

        'Update Database with new Dataset
        Try
            daSynch.Update(ds)
        Catch ex As System.Data.oledb.OleDbException
            MsgBox(ex.ToString)
        End Try

        Builder.Dispose()
        daSynch.Dispose()
        conn1.Dispose()

    End Sub

    Sub daSynch_OnRowUpdate(ByVal sender As Object, ByVal e As OleDbRowUpdatedEventArgs)

        Dim oCmd As OleDbCommand = New OleDbCommand("SELECT @@IDENTITY", _
                                                    e.Command.Connection)

        e.Row("Index") = oCmd.ExecuteScalar()
        e.Row.AcceptChanges()
    End Sub

    Public Sub BuildSelectCmd(ByVal sql As String)

        'Build Select Command 
        SelectCmd.CommandType = CommandType.Text
        SelectCmd.CommandText = sql

    End Sub

#End Region

    Sub GetHPData()
        Dim ds As New DataSet

        'path to Access2000 Database
        ifr = New IniFileReader(Application.StartupPath & "\config.ini", True)
        DataSource = ifr.GetIniValue("Harry Potter", "Path")

        'Table you are accessing
        Tbl = "Select * From HP Order By Folder"

        'Build Select Statement
        BuildSelectCmd(Tbl)

        'Open Connection to Database
        CreateConnection(DatabaseType.Access)
        'Load Data into Dataset
        ds = LoadData(SelectCmd)


        'Initialize Debug Console
        Initialize( _
                    forms.frmDebug _
                  )
        'Update Data in Debug Console
        frmDebug.UpdateDBData(ds)

        Application.DoEvents()
        Application.DoEvents()

    End Sub

    Sub GetRanmaData()
        Dim ds As New DataSet

        'path to Access2000 Database

        ifr = New IniFileReader(Application.StartupPath & "\config.ini", True)
        DataSource = ifr.GetIniValue("Ranma", "Path")

        'Table you are accessing
        Tbl = "Select * From Ranma Order By Folder"

        'Build Select Statement
        BuildSelectCmd(Tbl)

        'Open Connection to Database
        CreateConnection(DatabaseType.Access)
        'Load Data into Dataset
        ds = LoadData(SelectCmd)


        'Initialize Debug Console
        Initialize( _
                    forms.frmDebug _
                  )
        'Update Data in Debug Console
        frmDebug.UpdateDBData(ds)

        Application.DoEvents()
        Application.DoEvents()

    End Sub

End Module
