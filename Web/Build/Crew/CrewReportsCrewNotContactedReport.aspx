<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsCrewNotContactedReport.aspx.cs" Inherits="Crew_ReportsCrewNotContactedReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip"  Src="~/UserControls/UserControlTabs.ascx"  %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error"  Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc"  TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx"%>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Not Contacted Report</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>                
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>
   </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager> 
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false">  </eluc:Error>
        <eluc:UserControlStatus ID="ucStatus" runat="server" />           
        <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top;
                    width: 100%">
        <div class="subHeader" style="position: relative">
             <eluc:Title runat="server" ID="Title3" Text="Report Filter" ShowMenu="true"></eluc:Title>
             <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
             <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand" TabStrip ="false">
             </eluc:TabStrip>
        </div>
        <div>
            <table width="100%" class="style4">
            <tr>
                <td>
                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                </td>
                <td>
                <eluc:Rank ID="ucRank1" runat="server" AppendDataBoundItems="true" CssClass="input"  />
                </td> 
                <td>
                <asp:Literal ID="lblZone" runat="server" Text="Zone"></asp:Literal>
                </td>
                <td>
                <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" CssClass="input"  />
                </td>
            </tr>
         </table>
         <div>
        <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 500px; width: 100%; " >
                </iframe>
        </div>
    
    </div>
    </div>
    </form>
</body>
</html>
