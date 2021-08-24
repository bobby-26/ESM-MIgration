<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestFilter.aspx.cs" Inherits="CrewLicenceRequestFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc"  TagName="RankList"  Src="../UserControls/UserControlRankList.ascx"  %>
<%@ Register TagPrefix="eluc" TagName="VesselList"  Src="../UserControls/UserControlVesselCheckBoxList.ascx"  %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

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
    
    
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPlanReliever">
        <ContentTemplate>
            <div class="subHeader">
                <eluc:Title runat="server" ID="ttlTile" Text="Filter" ShowMenu="false"></eluc:Title>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="PlanRelieverFilterMain" runat="server" OnTabStripCommand="PlanRelieverFilterMain_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
                </eluc:Error>
            <div id="divFind">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFlag" runat="server" Text="Flag"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Flag ID="ddlFlag" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                OnTextChangedEvent="ddlFlag_TextChangedEvent" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblVesselList" runat="server" Text="Vessel List"></asp:Literal>
                        </td>
                        <td>
                            <eluc:VesselList ID="ddlVessel" runat="server" CssClass="input"  VesselsOnly="true" AssignedVessel="true" EntityType="VSL" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCrewChangeDate" runat="server" Text="Crew Change Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                            To
                            <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblRankList" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td>
                            <eluc:RankList ID="ucRankList" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard runat="server" ID="ucStatus" CssClass="input" AppendDataBoundItems="true"
                               HardTypeCode="225" ShortNameFilter ="ACA,AFA,CLP,CCL"/>
                        </td>
                        <td colspan="2">
                                <asp:Panel ID="pnlEmp" runat="server" GroupingText="Employee">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                                            </td>
                                            <td>
                                               <asp:TextBox ID="txtName" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblFileNo" runat="server" Text="File No"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFileno" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>                                        
                                    </table>
                                </asp:Panel>
                        </td>                        
                    </tr>
                </table>
            </div>
            <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
