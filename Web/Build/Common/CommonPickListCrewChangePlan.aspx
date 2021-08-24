<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListCrewChangePlan.aspx.cs" Inherits="Common_CommonPickListCrewChangePlan" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User</title>
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

        <telerik:RadLabel ID="lblBudget" runat="server" Text="On-Signer List"></telerik:RadLabel>


        <eluc:TabStrip ID="MenuOrderLine" runat="server" OnTabStripCommand="MenuOrderLine_TabStripCommand"></eluc:TabStrip>

        <br clear="all" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblsignon" runat="server" Text="Sign On Date from"></telerik:RadLabel>

                    </td>
                    <td>
                        <table width="50%">
                            <tr>
                                <td>
                                    <eluc:Date ID="txtfromdate" runat="server" CssClass="input" DatePicker="false" />
                                </td>
                                <td>to  </td>
                                <td>
                                    <eluc:Date ID="txttodate" runat="server" CssClass="input" DatePicker="false" />
                                </td>
                            </tr>

                        </table>
                    </td>

                </tr>
            </table>

            <div id="divGrid" style="position: relative;">
                <%--<asp:GridView ID="gvcrewplan" runat="server" AutoGenerateColumns="false" Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true"
                    OnRowCommand="gvcrewplan_RowCommand" OnRowEditing="gvcrewplan_RowEditing">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvcrewplan" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvcrewplan_NeedDataSource"
                    OnItemCommand="gvcrewplan_ItemCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />

                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblonsignerfn" runat="server">File No.&nbsp;
                                </telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblcrewplanid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblemployeeid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblfileno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblonsignername" runat="server">Name&nbsp;</telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>

                                <asp:LinkButton ID="lnkName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></asp:LinkButton>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblFirstNameHeader" runat="server">
                                    Rank
                                </telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblrankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblsignonHeader" runat="server">
                                    Sign-on Date
                                </telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}") %>
                            </itemtemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblVESSELname" runat="server">Vessel</telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblVESSELnamedetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>

                            </itemtemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </div>

        </div>

    </form>
</body>
</html>
