<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewVesselPositionArrival.aspx.cs" Inherits="Crew_CrewVesselPositionArrival" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DTime" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="UserControlStatus" TagPrefix="eluc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Arrival Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuVesselPosition" runat="server" OnTabStripCommand="MenuVesselPosition_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" AppendDataBoundItems="true" Entitytype="VSL" ActiveVesselsOnly="true" AssignedVessels="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoyage" runat="server" Text=" Voyage (Month & Year)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlHard" runat="server" CssClass="input_mandatory" HardTypeCode="55" AppendDataBoundItems="true" SortByShortName="true" />
                        <eluc:Quick ID="ddlQuick" runat="server" CssClass="input_mandatory" QuickTypeCode="55" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ddlPortAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" SeaportList='<%#PhoenixRegistersSeaport.ListSeaport() %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel></td>
                    <td>
                        <eluc:Date ID="txtETAAdd" runat="server"  />
                        <telerik:RadTimePicker ID="txtETATimeAdd" runat="server"  Width="75px"   />                  
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblETD" runat="server" Text="ETD"></telerik:RadLabel></td>
                    <td>
                        <eluc:Date ID="txtETDAdd" runat="server"  />
                        <telerik:RadTimePicker ID="txtETDTimeAdd" runat="server"  Width="75px"  />                   
                    </td>
                </tr>
            </table>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
        </div>
        
    </form>
</body>
</html>
