<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDistributedDocumentList.aspx.cs"
    Inherits="DocumentManagementDistributedDocumentList" EnableEventValidation="true" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Documents/Forms</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />


        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <br />
            <table id="tblDocument" width="100%" align="center" runat="server">
                
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCategoryName" runat="server" CssClass="readonlytextbox" Width="200px" MaxLength="100"></telerik:RadTextBox>

                    </td>
                </tr>
            </table>
            <br />
            <b>
                <telerik:RadLabel ID="lblDocument" runat="server" Text="Documents"></telerik:RadLabel>
            </b>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocument" runat="server" AutoGenerateColumns="False" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Font-Size="11px"
                Width="100%" Height="50%" CellPadding="3" OnNeedDataSource="gvDocument_NeedDataSource" OnItemDataBound="gvDocument_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Auto" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDDOCUMENTID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Seq">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="40px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSNO" Text="S.No" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSequenceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="200px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text="Document"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <eluc:ToolTip ID="ucDocumentName" TargetControlId="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' />
                                <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME").ToString().Length > 80 ? (DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME").ToString().Substring(0, 80) + "...") : DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME").ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Company">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="60px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCompany" Text="Company" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompanyCode" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYSHORTCODE")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanyId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYID")) %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revision Number">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="150px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRevision" Text="Current Revision" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevisionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="395px" SaveScrollPosition="true" FrozenColumnsCount="7" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
            <br />
            <b>
                <telerik:RadLabel ID="lblForm" runat="server" Text="Forms"></telerik:RadLabel>
            </b>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvForm" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" Width="100%" Height="50%" CellPadding="3" OnItemDataBound="gvForm_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="false" >
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Auto" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDFORMID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="60px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFForm" Text="Form No." runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sort Order">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="150px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFName" Text="Form" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMREVISIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTION").ToString().Length > 60 ? (DataBinder.Eval(Container, "DataItem.FLDCAPTION").ToString().Substring(0, 60) + "...") : DataBinder.Eval(Container, "DataItem.FLDCAPTION").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucFilenameTT" runat="server" TargetControlId="lblDocumentName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTION") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="150px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFRemarks" Text="Remarks" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurpose" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString().Length > 60 ? (DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString().Substring(0, 60)+ "...") : DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucPurpose" TargetControlId="lblPurpose" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Company">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="60px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFCompany" Text="Company" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompanyCode" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYSHORTCODE")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanyId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYID")) %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revison Number">
                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFRevision" Text="Current Revision" runat="server"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVersionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="395px" SaveScrollPosition="true" FrozenColumnsCount="7" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

