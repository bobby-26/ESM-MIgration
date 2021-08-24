<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentSubSectionAdd.aspx.cs"
    Inherits="DocumentManagementDocumentSubSectionAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sub Section Add</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }

            function SetParentHiddenKey(keyvalue) {
                var o;

                if (parent.document.getElementById('filterandsearch') != null)
                    o = parent.document.getElementById('filterandsearch');

                if (parent.document.getElementById('fraPhoenixApplication') != null)
                    o = parent.document.getElementById('fraPhoenixApplication').contentDocument.getElementById('filterandsearch');

                //                if (o.contentWindow.document.getElementById(refreshiframe) == null)
                //                    refreshiframe = null;

                //                if (refreshiframe == null)
                //                    key = o.contentDocument.getElementById('hiddenkey');
                //                else
                //                    key = o.contentWindow.document.getElementById(refreshiframe).contentDocument.getElementById('hiddenkey');

                o.contentDocument.getElementById('hiddenkey').value = keyvalue;
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" >
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="MenuLocation_TabStripCommand"></eluc:TabStrip>
        <br />
        <table cellpadding="1" cellspacing="1" width="75%">
            <tr>
                <td>Name
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSectionNameAdd" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>Active YN
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server" Checked="true" />
                    <telerik:RadTextBox ID="txtRevisionnumberAdd" runat="server" CssClass="input" Width="90px"
                        Visible="false" onkeypress="return isNumberKey(event)"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></telerik:RadLabel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
