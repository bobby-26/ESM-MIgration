<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationUpdate.aspx.cs" Inherits="Inspection_InspectionRegulationUpdate" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Regulation update</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">


            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
                <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
                <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                    <eluc:TabStrip ID="NewRegulation" runat="server" OnTabStripCommand="NewRegulation_TabStripCommand"></eluc:TabStrip>
                </telerik:RadCodeBlock>

                <table style="width: 100%;">

                    <tr>
                        <td style="width: 15%">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                        </td>
                        <td style="width: 35%">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtTitle" runat="server" Enabled="true" TextMode="MultiLine" Rows="2" Resize="Both" Width="98%"></telerik:RadTextBox>
                        </td>

                        <td style="width: 15%">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblRegulationId" runat="server" Visible="false"></telerik:RadLabel>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblIssuedDate" runat="server" Text="Issued Date"></telerik:RadLabel>
                        </td>
                        <td style="width: 35%">
                            <eluc:Date runat="server" ID="txtIssuedDate" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblIssuedBy" runat="server" Text="Issued By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtIssuedBy" runat="server" TextMode="MultiLine" Rows="1" Enabled="true" Resize="Both" Width="98%"></telerik:RadTextBox>
                        </td>

                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblDueDate" runat="server" Text="Due Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtDuedate" />
                        </td>

                    </tr>
                    <tr style="vertical-align: top;">
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblActionRequired" runat="server" Text="Action Required"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" TextMode="MultiLine" ID="txtActionRequired" runat="server" Width="98%" Rows="5" Enabled="true" Resize="Both"></telerik:RadTextBox>
                        </td>
                        <td rowspan="3">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                        </td>
                        <td rowspan="3">
                            <div style="height: 300px; overflow: auto; border: 1px; border-style: groove; width: 98%;">
                                <telerik:RadListBox RenderMode="Lightweight" ID="chkvesselList" DataTextField="FLDTYPEDESCRIPTION" DataValueField="FLDVESSELTYPEID"
                                    CheckBoxes="true" ShowCheckAll="true" runat="server" Width="100%">
                                </telerik:RadListBox>
                            </div>
                        </td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescription" runat="server" TextMode="Multiline" Enabled="true" Width="98%" Rows="5" Resize="Both"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" TextMode="MultiLine" ID="txtRemarks" runat="server" Width="98%" Rows="5" Enabled="true" Resize="Both"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
