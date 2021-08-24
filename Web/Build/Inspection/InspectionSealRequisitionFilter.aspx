<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealRequisitionFilter.aspx.cs" Inherits="InspectionSealRequisitionFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Requisition Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealReqFilter" runat="server">
       
        <eluc:TabStrip ID="MenuSealFilterMain" Title="Seal Requisition Filter" runat="server" OnTabStripCommand="MenuSealFilterMain_TabStripCommand"></eluc:TabStrip>

        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlSealEntry">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" Width="240px" VesselsOnly="true"
                                 />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRequestNoHeader" runat="server" Text="Request No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRefNo" MaxLength="50" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtFromDate" runat="server"  DatePicker="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtToDate" runat="server"  DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRequestStatus" runat="server" Text="Request Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" 
                                HardTypeCode="196" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
