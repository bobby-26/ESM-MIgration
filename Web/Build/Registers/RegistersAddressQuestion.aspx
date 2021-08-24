<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAddressQuestion.aspx.cs"
    Inherits="RegistersAddressQuestion" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Question</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuAddressMain" runat="server" OnTabStripCommand="AddressMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuQuestion" runat="server" OnTabStripCommand="MenuQuestion_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <table>
                <tr style="width: 10%">
                    <td>
                        <telerik:RadLabel ID="lblProposedBy" runat="server" Text="Proposed By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtProposedBy" TextMode="MultiLine" CssClass="input" 
                           Height="30px" Width="400px" runat="server"></telerik:RadTextBox>
                    </td>

                    <td style="width: 30%"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReasonforintroducing" runat="server" Text="Reason for introducing"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtIntroducingReason" TextMode="MultiLine" CssClass="input" 
                           Height="30px" Width="400px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOtherAlternativesifany" runat="server" Text="Other Alternatives if any"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOtherAlternative" TextMode="MultiLine" CssClass="input" 
                           Height="30px" Width="400px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel  ID="lblRiskAssociatedifany" runat="server" Text="Risk Associated if any"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRiskAssociated" TextMode="MultiLine" CssClass="input" 
                           Height="30px" Width="400px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarksbySuperintendentFleetManager" runat="server" Text="Remarks by Superintendent/Fleet Manager"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSuperintendentRemarks"  CssClass="input" TextMode="MultiLine"
                            Height="30px" Width="400px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
