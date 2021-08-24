<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSManualVesselsAdd.aspx.cs" Inherits="DocumentManagement_DocumentManagementFMSManualVesselsAdd" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FMS File No</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" Text="" />
    <eluc:TabStrip ID="MenuFMSFileNo" runat="server" OnTabStripCommand="FMSFileNo_TabStripCommand"
        TabStrip="true"></eluc:TabStrip>
    <table cellpadding="1" cellspacing="1" width="100%">
        <tr>
            <td>
                <telerik:RadLabel ID="lblNote" runat="server" ForeColor="Blue" Font-Bold="true" Text="Note : Please select vessels to distribute.">
                </telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divVesselType" runat="server" class="input" onscroll="javascript:setScrollPosition('divVesselType','hdnVesselTypeScroll');"
                    style="overflow: auto; height: 200px;">
                    <asp:HiddenField ID="hdnVesselTypeScroll" runat="server" />
                    &nbsp;<telerik:RadCheckBox ID="chkVesselTypeAll" runat="server" AutoPostBack="true"
                        OnCheckedChanged="chkVesselTypeAll_Changed" Text="---SELECT ALL---" />
                    <telerik:RadCheckBoxList ID="chkVesselType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkVesselType_Changed"
                        Direction="Vertical" Columns="2">
                    </telerik:RadCheckBoxList>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divVessel" runat="server" class="input" onscroll="javascript:setScrollPosition('divVessel','hdnVesselScroll');"
                    style="overflow: auto; height: 200px">
                    <asp:HiddenField ID="hdnVesselScroll" runat="server" />
                    <telerik:RadCheckBoxList ID="chkVessel" runat="server" Columns="2" Direction="Vertical">
                    </telerik:RadCheckBoxList>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
