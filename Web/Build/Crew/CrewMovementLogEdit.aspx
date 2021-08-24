<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewMovementLogEdit.aspx.cs" Inherits="Crew_CrewMovementLogEdit" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Movement" Src="~/UserControls/UserControlMovement.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .right {
                text-align:right;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuMovementLog" runat="server" OnTabStripCommand="MenuMovementLog_TabStripCommand"></eluc:TabStrip>
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox runat="server" ID="txtName" MaxLength="100" Width="240px"></telerik:RadTextBox>
                </td>                
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                        Width="240px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblMovement" runat="server" Text="Movement"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Movement ID="ddlMovement" runat="server" AppendDataBoundItems="true" CssClass="input"
                        AutoPostBack="true" Width="180px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                     <telerik:RadLabel ID="lblContract" runat="server" Text="Contract"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Address ID="ddlUnion" runat="server" AppendDataBoundItems="true" AddressType="134" Width="240px" EmptyMessage="Type to select contract" />
                </td>
                <td>
                     <telerik:RadLabel ID="lblTotalAmount" runat="server" Text="Total Amount"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtAmount" Enabled="false" CssClass="right"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
