<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelExceedTicketCancellation.aspx.cs" Inherits="CrewTravelExceedTicketCancellation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ticket Cancellation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadLabel ID="lblNotes" runat="server" Text="Amount is exceeding Standard Cancellation Invoice Amount."></telerik:RadLabel>
            <br />
            <br />
            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Please kindly confirm if :"></telerik:RadLabel>        
            <telerik:RadRadioButtonList ID="rblCancellation" runat="server" Layout="Flow" Columns="1" Direction="Vertical">               
                <Items>
                    <telerik:ButtonListItem Value="1" Text="Credit Note will be received later" />
                    <telerik:ButtonListItem Value="2" Text="Non Refundable ticket" />
                    <telerik:ButtonListItem Value="3" Text="High Cancellation Charges" />
                </Items>
            </telerik:RadRadioButtonList>
            <div id="div1">
                <p style="margin-left: 80px">
                    <asp:Button ID="btnProceed" runat="server"
                        OnClick="cmdProceed_Click" Text="Proceed" Width="45px" />
                </p>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
