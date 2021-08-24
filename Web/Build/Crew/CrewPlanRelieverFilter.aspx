<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanRelieverFilter.aspx.cs"
    Inherits="Crew_CrewPlanRelieverFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Src="../UserControls/UserControlNationalityList.ascx" TagName="UserControlNationalityList"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRankList.ascx" TagName="UserControlRankList"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlVesselList.ascx" TagName="UserControlVesselList"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlEngineTypeList.ascx" TagName="UserControlEngineTypeList"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlZoneList.ascx" TagName="UserControlZoneList"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlVesselTypeList.ascx" TagName="UserControlVesselTypeList"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlPoolList.ascx" TagName="UserControlPoolList"
    TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Applicant Query Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <eluc:Title runat="server" ID="ttlTile" Text="Filter" ShowMenu="false"></eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="PlanRelieverFilterMain" runat="server" OnTabStripCommand="PlanRelieverFilterMain_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlPlanReliever">
            <ContentTemplate>
                <div id="divFind">
                    <table cellpadding="2" cellspacing="2">
                        <tr>
                            <td>
                                <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:UserControlVesselTypeList ID="ucVslType" runat="server" CssClass="input" />
                            </td>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td>
                                <eluc:UserControlRankList ID="ucRank" runat="server" CssClass="input" />
                            </td>
                            <td>
                                <asp:Literal ID="lblEngineType" runat="server" Text="Engine Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:UserControlEngineTypeList ID="ucEngType" runat="server" CssClass="input" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVesselexperience" runat="server" Text="Vessel Type Experience (in Months)"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtVslTypeExperience" runat="server" CssClass="input txtNumber"
                                    MaxLength="3" />
                            </td>
                            <td>
                                <asp:Literal ID="lblRankExperience" runat="server" Text="Rank Experience (in Months)"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtRankExperience" runat="server" CssClass="input txtNumber" MaxLength="3" />
                            </td>
                            <td>
                                <asp:Literal ID="lblEngineexperience" runat="server" Text="Engine Type Experience (in Months)"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtEngTypeExperience" runat="server" CssClass="input txtNumber"
                                    MaxLength="3" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCompanyExperience" runat="server" Text="Company Experience (in Months)"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtCompanyExperience" runat="server" CssClass="input txtNumber"
                                    MaxLength="3" />
                            </td>
                            <%--<tr>
                        <td>
                            <asp:CheckBox ID="chkEngType" runat="server" OnCheckedChanged="chkEngType_CheckedChanged" AutoPostBack="true" />
                            &nbsp; Same Engine Type in the Rank of
                        </td>
                        <td>
                            <eluc:UserControlRankList ID="ucEngTypeRank" runat="server" CssClass="readonlytextbox" Enabled="false" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkPool" runat="server" />
                            &nbsp;Same Pool
                        </td>
                    </tr>--%>
                            <%--<tr>
                        <td>
                            <asp:CheckBox ID="chkAdditionalRank" runat="server" OnCheckedChanged="chkAdditionalRank_CheckedChanged" AutoPostBack="true" />
                            &nbsp; Include additionally the Ranks
                        </td>
                        <td>
                            <eluc:UserControlRankList ID="ucAdditionalRank" runat="server" CssClass="readonlytextbox" Enabled="false" />
                        </td>
                        
                    </tr>--%>
                            <td>
                                <asp:Literal ID="lblSameNationality" runat="server" Text="Same Nationality"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkNationality" runat="server" />
                            </td>
                            <%-- <td>
                            Suitable by Documents
                       </td>
                        <td>
                            <asp:CheckBox ID="chkDocuments" runat="server" />                           
                        </td>--%>
                            <td>
                                <asp:Literal ID="lblIncludePlanned" runat="server" Text="Include Planned Persons"></asp:Literal>
                            </td>
                            <td colspan="2">
                                <asp:CheckBox ID="chkPlanned" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                a<asp:Literal ID="lblDOAFrom" Text="DOA From" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtDOAFromDate" runat="server" CssClass="input" />
                            </td>
                            <td>
                                <asp:Literal ID="lblDOATo" Text="DOA To" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtDOAFromTo" runat="server" CssClass="input" />
                            </td>
                             <td>
                                <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Principal ID="ucPrinicpal" runat="server" AddressType="128" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
