<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersPurchaseFilter.aspx.cs" Inherits="OwnersPurchaseFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOwnersVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Purchase Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
   <form id="frmStockItemFilter" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDiscussion">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader" style="position: relative">
                <div id="div2" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Purchase Form Filter" ShowMenu="True">
                    </eluc:Title>
                </div>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuFormFilter" runat="server" OnTabStripCommand="MenuFormFilter_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <br />
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                border: none; width: 100%">
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblFormNo" runat="server" Text="Form No"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFormNumber" runat="server" Width="90px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lbloFormType" runat="server" Text="Form Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucHard" runat="server" AppendDataBoundItems="true" HardTypeCode="20" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblCreatedDate" runat="server" Text="Created Date"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreatedDate" runat="server" Width="90px" CssClass="input"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtCreatedDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                            -
                            <asp:TextBox ID="txtCreatedToDate" runat="server" CssClass="input"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtCreatedToDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFormStatus" runat="server" Text="Form Status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucFormStatus" AppendDataBoundItems="true" CssClass="input" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
