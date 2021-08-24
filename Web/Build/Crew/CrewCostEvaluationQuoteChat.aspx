<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCostEvaluationQuoteChat.aspx.cs"
    Inherits="CrewCostEvaluationQuoteChat" EnableEventValidation="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Cost Evaluation Quote</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">

            function trigger(keyCode, event) {
                var evt = e ? e : window.event;
                if (keyCode == 13) {
                    if (!event.shiftKey == 1) {
                        document.getElementById("<% =btnPost.ClientID%>").click();
                    }
                    else {
                        return;
                    }
                }
                else {
                    return;
                }
            }

            function DoWaterMarkOnFocus(txt) {
                var text = document.getElementById("defauttext");
                if (txt.value == text.defaultValue) {
                    txt.value = "";
                }
                txt.style.color = "black";
            }
            function DoWaterMarkOnBlur(txt) {
                var text = document.getElementById("defauttext");

                if (txt.value == "") {
                    txt.value = text.defaultValue;
                    txt.style.color = "Gray";
                }
                else {
                    txt.style.color = "black";
                }
            }

        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewTravelQuotationItems" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTab" runat="server" Title=""></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="50%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table width="100%" border="0">
                <tr>
                    <td colspan="3">
                        <iframe runat="server" id="ifMoreInfoQuote" style="min-height: 300px; width: 100%"
                            scrolling="yes"></iframe>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPostCommentsHere" runat="server" Font-Bold="true" Text="Post Comments Here"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtChat" runat="server" TextMode="MultiLine" onkeypress="trigger(event.keyCode,event)"
                            onblur="DoWaterMarkOnBlur(this)" onfocus="DoWaterMarkOnFocus(this)" Width="700px">
                        </telerik:RadTextBox>
                        <asp:Button ID="btnPost" runat="server" Text="Button" OnClick="btnPost_Click" Width="1px"
                            Height="1px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <iframe runat="server" id="ifMoreInfoChat" style="min-height: 500px; width: 100%"
                            scrolling="yes"></iframe>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
