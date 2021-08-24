<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRASubHazardExtn.aspx.cs" Inherits="InspectionRASubHazardExtn" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardTypeExtn.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Undesirable Event and Worst case</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvRASubHazard.ClientID %>"));
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
    <form id="frmRASubHazard" runat="server">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
     <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
     <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"></telerik:RadWindowManager>
     <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigure" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
                <div class="demo-container no-bg size-wide">
                    <table id="tblConfigure" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblHazardHr" runat="server" Text="Category"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hazard ID="ucHazard" runat="server" AppendDataBoundItems="true" Width="80%" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblSubHazard" runat="server" Text="Hazard"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtName" runat="server" MaxLength="100" Width="360px" CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                    <eluc:TabStrip ID="MenuRASubHazard" runat="server" OnTabStripCommand="RASubHazard_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvRASubHazard" DecoratedControls="All" EnableRoundedCorners="true" />
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvRASubHazard" runat="server" AllowCustomPaging="true" 
                         Font-Size="11px" AllowPaging="true" AllowSorting="true" OnNeedDataSource="gvRASubHazard_NeedDataSource" 
                        Width="100%" CellPadding="3" OnItemCommand="gvRASubHazard_ItemCommand" OnSortCommand="gvSubHazard_SortCommand" OnItemDataBound="gvRASubHazard_ItemDataBound"
                         ShowFooter="false" EnableHeaderContextMenu="true" GroupingEnabled="false"
                        ShowHeader="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDHAZARDID">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblHazard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Hazard" HeaderStyle-Width="35%">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSubHazardId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblHazardId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Impact" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Severity" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEVERITY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Score" HeaderStyle-Width="10%" AllowSorting="true" SortExpression="FLDSCORE">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Cons" HeaderTooltip="Consequence Category" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCategory"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>                                
                            <telerik:GridTemplateColumn HeaderText="Contact Type / <br> Undesirable Event" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblUndisrableImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNDESIRABLEEVENT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Worst Case" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWorstCase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORSTCASE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PPE" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPPE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPPELIST") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>                                                             
                            <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="7%"></ItemStyle>
                                <ItemTemplate>                                   
                                    <asp:LinkButton runat="server" AlternateText="Edit" CommandName="TEDIT" ID="cmdTypeMapping" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>                                
                            </telerik:GridTemplateColumn>
                        </Columns>
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
