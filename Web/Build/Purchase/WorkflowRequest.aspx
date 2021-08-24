<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowRequest.aspx.cs" Inherits="WorkflowRequest" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOrder" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblSearch">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" OnTextChangedEvent="UcVessel_TextChangedEvent" AutoPostBack="true" VesselsOnly="true" AppendDataBoundItems="true" />
                    </td>

                </tr>
            </table>
            <eluc:TabStrip ID="MenuWFOrderList" runat="server" OnTabStripCommand="MenuWFOrderList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWFOrderList" runat="server" CellSpacing="0" GridLines="None" Height="88%" EnableHeaderContextMenu="true"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" GroupingEnabled="false" EnableViewState="false"
                OnNeedDataSource="gvWFOrderList_NeedDataSource" OnItemCommand="gvWFOrderList_ItemCommand" OnItemDataBound="gvWFOrderList_ItemDataBound">

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
                        <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="VESSEL" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblOrderId" Text='<%#DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Stock Type" UniqueName="STOCKTYPE" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockType" runat="server" Text='<% #DataBinder.Eval(Container, "DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Form No" UniqueName="FORMNO" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblFormNo" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Title" UniqueName="TITLE" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblOrderTitle" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Form Type" UniqueName="FORMTYPE" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblFormType" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            
                                 <asp:LinkButton runat="server" AlternateText="Action" CommandName="ACTION"  ID="cmdActivity" ToolTip="Action">
                                <span class="icon"><i class="fa fa-pencil-ruler"></i></span>
                                </asp:LinkButton>
                            
                                <asp:LinkButton runat="server" AlternateText="Request" CommandName="REQUEST" ID="cmdRequest" ToolTip="Request">
                                     <span class="icon"><i class="fas fa-sign-in-alt"></i></span>
                                </asp:LinkButton>                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>


                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" EnableAllOptionInPagerComboBox="true" />
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
