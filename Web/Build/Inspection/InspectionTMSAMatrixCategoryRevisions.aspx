<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionTMSAMatrixCategoryRevisions.aspx.cs" Inherits="Inspection_InspectionTMSAMatrixCategoryRevisions" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form Revisions</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
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

    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegistersCountry" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuRegistersCountry" runat="server" OnTabStripCommand="RegistersCountry_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFormRevision" runat="server" Font-Size="11px"
                Width="100%" CellPadding="3" GridLines="None" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                OnNeedDataSource="gvFormRevision_NeedDataSource" OnItemCommand="gvFormRevision_ItemCommand"
                OnItemDataBound="gvFormRevision_ItemDataBound" ShowFooter="true" ShowHeader="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDREVISIONID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <HeaderTemplate>
                                Added Date
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%-- <asp:LinkButton ID="lnkAddedDate" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></asp:LinkButton>--%>
                                <asp:HyperLink ID="hlnkAddedDate" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'
                                    Height="14px" ToolTip="Download Form" Style="text-decoration: underline; cursor: pointer; color: Blue;"></asp:HyperLink>
                                <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Added By">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Added By
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revison Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Revision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVersionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Published Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Published Date
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApprovedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblApprovedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Published By">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Published By
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApprovedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Select"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="View Content" CommandName="VIEWCONTENT" ID="cmgViewContent"
                                    ToolTip="View Content" Visible="false"><span class="icon"><i class="fas fa-receipt"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="APPROVE"
                                    CommandName="APPROVE" ID="cmdApprove"
                                    ToolTip="Approve"><span class="icon"><i class="fas fa-file"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="NEWREVISION"
                                    CommandName="NEWREVISION" ID="cmdCreate"
                                    ToolTip="New Revision" Visible="false"><span class="icon"><i class="fas fa-receipt"></i></span></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONID" ) %>'></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="Comments" CommandName="INFO" ID="cmdMoreInfo"
                                    ToolTip="Comments"><span class="icon"><i class="fas fa-file"></i></span></asp:LinkButton>
                                <%--                        <telerik:RadLabel ID="lblprocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDUREID" ) %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblcategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTMSACATEGORYID" ) %>'></telerik:RadLabel>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="395px" SaveScrollPosition="true" FrozenColumnsCount="7" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <br />
            <eluc:TabStrip ID="MenuField" runat="server" OnTabStripCommand="MenuField_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFormField" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowDataBound="gvFormField_RowDataBound" OnRowDeleting="gvFormField_RowDeleting"
                OnRowUpdating="gvFormField_RowUpdating" OnRowCommand="gvFormField_RowCommand"
                OnRowEditing="gvFormField_RowEditing" Style="margin-bottom: 0px" OnSelectedIndexChanging="gvFormField_SelectedIndexChanging"
                EnableViewState="false" ShowFooter="true" OnRowCancelingEdit="gvFormField_RowCancelingEdit">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None" DataKeyNames="FLDFIELDID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Sort Order">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Sort Order
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFieldId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMREVISIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucSortOrderEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'
                                    IsPositive="true" MaxLength="5" DecimalPlace="1" CssClass="gridinput_mandatory"
                                    Width="40px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucSortOrderAdd" runat="server" IsPositive="true" MaxLength="5" DecimalPlace="1"
                                    CssClass="gridinput_mandatory" Width="40px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Label">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Label
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLabel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLABEL") %>'
                                    Style="word-wrap: break-word;" Width="300px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLabelEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLABEL") %>'
                                    CssClass="input" Width="300px"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtLabelAdd" runat="server" CssClass="input" Width="300px"></asp:TextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bold">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Bold Y/N
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBold" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBOLDSTATUS") %>'
                                    Width="40px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkBoldEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDBOLDYN").ToString().Equals("1"))?true:false %>'
                                    Width="40px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkBoldAdd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Type
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTypeName" runat="server" CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlTypeEdit" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTypeEdit_Changed" Width="120px">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlTypeAdd" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTypeAdd_Changed" Width="120px">
                                </asp:DropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Width">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Width
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaxSize" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucMaxSizeEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZE") %>'
                                    IsInteger="true" IsPositive="true" MaxLength="3" DecimalPlace="0" Width="40px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucMaxSizeAdd" runat="server" IsInteger="true" IsPositive="true"
                                    MaxLength="3" DecimalPlace="0" CssClass="input" Width="40px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Height">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Height
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHeight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEIGHT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucHeightEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEIGHT") %>'
                                    IsInteger="true" IsPositive="true" MaxLength="3" DecimalPlace="0" Width="40px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucHeightAdd" runat="server" IsInteger="true" IsPositive="true"
                                    MaxLength="3" DecimalPlace="0" CssClass="input" Width="40px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Precision">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Precision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrecision" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRECISION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucPrecisionEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRECISION") %>'
                                    IsInteger="true" IsPositive="true" MaxLength="2" DecimalPlace="0" Width="40px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucPrecisionAdd" runat="server" IsInteger="true" IsPositive="true"
                                    MaxLength="2" DecimalPlace="0" CssClass="input" Width="40px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Scale">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Scale
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScale" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCALE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucScaleEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCALE") %>'
                                    IsInteger="true" IsPositive="true" MaxLength="2" DecimalPlace="0" Width="40px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucScaleAdd" runat="server" IsInteger="true" IsPositive="true" MaxLength="2"
                                    DecimalPlace="0" CssClass="input" Width="40px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rows" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Rows
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRows" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucRowsEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWS") %>'
                                    IsInteger="true" IsPositive="true" MaxLength="2" DecimalPlace="0" Width="40px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucRowsAdd" runat="server" IsInteger="true" IsPositive="true" MaxLength="2"
                                    DecimalPlace="0" CssClass="input" Width="40px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Columns" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Columns
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblColumns" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucColumnsEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMNS") %>'
                                    IsInteger="true" IsPositive="true" MaxLength="2" DecimalPlace="0" Width="40px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucColumnsAdd" runat="server" IsInteger="true" IsPositive="true"
                                    MaxLength="2" DecimalPlace="0" CssClass="input" Width="40px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Mandatory">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Mandatory Y/N
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANDATORYSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkMandatoryEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDMANDATORY").ToString().Equals("1"))?true:false %>'
                                    Width="80px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkMandatoryAdd" runat="server" Width="40px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Default Value" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Default Value
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDefaultValue" runat="server" Text=""></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDefaultValueEdit" runat="server" Text="" CssClass="input"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDefaultValueAdd" runat="server" CssClass="input"></asp:TextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit"
                                    ToolTip="Edit"></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="CHOICE"
                                    CommandName="CHOICE" ID="imgChoice"
                                    ToolTip="Add Values" Visible="false"><span class="icon"><i class="fas fa-file"></i></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" ID="cmdSave"
                                    ToolTip="Save"><span class="icon"><i class="fas fa-save"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel"><span class="icon"><i class="fas fa-times"></i></span></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"><span class="icon"><i class="fa fa-plus-circle"></i></span></asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
