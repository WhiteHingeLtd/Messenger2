Imports WHLClasses
Public Module Class1
    Public Function CheckForUserInThread(EmployeeID As Integer, threadid As Integer) As Boolean
        'Dim EmpCol As New EmployeeCollection
        'Dim EmployeeID As Integer
        'Dim UserNameReplaced As String = My.User.Name.Replace("AD\", "")
        'EmployeeID = EmpCol.FindEmployeeByADUser(UserNameReplaced).PayrollId
        Dim WhichThreads As ArrayList
        WhichThreads = WHLClasses.MySQL.SelectData("SELECT participantid FROM whldata.messenger_threads WHERE (ThreadID=" + threadid.ToString + ") ORDER BY idmessenger_threads DESC ;")
        For Each CheckUser As ArrayList In WhichThreads
            If CheckUser(0) = EmployeeID Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function SendMessage(TextBox As Object, EmployeeID As Integer, threadid As Integer)
        Dim theText As String = TextBox.Text.Replace("\", "\\").Replace("'", "\'").Replace(vbCrLf, " ").Replace(vbLf, " ").Replace(vbCr, " ")

        Dim responseInsert As Object = WHLClasses.MySQL.insertUpdate("INSERT INTO whldata.messenger_messages (participantid, messagecontent, timestamp, threadid ) VALUES (" + EmployeeID.ToString + ",'" + theText + "','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + threadid.ToString + ");")

        Return TextBox.Text = ""
    End Function
    Public Function UpdateThreads(ThreadList As Label, EmployeeID As Integer)
        Dim EmpColl As New EmployeeCollection
        Dim ThreadUpdates As New ArrayList
        Dim ThreadUsers As New ArrayList
        Dim ThreadString As String = ""
        ThreadUpdates = WHLClasses.MySQL.SelectData("SELECT * FROM whldata.messenger_threads WHERE (participantid=" + EmployeeID.ToString + ") ORDER BY idmessenger_threads DESC ;")
        For Each Thread As ArrayList In ThreadUpdates
            If Thread(2) = EmployeeID Then
                ThreadUsers = WHLClasses.MySQL.SelectData("SELECT participantid FROM whldata.messenger_threads WHERE (ThreadID=" + Thread(1).ToString + ") ORDER BY idmessenger_threads DESC ;")
                For Each ThreadUser As ArrayList In ThreadUsers
                    ThreadString = ThreadString + "<br>" + EmpColl.FindEmployeeByID(Convert.ToInt32(ThreadUser(2))).FullName
                Next
            End If
            'Not our Thread so doesn't apply to us
        Next
        ThreadList.Text = ThreadString
        Return Nothing
    End Function

End Module
