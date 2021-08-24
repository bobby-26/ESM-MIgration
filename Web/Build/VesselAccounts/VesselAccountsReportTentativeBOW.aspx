<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReportTentativeBOW.aspx.cs"
    Inherits="VesselAccountsReportTentativeBOW" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tentative BOW</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTentativeBow" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlTentativeBow">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="div2" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Tentative BOW" ShowMenu="True"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReportTentativeBow" runat="server" OnTabStripCommand="MenuReportTentativeBow_TabStripCommand"></eluc:TabStrip>
                </div>
                <div>
                    <table width="70%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td width="10%">
                                <asp:Literal ID="lblStaffName" runat="server" Text="Staff Name"></asp:Literal>
                            </td>
                            <td>

                                <eluc:VesselCrew ID="ddlEmployee" runat="server" CssClass="input mandatory" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDate" runat="server" CssClass="input_mandatory" Width="60px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtDate" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px; width: 100%;"
                        class="style3"></iframe>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
