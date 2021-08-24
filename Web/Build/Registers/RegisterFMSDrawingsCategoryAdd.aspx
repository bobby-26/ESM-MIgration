<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterFMSDrawingsCategoryAdd.aspx.cs" Inherits="RegisterFMSDrawingsCategoryAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Drawing Category Add</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersManualSubCategoryAdd" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuFMSManualSubCategoryAdd" runat="server" OnTabStripCommand="MenuFMSManualSubCategoryAdd_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <br />
            <table runat="server">
                <tr>
                    <td style="padding-right: 30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lblSortno" runat="server" Text="Sort Order"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 30px">
                        <telerik:RadTextBox ID="txtSortNo" runat="server" CssClass="gridinput_mandatory" InputType="Number"
                            MaxLength="3" Width="270px">
                        </telerik:RadTextBox>
                    </td>
                </tr>               
                <tr>
                    <td style="padding-right: 30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lblsubcategoryCode" runat="server" Text="Drawing Code"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 30px">
                        <telerik:RadTextBox ID="txtCodeAdd" runat="server" CssClass="gridinput_mandatory"
                             Width="270px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lblsubcategoryname" runat="server" Text="Drawing Name"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 30px">
                        <telerik:RadTextBox ID="txtSubCategoryAdd" runat="server" CssClass="gridinput_mandatory"
                            Width="270px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

