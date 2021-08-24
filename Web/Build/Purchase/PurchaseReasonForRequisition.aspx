<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReasonForRequisition.aspx.cs" Inherits="PurchaseReasonForRequisition" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reason for requisition..</title>    
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>      
</head>
<body>
    <form id="frmPurchaseFormDetail" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuFormDetail">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuFormDetail" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuFormDetail" runat="server" OnTabStripCommand="MenuFormDetail_TabStripCommand" Title="Reason for Requisition">
            </eluc:TabStrip>
        </div>
    <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%;">                 
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <table width="100%">
            <tr>
                <td style="width: 10%;">
                  <telerik:RadLabel ID="lblReason" runat="server" Text="Reason"></telerik:RadLabel>
                </td>
                <td style="width: 90%;">
                    <%--<asp:TextBox runat="server" ID="txtReasonForReq" CssClass="input_mandatory" TextMode="MultiLine"
                        Width="100%" Height="200px"></asp:TextBox>--%>
                    <telerik:RadTextBox RenderMode="Lightweight" Width="90%" ID="txtReasonForReq" runat="server" TextMode="MultiLine" Resize="Both"
                        EmptyMessage="Type Reason for Requisition" Height="200px">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>