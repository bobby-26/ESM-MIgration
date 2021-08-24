<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderReportSimple.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderReportSimple" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmWorkOrderGeneral" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
   <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="General" ShowMenu="false"></eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuWorkOrderGeneral" runat="server" OnTabStripCommand="MenuWorkOrderGeneral_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlMenuWorkOrderGeneral">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Literal ID="lblDateDone" runat="server" Text="Date Done"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtWorkDoneDate" runat="server" CssClass="input_mandatory" MaxLength="20"
                            Width="90px"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="clreWorkDoneDate" runat="server" Format="dd/MMM/yyyy"
                            Enabled="True" TargetControlID="txtWorkDoneDate" PopupPosition="TopLeft">
                        </ajaxToolkit:CalendarExtender>
                    </td>
                    <td>
                        <asp:Literal ID="lblTotalDurationHrs" runat="server" Text="Total Duration(Hrs.)"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtWorkDuration" runat="server" CssClass="input_mandatory" Mask="999.99"
                            Text="0" Width="60px" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkUnplanned" Text="Unplanned Work" runat="server" />
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkCompleted" Text="Mark Work Order as completed" runat="server" />
                    </td>
                </tr>              
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblReadingDate" runat="server" Text="Reading Date"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDueDate" runat="server" CssClass="input" MaxLength="20" Enabled="false"
                            Width="90px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblReadingValues" runat="server" Text="Reading Values"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="input" MaxLength="20" Enabled="false"
                            Width="90px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblCounterType" runat="server" Text="Counter Type"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Hard ID="ucCounter" runat="server" CssClass="input" HardTypeCode="111" AppendDataBoundItems="true"  />
                    </td>
                    <td>
                        <asp:Literal ID="lblCurrentValue" runat="server" Text="Current Value"></asp:Literal>
                    </td>
                    <td>
                         <eluc:Decimal ID="txtCurrentValues" runat="server" CssClass="input" MaxLength="20"
                             Width="90px" Mask ="999999999.99" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
