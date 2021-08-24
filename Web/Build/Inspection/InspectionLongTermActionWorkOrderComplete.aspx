<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionLongTermActionWorkOrderComplete.aspx.cs" Inherits="InspectionLongTermActionWorkOrderComplete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Complete</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat ="server" >
     <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmWorkOrderCompleteDate" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolScriptManager1" runat ="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat ="server" Text="" Visible ="false" />
    <eluc:Status ID="ucStatus" runat ="server" />
    <br />

        <table cellpadding ="1" cellspacing ="1" width ="50%">
        <tr>
            <td colspan ="2">
            <B><asp:Literal ID="lblEnterCompletionDate" runat="server" Text="Enter Completion Date"></asp:Literal></B>
            </td>
        </tr>
         <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
        <td>
            <asp:Literal ID="lblWorkOrderNo" runat="server" Text="Work Order Number"></asp:Literal>
        </td>
        <td>
            <asp:TextBox ID="txtWorkOrderNumer" runat ="server" CssClass="readonlytextbox" ReadOnly ="true" ></asp:TextBox>
        </td>
        </tr>
        <tr>
            <td>
                <asp:literal ID="lblCompletionDate" runat="server" Text="Completion Date"></asp:literal>
            </td>
            <td>
            <eluc:Date ID="txtCompletionDate" runat ="server" CssClass="input_mandatory" DatePicker ="true" />
            </td>
        </tr>
         <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat ="server" ForeColor="Red" Font-Bold ="true" Font-Size ="Small" ></asp:Label>
            </td>
        </tr>
        <tr>
        <td colspan="2">
        <asp:Label ID="lblActionTaken"  runat="server" Visible ="false" ></asp:Label>
        <asp:Label ID="lblWODescription" runat ="server" Visible ="false" ></asp:Label>
        <asp:Label ID="lblWOStatus" runat ="server" Visible ="false" ></asp:Label>
        </td>
        </tr>
        </table>
        

    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="MenuReport_TabStripCommand">
        </eluc:TabStrip>
    </div>
    </form>
</body>
</html>
