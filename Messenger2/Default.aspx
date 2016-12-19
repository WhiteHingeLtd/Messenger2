<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="Messenger2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type ="text/javascript" src="Notifications.js">NotifyMe();</script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_beginRequest(beginRequest);

        function beginRequest()
        {
            prm._scrollPosition = null;
        }
        var threadid = '';
        var employee = '';
        threadid = <%= Session("ActiveThreadID")%>;
        employee = <%= Session("EmployeeID")%>;
        console.log(threadid);
        console.log(employee);
    </script>
    <div style="margin-right: 120px">&nbsp;<table border="0">
    <tbody>
      <tr>
           
        <th scope="row" style="width: 512px; height: 206px;" onload="notifyMe()">
            <asp:Button ID="Contacts" runat="server" Text="Contacts" />
            <asp:Button ID="Threads" runat="server" Text="Threads" />
            <asp:UpdatePanel ID="ContactsPanel" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="ContactsTimer" runat="server" Interval="50" ClientIDMode="AutoID">
                    </asp:Timer>
                </ContentTemplate>
            </asp:UpdatePanel>
          </th>
        <td style="height: 206px; width: 748px;">
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" ClientIDMode="AutoID">
                   <ContentTemplate>
                       <asp:Timer ID="ThreadTimer" runat="server" Interval="50">
                       </asp:Timer>
                   </ContentTemplate>
                   <Triggers>
                   <asp:AsyncPostBackTrigger ControlID="Send" />
                 </Triggers>
            </asp:UpdatePanel>
          </td>

      </tr>
      <tr>
        <th scope="row" style="width: 512px">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel>
          </th>
        <td style="width: 748px">
            <div __designer:mapid="16">
            <asp:TextBox ID="TextBox1" runat="server" Height="23px" Width="424px"></asp:TextBox>
            <asp:Button ID="Send" runat="server" Text="Send" ValidateRequestMode="Disabled" style="margin-bottom: 16" />
                <asp:FileUpload ID="FileUpload1" runat="server" Visible="False" />
<%--                <asp:RegularExpressionValidator 
                 id="RegularExpressionValidator1" runat="server" 
                 ErrorMessage="Only image files are allowed!" 
                 ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))
                    +(.jpg|.JPEG|.jpeg|.JPG|.png|.PNG|.tiff|.tif|.TIFF|.TIF|.gif|.GIF|.bmp|.BMP)$" 
                 ControlToValidate="FileUpload1"></asp:RegularExpressionValidator>--%>
                <asp:Button ID="UploadPhoto" runat="server" Text="Send Photo" Width="107px" />
                <div>
                    <asp:UpdatePanel ID="NotificationPanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Timer ID="Notifications" runat="server" Interval="60" ClientIDMode="AutoID">
                            </asp:Timer>
                            <asp:Label ID="ActiveUsers" runat="server" Text="Active Users:"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
          </td>

      </tr>
    </tbody>
  </table>
</div>
</asp:Content>

