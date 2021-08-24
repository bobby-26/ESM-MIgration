<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDeliveryTemp.aspx.cs" Inherits="PurchaseDeliveryTemp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Delivery Temp</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseDeliveryTemp" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="General" ShowMenu="false">
                </eluc:Title>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuDelivery" runat="server" OnTabStripCommand="MenuDelivery_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div>
            <br clear="all" />
            <asp:UpdatePanel runat="server" ID="pnlFormGeneral">
                <ContentTemplate>
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                        border: none; width: 100%">
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtVesselName" CssClass="input" ></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblFormNos" runat="server" Text="Form Nos"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFormNo" CssClass="input" ></asp:TextBox>
                                    <asp:Label runat="server" ID="lblDeliveryTempId" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblForwarder" runat="server" Text="Forwarder"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtForwarder" CssClass="input" ></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblForwarderReceivedOn" runat="server" Text="Forwarder Received On"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReceivedForwarder" runat="server" Width="90px" CssClass="input"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="ceReceivedForwarder" runat="server" Format="dd/MMM/yyyy"
                                        Enabled="True" TargetControlID="txtReceivedForwarder" PopupPosition="TopLeft">
                                    </ajaxToolkit:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>                                   
                                     <asp:Literal ID="lblHAWBHBL" runat="server" Text="HAWB/HBL"></asp:Literal>
                                </td>
                                <td>
                                     <asp:TextBox ID="txtHawb" runat="server" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                   <asp:Literal ID="lblMAWBOBL" runat="server" Text="MAWB/OBL"></asp:Literal>
                                </td>
                                <td>
                                   <asp:TextBox runat="server" ID="txtMawb" CssClass="input" ></asp:TextBox>
                                </td>                              
                            </tr>
                            <tr>
                                <td>
                                   <asp:Literal ID="lblIsDelivered" runat="server" Text="Is Delivered?"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtIsDelivered" CssClass="input" ></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblSupplier" runat="server" Text="Supplier"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSupplier" CssClass="input" ></asp:TextBox>                          
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblNoofPackages" runat="server" Text="No. of Packages"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtNoOfPackages" Style="text-align: right" CssClass="input"
                                        Width="80px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtNoOfPackages"
                                        Mask="9999" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </td>
                                <td>
                                    <asp:Literal ID="lblTotalWeight" runat="server" Text="Total Weight"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="ucTotalWeight" Style="text-align: right" CssClass="input"
                                        Width="80px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="ucTotalWeight"
                                        Mask="9999.99" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblOrigin" runat="server" Text="Origin"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtOrigin" CssClass="input" ></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblStorageLocation" runat="server" Text="Storage Location"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtLocation" CssClass="input" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblShipmentMode" runat="server" Text="Shipment Mode"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtShipmentMode" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblIsDGR" runat="server" Text="Is DGR?"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDGR" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtCurrency" CssClass="input" ></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Decimal ID="txtAmount" runat="server" Width="100px" CssClass="input" ReadOnly="true" />                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblShortNote" runat="server" Text="Short Note"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtShortNote" CssClass="input" Width="300px" TextMode="MultiLine" ></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblError" runat="server" Text="Error"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtError" CssClass="input" ReadOnly="true" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
