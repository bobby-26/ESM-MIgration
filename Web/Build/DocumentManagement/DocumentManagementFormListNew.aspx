<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFormListNew.aspx.cs" Inherits="DocumentManagement_DocumentManagementFormListNew" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form</title>
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
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvForm.ClientID %>"));
                }, 200);
                }
                window.onresize = window.onload = Resize;
        </script>
        <style type="text/css">
            .lblheader {
                font-weight: bold;
            }
        </style>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmField" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvForm" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="92%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuDocument" runat="server" Visible="false" OnTabStripCommand="MenuDocument_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <br />
            <table id="tblFind" runat="server" width="100%">
                <tr>
                    <td style="width: 30px">Form No.
                    </td>
                    <td style="width: 35px">
                        <telerik:RadTextBox ID="txtFormNo" runat="server" CssClass="input" Width="150px" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td style="width: 40px">Form Name
                    </td>
                    <td style="width: 35px">
                        <telerik:RadTextBox ID="txtFormName" runat="server" CssClass="input" Width="150px" MaxLength="100"></telerik:RadTextBox>
                    </td>
                    <td style="width: 30px">Category
                    </td>
                    <td style="width: 35px">
                        <span id="spnPickListCategory">
                            <telerik:RadTextBox ID="txtCategory" runat="server" Width="200px" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowCategory" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtCategoryid" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                    <%-- <td style="width: 30px">Content
                    </td>
                     <td style="width: 35px">
                        <telerik:RadTextBox ID="txtcontent" runat="server" CssClass="input" Width="150px" MaxLength="100"></telerik:RadTextBox>
                    </td>--%>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuRegistersCountry" runat="server" OnTabStripCommand="RegistersCountry_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvForm" runat="server" Height="92%" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true" GroupingEnabled="false"
                CellSpacing="0" GridLines="None" Font-Size="11px" Width="100%" CellPadding="3" OnNeedDataSource="gvForm_NeedDataSource" EnableHeaderContextMenu="true"
                OnItemCommand="gvForm_ItemCommand" OnItemDataBound="gvForm_ItemDataBound" ShowFooter="false" EnableViewState="true" AutoGenerateColumns="false"
                OnRowDeleting="gvForm_RowDeleting" OnUpdateCommand="gvForm_UpdateCommand" ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AllowNaturalSort="false" GroupsDefaultExpanded="true" AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None"
                    GroupLoadMode="Client" DataKeyNames="FLDFORMID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <HeaderStyle Width="3%" />
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
                    <NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDFORMID" DetailKeyField="FLDFORMID" />
                        </ParentTableRelation>
                    </NestedViewSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" Visible="false" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" Width="2%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" CommandName="EXPAND"
                                    ImageUrl="<%$ PhoenixTheme:images/sidearrow.png %>" ID="cmdBDetails" ToolTip="Form Details"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowSorting="false" ShowSortIcon="true" AllowFiltering="true">
                            <HeaderStyle HorizontalAlign="Center" Width="2.5%"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadCheckBox ID="chkAllForm" runat="server" Text="" AutoPostBack="true" OnClick="CheckAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form NO." AllowSorting="false" ShowSortIcon="true" AllowFiltering="true" UniqueName="FormNO">
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtFormNoEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'
                                    Width="80px">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <%-- <FooterTemplate>
                                <telerik:RadTextBox ID="txtFormNoAdd" runat="server" CssClass="gridinput_mandatory" Width="50px"
                                    MaxLength="50">
                                </telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="Name">
                            <HeaderStyle Width="18%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMREVISIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" runat="server" ToolTip="Download Form"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCAPTION") %>'>
                                </asp:HyperLink>
                                <eluc:ToolTip ID="ucFilenameTT" runat="server" TargetControlId="lnkfilename" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTION") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFormIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadTextBox ID="txtFormNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTION") %>'
                                    CssClass="gridinput_mandatory">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <%-- <FooterTemplate>
                                <telerik:RadTextBox ID="txtFormNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="100"></telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>

                        <%--<telerik:GridTemplateColumn HeaderText="Published">
                          <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblPublishedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucPublishedDateEdit" runat="server" CssClass="gridinput_mandatory"
                                        Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="Type">
                            <HeaderStyle Width="5%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblTypeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:RadioButtonList ID="rListEdit" runat="server" RepeatDirection="Vertical">
                                    <asp:ListItem Text="Desined Form" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Upload" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </EditItemTemplate>
                            <%-- <FooterTemplate>
                                <asp:RadioButtonList ID="rListAdd" runat="server" RepeatDirection="Vertical" ValidationGroup="type">
                                    <asp:ListItem Text="Design" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Upload" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="Category">
                            <HeaderStyle Width="6%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCategoryNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCategoryIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadDropDownList ID="ddlCategoryIdEdit" runat="server" CssClass="gridinput_mandatory"
                                    Visible="false">
                                </telerik:RadDropDownList>
                                <span id="spnPickListCategoryedit">
                                    <telerik:RadTextBox ID="txtCategoryEdit" runat="server" Width="120px" Enabled="False" CssClass="input_mandatory"></telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowCategoryEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." />
                                    <telerik:RadTextBox ID="txtCategoryidEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterStyle Wrap="false" />
                            <%--  <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlCategoryIdAdd" runat="server" Visible="false" CssClass="gridinput_mandatory">
                                </telerik:RadDropDownList>
                                <span id="spnPickListCategoryAdd">
                                    <telerik:RadTextBox ID="txtCategoryAdd" runat="server" Width="120px" Enabled="False" CssClass="input_mandatory"></telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowCategoryAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." />
                                    <telerik:RadTextBox ID="txtCategoryidAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="Active">
                            <HeaderStyle Width="4%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVESTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>' />
                            </EditItemTemplate>
                            <%--  <FooterTemplate>
                                <asp:CheckBox ID="chkActiveYNAdd" runat="server" />
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="Remarks">
                            <HeaderStyle Width="6%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurpose" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString().Length > 14 ? DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString().Substring(0, 14) : DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucPurpose" runat="server" TargetControlId="lblPurpose" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPurposeEdit" runat="server" Width="70px" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <%--  <FooterTemplate>
                                <telerik:RadTextBox ID="txtPurposeAdd" runat="server" Width="70px" CssClass="input" MaxLength="100"></telerik:RadTextBox>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Company" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="Company">
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompanyCode" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYSHORTCODE")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanyId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYID")) %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Company ID="ucCompanyEdit" runat="server" Width="80px" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </EditItemTemplate>
                            <%--  <FooterTemplate>
                                <eluc:Company ID="ucCompanyAdd" runat="server" Width="80px" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Added date" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="AddedDate">
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Added By" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="AddedBy">
                            <HeaderStyle Width="6%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddedByName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAUTHORNAME").ToString().Length > 14 ? DataBinder.Eval(Container, "DataItem.FLDAUTHORNAME").ToString().Substring(0, 14) : DataBinder.Eval(Container, "DataItem.FLDAUTHORNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucAddedByNameTT" runat="server" TargetControlId="lblAddedByName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- <telerik:GridTemplateColumn HeaderText="Published">
                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                            <HeaderTemplate>
                                Published
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVersionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Revison" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="Revison">
                            <HeaderStyle Width="11%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVersionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Draft" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="Draft">
                            <HeaderStyle Width="7%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:HyperLink ID="hlnkDraftName" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAFTREVISION") %>'
                                    Height="14px" ToolTip="Download Form" Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                <telerik:RadLabel ID="lblDraftRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAFTREVISIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Published" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="Published">
                            <HeaderStyle Width="6%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblPublishedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucPublishedDateEdit" runat="server" CssClass="gridinput_mandatory"
                                    Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false" UniqueName="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="17%"></HeaderStyle>
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <%--  <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT"
                                    ID="cmdEdit" ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                --%>
                                <asp:LinkButton runat="server" AlternateText="EditDetails"
                                    CommandName="EDITDETAILS" ID="cmdEditDetails" ToolTip="Edit Details"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" ID="cmdDelete" ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="UPLOAD" ID="cmdUpload"
                                    CommandName="UPLOAD" Visible="false" ToolTip="Upload Form"><span class="icon"><i class="fas fa-upload"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Create"
                                    CommandName="DESIGNEDFORM" ID="cmdDesignFormUpload"
                                    Visible="false" ToolTip="Design Form Upload"><span class="icon"><i class="fas fa-receipt"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="APPROVE"
                                    CommandName="APPROVE" ID="cmdApprove" ToolTip="Approve & Publish"><span class="icon"><i class="fas fa-file-approve"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="View Revisions"
                                    CommandName="VIEWREVISION" ID="cmdRevision"
                                    ToolTip="View Revisions"><span class="icon"><i class="fas fa-copy-requisition"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="View Revisions"
                                    CommandName="REVISIONRESET" ID="cmdRevisionReset" ToolTip="Reset Revision"><span class="icon"><i class="fas fa-undo"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Vessel List"
                                    CommandName="VESSELLIST" ID="cmdVesselList"
                                    ToolTip="Distributed Vessel List"><span class="icon"><i class="fas fa-ship"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Documents" CommandName="DOCUMENTS" ID="cmdDocuments"
                                    ToolTip="Linked Documents"><span class="icon"><i class="fas fa-proposeST"></i></span></asp:LinkButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" ID="cmdSave"
                                    ToolTip="Save"><span class="icon"><i class="fas fa-save"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel"><span class="icon"><i class="fas fa-times"></i></span></asp:LinkButton>
                            </EditItemTemplate>
                            <%--   <FooterStyle HorizontalAlign="Center" />--%>
                            <%--  <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"><span class="icon"><i class="fa fa-plus-circle"></i></span></asp:LinkButton>
                            </FooterTemplate>--%>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NestedViewTemplate>
                        <table style="width: 1000px;" align="left">
                            <tr>
                                <td><b>Form Details</b></td>
                            </tr>
                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblFileNo" runat="server" Text="Form No :"></telerik:RadLabel>

                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                                </td>
                                <%--   <td class="lblheader">
                                    <telerik:RadLabel ID="lblPrimaryOwnerShip" runat="server" Text="Primary Owner Ship :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltPrimaryOwnerShip" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRIMARYOWNERSHIP") %>'></telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblSecondaryOwnerShip" runat="server" Text="Secondary Owner Ship :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltSecondaryOwnerShip" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSECONDARYOWNERSHIP") %>'></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblOtherParticipants" runat="server" Text="Other Participants :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltOtherParticipants" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERPARTICIPANTS") %>'></telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblPrimaryOwnerOffice" runat="server" Text="Primary Owner Office :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltPrimaryOwnerOffice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIMARYOWNEROFFICE") %>'></telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblSecondaryOwnerOffice" runat="server" Text="Secondary Owner Office :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltSecondaryOwnerOffice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDARYOWNEROFFICE") %>'></telerik:RadLabel>
                                </td>--%>
                            </tr>

                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblShipDept" runat="server" Text="Ship Department :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltShipDept" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPDEPARTMENT") %>'></telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblOfficeDept" runat="server" Text="Office Department :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltOfficeDept" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEDEPARTMENT") %>'></telerik:RadLabel>
                                </td>
                                <%-- <td class="lblheader">
                                    <telerik:RadLabel ID="lblShipType" runat="server" Text="Ship Type :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltShipType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPTYPE") %>'></telerik:RadLabel>
                                </td>--%>
                            </tr>

                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblcountry" runat="server" Text="Country/Port :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltcountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYPORT") %>'></telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblEqMaker" runat="server" Text="Equipment Maker/Model :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltEqMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENTMAKER") %>'></telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblPMSComp" runat="server" Text="PMS Component/Work Order :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltPMSComp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPMSCOMPONENT") %>'></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblActivity" runat="server" Text="Activity/Operation :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="txtActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>'></telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblTimeInterval" runat="server" Text="Time Interval :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltTimeInterval" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMEINTERVAL") %>'></telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblProcedure" runat="server" Text="Procedure :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltProcedure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURE") %>'></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblRA" runat="server" Text="RA :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbtRA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRA") %>'></telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblJHA" runat="server" Text="JHA :"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltJHA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHA") %>'></telerik:RadLabel>
                                </td>
                                <td class="lblheader"></td>
                                <td></td>
                            </tr>
                        </table>
                    </NestedViewTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowExpandCollapse="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="395px" SaveScrollPosition="true" FrozenColumnsCount="7" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
