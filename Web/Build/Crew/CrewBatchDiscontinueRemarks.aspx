<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBatchDiscontinueRemarks.aspx.cs" Inherits="Crew_CrewBatchDiscontinueRemarks" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Discontinue Remarks</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRemarks" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID ="RadAjaxPane1">
            <eluc:TabStrip ID="MenuRemarks" runat="server" OnTabStripCommand="MenuRemarks_TabStripCommand"></eluc:TabStrip>

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <br />
            <table cellpadding="1" cellspacing="1" >
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Discontinue Remarks"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" Height="50px"
                            Rows="4" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
