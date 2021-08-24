<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCDISIREMatrixOnboardChecks.aspx.cs" Inherits="Inspection_InspectionCDISIREMatrixOnboardChecks" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Onboard/Office Checks</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDirectorComment" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <eluc:TabStrip ID="MenuCommentsEdit" runat="server" OnTabStripCommand="MenuCommentsEdit_TabStripCommand"></eluc:TabStrip>
        <br />
        <table width="100%">
            <tr>
                <td ><b>Onboard Verification</b>
                </td>
                <td></td>
                <td>
                    <b>Office Verification</b>
                </td>
            </tr>
            <tr >
                <td >Onboard Checks
                </td>
                <td >
                    <asp:RadioButtonList ID="rblbtn1" runat="server" RepeatDirection="Vertical" RepeatLayout="Table" Width="100px" AutoPostBack="true">
                        <asp:ListItem Text="Yes" Value="1" Selected="False"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>
                        <asp:ListItem Text="NA" Value="2" Selected="False"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td >Office Checks
                </td>
                <td >
                    <asp:RadioButtonList ID="rblofficechecks" runat="server" RepeatDirection="Vertical" RepeatLayout="Table" Width="200px" AutoPostBack="true">
                        <asp:ListItem Text="NA" Value="3" Selected="False"></asp:ListItem>  
                        <asp:ListItem Text="Office Check Req" Value="2" Selected="False"></asp:ListItem>                       
                        <asp:ListItem Text="Accepted" Value="1" Selected="False"></asp:ListItem>
                        <asp:ListItem Text="Not Accepted" Value="0" Selected="False"></asp:ListItem>     
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trevidence" runat="server">
                <td>
                    Upload Evidence
                </td>
                <td>
                    <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgEvidence"
                        runat="server" ToolTip="Upload Evidence" />
                </td>
            </tr>
            <tr></tr>
            <tr>
                <td>Onboard Remarks
                </td>
                <td align="left" style="vertical-align: top;">
                    <telerik:RadTextBox ID="txtOnboardRemarks" runat="server" CssClass="gridinput" Height="100px"
                        TextMode="MultiLine" Width="400px">
                    </telerik:RadTextBox>
                </td>
               <td>Office Remarks
                </td>
                <td align="left" style="vertical-align: top;">
                    <telerik:RadTextBox ID="txtOfficeRemarks" runat="server" CssClass="gridinput" Height="100px"
                        TextMode="MultiLine" Width="400px">
                    </telerik:RadTextBox>
                </td>
            </tr>                        
            <tr>       
                <td >
                    Done By
                </td>
                <td >
                        <telerik:RadTextBox ID="txtOnboardDoneBy" runat="server" Width="185px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true">
                        </telerik:RadTextBox>
                </td>                        
                <td >
                    Done By
                </td>
                <td >
                        <telerik:RadTextBox ID="txtOfficeDoneBy" runat="server" Width="185px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true">
                        </telerik:RadTextBox>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
