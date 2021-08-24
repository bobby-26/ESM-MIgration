<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionChart.aspx.cs" Inherits="VesselPositionChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByUserType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voyage</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            #myBtn {
                display: none;
                position: fixed;
                bottom: 20px;
                right: 30px;
                z-index: 99;
                font-size: 18px;
                border: none;
                outline: none;
                background-color: cornflowerblue;
                color: white;
                cursor: pointer;
                padding: 15px;
                border-radius: 4px;
            }

                #myBtn:hover {
                    background-color: #555;
                }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <script>
        // When the user scrolls down 20px from the top of the document, show the button
        window.onscroll = function () { scrollFunction() };

        function scrollFunction() {
            if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
                document.getElementById("myBtn").style.display = "block";
            } else {
                document.getElementById("myBtn").style.display = "none";
            }
        }

        // When the user clicks on the button, scroll to the top of the document
        function topFunction() {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
        }
    </script>
    <button onclick="topFunction()" id="myBtn" title="Go to top">Top</button>
    <form id="frmVoyage" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVoyageList" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlVoyageList">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblmVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="UcVessel" runat="server" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" VesselsOnly="true" SyncActiveVesselsOnly="True" AssignedVessels="true" />
                        </td>

                        <td>
                            <telerik:RadLabel ID="lblVesselStatus" runat="server" Text="Vessel Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlVesselStatus" CssClass="input" runat="server">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="In Port" Value="INPORT"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="At Anchor" Value="ATANCHOR"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="At Sea" Value="ATSEA"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblreporttype" runat="server" Text="Report Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlreporttype" CssClass="input" runat="server">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Weekly" Value="WEEKLY"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Monthly" Value="MONTHLY"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Quartely" Value="QUARTELY"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Half Yearly" Value="HALF YEARLY"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblfromdate" runat="server" Text="Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="FromDate" runat="server" CssClass="input" />
                            <eluc:Date ID="ToDate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                </table>
                <eluc:TabStrip ID="MenuVoyageList" runat="server" OnTabStripCommand="VoyageList_TabStripCommand"></eluc:TabStrip>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Chart ID="ChartSlip" runat="server" Height="450px" Width="1000">
                            </asp:Chart>
                        </td>
                        <td>
                            <asp:Button ID="btnslip" runat="server" Text="Export To PDF"
                                OnClick="btnslip_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Chart ID="ConsumtionAnalysis" runat="server" Height="450px" Width="1000">
                            </asp:Chart>
                        </td>
                        <td>
                            <asp:Button ID="btnconsanalysis" runat="server" Text="Export To PDF"
                                OnClick="btnconsanalysis_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Chart ID="MainEngileTempAnalysis" runat="server" Height="450px" Width="1000">
                            </asp:Chart>
                        </td>
                        <td>
                            <asp:Button ID="btnMainEngineanlysis" runat="server" Text="Export To PDF"
                                OnClick="btnMainEngineanlysis_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Chart ID="MainEngineScavePressureAnalysys" runat="server" Height="450px" Width="1000">
                            </asp:Chart>
                        </td>
                        <td>
                            <asp:Button ID="btnScavepressureanalysis" runat="server" Text="Export To PDF"
                                OnClick="btnScavepressureanalysis_Click" />
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:Chart ID="SpeedAnalysis" runat="server" Height="450px" Width="1000">
                            </asp:Chart>
                        </td>
                        <td>
                            <asp:Button ID="btnspeedanalysis" runat="server" Text="Export To PDF"
                                OnClick="btnspeedanalysis_Click" />
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:Chart ID="PowerVSMEConsumption" runat="server" Height="450px" Width="1000">
                            </asp:Chart>
                        </td>
                        <td>
                            <asp:Button ID="btnPowerVSMEConsumption" runat="server" Text="Export To PDF"
                                OnClick="btnPowerVSMEConsumption_Click" />
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:Chart ID="SludgeTank" runat="server" Height="450px" Width="1000">
                            </asp:Chart>
                        </td>
                        <td>
                            <asp:Button ID="btnSludgeTank" runat="server" Text="Export To PDF"
                                OnClick="btnSludgeTank_Click" />
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:Chart ID="FreshWaterProd" runat="server" Height="450px" Width="1000">
                            </asp:Chart>
                        </td>
                        <td>
                            <asp:Button ID="btnFreshWaterProd" runat="server" Text="Export To PDF"
                                OnClick="FreshWaterProd_Click" />
                        </td>
                    </tr>
                </table>

                <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
