<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderPostponementQuestionAdd.aspx.cs" 
    Inherits="PlannedMaintenance_PlannedMaintenanceWorkOrderPostponementQuestionAdd" %>

<!DOCTYPE html>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript">
            function checkTextAreaMaxLength(textBox, e, length) {

                var mLen = textBox["MaxLength"];
                if (null == mLen)
                    mLen = length;

                var maxLength = parseInt(mLen);
                if (!checkSpecialKeys(e)) {
                    if (textBox.value.length > maxLength - 1) {
                        if (window.event)//IE
                            e.returnValue = false;
                        else//Firefox
                            e.preventDefault();
                    }
                }
            }

            function checkSpecialKeys(e) {
                if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                    return false;
                else
                    return true;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text=""></eluc:Status>
            <eluc:TabStrip ID="MenuRAPostponementQuestions" runat="server" OnTabStripCommand="MenuRAPostponementQuestions_TabStripCommand"
                Title="Add"></eluc:TabStrip>
            <table width="90%" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblQuestionNo" runat="server" Text="Question No"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtQuestionNo" runat="server" CssClass="input_mandatory" IsPositive="true"
                            IsInteger="true" Width="50px" MaxLength="3" />
                    </td>                     
                </tr> 
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblQuestionName" runat="server" Text="Question Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtQuestion" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                            onkeyDown="checkTextAreaMaxLength(this,event,'500');" Width="300px" Height="80px" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrderNo" runat="server" Text="Order No"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtOrderNo" runat="server" CssClass="input_mandatory" IsPositive="true"
                            IsInteger="true" Width="50px" MaxLength="3" />
                    </td>                     
                </tr> 
                <tr>
                     <td>
                        <telerik:RadLabel ID="lblCommentsYN" runat="server" Text="Comments Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkCommentsYN" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>     
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkActiveYN" runat="server" Checked="true"></telerik:RadCheckBox>
                    </td>
                </tr>    
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
