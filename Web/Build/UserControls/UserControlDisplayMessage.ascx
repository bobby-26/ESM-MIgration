<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDisplayMessage.ascx.cs"
    Inherits="UserControlDisplayMessage" %>
<asp:Panel runat="server" ID="pnlErrorMessage" HorizontalAlign="Justify" BorderColor="Black" BorderStyle="Double" Width="360px">
    <table width="100%">
        <tr>
            <td valign="top" colspan="2">
                <font size="2"><b>Message</b></font>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <span runat="server" id="spnHeaderMessage"></span><br />
                <span runat="server" id="spnErrorMessage"></span><br />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button runat="server" ID="cmdClose" Text="Close" OnClick="cmdClose_Click" Width="120px" CssClass="input"/>
            </td>
        </tr>
    </table>
</asp:Panel>
