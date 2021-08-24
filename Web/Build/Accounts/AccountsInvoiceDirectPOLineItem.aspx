<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceDirectPOLineItem.aspx.cs"
    Inherits="AccountsInvoiceDirectPOLineItem" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseSatging" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuDirectPO" runat="server" OnTabStripCommand="RegistersStockItem_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuRegistersStockItem" runat="server" OnTabStripCommand="RegistersStockItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOrderLine" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvOrderLine_ItemCommand" OnItemDataBound="gvOrderLine_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="false"
                OnNeedDataSource="gvOrderLine_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDORDERLINEID">
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
                        <telerik:GridTemplateColumn HeaderText="Item Name">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblorderlineitem" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDORDERLINEID")%>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPARTNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblorderlineitem" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDORDERLINEID")%>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtPartNameEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNAME") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPartNameAdd" runat="server" CssClass="input"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="80%" Enabled="false">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODEID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListBudgetAdd">
                                    <telerik:RadTextBox ID="txtBudgetCodeAdd" runat="server" MaxLength="20" CssClass="input_mandatory"
                                        Width="80%" Enabled="false">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameAdd" runat="server" Width="0px" CssClass="hidden" Enabled="False"></telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtBudgetIdAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="8%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" Width="90px" Mask="9999999.99"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory" Width="90px" Mask="9999999.99"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Discount">
                            <HeaderStyle Width="8%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPODISCOUNT", "{0:n2}")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtDiscountEdit" runat="server" CssClass="input" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPODISCOUNT") %>' Mask="9999999.99"></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtDiscountAdd" runat="server" CssClass="input" Width="90px" Mask="9999999.99"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Amount">
                            <HeaderStyle Width="8%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTOTALAMOUNT", "{0:n2}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Discount">
                            <HeaderStyle Width="8%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDVESSELDISCOUNT", "{0:n2}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Vessel Amount">
                            <HeaderStyle Width="8%" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTOTALVESSELAMOUNT", "{0:n2}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdUpdate"
                                    ToolTip="Update"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table width="100%" border="0" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblAdditionalChargeItem" runat="server" Text="Additional Charge Item"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="AdditionalChargeItem" runat="server" OnTabStripCommand="AdditionalChargeItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTax" Height="88%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvTax_ItemCommand" OnItemDataBound="gvTax_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="false"
                OnNeedDataSource="gvTax_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDORDERTAXCODE">
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
                        <telerik:GridTemplateColumn HeaderText="Charge Description">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblordertaxcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDORDERTAXCODE")%>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container,"DataItem.FLDDISCRIPTION") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblordertaxcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDORDERTAXCODE")%>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" CssClass="gridinput_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCRIPTION") %>' MaxLength="45">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblIsGST" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISGST") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="45">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPENAME") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:TaxType ID="ucTaxTypeEdit" runat="server" TaxType='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:TaxType ID="ucTaxTypeAdd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblTaxType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPE") %>'></telerik:RadLabel>
                                <span id="spnPickListTaxBudgetEdit" runat="server">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="80%" Enabled="false">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListTaxBudgetAdd">
                                    <telerik:RadTextBox ID="txtBudgetCodeAdd" runat="server" Width="80%" CssClass="input_mandatory"
                                        Enabled="false">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameAdd" runat="server" Width="0px" CssClass="hidden" Enabled="False"></telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtBudgetIdAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Value">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVALUE","{0:n2}") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtValueEdit" runat="server" Width="90px" CssClass="input_mandatory" Mask="9999999.99"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE","{0:n2}") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtValueAdd" runat="server" Width="90px" CssClass="input_mandatory" Mask="9999999.99" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Discount">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPODISCOUNT","{0:n2}") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtDiscountEdit" runat="server" Width="90px" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPODISCOUNT","{0:n2}") %>' Mask="9999999.99" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtDiscountAdd" runat="server" Width="90px" CssClass="gridinput" Mask="9999999.99" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT", "{0:n2}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Amount">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTOTALAMOUNT", "{0:n2}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Discount">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDVESSELDISCOUNT", "{0:n2}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Vessel Amount">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTOTALVESSELAMOUNT", "{0:n2}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Visible="false"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
