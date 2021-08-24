<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewDeBriefingSummaryFilter.aspx.cs"
    Inherits="CrewDeBriefingSummaryFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Src="../UserControls/UserControlRank.ascx" TagName="Rank" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlHard.ascx" TagName="Hard" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlVesselCommon.ascx" TagName="Vessel" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>De-Briefing Summary Filter</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>


        <eluc:TabStrip ID="MenuPD" runat="server" OnTabStripCommand="PD_TabStripCommand"></eluc:TabStrip>


        <div id="divFind">
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" Width="150px" runat="server"  AppendDataBoundItems="true" Entitytype="VSL" ActiveVesselsOnly="true" VesselsOnly="true" AssignedVessels="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" Width="150px" runat="server" AppendDataBoundItems="true"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromdate" runat="server" Text="Received From "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromdate" runat="server"  DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltodate" runat="server" Text=" To "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtTodate" runat="server"  DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatusBy" runat="server" Text="Status"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Hard ID="ucstatus" Width="150px" runat="server" AppendDataBoundItems="true" 
                            HardTypeCode="146" ShortNameFilter=",OPN,REV,"></eluc:Hard>
                    </td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
