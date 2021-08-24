<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCRoleConfigurationEdit.aspx.cs" Inherits="InspectionMOCRoleConfigurationEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Role Edit</title>

     <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersCountryEdit" runat="server" submitdisabledcontrols="true">
   
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server"    Height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                
                    <eluc:TabStrip ID="MenuMOCRoleConfigurationEdit" runat="server" TabStrip="true"    OnTabStripCommand="MenuMOCRoleConfigurationEdit_TabStripCommand">
                    </eluc:TabStrip>
                <br />
            <table >
             <tr>
                  <td style="padding-right:30px">
                      &nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lblshortcodeedit1"  Text="Short Code"   runat="server"></telerik:RadLabel>
                  </td>
                 <td> 
                     <telerik:RadTextBox ID ="txtShortCodeEdit" Width="270px" MaxLength="3"   CssClass="gridinput_mandatory"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadTextBox>
                 </td>
             </tr>    
             <tr>
                 <td style="padding-right:30px">
                      &nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lblrole"  Text="Category"   runat="server"></telerik:RadLabel>
                  </td>

                 <td>
                 <telerik:RadTextBox ID="txtRoleEdit" runat="server"  Width="270px"     Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCAPPROVERROLE") %>'  
                                          CssClass="gridinput_mandatory" ></telerik:RadTextBox>
                 <telerik:RadLabel ID="lblRoleIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCAPPROVERROLEID") %>' Visible = "false"></telerik:RadLabel>
                 </td>
             </tr>
            </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>