<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFormPreview.aspx.cs" Inherits="DocumentManagementFormPreview" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form Field</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        
        <script>
            function getUploadedFile(ifile, imgid) {
                var img = document.getElementById(imgid);
                var fullPath = ifile.value;
                if (fullPath) {
                    var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
                    var filename = fullPath.substring(startIndex);
                    if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                        filename = filename.substring(1);
                    }
                    img.src = filename;
                    alert(fullPath + filename);                    
                }
            }
        </script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form runat="server" id="form1">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInvoice">
        <ContentTemplate>
            <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: text-top;
                height: auto; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="ttlContent" Text="Form Preview" ShowMenu="false">
                        </eluc:Title>
                    </div>
                </div>                
                <br />
                <div id="divForm" runat="server" style="word-wrap: break-word; padding-left: 5%;
                    width: 90%; ">
                    <table id="tblForm" runat="server" style="word-wrap: break-word; border: 0.5px solid;
                        table-layout: fixed;" width="100%">
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
