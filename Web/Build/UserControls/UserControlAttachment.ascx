<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAttachment.ascx.cs"
    Inherits="UserControls_UserControlAttachment" %>

<table width="100%" runat="server" style="border-width: thin; border-style: solid;">
    <tr>
        <td align="left">
            To include an attachment, click Browse, and then select the file, After you have
            selected the file, click Attach. The file will be uploaded to your message.
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblAttachments" runat="server" Text="Attachments :"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%-- <asp:CheckBox ID="chkFile1" runat="server" Checked="true" /></span>--%>
            <asp:FileUpload ID="fileUpload1" runat="server" Width="650px" CssClass="input" />
        </td>
    </tr>
    <tr>
        <td>
            <%--<asp:CheckBox ID="chkFile2" runat="server"  Checked="true"/>--%>
            <asp:FileUpload ID="fileUpload2" runat="server" Width="650px" CssClass="input" />
        </td>
    </tr>
    <tr>
        <td>
            <%--<asp:CheckBox ID="chkFile3" runat="server"  Checked="true" />--%>
            <asp:FileUpload ID="fileUpload3" runat="server" Width="650px" CssClass="input" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnAttach" Text="Attach" runat="server" OnClick="btnAttach_Click" />            
        </td>
    </tr>
</table>
