<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionQuarterlyReport.aspx.cs"
    Inherits="VesselPositionQuarterlyReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quarterly Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmQuarterlyReport" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlQuarterlyReportData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuQuarterlyReportTap" TabStrip="true" runat="server" OnTabStripCommand="QuarterlyReportTapp_TabStripCommand"></eluc:TabStrip>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>
        <asp:Button runat="server" ID="cmdHiddenSubmits" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadAjaxPanel runat="server" ID="pnlQuarterlyReportData">
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td></td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtCurrentDate" runat="server" CssClass="input" Enabled="false" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSelectPeriod" runat="server" Text="Select Period"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadComboBox ID="ddlPeriod" runat="server" CssClass="input" AutoPostBack="true"
                            OnTextChanged="ddlPeriod_TextChangedEvent">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Jan-Mar" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Apr-Jun" Value="4"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Jul-Sep" Value="7"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Oct-Dec" Value="10"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        &nbsp;&nbsp;
                            <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input" AutoPostBack="true"
                                OnTextChanged="ddlPeriod_TextChangedEvent">
                            </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtFromDate" runat="server" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtToDate" runat="server" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblBallast" runat="server" Text="Ballast"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLoaded" runat="server" Text="Loaded"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTotal" runat="server" Text="Total"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDistanceSteamed" runat="server" Text="Distance Steamed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastdistancesteamed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadeddistancesteamed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotaldistancesteamed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSteamingTime" runat="server" Text="Steaming Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballaststeamingtime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedsteamingtime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalsteamingtime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageSpeed" runat="server" Text="Average Speed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageRPM" runat="server" Text="Average RPM"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgrpm" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgrpm" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgrpm" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageSlip" runat="server" Text="Average Slip"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgslip" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgslip" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgslip" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageFOCons" runat="server" Text="Average FO Cons/Day"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgfoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgfoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgfoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMEStoppageTime" runat="server" Text="M/E Stoppage Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastmestoppagetime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedmestoppagetime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalmestoppagetime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageMEFOCons" runat="server" Text="Average M/E FO Cons/Day"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgmefoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgmefoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgmefoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageAEDOCons" runat="server" Text="Average A/E DO Cons/Day"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgaedoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgaedoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgaedoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageBoilerFOCons" runat="server" Text="Average Boiler FO Cons/Day"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgboilerfoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgboilerfoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgboilerfoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTankCleaning" runat="server" Text="Average FO Cons/Day for Tank Cleaning"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgfoconsumptionperdayfortankcleaning" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgfoconsumptionperdayfortankcleaning" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgfoconsumptionperdayfortankcleaning" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageMECC" runat="server" Text="Average MECC/Day"></telerik:RadLabel>
                    </td>
                    <td>-    
                    </td>
                    <td>-     
                    </td>
                    <td>
                        <eluc:Number ID="txtavgmeccconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageMECYL" runat="server" Text="Average MECYL/Day"></telerik:RadLabel>
                    </td>
                    <td>-     
                    </td>
                    <td>-     
                    </td>
                    <td>
                        <eluc:Number ID="txtavgmecylconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageMECYLLTBN" runat="server" Text="Average MECYLLTBN/Day"></telerik:RadLabel>
                    </td>
                    <td>-    
                    </td>
                    <td>-     
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgmecylltbnperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMECYLSpecificCons" runat="server" Text="MECYL Specific Cons"></telerik:RadLabel>
                    </td>
                    <td>-     
                    </td>
                    <td>-     
                    </td>
                    <td>
                        <eluc:Number ID="txtmecylspecificconsumption" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMECYLLTBNSpecificCons" runat="server" Text="MECYLLTBN Specific Cons"></telerik:RadLabel>
                    </td>
                    <td>-     
                    </td>
                    <td>-     
                    </td>
                    <td>
                        <eluc:Number ID="txttotalmecylltbnspecificconsumption" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAverageAECCCons" runat="server" Text="Average AECC Cons/Day"></telerik:RadLabel>
                    </td>
                    <td>-    
                    </td>
                    <td>-     
                    </td>
                    <td>
                        <eluc:Number ID="txtavgaeccconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="60px" />
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
