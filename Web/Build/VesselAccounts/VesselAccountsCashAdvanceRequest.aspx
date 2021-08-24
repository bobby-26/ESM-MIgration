<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCashAdvanceRequest.aspx.cs" Inherits="VesselAccountsCashAdvanceRequest" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="../UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component</title>
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
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmBondReq" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmBondReq" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuCashAdvanceRequest" runat="server" OnTabStripCommand="MenuCashAdvanceRequest_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCashAdvanceRequest" Height="98%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCashAdvanceRequest_ItemCommand" OnItemDataBound="gvCashAdvanceRequest_ItemDataBound" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvCashAdvanceRequest_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <HeaderStyle Width="102px" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Date">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequestId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSeaPortId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDSEAPORTID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDTKEY"] %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cuurency">
                            <HeaderStyle Width="27%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcurrency" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCURRENCYCODE"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supplier Name">
                            <HeaderStyle Width="27%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDSUPPLIERNAME"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SeaPort Name">
                            <HeaderStyle Width="28%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeaPortName" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDSEAPORTNAME"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="28%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblstatus" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDSTATUSDESC"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="SELECT" ID="cmdDetails" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                    ToolTip="Attachment"><span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
