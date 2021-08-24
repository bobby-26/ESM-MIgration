<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsEmployeeBondRequisitionLineItem.aspx.cs" Inherits="VesselAccountsEmployeeBondRequisitionLineItem" %>


<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Issue of Bonded Stores</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="OrderForm_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenPick_Click" />
            <table width="65%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOnAccountFor" runat="server" Text="Account Of"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselCrew ID="ddlEmployee" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ddlEmployee_TextChangedEvent" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOrderNo" runat="server" Text="Order No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"
                            Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIssueType" runat="server" Text="Issue Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlHard" runat="server" CssClass="input_mandatory" HardTypeCode="97"
                            ShortNameFilter="BND" AutoPostBack="true" OnTextChangedEvent="ddlEmployee_TextChangedEvent"
                            Enabled="false" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuBondIssue" runat="server" OnTabStripCommand="MenuBondIssue_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" Height="89%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCrewSearch_ItemCommand" OnItemDataBound="gvCrewSearch_ItemDataBound" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvCrewSearch_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Employee">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblorderlineid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDORDERLINEID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOldQty" Visible="false" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDORDEREDQUANTITY"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblorderid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDORDERID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblstoreitemid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSTOREITEMID"] %>'></telerik:RadLabel>

                                <%#((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDSTORENAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDUNITNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDORDEREDQUANTITY"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtQuantityEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                    Text='<%#((DataRowView)Container.DataItem)["FLDORDEREDQUANTITY"] %>' IsInteger="true"
                                    IsPositive="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit Price">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDUNITPRICE"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtUnitPriceEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                    Text='<%#((DataRowView)Container.DataItem)["FLDUNITPRICE"] %>' IsInteger="true"
                                    IsPositive="true" />
                                <telerik:RadLabel ID="lblorderidedit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDORDERID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblstoreitemidedit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSTOREITEMID"] %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotal" runat="server" Text="Total"></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDTOTALPRICE"]%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server"></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued On">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDISSUEDDATE"])%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtDateEdit" runat="server" CssClass="input_mandatory" Text='<%#((DataRowView)Container.DataItem)["FLDISSUEDDATE"] %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItem%>" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdUpdate"
                                    ToolTip="Update"><span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
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

