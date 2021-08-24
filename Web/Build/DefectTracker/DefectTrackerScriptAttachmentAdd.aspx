<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerScriptAttachmentAdd.aspx.cs"
    Inherits="DefectTrackerScriptAttachmentAdd" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Module" Src="~/UserControls/UserControlSEPModuleList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Script Attachment</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                Add Script File</div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuDefectTrackerScriptAdd" runat="server" OnTabStripCommand="DefectTrackerScriptAdd_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" width="75%">
            <tr>
                <td>
                    Subject
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSubject" CssClass="input_mandatory" Width="50%" />
                </td>
            </tr>
            <tr>
                <td>
                    Created by
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCreatedby" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    Module
                </td>
                <td>
                    <eluc:Module ID="ucModule" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    Deploy on
                </td>
                <td>
                    <asp:Panel ID="pnlDeploymentServer" runat="server">
                        <asp:CheckBoxList ID="chklstDeploymentServer" Width="75%" CssClass="input_mandatory"
                            runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnTextChanged="chklstDeploymentServer_TextChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    File name
                </td>
                <td>
                    <asp:FileUpload runat="server" ID="filPatchAttachment" Width="50%" CssClass="input_mandatory" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
