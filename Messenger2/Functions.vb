Imports WHLClasses
Public Module Functions

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
        TextBox.Text = ""
        Return Nothing
    End Function
    Public Function UpdateThreads(ThreadList As Panel, EmployeeID As Integer)
        Dim EmpColl As New EmployeeCollection
        Dim ThreadUpdates As New ArrayList
        Dim ThreadUsers As New ArrayList
        Dim ThreadString As String = ""
        ThreadUpdates = WHLClasses.MySQL.SelectData("SELECT a.*,b.messagecontent as Message,b.Timestamp as SendTime,b.participantid as sender FROM 	whldata.messenger_threads a Left Join (SELECT m1.* FROM whldata.messenger_messages m1 LEFT JOIN whldata.messenger_messages m2 ON (m1.threadid = m2.threadid AND m1.messageid < m2.messageid) WHERE m2.messageid IS NULL) b on b.threadid=a.ThreadID WHERE (a.participantid='" + EmployeeID.ToString + "') ORDER BY b.timestamp DESC;")
        For Each Thread As ArrayList In ThreadUpdates
            If Thread(2) = EmployeeID Then
                ThreadUsers = WHLClasses.MySQL.SelectData("SELECT participantid FROM whldata.messenger_threads WHERE (ThreadID=" + Thread(1).ToString + ") ORDER BY idmessenger_threads DESC ;")
                For Each ThreadUser As ArrayList In ThreadUsers
                    ThreadString = ThreadString + EmpColl.FindEmployeeByID(Convert.ToInt32(ThreadUser(0))).FullName + ", "
                Next
                Dim label As New Button
                Dim labelspace As New Label
                labelspace.Text = "<br>"
                label.ID = Thread(1).ToString
                label.Text = ThreadString
                AddHandler label.Click, AddressOf ProcessButton
                ThreadList.Controls.Add(label)
                ThreadList.Controls.Add(labelspace)
                ThreadString = ""

            End If
            'Not our Thread so doesn't apply to us
        Next
        'ThreadList.Text = ThreadString
        Dim DateTime As String = "<br><br>Last Updated:" + Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim DateTimeLabel As New Label
        DateTimeLabel.Text = DateTime
        ThreadList.Controls.Add(DateTimeLabel)

        Return Nothing
    End Function
    Public Function ListThreadUsers(ThreadID As Integer, Label As Label)
        Dim ThreadUsers As New ArrayList
        Dim EmpColl As New EmployeeCollection
        Dim ThreadString As String
        ThreadString = ""
        ThreadUsers = WHLClasses.MySQL.SelectData("SELECT participantid FROM whldata.messenger_threads WHERE (ThreadID=" + ThreadID.ToString + ") ORDER BY idmessenger_threads DESC ;")
        For Each ThreadUser As ArrayList In ThreadUsers
            ThreadString = ThreadString + EmpColl.FindEmployeeByID(Convert.ToInt32(ThreadUser(0))).FullName + ", "
        Next
        ThreadString.TrimEnd(",")
        Label.Text = ThreadString
        Return Nothing
    End Function
    Public Function UpdateContacts(ContactList As Panel, EmployeeID As Integer)
        Dim EmpColl As New EmployeeCollection
        Dim ContactName As String
        Dim ContactList1 As New ArrayList
        ContactName = ""
        For Each employee As Employee In EmpColl.Employees
            If Not employee.PayrollId = EmployeeID Then

                If employee.Visible Then

                    ContactName = employee.FullName
                    Dim label As New Button
                    Dim labelspace As New Label
                    labelspace.Text = "<br>"
                    label.Text = ContactName
                    ContactList.Controls.Add(label)
                    ContactList.Controls.Add(labelspace)
                    ContactName = ""
                End If
            End If
        Next
        Return Nothing
    End Function
    Public Sub ProcessButton(Sender As Button, e As Object)
        ActiveThreadID = Convert.ToInt32(Sender.ID)

    End Sub
    Public Function CleanPanel(Panel As Panel)

        Return Nothing
    End Function
    Public Function LoadMessages(Panel As Panel, Thread As Integer, employeeid As Integer)
        Dim EmpColl As New EmployeeCollection
        Dim ThreadUpdates As New ArrayList
        Dim ThreadUsers As New ArrayList
        Dim ThreadString As String = ""
        ThreadUpdates = WHLClasses.MySQL.SelectData("SELECT * from whldata.messenger_messages where threadid = '" + Thread.ToString + "' order by timestamp DESC;")
        For Each ThreadLoad As ArrayList In ThreadUpdates

            ThreadString = EmpColl.FindEmployeeByID(Convert.ToInt32(ThreadLoad(1))).FullName + ":" + ThreadLoad(2).ToString + "<br>"
            Dim label As New Label
            Dim labelspace As New Label
            labelspace.Text = "<br>"
            label.Text = ThreadString
            Panel.Controls.Add(label)
            Panel.Controls.Add(labelspace)
            ThreadString = ""


            'Not our Thread so doesn't apply to us
        Next
        'ThreadList.Text = ThreadString
        Dim DateTime As String = "<br><br>Last Updated:" + Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim DateTimeLabel As New Label
        DateTimeLabel.Text = DateTime
        Panel.Controls.Add(DateTimeLabel)

        Return Nothing
    End Function
End Module
