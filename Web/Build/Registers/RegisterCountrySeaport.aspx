<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCountrySeaport.aspx.cs" Inherits="RegisterCountrySeaport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="~/UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seaport</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlSeaportEntry">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table id="tblConfigureSeaport" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<eluc:Country ID="ddlCountrySearch" runat="server" CssClass="dropdown_mandatory" Enabled="false" CountryList='<%# PhoenixRegistersCountry.ListCountry(1,1) %>' AppendDataBoundItems="true" />--%>
                        <telerik:RadTextBox ID="LblCountryName" runat="server" Enabled="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="LblCountryID" runat="server" Enabled="false" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeaportCode" runat="server" MaxLength="6" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersSeaport" runat="server" OnTabStripCommand="RegistersSeaport_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvSeaport" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="false" ShowFooter="true"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvSeaport_NeedDataSource" OnItemDataBound="gvSeaport_ItemDataBound" OnItemCommand="gvSeaport_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeaportcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSeaportcodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTCODE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="6">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSeaportcodeAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="6">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeaportid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkSeaportName" runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>' Visible="false"></asp:LinkButton>
                                <telerik:RadLabel ID="lblSeaportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblSeaportidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtSeaportNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSeaportNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="100" ToolTip="Enter Seaport Name">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Airport">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblairportid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRPORTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkairportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAirportId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRPORTID") %>'></telerik:RadLabel>
                                <eluc:Airport ID="Airport1" runat="server" AirportList='<%# PhoenixRegistersAirport.ListAirport(null)%>'
                                    CssClass="dropdown_mandatory" AppendDataBoundItems="true" SelectedAirport='<%# DataBinder.Eval(Container,"DataItem.FLDAIRPORTID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Airport ID="Airport2" runat="server" CssClass="dropdown_mandatory" AirportList='<%# PhoenixRegistersAirport.ListAirport(null)%>'
                                    AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'></asp:CheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="chkActiveYNAdd" runat="server"></asp:CheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="" CommandName="PORTCOMMENTS" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdPortComments" ToolTip="Port Comments">
                                    <span class="icon"><i class="fas fa-comment-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="SAVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave" ToolTip="Save">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel" ToolTip="Cancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="ADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd" ToolTip="Add New">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" Position="Bottom" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" Scrolling-EnableNextPrevFrozenColumns="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
