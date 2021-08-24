<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreCrewCommentConfirmMsg.aspx.cs"
    Inherits="CrewOffshoreCrewCommentConfirmMsg" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Comments</title>
    
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
        </head>
<body>
    <form id="form1" runat="server">
       <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
        <div id="dvmsg" runat="server" style="width: 1500px; height :500px; color: Blue; margin-top:20px;">
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
   
        <h1>
            <b>
                <telerik:RadLabel ID="lblmsg" runat="server" Text="Seafarer's Comment Updated Sucessfully for"
                    Font-Size="Large" ></telerik:RadLabel></b>
        </h1>
    </div>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
