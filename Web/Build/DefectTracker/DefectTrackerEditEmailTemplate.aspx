<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerEditEmailTemplate.aspx.cs"
    Inherits="DefectTracker_DefectTrackerEditEmailTemplate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Module" Src="~/UserControls/UserControlSEPModuleList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Email Template</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div style="position: absolute; right: 0px">
            <eluc:TabStrip ID="MenuBugAttachment" runat="server" OnTabStripCommand="MenuSaveTemplate_TabStripCommand">
            </eluc:TabStrip>
        </div>
    </div>   
    <div>
        <table width="100%" border="1">
            <tr>
                <td width="40%">
                    <table width="100%">
                        <tr>
                            <td width="10%">
                                Cc
                            </td>
                            <td>
                                <asp:TextBox ID="txtCc" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Subject
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="10%">
                                IncidentCode
                            </td>
                            <td width="90%">
                                <asp:TextBox ID="txtTemplateId" runat="server" Visible="false" />
                                <asp:TextBox ID="txtIncidentCode" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                            </td>
                            <td>
                                Module
                            </td>
                            <td>
                                <eluc:Module ID="ucModule" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="40%">
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td width="15%">
                    <font color="blue" size="0"><b>Attachments</b>
                        <li>Browse and select the attachment </li>
                    </font>
                </td>
                <td width="35%">
                    <asp:FileUpload runat="server" ID="TemplateAttachment" CssClass="input" Width="87.5%" />
                </td>
                <td width="10%">
                    Description
                </td>
                <td width="40%">
                    <asp:TextBox ID="txtDescription" CssClass="input" runat="server" Width="90%" />
                </td>
            </tr>
        </table>
        <table border="1" width="100%">
            <tr>
                <td valign="top" width="8%">
                    Mail Body
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <asp:TextBox ID="txtEmailBody" runat="server" TextMode="MultiLine" MaxLength="500"
                        CssClass="input" Width="100%" Height="260px" Font-Size="Small"></asp:TextBox>
                </td>
            </tr>
        </table>
        <eluc:Status runat="server" ID="ucStatus" />
    </div>
    </form>
</body>
</html>
