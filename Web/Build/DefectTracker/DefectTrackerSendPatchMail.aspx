<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerSendPatchMail.aspx.cs"
    Inherits="DefectTracker_DefectTrackerSendPatchMail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="SEPModule" Src="~/UserControls/UserControlSEPbugModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPIncident" Src="~/UserControls/UserControlSEPIncidentList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register src="../UserControls/UserControlMultiColumnCity.ascx" tagname="UserControlMultiColumnCity" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MailManager</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div id="divHeading" class="divFloatLeft">
            <eluc:Title runat="server" ID="ucTitle" Text="Send Patch" ShowMenu="false"></eluc:Title>
        </div>
        <div style="position: absolute; right: 0px">
            <eluc:TabStrip ID="MenuNewMail" runat="server" OnTabStripCommand="MenuNewMail_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table width="100%" border="1">
            <tr>
                <td width="45%">
                    <table width="100%">
                        <tr>
                            <td>
                                To
                            </td>
                            <td>
                                <asp:TextBox ID="txtTo" runat="server" CssClass="input" Width="90%"></asp:TextBox>                                 
                            </td>
                        </tr>
                        <tr>
                            <td>
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
                    </table>
                </td>
                <td width="55%">
                    <table width="100%">
                       <tr>
                            <td>                              
                               <eluc:SEPVessel ID="UcVessel" runat="server" CssClass="input" AutoPostBack="true"
                                    OnTextChangedEvent="VesselTo_Changed" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" runat="server" Width="14px" 
                                        Height="14px" ToolTip="Download File">
                                    </asp:HyperLink>

                            </td>
                        </tr>
                        <tr>
                            <td>
                              <br />
                              
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td colspan="2" width="100%">
                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="20" Width="100%"
                        CssClass="input" Font-Size="Small" onkeydown="HandleKeyDown(this, event)" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
