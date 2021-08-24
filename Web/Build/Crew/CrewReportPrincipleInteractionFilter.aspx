<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportPrincipleInteractionFilter.aspx.cs" Inherits="Crew_CrewReportPrincipleInteractionFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="VesselList" Src="../UserControls/UserControlVesselList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Principle Interaction Filter</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Filter"></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="PrincipleInteractionFilter" runat="server" OnTabStripCommand="PrincipleInteractionFilter_TabStripCommand">
        </eluc:TabStrip>
        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNewApplicantEntry">
        <ContentTemplate>
            <div id="divfind">
            <table>
            <tr>
                        <td>
                            <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                        </td>
                        <td>
                            <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" CssClass="input"
                                AutoPostBack="true"  AppendDataBoundItems="true"
                                Width="80%" />
                        </td>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <div runat="server" id="dvVessel" class="input" style="overflow: auto; width: 80%;
                                height: 80px">
                                <asp:CheckBoxList runat="server" ID="cblVessel" Height="100%" RepeatColumns="1" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" Width="200px">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
            </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
