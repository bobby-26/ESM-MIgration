<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReportInvNotReceived.aspx.cs"
    Inherits="PurchaseReportInvNotReceived" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice Not Received for PO</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                    <eluc:Title runat="server" ID="Title1" Text="Po Without Ivoice" ShowMenu="True">
                    </eluc:Title>
                </div>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                          <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                        <td>
                          <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                            
                        </td>
                        <td>
                            <div id="divFleet" runat="server" class="input" style="overflow: auto; width: 60%;
                                height: 80px">
                                <asp:CheckBoxList ID="chkFleetList" runat="server" AutoPostBack="true" Height="100%"
                                    OnSelectedIndexChanged="chkFleetList_Changed" RepeatColumns="1" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td>
                          <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            
                        </td>
                        <td>
                            <div id="dvVessel" runat="server" class="input" style="overflow: auto;height: 80px">
                                <asp:CheckBoxList ID="chkVesselList" runat="server" Height="100%" AutoPostBack="true"
                                    OnSelectedIndexChanged="chkVesselList_Changed" RepeatColumns="1" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <asp:Literal ID="lblPurchaser" runat="server" Text="Purchaser"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPurchaserList" runat="server" CssClass="input" Width="100px">
                            </asp:DropDownList>
                        </td>
                        <td>
                          <asp:Literal ID="lblSuperintendent" runat="server" Text="Superintendent"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSupdtList" runat="server" CssClass="input" Width="100px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                            
                        </td>
                        <td>
                            <span id="spnPickListMaker">
                                <asp:TextBox ID="txtVendorCode" runat="server" Width="60px" CssClass="input"></asp:TextBox>
                                <asp:TextBox ID="txtVendorName" runat="server" BorderWidth="1px" Width="210px" CssClass="input"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,132&ignoreiframe=true', true);"
                                    Text=".." />
                                <asp:TextBox ID="txtVendorId" runat="server" Width="1" CssClass="input"></asp:TextBox>
                            </span>
                        </td>
                        <td>
                          <asp:Literal ID="lblPONumber" runat="server" Text="PO Number"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtPONumber" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <asp:Literal ID="lblPOAmount" runat="server" Text="PO Amount"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:Number ID="ucPoAmount" runat="server" CssClass="input" DecimalPlace="2" />
                        </td>
                        <td>
                          <asp:Literal ID="lblNoofDays" runat="server" Text="No of Days"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlNoOfDays" runat="server" CssClass="input">
                            <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Days<=10"></asp:ListItem>
                            <asp:ListItem Value="2" Text="10<Days"></asp:ListItem>
                            <asp:ListItem Value="3" Text="20<Days"></asp:ListItem>
                            <asp:ListItem Value="4" Text="30<Days"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
