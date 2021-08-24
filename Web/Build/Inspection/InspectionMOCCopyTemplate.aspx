<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCCopyTemplate.aspx.cs" Inherits="InspectionMOCCopyTemplate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Copy Requisition Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="Div1" runat="server">
            <%: Scripts.Render("~/bundles/js") %>
            <%: Styles.Render("~/bundles/css") %>
        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComment" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <div class="subHeader" style="position: relative">
                <div id="divHeading" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Copy MOC Template" ShowMenu="false"></eluc:Title>
                </div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuComment" runat="server" OnTabStripCommand="MenuComment_TabStripCommand"></eluc:TabStrip>
            </div>
            <br />
            <table id="tblcopy" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblFromVessel" runat="server" Text="From Vessel"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVesselFrom" AppendDataBoundItems="true" CssClass="input_mandatory" AssignedVessels="true" Enabled="false"
                            VesselsOnly="true"/>
                    </td>
                    <td>
                        <asp:Literal ID="lblToVessel" runat="server" Text="To Vessel"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVesselTo" AppendDataBoundItems="true" AutoPostBack="true" CssClass="input_mandatory" AssignedVessels="true"
                            VesselsOnly="true" OnTextChangedEvent="ucVesselTo_TextChangedEvent"/>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" Enabled="false" AppendDataBoundItems="true"
                            CssClass="input" Width="240px" />
                    </td>
                </tr>
            </table>

        </div>

    </form>
</body>
</html>
