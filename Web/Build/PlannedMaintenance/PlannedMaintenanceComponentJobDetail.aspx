<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJobDetail.aspx.cs"
    Inherits="PlannedMaintenanceComponentJobDetail" %>

<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job Description</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resize() {
                var $ = $telerik.$;
                var height = $(window).height();    
                height = (height - 15);
                var frame = document.querySelector('iframe:not([style*="display:none"]):not([style*="display: none"])');
                frame.style.height = height + "px";   
                var table = document.querySelector('#txtComponentDetail_ucCustomEditor table');
                table.style.height = height + "px";   
                if (parent != null) {
                    var parentframe = parent.document.getElementById("ifMoreInfo");
                    if (parentframe != null) {
                        var td = parentframe.parentElement;
                        parentframe.style.height = td.style.height;
                    }
                }
            }
            window.onresize = window.onload = resize;
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPlannedMaintenanceComponentDetail" runat="server" autocomplete="off">
        <%--       <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />--%>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--   <div class="subHeader" style="position: relative">
                <div id="div2" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Job Description" ShowMenu="false"></eluc:Title>
                </div>
            </div>--%>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <eluc:Custom ID="txtComponentDetail" runat="server" Width="100%" PictureButton="false"
                                DesgMode="false" HTMLMode="false" PrevMode="false" />
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:Status runat="server" ID="ucStatus" />
        </div>
    </form>
</body>
</html>
