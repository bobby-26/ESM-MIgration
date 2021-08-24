<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationDeliveryInstructionBulk.aspx.cs" Inherits="PurchaseQuotationDeliveryInstructionBulk" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delivery Instruction</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmDeliveryInstruction" runat="server">    
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuDelInstruction">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuDelInstruction" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
    
    

    <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
        <div style="font-weight:600;font-size:12px" runat="server">
            <eluc:TabStrip ID="MenuDelInstruction" runat="server" OnTabStripCommand="MenuDelInstruction_TabStripCommand">
            </eluc:TabStrip>
        </div>
        
        <br clear="all" />
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                  <telerik:RadLabel RenderMode="Lightweight" ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel>
                </td>
                <td>
                    <%--<eluc:Date ID="txtETA" runat="server" CssClass="input" />--%>
                    <telerik:RadDateTimePicker RenderMode="Lightweight" ID="txtETA" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm" 
                        DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm" Width="160px">

                    </telerik:RadDateTimePicker>
                    <%--<asp:TextBox ID="txtETATime" runat="server" CssClass="input" Width="50px" /> --%> 
                    <%--<telerik:RadMaskedTextBox RenderMode="Lightweight" ID="txtETATime" runat="server" Width="50px" Mask="<0..24>:<0..60>" />--%>
                </td>
                <td>
                  <telerik:RadLabel RenderMode="Lightweight" ID="lblETD" runat="server" Text="ETD"></telerik:RadLabel>
                    
                </td>
                <td>
                    <telerik:RadDateTimePicker RenderMode="Lightweight" ID="txtETD" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm" 
                        DateInput-DisplayDateFormat="dd/MM/yyyy HH:mm" Width="160px"></telerik:RadDateTimePicker>
                    <%--<eluc:Date ID="txtETD" runat="server" CssClass="input" />--%>
                    <%--<telerik:RadMaskedTextBox  RenderMode="Lightweight" ID="txtETDTime" runat="server" Width="50px" Mask="<0..24>:<0..60>" />--%>
                    <%--<asp:TextBox ID="txtETDTime" runat="server" CssClass="input" Width="50px" />  
                    <ajaxToolkit:MaskedEditExtender ID="txtETDTimeMask" runat="server" TargetControlID="txtETDTime"
                        Mask="99:99" MaskType="Time"  ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" AcceptAMPM="false"
                        UserTimeFormat="TwentyFourHour"/>--%>
                </td>
            </tr>
            <tr>
                <td>
                  <telerik:RadLabel RenderMode="Lightweight" ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    
                </td>
                <td colspan="3">
                    <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input" Width="400px"/>
                </td>
            </tr>
            <tr>
                <td>
                  <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryInstruction" runat="server" Text="Delivery Instruction"></telerik:RadLabel>
                    
                </td>
                <td colspan="3">
                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtDeliveryInstruction" CssClass="input_mandatory" TextMode="MultiLine"
                     Width="100%" Height="200px"></telerik:RadTextBox>
                </td>
            </tr>
             
        </table>
    </div>
    </form>
</body>
</html>
