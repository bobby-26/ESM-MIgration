<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementSearchResultsNew.aspx.cs" Inherits="DocumentManagement_DocumentManagementSearchResultsNew" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentTreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search Results</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            var DWPgrid = null;
            function Resize() {
                var $ = $telerik.$;
                var height = $(window).height();
                if (DWPgrid != null && DWPgrid.GridDataDiv != null) {
                    var gridPagerHeight = (DWPgrid.PagerControl) ? DWPgrid.PagerControl.offsetHeight : 0;
                    DWPgrid.GridDataDiv.style.height = (height - gridPagerHeight - 240) + "px";
                } else {
                    var gvSectionMatches = $find("<%= gvSectionMatches.ClientID %>");
                    var gvFormMatch = $find("<%= gvFormMatch.ClientID %>");
                    var gvRiskAssessment = $find("<%= gvRiskAssessment.ClientID %>");
                    var gvJobHazard = $find("<%= gvJobHazard.ClientID %>");
                    
                    gvSectionMatches.GridDataDiv.style.height = (Math.round(height / 2) - 80 )+ "px";
                    gvFormMatch.GridDataDiv.style.height = (Math.round(height / 2) - 80) + "px";
                    gvRiskAssessment.GridDataDiv.style.height = (Math.round(height / 2) - 80) + "px";
                    gvJobHazard.GridDataDiv.style.height = (Math.round(height / 2) - 80) + "px";

                }

            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSearchResults" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--<eluc:Title runat="server" ID="ucTitle" Text="Search Results" ShowMenu="false"></eluc:Title>                        --%>
            <eluc:TabStrip ID="MenuDocumentList" runat="server" Title="Search Results" TabStrip="false"></eluc:TabStrip>
            <eluc:Status runat="server" ID="ucStatus" />
            <br />
            <table width="100%">
                <tr>
                    <td width="60%">
                        <asp:LinkButton ID="lblSectionTitle" runat="server" Text="0 matches in section title"
                            Style="font-weight: bold"></asp:LinkButton>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvSectionMatches" runat="server" AutoGenerateColumns="False" Font-Size="11px" 
                            Width="100%" CellPadding="3" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" OnNeedDataSource="gvSectionMatches_NeedDataSource"
                            OnItemDataBound="gvSectionMatches_RowDataBound" OnItemCommand="gvSectionMatches_RowCommand"
                            OnSelectedIndexChanging="gvSectionMatches_SelectedIndexChanging" EnableViewState="false" ShowFooter="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDSECTIONID">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Name">
                                        <HeaderStyle Width="24%" />
                                        <HeaderTemplate>
                                            Category
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblcategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblcategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Type">
                                        <HeaderStyle Width="29%" />
                                        <HeaderTemplate>
                                            Subcategory
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME").ToString().Length > 39 ? (DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME").ToString().Substring(0, 39) + "...") : DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME").ToString() %>'></telerik:RadLabel>
                                            <eluc:ToolTip ID="ucDocumentName" TargetControlId="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Type">
                                        <HeaderStyle Width="43%" />
                                        <HeaderTemplate>
                                            Section
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSectionName" runat="server" CommandName="SECTIONSELECT"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION").ToString().Length > 65 ? (DataBinder.Eval(Container, "DataItem.FLDSECTION").ToString().Substring(0, 65) + "...") : DataBinder.Eval(Container, "DataItem.FLDSECTION").ToString() %>'></asp:LinkButton>
                                            <telerik:RadLabel ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblSectionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <eluc:ToolTip ID="ucSectionName" TargetControlId="lnkSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION") %>' Width="400px" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="">
                                        <HeaderStyle HorizontalAlign="Center" Width="9%"></HeaderStyle>

                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="ReadUnread" CommandName="READUNREAD" ID="lnkReadUnreadSec"
                                                ToolTip="Read/Unread List"><span class="icon"><i class="fa-coins-ei"></i></span></asp:LinkButton>

                                            <asp:LinkButton runat="server" AlternateText="Forms and Checklist" CommandName="FORMS" ID="cmdForms"
                                                ToolTip="Linked Forms and Checklist"><span class="icon"><i class="fa-file-contract-af"></i></span></asp:LinkButton>
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
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                    <td width="40%">
                        <asp:LinkButton ID="lblJobHazardTitle" runat="server" Text="0 matches in JHA's"
                            Style="font-weight: bold"></asp:LinkButton>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvJobHazard" runat="server" AutoGenerateColumns="False" Font-Size="11px" 
                            Width="100%" CellPadding="3" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" OnNeedDataSource="gvJobHazard_NeedDataSource"
                            OnItemDataBound="gvJobHazard_RowDataBound" OnItemCommand="gvJobHazard_RowCommand"
                            OnSelectedIndexChanging="gvJobHazard_SelectedIndexChanging" ShowFooter="false" EnableViewState="true">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDJOBHAZARDID">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Type" Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                        <HeaderTemplate>
                                            Type
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Process">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                        <HeaderTemplate>
                                            Process
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Job">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                        <HeaderTemplate>
                                            Job
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblJob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB").ToString().Length > 42 ? (DataBinder.Eval(Container, "DataItem.FLDJOB").ToString().Substring(0, 42) + "...") : DataBinder.Eval(Container, "DataItem.FLDJOB").ToString() %>'></telerik:RadLabel>
                                            <eluc:ToolTip ID="ucJob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Ref Number">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                        <HeaderTemplate>
                                            Ref Number
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="JHASELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'></asp:LinkButton>
                                            <telerik:RadLabel ID="lblJobHazardId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
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
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lblFormTitle" runat="server" Text="0 matches in form title"
                            Style="font-weight: bold"></asp:LinkButton>
                        <telerik:RadGrid ID="gvFormMatch" runat="server" AutoGenerateColumns="False" Font-Size="11px" 
                            Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="false" AllowPaging="false"
                            OnNeedDataSource="gvFormMatch_NeedDataSource" OnItemDataBound="gvFormMatch_RowDataBound" OnItemCommand="gvFormMatch_RowCommand"
                            OnSelectedIndexChanging="gvFormMatch_SelectedIndexChanging" ShowFooter="false" EnableViewState="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDFORMID">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Top Category">
                                        <HeaderStyle Width="25%" />
                                        <HeaderTemplate>
                                            Category
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTopCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Category">
                                        <HeaderStyle Width="25%" />
                                        <HeaderTemplate>
                                            Subcategory
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblcategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Type">
                                        <HeaderStyle Width="45%" />
                                        <HeaderTemplate>
                                            Forms / Checklists / Circulars
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkFormName" runat="server" CommandName="FORMSELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME").ToString().Length > 65 ? (DataBinder.Eval(Container, "DataItem.FLDNAME").ToString().Substring(0, 65) + "...") : DataBinder.Eval(Container, "DataItem.FLDNAME").ToString() %>'></asp:LinkButton>
                                            <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lbltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <eluc:ToolTip ID="ucFormName" TargetControlId="lnkFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' Width="400px" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="5%">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Documents" CommandName="DOCUMENTS" ID="cmdDocuments"
                                                ToolTip="Linked Documents"><span class="icon"><i class="fas fa-proposeST"></i></span></asp:LinkButton>
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
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                    <td>
                        <asp:LinkButton ID="lblRiskAssessmentTitle" runat="server" Text="0 matches in RA's"
                            Style="font-weight: bold"></asp:LinkButton>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessment" runat="server" AutoGenerateColumns="False" Font-Size="11px" 
                            Width="100%" CellPadding="3" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" OnNeedDataSource="gvRiskAssessment_NeedDataSource"
                            OnItemDataBound="gvRiskAssessment_RowDataBound" OnItemCommand="gvRiskAssessment_RowCommand"
                            OnSelectedIndexChanging="gvRiskAssessment_SelectedIndexChanging" ShowFooter="false" EnableViewState="false" DataKeyNames="FLDPROCESSID">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDPROCESSID">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Type" Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                        <HeaderTemplate>
                                            Type
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Category">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                        <HeaderTemplate>
                                            Process
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblcategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Process">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                        <HeaderTemplate>
                                            Activity
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblActivityCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYORCONDITION").ToString().Length > 45 ? (DataBinder.Eval(Container, "DataItem.FLDACTIVITYORCONDITION").ToString().Substring(0, 45) + "...") : DataBinder.Eval(Container, "DataItem.FLDACTIVITYORCONDITION").ToString() %>'></telerik:RadLabel>
                                            <eluc:ToolTip ID="ucActivityCondition" TargetControlId="lblActivityCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYORCONDITION") %>'
                                                Width="400px" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Process Name" Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                        <HeaderTemplate>
                                            Process Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROCESSNAME").ToString().Length > 45 ? (DataBinder.Eval(Container, "DataItem.FLDPROCESSNAME").ToString().Substring(0, 45) + "...") : DataBinder.Eval(Container, "DataItem.FLDPROCESSNAME").ToString()%>'></telerik:RadLabel>
                                            <eluc:ToolTip ID="ucProcessName" TargetControlId="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSNAME") %>'
                                                Width="350px" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                        <HeaderTemplate>
                                            Ref Number
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="RASELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO") %>'></asp:LinkButton>
                                            <telerik:RadLabel ID="lblProcessId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
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
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
