<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWGQuotationAgentDetail.aspx.cs"
    Inherits="CrewWGQuotationAgentDetail" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewTravelQuotationAgentDetail" runat="server" autocomplete="off">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" DestroyOnClose="true"></telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuAgent" runat="server" OnTabStripCommand="MenuAgent_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuTitle" runat="server" Title=""></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
            <telerik:RadLabel ID="lblTitle" Visible="false" runat="server" Text="Quotation"></telerik:RadLabel>
            <telerik:RadLabel ID="lblTravelAgents" runat="server" Text="Suppliers" Font-Bold="true"></telerik:RadLabel>
            <eluc:TabStrip ID="MenuAgentList" runat="server" OnTabStripCommand="MenuAgentList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvAgent" runat="server" EnableViewState="false" Height="35%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvAgent_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvAgent_ItemDataBound"
                OnItemCommand="gvAgent_ItemCommand" ShowFooter="false" OnUpdateCommand="gvAgent_UpdateCommand" OnDeleteCommand="gvAgent_DeleteCommand"
                OnSortCommand="gvAgent_SortCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="3%" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" Checked="true" runat="server" EnableViewState="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supplier" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNeededId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEEDEDID") %>' Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAgentID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>' Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblApproveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALYN") %>' Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkAgentName" CommandArgument='<%# Container.DataSetIndex %>'
                                    runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTNAME") + " - " + DataBinder.Eval(Container,"DataItem.FLDAGENTCODE")%> '></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkAgentNameEdit" CommandArgument='<%# Container.DataSetIndex %>'
                                    runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTNAME") + " - " + DataBinder.Eval(Container,"DataItem.FLDAGENTCODE")%> '></asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Query Sent" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblquerySendDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDQUERYSENTDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quotation Received" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuotationReceivedDateItem" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDQUOTEDRECEIVEDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purchase Order Sent" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPOSentDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDPOSENTDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Amount" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="12%">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblAgentIDEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:UserControlCurrency ID="ucCurrencyEdit" runat="server" AppendDataBoundItems="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'
                                    CssClass="input_mandatory"></eluc:UserControlCurrency>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" HeaderStyle-Width="10%" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdApprove" CommandName="APPROVEANDFINALIZE" ToolTip="Approve" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdSemdMail" CommandName="SENDMAIL" ToolTip="Send PO" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-envelope"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <telerik:RadLabel ID="lbllineitem" runat="server" Text="Items" Font-Bold="true"></telerik:RadLabel>
            <eluc:TabStrip ID="MenuRegistersWorkingGearItem" runat="server" OnTabStripCommand="RegistersWorkingGearItem_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvRegistersworkinggearitem" runat="server" EnableViewState="false" Height="45%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvRegistersworkinggearitem_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvRegistersworkinggearitem_ItemDataBound1"
                OnItemCommand="gvRegistersworkinggearitem_ItemCommand" ShowFooter="false" OnUpdateCommand="gvRegistersworkinggearitem_UpdateCommand"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="FLDWORKINGGEARITEMID" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Working Gear Item" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblworkinggearlineitem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWWGNEEDEDLINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblGearitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblworkinggearlineEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWWGNEEDEDLINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblGearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuantityitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblQuantityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit Price" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuotedPriceItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:MaskNumber ID="txtRQuantityEdit" runat="server" CssClass="input" Width="90px" MaxLength="7"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Price" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTOTALQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblTOTALQuotedPriceEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" HeaderStyle-Width="10%" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"/>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
