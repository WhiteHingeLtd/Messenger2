﻿Imports WHLClasses
Imports System.Collections.Generic
Public Class _Default
    Inherits Page

    Dim EmpCol As New EmployeeCollection
    Dim EmployeeID As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim UserNameReplaced As String = My.User.Name.Replace("AD\", "")
        Dim EmployeesInThread As String = ""
        Dim WhichThreads As New ArrayList
        If Session("ActiveThreadID") = Nothing Then
            Session("ActiveThreadID") = 0
        End If
        If Session("Load") = Nothing Then
            Session("Load") = "Threads"
        End If
        Session("UserAgent") = Request.ServerVariables("HTTP_USER_AGENT")

        EmployeeID = EmpCol.FindEmployeeByADUser(UserNameReplaced).PayrollId

        Session("EmployeeID") = EmployeeID
        If Convert.ToInt32(Session("ActiveThreadID")) > 0 Then
            Try
                Dim SessionThreadID As String = Session("ActiveThreadID")
                LoadMessages(UpdatePanel2, Convert.ToInt32(SessionThreadID), EmployeeID)

                UpdatePanel2.Update()
            Catch ex As Exception
            End Try
        End If
        If Session("Load") = "Threads" Then
            UpdateThreads(ContactsPanel, EmployeeID)
            Session("Load") = "Threads"
            ContactsPanel.Update()
        End If
        If Session("Load") = "Contacts" Then
            UpdateContacts(ContactsPanel, EmployeeID)
            Session("Load") = "Contacts"
            UpdatePanel2.Update()
            ContactsPanel.Update()
        End If
        Dim UserAgent As String = Session("UserAgent")
        If UserAgent.Contains("Awesomium") Then
            Session("BrowserNotifications") = False
        Else
            Session("BrowserNotifications") = True
        End If




    End Sub
    Public Sub ProcessButton(Sender As LinkButton, e As Object)
        Session("ActiveThreadID") = Convert.ToInt32(Sender.ID)
        Try
            Dim SessionThreadID As String = Session("ActiveThreadID")
            LoadMessages(UpdatePanel2, Convert.ToInt32(SessionThreadID), EmployeeID)

            UpdatePanel2.Update()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub Send_Click(sender As Object, e As EventArgs) Handles Send.Click
        Dim UserNameReplaced As String = My.User.Name.Replace("AD\", "")
        Dim ActiveThreadIDString As String = Session("ActiveThreadID")
        EmployeeID = EmpCol.FindEmployeeByADUser(UserNameReplaced).PayrollId
        Dim uTime As Integer
        uTime = (DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds
        If FileUpload1.Visible = True Then
            If FileUpload1.HasFile Then
                Try
                    FileUpload1.SaveAs("C:\Data Storage\Intra\Uploads\" & uTime.ToString &
                   FileUpload1.FileName)
                    TextBox1.Text = "http://apps.ad.whitehinge.com/Uploads/" & uTime.ToString & FileUpload1.FileName


                    Dim theText As String = "http://apps.ad.whitehinge.com/Uploads/" & uTime.ToString & FileUpload1.FileName
                    Dim responseInsert As Object = WHLClasses.MySql.insertUpdate("INSERT INTO whldata.messenger_messages (participantid, messagecontent, timestamp, threadid ) VALUES (" + EmployeeID.ToString + ",'" + theText.ToString + "','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Session("ActiveThreadID").ToString + ");")
                Catch ex As Exception
                    TextBox1.Text = "ERROR: " & ex.Message.ToString()
                End Try
            Else
                TextBox1.Text = "You have not specified a file."
            End If
        Else

            If Not IsNothing(TextBox1.Text) Then
                SendMessage(TextBox1, EmployeeID, Convert.ToInt32(ActiveThreadIDString))
            End If

        End If
        TextBox1.Text = String.Empty
        UpdatePanel2.Update()
        ContactsPanel.Update()
    End Sub

    Public Sub Contacts_Click(sender As Object, e As EventArgs) Handles Contacts.Click

        Session("Load") = "Contacts"
        ContactsPanel.Update()
    End Sub

    Public Sub Threads_Click(sender As Object, e As EventArgs) Handles Threads.Click
        Session("Load") = "Threads"
        ContactsPanel.Update()
        'UpdateThreads(ThreadPanel, EmployeeID)
    End Sub

    Protected Sub ThreadTimer_Tick(sender As Object, e As EventArgs) Handles ThreadTimer.Tick
        UpdatePanel2.Update()
    End Sub

    Protected Sub ContactsTimer_Tick(sender As Object, e As EventArgs) Handles ContactsTimer.Tick
        ContactsPanel.Update()
    End Sub



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


    Public Function UpdateThreads(ThreadList As UpdatePanel, EmployeeID As Integer)
        Dim EmpColl As New EmployeeCollection
        Dim ThreadUpdates As New ArrayList
        Dim ThreadUsers As New ArrayList
        Dim ThreadString As String = ""
        ThreadUpdates = WHLClasses.MySQL.SelectData("SELECT a.*,b.messagecontent as Message,b.Timestamp as SendTime,b.participantid as sender FROM 	whldata.messenger_threads a Left Join (SELECT m1.* FROM whldata.messenger_messages m1 LEFT JOIN whldata.messenger_messages m2 ON (m1.threadid = m2.threadid AND m1.messageid < m2.messageid) WHERE m2.messageid IS NULL) b on b.threadid=a.ThreadID WHERE (a.participantid='" + EmployeeID.ToString + "') ORDER BY b.timestamp DESC;")
        For Each Thread As ArrayList In ThreadUpdates
            If Thread(2) = EmployeeID Then
                ThreadUsers = WHLClasses.MySQL.SelectData("SELECT participantid FROM whldata.messenger_threads WHERE (ThreadID=" + Thread(1).ToString + ") ORDER BY idmessenger_threads DESC ;")
                For Each ThreadUser As ArrayList In ThreadUsers
                    ThreadString = ThreadString + EmpColl.FindEmployeeByID(Convert.ToInt32(ThreadUser(0))).FullName + " "
                Next
                Try
                    Dim ThreadPanel1 As New Panel
                    Dim LinkButton As New LinkButton
                    Dim labelspace As New Label
                    LinkButton.CssClass = "ThreadButton"
                    ThreadString.TrimEnd()
                    'labelspace.Text = "<br>"
                    LinkButton.ID = Thread(1).ToString
                    LinkButton.Text = ThreadString
                    AddHandler LinkButton.Click, AddressOf ProcessButton
                    ThreadPanel1.Controls.Add(LinkButton)
                    ThreadList.ContentTemplateContainer.Controls.Add(ThreadPanel1)
                    ThreadString = ""
                    ThreadList.Update()
                Catch Ex As Exception
                End Try
            End If
            'Not our Thread so doesn't apply to us
        Next
        'ThreadList.Text = ThreadString
        Dim DateTime As String = "<br><br>Last Updated:" + Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim DateTimeLabel As New Label
        DateTimeLabel.Text = DateTime
        ThreadList.ContentTemplateContainer.Controls.Add(DateTimeLabel)
        ThreadList.Update()
        Return Nothing
    End Function
    Public Function ListThreadUsers(ThreadID As Integer, Panel As UpdatePanel)
        Dim ThreadUsers As New ArrayList
        Dim EmpColl As New EmployeeCollection
        Dim ThreadString As String
        ThreadString = "Users in this thread:<br>"
        ThreadUsers = WHLClasses.MySql.SelectData("SELECT participantid FROM whldata.messenger_threads WHERE (ThreadID=" + ThreadID.ToString + ") ORDER BY idmessenger_threads DESC ;")
        For Each ThreadUser As ArrayList In ThreadUsers
            Dim UserPanel As New Panel
            Dim UserName1 As New Label
            Dim RemoveUser As New LinkButton

            UserName1.Text = EmpCol.FindEmployeeByID(Convert.ToInt32(ThreadUser(0))).FullName

            RemoveUser.Text = "Remove User"
            RemoveUser.ID = "Remove" + ThreadUser(0)
            AddHandler RemoveUser.Click, AddressOf ProcessRemoveButton
            UserPanel.Controls.Add(UserName1)
            UserPanel.Controls.Add(RemoveUser)
            Panel.ContentTemplateContainer.Controls.Add(UserPanel)

        Next
        Return Nothing
    End Function
    Public Function UpdateContacts(ContactList As UpdatePanel, EmployeeID As Integer)
        Dim EmpColl As New EmployeeCollection
        Dim ContactName As String
        Dim ContactList1 As New ArrayList
        ContactName = ""
        For Each employee As Employee In EmpColl.Employees
            If Not employee.PayrollId = EmployeeID Then

                If employee.Visible Then

                    ContactName = employee.FullName
                    Dim ContactButton As New LinkButton
                    Dim ContactPanel As New Panel
                    Dim AddToThread As New LinkButton
                    ContactButton.CssClass = "ContactButton"
                    AddToThread.CssClass = "ThreadAdd"
                    AddToThread.Text = "Add To Current Thread"
                    AddToThread.ID = "AddTo" + employee.PayrollId.ToString
                    ContactButton.ID = employee.PayrollId.ToString
                    ContactButton.Text = ContactName
                    AddHandler ContactButton.Click, AddressOf ProcessContactButton
                    AddHandler AddToThread.Click, AddressOf ProcessAddButton
                    ContactPanel.Controls.Add(ContactButton)
                    ContactPanel.Controls.Add(AddToThread)
                    ContactList.ContentTemplateContainer.Controls.Add(ContactPanel)
                    ContactName = ""
                    ContactList.Update()
                End If
            End If
        Next
        Return Nothing
    End Function
    Public Sub ProcessAddButton(Sender As LinkButton, e As Object)
        Dim AddButtonString As String = Sender.ID
        AddButtonString = AddButtonString.Replace("AddTo", "")
        AddToThread(Convert.ToInt32(Session("ActiveThreadID")), AddButtonString)
    End Sub
    Public Sub ProcessContactButton(Sender As LinkButton, e As Object)
        Dim employeeID As Integer
        Dim EmpCol As New EmployeeCollection

        Dim UserNameReplaced As String = My.User.Name.Replace("AD\", "")
        employeeID = EmpCol.FindEmployeeByADUser(UserNameReplaced).PayrollId
        Dim CreateNewThread1 As String = Sender.ID + employeeID.ToString


        Dim GetLastMessage As ArrayList
        Dim LastThread As Integer
        Dim NewThread As Integer
        GetLastMessage = WHLClasses.MySql.SelectData("SELECT ThreadID FROM whldata.messenger_threads ORDER BY ThreadID DESC LIMIT 1;")
        For Each Message As ArrayList In GetLastMessage
            LastThread = Convert.ToInt32(Message(0))
            NewThread = LastThread + 1
        Next
        Session("ActiveThreadID") = Convert.ToInt32(NewThread)
        CreateNewThread(Sender.ID)
        CreateNewThread(employeeID)


    End Sub
    Public Function CreateNewThread(EmployeeID As Integer)
        Dim ActiveThreadIDString As String = Session("ActiveThreadID").ToString
        Dim responseInsert As Object = WHLClasses.MySQL.insertUpdate("INSERT INTO whldata.messenger_threads (ThreadID, participantid ) VALUES (" + "'" + ActiveThreadIDString.ToString + "','" + EmployeeID.ToString + "');")
        Return Nothing
    End Function
    Public Function AddToThread(ThreadID As Integer, EmployeeID As Integer)
        If CheckForUserInThread(EmployeeID, ThreadID) = False Then
            Dim responseInsert As Object = WHLClasses.MySql.insertUpdate("INSERT INTO whldata.messenger_threads (ThreadID, participantid ) VALUES ('" + ThreadID.ToString + "','" + EmployeeID.ToString + "');")
        Else
            MsgBox("This user is already in this thread")
        End If
        Return Nothing
    End Function
    Public Function RemoveFromThread(RemovedID As Integer, ThreadID As Integer)
        Dim OldThreadID As String = Session("ActiveThreadID").ToString
        Dim NewActiveID As String = OldThreadID.Replace(RemovedID.ToString, "")
        Session("ActiveThreadID") = Convert.ToInt32(NewActiveID)
        Dim responseInsert As Object = WHLClasses.MySQL.insertUpdate("INSERT INTO whldata.messenger_threads (ThreadID, participantid ) VALUES ('" + NewActiveID.ToString + "','" + RemovedID.ToString + "');")
        Dim responseInsert2 As Object = WHLClasses.MySQL.insertUpdate("UPDATE whldata.messenger_threads set ThreadID='" + NewActiveID.ToString + "' WHERE ThreadID ='" + OldThreadID.ToString + "';")
        Dim responseInsert3 As Object = WHLClasses.MySQL.insertUpdate("UPDATE whldata.messenger_messages set threadid='" + NewActiveID.ToString + "' WHERE threadid ='" + OldThreadID.ToString + "';")

        Return Nothing
    End Function

    Public Function CleanPanel(Panel As Panel)

        Return Nothing
    End Function
    Public Function LoadMessages(Panel As UpdatePanel, Thread As Integer, employeeid As Integer)
        Dim EmpColl As New EmployeeCollection
        Dim ThreadUpdates As New ArrayList
        Dim ThreadUsers As New ArrayList
        Dim EmployeeName As String = ""
        Dim ThreadString As String = ""
        Dim CurrentDirectory As String = "http://apps.ad.whitehinge.com/Uploads/"

        ThreadUpdates = WHLClasses.MySQL.SelectData("Select * from whldata.messenger_messages where threadid = '" + Thread.ToString + "' order by timestamp DESC;")
        For Each ThreadLoad As ArrayList In ThreadUpdates
            If employeeid = Convert.ToInt32(ThreadLoad(1)) Then
                ThreadString = ":" + ThreadLoad(2).ToString
                Dim UserLabel As New Label
                Dim Threadlabel As New Label
                Dim HoldingPanel As New Panel
                Dim Image As New Image
                If ThreadString.Contains(CurrentDirectory) Then
                    Image.ImageUrl = ThreadLoad(2).ToString
                Else
                    Threadlabel.Text = ThreadString
                End If

                UserLabel.CssClass = "User"
                Threadlabel.CssClass = "UserMessage"
                HoldingPanel.CssClass = "UserPanel"
                Image.CssClass = "UserImage"

                UserLabel.Text = EmpColl.FindEmployeeByID(Convert.ToInt32(ThreadLoad(1))).FullName


                HoldingPanel.Controls.Add(UserLabel)
                HoldingPanel.Controls.Add(Threadlabel)
                HoldingPanel.Controls.Add(Image)
                Panel.ContentTemplateContainer.Controls.Add(HoldingPanel)
                ThreadString = ""
                Panel.Update()
            Else
                ThreadString = ": " + ThreadLoad(2).ToString
                Dim UserLabel As New Label
                Dim Threadlabel As New Label
                Dim HoldingPanel As New Panel
                Dim Image As New Image
                If ThreadString.Contains(CurrentDirectory) Then
                    Image.ImageUrl = ThreadLoad(2).ToString
                Else
                    Threadlabel.Text = ThreadString
                End If
                UserLabel.CssClass = "Other"
                Threadlabel.CssClass = "OtherMessage"
                HoldingPanel.CssClass = "OtherPanel"
                Image.CssClass = "OtherImage"

                UserLabel.Text = EmpColl.FindEmployeeByID(Convert.ToInt32(ThreadLoad(1))).FullName

                HoldingPanel.Controls.Add(UserLabel)
                HoldingPanel.Controls.Add(Threadlabel)
                HoldingPanel.Controls.Add(Image)
                Panel.ContentTemplateContainer.Controls.Add(HoldingPanel)
                ThreadString = ""
                Panel.Update()
            End If
            'Not our Thread so doesn't apply to us
        Next
        'ThreadList.Text = ThreadString
        Dim DateTime As String = "<br><br>Last Updated:" + Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim DateTimeLabel As New Label
        DateTimeLabel.Text = DateTime
        Panel.ContentTemplateContainer.Controls.Add(DateTimeLabel)
        Panel.Update()
        Return Nothing
    End Function
    Public Function SendNotification(ThreadID As Integer, EmployeeID As Integer)
        Dim ThreadUsers As New ArrayList
        Dim EmpColl As New EmployeeCollection
        Dim ThreadString As String
        Dim NotificationString As New StringBuilder
        Dim Message As String = "You have a new Message"
        ThreadString = "Users in this thread:"
        ThreadUsers = WHLClasses.MySql.SelectData("SELECT participantid FROM whldata.messenger_threads WHERE (ThreadID=" + ThreadID.ToString + ") ORDER BY idmessenger_threads DESC ;")
        For Each ThreadUser As ArrayList In ThreadUsers
            ThreadString = ThreadString + EmpColl.FindEmployeeByID(Convert.ToInt32(ThreadUser(0))).FullName
        Next

        NotificationString.Append("spawnNotification(""" + Message + """,""" + ThreadString + """);" + vbNewLine)
        ScriptManager.RegisterClientScriptBlock(Page, Me.GetType, "memes", NotificationString.ToString, True)
        Return Nothing
    End Function

    Public Sub GetNotifications(EmployeeID)
        'Here we check for the Notification Status. For each thread that is new we send a new notification. 0 is not notified, 1 is NotificationSent, 2 is NotificationRead
        'We send a notification on initial start up if the notification is sent but not read.
        Dim Notifications As New ArrayList
        Notifications = WHLClasses.MySql.SelectData("SELECT * FROM whldata.messenger_threads WHERE (participantid = '" + EmployeeID.ToString + "') ORDER BY idmessenger_threads DESC ;")
        For Each Thread As ArrayList In Notifications
            If Convert.ToInt32(Thread(3)) = 0 Then
                SendNotification(Thread(1), EmployeeID)
                WHLClasses.MySql.insertUpdate("UPDATE whldata.messenger_threads set Notified='1' WHERE threadid ='" + Thread(1).ToString + "';")
            End If
        Next


    End Sub
    Protected Sub Notifications_Tick(sender As Object, e As EventArgs) Handles Notifications.Tick
        Dim UserNameReplaced As String = My.User.Name.Replace("AD\", "")
        EmployeeID = EmpCol.FindEmployeeByADUser(UserNameReplaced).PayrollId
        If Session("BrowserNotifications") = True Then
            GetNotifications(EmployeeID)
        End If

        ListThreadUsers(Convert.ToInt32(Session("ActiveThreadID")), NotificationPanel)
    End Sub
    Public Sub SetNotificationStatus(Notified As Integer, ThreadID As Integer, EmployeeID As Integer)
        Dim responseInsert As Object = WHLClasses.MySql.insertUpdate("UPDATE whldata.messenger_threads set Notified='" + Notified.ToString + "' WHERE threadid ='" + ThreadID.ToString + "' AND participantid='" + EmployeeID.ToString + "';")
    End Sub
    Public Sub ProcessRemoveButton(Sender As Object, e As Object)
        Dim RemoveButtonString As String = Sender.ID
        RemoveButtonString = RemoveButtonString.Replace("Remove", "")
        RemoveFromThread(Convert.ToInt32(Session("ActiveThreadID")), RemoveButtonString)
    End Sub
    Public Sub RemoveFromThread(ThreadID, RemovedID)
        If CheckForUserInThread(RemovedID, ThreadID) = True Then
            Dim responseInsert As Object = WHLClasses.MySql.insertUpdate("DELETE FROM whldata.messenger_threads WHERE ThreadID='" + ThreadID.ToString + "' AND participantid='" + RemovedID.ToString + "';")
        Else
        End If

    End Sub

    Protected Sub UploadPhoto_Click(sender As Object, e As EventArgs) Handles UploadPhoto.Click
        Dim uTime As Integer
        uTime = (DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds
        If FileUpload1.Visible = True Then
            If FileUpload1.HasFile Then
                Try
                    FileUpload1.SaveAs("C:\Data Storage\Intra\Uploads\" & uTime.ToString &
                   FileUpload1.FileName)
                    TextBox1.Text = "http://apps.ad.whitehinge.com/Uploads/" & uTime.ToString & FileUpload1.FileName


                    Dim theText As String = "http://apps.ad.whitehinge.com/Uploads/" & uTime.ToString & FileUpload1.FileName
                    Dim responseInsert As Object = WHLClasses.MySql.insertUpdate("INSERT INTO whldata.messenger_messages (participantid, messagecontent, timestamp, threadid ) VALUES (" + EmployeeID.ToString + ",'" + theText.ToString + "','" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Session("ActiveThreadID").ToString + ");")
                Catch ex As Exception
                    TextBox1.Text = "ERROR: " & ex.Message.ToString()
                End Try
            Else
                TextBox1.Text = "You have not specified a file."
            End If
        Else
            FileUpload1.Visible = True
        End If


    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ThreadTimer.Enabled = False Then
            ThreadTimer.Enabled = True
        Else
            ThreadTimer.Enabled = False
        End If
        If ContactsTimer.Enabled = False Then
            ContactsTimer.Enabled = True
        Else
            ContactsTimer.Enabled = False
        End If
        If Notifications.Enabled = False Then
            Notifications.Enabled = True
        Else
            Notifications.Enabled = False
        End If
    End Sub
End Class