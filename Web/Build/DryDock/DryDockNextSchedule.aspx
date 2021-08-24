<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockNextSchedule.aspx.cs"
    Inherits="DryDockNextSchedule" %>

<%@ Import Namespace="SouthNests.Phoenix.PlannedMaintenance" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmNextSchedule" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader" style="position: relative">
                <div id="divHeading" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Schedule" ShowMenu="false"></eluc:Title>
                    <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
                </div>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuSchedule" runat="server" OnTabStripCommand="Schedule_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <br clear="all" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                       <asp:Literal ID="lblTitle" runat="server" Text="Title"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtTitle" MaxLength="200" Width="480px" CssClass="input"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Literal ID="lblLastDoneDate" runat="server" Text="Last Done Date"></asp:Literal>
                        
                    </td>
                    <td>
                        <eluc:Date ID="ucLastDoneDate" runat="server" CssClass="readonlytextbox" Enabled="false"/>
                    </td>
                    <td>
                       <asp:Literal ID="lblDueDate" runat="server" Text="Due Date"></asp:Literal>
                        
                    </td>
                    <td>
                        <eluc:Date ID="ucDueDate" runat="server" CssClass="input_mandatory"  DatePicker="true" />
                    </td>
                </tr>
                <tr>
                   
                    <td width="20%">
                       <asp:Literal ID="lblWindowPeriodindays" runat="server" Text="Window Period ( in days )"></asp:Literal>
                        
                    </td>
                    <td width="30%">
                        <eluc:Number ID="txtWindowperiod" runat="server" CssClass="input_mandatory" MaxLength="3"
                            IsPositive="true" Width="45px"></eluc:Number>
                        <eluc:Hard ID="ucwindowperiodtype" Visible="false" runat="server" AppendDataBoundItems="false"
                            CssClass="dropdown_mandatory" HardTypeCode="7" />
                    </td>
                    <td>
                       <asp:Literal ID="lblNoofalerts" runat="server" Text="No of alerts"></asp:Literal>
                    
                    </td>
                    <td>
                       <eluc:Number ID="txtnoofalerts" runat="server" CssClass="input_mandatory" MaxLength="3"
                            IsPositive="true" Width="45px"></eluc:Number>
                    </td>
                      <td>
                       <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        
                      </td>
                    <td>
                        <asp:DropDownList ID="ucStatus" runat="server" CssClass="input_mandatory" DataTextField="FLDDESCRIPTION"
                            DataValueField="FLDDRYDOCKSTATUS">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                            Height="60px" Width="320px"></asp:TextBox>
                    </td>
                </tr>
            </table>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
