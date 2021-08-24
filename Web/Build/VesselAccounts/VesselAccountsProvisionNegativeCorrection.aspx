<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsProvisionNegativeCorrection.aspx.cs"
    Inherits="VesselAccountsProvisionNegativeCorrection" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Provision Negative Correction</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmProvisionNegativeCorrection" DecoratedControls="All" />
    <form id="frmProvisionNegativeCorrection" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <eluc:TabStrip ID="MenuFormCorrection" runat="server" OnTabStripCommand="MenuFormCorrection_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="40%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" AppendDataBoundItems="true"
                            AssignedVessels="true" AutoPostBack="true" Entitytype="VSL" ActiveVessels="true" Width="180px" />
                      
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClosingDate" runat="server" Text="Closing Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtClosingDate" runat="server" CssClass="input_mandatory" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
            <input type="button" runat="server" id="Button1" name="isouterpage" style="visibility: hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
