<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareItemGeneral.aspx.cs" Inherits="InventorySpareItemGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spare Item Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuStockItemGeneral" runat="server" OnTabStripCommand="InventoryStockItemGeneral_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <br clear="all" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%">
                        <eluc:MaskNumber runat="server" ID="txtNumber" MaxLength="20" MaskText="###.##.##.###" Enabled="true" Width="223px" />
                        <asp:ImageButton ID="cmdNextNumber" runat="server" AlternateText="Next Number" ImageUrl="<%$ PhoenixTheme:images/next-number.png%>"
                            ToolTip="NextNumber" OnClick="cmdNextNumber_OnClick" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtName" CssClass="input_mandatory" MaxLength="200"
                            Width="216px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 10%"></td>
                </tr>
                <tr>
                    <td class="style1">
                        <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker"></telerik:RadLabel>
                    </td>
                    <td class="style2">
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtMakerCode" runat="server" Width="80px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtMakerName" runat="server" Width="140px" CssClass="input"></telerik:RadTextBox>
                            <asp:LinkButton ID="ImgShowMaker" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', 'Common/CommonPickListAddress.aspx?addresstype=130,131,132&framename=ifMoreInfo', true);">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="imgClearMaker" ImageAlign="AbsMiddle" Text=".." OnClick="ClearMaker">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtMakerId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                        </span>&nbsp;
                    </td>
                    <td class="style1">
                        <telerik:RadLabel ID="lblPreferredVendor" runat="server" Text="Preferred Vendor"></telerik:RadLabel>
                    </td>
                    <td class="style2">
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtPreferredVendorCode" runat="server" CssClass="input" Width="60px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtPreferredVendorName" runat="server" CssClass="input" Width="150px"></telerik:RadTextBox>
                            <asp:LinkButton ID="ImgShowMakerVendor" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListVendor', 'codehelp1', '', 'Common/CommonPickListAddress.aspx?addresstype=130,131,132&framename=ifMoreInfo', true);">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="imgClearVendor" ImageAlign="AbsMiddle" Text=".." OnClick="ClearVendor">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblMakersReference" runat="server" Text="Maker Reference"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox ID="txtMakerReference" runat="server" CssClass="input" MaxLength="50"
                            Width="223px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">&nbsp;
                    </td>
                    <td style="width: 30%">&nbsp;
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr valign="top" style="width: 15%">
                    <td>
                        <telerik:RadLabel ID="lblStockMaximum" runat="server" Text="Stock Maximum"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtStockMaximumQuantity" CssClass="input txtNumber" Type="Number" Width="223px"
                            MaxLength="9" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReorderLevel" runat="server" Text="Reorder Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtReOrderLevel" CssClass="input txtNumber" Type="Number" Width="223px"
                            MaxLength="2" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblStockMinimum" runat="server" Text="Stock Minimum"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtStockMinimumQuantity" CssClass="input txtNumber" Type="Number" Width="223px"
                            MaxLength="9" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReorderQuantity" runat="server" Text="Reorder Quantity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtReOrderQuantity" CssClass="input txtNumber" Type="Number" Width="223px"
                            MaxLength="9" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblWantedQuantity" runat="server" Text="Wanted Quantity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtWantedQuantity" CssClass="input txtNumber" Type="Number" Width="223px"
                            MaxLength="9" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblUnit" runat="server" Text="Unit"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlUnit ID="ddlStockUnit" runat="server" AppendDataBoundItems="true" Width="223px"
                            CssClass="input_mandatory" />
                        <asp:LinkButton runat="server" ID="imgUnitMap" ImageAlign="AbsMiddle" Text=".." ToolTip="Measurable Units">
                                <span class="icon"><i class="fas fa-balance-scale"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblStockAveragePrice" runat="server" Text="Stock Average Price"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlStockAveragePriceCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="70px"
                            runat="server" AppendDataBoundItems="true" Enabled="false" />
                        <eluc:Decimal runat="server" ID="txtStockAveragePrice" CssClass="input txtNumber readonlytextbox" Type="Currency" Width="150px"
                            MaxLength="9" />
                    </td>
                    <td style="width: 15%">
                        <%-- Component Class--%>
                    </td>
                    <td style="width: 15%">
                        <eluc:UserControlHard ID="ddlStockClass" runat="server" CssClass="input" Visible="false"
                            AppendDataBoundItems="true" />
                        <telerik:RadCheckBox ID="chkIsCritical" runat="server" Checked="false" Text="Critical" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblLastPurchasedPrice" runat="server" Text="Last Purchased Price"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlLastPurchasedPriceCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="70px"
                            runat="server" AppendDataBoundItems="true" Enabled="false" />
                        <eluc:Decimal runat="server" ID="txtLastPurchasedPrice" CssClass="input txtNumber"
                            MaxLength="9" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTotalStock" Font-Underline="true" Font-Bold="true" runat="server" Text="Total Stock"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtTotalStock" CssClass="input txtNumber" Type="Number" Enabled="false" Width="223px"
                            MaxLength="9" />
                    </td>
                   
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastPurchasedDate" runat="server" Text="Last Purchased Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtlastpurchaseDate" runat="server" CssClass="input" Width="223px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblItemisinMarket" runat="server" Text="Item is in Market"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkMarket" runat="server" Checked="true" />
                    </td>
                    <td></td>
                     </tr>
                    <tr>
                     <td style="width: 15%">
                        <telerik:RadLabel ID="lblmaterialnumber" runat="server" Text="Material Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtmaterialnumber" runat="server"  CssClass="input"
                            Width="223px">
                        </telerik:RadTextBox>
                    </td>
                       </tr>
                   
               
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
