<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareInOut.aspx.cs" Inherits="InventorySpareInOut" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spare Item In Out</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvSpareInOut.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSpareInOut" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuInventorySpareInOut" runat="server" OnTabStripCommand="MenuInventorySpareInOut_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTransactionDate" runat="server" Text="Transaction Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDispositionDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTransactionType" runat="server" Text="Transaction Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlDispositionType" runat="server" AppendDataBoundItems="true"
                            CssClass="input_mandatory" />
                    </td>
                    <td style="width: 15%"></td>
                    <td></td>
                    <td style="width: 15%"></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblWorkOrder" runat="server" Text="Work Order"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListWorkOrder">
                            <telerik:RadTextBox ID="txtWorkOrderNumber" RenderMode="Lightweight" runat="server" CssClass="input" MaxLength="20"
                                Width="100px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtWorkOrderDescription" RenderMode="Lightweight" runat="server" CssClass="input" MaxLength="20"
                                Width="200px">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="img1" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListWorkOrder', 'codehelp1', '', 'Common/CommonPickListWorkOrderCompleted.aspx', true);">
                                 <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="imgWorkorder" ImageAlign="AbsMiddle" Text=".." OnClick="imgWorkorder_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtWorkOrderId" runat="server" CssClass="hidden" RenderMode="Lightweight" MaxLength="20" Width="10px"></telerik:RadTextBox>
                        </span>&nbsp;
                               
                    </td>
                    <td style="width: 25"></td>
                    <td></td>
                    <td style="width: 20"></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListParentComponent">
                            <telerik:RadTextBox ID="txtComponentName" runat="server" ReadOnly="false" CssClass="input readonlytextbox" RenderMode="Lightweight"
                                MaxLength="20" Width="100px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentNumber" runat="server" ReadOnly="false" CssClass="input readonlytextbox" RenderMode="Lightweight"
                                MaxLength="20" Width="200px">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgShowParentComponent" ImageAlign="AbsMiddle" Text="..">
                                 <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="ImageButton1" ImageAlign="AbsMiddle" Text=".." OnClick="ImageButton1_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtComponentID" runat="server" CssClass="hidden" MaxLength="20" RenderMode="Lightweight"></telerik:RadTextBox>
                        </span>&nbsp;
                                
                    </td>
                    <td style="width: 15%"></td>
                    <td></td>
                    <td style="width: 15%"></td>
                    <td></td>
                </tr>
                <tr valign="top" style="width: 15%">
                    <td>
                        <%--  Reference--%>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="txtDispositionReference" Visible="false" CssClass="input" RenderMode="Lightweight"
                            MaxLength="10" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td style="width: 15%"></td>
                    <td></td>
                </tr>
                <tr style="width: 15%" valign="top">
                    <td colspan="6"></td>
                </tr>
            </table>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvSpareInOut" DecoratedControls="All" EnableRoundedCorners="true" />
            <eluc:TabStrip ID="MenuGridSpareInOut" runat="server" OnTabStripCommand="MenuGridSpareInOut_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSpareInOut" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnDeleteCommand="gvSpareInOut_DeleteCommand" OnNeedDataSource="gvSpareInOut_NeedDataSource" Width="100%" 
                OnItemDataBound="gvSpareInOut_ItemDataBound" OnSortCommand="gvSpareInOut_SortCommand" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="None">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="10%" AllowSorting="true" SortExpression="FLDNUMBER">
                            <ItemTemplate>
                                <asp:Label ID="lblStoreItemId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></asp:Label>
                                <asp:Label ID="lblStoreItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="40%" AllowSorting="true" SortExpression="FLDNAME">
                            <ItemTemplate>
                                <asp:Label ID="lblStoreItemName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location" HeaderStyle-Width="40%" AllowSorting="true" SortExpression="LOCATIONNAME">
                            <ItemTemplate>
                                <asp:Label ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></asp:Label>
                                <asp:Label ID="lblLocationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.LOCATIONNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity" HeaderStyle-Width="10%" AllowSorting="true" SortExpression="FLDQUANTITY" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblCurrentStock" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIONQUANTITY","{0:n0}") %>'></asp:Label>
                            </ItemTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
