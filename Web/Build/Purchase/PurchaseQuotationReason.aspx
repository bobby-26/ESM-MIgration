<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationReason.aspx.cs" Inherits="PurchaseQuotationReason" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reasons</title>
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDeliveryInstruction" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCourseListEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <eluc:TabStrip ID="MenuFormGeneral" runat="server" OnTabStripCommand="MenuFormGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1" runat="server" id="tbl">
                <tr>
                    <td colspan="2">
                        <h3>
                            <telerik:RadLabel ID="lblMSG" runat="server" ></telerik:RadLabel>
                        </h3>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%">
                        <telerik:RadLabel ID="lblReason" runat="server" Text="Reason"></telerik:RadLabel>
                    </td>
                    <td style="width: 75%">
                        <eluc:Quick ID="ucReason" runat="server" QuickTypeCode="179" />
                    </td>
                </tr>
            </table>
            <div runat="server" id="grid">
                <h3>
                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Please specify the reason for selecting quote higher than the minimum quote" Visible="false"></telerik:RadLabel>
                    <telerik:RadLabel ID="RadLabel2" runat="server" Text=" Reason for selecting quote higher than the minimum quote" Visible="false"></telerik:RadLabel>
                    
                </h3>
                <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false"
                    OnNeedDataSource="rgvLine_NeedDataSource" OnItemDataBound="rgvLine_ItemDataBound" OnItemCommand="rgvLine_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDORDERLINEID,FLDQUOTATIONID,FLDQUOTATIONLINEID" TableLayout="Fixed">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="S.No" UniqueName="SNO">
                                <ItemStyle Width="40px" />
                                <HeaderStyle Width="40px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                                <ItemStyle Width="240px" />
                                <HeaderStyle Width="240px" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkStockItemCode" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDNAME")%>'> </asp:LinkButton><br />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="UNIT">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Price" UniqueName="PRICE">
                                <ItemStyle Width="70px" HorizontalAlign="Right" />
                                <HeaderStyle Width="70px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n3}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Price (USD)" UniqueName="PRICEUSD">
                                <ItemStyle Width="60px" HorizontalAlign="Right" />
                                <HeaderStyle Width="60px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotedPriceUSD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICEUSD","{0:n3}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reason" UniqueName="REASON">
                                <HeaderStyle Width="200px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHIGHERVALUEREASON") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucReasonEdit" runat="server" Visible="false" QuickTypeCode="180" AutoPostBack="false" AppendDataBoundItems="true" CssClass="input" />
                                    <telerik:RadComboBox runat="server" ID="ucReasonEdit1" CssClass="input" DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE"></telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItem%>' ID="cmdEdit"
                                        ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Update" CommandArgument='<%# Container.DataItem %>' ID="cmdUpdate"
                                        ToolTip="Update">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItem%>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                            PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
             <div runat="server" id="divgvnongenuine" visible="false">
                <h3>
                    <telerik:RadLabel ID="radlblnongenuine" runat="server" Text="Please Specify the reason for Non Genuine items in selected quote." Visible="false"></telerik:RadLabel>
                     <telerik:RadLabel ID="RadLabel3" runat="server" Text=" Reason for Non Genuine items in selected quote." Visible="false"></telerik:RadLabel>
                </h3>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvnongenuine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false"
                    OnNeedDataSource="gvnongenuine_NeedDataSource" OnItemDataBound="gvnongenuine_ItemDataBound" OnItemCommand="gvnongenuine_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDORDERLINEID,FLDQUOTATIONID,FLDQUOTATIONLINEID" TableLayout="Fixed">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="S.No" >
                                <ItemStyle Width="40px" />
                                <HeaderStyle Width="40px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="radlblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Number" >
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="radlblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name" >
                                <ItemStyle Width="240px" />
                                <HeaderStyle Width="240px" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="radlnkStockItemCode" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDNAME")%>'> </asp:LinkButton><br />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit" >
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="radlblUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

               
                            <telerik:GridTemplateColumn HeaderText="Reason" UniqueName="REASON">
                                <HeaderStyle Width="200px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="radlblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONOEMREASON") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    
                                    <telerik:RadComboBox runat="server" ID="ucnonoemReasonEdit1" CssClass="input" DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE"></telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Action" >
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItem%>' ID="cmdEdit"
                                        ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Update" CommandArgument='<%# Container.DataItem %>' ID="cmdUpdate"
                                        ToolTip="Update">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItem%>' ID="cmdCancel"
                                        ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                            PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
