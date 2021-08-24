<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionESIChart.aspx.cs"
    Inherits="VesselPositionESIChart" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Vessel Sign-On</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlNoonReportData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlNoonReportData">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuTab" TabStrip="true" runat="server" OnTabStripCommand="MenuTab_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td width="25%">
                        <br />
                        <div runat="server" id="div3" class="input" style="overflow: auto; height: 160px">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="input" SyncActiveVesselsOnly="True" AppendDataBoundItems="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <telerik:RadCheckBox ID="chkByDate" runat="server" Text="By Date" AutoPostBack="true" OnCheckedChanged="chkByDate_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input" DatePicker="true" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtToDate" runat="server" CssClass="input" DatePicker="true" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <telerik:RadButton ID="btnShowChart" runat="server" Text="View" OnClick="ShowChart" CssClass="input" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblby" runat="server" Text="By"></telerik:RadLabel>
                        <br />
                        <div runat="server" id="divCheckboxList" class="input" style="overflow: auto; height: 160px">
                            <telerik:RadRadioButtonList runat="server" ID="rblday" RepeatDirection="Vertical" AutoPostBack="true"
                                OnSelectedIndexChanged="rbl_SelectedIndexChanged">
                                <Items>
                                    <telerik:ButtonListItem Text="Daily" Value="1" Selected="True"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="Monthly" Value="2"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="Yearly" Value="3"></telerik:ButtonListItem>
                                </Items>
                            </telerik:RadRadioButtonList>
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEsi" runat="server" Text="ESI"></telerik:RadLabel>
                        <br />
                        <div runat="server" id="div1" class="input" style="overflow: auto; height: 160px">
                            <telerik:RadRadioButtonList runat="server" ID="rblParameter"
                                RepeatDirection="Vertical" AutoPostBack="true"
                                OnSelectedIndexChanged="rbl_SelectedIndexChanged">
                                <Items>
                                    <telerik:ButtonListItem Text="CO2 Emission" Value="CO2EMISSION" Selected="True"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="CO2 Index" Value="CO2INDEX"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="EEOI" Value="EEOI"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="ESI_SOx" Value="ESISOX"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="SOx_Indicator" Value="SOXIND"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="NOx_Indicator" Value="NOXIND"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="Overall ESI" Value="OVERALLESI"></telerik:ButtonListItem>
                                </Items>
                            </telerik:RadRadioButtonList>
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoyage" runat="server" Text="Voyage"></telerik:RadLabel>
                        <br />
                        <div runat="server" id="div2" class="input" style="overflow: auto; height: 160px">
                            <telerik:RadRadioButtonList runat="server" ID="rblballastladen"
                                RepeatDirection="Vertical" AutoPostBack="true"
                                OnSelectedIndexChanged="rbl_SelectedIndexChanged">
                                <Items>
                                    <telerik:ButtonListItem Text="Ballast" Value="0"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="Laden" Value="1"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="Both" Value="" Selected="True"></telerik:ButtonListItem>
                                </Items>
                            </telerik:RadRadioButtonList>
                        </div>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Chart ID="ChartESIScores" runat="server" Height="450px" Width="1100px">
                </asp:Chart>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
