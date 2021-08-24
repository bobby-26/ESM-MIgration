<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkingGearOrderConfirm.aspx.cs" Inherits="Crew_CrewWorkingGearOrderConfirm" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Working Gear Confirm</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function btnconfirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">


        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="btnconfirm" runat="server" Text="btnconfirm" OnClick="btnconfirm_Click" />
            <eluc:TabStrip ID="MenuWorkGearGeneral" runat="server" OnTabStripCommand="MenuWorkGearGeneral_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblTotalAmounttothePaid" runat="server" Text="Total Amount to be Paid"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtTotalAmount" runat="server" CssClass="readonlytextbox" DefaultZero="false" ReadOnly="true"
                            Width="90px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ucCurrency" runat="server" AppendDataBoundItems="true"
                            CssClass="input_mandatory"></eluc:UserControlCurrency>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" TextMode="MultiLine" runat="server"
                            Width="370px">
                        </telerik:RadTextBox>
                    </td>

                </tr>

            </table>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
