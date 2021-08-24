<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenancePostponeRAList.aspx.cs" Inherits="PlannedMaintenancePostponeRAList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HistoryTemplate" Src="~/UserControls/UserControlHistoryTemplateList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maintenance Form Reports</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="formPMHistoryTemplateReports" runat="server">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
            runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuHistoryTemplateGeneral" runat="server"
                    TabStrip="True"></eluc:TabStrip>
            </div>
            <div id="divHeading">
                <eluc:Title runat="server" ID="ucTitle" Text="Risk Assessment Reports for Priority 2,3 Jobs"></eluc:Title>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </div>
        </div>
        <div>
            <table width="15%">
                <tr>
                    <td>
                        <asp:Literal ID="lblFrom" runat="server" Text="From"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblTo" runat="server" Text="To"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                    </td>
                </tr>
            </table>
            <br />
        </div>
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuHistoryTemplateReports" runat="server" OnTabStripCommand="MenuHistoryTemplateReports_TabStripCommand"></eluc:TabStrip>
        </div>
        <asp:GridView ID="gvRATemplate" runat="server" AutoGenerateColumns="False"
            Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false"
            OnRowCommand="gvRATemplate_RowCommand" OnSorting="gvRATemplate_Sorting"
            AllowSorting="true" OnRowDataBound="gvRATemplate_RowDataBound">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <RowStyle Height="10px" />
            <Columns>
                <asp:TemplateField HeaderText="SI. No.">
                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20px"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblserialno" runat="server">S.No</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkTemplateNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDWORKORDERNUMBER"
                            Visible="true" ForeColor="White">Workorder Number&nbsp;</asp:LinkButton>
                        <img id="FLDWORKORDERNUMBER" runat="server" visible="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblWorkorderName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER")%>'></asp:Label>
                        <asp:Label ID="lblReportId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREPORTID")%>'></asp:Label>
                        <asp:Label ID="lblRAReportID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWRRAREPORT")%>'></asp:Label>
                        <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblReportDate" runat="server">Report Date</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <eluc:ToolTip ID="uclblReportDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREPORTDATE")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkReportDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDAPPROVEDATE"
                            ForeColor="White">Approve Date&nbsp;</asp:LinkButton>
                        <img id="FLDREPORTDATE" runat="server" visible="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblApproveDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDAPPROVEDATE"))%>'> </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblApprovedByHeader" runat="server" Text="Approved By"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblApprovedByItem" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAPPROVERNAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblActionHeaderDownload" runat="server">Action</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/download_1.png %>"
                            CommandName="DOWNLOAD" CommandArgument='<%# Container.DataItemIndex %>' ID="DOWNLOAD"
                            ToolTip="DownLoad"></asp:ImageButton>
                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                            width="3" />
                        <asp:ImageButton runat="server" Visible="false" ImageUrl="<%$ PhoenixTheme:images/45.png %>"
                            CommandName="MAPPING" CommandArgument='<%#Container.DataItemIndex %>' ID="CmdMapping"
                            ToolTip="Map To Work Order"></asp:ImageButton>
                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                            width="3" />
                        <asp:ImageButton Visible="false" runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                            ToolTip="Delete"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div id="divPage" style="position: relative;">
            <table width="100%" border="0" class="datagrid_pagestyle">
                <tr>
                    <td nowrap="nowrap" align="center">
                        <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                        <asp:Label ID="lblPages" runat="server"> </asp:Label>
                        <asp:Label ID="lblRecords" runat="server"> </asp:Label>
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
                        <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                            Width="40px"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
