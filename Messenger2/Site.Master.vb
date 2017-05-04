Imports WHLClasses
Public Class SiteMaster
    Inherits MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim emps As EmployeeCollection = Application("EmpCol")
        Dim CurrentEmp As Employee
        Try
            CurrentEmp = emps.FindEmployeeByADUser(My.User.Name.Replace("AD\", ""))
            Session("User") = CurrentEmp
        Catch ex As Exception
        End Try

    End Sub


End Class