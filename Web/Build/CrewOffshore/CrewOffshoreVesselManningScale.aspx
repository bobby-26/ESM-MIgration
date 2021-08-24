<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreVesselManningScale.aspx.cs" Inherits="CrewOffshoreVesselManningScale" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmOffshoreVesselManningScale" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:TabStrip ID="CrewQuery" runat="server" TabStrip="true" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>

            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                        </td>
                    </tr>
                </table>
            </div>

            <eluc:TabStrip ID="MenuOffshoreVesselManningScale" runat="server" OnTabStripCommand="MenuOffshoreVesselManningScale_TabStripCommand"></eluc:TabStrip>

            <div id="divGrid" style="position: relative; z-index: 0">
                <%--  <asp:GridView ID="gvVesselManningScale" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowFooter="false" OnRowCreated="gvVesselManningScale_RowCreated"
                    ShowHeader="true" EnableViewState="false" OnRowDataBound="gvVesselManningScale_RowRowDataBound">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselManningScale" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvVesselManningScale_NeedDataSource"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                   OnItemDataBound="gvVesselManningScale_ItemDataBound"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <HeaderStyle Width="102px" />
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Budgeted" Name="Budgeted" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Actual" Name="Actual" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                         <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Variance" Name="Variance" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn HeaderText="No">
                                <HeaderStyle Width="50px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                             
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank" ColumnGroupName="Budgeted">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETEDRANK") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Nationality" ColumnGroupName="Budgeted">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNationality" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREFERREDNATIONALITYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Wages" ColumnGroupName="Budgeted">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWages" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETEDWAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank" ColumnGroupName="Actual">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActualRank" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALRANK") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Nationality" ColumnGroupName="Actual">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActualNationality" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALNATIONALITY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Wages" ColumnGroupName="Actual">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActualWages" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALWAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Variance" ColumnGroupName="Variance">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVariance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Variance %" ColumnGroupName="Variance">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblVariancePercentHeader" runat="server" Text="Variance %"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVariancePercent" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCEPERCENTAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </div>

    </form>
</body>
</html>
