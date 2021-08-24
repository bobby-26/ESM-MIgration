<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterFMSManualSubCategory.aspx.cs" Inherits="Registers_RegisterFMSMannualSubCategory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FMS Mannual SubCategory</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersFMSMannualSubCategory" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
                DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divFind"></telerik:RadFormDecorator>
            <table id="tblConfigureFMSMannualSubCategory">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" Width="270px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                            AutoPostBack="true" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true"
                            EmptyMessage="Type to select Category">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuFMSMannualSubCategory" runat="server" OnTabStripCommand="MenuFMSMannualSubCategory_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvFMSMannualSubCategory" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvFMSMannualSubCategory_ItemCommand"
                Height="84%" OnItemDataBound="gvFMSMannualSubCategory_ItemDataBound" ShowHeader="true"
                EnableViewState="false" AllowSorting="true" OnSorting="gvFMSMannualSubCategory_Sorting"
                AllowPaging="true" AllowCustomPaging="true" GridLines="None" OnNeedDataSource="gvFMSMannualSubCategory_NeedDataSource"
                RenderMode="Lightweight" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDSUBCATEGORYCODE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <HeaderStyle Width="10%" Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYCODE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SubCategory" AllowSorting="true" SortExpression="FLDSUBCATEGORYNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <HeaderStyle Width="75%" Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME").ToString().Length > 50 ? DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString().Substring(0, 50) + "..." : DataBinder.Eval(Container, "DataItem.FLDSUBCATEGORYNAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="SubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>' />
                                <telerik:RadLabel ID="lblSubCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFMSMANUALSUBCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="MAP" ID="cmdMapVesselType" ToolTip="Map Vessel Types">
                                    <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="MAPVESSEL" ID="cmdMapVessel" ToolTip="Map Vessels">
                                    <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
