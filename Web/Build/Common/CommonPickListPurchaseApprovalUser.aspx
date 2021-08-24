<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListPurchaseApprovalUser.aspx.cs"
    Inherits="CommonPickListPurchaseApprovalUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
</telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("gvApprovalUser");
            grid._gridDataDiv.style.height = (browserHeight - 150) + "px";
        }
        function pageLoad() {
            PaneResized();
        }

   </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersUser" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
                <br />
                    <table id="tblConfigureUser" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblUserName" runat="server" Text="User Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtUserName" runat="server" MaxLength="100" Width="360px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtName" MaxLength="100" Width="360px"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    
                 <telerik:RadGrid RenderMode="Lightweight" ID="gvApprovalUser" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvApprovalUser_NeedDataSource" OnItemCommand="gvApprovalUser_ItemCommand" >
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                         CommandItemDisplay="Top" DataKeyNames="FLDUSERCODE"  AutoGenerateColumns="false"  TableLayout="Fixed">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false" ShowPrintButton="false"  ShowRefreshButton="true" RefreshText="Search" />
                             <Columns>
                                <telerik:GridTemplateColumn HeaderText="Department">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblUser" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Username">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkUserName" runat="server" CommandName="EDIT" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="First Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Last Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Middle Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblMiddleName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Designation">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblDesignation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></telerik:RadLabel>                                    
                                </ItemTemplate>     
                            </telerik:GridTemplateColumn> 

                                </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                                PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                        </ClientSettings>
                    </telerik:RadGrid>
            </div>
    </form>
</body>
</html>
