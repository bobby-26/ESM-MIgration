<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionTMSAMatrixCategory.aspx.cs" Inherits="Inspection_InspectionTMSAMatrixCategory" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Document Category</title>
    <telerik:RadCodeBlock runat="server" ID="DivHeader">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <style>
        /*Vertical Splitbars*/
        .rspCollapseBarExpand, .rspCollapseBarExpandOver,
        .rspCollapseBarCollapse, .rspCollapseBarCollapseOver{
            height: 35px !important; /*the height of your button-image */
            line-height: 35px !important; /*the height of your button-image */
            width: 10px !important;
            background-position: 0 !important;
        }
        .RadSplitter .rspCollapseBarExpand:before,
        .RadSplitter .rspCollapseBarCollapse:before {
            font-size: 14px !important;
            width: 10px !important;
        } 
        </style>

        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
               && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocumentCategory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxPanel1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwDocumentCategory">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tvwDocumentCategory" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuDocumentCategoryMain" runat="server" Title="TMSA/ISM/ISO Matrix" OnTabStripCommand="MenuDocumentCategoryMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" SplitBarsSize="10" runat="server" Height="100%" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="30%" Height="100%">
                <eluc:TreeView runat="server" ID="tvwDocumentCategory" RootText="ROOT" OnNodeClickEvent="ucTree_SelectNodeEvent"></eluc:TreeView>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward" Height="100%">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Width="70%" Height="100%">
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <table>
                        <tr style="position: absolute">
                            <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCategoryId" runat="server"></telerik:RadLabel>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="5">

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAuditInspection" runat="server" Text="Audit"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlAudit" runat="server" Width="200px"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                                <eluc:Inspection runat="server" ID="ucAudit" Visible="false" AppendDataBoundItems="true"
                                  Width="155px" />
                                <eluc:Hard ID="ucAuditType" runat="server" Visible="false" ShortNameFilter="AUD"
                                    AutoPostBack="true" HardTypeCode="148" OnTextChangedEvent="Bind_UserControls" />
                            </td>
                        </tr>
                        <tr>
                            <td>Category Name
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDocumentCategory" CssClass="input_mandatory" Width="180px" Height="20px" MaxLength="100" runat="server"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Category Number
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCategoryNumber" runat="server" CssClass="gridinput_mandatory"
                                    onkeypress="return isNumberKey(event)" Width="180px" Height="20px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr runat="server" visible="false">
                            <td>Category Short Code
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtcategoryshortcode" CssClass="input_mandatory" Width="180px" Height="20px" MaxLength="100" runat="server"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Company</td>
                            <td>
                                <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="180px" Height="20px" />
                            </td>
                        </tr>
                        <tr>
                            <td>Active
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkActiveyn" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server" id="tblnoofcolumns">
                            <td>No of Columns
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtnoofcolumns" runat="server" CssClass="gridinput_mandatory"
                                    onkeypress="return isNumberKey(event)" Width="180px" Height="20px">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:Button ID="btn" runat="server" Visible="false" Text="Submit" OnClick="btn_Click" />
                            </td>
                        </tr>
                        <tr runat="server"  id="tblnoofstages">
                            <td>No of Stages
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtnoofstages" runat="server" CssClass="gridinput_mandatory"
                                    onkeypress="return isNumberKey(event)" Width="180px" Height="20px">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" Visible="false" Text="Submit" OnClick="btn_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Pnlcolumnlabel" runat="server">
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlcolumns" runat="server">
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <%--                <eluc:TabStrip ID="MenuPhoenixQuery" runat="server" OnTabStripCommand="MenuPhoenixQuery_TabStripCommand">
                </eluc:TabStrip>--%>
                        </tr>

                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                    <eluc:TabStrip ID="MenuPhoenixQuery" runat="server" OnTabStripCommand="MenuPhoenixQuery_TabStripCommand"></eluc:TabStrip>

                    <telerik:RadGrid RenderMode="Lightweight" ID="gvTMSAMatrix" runat="server" AutoGenerateColumns="false" Font-Size="11px" GridLines="None"
                        Width="100%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellPadding="3" OnNeedDataSource="gvTMSAMatrix_NeedDataSource"
                        OnItemCommand="gvTMSAMatrix_RowCommand" OnItemDataBound="gvTMSAMatrix_ItemDataBound" ShowFooter="true">
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                            AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="None"
                            GroupsDefaultExpanded="true" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" DataKeyNames="FLDID">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                            <HeaderStyle Width="102px" />
<%--                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" FieldAlias="Category" />
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" FieldAlias="Category" />
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>--%>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Objective Evidence">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblid1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBJECTIVEEVIDENCE" ) %>'></telerik:RadLabel>
                                        <%--                        <telerik:RadLabel ID="lblprocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDUREID" ) %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblcategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTMSACATEGORYID" ) %>'></telerik:RadLabel>--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Procedures">
                                    <ItemTemplate>
                                        <div id="divForms" runat="server" style="height: 75px; width: auto; border-width: 1px; border-style: solid; border: 0px solid #9f9fff">
                                            <table id="tblForms" runat="server">
                                            </table>
                                        </div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID" ) %>'></telerik:RadLabel>
                                        <asp:LinkButton runat="server" ID="Edit" CommandArgument="<%# Container.DataItem %>"
                                            CommandName="EDIT"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" Visible="false" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>
                                        <%--                        <telerik:RadLabel ID="lblprocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDUREID" ) %>'></telerik:RadLabel>
                        <telerik:RadLabel ID="lblcategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTMSACATEGORYID" ) %>'></telerik:RadLabel>--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />                            
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                                <asp:Button ID="confirm" runat="server" Text="confirm" PostBackUrl="../Inspection/InspectionTMSAMatrixCategory.aspx" OnClick="confirm_Click" />
                </telerik:RadAjaxPanel>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
