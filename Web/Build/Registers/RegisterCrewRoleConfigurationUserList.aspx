<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCrewRoleConfigurationUserList.aspx.cs" Inherits="Registers_RegisterCrewRoleConfigurationUserList" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Users</title>
   <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersUser" runat="server">
      <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
  <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <eluc:TabStrip ID="MenuUser" runat="server" OnTabStripCommand="MenuUser_TabStripCommand">
            </eluc:TabStrip>
            <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
                DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divFind">
            </telerik:RadFormDecorator>
            <div id="divFind" style="position: relative; z-index: 2">
                <b>
                <table id="tblConfigureUser" >
                    <tr>
                        <td >
                            <telerik:RadLabel ID="lblrole" runat="server" Text="Role"></telerik:RadLabel>
                        </td>
                        <td >
                            <telerik:RadTextBox ID="txtRole" runat="server" ReadOnly="true" Width="300px" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td >
                            <telerik:RadLabel ID="lblDesignation2" runat="server" Text="Designation"></telerik:RadLabel>
                        </td>
                        <td >
                          <telerik:RadComboBox ID="ddlDesignation" runat="server" Width="300px" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged"
                                AutoPostBack="true" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Designation">
                            </telerik:RadComboBox>
                           <%-- <asp:DropDownList ID="ddlDesignation" runat="server" Width="300px" CssClass="input"
                                OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged" AppendDataBoundItems="true"
                                AutoPostBack="true" />--%>
                        </td>
                    </tr>
                </table>
                    </b>
            </div>
            <telerik:RadGrid ID="gvUser" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowCustomPaging="true" 
               Width="100%" CellPadding="3" OnItemCommand="gvUser_ItemCommand" OnItemDataBound="gvUser_ItemDataBound" GridLines="None"
               ShowHeader="true" AllowSorting="true" OnSorting="gvUser_Sorting"  EnableViewState="false" AllowPaging="true"  
               OnNeedDataSource="gvUser_NeedDataSource" RenderMode="Lightweight" GroupingEnabled="false" EnableHeaderContextMenu="true" >
                       
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"   >
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Department">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblUser" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Username"   SortExpression="FLDUSERNAME"    AllowSorting="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblUserCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblEmail" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn  HeaderText=" First Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn  HeaderText=" Last Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                       
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn  HeaderText=" Middle Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                       
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMiddleName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn  HeaderText="Designation">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDesignation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                   </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        ScrollHeight="370px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
     
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
