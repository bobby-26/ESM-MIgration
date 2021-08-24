<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSOfficeFormList.aspx.cs"
    Inherits="DocumentManagementFMSOfficeFormList" %>

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
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvForm.ClientID %>"));
               }, 200);
           }
           window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
           }
        </script>
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
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1">
        </telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvForm" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuFMS" runat="server" OnTabStripCommand="MenuFMS_TabStripCommand" Visible="false"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuDocument" runat="server" Visible="false" OnTabStripCommand="MenuDocument_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <table id="tblFind" runat="server" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfileno" Text="File No." runat="server">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFormNo" runat="server" CssClass="input" Width="150px"
                            MaxLength="20">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblformname" Text="Form Name" runat="server">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFormName" runat="server" CssClass="input" Width="150px"
                            MaxLength="100">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" Text="Category" runat="server">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListCategory">
                            <telerik:RadTextBox ID="txtCategory" runat="server" Width="200px" CssClass="input">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowCategory" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtCategoryid" runat="server" Width="0px" CssClass="hidden">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselname" runat="server" Text="Vessel Name" Visible="false">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlvessel" runat="server" CssClass="input" AppendDataBoundItems="true"
                            Visible="false" AssignedVessels="true" AutoPostBack="true" Width="240px" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuRegistersCountry" runat="server" OnTabStripCommand="RegistersCountry_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvForm" runat="server" AllowPaging="true"
                AllowCustomPaging="true" AllowSorting="true" CellSpacing="0" GridLines="None"
                Font-Size="11px" Width="100%" CellPadding="3" OnNeedDataSource="gvForm_NeedDataSource"
                OnItemCommand="gvForm_ItemCommand" OnItemDataBound="gvForm_ItemDataBound"
                ShowFooter="false" EnableViewState="true" OnRowDeleting="gvForm_RowDeleting"
                OnUpdateCommand="gvForm_UpdateCommand" ShowHeader="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    GroupsDefaultExpanded="true" AutoGenerateColumns="false" TableLayout="Fixed"
                    EnableHeaderContextMenu="true" CommandItemDisplay="top" GroupHeaderItemStyle-Font-Bold="true"
                    GroupLoadMode="Client" DataKeyNames="FLDFORMID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false"
                        ShowRefreshButton="false" />
                    <HeaderStyle Width="102px" />
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
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="20px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" CommandName="EXPAND" ImageUrl="<%$ PhoenixTheme:images/sidearrow.png %>"
                                    ID="cmdBDetails" ToolTip="Form Details"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                            <HeaderTemplate>
                                File No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sort Order">
                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                            <HeaderTemplate>
                                Name
                            </HeaderTemplate>
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
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Published" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblPublishedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle HorizontalAlign="Center" Width="75px"></HeaderStyle>
                            <HeaderTemplate>
                                Type
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Label" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Category
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <HeaderTemplate>
                                Active
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVESTATUS") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <HeaderTemplate>
                                Remarks
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurpose" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString().Length > 14 ? DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString().Substring(0, 14) : DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucPurpose" runat="server" TargetControlId="lblPurpose" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Company" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Company
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompanyCode" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYSHORTCODE")) %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompanyId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPANYID")) %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Added date" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Added Date
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Added By" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Added By
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddedByName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAUTHORNAME").ToString().Length > 14 ? DataBinder.Eval(Container, "DataItem.FLDAUTHORNAME").ToString().Substring(0, 14) : DataBinder.Eval(Container, "DataItem.FLDAUTHORNAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucAddedByNameTT" runat="server" TargetControlId="lblAddedByName"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revison Number" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                Revision
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVersionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Draft" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                            <HeaderTemplate>
                                Draft
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="hlnkDraftName" Target="_blank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAFTREVISION") %>'
                                    Height="14px" ToolTip="Download Form" Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                <telerik:RadLabel ID="lblDraftRevisionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAFTREVISIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--                            <asp:LinkButton runat="server" AlternateText="UPLOAD" ID="cmdUpload" CommandName="UPLOAD"
                                Visible="false" ToolTip="Upload Form"><span class="icon"><i class="fas fa-file-upload"></i></span></asp:LinkButton>--%>
                                <asp:LinkButton runat="server" AlternateText="Vessel List" CommandName="VESSELLIST"
                                    ID="cmdVesselList" ToolTip="Distributed Vessel List"><span class="icon"><i class="fas fa-ship"></i></span></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NestedViewTemplate>
                        <table style="width: 1000px;" align="left">
                            <tr>
                                <td>
                                    <b>Form Details</b>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'>
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblShipDept" runat="server" Text="Ship Department :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltShipDept" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPDEPARTMENT") %>'>
                                    </telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblOfficeDept" runat="server" Text="Office Department :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltOfficeDept" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPDEPARTMENT") %>'>
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblcountry" runat="server" Text="Country/Port :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltcountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYPORT") %>'>
                                    </telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblEqMaker" runat="server" Text="Equipment Maker/Model :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltEqMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENTMAKER") %>'>
                                    </telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblPMSComp" runat="server" Text="PMS Component/Work Order :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltPMSComp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPMSCOMPONENT") %>'>
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblActivity" runat="server" Text="Activity/Operation :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>'>
                                    </telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblTimeInterval" runat="server" Text="Time Interval :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltTimeInterval" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMEINTERVAL") %>'>
                                    </telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblProcedure" runat="server" Text="Procedure :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltProcedure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURE") %>'>
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblRA" runat="server" Text="RA :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbtRA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRA") %>'>
                                    </telerik:RadLabel>
                                </td>
                                <td class="lblheader">
                                    <telerik:RadLabel ID="lblJHA" runat="server" Text="JHA :">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbltJHA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHA") %>'>
                                    </telerik:RadLabel>
                                </td>
                                <td class="lblheader"></td>
                                <td></td>
                            </tr>
                        </table>
                    </NestedViewTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowExpandCollapse="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="395px" SaveScrollPosition="true"
                        FrozenColumnsCount="7" EnableNextPrevFrozenColumns="false" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
