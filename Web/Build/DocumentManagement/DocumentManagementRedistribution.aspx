<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementRedistribution.aspx.cs" Inherits="DocumentManagementRedistribution" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DMS Redistribution</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRedistribution" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlRedistribution">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Redistribution" ShowMenu="false"></eluc:Title>
                    <eluc:Status ID="ucStatus" runat="server" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuRedistribution" runat="server" OnTabStripCommand="MenuRedistribution_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table width="100%">
                    <tr>
                        <td>
                            Vessel List
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessellist" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            Mapped Vessel Type
                        </td>
                        <td>
                            <eluc:Number ID="ucMappedVesselTypeid" runat="server" CssClass="input_mandatory" MaxLength="3"
                                IsPositive="true"></eluc:Number>
                        </td>
                    </tr>--%>
                    <tr visible="false">
                        <td colspan="2">
                            <br />
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Seal Nos
                        </td>
                        <td>
                            <asp:TextBox ID="txtSealReqNo" Visible="false" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            <asp:TextBox ID="txtSealNo" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Status
                        </td>
                        <td>
                            <asp:TextBox ID="txtStatus" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
