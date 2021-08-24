<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementShowChanges.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="DocumentManagementShowChanges" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Mode</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDMSShowChanges" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <br />
            <table id="tblDMS" width="100%">
                <tr>
                    <td style="width: 10%;">From Date
                    </td>
                    <td style="width: 25%">
                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                    <td style="width: 10%">To Date
                    </td>
                    <td style="width: 25%">
                        <eluc:Date ID="ucToDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                            <eluc:Vessel runat="server" ID="ddlVessel" AppendDataBoundItems="true" 
                                AssignedVessels="true" Width="150px" />
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand"></eluc:TabStrip>

            <telerik:RadLabel ID="lblSectionTitle" runat="server" Text="0 changes in documents / sections"
                Style="font-weight: bold">
            </telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSectionChanges" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                Width="100%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellPadding="3" OnNeedDataSource="gvSectionChanges_NeedDataSource"
                OnItemCommand="gvSectionChanges_RowCommand" OnItemDataBound="gvSectionChanges_ItemDataBound" ShowFooter="true" ShowHeader="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDSECTIONREVISIONID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYFULLNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="17%"></ItemStyle>
                            <HeaderTemplate>
                                Document
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTFULLNAME").ToString().Length > 25 ? (DataBinder.Eval(Container, "DataItem.FLDDOCUMENTFULLNAME").ToString().Substring(0, 25) + "...") : DataBinder.Eval(Container, "DataItem.FLDDOCUMENTFULLNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucDocumentName" TargetControlId="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document Revision">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="11%"></ItemStyle>
                            <HeaderTemplate>
                                Document Revision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentRevision" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTREVISION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTREVISIONID")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Section">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <HeaderTemplate>
                                Section
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSECTIONFULLNAME").ToString().Length > 90 ? (DataBinder.Eval(Container, "DataItem.FLDSECTIONFULLNAME").ToString().Substring(0, 90) + "...") : DataBinder.Eval(Container, "DataItem.FLDSECTIONFULLNAME").ToString()%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSectionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucSectionName" TargetControlId="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONFULLNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Details of Change">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="40%"></ItemStyle>
                            <HeaderTemplate>
                                Details of Change
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDetailsofChange" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFICEREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Section Revision">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Section Revision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSectionRevision" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONREVISION") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblSectionRevision" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONREVISION") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblSectionRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONREVISIONID") %>'
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
            <telerik:RadLabel ID="lblFormTitle" runat="server" Text="0 changes in forms / posters / books"
                Style="font-weight: bold">
            </telerik:RadLabel>
            <telerik:RadGrid ID="gvFormChanges" RenderMode="Lightweight" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnNeedDataSource="gvFormChanges_NeedDataSource"
                OnItemDataBound="gvFormChanges_OnItemDataBound" OnItemCommand="gvFormChanges_ItemCommand"
                ShowFooter="true" ShowHeader="true" Style="margin-bottom: 0px"
                EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDFORMREVISIONID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Top Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Subcategory">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                Subcategory
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Forms / Checklists / Circulars">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50%"></ItemStyle>
                            <HeaderTemplate>
                                Forms / Checklists / Circulars
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME").ToString().Length > 90 ? (DataBinder.Eval(Container, "DataItem.FLDFORMNAME").ToString().Substring(0, 90) + "...") : DataBinder.Eval(Container, "DataItem.FLDFORMNAME").ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucFormName" TargetControlId="lblFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revision">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Revision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="hlnkRevison" Target="_blank" runat="server" Height="14px" Style="text-decoration: underline; cursor: pointer; color: Blue;"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMREVISION") %>'>
                                </asp:HyperLink>
                                <telerik:RadLabel ID="lblRevison" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMREVISION") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMREVISIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
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
            <telerik:RadLabel ID="lblJobHazardTitle" runat="server" Text="0 changes in JHA's" Style="font-weight: bold"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvJobHazard" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnNeedDataSource="gvJobHazard_NeedDataSource"
                OnItemDataBound="gvJobHazard_OnItemDataBound" OnItemCommand="gvJobHazard_OnItemCommand"
                ShowFooter="true" Style="margin-bottom: 0px"
                EnableViewState="false" ShowHeader="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDJOBHAZARDID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                Type
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <HeaderTemplate>
                                Job
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJob" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDJOB").ToString().Length > 90 ? (DataBinder.Eval(Container, "DataItem.FLDJOB").ToString().Substring(0, 90) + "...") : DataBinder.Eval(Container, "DataItem.FLDJOB").ToString()%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucJob" TargetControlId="lblJob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                Ref Number
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobHazardId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revision">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Revision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRevision" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHAREVISION") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblRevision" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHAREVISION") %>'
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
            <telerik:RadLabel ID="lblRiskAssessmentTitle" runat="server" Text="0 changes in RA's" Style="font-weight: bold"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvProcessRA" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                Width="100%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellPadding="3" OnNeedDataSource="gvProcessRA_NeedDataSource"
                OnItemDataBound="gvProcessRA_OnItemDataBound" OnItemCommand="gvProcessRA_OnItemCommand"
                ShowFooter="true" ShowHeader="true" Style="margin-bottom: 0px"
                EnableViewState="false" DataKeyNames="FLDRISKASSESSMENTPROCESSID">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDRISKASSESSMENTPROCESSID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                Type
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity / Condition">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <HeaderTemplate>
                                Activity / Condition
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROCESS").ToString().Length > 60 ? (DataBinder.Eval(Container, "DataItem.FLDPROCESS").ToString().Substring(0, 60) + "...") : DataBinder.Eval(Container, "DataItem.FLDPROCESS").ToString()%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucProcessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%" />
                            <HeaderTemplate>
                                Ref Number
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFNO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblProcessId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTPROCESSID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revision">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Revision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRevision" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREVISION") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblRevision" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREVISION")%>'
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
