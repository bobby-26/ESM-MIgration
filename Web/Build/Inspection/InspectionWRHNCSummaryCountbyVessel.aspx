<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionWRHNCSummaryCountbyVessel.aspx.cs" Inherits="Inspection_InspectionWRHNCSummaryCountbyVessel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NC Type Count by Vessel</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCrewReportEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="NC Type Count by Vessel"></eluc:Title>
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                                TabStrip="false"></eluc:TabStrip>
                        </span>
                    </div>
                    <table width="100%" border="1" style="border-collapse: collapse;">
                        <tr valign="top">
                            <td width="25%">
                                <table>
                        </tr>
                    </table>
                    <table cellpadding="2" cellspacing="2">
                        <tr align="top">
                            <%--<td style="display: inline-block;">--%>
                            <td style="width: 55px">
                                <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                                <br />
                                <br />
                                <br />
                                <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                            </td>
                            <td style="width: 35px">
                                <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                                <br />
                                <br />
                                <br />
                                <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblfleet" runat="server" Text="Fleet"></asp:Literal>
                                <eluc:Fleet ID="ucFleet" runat="server" AppendDataBoundItems="true" CssClass="input" vesselsonly="true" Width="100px" />
                            </td>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" VesselsOnly="true" Width="150px" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="div1" runat="server">
                    <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; overflow: auto; z-index: 0; width: 99%">
                        <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            OnRowDataBound="gvCrew_RowDataBound" OnRowCommand="gvCrew_RowCommand" Width="100%"
                            CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                            OnSorting="gvCrew_Sorting">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblVendorName" runat="server">Vessel</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDVESSEL") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblStockType" runat="server">Month</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDPERIOD") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDeliveredOn" runat="server">Total No. of NC`s</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDNCCOUNT") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <div id="divPage" runat="server" style="position: relative; width: 99%">
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
                <br />
                <div id="div2" runat="server">
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="ltGrid" runat="server" Text=""></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
