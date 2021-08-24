<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollEmployerAdd.aspx.cs" Inherits="PayRoll_PayRollEmployer" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payroll Employer</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
  
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <table>
            <tr>
                <td>Payroll</td>
                <td>
                    <telerik:RadComboBox DropDownPosition="Static" Style="width: 180px" ID="ddlPayroll" runat="server" EnableLoadOnDemand="True"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr> 
                <td>Employee</td>
                <td>
                    <telerik:RadComboBox DropDownPosition="Static" Style="width: 180px" ID="ddlEmployee" runat="server" EnableLoadOnDemand="True"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>Name</td>
                <td><telerik:RadTextBox ID="txtName" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
            <tr>
                 <td>Nature of Employment</td>
                <td><telerik:RadTextBox ID="txtNatureOfEmployement" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
            <tr>
                <td>Address of Employer</td>
                <td><telerik:RadTextBox ID="txtAddress" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
             <tr>
                <td>State</td>
                <td><telerik:RadTextBox ID="txtState" runat="server" Width="180px"> </telerik:RadTextBox></td>    
                <td><eluc:State runat="server" Visible="false" ID="ddlState" Width="150px"/></td>    
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
                <td>TAN No.</td>
                <td><telerik:RadTextBox ID="txtTanNo" runat="server" Width="180px"> </telerik:RadTextBox></td>    
            </tr>
        </table>
 
    </form>
</body>
</html>
