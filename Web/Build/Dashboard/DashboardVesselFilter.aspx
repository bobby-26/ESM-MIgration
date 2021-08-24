<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardVesselFilter.aspx.cs"
    Inherits="DashboardVesselFilter" %>
    
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlRankList" Src="../UserControls/UserControlRankList.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text=""></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
        <ContentTemplate>
            <div id="divFind">
                <table>
                    <tr>
                        <td valign="top">
                            <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                        </td>
                        <td>
                            <div id="divFleet" runat="server" class="input" style="overflow: auto; width: 150px;
                                height: 80px">
                                <asp:CheckBoxList ID="chkFleetList" runat="server" AutoPostBack="true" Height="100%"
                                    OnSelectedIndexChanged="chkFleetList_Changed" RepeatColumns="1" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td valign="top">
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCriteria" CssClass="input" runat="server">
                                <asp:ListItem Value="LIKE" Text="Like"></asp:ListItem>
                                <asp:ListItem Value="START" Text="Starts With"></asp:ListItem>
                                <asp:ListItem Value="END" Text="Ends With"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox runat="server" ID="txtFilter" CssClass="input" Width="50px"></asp:TextBox>
                            <asp:Button runat="server" ID="cmdSelect" Text="V" CssClass="input" OnClick="cmdSelect_Click" />
                            <asp:Button runat="server" ID="cmdClear" Text="X" CssClass="input" OnClick="cmdClear_Click" />
                            <div id="divVessel" class="input" style="height: 90px; width: 400px; overflow-y: auto">
                                <asp:CheckBoxList runat="server" ID="chkVesselList" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" />
                                &nbsp;
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
