<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementSearchResults.aspx.cs" Inherits="DocumentManagementSearchResults" MaintainScrollPositionOnPostback="true" %>

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

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search Results</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript">
            function SetParentIframeURL(type, url, selectednode) {
                window.parent.SetSourceURL(type, url, selectednode);
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSearchResults" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--<eluc:Title runat="server" ID="ucTitle" Text="Search Results" ShowMenu="false"></eluc:Title>                        --%>
                <eluc:TabStrip ID="MenuDocumentList"  runat="server" Title="Search Results" TabStrip="false">
                  </eluc:TabStrip>
            <telerik:RadLabel ID="lblDocumentTitle" runat="server" Text="0 matches in document title"
                Style="font-weight: bold">
            </telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSearchResults" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnNeedDataSource="gvSearchResults_NeedDataSource"
                OnItemCommand="gvSearchResults_RowCommand" OnItemDataBound="gvSearchResults_ItemDataBound"
                ShowFooter="true" ShowHeader="true" OnSelectedIndexChanging="gvSearchResults_SelectedIndexChanging">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDDOCUMENTID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name">                            
                            <HeaderStyle Width ="40%" />
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
                            <HeaderStyle Width ="60%" />
                            <HeaderTemplate>
                                Document
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDocumentName" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' OnClientClick='<%# string.Format("javascript:SetParentIframeURL(\"{0}\",\"{1}\",\"{2}\")", Eval("FLDTYPE"),GetParentIframeURL(Eval("FLDDOCUMENTID").ToString()), Eval("FLDDOCUMENTID")) %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
            <br />
            <telerik:RadLabel ID="lblSectionTitle" runat="server" Text="0 matches in section title"
                Style="font-weight: bold">
            </telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSectionMatches" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnNeedDataSource="gvSectionMatches_NeedDataSource"
                OnItemDataBound="gvSectionMatches_RowDataBound" OnItemCommand="gvSectionMatches_RowCommand"
                OnSelectedIndexChanging="gvSectionMatches_SelectedIndexChanging" EnableViewState="false" ShowFooter="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDSECTIONID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Width ="25%" />
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
                            <HeaderStyle Width ="30%" />
                            <HeaderTemplate>
                                Document
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME").ToString().Length > 39 ? (DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME").ToString().Substring(0, 39) + "...") : DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucDocumentName" TargetControlId="lblDocumentName"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width ="50%" />
                            <HeaderTemplate>
                                Section
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSectionName" runat="server" CommandName="SELECT" 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION").ToString().Length > 65 ? (DataBinder.Eval(Container, "DataItem.FLDSECTION").ToString().Substring(0, 65) + "...") : DataBinder.Eval(Container, "DataItem.FLDSECTION").ToString() %>'
                                    OnClientClick='<%# string.Format("javascript:SetParentIframeURL(\"{0}\",\"{1}\",\"{2}\")", Eval("FLDTYPE"), GetParentIframeURL(Eval("FLDSECTIONID").ToString()), Eval("FLDSECTIONID")) %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblSectionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucSectionName" TargetControlId="lnkSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION") %>' Width="400px" />
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
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadLabel ID="lblFormTitle" runat="server" Text="0 matches in form title" Style="font-weight: bold"></telerik:RadLabel>
            <telerik:RadGrid ID="gvFormMatch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                OnNeedDataSource="gvFormMatch_NeedDataSource" OnItemDataBound="gvFormMatch_RowDataBound" OnItemCommand="gvFormMatch_RowCommand"
                OnSelectedIndexChanging="gvFormMatch_SelectedIndexChanging" ShowFooter="true" EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDFORMID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Top Category">
                            <HeaderStyle Width ="25%" />
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTopCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <HeaderStyle Width ="25%" />
                            <HeaderTemplate>
                                Subcategory
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width ="50%" />
                            <HeaderTemplate>
                                Forms / Checklists / Circulars
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkFormName" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME").ToString().Length > 65 ? (DataBinder.Eval(Container, "DataItem.FLDNAME").ToString().Substring(0, 65) + "...") : DataBinder.Eval(Container, "DataItem.FLDNAME").ToString() %>'
                                    OnClientClick='<%# string.Format("javascript:SetParentIframeURL(\"{0}\",\"{1}\",\"{2}\")", Eval("FLDTYPE"),GetParentIframeURL(Eval("FLDFORMID").ToString()), Eval("FLDFORMID")) %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucFormName" TargetControlId="lnkFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' Width="400px" />
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
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadLabel ID="lblRiskAssessmentTitle" runat="server" Text="0 matches in RA's" Style="font-weight: bold"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnNeedDataSource="gvRiskAssessment_NeedDataSource"
                OnItemDataBound="gvRiskAssessment_RowDataBound" OnItemCommand="gvRiskAssessment_RowCommand"
                OnSelectedIndexChanging="gvRiskAssessment_SelectedIndexChanging" ShowFooter="true" EnableViewState="false" DataKeyNames="FLDPROCESSID">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDPROCESSID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                Type
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="18%"></ItemStyle>
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                Activity / Condition
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActivityCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYORCONDITION").ToString().Length > 45 ? (DataBinder.Eval(Container, "DataItem.FLDACTIVITYORCONDITION").ToString().Substring(0, 45) + "...") : DataBinder.Eval(Container, "DataItem.FLDACTIVITYORCONDITION").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucActivityCondition" TargetControlId="lblActivityCondition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYORCONDITION") %>'
                                    Width="400px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process Name">
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
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Ref Number
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO") %>' OnClientClick='<%# string.Format("javascript:SetParentIframeURL(\"{0}\",\"{1}\",\"{2}\")", Eval("FLDTYPE"),GetParentIframeURL(Eval("FLDPROCESSID").ToString()), Eval("FLDPROCESSID")) %>'></asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadLabel ID="lblJobHazardTitle" runat="server" Text="0 matches in JHA's" Style="font-weight: bold"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvJobHazard" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnNeedDataSource="gvJobHazard_NeedDataSource"
                OnItemDataBound="gvJobHazard_RowDataBound" OnItemCommand="gvJobHazard_RowCommand"
                OnSelectedIndexChanging="gvJobHazard_SelectedIndexChanging" ShowFooter="true" EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDJOBHAZARDID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                Type
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                Job
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB").ToString().Length > 42 ? (DataBinder.Eval(Container, "DataItem.FLDJOB").ToString().Substring(0, 42) + "...") : DataBinder.Eval(Container, "DataItem.FLDJOB").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucJob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                Ref Number
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>' OnClientClick='<%# string.Format("javascript:SetParentIframeURL(\"{0}\",\"{1}\",\"{2}\")", Eval("FLDTYPE"),GetParentIframeURL(Eval("FLDJOBHAZARDID").ToString()), Eval("FLDJOBHAZARDID")) %>'></asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
