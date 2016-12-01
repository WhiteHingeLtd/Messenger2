Imports WHLClasses

Public Class _Default
    Inherits Page
    Dim EmpCol As New EmployeeCollection
    Dim EmployeeID As Integer
    Dim ThreadID As Integer = 1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim UserNameReplaced As String = My.User.Name.Replace("AD\", "")
        Dim EmployeesInThread As String = ""
        Dim WhichThreads As New ArrayList
        'SomethingUseful.Text = EmpCol.FindEmployeeByADUser(UserNameReplaced).FullName
        EmployeeID = EmpCol.FindEmployeeByADUser(UserNameReplaced).PayrollId
        UpdateThreads(ThreadList, EmployeeID)
        If CheckForUserInThread(EmpCol.FindEmployeeByADUser(UserNameReplaced).PayrollId, ThreadID) = True Then
            '  SomethingUseful.Text = "You are in this Thread"

        ElseIf CheckForUserInThread(EmpCol.FindEmployeeByADUser(UserNameReplaced).PayrollId, ThreadID) = False Then
            ' SomethingUseful.Text = "You aren't in this thread"
        End If
        'WhichThreads = WHLClasses.MySQL.SelectData("SELECT participantid FROM whldata.messenger_threads WHERE (ThreadID=" + ThreadID.ToString + ") ORDER BY idmessenger_threads DESC ;")

        'For Each Meme As ArrayList In WhichThreads
        '    EmployeesInThread = EmployeesInThread + vbNewLine + EmpCol.FindEmployeeByID(Meme(0)).FullName

        'Next
        'SomethingUseful.Text = EmployeesInThread
        'Dim FirstMessage As New ArrayList

        'FirstMessage = WHLClasses.MySQL.SelectData("SELECT messagecontent FROM whldata.messenger_messages WHERE (participantid=" + EmployeeID.ToString + " AND threadid=" + ThreadID.ToString + ") ORDER BY messageid DESC LIMIT 1;")
        'For Each Message As ArrayList In FirstMessage
        '    LastMessage.Text = Message(0).ToString
        'Next
    End Sub

    Protected Sub Send_Click(sender As Object, e As EventArgs) Handles Send.Click
        Dim UserNameReplaced As String = My.User.Name.Replace("AD\", "")
        EmployeeID = EmpCol.FindEmployeeByADUser(UserNameReplaced).PayrollId
        If Not IsNothing(TextBox1.Text) Then
            SendMessage(TextBox1, EmployeeID, ThreadID)
        End If

        'Dim theText As String = TextBox1.Text.Replace("\", "\\").Replace("'", "\'").Replace(vbCrLf, " ").Replace(vbLf, " ").Replace(vbCr, " ")

        'Dim responseInsert As Object = WHLClasses.MySQL.insertUpdate("INSERT INTO whldata.messenger_messages (participantid, messagecontent, timestamp, threadid ) VALUES (" + EmployeeID.ToString + ",'" + theText + "','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + ThreadID.ToString + ");")

        ''If Not IsNumeric(responseInsert) Then
        ''MsgBox(responseInsert)
        ''End If
        'TextBox1.Focus()
        'TextBox1.Text = ""
        ''UpdateNewMessages()
        'TextBox1.Text = responseInsert.ToString
    End Sub

End Class