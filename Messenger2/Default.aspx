<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="Messenger2._Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Messenger</h1>
        <p class="lead">First build of the messenger test platform.</p>
    </div>

    <div>&nbsp;<table width="1488" height="324" border="0">
    <tbody>
      <tr>
        <th scope="row" style="width: 512px">
            <asp:Label ID="ThreadList" runat="server" Text="Conversations"></asp:Label>
          </th>
        <td>&nbsp;</td>

      </tr>
      <tr>
        <th scope="row" style="width: 512px">
            <asp:Panel ID="ThreadPanel" runat="server">
            </asp:Panel>
          </th>
        <td>
            <asp:TextBox ID="TextBox1" runat="server" Height="23px" Width="424px"></asp:TextBox>
            <asp:Button ID="Send" runat="server" Text="Send" ValidateRequestMode="Disabled" />
          </td>

      </tr>
    </tbody>
  </table>
</div>
</asp:Content>

