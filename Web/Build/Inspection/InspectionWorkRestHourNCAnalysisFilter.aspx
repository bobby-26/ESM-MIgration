<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionWorkRestHourNCAnalysisFilter.aspx.cs" Inherits="Inspection_InspectionWorkRestHourNCAnalysisFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOff" Src="~/UserControls/UserControlSignOffReasonList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    <style>
        #ucVessel_divVesselList {
            width: 240px;
        }

        #ucPrincipal_chkboxlist, #ucRank_divRankList {
            overflow-y: hidden !important;
        }
    </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div class="subHeader" style="position: relative">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="divHeading" style="vertical-align: top">
                <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Crew Contract"></asp:Label>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
        </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlNewApplicantEntry">
            <ContentTemplate>
                <div id="divFind">
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblYear" runat="server" Text="Year"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="input" AutoPostBack="false" Style="width: 80px;">
                                </asp:DropDownList>
                            </td>
                          <%--     <td>
                                <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="input" HardTypeCode="81" Width="240px" />
                            </td>--%>
                            <td>
                                <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ucVesselType" runat="server" Width="240px" AppendDataBoundItems="true" CssClass="input" HardTypeCode="81" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="litQuarter" runat="server" Text="Quarter"></asp:Literal>
                            </td>
                            <td>
                                <asp:ListBox ID="lstQuarter" SelectionMode="Multiple" AppendDataBoundItems="true" CssClass="input" Width="240px" Height="70px" runat="server">
                                    <asp:ListItem Value="" Text="--Select--" Selected="False"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Q1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Q2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Q3"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Q4"></asp:ListItem>
                                </asp:ListBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel">
                                </asp:Literal>
                            </td>
                            <td>
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" VesselsOnly="true" Width="240px" />
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="litMonth" runat="server" Text="Month"></asp:Literal>
                            </td>
                            <td>
                                <asp:ListBox ID="lstMonth" SelectionMode="Multiple" AppendDataBoundItems="true" CssClass="input" Width="240px" Height="70px" runat="server">
                                    <asp:ListItem Value="" Text="--Select--" Selected="False"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="July"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                                </asp:ListBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                            </td>
                            <td>
                                <div style="overflow: hidden;">
                                    <eluc:Principal ID="ucPrincipal" runat="server" AppendDataBoundItems="true" CssClass="input" AddressType="128" Width="240px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input" Width="240px" />
                            </td>
                            <td>
                                <asp:Literal ID="lblfleet" runat="server" Text="Fleet"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Fleet ID="ucFleet" runat="server" AppendDataBoundItems="true" CssClass="input" vesselsonly="true" Width="240px" />
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
