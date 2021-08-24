<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFalApproveLimitEdit.aspx.cs" Inherits="PurchaseFalApproveLimitEdit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="FalRules" Src="~/UserControls/UserControlPurchaseFalRules.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">


            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPurchaseFalApproveLimitEdit" runat="server" OnTabStripCommand="MenuPurchaseFalApproveLimitEdit_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">

                  <tr>
                    <td>
                         <telerik:RadLabel ID="RadLabel2" runat="server" Text="Fal Approve"></telerik:RadLabel>
                    </td>
                    <td>
                          <telerik:RadLabel ID="txtName" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>     


                 <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Condition"></telerik:RadLabel>
                    </td>
                    <td>
                         <eluc:FalRules ID="UcRules" runat="server" Width="220px" />
                    </td>
                </tr>


                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Approval Limit"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="lblLevel" runat="server" Text="" DecimalPlace="2" IsInteger="true" MaxLength="10" Width="100px" />

                        <telerik:RadLabel ID="txtApprove" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
               
                

            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
