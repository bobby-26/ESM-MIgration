<%@ Page Language="C#" AutoEventWireup="true" Inherits="StandardFormSimpleJobs" CodeFile="StandardFormSimpleJobs.aspx.cs" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>StandardForm Simple Job</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
    <script language="javascript" type="text/javascript">
        function cmdPrint_Click()
        {            
            document.getElementById('cmdPrint').style.visibility = "hidden";
            window.print();
        }
    </script>
    <style type="text/css">
        .tblclass
        {
            border-collapse: collapse;
        }
        .tblclass tr td
        {
            border: 1px solid black;
        }
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAuxEngine" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Simple Jobs" ShowMenu="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>       
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
            <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
                <table cellpadding="1" cellspacing="1">                    
                    <tr>
                        <td>
                           <asp:Literal ID="lblComponent" runat="server" Text="Component"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtComponentNumber" runat="server" Enabled="false"  CssClass="input readonlytextbox"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtComponentName" runat="server" Enabled="false"  CssClass="input readonlytextbox" Width="200px"></asp:TextBox>
                        </td>                       
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDateofInspection" runat="server" Text="Date of Inspection"></asp:Literal>
                        </td>
                        <td>                           
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                        <td>
                           <asp:Literal ID="lblLastInspection" runat="server" Text="Last Inspection"></asp:Literal>
                        </td>
                         <td>
                            <eluc:Date ID="txtLastInspection" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRunningHours" runat="server" Text="Running Hours (if counter based job)"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtRunnigHrs" runat="server" CssClass="input" IsInteger="true"/>
                        </td>                      
                        <td>
                             <eluc:Number ID="txtLastRunnigHrs" runat="server" CssClass="input" IsInteger="true"/>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%" class="tblclass" cellpadding="1" cellspacing="1">                   
                    <tr>
                        <td width="20%">
                           <asp:Literal ID="lblDisassembled" runat="server" Text="Disassembled"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblDisassembled" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCleaned" runat="server" Text="Cleaned"></asp:Literal>                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblCleaned" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr> 
                    <tr>
                        <td>
                            <asp:Literal ID="lblInspected" runat="server" Text="Inspected"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblInspected" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr> 
                    <tr>
                        <td>
                            <asp:Literal ID="lblTested" runat="server" Text="Tested"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblTested" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:literal ID="lblConditionofComponent" runat="server" Text="Condition of component"></asp:literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblComponentCondition" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                             AutoPostBack="true" OnSelectedIndexChanged="rblComments_SelectedIndexChanged">
                                <asp:ListItem Text="Good" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Satisfactory" Value="2"></asp:ListItem>
                                 <asp:ListItem Text="Poor" Value="3"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblComments" runat="server" Text="Comments"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblComments" runat="server" RepeatLayout="Flow"  RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblComments_SelectedIndexChanged">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>                        
                    </tr>
                    <tr>
                        <td colspan="2">                            
                            <asp:TextBox ID="txtComments" runat="server" Enabled="false"  Width="400px" Height="60px"
                            CssClass="input readonlytextbox" TextMode="MultiLine"></asp:TextBox>                        
                        </td>
                    </tr>                   
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
