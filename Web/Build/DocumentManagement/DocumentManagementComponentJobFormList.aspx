<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementComponentJobFormList.aspx.cs"
    Inherits="DocumentManagementComponentJobFormList" %>

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
            
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvForm");
                grid._gridDataDiv.style.height = (browserHeight - 180) + "px";
            }
            function pageLoad() {
                PaneResized();
            }
        </script>
        <style type="text/css">
            .lblheader {
                font-weight: bold;
            }
        </style>
    </telerik:RadCodeBlock>

</head>
<body onreset="PaneResized()" onload="PaneResized()">
    <form id="frmField" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvForm" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            
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
                    <td style="width: 30px">
                    </td>
                    <td style="width: 35px">
                        <span id="spnPickListCategory">
                            <telerik:RadTextBox ID="txtCategory" runat="server" Width="200px" CssClass="hidden"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowCategory" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." Visible="false" />
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

            <telerik:RadGrid ID="gvForm" runat="server" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" Font-Size="11px" Width="100%" CellPadding="3" OnNeedDataSource="gvForm_NeedDataSource"
                OnItemCommand="gvForm_ItemCommand" OnItemDataBound="gvForm_ItemDataBound" ShowFooter="false" EnableViewState="true" 
                ShowHeader="true" >
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AllowNaturalSort="false" GroupsDefaultExpanded="true" AutoGenerateColumns="false" TableLayout="Fixed" EnableHeaderContextMenu="true" CommandItemDisplay="Bottom" GroupHeaderItemStyle-Font-Bold="true"
                    GroupLoadMode="Client" DataKeyNames="FLDFORMID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
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
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" CommandName="EXPAND"
                                    ImageUrl="<%$ PhoenixTheme:images/sidearrow.png %>" ID="cmdBDetails" ToolTip="Form Details"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
                            <HeaderTemplate>
                                Form No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Center" Width="250px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'
                                    Visible="false" >
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
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active">
                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVESTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurpose" runat="server" Width="70px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString().Length > 14 ? DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString().Substring(0, 14) : DataBinder.Eval(Container, "DataItem.FLDPURPOSE").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucPurpose" runat="server" TargetControlId="lblPurpose" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Added Date">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Added By">
                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddedByName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAUTHORNAME").ToString().Length > 14 ? DataBinder.Eval(Container, "DataItem.FLDAUTHORNAME").ToString().Substring(0, 14) : DataBinder.Eval(Container, "DataItem.FLDAUTHORNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucAddedByNameTT" runat="server" TargetControlId="lblAddedByName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Revison Number">
                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVersionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Published">
                          <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblPublishedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                    </Columns>
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
