<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseVendorAddress.aspx.cs"
    Inherits="PurchaseVendorAddress" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="~/UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Address</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager"></telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" ID="RadSkinManager"></telerik:RadSkinManager>
<telerik:RadCodeBlock ID="radcodeblock2" runat="server">
    <div style="height: 60px;" class="pagebackground">
            <div style="position: absolute; top: 15px;">
                <img id="Img1" runat="server" style="vertical-align: middle" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                    alt="Phoenix" onclick="parent.hideMenu();" />
                <span class="title" style="color: White">
                    <%=Application["softwarename"].ToString() %></span><asp:Label runat="server" ID="lblDatabase"
                        ForeColor="Red" Font-Size="Large" Visible="false" Text="Testing on "></asp:Label><br />
            </div>
    </div>
</telerik:RadCodeBlock>    
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status runat="server" ID="ucStatusMessage" />
    
    
        <eluc:TabStrip ID="MenuOfficeMain" runat="server" OnTabStripCommand="OfficeMain_TabStripCommand" Title="Address">
        </eluc:TabStrip>
    
<telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
            <table align="left" width="100%">
                <tr valign="top">
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtName" CssClass="input_mandatory" Width="90%"></telerik:RadTextBox>
                    </td>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblContactPerson" runat="server" Text="Contact Person 1"></telerik:RadLabel>
                        
                    </td>
                    <td width="80%">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtAttention" runat="server" Width="90%" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>  
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <eluc:Country runat="server" ID="ucCountry" AutoPostBack="true" AppendDataBoundItems="true"
                            CssClass="input" OnTextChangedEvent="ucCountry_TextChanged" Width="250px" />
                    </td>                 
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblInCharge" runat="server" Text="In-Charge"></telerik:RadLabel>
                       
                    </td>
                    <td>
                       <telerik:RadTextBox RenderMode="Lightweight" ID="txtInCharge" runat="server" Width="90%" CssClass="input"></telerik:RadTextBox> 
                    </td>
                </tr>
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblState" runat="server" Text="State"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <eluc:State ID="ucState" CssClass="input" runat="server" AppendDataBoundItems="true"  AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" Width="250px" />
                    </td>   
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblStateAddress" runat="server" Text="Address 1"></telerik:RadLabel>
                       
                    </td>
                    <td>
                       <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddress1" runat="server" Width="90%" CssClass="input"></telerik:RadTextBox> 
                    </td>
                </tr>
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <eluc:city id="ddlCity" runat="server" appenddatabounditems="true" cssclass="input" Width="250px" />
                    </td>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblCityAddress" runat="server" Text="Address 2"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddress2" runat="server" Width="90%" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblPostalCode" runat="server" Text="Postal Code"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtPostalCode" runat="server" Width="250px" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblPostalCodeAddress" runat="server" Text="Address 3"></telerik:RadLabel>
                         
                    </td>
                    <td>
                       <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddress3" runat="server" Width="90%" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        
                    </td>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblAddress" runat="server" Text="Address 4"></telerik:RadLabel>
                         
                    </td>
                    <td>
                       <telerik:RadTextBox RenderMode="Lightweight" ID="txtAddress4" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox> 
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td width="10%">
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblPhone" runat="server" Text="Phone 1"></telerik:RadLabel>
                        
                    </td>
                    <td width="40%">
                        <eluc:PhoneNumber ID="txtPhone1" runat="server" CssClass="input_mandatory" Width="35%"
                            IsMobileNumber="true" />
                    </td>
                    <td width="10%">
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblPhone1" runat="server" Text="Phone 2"></telerik:RadLabel>
                        
                    </td>
                    <td width="40%">
                        <eluc:PhoneNumber ID="txtPhone2" runat="server" CssClass="input" Width="35%" />
                    </td>
                </tr>
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblFax" runat="server" Text="Fax 1"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <eluc:MaskNumber runat="server" ID="txtFax1" CssClass="input" MaskText="##########" Width="35%" />
                    </td>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblFax1" runat="server" Text="Fax 2"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <eluc:MaskNumber runat="server" ID="txtFax2" CssClass="input" MaskText="##########" Width="35%" />
                    </td>
                </tr>
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblEmail" runat="server" Text="Email 1"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtEmail1" runat="server" Width="80%" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblEmail1" runat="server" Text="Email 2"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtEmail2" runat="server" Width="80%" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                       <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblAOHTelephone" runat="server" Text="(AOH) Telephone"></telerik:RadLabel>
                             
                       </td>
                       <td>
                           <eluc:MaskNumber runat="server" ID="txtaohTelephoneno" CssClass="input" MaskText="###########" Width="35%" />
                       </td>
                       <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblAOHMobile" runat="server" Text="(AOH) Mobile"></telerik:RadLabel>
                            
                       </td>
                       <td>
                           <eluc:MaskNumber runat="server" ID="txtaohMobileno" CssClass="input" MaskText="###########" Width="35%" />
                       </td>
                </tr>
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblWebSite" runat="server" Text="Web Site"></telerik:RadLabel>
                        
                    </td>
                    <td>
                       <telerik:RadTextBox RenderMode="Lightweight" ID="txtURL" runat="server" Width="80%" CssClass="input"></telerik:RadTextBox> 
                    </td>
                    <td colspan="2">
                        
                    </td>
                </tr>
            </table>
            <br clear="all" />
    
    <telerik:RadPanelBar RenderMode="Lightweight" ID="MyAccordion" runat="server" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Business Type" Width="100%">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="Business Type"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <telerik:RadCheckBoxList RenderMode="Lightweight" runat="server" ID="cblProduct" Height="90%" Columns="3"
                                Direction="Vertical" Layout="Flow" AutoPostBack="false">
                            </telerik:RadCheckBoxList>
                        </ContentTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
