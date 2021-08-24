<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogDeckLog.aspx.cs" Inherits="Log_ElectricLogDeckLog" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deck log</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:radcodeblock>
    <style>
        .btnwidth{
            width:105px;
            height:105px;
            padding-top:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />
        <br />
        <br />

             <table cellpadding="2px" cellspacing="0px" style="width: 100%;" id="logmenu">
                  <tr style="text-align: center;">
                    <td>
                        <telerik:radbutton id="btnDeckSoundingLog" runat="server" visible="True" Cssclass="btnwidth" text="BD 1 - Deck Sounding Book"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnBridgeNoteBook" runat="server" visible="True" Cssclass="btnwidth" text="BD 2 - Bridge Note Book"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnAnchorWatchLog" runat="server" visible="True" Cssclass="btnwidth" text="BD 3 - Anchor Watch Log"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnGpsEchoSounderLog" runat="server" Cssclass="btnwidth" visible="True" text="BD 5 - GPS Echo sounder Log"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnRadarLog" runat="server" Cssclass="btnwidth" visible="True" text="BD 7 - Radar Log"></telerik:radbutton>
                    </td>
                </tr>
                <tr style="text-align: center;" class="second-row">
                
                    <td>
                        <telerik:radbutton id="btnDeckLogBook" runat="server" visible="True" Cssclass="btnwidth" text="BD 8 - Deck Log book"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnCompassObservationLog" runat="server" visible="True" Cssclass="btnwidth" text="BD 10 - Compass Observation log"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnTowingLog" runat="server" visible="True" Cssclass="btnwidth" text="BD 18 - Towing Log"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnAnchorHandlingLog" runat="server" Cssclass="btnwidth" visible="True" text="BD 19 - Anchor Handling Log"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnDPLog" runat="server" Cssclass="btnwidth" visible="True" text="BD 20 - DP Log"></telerik:radbutton>
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>