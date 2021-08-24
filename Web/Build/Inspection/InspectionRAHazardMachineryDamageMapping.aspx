<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAHazardMachineryDamageMapping.aspx.cs" Inherits="InspectionRAHazardMachineryDamageMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Severity" Src="~/UserControls/UserControlRASeverity.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Machinery Damage/ Failure and Hazard mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvRASubHazard").height(browserHeight - 40);
            });
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRASubHazard" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="ucTitle" Text="Machinery Damage/ Failure and Hazard mapping" ShowMenu="false" Visible="false"></eluc:Title>
            <table id="tblConfigure" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHazard" runat="server" Text="Hazard"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hazard ID="ucHazard" runat="server" AppendDataBoundItems="true" Width="30%"
                            AutoPostBack="true" OnTextChangedEvent="ucHazard_Changed" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRASubHazard" runat="server" OnTabStripCommand="RASubHazard_TabStripCommand">
            </eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRASubHazard" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" Height="84%" EnableHeaderContextMenu="true" GroupingEnabled="false"
                 OnNeedDataSource="gvRASubHazard_NeedDataSource" ShowFooter="true" OnItemCommand="gvRASubHazard_ItemCommand"
                ShowHeader="true" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderText="Hazard" HeaderStyle-Width="50%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHazard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSubHazardId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblHazardId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process Loss" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkProcessLoss" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSELECTEDFORPROCESSYN").ToString().Equals("1"))?true:false %>'
                                    AutoPostBack="true" OnCheckedChanged="chkProcess_CheckedChanged"></asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cost" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCost" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSELECTEDFORCOSTYN").ToString().Equals("1"))?true:false %>'
                                    AutoPostBack="true" OnCheckedChanged="chkCost_CheckedChanged"></asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
