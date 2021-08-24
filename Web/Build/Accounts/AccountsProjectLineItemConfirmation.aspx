<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectLineItemConfirmation.aspx.cs" Inherits="Accounts_AccountsProjectLineItemConfirmation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Awaiting Order Confirmation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAwaitingOrderFormItemDetails" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Title runat="server" ID="Title1" Text="Awaiting Confirmation" ShowMenu="true" Visible="false"></eluc:Title>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="Menu" runat="server" OnTabStripCommand="Menu_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
            Height="81%" Width="100%" CellPadding="3" OnItemCommand="gvLineItem_ItemCommand" OnItemDataBound="gvLineItem_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false"
            OnRowCancelingEdit="gvLineItem_RowCancelingEdit" OnRowDeleting="gvLineItem_RowDeleting" OnRowUpdating="gvLineItem_RowUpdating" OnNeedDataSource="gvLineItem_NeedDataSource"
            ShowHeader="true"
            EnableViewState="false" AllowSorting="true">
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
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
                    <telerik:GridTemplateColumn HeaderText="Requisition Number" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRequisitionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNUMBER") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblConfirmationLineItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTCODECONFIRMATIONLINEITEMID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblConfirmationType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIRMATIONTYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Supplier Code" HeaderStyle-Width="15%">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSupplierCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Supplier Name" HeaderStyle-Width="20%">
                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Request Date" HeaderStyle-Width="13%">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRequestedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Requisition Status" HeaderStyle-Width="20%">
                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRequisitionStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONSTATUS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Requisition Amount" HeaderStyle-Width="13%">
                         <HeaderStyle  HorizontalAlign="Right"/>
                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRequisitionAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONAMOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Estimated Amount" HeaderStyle-Width="13%">
                         <HeaderStyle  HorizontalAlign="Right"/>
                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEstimatedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESTIMATEDTOTAL","{0:n2}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblConfirmationLineItemIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTCODECONFIRMATIONLINEITEMID") %>'></telerik:RadLabel>
                            <asp:TextBox ID="txtEstimatedAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESTIMATEDTOTAL","{0:n2}")%>'
                                CssClass="gridinput">
                            </asp:TextBox>
                            <%--               <ajaxtoolkit:telerik:GridTemplateColumn id="MaskedAmount" runat="server" targetcontrolid="txtEstimatedAmountEdit"
                            mask="999,999,999.99" masktype="Number" inputdirection="RightToLeft">
                                        </ajaxtoolkit:telerik:GridTemplateColumn>--%>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Budget Code" HeaderStyle-Width="13%">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Owner Budget Code" HeaderStyle-Width="14%">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Center" HeaderText="Action" HeaderStyle-Width="10%">
                        <HeaderStyle />
                        <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img id="Img7" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>
                            <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img alt="" src="../css/Theme1/images/spacer.png" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
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
<%--        <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
            <tr>
                <td nowrap align="center">
                    <telerik:RadLabel ID="lblPagenumber" runat="server">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="lblPages" runat="server">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="lblRecords" runat="server">
                    </telerik:RadLabel>
                    &nbsp;&nbsp;
                </td>
                <td nowrap align="left" width="50px">
                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                </td>
                <td width="20px">&nbsp;
                </td>
                <td nowrap align="right" width="50px">
                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                </td>
                <td nowrap align="center">
                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                    </asp:TextBox>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                        Width="40px"></asp:Button>
                </td>
            </tr>
        </table>--%>
        <div>
            <table>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblTotalOrder" runat="server" Text="Total Estimated Awaiting Orders:"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltotal" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
