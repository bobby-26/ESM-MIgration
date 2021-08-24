<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementAdminDocumentListNew.aspx.cs" Inherits="DocumentManagement_DocumentManagementAdminDocumentListNew" %>

<!DOCTYPE html >

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>Document</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function fnConfirmTelerik(sender, msg) {
                var callBackFn = function (shouldSubmit) {
                    if (shouldSubmit) {
                        //sender.click();
                        //if (Telerik.Web.Browser.ff) {
                        //    sender.get_element().click();
                        //}
                        eval(sender.target.parentElement.parentElement.href);
                    }
                    else {
                        if (e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                var confirm;

                if (msg == null)
                    confirm = radconfirm("Are you sure you want to delete this record?", callBackFn);
                else
                    confirm = radconfirm(msg, callBackFn);

                return false;
            }
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvDocument.ClientID %>"));
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
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand" Visible="false"
                TabStrip="true"></eluc:TabStrip>
            <table id="tblDocument" width="50%" align="center" runat="server" visible="false">
                <tr>
                    <td>Keyword
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDocumentName" runat="server" CssClass="input" Width="200px" MaxLength="100"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuDocumentList" runat="server" TabStrip="false" OnTabStripCommand="MenuDocumentList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDocument" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" Font-Size="11px" Width="100%" CellPadding="3" Height="87%" OnNeedDataSource="gvDocument_NeedDataSource"
                OnItemCommand="gvDocument_ItemCommand" OnItemDataBound="gvDocument_ItemDataBound" ShowFooter="false"
                OnRowDeleting="gvDocument_RowDeleting" OnUpdateCommand="gvDocument_UpdateCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="Bottom" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" DataKeyNames="FLDDOCUMENTID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" FieldAlias="Category" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" FieldAlias="Category" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="3%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSequenceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtSequenceNumberEdit" runat="server" CssClass="gridinput_mandatory"
                                    IsInteger="true" MaxLength="3" IsPositive="true" Width="99%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtSequenceNumberAdd" runat="server" CssClass="gridinput_mandatory"
                                    IsInteger="true" MaxLength="3" IsPositive="true" Width="99%"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="17%" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDocumentName" runat="server" CommandName="NAVIGATE"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME").ToString().Length > 40 ? (DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME").ToString().Substring(0, 40) + "...") : DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME").ToString() %>'></asp:LinkButton>
                                <eluc:ToolTip ID="ucDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' TargetControlId="lnkDocumentName" />
                                <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDocumentIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadTextBox ID="txtDocumentNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="99%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDocumentNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    Width="99%" MaxLength="200">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="17%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlDocumentCategoryEdit" runat="server" CssClass="gridinput_mandatory"
                                    Width="0px" Visible="false">
                                </telerik:RadDropDownList>
                                <telerik:RadLabel ID="lblCategoryNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'
                                    Width="0px" Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCategoryIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                    Width="0px" Visible="false">
                                </telerik:RadLabel>
                                <span id="spnPickListCategoryedit">
                                    <telerik:RadTextBox ID="txtCategoryEdit" runat="server" Width="170px" Enabled="False" CssClass="input_mandatory"></telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowCategoryEdit" runat="server" Text="..">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtCategoryidEdit" runat="server" Width="1px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlDocumentCategoryAdd" runat="server" CssClass="gridinput_mandatory"
                                    Width="0px" Visible="false">
                                </telerik:RadDropDownList>
                                <span id="spnPickListCategoryAdd">
                                    <telerik:RadTextBox ID="txtCategoryAdd" runat="server" Width="99%" Enabled="False" CssClass="input_mandatory"></telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowCategoryAdd" runat="server" Text="..">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtCategoryidAdd" runat="server" Width="1px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="4%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVESTATUS")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server" Checked="true"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <%--                            <asp:TemplateField HeaderText="Comment Accepted Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                Comment Accepted Y/N
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCommentAcceptedYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACCEPTEDYNSTATUS")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <telerik:GridTemplateColumn HeaderText="Company" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="8.5%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompanyCode" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYSHORTCODE")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanyId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYID")) %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Company ID="ucCompanyEdit" runat="server" Enabled="false" Width="99%" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Company ID="ucCompanyAdd" runat="server" Enabled="false" Width="99%" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Added" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Added By" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Published" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPublishedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAPPROVEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucPublishedDateEdit" runat="server" Width="99%" CssClass="gridinput_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAPPROVEDDATE")) %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revision" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="12%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevisionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Published" Visible="false" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="7%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPublishedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Publish Required" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRevisionPublished" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TOPUBLISHED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Whole Manual Read" >
                            <HeaderStyle Width="5%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWholeManualRead" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWHOLEMANUALREAD") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWholemanualreadyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWHOLEMANUALREADYN") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION" AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="APPROVE"
                                    CommandName="APPROVE" ID="cmdApprove"
                                    ToolTip="Publish Document"><span class="icon"><i class="fas fa-file-pd"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="View Document"
                                    CommandName="VIEWDOCUMENT" ID="cmgViewContent"
                                    ToolTip="View Document"><span class="icon"><i class="fas fa-glasses-tv"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="View Revisions"
                                    CommandName="VIEWREVISION" ID="cmdRevision"
                                    ToolTip="View Revisions"><span class="icon"><i class="fas fa-copy"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Vessel List"
                                    CommandName="VESSELLIST" ID="cmdVesselList"
                                    ToolTip="Distributed Vessel List"><span class="icon"><i class="fas fa-ship"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Forms and Checklist" CommandName="FORMS" ID="cmdForms"
                                    ToolTip="Linked Forms and Checklist"><span class="icon"><i class="fa-file-contract-af"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="ReadUnread" CommandName="READUNREAD" ID="lnkReadUnread"
                                    ToolTip="Read/Unread User List"><span class="icon"><i class="fa-coins-ei"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="User Configuration" CommandName="CONFIGURATION" ID="cmdConfig"
                                    ToolTip="Reading User Configuration"><span class="icon"><i class="fas fa-user-tie"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Data" CommandName="BASE64" ID="cmdbase64"
                                    ToolTip="Image Base64 Update"><span class="icon"><i class="fa-check-select"></i></span></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" ID="cmdSave"
                                    ToolTip="Save"><span class="icon"><i class="fas fa-save"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"><span class="icon"><i class="fa fa-plus-circle"></i></span></asp:LinkButton>
                            </FooterTemplate>
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
