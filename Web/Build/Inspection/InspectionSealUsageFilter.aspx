<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealUsageFilter.aspx.cs" Inherits="InspectionSealUsageFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Usage Filter</title>
    <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealUsageFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
     
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuSealFilterMain" Title="Seal Usage Filter" runat="server" OnTabStripCommand="MenuSealFilterMain_TabStripCommand"></eluc:TabStrip>
        </div>

        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSealType" runat="server" Text="Seal Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucSealType" Width="200px" runat="server"  AppendDataBoundItems="true" QuickTypeCode="87" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlLocation" DefaultMessage="--Select--" runat="server"  DataTextField="FLDLOCATIONNAME" 
                        DataValueField="FLDLOCATIONID" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select the location"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSealNumber" runat="server" Text="Seal Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSealNumber" runat="server" ></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblSealAffixedBy" runat="server" Text="Seal Affixed by"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSealAffixedby" runat="server" ></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server"  />
                </td>
                <td>
                    <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server"  />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReason" runat="server" Text="Reason"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucReason" runat="server"  AppendDataBoundItems="true" QuickTypeCode="88" />
                </td>
            </tr>
        </table>
      
    </form>
</body>
</html>
