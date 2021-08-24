<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentReportByUnSafeActsConditions.aspx.cs" Inherits="InspectionIncidentReportByUnSafeActsConditions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Unsafe Acts/Conditions</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="divHead" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmUnsafe" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="pnlUN" runat="server">
        <ContentTemplate>
            <eluc:Error ID="ucError" Visible="false" runat="server" Text="" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title Text="S3 - Safety Performance Statistics" ID="ucTitle" runat="server" ShowMenu="true" />
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                            <eluc:TabStrip ID="MenuUnSafeActsConditionsGeneral" TabStrip="true" runat="server" OnTabStripCommand="MenuUnSafeActsConditionsGeneral_TabStripCommand" />
                        </div>
                    </div>
                </div>
                <table id="tblUN" width="100%" runat="server">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFromMonth" runat="server" Text="From">
                            </asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucFromMonth" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                HardTypeCode="55" SortByShortName="true" />
                            <eluc:Quick ID="ddlFromYear" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                QuickTypeCode="55" />
                        </td>
                        <td>
                            <asp:Literal ID="lblToMonth" runat="server" Text="To">
                            </asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucToMonth" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                HardTypeCode="55" SortByShortName="true" />
                            <eluc:Quick ID="ddlToYear" runat="server" QuickTypeCode="55" AppendDataBoundItems="true"
                                CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                       <td>
                        <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="208" AutoPostBack="true" OnTextChangedEvent="ucCategory_TextChanged" />
                        </td>
                        <td>
                            <asp:Literal ID="lblSubCategory" runat="server" Text="Sub-Category"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <div id="divUnsafeSubCategory" runat="server" class="input" style="overflow: auto; height: 100px;
                                width: 400px;">&nbsp;<asp:CheckBox ID="chkSubCategoryAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSubCategoryAll_Changed"
                                    Text="---SELECT ALL---" />
                                <asp:CheckBoxList ID="chkSubCategory" runat="server" RepeatColumns="2">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                       
                    </tr>
                </table>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuUnSafeActsConditions" runat="server" OnTabStripCommand="MenuUnSafeActsConditions_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvUN" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvUN_ItemDataBound" ShowHeader="true"
                        EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUnSafeActsConditionsHeader" runat="server">UnSafe Acts/Conditions Category</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUnsafeSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPeriodHeader" runat="server">Period</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIODNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIncidentCountHeader" runat="server"> Incident Count</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                <center>
                                    <asp:Label ID="lblIncidentCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTCOUNT") %>'></asp:Label>
                                </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
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
                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                </div>
                <br />
                <b><asp:Literal ID="lblUnsafeActs" runat="server" Text="Unsafe Acts/Conditions Comparison"></asp:Literal></b>
                <br />
                <div id="divGridAvgComparison" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvAvg" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAvgPeriodHeader" runat="server">Period</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAvgPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIODNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTotalHeader" runat="server">Total</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <center>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALINCIDENT") %>'></asp:Label>
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate >
                                    <asp:Label ID="lblAvgperShipperMonthHeader" runat="server">Avg. per ship per month</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                <center>
                                    <asp:Label ID="lblAvgpershipperMonth" runat="server"  Text ='<%# DataBinder.Eval(Container,"DataItem.FLDAVGPERSHIPPERMONTH") %>'></asp:Label>
                                </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <eluc:Status runat="server" ID="Status1" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
