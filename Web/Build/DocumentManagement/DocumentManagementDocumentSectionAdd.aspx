<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentSectionAdd.aspx.cs" Inherits="DocumentManagement_DocumentManagementDocumentSectionAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="MenuDocumentSectionGeneral" TabStrip="false" runat="server" OnTabStripCommand="MenuDocumentSectionGeneral_TabStripCommand"></eluc:TabStrip>
            <table width="80%">
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="lblSectionNo" runat="server" Text="Section Number"></telerik:RadLabel>
                    </td>
                    <td width="80%s">
                        <telerik:RadTextBox ID="txtSectionNumber" runat="server" CssClass="input_mandatory" Width="60px" onkeypress="return isNumberKey(event)" MaxLength="50">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSectionName" runat="server" Text="Section Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSectionName" runat="server" CssClass="input_mandatory" MaxLength="300" Width="350px" Height="50px" Resize="Both" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActive" runat="server" Text="Active"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkActiveYN" runat="server" Checked="true"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr id="trstatus" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox RenderMode="Lightweight" ID="ddlRevisionStatus" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" CssClass="input" OnSelectedIndexChanged="ddlRevisionStatus_SelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Preparation" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Pending Approval" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Approved" Value="3"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
