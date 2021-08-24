<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsCrewJoinningFBAnalysis.aspx.cs"
    Inherits="CrewReportsCrewJoinningFBAnalysis" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
   </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCrew">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="FeedBack Analysis"></eluc:Title>
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuFeedback" runat="server" OnTabStripCommand="MenuFeedback_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div class="subHeader">
                        <div class="divFloat" style="clear: right">
                            <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="Report_TabStripCommand"
                                TabStrip="false"></eluc:TabStrip>
                        </div>
                    </div>
                    <div id="divFind" style="position: relative; z-index: 2">
                        <div>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblFromDate" runat="server" Text="Sign On Period"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input" />
                                        <asp:Literal ID="lblDash" Text="-" runat="server"></asp:Literal>
                                        <eluc:Date ID="ucToDate" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblManager" runat="server" Text="Manager"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                                            CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblFeedbackStatus" runat="server" Text="Feedback Status"></asp:Literal>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlFeedbackStatus" runat="server" CssClass="input" Width="100px">
                                            <asp:ListItem Text="--All--" Value="DUMMY"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                    </td>
                                    <td>
                                        <div runat="server" id="Divvessel" style="overflow-y: auto; overflow-x: hidden;">
                                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                                VesselsOnly="true" Width="240px" EntityType="VSL" AssignedVessels="true" />
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblvesselType" runat="server" Text="Vessel Type"></asp:Literal>
                                    </td>
                                    <td>
                                        <div runat="server" id="DivvesselType" style="overflow-y: auto; overflow-x: hidden;">
                                            <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="input" Width="240px"  />
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblZone" runat="Server" Text="Zone"></asp:Literal>
                                    </td>
                                    <td>
                                        <div runat="server" id="DivZone" style="overflow-y: auto; overflow-x: hidden;">
                                            <eluc:Zone runat="server" ID="ucZone" AppendDataBoundItems="true" CssClass="input" Width="240px"/>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblPool" runat="server" Text="Pool"></asp:Literal>
                                    </td>
                                    <td>
                                       <%-- <div runat="server" id="DivPool" style="overflow-y: auto; overflow-x: hidden;width:200px">--%>
                                            <eluc:Pool ID="lstPool" runat="server" AppendDataBoundItems="true" CssClass="input" Width="240px" />
                                        <%--</div>--%>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                    </td>
                                    <td>
                                        <div runat="server" id="DivRank" style="overflow-y: auto; overflow-x: hidden;">
                                            <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="navSelect" runat="server" id="divTab1" style="position: relative; width: 15px">
                                <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
                            </div>
                            <asp:GridView ID="gvFB" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvFB_OnRowDataBound">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblEmployeeHeader" runat="server" Text="File No"> </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>'></asp:Label>
                                            <asp:Label ID="lblEmployeeID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Height="5px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNameHeader" runat="server" Text="Name"> </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Height="5px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblRank" runat="server" Text="Rank"> </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Height="5px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblVesselNameHeader" runat="server" Text="Vessel"> </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Height="5px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
