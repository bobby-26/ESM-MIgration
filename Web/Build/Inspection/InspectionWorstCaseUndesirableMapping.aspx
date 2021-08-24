<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionWorstCaseUndesirableMapping.aspx.cs" Inherits="InspectionWorstCaseUndesirableMapping" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection Mapping</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionMapping" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlTypeMapping">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Undesirable Event Mapping" ShowMenu="false">
                        </eluc:Title>
                    </div>
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblWorstCase" runat="server" Font-Bold="true" Text="Worst Case"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWorstCase" runat="server" ReadOnly="true" Width="300px"
                                    CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="2">
                                <asp:Label ID="lblUndesirablEvent" runat="server" Font-Bold="true" Text="Undesirable Event"></asp:Label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="2">
                                <div id="dvInspection" runat="server" class="input" style="overflow: auto; width: 55%;
                                    left:1%; position: absolute; height: 350px;">
                                    <asp:CheckBoxList ID="cblType" runat="server" RepeatDirection="Vertical" RepeatColumns="1">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>

