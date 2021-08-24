<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormAddress.aspx.cs"
    Inherits="PurchaseFormAddress" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Address</title>
    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvForm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvForm" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" LoadingPanelID="RadAjaxLoadingPanel"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>  
        <br clear="all" />
            <table align="left" width="100%">
                <tr valign="top">
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight"  runat="server" ID="txtName" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                    </td> 
                    </tr> 
                <tr valign="top">           
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblAddress1" runat="server" Text="Address 1"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddress1" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                </td>
                </tr> 
                <tr valign="top"> 
                    <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblAddress2" runat="server" Text="Address 2"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddress2" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                </td>
                </tr> 
                <tr valign="top"> 
                    <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblAddress3" runat="server" Text="Address 3"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddress3" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                </td>
                </tr> 
                <tr valign="top"> 
                    <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblAddress4" runat="server" Text="Address 4"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddress4" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                </td> 
                </tr> 
                <tr valign="top"> 
                    <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:City ID="ddlCity" runat="server" AppendDataBoundItems="true" Width="300px" Enabled="false"/>                            
                    </td> 
                </tr> 
                <tr valign="top"> 
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblState" runat="server" Text="State"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:State ID="ddlState" runat="server" AppendDataBoundItems="true"  Width="300px"/>
                                           
                </td>
                </tr> 
                <tr valign="top"> 
                              
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Country runat="server" ID="ucCountry" AutoPostBack="true" AppendDataBoundItems="true" Enabled="false"
                        Width="300px" />
                </td>  
                </tr> 
                <tr valign="top">                 
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPostalCode" runat="server" Text="Postal Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtPostalCode" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                </td>
                </tr> 
                <tr valign="top"> 
                
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPhone1" runat="server" Text="Phone 1"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtPhone1" Width="300px" runat="server" ReadOnly="true"
                        IsMobileNumber="true"/>
                </td>
                </tr> 
                <tr valign="top"> 
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPhone2" runat="server" Text="Phone 2"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtPhone2" Width="300px" runat="server" ReadOnly="true" />
                </td>
                </tr> 
                <tr valign="top"> 
                
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblFax1" runat="server" Text="Fax 1:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtFax1" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                </td>
                </tr> 
                <tr valign="top"> 
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblFax2" runat="server" Text="Fax 2:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtFax2" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                </td>
                </tr> 
                <tr valign="top"> 
               
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblEmail1" runat="server" Text="Email 1"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtEmail1" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                </td>
                </tr> 
                <tr valign="top"> 
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblEmail2" runat="server" Text="Email 2"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtEmail2" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                </td> 
                </tr> 
                <tr valign="top">               
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblWebSite" runat="server" Text="Web Site"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtURL" runat="server" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                </td>               
            </tr>
            </table>
    </form>
</body>
</html>
