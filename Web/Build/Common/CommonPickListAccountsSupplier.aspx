<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListAccountsSupplier.aspx.cs" Inherits="Common_CommonPickListAccountsSupplier" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address List</title>
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuAddress" runat="server" OnTabStripCommand="MenuAddress_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNameSearch" CssClass="input" runat="server" Text=""></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCode" CssClass="input" Text=""></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCountryNameSearch" CssClass="input" Text=""></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <%--<ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                            FadeTransitions="false" FramesPerSecond="40" TransitionDuration="50" AutoSize="None"
                            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                            <Panes>
                                <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                                    <Header>
                                        <a href="" class="accordionLink">Product/Services</a>
                                    </Header>
                                    <Content>
                                        <asp:CheckBoxList runat="server" ID="cblProductType" Height="26px" RepeatColumns="7"
                                            RepeatDirection="Horizontal" RepeatLayout="Table">
                                        </asp:CheckBoxList>
                                    </Content>
                                </ajaxToolkit:AccordionPane>
                            </Panes>
                        </ajaxToolkit:Accordion>--%>
                        <telerik:RadPanelBar RenderMode="Lightweight" ID="RadPanelBar0" runat="server" Width="100%">
                            <Items>
                                <telerik:RadPanelItem Text="PanleItem1" Width="100%">
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Product/Services"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <telerik:RadCheckBoxList RenderMode="Lightweight" runat="server" ID="cblProductType" Height="90%" Columns="4"
                                            Direction="Vertical" Layout="Flow" AutoPostBack="false">
                                        </telerik:RadCheckBoxList>
                                    </ContentTemplate>
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelBar>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAddress" runat="server" CellSpacing="0" GridLines="None" EnableViewState="false"
                Font-Size="11px" OnItemCommand="gvAddress_ItemCommand" AllowPaging="true" AllowSorting="true" AllowCustomPaging="true" OnItemDataBound="gvAddress_ItemDataBound"
                Width="100%" OnNeedDataSource="gvAddress_NeedDataSource" Height="80%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDADDRESSCODE">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDCODE">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="14%" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAddressCode" Text='<%# Bind("FLDADDRESSCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblEmail" Text='<%# Bind("FLDEMAIL1") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkAddressName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CommandName="PICKADDRESS"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Phone 1" AllowSorting="true" SortExpression="FLDPHONE1">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Email 1" AllowSorting="true" SortExpression="FLDEMAIL1">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmail1" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMAIL1").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDEMAIL1").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDEMAIL1").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucEmailTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL1") %>' />
                                <img id="imgEmail" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/te_view.png %>"
                                    onmousedown="javascript:closeMoreInformation()" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Services" AllowSorting="true" SortExpression="FLDPRODUCTTYPENAME">
                            <HeaderStyle Width="" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblServices" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRODUCTTYPENAME").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDPRODUCTTYPENAME").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDPRODUCTTYPENAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucServicesTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTTYPENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="City" AllowSorting="true" SortExpression="FLDCITY">
                            <HeaderStyle Width="9%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="State" AllowSorting="true" SortExpression="FLDSTATE">
                            <HeaderStyle Width="9%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblState" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Country" AllowSorting="true" SortExpression="FLDCOUNTRYNAME">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="true" SortExpression="FLDHARDNAME">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle Width="8%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Relation" CommandName="RELATION" ToolTip="Related Address" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-address-card"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
