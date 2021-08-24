<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPEARSRAStandard.aspx.cs" Inherits="InspectionPEARSRAStandard" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>PEARS RA</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvPEARSSTDRA.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
        <script type="text/javascript">
            function ConfirmApprove(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmApprove.UniqueID %>", "");
                }
            } function ConfirmRevision(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRevision.UniqueID %>", "");
                }
            }
             function confirmissue(args) {
                if (args) {
                    __doPostBack("<%=btnstandardtemplateissue.UniqueID %>", "");
                }
             }
        </script>        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuPEARSRA" runat="server" OnTabStripCommand="MenuPEARSRA_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvPEARSRA" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPEARSSTDRA" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true" EnableViewState="false" AllowFilteringByColumn="true"
                OnItemDataBound="gvPEARSSTDRA_ItemDataBound" OnItemCommand="gvPEARSSTDRA_ItemCommand" OnNeedDataSource="gvPEARSSTDRA_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRISKASSESSMENTID" ClientDataKeyNames="FLDRISKASSESSMENTID" AllowFilteringByColumn="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Ref. No" AllowFiltering="true" DataField="FLDREFERENCENUMBER" UniqueName="FLDREFERENCENUMBER"
                            FilterDelay="2000" AutoPostBackOnFilter="false" ShowFilterIcon="false" FilterControlWidth="95%" CurrentFilterFunction="Contains" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRAid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTID")  %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prepared" AllowFiltering="false" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblprepared" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCREATEDDATE"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type of Activity" DataField="FLDTYPEOFACTIVITY" UniqueName="FLDTYPEOFACTIVITY"
                            FilterDelay="2000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterControlWidth="95%" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobActivity" runat="server" CommandName="EDITROW" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEOFACTIVITY")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity Worksite" DataField="FLDACTIVITYWORKSITE" UniqueName="FLDACTIVITYWORKSITE"
                            FilterDelay="2000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterControlWidth="95%" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActivityWorkSite" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYWORKSITE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rev No" AllowFiltering="false" HeaderStyle-Width="6%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevisionNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNUMBER")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="11%" AllowFiltering="false">                            
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSID")  %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false" HeaderStyle-Width="14%">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Issue as Standard Template" CommandName="STDISSUE" ID="imgSTD" ToolTip="Issue as Standard Template">
                                   <span class="icon"><i class="fas fa-proposeST"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Approve" CommandName="APPROVE" ID="imgApprove" ToolTip="Approve">
                                    <span class="icon"><i class="fas fa-user-check"></i></span>
                                </asp:LinkButton>                                
                                <asp:LinkButton runat="server" AlternateText="Revision" CommandName="REVISION" ID="imgrevision" ToolTip="Create Revision">
                                    <span class="icon"><i class="fas fa-registered"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Report" CommandName="PEARS" ID="cmdRAReport" ToolTip="Show PDF">
                                    <span class="icon"><i class="fas fa-file-pdf"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Revision" CommandName="VIEWREVISION" ID="cmdRevision" ToolTip="View Revisions">
                                    <span class="icon"><i class="fas fa-eye"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Copy" CommandName="COPYTEMPLATE" ID="imgCopyTemplate" ToolTip="Copy">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
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
            <asp:Button ID="ucConfirmApprove" runat="server" Text="confirmApprove" OnClick="ucConfirmApprove_Click" />
            <asp:Button ID="ucConfirmRevision" runat="server" Text="confirmRevision" OnClick="ucConfirmRevision_Click" />
            <asp:Button ID="btnstandardtemplateissue" runat="server" Text="confirmissue" OnClick="btnstandardtemplateissue_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>


