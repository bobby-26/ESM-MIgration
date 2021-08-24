<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanEventAdd.aspx.cs" Inherits="CrewPlanEventAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="~/UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Event</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       <%-- <script type="text/javascript">

            function setHeight(sender, args) {
                window.setTimeout(function () {
                    sender._textBoxElement.style.height = "";
                    window.setTimeout(function () {
                        sender._textBoxElement.style.height = sender._textBoxElement.scrollHeight + "px";
                        sender._originalTextBoxCssText += "height: " + sender._textBoxElement.style.height + ";";
                    }, 1);
                }, 1);
            }

        </script>--%>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewPlanTabs" runat="server" OnTabStripCommand="CrewPlanTabs_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" Width="50%" Entitytype="VSL" ActiveVesselsOnly="true"
                            VesselsOnly="true" AppendDataBoundItems="true" AssignedVessels="true" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEventPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucport" runat="server" Width="50%" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEventDate" runat="server" Text="Starting"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtEventDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                  <tr>
                    <td>
                        <telerik:RadLabel ID="lblEventToDate" runat="server" Text="Ending"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtEventToDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselETA" runat="server" Text="Vessel ETA"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtVesselArrival" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselETD" runat="server" Text="Vessel ETD"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtVesselDepature" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPortAgent" runat="server" Text="Port Agent"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiAddress runat="server" ID="ucAddrPortAgent" AddressType='1255'
                            Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="50%" Resize="Vertical">                          
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

