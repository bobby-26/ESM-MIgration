<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCExtentionApproval.aspx.cs"
    Inherits="InspectionMOCExtentionApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MOC Extention Approval Remarks</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmApprovalRemarks" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblGeneric" runat="server"
        DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%" EnableAJAX="false">
        <eluc:TabStrip ID="MenuApprovalRemarks" runat="server" OnTabStripCommand="MenuApprovalRemarks_TabStripCommand">
        </eluc:TabStrip>
        <table id="tblGeneric" runat="server" cellpadding="1" cellspacing="1" width="75%">
            <tr>
                <td colspan="2">
                    <b>
                        <telerik:RadLabel ID="lblNote" runat="server" Text="* Note: Once the RA is Approved/Rejected, it cannot be edited."
                            Visible="false">
                        </telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td width="10%">
                    <b>
                        <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text="Remarks">
                        </telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <asp:TextBox ID="txtApprovalRemarks" runat="server" CssClass="input_mandatory" Height="70px"
                        Rows="5" TextMode="MultiLine" Width="60%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"
                        Font-Size="Small">
                    </telerik:RadLabel>
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
