<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitSubmitForApprovalConfirmation.aspx.cs" Inherits="AccountsVesselVisitSubmitForApprovalConfirmation" %>

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
    <asp:UpdatePanel runat="server" ID="pnlCancellation">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            If Travel advance is required, please fill in Travel Advance form before submitting for approval. 
            <asp:RadioButtonList ID="rblConfirmation" runat="server" RepeatDirection="Vertical" >
                <asp:ListItem Text="Travel Advance Already Requested" Value="1"></asp:ListItem>
                <asp:ListItem Text="Travel Advance Required" Value="3"></asp:ListItem>
                <asp:ListItem Text="Travel Advance Not Required" Value="2"></asp:ListItem>
            </asp:RadioButtonList>
            <div id="div2">
                <p style="margin-left: 80px">
                    <asp:Button ID="btnProceed" runat="server" CssClass="input" 
                        OnClick="cmdProceed_Click" Text="Proceed" Width="70px" />
                </p>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
