<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPEARSRARiskLevelAdd.aspx.cs" Inherits="InspectionPEARSRARiskLevelAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Level</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuRARiskLevelAdd" runat="server" OnTabStripCommand="MenuRARiskLevel_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td Width="38%">
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code" ></telerik:RadLabel>
                    </td>
                    <td >
                        <telerik:RadTextBox ID="txtCode" runat="server" CssClass="input_mandatory" Text="" Width="70px" MaxLength="5"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRiskLevel" runat="server" Text="Risk Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input_mandatory" Width="250px" MaxLength="100"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRange" runat="server" Text="Range"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucMinRange" runat="server" IsInteger="true" IsPositive="true" MaxLength="3" Width="50px" CssClass="input_mandatory" Tooltip="Minimum" />
                        to
                        <eluc:Number ID="ucMaxRange" runat="server" IsInteger="true" IsPositive="true" MaxLength="3" Width="50px" CssClass="input_mandatory" Tooltip="Maximum" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblColor" runat="server" Text="Color"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadColorPicker RenderMode="Lightweight" runat="server"
                            SelectedColor="#FFFFFF" ID="txtColorAdd" ShowIcon="true" Preset="Web216" CssClass="input_mandatory">
                        </telerik:RadColorPicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="350px" Height="70px" Resize="Both" CssClass="input_mandatory" MaxLength="500"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                         <telerik:RadCheckBox ID="cbActiveyn" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
