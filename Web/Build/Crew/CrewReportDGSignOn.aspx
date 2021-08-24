<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportDGSignOn.aspx.cs"
    Inherits="Crew_CrewReportDGSignOn" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="../UserControls/UserControlNationality.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DG Shipping Sign On</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
             <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           


                <eluc:TabStrip ID="CrewMenu" runat="server"  OnTabStripCommand="CrewMenu_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>

                <eluc:TabStrip ID="CrewMenuGeneral" Title="Sign on Across Fleet" runat="server" OnTabStripCommand="CrewMenuGeneral_TabStripCommand"></eluc:TabStrip>

                <b style="color: Blue;">
                    <telerik:RadLabel ID="ltrText" runat="server" Text='This report confirms to the DG shipping import of S/on,S/off data required to be updated into their website'></telerik:RadLabel>
                </b>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblManager" runat="server" Text="Manager"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" Width="200px" />
                            </td>
                            <td colspan="2">
                                <asp:Panel ID="pnlPeriod" runat="server" GroupingText="Period" Width="100%">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblFromDate" Text="From Date" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true"  Width="200px"/>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Principal ID="ucPrinicipal" runat="server" AppendDataBoundItems="true" AddressType="128" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Nationality ID="ucNatioanlity" runat="server" AppendDataBoundItems="true" Width="200px" />
                            </td>
                        </tr>
                    </table>
                </div>

                <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

         
                    <%--  <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowDataBound="gvCrew_RowDataBound" OnRowCommand="gvCrew_RowCommand" Width="100%"
                        CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false"
                        AllowSorting="true">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrew_NeedDataSource"
                        OnItemCommand="gvCrew_ItemCommand"
                        OnItemDataBound="gvCrew_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="Date of" Name="date" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="70px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Sr.No">
                                    <HeaderStyle Width="50px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSrNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Seafarer">
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFamilyId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                        <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="INDoS No.">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIndosNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINDOSNO") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="CDC No.">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCdcNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRANKCODE") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNameOfVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel Flag">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDDGFLAGCODE").ToString().ToUpper() == "IM" ? "GBIOM" : DataBinder.Eval(Container, "DataItem.FLDDGFLAGCODE").ToString()%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Contract" ColumnGroupName="date">
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCommencementOfContract" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sign-Off" ColumnGroupName="date">
                                      <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDateOfSignOff" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFSIGNOFF") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Contract Complete/ Arriving India" ColumnGroupName="date">
                                   <HeaderStyle Width="80px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDateOfCOntract" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFCOMPLETION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks">
                                    <HeaderStyle Width="175px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRemarks" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
             

            </div>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
