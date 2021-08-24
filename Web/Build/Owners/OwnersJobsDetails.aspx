<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersJobsDetails.aspx.cs"
    Inherits="Owners_OwnersJobsDetails" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Literal ID="lblJobDetails" runat="server" Text="Job Details"></asp:Literal>
        </div>
    </div>
    <div>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td width="10%">
                    <asp:Literal ID="lblJobCode" runat="server" Text="Job Code"></asp:Literal>
                </td>
                <td width="63%">
                    <asp:TextBox runat="server" ID="txtJobCode" MaxLength="50" Width="180px" CssClass="input" ReadOnly ="true" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblJobTitle" runat="server" Text="Job Title"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtJobTitle" MaxLength="50" Width="360px" CssClass="input" ReadOnly ="true"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <br />
        
        <table width="100%">
        <tr>
        <td>
        <asp:Label ID="lblJobdescription" runat="server" Text='Job Description' Font-Size ="Small"  ></asp:Label>
        </td>
        </tr>
            <tr>
                <td>
                    <eluc:Custom ID="txtJobDetail" runat="server" Width="90%" Height="260px" PictureButton="false" 
                        DesgMode="true" HTMLMode="false" PrevMode="true" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
