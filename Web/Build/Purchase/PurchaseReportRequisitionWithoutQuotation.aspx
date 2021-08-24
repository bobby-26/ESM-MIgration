<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReportRequisitionWithoutQuotation.aspx.cs"
    Inherits="PurchaseReportRequisitionWithoutQuotation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Requisition Without Quotation</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <div>
        <form id="frmRequisitionWithoutQuotation" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top;
            width: 100%">
            <div class="subHeader" style="position: relative">
                <eluc:Title runat="server" ID="Title1" Text="Requisition Without Quotation" ShowMenu="True">
                </eluc:Title>
                <asp:Button runat="server" ID="cmdHiddenSubmit" />
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                    TabStrip="false"></eluc:TabStrip>
            </div>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                          <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="ucFromDate" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="ucFromDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                          <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="ucToDate" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="ucToDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px;
                    width: 100%;"></iframe>
            </div>
        </div>
        </form>
    </div>
</body>
</html>
