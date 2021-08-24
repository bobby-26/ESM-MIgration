<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitTravelClaimPostMaster.aspx.cs"
    Inherits="AccountsVesselVisitTravelClaimPostMaster" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Travel Claim</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmtravelClaim" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="Title1" Text="Travel Claim" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />
            <eluc:TabStrip ID="MenuTravelClaimMain" runat="server" OnTabStripCommand="MenuTravelClaimMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuTravelClaim" runat="server" OnTabStripCommand="MenuTravelClaim_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelClaim" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
               Height="88%"  Width="100%" CellPadding="3" OnRowDataBound="gvTravelClaim_ItemDataBound" ShowHeader="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvTravelClaim_NeedDataSource"
                OnItemCommand="gvTravelClaim_ItemCommand" OnSelectedIndexChanging="gvTravelClaim_SelectedIndexChanging">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true" DataKeyNames="FLDTRAVELCLAIMID">
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
                        <telerik:GridTemplateColumn HeaderText="Form Number" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkFormNumber" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--  <telerik:GridTemplateColumn HeaderText="Employee Id">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblEmployeeIdHeader" runat="server">Employee Id</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                  
                                    <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>                                    
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Employee Name" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVisitId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVISITID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelClaimId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELCLAIMID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Department" HeaderStyle-Width="11%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFleet" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurpose" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Claim Status" HeaderStyle-Width="12%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClaimStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLAIMSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
