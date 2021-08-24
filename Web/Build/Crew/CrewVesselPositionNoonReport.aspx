<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewVesselPositionNoonReport.aspx.cs" Inherits="CrewVesselPositionNoonReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Position Noon Report</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselPosition" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuVesselPositionNoonReport" runat="server" OnTabStripCommand="MenuVesselPositionNoonReport_TabStripCommand"></eluc:TabStrip>

            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblMessagetype" runat="server" Text="Message Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ddlMessageType" runat="server" CssClass="input" HardTypeCode="194" AppendDataBoundItems="true" />
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblReportDate" runat="server" Text="Report Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtNoonReportDate" runat="server" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLatitude" runat="server" Text="Latitude"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Latitude ID="ucLatitude" runat="server" CssClass="input" />
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblLongitude" runat="server" Text="Longitude"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Longitude ID="ucLongitude" runat="server" CssClass="input" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblWindDirection" runat="server" Text="Wind Direction"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtWindDirection" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblSeaDirection" runat="server" Text="Sea Direction"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSeaDirection" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblWindForce" runat="server" Text="Wind Force"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucWindForce" runat="server" CssClass="input" DecimalPlace="2" />
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblSeaForce" runat="server" Text="Sea Force"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucSeaForce" runat="server" CssClass="input" DecimalPlace="2" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCurrentDirection" runat="server" Text="Current Direction"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCurrDirection" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblCurrectSpeed" runat="server" Text="Current Speed"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucCurrSpeed" runat="server" CssClass="input" DecimalPlace="2" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSwellDirection" runat="server" Text="Swell Direction"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSwellDirection" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblSwellHeight" runat="server" Text="Swell Height"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSwellHeight" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselCourse" runat="server" Text="Vessel Course"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVesselCourse" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblBallast" runat="server" Text="Ballast"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsBallast" runat="server" Checked='<%#(DataBinder.Eval(Container,"DataItem.FLDISBALLAST").ToString().Equals("1")) %>' />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCPSpeed" runat="server" Text="CP Speed"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCpSpeed" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblAverageSpeed" runat="server" Text="Average Speed"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAverageSpeed" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEngineRPM" runat="server" Text="Engine RPM"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEngineRpm" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblSeaTemperature" runat="server" Text="Sea Temperature °C"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucSeaTempreature" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCPFO" runat="server" Text="CP FO"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucCpFo" runat="server" CssClass="input" DecimalPlace="2" />
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblCPDO" runat="server" Text="CP DO"></telerik:RadLabel>

                        </td>
                        <td>
                            <eluc:Number ID="ucCpDo" runat="server" CssClass="input" DecimalPlace="2" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblActualFO" runat="server" Text="Actual FO"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucActualFo" runat="server" CssClass="input" DecimalPlace="2" />
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblActualDO" runat="server" Text="Actual DO"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucActualDo" runat="server" CssClass="input" DecimalPlace="2" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCurrenctCargo" runat="server" Text="Current Cargo"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCurrentCargo" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Width="250px" TextMode="MultiLine"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
    </form>
</body>
</html>
