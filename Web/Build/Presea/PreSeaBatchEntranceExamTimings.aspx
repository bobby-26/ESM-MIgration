<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchEntranceExamTimings.aspx.cs"
    Inherits="PreSeaBatchEntranceExamTimings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Venue" Src="~/UserControls/UserControlPreSeaExamVenue.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Date Of Availabilty</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewDateOfAvailability" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDOA">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td style="width: 10%">
                            Batch
                        </td>
                        <td style="width: 15%">
                            <asp:TextBox ID="txtBatchName" runat="server" CssClass="readonlytextbox" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            Exam Venue
                        </td>
                        <td>
                            <asp:TextBox ID="txtExamVenue" runat="server" CssClass="readonlytextbox" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            Exam Date
                        </td>
                        <td>
                            <eluc:Date ID="txtStartDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            Reporting Time
                        </td>
                        <td>
                            <eluc:MaskNumber runat="server" ID="txtReportTime" CssClass="input_mandatory" Mask="99:99" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <h4>
                                Exam day timing Details</h4>
                            <cc1:Editor ID="Editor1" runat="server" Height="200px" Width="100%" />
                        </td>
                    </tr>
                </table>
                <br style="clear: both;" />
                
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
