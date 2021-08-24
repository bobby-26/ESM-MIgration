<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCashAdvanceRequestGeneral.aspx.cs" Inherits="VesselAccountsCashAdvanceRequestGeneral" %>

<!DOCTYPE html >
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlVesselMappingCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MapCurrency" Src="~/UserControls/UserControlVesselMappingCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiVesselSupplier" Src="~/UserControls/UserControlMultiColumnVesselSupplier.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuCashAdvanceRequest" runat="server" OnTabStripCommand="MenuCashAdvanceRequest_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuCrewBond" runat="server" OnTabStripCommand="MenuCrewBond_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%" CssClass="scrolpan">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucstatus" runat="server" Visible="false" />

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>

                    <td style="width: 8%;">
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 18%;">
                        <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:Currency ID="ddlCurrency" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="130px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 8%;">
                        <telerik:RadLabel ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel>
                    </td>
                    <td style="width: 18%;">
                        <eluc:Date ID="txtETA" runat="server" />

                    </td>

                    <td>
                        <telerik:RadLabel ID="lblETD" runat="server" Text="ETD"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtETD" runat="server" CssClass="input" />

                    </td>
                </tr>
                <tr>
                    <td style="width: 8%;">
                        <telerik:RadLabel ID="lblExpectedPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td style="width: 28%;">
                        <eluc:MultiPort ID="ucPort" runat="server" Width="290px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPortAgent" runat="server" Text="Port Agent"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiVesselSupplier ID="ucVesselSupplier" runat="server" Width="290px" />
                        <%-- <span id="spnPickListSupplier">
                            <telerik:RadTextBox ID="txtSupplierCode" runat="server" Width="78px"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtSupplierName" runat="server" Width="178px"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="cmdShowSupplier" OnClientClick="return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListVesselSupplier.aspx', true);">
                                    <span class="icon"><i class="fas fas fa-list-alt"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1px"></telerik:RadTextBox>
                        </span>--%>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuCashLineItem" runat="server" OnTabStripCommand="MenuCashLineItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCashLineItem" runat="server" Height="77%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCashLineItem_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvCashLineItem_ItemDataBound"
                OnItemCommand="gvCashLineItem_ItemCommand" ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Width="25%" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLineItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEarningDeductionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEARNINGDEDUCTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <HeaderStyle VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSACTIONAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <HeaderStyle VerticalAlign="Middle" Width="20%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurpose" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Paid From">
                            <HeaderStyle VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpaidfrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAIDFROMDESC") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" ToolTip="Delete" CommandName="DELETE" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>

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

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
