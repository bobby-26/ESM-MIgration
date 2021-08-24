<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationClosedVesselFilter.aspx.cs" Inherits="Inspection_InspectionRegulationClosedVesselFilter" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Regulation Closed Vessel Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
         <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

          <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

          <div id="divFind">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblComplianceStatus" runat="server" Text="Complian Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="chkComplianceStatusAdd" runat="server" AutoPostBack="false">
                                <Items>
                                    <telerik:ButtonListItem Text="Regulation Non Complianed By Vessel (Open)" Value="1097" />
                                    <telerik:ButtonListItem Text="Regulation Complianed By Vessel (Completed)" Value="717"/>
                                    <telerik:ButtonListItem Text="Regulation Complianed By Vessel and Office (Closed)" Value="1049"/>
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </form>
</body>
</html>
