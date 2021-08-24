<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAddress.aspx.cs"
    Inherits="RegistersAddress" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuOffice" runat="server" OnTabStripCommand="MenuOffice_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" Height="98.5%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnSortCommand="RadGrid1_SortCommand" OnNeedDataSource="RadGrid1_NeedDataSource"
                OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" AutoGenerateColumns="false" EnableViewState="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCODE" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code" HeaderStyle-Width="6%" AllowSorting="true" SortExpression="FLDCODE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAddressType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsBlacklisted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISBLACKLISTED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDNAME" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkAddressName" runat="server" CommandArgument='<%# Bind("FLDADDRESSCODE") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                <eluc:ToolTip ID="ucToolTipAddress" runat="server" TargetControlId="lnkAddressName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESS1")+"<br>"+ DataBinder.Eval(Container,"DataItem.FLDADDRESS2")+"<br>"+ DataBinder.Eval(Container,"DataItem.FLDADDRESS3") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Phone 1" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="City" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Country" AllowSorting="true" SortExpression="FLDCOUNTRYNAME"  HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="Label1" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="true" SortExpression="FLDHARDNAME" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-Width="8%">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Width="60px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="ADDRESSEDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="RELATION" ID="cmdRelation" ToolTip="Related Address">
                                    <span class="icon"><i class="fas fa-landmark"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="SUPMAP" ID="cmdSupplierMap" ToolTip="Prefered Suppliers">
                                        <span class="icon"><i class="fas fa-users"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="APPROVE" ID="cmdApprove" ToolTip="Supplier Approve">
                                      <span class="icon"><i class="fas fa-user-check"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="GETADDRESSTYPE" Visible="false" ID="imgOwner" ToolTip="Owner Mapping">
                                      <span class="icon"><i class="fas fa-owner"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" Visible="false" CommandName="CHARTERERCONFIGURATION" ID="imgChartererConfig" ToolTip="Charterer Configuration">
                                      <span class="icon"><i class="fas fa-user-cog-add"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="CREATELOGIN" ID="cmdLogin" ToolTip="Create Login">
                                    <span class="icon"><i class="fas fa-sign-in-alt-user"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" Visible="false" CommandName="INSTITUTEASSESSMENT" ID="imgInstituteAssessment" ToolTip="Assessment">
                                      <span class="icon"><i class="fa fa-pencil-ruler"></i></span>
                                </asp:LinkButton>
                                <asp:ImageButton runat="server" ID="cmdStoreTypeMap" CommandArgument="STORETYPEMAPPING" ToolTip="Store Type Mapping" Visible="false"
                                    ImageUrl="<%$ PhoenixTheme:images/Planned.png%>">
                                </asp:ImageButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
