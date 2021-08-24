<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCTypeofChangeSubCategoryAdd.aspx.cs"
    Inherits="InspectionMOCTypeofChangeSubCategoryAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MOC SubCategory Add</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:radcodeblock>

</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:radscriptmanager id="radscript1" runat="server"></telerik:radscriptmanager>
        <telerik:radajaxpanel id="panel1" runat="server" height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <eluc:TabStrip ID="MenuMOCSubCategoryAdd" runat="server" OnTabStripCommand="MenuMOCSubCategoryAdd_TabStripCommand"  TabStrip="true">
            </eluc:TabStrip>
       
            <br />
<table runat="server">
    
    <tr>
        <td style="padding-right:30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lbl6" runat="server"  text="Category"></telerik:RadLabel></td>
        <td style="padding-right:30px"><telerik:RadComboBox ID="ddlCategory" runat="server"   AutoPostBack="true" 
             AppendDataBoundItems="true"    Width="270px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Category"></telerik:RadComboBox></td>
        <%--OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"--%>
    </tr>
    
    <tr>
        <td style="padding-right:30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lbl1" runat="server"  text="Code"></telerik:RadLabel></td>
        <td style="padding-right:30px"><telerik:RadTextBox ID="txtCodeAdd" runat="server" CssClass="gridinput_mandatory"  Maxlength="3"   Width="270px"></telerik:RadTextBox></td>
    </tr>
    <tr>
        <td style="padding-right:30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lbl2" runat="server"  text="SubCategory"></telerik:RadLabel></td>
        <td style="padding-right:30px"><telerik:RadTextBox ID="txtSubCategoryAdd" runat="server" CssClass="gridinput_mandatory"
                                  Width="270px"></telerik:RadTextBox></td>
    </tr>
    <tr>
        <td style="padding-right:30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lbl3" runat="server"  text="Proposer Level"></telerik:RadLabel></td>
        <td style="padding-right:30px"><telerik:RadComboBox ID="ddlProposerRoleAdd" runat="server" DataTextField="FLDMOCAPPROVERROLE" DataValueField="FLDMOCAPPROVERROLEID"
                                 Width="270px"   Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Proposer Level">
                            </telerik:RadComboBox></td>
    </tr>
    <tr>
        <td style="padding-right:30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lbl4" runat="server"  text="Approver Level 1"></telerik:RadLabel></td>
        <td style="padding-right:30px"><telerik:RadComboBox ID="ddlTempApproverRoleAdd" runat="server" DataTextField="FLDMOCAPPROVERROLE" DataValueField="FLDMOCAPPROVERROLEID"    
                                 Width="270px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Approval Level 1">
                            </telerik:RadComboBox></td>
    </tr>
    <tr>
        <td style="padding-right:30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lbl5" runat="server"  text="Approver Level 2"></telerik:RadLabel></td>
        <td style="padding-right:30px"><telerik:RadComboBox ID="ddlPermanantApproverRoleAdd" runat="server" 
                              DataTextField="FLDMOCAPPROVERROLE" DataValueField="FLDMOCAPPROVERROLEID"
                                Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Approval Level 2"       Width="270px" >
                            </telerik:RadComboBox></td>
    </tr>

    <tr>
        <td style="padding-right:30px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadLabel ID="lblresponsibleperson" runat="server"  text="Responsible Person"></telerik:RadLabel></td>
        <td style="padding-right:30px"><telerik:RadComboBox ID="ddlresponsiblepersonadd" runat="server" 
                              DataTextField="FLDMOCAPPROVERROLE" DataValueField="FLDMOCAPPROVERROLEID"
                                Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Responsible Person"  Width="270px" >
                            </telerik:RadComboBox></td>
    </tr>
</table>           
                        
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
    </telerik:radajaxpanel>
    </form>
</body>
</html>