<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogMarpolRecordBook.aspx.cs" Inherits="Log_ElectricLogMarpolRecordBook" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Marpol Records</title>
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
                        <telerik:radbutton id="btnOilRecordBookP1" runat="server" visible="True" Cssclass="btnwidth" text="Oil Record Book Part I (Machinery Space Operations)"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnOilRecordBookP2" runat="server" visible="True" Cssclass="btnwidth" text="Oil Record Book Part II (Cargo/Ballast Operations)"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnCargoRecordBook" runat="server" visible="True" Cssclass="btnwidth" text="Cargo Record Book"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnGarbageRecordBookP1" runat="server" Cssclass="btnwidth" visible="True" text="Garbage Record Book Part I"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnGarbageRecordBookP2" runat="server" Cssclass="btnwidth" visible="True" text="Garbage Record Book Part II"></telerik:radbutton>
                    </td>
                    <td>
                        <telerik:radbutton id="btnAnnexure" runat="server" Cssclass="btnwidth" visible="True" text="Annexure VI Record Book"></telerik:radbutton>
                    </td>

                    

                </tr>
            </table>
    </form>
</body>
</html>
