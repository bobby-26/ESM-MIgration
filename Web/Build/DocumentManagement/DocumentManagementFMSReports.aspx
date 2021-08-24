<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSReports.aspx.cs" Inherits="DocumentManagement_DocumentManagementFMSReports" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FMS Reports</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        
        <%-- For Popup Relaod --%>
         <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />  

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <eluc:Status ID="ucStatus" runat="server" ></eluc:Status>

        <%--<telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
            <eluc:TabStrip ID="MenuDivRegulation" runat="server" OnTabStripCommand="plan_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>--%>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvFMSReport" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
            CellSpacing="0" GridLines="None"
            OnNeedDataSource="gvFMSReport_NeedDataSource"
            OnItemDataBound="gvFMSReport_ItemDataBound"
            OnItemCommand="gvFMSReport_ItemCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <Columns>

                    <telerik:GridTemplateColumn HeaderText="Menu Name" AllowSorting="true">
                        <HeaderStyle Width="118px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                             <telerik:RadLabel ID="lblMenuName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUNAME") %>'></telerik:RadLabel>

                            <telerik:RadLabel ID="lblMenuValue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUVALUE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblMenuCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMENUCODE") %>'></telerik:RadLabel>
                             <telerik:RadLabel ID="lblOrgMenuName" runat="server"  Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGMENUNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAppliesTo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLIESTO") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblParentCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblsortorder" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Action" >
                        <HeaderStyle Width="130px" HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>

                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="SUMMARYEDIT" ID="cmdEdit"
                                ToolTip="Edit" Width="20PX" Height="20PX"> 
                                <asp:ImageButton runat="server"  ImageUrl="<%$ PhoenixTheme:images/28.png %>"
                                    CommandName="PAGERIGHTS"  ID="ImageButton1"
                                    ToolTip="Insert FMS"></asp:ImageButton>
                            </asp:LinkButton>

                        </ItemTemplate>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="460px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
