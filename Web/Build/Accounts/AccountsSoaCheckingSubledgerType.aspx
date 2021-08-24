<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaCheckingSubledgerType.aspx.cs" Inherits="AccountsSoaCheckingSubledgerType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sub Ledger Type</title>
   <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

     
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmDirectorComment" runat="server">
     <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
  <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" Text="" />
   <%-- <div class="subHeader" style="position: relative">
        <div id="divHeading">
            <eluc:Title runat="server" ID="ucTitle" Text="SOA Checking Sub Ledger Type" ShowMenu="false"></eluc:Title>
        </div>
    </div>--%>
   
        <eluc:TabStrip ID="MenuCommentsEdit" runat="server" OnTabStripCommand="MenuCommentsEdit_TabStripCommand">
        </eluc:TabStrip>
    <br />
    <div id="divFind" style="position: relative; z-index: 2">
        <table width="60%" runat="server" visible="false">
            <tr style = "height: 30px">
            <td width="10%">
                    Sub Ledger Type
            </td>
            <td colspan="2" style="width: 60%">
                <asp:RadioButtonList ID="rblbtn1" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="50%" AutoPostBack="true"  >
                <asp:ListItem Text="Budget code" Value="1" Selected="False"></asp:ListItem>
                <asp:ListItem Text="Owner Budget Code" Value="0" Selected="False"></asp:ListItem>
                </asp:RadioButtonList>                       
            </td>
<%--                <td width="10%">
                    Accepted
                </td>
                <td style="width: 35%">
                    <asp:CheckBox ID="chkAccepted" runat="server" AutoPostBack="true" />
                </td>--%>                
            </tr>
            <tr></tr>
        </table>
    </div>
         <eluc:Confirm ID="ucConfirmMsg" runat="server" Visible="false" OnConfirmMesage="CheckMapping_Click"
                    OKText="Yes" CancelText="No" />
    </form>
</body>
</html>
