<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFalApprovalRollback.aspx.cs" Inherits="Purchase_PurchaseFalApprovalRollback" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Purchase Fal Approval Rollback</title>
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
          
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

          <eluc:TabStrip ID="Menu" runat="server" OnTabStripCommand="Menu_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
        <br />
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel runat="server" ID="radlblapprovaltitle" Text="Rollback approval to level" />
                </td>
                <td>
                    &nbsp
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="radcblevel" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp
                </td>
            </tr>
            <tr>
                <td>
                <telerik:RadLabel runat="server" ID="radlblreason" Text="Reason" />
                    </td>
                <td>
                    &nbsp
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="radtbreson" TextMode="MultiLine" Rows="4" Width="100%" EmptyMessage="Enter reason for rollback" />
                    <telerik:radtext
                </td>
            </tr>
        </tabl>
             
    </form>
</body>
</html>
