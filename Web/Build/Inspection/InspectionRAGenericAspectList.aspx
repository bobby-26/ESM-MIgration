<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAGenericAspectList.aspx.cs" Inherits="InspectionRAGenericAspectList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RA Aspects</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRAOperationalHazard" runat="server" AllowCustomPaging="false"
                Font-Size="11px" AllowPaging="false" AllowSorting="true" EnableViewState="true" OnNeedDataSource="gvRAOperationalHazard_NeedDataSource"
                Width="100%" CellPadding="3" OnItemCommand="gvRAOperationalHazard_ItemCommand" OnItemDataBound="gvRAOperationalHazard_ItemDataBound"
                GroupingEnabled="false" ShowHeader="true" ShowFooter="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRAGENERICOPERATIONID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Aspects" HeaderStyle-Width="70%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="70%"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOperationalHeader" runat="server">Aspects</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOperationalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAGENERICOPERATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOperationalHazard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECT") %>' Visible="false"></telerik:RadLabel>                                
                                 <asp:LinkButton ID="lnkaspets" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECT") %>' CommandName="PICKLIST"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Person's/Involved" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPersons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERSONSINVOLVED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

