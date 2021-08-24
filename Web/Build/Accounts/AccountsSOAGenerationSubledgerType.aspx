<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSOAGenerationSubledgerType.aspx.cs" Inherits="AccountsSOAGenerationSubledgerType" %>

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
    <title>SOA Generation Sub Ledger Type</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
                else
                {
                     __doPostBack("<%=confirmno.UniqueID %>", "");
                }
        }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDirectorComment" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <asp:Button ID="confirm" OnClick="CheckMapping_Click" runat="server" />
         <asp:Button ID="confirmno" OnClick="confirmno_Click" runat="server" />
        <eluc:TabStrip ID="MenuCommentsEdit" runat="server" OnTabStripCommand="MenuCommentsEdit_TabStripCommand"></eluc:TabStrip>

        <br />
        <div id="divFind" style="position: relative; z-index: 2">
            <table width="60%">
                <tr style="height: 30px" runat="server" visible="false">
                    <td width="10%">Sub Ledger Type
                    </td>
                    <td colspan="2" style="width: 60%">
                        <telerik:RadRadioButtonList ID="rblbtn1" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="50%" AutoPostBack="true">
                            <Items>
                                <telerik:ButtonListItem Text="Budget code" Value="1" Selected="False"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Owner Budget Code" Value="0" Selected="False"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                  
                </tr>
                <tr></tr>
            </table>
        </div>
        <%--<eluc:Confirm ID="ucConfirmMsg" runat="server" Visible="false" OnConfirmMesage="CheckMapping_Click"
            OKText="Yes" CancelText="No" />--%>
        <telerik:RadWindowManager runat="server" ID="ucConfirmMsg"   Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
    </form>
</body>
</html>
