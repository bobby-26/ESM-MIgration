<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDirectory.aspx.cs" Inherits="DocumentManagementDirectory" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document Directory</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script language="Javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }       
    </script>

</telerik:RadCodeBlock></head>
<form id="frmDirectory" runat="server">
<ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
    runat="server">
</ajaxToolkit:ToolkitScriptManager>
<eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucTitle" Text="Directory"></eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuDirectory" runat="server" OnTabStripCommand="MenuDirectory_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <div style="overflow: scroll; width: 30%; float: left; height: 450px;" id="divDocumentCategory">
        <table style="float: left; width: 100%;">
            <tr style="position: absolute">
                <eluc:TreeView runat="server" ID="tvwDirectory" OnSelectNodeEvent="ucTree_SelectNodeEvent">
                </eluc:TreeView>
                <asp:label runat="server" id="lblSelectedNode"></asp:label>
                <asp:label id="lblDirectoryId" runat="server"></asp:label>
            </tr>
        </table>
    </div>
    <eluc:VerticalSplit runat="server" ID="ucVerticalSplit" TargetControlID="divDocumentCategory" />
    <div style="position: relative; float: left; margin: 5px; width: auto">
        <table width="100%" cellpadding="5">
            <tr>
                <td>
                    Directory Name
                </td>
                <td>
                    <asp:textbox id="txtDirectoryName" cssclass="input_mandatory" width="300" runat="server"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td>
                    Directory Number
                </td>
                <td>
                    <asp:textbox id="txtDirectoryNumber" runat="server" cssclass="gridinput_mandatory"
                        onkeypress="return isNumberKey(event)" width="90px"></asp:textbox>
                </td>
            </tr>
        </table>
    </div>
</div>
</form>
</html>
