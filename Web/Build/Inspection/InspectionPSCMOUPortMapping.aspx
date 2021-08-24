<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPSCMOUPortMapping.aspx.cs" Inherits="Inspection_InspectionPSCMOUPortMapping" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Incident</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvCompany.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmIncidentOilmajorCompany" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%" cellpadding="2" cellspacing="2" runat="server" visible="false">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompanyName" runat="server" Text="PSC MOUs"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCompanyName" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuCompany" runat="server" OnTabStripCommand="MenuCompany_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCompany" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCompany_ItemCommand" OnNeedDataSource="gvCompany_NeedDataSource"
                OnItemDataBound="gvCompany_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true"
                OnSortCommand="gvCompany_SortCommand">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left">
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
                        <telerik:GridTemplateColumn AllowSorting="true" ShowSortIcon="true" SortExpression="FLDOILMAJORCOMPANYNAME" HeaderStyle-Width="200px" HeaderText="Company Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOilMajorCompanyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILMAJORCOMPANYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILMAJORCOMPANYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOilMajorCompanyIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILMAJORCOMPANYID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtCompanyNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILMAJORCOMPANYNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="98%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="300px" HeaderText="Port Mapping">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeaport" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTLISTNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipSeaport" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTLISTNAME") %>' Width="250px" TargetControlId="lblInspection" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Port Mapping" ToolTip="Port Mapping" Width="20PX" Height="20PX"
                                    CommandName="PORTMAPPING" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPortMapping">
                                <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>
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
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
