<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreWageTrackerSummary.aspx.cs" Inherits="CrewOffshoreWageTrackerSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Wage Tracker</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmWageTracker" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="CrewTrainingMenu" runat="server" OnTabStripCommand="CrewTrainingMenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <table width="80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportfortheMonthof" runat="server" Text="Report for the Month of :"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                            Filter="Contains" EmptyMessage="Type to select the month" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="January" Value="1" />
                                <telerik:RadComboBoxItem Text="February" Value="2" />
                                <telerik:RadComboBoxItem Text="March" Value="3" />
                                <telerik:RadComboBoxItem Text="April" Value="4" />
                                <telerik:RadComboBoxItem Text="May" Value="5" />
                                <telerik:RadComboBoxItem Text="June" Value="6" />
                                <telerik:RadComboBoxItem Text="July" Value="7" />
                                <telerik:RadComboBoxItem Text="August" Value="8" />
                                <telerik:RadComboBoxItem Text="September" Value="9" />
                                <telerik:RadComboBoxItem Text="October" Value="10" />
                                <telerik:RadComboBoxItem Text="November" Value="11" />
                                <telerik:RadComboBoxItem Text="December" Value="12" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year :"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                            Filter="Contains" EmptyMessage="Type to select the month" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>

                    <td>
                        <eluc:Date ID="txtfromdate" Visible="false" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Closing Date "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtAsOnDate" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnTextChangedEvent="txtAsOnDate_TextChanged" />
                    </td>

                </tr>
            </table>


            <eluc:TabStrip ID="MenuOffshoreWageTracker" runat="server" OnTabStripCommand="MenuOffshoreWageTracker_TabStripCommand"></eluc:TabStrip>


            <%-- <asp:GridView ID="gvWageTracker" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowCreated="gvWageTracker_RowCreated" OnRowCommand="gvWageTracker_RowCommand"
                ShowHeader="true" ShowFooter="true" EnableViewState="false" OnRowDataBound="gvWageTracker_RowDataBound"
                DataKeyNames="FLDVESSELID">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWageTracker" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvWageTracker_NeedDataSource"
                OnItemCommand="gvWageTracker_ItemCommand"
                OnItemDataBound="gvWageTracker_ItemDataBound"

                GroupingEnabled="false" EnableHeaderContextMenu="true"
             
                AutoGenerateColumns="false" ShowFooter="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDVESSELID">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                       
                        <telerik:GridTemplateColumn HeaderText="No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                          <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNo" runat="server" Text='<%# Container.DataSetIndex + 1 %>'></telerik:RadLabel>
                                <%--<telerik:RadLabel ID="lblNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNO") %>'></telerik:RadLabel>                                    --%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reporting Currency">
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReportingCurrencyItem" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Available Budget">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                           <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAvailableBudget" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVAILABLEBUDGETINRPC") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <telerik:RadLabel ID="lblTotalAvailableBudget" runat="server"></telerik:RadLabel>
                                    <%=TotalAvailableBudegt %> </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Actual Wage">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActualWage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALWAGETOTALRPC") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <telerik:RadLabel ID="lblTotalActualWage" runat="server"></telerik:RadLabel>
                                    <%=TotalActualWageTotal%> </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Variance">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                           <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVariance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALVARIANCERPC") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <telerik:RadLabel ID="lblTotalVariance" runat="server"></telerik:RadLabel>
                                    <%=TotalVaraiance%> </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Variance Percentage">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                           <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVariancePercentage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCEPERCENTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <telerik:RadLabel ID="lblTotalVariancePercentage" runat="server"></telerik:RadLabel>
                                    <%=TotalVaraiancePercentage%>% </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                          
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                              
                                <asp:LinkButton runat="server" CommandArgument="<%# Container.DataSetIndex %>" CommandName="WAGETRACKER"
                                    ID="cmdWageTracker"  ImageAlign="AbsMiddle"
                                    Text=".." ToolTip="Show Wage Tracker" >
                                    <span class="icon"><i class="fas fa-list-alt-jd"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
