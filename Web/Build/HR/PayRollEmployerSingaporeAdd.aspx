<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollEmployerSingaporeAdd.aspx.cs" Inherits="PayRoll_PayRollEmployerSingapore" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employer Singapore</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
   
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <table>
           
            <tr>
                <td>Company Name</td>
                <td><telerik:RadTextBox ID="txtcompanyName" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
         
            <tr>
                <td>Address of Employer</td>
                <td><telerik:RadTextBox ID="txtAddress" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
              <tr>
                <td>Country</td>
             
                <td>  <eluc:Country runat="server" ID="ddlcountry" AppendDataBoundItems="true"
                        AutoPostBack="true" Width="180px" OnTextChangedEvent="ddlCountry_Changed" /></td>    
            </tr>
             <tr>
                <td>State</td>
             
                <td><eluc:State runat="server" ID="ucState" AppendDataBoundItems="true"
                        Width="180px"/></td>    
            </tr>
              <tr>
                <td>City/Town</td>
                <td><telerik:RadTextBox ID="txtCity" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
             <tr>
                <td>Pin code</td>
                <td><telerik:RadTextBox ID="txtPincode" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
             <tr>
                <td>CSN No.</td>
                <td><telerik:RadTextBox ID="txtCSNNo" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
            <tr>
                <td>Unique Entity No.</td>
                <td><telerik:RadTextBox ID="txtunique" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
            <tr>
                <td>SingPass</td>
                <td><telerik:RadTextBox ID="txtsingpass" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
        </table>
 
    </form>
</body>
</html>
