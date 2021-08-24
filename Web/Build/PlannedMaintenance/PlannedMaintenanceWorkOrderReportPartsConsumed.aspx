<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderReportPartsConsumed.aspx.cs" Inherits="PlannedMaintenanceWorkOrderReportPartsConsumed" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Parts Used</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvUsesParts.ClientID %>"));
                }, 200);
            }
            function pageLoad() {
                Resize();
            }
             window.onload = window.onresize = Resize;
    </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmgvUsesParts" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenugvUsesParts" runat="server" OnTabStripCommand="gvUsesParts_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvUsesParts" runat="server" AllowCustomPaging="true" AllowSorting="false" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnItemCommand="gvUsesParts_ItemCommand"  Width="100%" OnBatchEditCommand="gvUsesParts_BatchEditCommand"            
            OnNeedDataSource="gvUsesParts_NeedDataSource" OnItemDataBound="gvUsesParts_ItemDataBound" GroupingEnabled="false" EnableHeaderContextMenu="true">
            
            <MasterTableView  CommandItemDisplay="Top" CommandItemSettings-ShowAddNewRecordButton="false" EditMode="Batch" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDWORKORDERLINEITEMID,FLDLOCATION">
                <BatchEditingSettings EditType="Cell" />                
                <HeaderStyle Width="102px" />
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Part No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNUMBER">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblWorkOrderLineID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERLINEITEMID") %>'></telerik:RadLabel>                            
                            <telerik:RadLabel ID="lblSpareItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblPartNumber" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Part Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPartName" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Location">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLocation" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                       <%-- <EditItemTemplate>
                            <telerik:RadDropDownList ID="ddlLocation" runat="server" CssClass="input_mandatory" DataTextField="FLDLOCATIONNAME"
                                DataValueField="FLDLOCATIONID" AppendDataBoundItems="true" Width="100%">
                            </telerik:RadDropDownList>
                        </EditItemTemplate>--%>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Unit">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblUnit" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB">
                        <ItemStyle Wrap="false" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Quantity in Location">
                        <ItemStyle Wrap="false" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLocationRob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONROB","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="FLDQUANTITY">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblQuantity" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Decimal runat="server" ID="txtQuantityEdit" CssClass="input_mandatory" Width="100%" Style="text-align: right;"
                                Mask="999" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>                    
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>

<%--                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>--%>
                            <%--<img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />--%>
                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save"
                                CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>--%>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" OnConfirmMesage="ucConfirm_OnClick" Visible="false" />
    </form>
</body>
</html>
