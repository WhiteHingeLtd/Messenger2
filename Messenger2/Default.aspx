<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="Messenger2._Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div>&nbsp;<table width="1488" height="324" border="0">
    <tbody>
      <tr>
        <th scope="row" style="width: 512px; height: 206px;">
            <asp:Timer ID="Timer1" runat="server" Interval="5000">
            </asp:Timer>
            <asp:Label ID="ThreadList" runat="server" Text="Conversations"></asp:Label>
            <asp:Button ID="Contacts" runat="server" Text="Contacts" />
            <asp:Button ID="Threads" runat="server" Text="Threads" />
            <asp:Panel ID="ThreadPanel" runat="server">
            </asp:Panel>
          </th>
        <td style="height: 206px">
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto">
            </asp:Panel>
          </td>

      </tr>
      <tr>
        <th scope="row" style="width: 512px">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            </asp:UpdatePanel>
          </th>
        <td>
            <div __designer:mapid="16">
            <asp:TextBox ID="TextBox1" runat="server" Height="23px" Width="424px"></asp:TextBox>
            <asp:Button ID="Send" runat="server" Text="Send" ValidateRequestMode="Disabled" style="margin-bottom: 16" />
            </div>
          </td>

      </tr>
    </tbody>
  </table>
</div>
</asp:Content>

