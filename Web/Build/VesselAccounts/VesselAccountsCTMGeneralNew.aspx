<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCTMGeneralNew.aspx.cs"
    Inherits="VesselAccountsCTMGeneralNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CTM General</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Cash Request"></eluc:Title>
                </div>
                <div class="navSelect" style="width: auto; float: right; margin-top: -26px">
                    <eluc:TabStrip ID="MenuCTMMain" runat="server" OnTabStripCommand="MenuCTMMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="subHeader">
                    <div style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuCTM" runat="server" OnTabStripCommand="MenuCTM_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div id="divMain" runat="server" style="width: 99%; padding: 5px 5px 5px 5px;">
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblhead" runat="server" Text='<%#SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.VesselName %>'></asp:Label>
                        </legend>
                        <div id="div1" runat="server" style="width: 100%;">
                            <fieldset style="border: none;">
                                <legend style="color: Black; font-weight: bold;">
                                    <asp:Label ID="Label1" runat="server" Text=''></asp:Label>
                                </legend>
                                <table width="100%" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <%-- <td>
                                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                        Width="50%" Text="<%#SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.VesselName %>"></asp:TextBox>
                                </td>--%>
                                        <td style="width: 10%;">
                                            <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                                        </td>
                                        <td style="width: 40%;">
                                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" />
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                                        </td>
                                        <td style="width: 40%;">
                                            <eluc:MultiPort ID="ddlPort" runat="server" CssClass="input_mandatory" Width="300px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblPortAgent" runat="server" Text="Port Agent"></asp:Literal>
                                        </td>
                                        <td>
                                            <span id="spnPickListSupplier">
                                                <asp:TextBox ID="txtSupplierCode" runat="server" Width="80px" CssClass="input_mandatory"
                                                    Enabled="False"></asp:TextBox>
                                                <asp:TextBox ID="txtSupplierName" runat="server" Width="220px" CssClass="input_mandatory"
                                                    Enabled="False"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="cmdShowSupplier" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListVesselSupplier.aspx', true);"
                                                    Text=".." />
                                                <asp:TextBox ID="txtSupplierId" runat="server" Width="1px" CssClass="input"></asp:TextBox>
                                            </span>
                                        </td>
                                        <td>
                                            <asp:Literal ID="lblETA" runat="server" Text="ETA"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Date ID="txtETA" runat="server" CssClass="input_mandatory" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblETD" runat="server" Text="ETD"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Date ID="txtETD" runat="server" CssClass="input_mandatory" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Number ID="txtAmount" CssClass="readonlytextbox input" ReadOnly="true" runat="server"
                                                Width="90px" MaxLength="8" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblArrangedAmount" runat="server" Text="Arranged Amount"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Number ID="txtAmountArranged" CssClass="input" runat="server" Width="90px"
                                                MaxLength="8" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="lblOfficeRemarks" runat="server" Text="Office Remarks"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                                                Width="250px" Height="35px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <div id="divSub" runat="server" style="width: 100%;">
                            <fieldset style="border: none;">
                                <legend style="color: Black; font-weight: bold;">
                                    <asp:Label ID="lblhead1" runat="server" Text='Received'></asp:Label>
                                </legend>
                                <table width="100%" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td style="width: 10%;">
                                            <asp:Literal ID="lblReceivedAmount" runat="server" Text="Amount"></asp:Literal>
                                        </td>
                                        <td style="width: 40%;">
                                            <eluc:Number ID="txtReceivedAmount" runat="server" CssClass="input" Width="90px"
                                                MaxLength="8" />
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:Literal ID="lblReceivedDate" runat="server" Text="Date"></asp:Literal>
                                        </td>
                                        <td style="width: 40%;">
                                            <eluc:Date ID="txtReceivedDate" runat="server" CssClass="input" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </fieldset>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
