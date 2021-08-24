<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersPortAgent.aspx.cs"
    Inherits="RegistersPortAgent" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add/Edit Port Agent</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <asp:Literal ID="lblAddress" runat="server" Text="Address"></asp:Literal></div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuCompanyList" runat="server" OnTabStripCommand="AgentPortList_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td width="15%">
                    <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtAgentName" runat="server" CssClass="input_mandatory" Width="360px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                </td>
                <td>
                    <eluc:Vessel ID="ucVesselName" runat="server" VesselsOnly="true" AppendDataBoundItems="true"
                        CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblAddressType" runat="server" Text="Address Type"></asp:Literal>
                </td>
                <td>
                    <eluc:Hard ID="ddlAddressType" runat="server" CssClass="input_mandatory" HardTypeCode="33"
                         AppendDataBoundItems="true" ShortNameFilter="CHA,AGT" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblAddress1" runat="server" Text="Address1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAddress1" CssClass="input_mandatory" Width="360px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblAddress2" runat="server" Text="Address2"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAddress2" CssClass="input" Width="360px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblAddress3" runat="server" Text="Address3"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAddress3" CssClass="input" MaxLength="200" Width="360px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Literal ID="lblFullAddress" runat="server" Text="Full Address"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFullAddress" TextMode="MultiLine" Width="360px"
                        Height="75px" CssClass="input"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblPostalCode" runat="server" Text="Postal Code"></asp:Literal>
                    <td>
                        <asp:TextBox runat="server" ID="txtPostalCode" CssClass="input" MaxLength="20" Width="180px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblCountry" runat="server" Text="Country"></asp:Literal>
                </td>
                <td>
                    <eluc:Country ID="ucCountry" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                        CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblState" runat="server" Text="State"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtState" runat="server" CssClass="input" MaxLength="20" Width="180px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblCity" runat="server" Text="City"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtCity" runat="server" CssClass="input" MaxLength="20" Width="180px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblTelephoneNumber1" runat="server" Text="Telephone Number1"></asp:Literal>
                    <td>
                        <asp:TextBox runat="server" ID="txtTelephoneNo1" CssClass="input" MaxLength="20"
                            Width="180px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblTelephoneNumber2" runat="server" Text="Telephone Number2"></asp:Literal>
                    <td>
                        <asp:TextBox runat="server" ID="txtTelephoneNo2" CssClass="input" MaxLength="20"
                            Width="180px"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblFaxNumber1" runat="server" Text="Fax Number1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFaxNo1" CssClass="input" MaxLength="20" Width="180px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblFaxNumber2" runat="server" Text="Fax Number2"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFaxNo2" CssClass="input" MaxLength="20" Width="180px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblEMail1" runat="server" Text="E-Mail 1"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEmail1" CssClass="input" MaxLength="200" Width="360px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblEMail2" runat="server" Text="E-Mail 2"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEmail2" CssClass="input" MaxLength="200" Width="360px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
