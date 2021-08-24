﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollEmployeePersonalDetail.aspx.cs" Inherits="PayRoll_PayRollEmployeePersonalDetail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Personal Detail</title>
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

    <div>
        
          <table>
            <tr>
                <td>First Name</td>
                <td><telerik:RadTextBox ID="txtFirstName" runat="server"  Width="180px"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtEmployeeId" runat="server" Width="180px" Visible="false"></telerik:RadTextBox>
                </td>   
            </tr>
            <tr> 
                <td>Middle Name</td>
                <td><telerik:RadTextBox ID="txtMiddleName" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
              <tr>
                <td>Last Name</td>
                <td><telerik:RadTextBox ID="txtLastName" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
            <tr> 
                <td>PAN No.</td>
                <td><telerik:RadTextBox ID="txtPAN" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>

            <tr>
                <td>Door No.</td>
                <td><telerik:RadTextBox ID="txtDoorNo" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
            <tr> 
                <td>Premises</td>
                <td><telerik:RadTextBox ID="txtPremises" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
              <tr>
                <td>Street</td>
                <td><telerik:RadTextBox ID="txtStreet" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
            <tr> 
                <td>Locality</td>
                <td><telerik:RadTextBox ID="txtLocality" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>

            <tr>
                <td>Town</td>
                <td><telerik:RadTextBox ID="txtTown" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
              <tr>
                <td>Country</td>
                <%--<td><telerik:RadTextBox ID="txtCountry" runat="server" Width="180px"></telerik:RadTextBox></td>  --%>
                <td><eluc:Country runat="server" ID="ddlCountry" Width="180px" AutoPostBack="true" OnTextChangedEvent="txtCountry_TextChangedEvent"/></td>
            </tr>
            <tr> 
                <td>State</td>
                <%--<td><telerik:RadTextBox ID="txtState" runat="server" Width="180px"></telerik:RadTextBox></td>   --%>
                <td><eluc:State runat="server" ID="ddlState" Width="180px"/></td>
            </tr>
            <tr> 
                <td>Pin Code</td>
                <td><telerik:RadTextBox ID="txtPinCode" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>

            <tr>
                <td>Email Address 1</td>
                <td><telerik:RadTextBox ID="txtEmail1" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
            <tr> 
                <td>Email Address 2</td>
                <td><telerik:RadTextBox ID="txtEmail2" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
              <tr>
                <td>Mobile Number 1</td>
                <td><telerik:RadTextBox ID="txtMobileNo1" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
            <tr> 
                <td>Mobile Number 2</td>
                <td><telerik:RadTextBox ID="txtMobileNo2" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>

             <tr> 
                <td>STD/ISD Code</td>
                <td><telerik:RadTextBox ID="txtSTDISDCode" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>

            <tr>
                <td>Office No</td>
                <td><telerik:RadTextBox ID="txtOfficeNo" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
            <tr> 
                <td>Residence No.</td>
                <td><telerik:RadTextBox ID="txtResidenceNo" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
              <tr>
                <td>Aadhar No.</td>
                <td><telerik:RadTextBox ID="txtAadharNo" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
            <tr> 
                <td>Enrollment Id</td>
                <td><telerik:RadTextBox ID="txtEnrollmentId" runat="server" Width="180px"></telerik:RadTextBox></td>   
            </tr>
        </table>
 

    </div>
    </form>
</body>
</html>
