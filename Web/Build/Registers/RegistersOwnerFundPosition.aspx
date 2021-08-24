<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOwnerFundPosition.aspx.cs" Inherits="Registers_RegistersOwnerFundPosition" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WCT Summary</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </div>
   
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOwnerFundPosition" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" />
    <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
   
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title3" Text="OwnerFundPosition" ShowMenu="true">
            </eluc:Title>
            <asp:Button runat="server" ID="cmdHiddenSubmit" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="OwnerFundPosition" runat="server" OnTabStripCommand="OwnerFundPositiontab_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
       <table id="tbldiv" runat="server" cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <div class="subHeader" style="position: relative;">
                        <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                            <eluc:TabStrip ID="OwnerFund" runat="server" OnTabStripCommand="OwnerFundPosition_TabStripCommand">
                            </eluc:TabStrip>
                        </span>
                    </div>
                </td>
            </tr>
        </table>
        <div>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td style="width: 10%">
                        <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                    </td>
                    <td style="width: 80%">
                        <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128"
                            OnTextChangedEvent="ucOwner_Onchange" AutoPostBack="true" AppendDataBoundItems="true" />
                    </td>
                </tr>
               
                <tr>
                    <td>
                         <asp:Literal ID="lblVesselname" runat="server" Text="Vessel Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList id="ddlvessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"></asp:DropDownList>
                    </td> 
                 
                </tr>
                 <tr>
                    <td style="width: 20%">
                        <asp:Literal ID="lblAsondate" runat="server" Text="As on Date"></asp:Literal>
                    </td>
                    <td style="width: 20%">
                        <eluc:Date ID="ucAsondate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>
        </div>
        
    </div>
    </form>
</body>
</html>
