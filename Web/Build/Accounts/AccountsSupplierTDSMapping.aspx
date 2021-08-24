<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSupplierTDSMapping.aspx.cs"
    Inherits="AccountsSupplierTDSMapping" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Company</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
      <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server" >
   <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
  
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
      
              
         
            <eluc:TabStrip ID="MenuSupplierRegister" runat="server" OnTabStripCommand="MenuSupplierRegister_TabStripCommand">
            </eluc:TabStrip>
       
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td width="10%">
                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text="Supplier Name"></telerik:RadLabel>
                </td>
                <td width="63%">
                    <telerik:RadTextBox runat="server" ID="txtSupplierName" MaxLength="2" Width="180px" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtSupplierCode" MaxLength="50" Width="360px" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTDSType" runat="server" Text="TDS Contractor Type"></telerik:RadLabel>
                </td>
                <td>
                    
                      <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlTDSType"  AutoPostBack="true" EnableLoadOnDemand="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                            <telerik:RadComboBoxItem Text="Company" Value="0" />
                            <telerik:RadComboBoxItem Text="Non-Company" Value="1" />
                        </Items>
                    </telerik:RadComboBox>
                    <%--<asp:DropDownList ID="ddlTDSType" runat="server" CssClass="input">
                        <asp:ListItem Text = "--Select--" Value="Dummy"></asp:ListItem>
                        <asp:ListItem Text = "Company" Value="0"></asp:ListItem>
                        <asp:ListItem Text = "Non-Company" Value="1"></asp:ListItem>    
                    </asp:DropDownList>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblWCTType" runat="server" Text="TDS on WCT Type"></telerik:RadLabel>
                </td>
                <td>
                     <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlWCTType"  AutoPostBack="true" EnableLoadOnDemand="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                            <telerik:RadComboBoxItem Text="Registered dealer" Value="0" />
                            <telerik:RadComboBoxItem Text="Non registered dealer" Value="1" />
                        </Items>
                    </telerik:RadComboBox>
                   <%-- <asp:DropDownList ID="ddlWCTType" runat="server" CssClass="input">
                        <asp:ListItem Text = "--Select--" Value="Dummy"></asp:ListItem>
                        <asp:ListItem Text = "Registered dealer" Value="0"></asp:ListItem>
                        <asp:ListItem Text = "Non registered dealer" Value="1"></asp:ListItem>    
                    </asp:DropDownList>--%>
                </td>
            </tr>
        </table>
   </telerik:RadAjaxPanel>
    </form>
</body>
</html>
