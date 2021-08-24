<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAddressPurpose.aspx.cs"
    Inherits="RegistersAddressPurpose" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>purpose</title>
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuAddressMain" runat="server" OnTabStripCommand="AddressMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuAddressPurpose" runat="server" OnTabStripCommand="AddressPurpose_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>


        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <div id="divPurpose">
                <table width="100%">
                    <tr style="width: 10%">
                        <td>
                            <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlPurpose" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" AutoPostBack="true"
                                OnTextChanged="ddlPurpose_TextChanged">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="height: -15px" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divDisplay">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAddressContactID" runat="server" Visible="false" />
                            <telerik:RadTextBox ID="txtName" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblEmail1" runat="server" Text="Email 1"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmail1" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td rowspan="5">
                            <div id="dvClass" runat="server" class="input" style="overflow: auto; height: 100px">
                                <asp:CheckBoxList ID="cblRegisteredCompany" runat="server" RepeatDirection="Vertical" Visible="false">
                                </asp:CheckBoxList>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblEmail2" runat="server" Text="Email 2"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmail2" runat="server" CssClass="input" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDepartment" runat="server" Text="Department"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtDepartment" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPhone1" runat="server" Text="Phone 1"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPhone1" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblMobile" runat="server" Text="Mobile"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMobile" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPhone2" runat="server" Text="Phone 2"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPhone2" runat="server" CssClass="input" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFax" runat="server" Text="Fax"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFax" runat="server" CssClass="input" />
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuAddressFind" runat="server" OnTabStripCommand="AddressFind_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            </div>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvAddressPurpose" runat="server" Height="95%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnSortCommand="gvAddressPurpose_SortCommand" OnNeedDataSource="gvAddressPurpose_NeedDataSource"
                OnItemDataBound="gvAddressPurpose_ItemDataBound" OnItemCommand="gvAddressPurpose_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" TableLayout="Fixed"
                    AutoGenerateColumns="false" DataKeyNames="FLDADDRESSCONTACTID" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="80px" AllowSorting="true" SortExpression="FLDNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddressContactId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCONTACTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAddressContactIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCONTACTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Email 1" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddressId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmail1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL1") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Department" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Phone1" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <%--                            <HeaderStyle Width="5px" />
                            <ItemStyle Width="5px" Wrap="false" />--%>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="UPDATE" ID="cmdEdit" ToolTip="Edit">
                                      <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                      <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>





            <%-- <div id="divgrid">
                <asp:GridView ID="gvAddressPurpose" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false"
                    AllowSorting="true" OnRowCommand="gvAddressPurpose_RowCommand"
                    OnRowDataBound="gvAddressPurpose_RowDataBound"
                    OnRowCancelingEdit="gvAddressPurpose_RowCancelingEdit"
                    OnRowEditing="gvAddressPurpose_RowEditing"
                    OnRowDeleting="gvAddressPurpose_RowDeleting"
                    OnSelectedIndexChanged="gvAddressPurpose_SelectedIndexChanged">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAddressContactId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCONTACTID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblAddressContactIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCONTACTID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblAddressContactIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCONTACTID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email 1">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblEmail1" runat="server" Text="Email 1"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAddressId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblEmail1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL1") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblAddressIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblEmail1Edit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL1") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Title">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblTitle" runat="server" Text="Title"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Department">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDepartment" runat="server" Text="Department"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phone1">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblPhone1" runat="server" Text="Phone 1"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <HeaderStyle Wrap="false"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
