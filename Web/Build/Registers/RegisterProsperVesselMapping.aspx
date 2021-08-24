<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterProsperVesselMapping.aspx.cs" Inherits="Registers_RegisterProsperVesselMapping" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Map Vessel Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTemplateMapping" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


                <eluc:TabStrip ID="MenuTemplateMapping" runat="server" OnTabStripCommand="MenuTemplateMapping_TabStripCommand"></eluc:TabStrip>

                <br />
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td width="90">
                            <telerik:RadLabel ID="lblmeasure" runat="server" Text="Measure"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtmeasure" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="385px"></telerik:RadTextBox>
                        </td>
                        <%--  <td>
                            <telerik:RadLabel ID="lblevent" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:TextBox ID="txtevent" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="385px"></asp:TextBox>
                        </td>--%>
                    </tr>
                    <tr>
                        <td colspan="4"></td>
                    </tr>
                    <tr>
                        <td width="90">
                            <telerik:RadLabel ID="lblevent" runat="server" Text="Vessel Type"></telerik:RadLabel></td>
                        <td colspan="3">
                            <div id="divVesseltype" runat="server" visible="true" class="input" style="overflow-y: auto; overflow-x: auto; width: 100%; height: 100%">
                                <asp:CheckBoxList ID="cblvesseltype" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="3">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
