<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseSpareMaintenance.aspx.cs" Inherits="PurchaseSpareMaintenance" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spare Required Maintance</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table id="tblSearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" OnTextChangedEvent="UcVessel_TextChangedEvent" AutoPostBack="true" CssClass="input_mandatory" VesselsOnly="true" Width="180px" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDueDays" runat="server" Text="Maintenance Due (Days)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDueDays" runat="server" Text="" IsInteger="true" MaxLength="2" CssClass="input_mandatory" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblitemnumber" runat="server" Text="Part Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtitemnumber" runat="server" Text=""></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblname" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtname" runat="server" Text="" Width="220px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuSpareMain" runat="server" OnTabStripCommand="MenuSpareMain_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSpareMaintance" runat="server"
                CellSpacing="0" GridLines="None" Height="90%" EnableHeaderContextMenu="true"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" GroupingEnabled="false" EnableViewState="false"
                OnNeedDataSource="gvSpareMaintance_NeedDataSource">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">

                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <Columns>
                         <telerik:GridTemplateColumn HeaderText="Job Number" UniqueName="JOBNUMBER" SortExpression="FLDJOBNO">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbljobnumber" runat="server" Text='<% #DataBinder.Eval(Container, "DataItem.FLDJOBNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Part Number" UniqueName="NUMBER" SortExpression="FLDNUMBER">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblitemnumber" runat="server" Text='<% #DataBinder.Eval(Container, "DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME" SortExpression="FLDNAME">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="True" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblSpareitemid" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblname" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Maker Reference" UniqueName="REFERENCE" SortExpression="FLDMAKERREFERENCE">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMakereference" runat="server" Text='<% #DataBinder.Eval(Container, "DataItem.FLDMAKERREFERENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Required Quantity">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblQuantity" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="ROB" UniqueName="ROB">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle  HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblROB" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDROBQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Lead Time">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="false" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblLeadtime" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEADTIME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">

                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>

            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
