<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSCView.aspx.cs" Inherits="Dashboard_DashboardBSCView" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>KPI Score Card</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .tblstyle td {
                border: 1px thin black;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
                DecorationZoneID="gvKPITargetlist" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" Width="100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
               <table border="1" cellspacing="0" cellpadding="0" rules="all" style="font-size: 11px; width: 100%; border-collapse: collapse; text-wrap: normal;"  runat="server">
                   <tr>
                       <td align="right" >
                          <telerik:RadLabel runat="server" ID="radlblmonthyear" Font-Bold="true"/>
                       </td>
                   </tr>
               </table>
                <table border="1" class="tblstyle" cellspacing="0" cellpadding="0" rules="all" style="font-size: 11px; width: 100%; border-collapse: collapse; text-wrap: normal;">
                    <tr><td>
                <telerik:RadLabel ID="kpitable" runat="server" Text="" Width="100%"></telerik:RadLabel>
                        </td>
                        </tr>
                    </table>
                 <table border="1" cellspacing="0" cellpadding="0" rules="all" style="font-size: 11px; width: 100%; border-collapse: collapse; text-wrap: normal;" id="pidiv" runat="server">
                    <tr>
                        <td align="center" style="width: 15%; background-color: #bdd7ee">

                            <telerik:RadLabel ID="radmeasurefrequency" runat="server" Text="Measure Frequency"></telerik:RadLabel>
                        </td>
                        <td style="width: 85%">

                            <telerik:RadLabel ID="pitable" runat="server" Text="" Width="100%"></telerik:RadLabel>

                        </td>
                    </tr>
                </table>
                 <table border="1" cellspacing="0" cellpadding="0" rules="all" style="font-size: 11px; width: 100%; border-collapse: collapse; text-wrap: normal;" id="lidiv" runat="server">
                    <tr>
                        <td align="center" style="width: 15%; background-color: #ffd966">

                            <telerik:RadLabel ID="lblactions" runat="server" Text="Actions"></telerik:RadLabel>
                        </td>
                        <td style="width: 85%;">

                            <telerik:RadLabel ID="tblli" runat="server" Text="" Width="100%"></telerik:RadLabel>

                        </td>
                    </tr>
                </table>
                 <table border="1" cellspacing="0" cellpadding="0" rules="all" style="font-size: 11px; width: 100%; border-collapse: collapse; text-wrap: normal;" id="initiativetbl" runat="server">
                    <tr>
                        <td align="center" style="width: 20%; background-color: #a9d08e">

                            <telerik:RadLabel ID="lblini" runat="server" Text="Initiatives"></telerik:RadLabel>
                        </td>
                        <td style="width: 80%;">

                            <telerik:RadLabel ID="tblini" runat="server" Text="" Width="100%"></telerik:RadLabel>

                        </td>
                    </tr>
                </table>
                 <table border="1" cellspacing="0" cellpadding="0" rules="all" style="font-size: 11px; width: 100%; border-collapse: collapse; text-wrap: normal;" runat="server" id="tbexecution">

                    <tr>
                        <th align="center" width="20%">
                            <telerik:RadLabel runat="server" Text="Execution Team" />
                        </th>

                        <td width="80%">
                            <telerik:RadComboBox runat="server" EmptyMessage="Type to Select Team" CheckBoxes="true" AllowCustomText="true" Width="100%" ID="radexecutioncb">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
                <telerik:RadLabel ID="issuetable" runat="server" Text="" Width="100%"></telerik:RadLabel>

            </telerik:RadAjaxPanel>

        </div>
    </form>
</body>
</html>
