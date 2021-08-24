<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormDistributeVesselList.aspx.cs"
    Inherits="StandardFormDistributeVesselList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Distribute to vessel</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>                

  
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselMapping" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlVesselMapping">
        <ContentTemplate>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="div1" style="vertical-align: top">
                        <asp:Literal ID="lblVesselMapping" runat="server" Text="Distribute"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuVesselMapping" runat="server" OnTabStripCommand="MenuVesselMapping_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table >
                <tr>
                        <td>
                            <b>
                                <asp:Literal ID="lblFormName" runat="server" Text="Form"  ></asp:Literal></b>
                        
                            <asp:TextBox ID="txtFormName" runat="server" Width="400px" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                </tr>
                <tr><td>
                <div id="divVesselType" runat="server" class="input" onscroll="javascript:setScrollPosition('divVesselType','hdnVesselTypeScroll');"
                            style="overflow: auto; height: 170px;">
                            <asp:HiddenField ID="hdnVesselTypeScroll" runat="server" />
                            <telerik:RadCheckBox ID="chkVesselTypeAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkVesselTypeAll_Changed"
                                Text="---SELECT ALL---" />
                            <telerik:RadCheckBoxList ID="chkVesselType" Width="500px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkVesselType_Changed">
                            </telerik:RadCheckBoxList>
                        </div>
                </td>
                </tr>
                <tr>
                <td>
                <div id="divVessel" runat="server" class="input" onscroll="javascript:setScrollPosition('divVessel','hdnVesselScroll');"
                            style="overflow: auto; height: 170px">
                            <asp:HiddenField ID="hdnVesselScroll" runat="server" />
                            <telerik:RadCheckBoxList ID="cblVessel" Width="500px" runat="server">
                            </telerik:RadCheckBoxList>
                        </div>
                </td>
                </tr>
                </table>
               <%-- <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <b>
                                <asp:Literal ID="lblFormName" runat="server" Text="Form"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFormName" runat="server" Width="400px" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkCheckAll" runat="server" Text="Check All" AutoPostBack="true"
                                OnCheckedChanged="SelectAll" />
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:CheckBoxList ID="cblVessel" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                        RepeatColumns="5">
                    </asp:CheckBoxList>
                </div>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
