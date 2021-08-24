<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetExpenseProposalReport.aspx.cs" Inherits="OwnerBudgetExpenseProposalReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>        

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form runat="server" id="form1">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInvoice">
        <ContentTemplate>
            <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: text-top;
                height: auto; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="ttlContent" Text="Expense Report" ShowMenu="true"></eluc:Title>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                     <eluc:TabStrip ID="MenuClose" runat="server" OnTabStripCommand="MenuClose_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
                </div>
               <%-- <table align ="center" title ="PROPOSED OPERATING COST" visible ="true"><tr><td><font face="BernhardMod BT" size ="5">Proposed Operating Cost</font></td></tr></table>               --%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <br />
    <br />
    <span id="span2" runat="server" title="View"  style="display: inline-block;margin-left: 20px;width: 780px;"></span>
    
</body>
</html>
