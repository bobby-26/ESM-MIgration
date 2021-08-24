<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormScript.aspx.cs" Inherits="PurchaseFormScript" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Form</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= rgPurchaseForm.ClientID %>"));
                }, 200);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="rsPurchaseForm" />
        <telerik:RadSkinManager ID="rsmPurchaseForm" runat="server" />
        <telerik:RadWindowManager ID="rwmPurchaseForm" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <telerik:RadAjaxPanel ID="rapPurchaseForm" runat="server">
            <%--<div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <%--<div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand" ></eluc:TabStrip>
            </div>--%>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFormNo" runat="server" Text="Form No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFormNo" CssClass="input" AutoPostBack="true" OnTextChanged="txtFormNo_Changed"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <telerik:RadGrid ID="rgPurchaseForm" runat="server" RenderMode="Lightweight" AllowSorting="true" CellSpacing="0" GridLines="None"
                OnNeedDataSource="rgPurchaseForm_NeedDataSource" OnItemDataBound="rgPurchaseForm_ItemDataBound" OnItemCommand="rgPurchaseForm_ItemCommand"
                EnableViewState="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDORDERID">
                    <Columns>
                            <telerik:GridTemplateColumn HeaderText="Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lnkFormNumberName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Form Title">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStockItemCode" runat="server"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="uclblStockItemCodeTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Form Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPrefferedVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Form Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWanted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMSTATUSNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Approved Date">
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblApprovedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEAPPROVEDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Ordered Date">
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOrderedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Received Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReceivedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORDELIVERYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrentStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERFORMSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Req Status" CommandName="REQSTATUS"
                                        ImageUrl="<%$ PhoenixTheme:images/task-list.png %>" CommandArgument='<%# Container.DataItem %>'
                                        ID="cmdReqStatus" ToolTip="To Correct the Requisition Status.."></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Remove Duplicate" CommandName="REMOVEDUPLICATE"
                                        ImageUrl="<%$ PhoenixTheme:images/task-list.png %>" CommandArgument='<%# Container.DataItem %>'
                                        ID="imgRemoveDuplicate" ToolTip="To remove the Form Duplicate"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Split PO Overwrite" CommandName="SPLITPOOVERWRITE"
                                        ImageUrl="<%$ PhoenixTheme:images/task-list.png %>" CommandArgument='<%# Container.DataItem %>'
                                        ID="imgSplitPOOverwrite" ToolTip="To correct the Split PO overwrite"></asp:ImageButton>
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
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--
                <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                    <asp:GridView ID="gvFormDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvFormDetails_RowCommand" OnRowDataBound="gvFormDetails_ItemDataBound"
                        AllowSorting="false" ShowHeader="true"
                        EnableViewState="false" DataKeyNames="FLDORDERID">

                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lnkFormNumberName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:Label>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="StockItem Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblFormTitle" runat="server" Text="Form Title"></asp:Literal>

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStockItemCode" runat="server"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString() %>'></asp:Label>
                                    <eluc:ToolTip ID="uclblStockItemCodeTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PreferredVendor">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblFormType" runat="server" Text="Form Type"></asp:Literal>

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPrefferedVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Form Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblFormStatus" runat="server" Text="Form Status"></asp:Literal>

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWanted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMSTATUSNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblApprovedDate" runat="server" Text="Approved Date"></asp:Literal>

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEAPPROVEDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOrderedDate" runat="server" Text="Ordered Date"></asp:Literal>

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblReceivedDate" runat="server" Text="Received Date"></asp:Literal>

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReceivedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORDELIVERYDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrentStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERFORMSTATUS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Req Status" CommandName="REQSTATUS"
                                        ImageUrl="<%$ PhoenixTheme:images/task-list.png %>" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="cmdReqStatus" ToolTip="To Correct the Requisition Status.."></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Remove Duplicate" CommandName="REMOVEDUPLICATE"
                                        ImageUrl="<%$ PhoenixTheme:images/task-list.png %>" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="imgRemoveDuplicate" ToolTip="To remove the Form Duplicate"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Split PO Overwrite" CommandName="SPLITPOOVERWRITE"
                                        ImageUrl="<%$ PhoenixTheme:images/task-list.png %>" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="imgSplitPOOverwrite" ToolTip="To correct the Split PO overwrite"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>--%>
            <%-- </div>--%>
        </telerik:RadAjaxPanel>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                Sys.Application.add_load(function () {
                    setTimeout(function () {
                        TelerikGridResize($find("<%= rgPurchaseForm.ClientID %>"));
                    }, 200);
                });
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
