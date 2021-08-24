<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewQuickReportContractDetails.aspx.cs"
    Inherits="CrewQuickReportContractDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Search</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCrewSeniorofficer">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="Contract"></eluc:Title>
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div class="subHeader" style="top: 27px; position: absolute;">
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                                TabStrip="false"></eluc:TabStrip>
                        </div>
                    </div>
                    <br />
                    <br />

                    <div id="divFind" style="position: relative; z-index: 2">
                        <div>
                            <table cellpadding="0" cellspacing="0" border="1">
                                <tr valign="top">
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="lblname" runat="server" Text="Name"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtname" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="lblFileNo" runat="server" Text="File No."></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFileNo" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="lblSEAMANBOOKNO" runat="server" Text="Seaman Book No."></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSeamanBookNo" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="lblpassportno" runat="server" Text="Passport No."></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtpassportno" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
                        </div>
                        <div id="divGrid" style="position: relative; overflow: auto; z-index: 0">
                            <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                OnRowDataBound="gvCrew_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                                EnableViewState="false">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblSlNoHeader" runat="server">S.No.</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container,"DataItem.FLDROW") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblEmpCodeHeader" runat="server">File No.</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNameHeader" runat="server">Name</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                            <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPassPortHeader" runat="server">Passport No.</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPassPort" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblSeamanBookNoHeader" runat="server">Seaman Book No</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSeamanBookNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>' />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblRankHeader" runat="server">Rank</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblZoneHeader" runat="server">Zone</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblZONE" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONENAME") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblLastVessel" runat="server">Last Vessel</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastVesselname" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSELNAME") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblLastVesselsignoff" runat="server">Sign-Off</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSignOffDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTSIGNOFFDATE") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPresentVesselHeader" runat="server">Present Vessel</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPresentVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSELNAME") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblSonDateHeader" runat="server">Sign-On</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSignon" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTSIGNONDATE") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblremarksHeader" runat="server">Remarks</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ID="imgToolTip"></asp:ImageButton>
                                            <asp:Label ID="lblremarks" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNOTESDESCRIPTION") %>'
                                                Width="50px"></asp:Label>
                                            <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNOTESDESCRIPTION") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDOAHeader" runat="server">Availability</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDOA" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOA") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblActiveyn" runat="server">Status</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblActive" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Action
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <img id="Img6" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="ContractHistory" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                                CommandName="CONTRACTHISTORY" CommandArgument="<%# Container.DataItemIndex %>"
                                                ID="cmdContractHistory" ToolTip="Contract History"></asp:ImageButton>
                                            <img id="Img7" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Portage Bill History" ImageUrl="<%$ PhoenixTheme:images/45.png %>"
                                                CommandName="PORTAGEBILLHISTORY" CommandArgument="<%# Container.DataItemIndex %>"
                                                ID="cmdPortageBillHistory" ToolTip="Portage Bill History"></asp:ImageButton>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div id="divPage" runat="server" style="position: relative;">
                            <table width="100%" border="0" class="datagrid_pagestyle">
                                <tr>
                                    <td nowrap="nowrap" align="center">
                                        <asp:Label ID="lblPagenumber" runat="server">
                                        </asp:Label>
                                        <asp:Label ID="lblPages" runat="server">
                                        </asp:Label>
                                        <asp:Label ID="lblRecords" runat="server">
                                        </asp:Label>&nbsp;&nbsp;
                                    </td>
                                    <td nowrap="nowrap" align="left" width="50px">
                                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                    </td>
                                    <td width="20px">&nbsp;
                                    </td>
                                    <td nowrap="nowrap" align="right" width="50px">
                                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                    </td>
                                    <td nowrap="nowrap" align="center">
                                        <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                        </asp:TextBox>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                            Width="40px"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
