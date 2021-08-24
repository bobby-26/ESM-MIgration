<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHotelBookingQuoteConfirmation.aspx.cs" Inherits="CrewHotelBookingQuoteConfirmation" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hotel Confirmation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <table cellspacing="5" cellpadding="5">
                <tr>
                    <td id="Td1" width="100%" valign="top">
                        <h1 id="H1" style="font: 14pt/16pt verdana; color: #0000ff">
                            <telerik:RadLabel ID="lblThankyouforsubmittingthebid" runat="server" Text="Thank you for submitting the bid."></telerik:RadLabel></h1>
                    </td>
                </tr>
                <tr>
                    <td id="tableProps" width="100%" valign="top">
                        <telerik:RadLabel ID="lbltext" runat="server"> </telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
