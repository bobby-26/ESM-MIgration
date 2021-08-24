<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSGEmployeeConfigEdit.aspx.cs" Inherits="HR_PayRollSGEmployeeConfigEdit" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Employee Payroll Configuration Edit</title>
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
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <br />
        <table>
            <tbody>
                <tr>
                    <td>
                        Employee Code
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblemployeecode" />
                    </td>
                    <td>&nbsp</td>
                    <td>
                        Name
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblname" />
                    </td>
                    <td>&nbsp</td>
                    <td>
                    Race
               </td>
                <td>
                    <eluc:Hard runat="server" ID="ddlrace" HardTypeCode="269" />
                </td>
                </tr>
              <tr>
                    <td colspan="8">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        NRIC No.
                    </td>
                     <td>
                         <telerik:RadTextBox ID="radtbnricno" runat="server" />
                     </td>
                        <td>&nbsp</td>
                    <td>
                        FIN No.
                    </td>
                    
                    <td>
                        <telerik:RadTextBox ID="radtbfinno" runat="server" />
                    </td>
                    <td>&nbsp</td>
                    <td>
                        Immigration File Ref No.
                    </td>
                    <td>
                        <telerik:RadTextBox ID="radimmigrationfileno" runat="server" />
                    </td>
                     </tr>
                 <tr>
                    <td colspan="8">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        Foreign Worker type
                    </td>
                    <td>
                         <eluc:Hard runat="server" ID="ddlworkertype" HardTypeCode="270" OnTextChangedEvent="ddlworkertype_TextChangedEvent" AutoPostBack="true"/>
                    </td>
                     <td>&nbsp</td>
                     <td>
                        Skill level
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="radskill"  HardTypeCode="271" OnTextChangedEvent="radskill_TextChangedEvent" AutoPostBack="true"/>
                    </td>
                    <td>&nbsp</td>
                     <td>
                        FWL Tier
                    </td>
                   
                    <td>
                        <telerik:RadComboBox AllowCustomText="false" runat="server" ID="radfwltier"  />
                    </td>
                    </tr>
                 <tr>
                    <td colspan="8">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        Tax Ref.No 
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="radtbiaxnumber" CssClass="input_mandatory"/>
                    </td>
                    <td>
                        &nbsp
                    </td>
                    <td>
                        CPF Account No.
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="radtbcpfaccountno" />
                    </td>
                    <td colspan="3">

                    </td>
                </tr>
                  <tr>
                    <td colspan="8">
                        <br />
                    </td>
                </tr>
                <tr>
                     <td>
                       Work permit number
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="radtbwpnumber" />
                    </td>
                    <td>&nbsp</td>
                    <td>
                        S Pass number 
                    </td>
                     <td>
                        <telerik:RadTextBox runat="server" ID="radtbspassnumber" />
                    </td>
                    <td>&nbsp</td>
                     <td>
                       Employee Pass number 
                    </td>
                     <td>
                        <telerik:RadTextBox runat="server" ID="radtbemployeepassnumber" />
                    </td>
                </tr>
                
                <tr>
                    <td colspan="8">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                      Applied date
                    </td>
                    <td>
                     <eluc:Date runat="server" ID="radapplydate" />
                    </td>
                   <td>&nbsp</td>
                   <td>
                        Issued date
                    </td>
                   <td>
                       <eluc:Date runat="server" ID="radfromdate" />
                   </td>
                     <td>&nbsp</td>
                    <td>
                       Expiry date
                    </td>
                   <td>
                        <eluc:Date runat="server" ID="radtodate" />
                   </td>
                </tr>
                 <tr>
                    <td colspan="8">
                        <br />
                    </td>
                </tr>
               
            </tbody>
        </table>

    </form>
</body>
</html>
