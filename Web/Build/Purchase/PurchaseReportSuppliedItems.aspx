﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReportSuppliedItems.aspx.cs" Inherits="PurchaseReportSuppliedItems" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supplied Items Report</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

        <script type="text/javascript">
        function resizeFrame() {
                                var obj = document.getElementById("ifMoreInfo");
                                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
                               }
        </script>

    </div>
</telerik:RadCodeBlock></head>
 <body onload="resizeFrame()">
        <form id="frmReportSuppliedItems" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top;
            width: 100%">
            <div class="subHeader" style="position: relative">
                <eluc:Title runat="server" ID="Title3" Text="Supplied Items Report" ShowMenu="true">
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
                           <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true"
                                CssClass="input" />
                        </td>
                        <td>
                           <asp:Literal ID="lblSupplier" runat="server" Text="Supplier"></asp:Literal>
                            
                        </td>
                        <td>
                           <span id="spnPickListMaker">
                                <asp:TextBox ID="txtVendorNumber" runat="server" Width="60px" CssClass="input readonlytextbox" ></asp:TextBox>
                                <asp:TextBox ID="txtVenderName" runat="server" Width="180px" CssClass="input readonlytextbox" ></asp:TextBox>
                                <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131&framename=filterandsearch', true);"
                                    Text=".." />
                                <asp:TextBox ID="txtVendor" runat="server" Width="1px" CssClass="input"></asp:TextBox>
                            </span>
                        </td>
                    </tr>
                    <tr>
                    <td>
                           <asp:Literal ID="lblPartNumber" runat="server" Text="Part Number"></asp:Literal>
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtPartNumber" runat="server" CssClass="input"></asp:TextBox>
                    </td>
                     <td>
                           <asp:Literal ID="lblItemName" runat="server" Text="Item Name"></asp:Literal>
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtPartName" runat="server" CssClass="input"></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtDateFrom" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                           <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtDateTo" PopupPosition="TopLeft">
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
    </body>
</html>
