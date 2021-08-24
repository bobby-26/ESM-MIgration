<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCDISIREMatrixDistribution.aspx.cs" Inherits="Inspection_InspectionCDISIREMatrixDistribution" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Distribution</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>


        <script type="text/javascript">
            function fnConfirmTelerik(sender, msg) {
                var callBackFn = function (shouldSubmit) {
                    if (shouldSubmit) {
                        //sender.click();
                        //if (Telerik.Web.Browser.ff) {
                        //    sender.get_element().click();
                        //}
                        eval(sender.target.parentElement.parentElement.href);
                    }
                    else {
                        if (e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                var confirm;

                if (msg == null)
                    confirm = radconfirm("Are you sure you want to delete this record?", callBackFn);
                else
                    confirm = radconfirm(msg, callBackFn);

                return false;
            }
        </script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocument" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand"></eluc:TabStrip>
            <table id="tblDocument" width="100%" align="center" runat="server" style="table-layout: fixed;">
                <tr>
                    <td style="text-align: center">
                        <b>Vessel Type</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divVesselType" runat="server" class="input">
                            <telerik:RadCheckBox ID="chkVesselTypeAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkVesselTypeAll_Changed"
                                Text="---SELECT ALL---" />
                            <br />
                            <telerik:RadCheckBoxList ID="chkVesselType" Columns="3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkVesselType_Changed">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
                </table>
            <table id="Table1" width="100%" align="center" runat="server" style="table-layout: fixed;">
                <tr>
                    <td style="text-align: center">
                        <b>Vessel</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divVessel" runat="server" class="input">
                            <telerik:RadCheckBoxList ID="chkVessel" Columns="6" runat="server">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>

        <%--        <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ucConfirm_Click" OKText="Yes"
            CancelText="No" />--%>
    </form>
</body>
</html>
