<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogEngineLog.aspx.cs" Inherits="Log_ElectricLogEngineLog" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Engine log</title>
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
    <script>
        function openWindow() {
            //javascript: parent.openNewWindow('EngineLog', '', 'Log/ElectricLogMainEngineLog.aspx', 'true', null, null, null, null, { 'disableMinMax': true })
            //javascript: parent.openNewWindow('EngineLog', 'Cheif Engineer"s Log', 'Log/ElectricLogMainEngineLogV1.aspx', 'true', null, null, null, null, { 'disableMinMax': true })
            javascript: parent.openNewWindow('EngineLog', 'Chief Engineer"s Log', 'Log/ElecticLogMainEngineLogV2.aspx', 'true', null, null, null, null, { 'disableMinMax': true })
        }
    </script>
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
                        <telerik:radbutton id="btnEngineLogBook" runat="server" visible="True" Cssclass="btnwidth" text="BD 11 - Engine Log Book" AutoPostBack="false" ></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnEngineRoomSoundingLog" runat="server" visible="True" Cssclass="btnwidth" text="BD 12 - Engine Room Sounding Log" ></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnBatteryLog" runat="server" visible="True" Cssclass="btnwidth" text="BD 13 - Battery Log" ></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnEMSLog" runat="server" Cssclass="btnwidth" visible="True" text="BD 14 - EMS Log" ></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnFuelOilRecordBook" runat="server" Cssclass="btnwidth" visible="True" text="BD 15 - Fuel Oil Record Book" ></telerik:radbutton>
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>
