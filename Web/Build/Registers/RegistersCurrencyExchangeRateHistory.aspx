<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersCurrencyExchangeRateHistory.aspx.cs"
    Inherits="RegistersCurrencyExchangeRateHistory" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register Src="../UserControls/UserControlMoney.ascx" TagName="UserControlMoney"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Exchange Rate History</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersAirlines" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuExchangeRateHistory" runat="server" OnTabStripCommand="ExchangeRateHistory_TabStripCommand"></eluc:TabStrip>
            </div>
            <table id="tblExchangeRate" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyName" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrencyName" RenderMode="Lightweight" runat="server" CssClass="input readonlytextbox" Width="250px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrencyCode" RenderMode="Lightweight" runat="server" CssClass="input readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="Modified From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtFromdate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="Modified To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtTodate" runat="server" CssClass="input" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuExchangeRate" runat="server" OnTabStripCommand="ExchangeRate_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="dgExchangerate" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" Font-Size="11px" Height="77%"
                OnItemDataBound="dgExchangerate_ItemDataBound" OnNeedDataSource="dgExchangerate_NeedDataSource"
                OnItemCommand="dgExchangerate_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDEXCHANGERATEID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Currency Name" AllowSorting="true" SortExpression="FLDCURRENCYNAME" Visible="false">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkModifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Indirect Rate to USD" AllowSorting="true" SortExpression="FLDEXCHANGERATE">
                            <HeaderStyle Width="15.25%" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkexchangerate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE","{0:f17}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnInDirectExchangeEdit">
                                    <%--<asp:TextBox ID="txtExchangerateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE","{0:f17}") %>'
                                                CssClass="gridinput_mandatory txtNumber" MaxLength="25"></asp:TextBox>--%>
                                    <%--<ajaxtoolkit:maskededitextender id="MaskedEditExchangeRate" runat="server" autocomplete="false"
                                                inputdirection="RightToLeft" mask="9999.99999999999999999" masktype="Number"
                                                    oninvalidcssclass="MaskedEditError" targetcontrolid="txtExchangeRateEdit" />--%>
                                    <eluc:Decimal ID="txtExchangerateEdit" runat="server" CssClass="input_mandatory"
                                        Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE") %>' />
                                </span>
                                <telerik:RadTextBox ID="txtCurrencyid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'
                                    Visible="false">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtExchangeRateHistoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATEHISTORYID") %>'
                                    Visible="false">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Direct Rate to USD" AllowSorting="true" SortExpression="FLDEXCHANGERATEUSD">
                            <HeaderStyle Width="15.25%" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkexchangerateuSD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATEUSD" ,"{0:f17}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnDirectExchangeEdit">
                                    <%--<asp:TextBox ID="txtExchangerateEditUSD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATEUSD","{0:f17}") %>'
                                                CssClass="gridinput_mandatory txtNumber" MaxLength="25"></asp:TextBox>--%>
                                    <%--<ajaxtoolkit:maskededitextender id="MaskedEditExchangeRateUSD" runat="server" autocomplete="false"
                                                    inputdirection="RightToLeft" mask="9999.99999999999999999" masktype="Number"
                                                        oninvalidcssclass="MaskedEditError" targetcontrolid="txtExchangerateEditUSD" />--%>
                                    <eluc:Decimal ID="txtExchangerateEditUSD" runat="server" CssClass="input_mandatory"
                                        Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATEUSD") %>' />
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Variance (%)" AllowSorting="true" SortExpression="FLDCHANGEPERCENTAGE">
                            <HeaderStyle Width="15%" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvarieschange" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCHANGEPERCENTAGE","{0:n2}" )%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Modified By" AllowSorting="true" SortExpression="FLDMODIFIEDUSERNAME">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmodifiedby" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMODIFIEDUSERNAME" )%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Modified Date" AllowSorting="true" SortExpression="FLDMODIFIEDDATE">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmodifiedDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMODIFIEDDATE" )%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadImageButton ID="cmdEdit" runat="server" AlternateText="Edit"
                                    CommandName="EDIT" Image-Url="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" Width="20px" Height="20px" />
                                <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadImageButton ID="cmdSave" runat="server" AlternateText="Save"
                                    CommandName="Save" Image-Url="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" Width="20px" Height="20px" />
                                <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <telerik:RadImageButton ID="cmdCancel" runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" Image-Url="<%$ PhoenixTheme:images/te_del.png%>" ToolTip="Cancel" Width="20px" Height="20px" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
