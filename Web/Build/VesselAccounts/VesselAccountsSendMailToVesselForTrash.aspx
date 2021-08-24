<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsSendMailToVesselForTrash.aspx.cs" Inherits="VesselAccountsSendMailToVesselForTrash" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register TagPrefix="eluc" TagName="SEPModule" Src="~/UserControls/UserControlSEPbugModuleList.ascx" %>--%>
<%--<%@ Register TagPrefix="eluc" TagName="SEPIncident" Src="~/UserControls/UserControlSEPIncidentList.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Send Mail To Vessel To Trash Vessel Accounting In Vessel Side</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>

<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="subHeader">
            <div id="divHeading" class="divFloatLeft">
                <eluc:Title runat="server" ID="ucTitle" Text="Message" ShowMenu="false"></eluc:Title>
            </div>
            <div style="position: absolute; right: 0px">
                <eluc:TabStrip ID="MenuMailRead" runat="server" OnTabStripCommand="MenuMailRead_TabStripCommand"></eluc:TabStrip>
            </div>
            <eluc:Status runat="server" ID="ucStatus" />
        </div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table width="100%" border="1">
            <tr>
                <td width="40%">
                    <table width="100%">
                        <tr>
                            <td width="10%">
                                <asp:Literal ID="lblFrom" runat="server" Text="From"></asp:Literal>
                            </td>
                            <td width="90%">
                                <asp:TextBox ID="txtFrom" runat="server" ReadOnly="true" CssClass="input" Width="90%" Text="sep@southnests.com"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblTo" runat="server" Text="To"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTo" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCc" runat="server" Text="Cc"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCc" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSubject" runat="server" Text="Subject"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="input" Width="90%" Text="For Vessel Accounting Initialization."></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="40%"></td>
            </tr>
        </table>
        <table>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td>
                    <b>
                        <asp:Literal ID="lblAttachment" runat="server" Text="Attachment:"></asp:Literal></b>
                    <asp:Label ID="lblFileName" runat="server" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:LinkButton ID="lknfilename" runat="server" Text="View" OnClick="lknfilename_OnClick" Font-Bold="true"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td colspan="2" width="100%">
                    <eluc:Custom ID="txtFormDetails" runat="server" Width="100%" Height="475px" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true" PrevMode="true" OnFileUploadEvent="btnInsertPic_Click" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
