<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListVesselEmployeeOnboard.aspx.cs" Inherits="CommonPickListVesselEmployeeOnboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>



        <br clear="all" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Rank ID="ucRank" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" AppendDataBoundItems="true" OnTextChangedEvent="ucRank_TextChangedEvent" />
                </td>
            </tr>
        </table>


        <%--  <asp:GridView ID="gvCrewList" runat="server" AutoGenerateColumns="False" CellPadding="3"
                Font-Size="11px" OnRowCommand="gvCrewList_RowCommand" OnRowDataBound="gvCrewList_ItemDataBound"
                OnRowEditing="gvCrewList_RowEditing" ShowFooter="true" ShowHeader="true" Width="100%"
                EnableViewState="false" OnSorting="gvCrewList_Sorting">

                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />--%>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewList" runat="server" Height="90%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvCrewList_NeedDataSource"
            OnSortCommand="gvCrewList_SortCommand"
            OnItemCommand="gvCrewList_ItemCommand"
            GroupingEnabled="false" EnableHeaderContextMenu="true"
            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <HeaderStyle Width="102px" />
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Name">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" Width="40%" />
                    
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" ID="lblCrewId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <asp:LinkButton ID="lnkCrew" runat="server" CommandArgument='<%# Container.DataSetIndex %>' CommandName="Select"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                  
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" ID="lblRank" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSIGNONRANKNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Nationality">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                      
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" ID="lblNationality" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDNATIONALITYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sign-on Date">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                    
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" ID="lblSignondate" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>'></telerik:RadLabel>

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>



    </form>
</body>
</html>
