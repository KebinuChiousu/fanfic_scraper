'
' Created by SharpDevelop.
' User: Kevin
' Date: 10/5/2012
' Time: 9:31 PM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public MustInherit Class DAL
	
	Public MustOverride Function GetConnectionString(ConnStr As String) As String
	Public MustOverride Function SetConnectionString(ConnStr As String) As String
	
	
End Class
