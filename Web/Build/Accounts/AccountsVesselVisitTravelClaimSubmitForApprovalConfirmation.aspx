<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitTravelClaimSubmitForApprovalConfirmation.aspx.cs"
    Inherits="AccountsVesselVisitTravelClaimSubmitForApprovalConfirmation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Visit</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    &nbsp;&nbsp;&nbsp;
    <asp:UpdatePanel runat="server" ID="pnlCancellation" >
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td valign="top">
                        <font size="2"><b>Confirmation</b></font>
                    </td>
                </tr>
            </table>
            <table width="50%">
                <tr>
                    <td >
                        <font color='#0000CC'><b>Please Confirm that cash take from vessel is
                            <asp:Literal ID="lblamount" runat="server"></asp:Literal></b></font><br />
                    </td>
                </tr>
            </table>
            <div id="div3">
                <p style="margin-left: 80px">
                    <asp:Button ID="btnYes" runat="server" CssClass="input" OnClick="cmdYes_Click" Text="Yes"
                        Width="45px" />
                    <asp:Button ID="btnNo" runat="server" CssClass="input" OnClick="cmdNo_Click" Text="No"
                        Width="45px" />
                </p>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
