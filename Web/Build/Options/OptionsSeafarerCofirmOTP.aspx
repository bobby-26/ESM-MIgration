<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsSeafarerCofirmOTP.aspx.cs" Inherits="OptionsSeafarerCofirmOTP" %>

<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Telephone" Src="~/UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cofirm OTP</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="radscript1" runat="server"></asp:ScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <center><table>
                <tr>
                    <td>Email id</td>
                    <td>
                        <telerik:RadLabel ID="txtemail" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>Enter OTP</td>
                    <td>
                        <telerik:RadTextBox ID="txtotp" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadButton ID="btnsubmit" Text="SUBMIT" OnClick="btnsubmit_Click" runat="server"></telerik:RadButton>
                    </td>
                </tr>
            </table></center>
             <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
            <eluc:Status runat="server" ID="ucStatus" />
        </div>
    </form>
</body>
</html>
