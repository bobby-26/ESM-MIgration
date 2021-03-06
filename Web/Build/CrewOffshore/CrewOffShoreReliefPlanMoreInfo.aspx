<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffShoreReliefPlanMoreInfo.aspx.cs"
    Inherits="CrewOffShoreReliefPlanMoreInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Editor" Src="~/UserControls/UserControlEditor.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <form id="frmComment" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <%--<asp:UpdatePanel runat="server" ID="pnlFormDetails">
        <ContentTemplate>--%>
         <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
            <div class="subHeader" style="position: relative">
                <div id="divHeading" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Remarks" ShowMenu="false"></eluc:Title>
                </div>
                 <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                 <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuComment" runat="server" OnTabStripCommand="MenuComment_TabStripCommand">
                </eluc:TabStrip>
            </div> 
             <eluc:Editor ID="txtComment" runat="server" Width="100%" Height="100%" 
                            DesgMode="true"  />       
              <asp:TextBox id="txtAreaComment" TextMode="MultiLine" runat="server" Visible="false" Height="100%" Width="100%" ></asp:TextBox>
            <%--<ajaxToolkit:Editor ID="txtComment" runat="server" Width="100%" Height="90%" ActiveMode="Design" />    --%>       
            </div>
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
   
    </form>
</body>
</html>
