<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowProcessGroupMemberAdd.aspx.cs" Inherits="WorkflowProcessGroupMemberAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Process Group Member Add</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuWFGroupMember" runat="server" OnTabStripCommand="MenuWFGroupMember_TabStripCommand" />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="radlabel1" runat="server" Text="User Name">  </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUsername" runat="server" Text=""></telerik:RadTextBox>
                    </td>
               
                    <td>
                        <telerik:RadLabel ID="radlabel2" runat="server" Text="First Name">  </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" Text=""></telerik:RadTextBox>
                    </td>
             
                    <td>
                        <telerik:RadLabel ID="radlabel3" runat="server" Text="Last Name">  </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" Text=""></telerik:RadTextBox>
                    </td>
                </tr>

            </table>

                <eluc:TabStrip ID="MenuGroupSearch" runat="server" OnTabStripCommand="MenuGroupSearch_TabStripCommand" />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvProcessGroupMember" runat="server" CellSpacing="0" GridLines="None"
               Height="80%"  EnableHeaderContextMenu="true" EnableViewState="true" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvProcessGroupMember_NeedDataSource">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="CheckUser" runat="server" AutoPostBack="false" ></telerik:RadCheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="User Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUserCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="First Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Last Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">

                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
