<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalSeafarerPlan.aspx.cs" Inherits="Portal_PortalSeafarerPlan" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/UserControls/UserControlErrorMessage.ascx" TagPrefix="eluc" TagName="Error" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />
        <script type="text/javascript">
            function Onclicktab(id) {
                //Get the Button reference and trigger the click event  
               
                if (id == 2) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("PLAN"); }, 100);
                 }
                 if (id == 3) {
                     setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("LETTEROFINTENT"); }, 100);
                 }
                 if (id == 4) {
                     setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("OFFERLETTER"); }, 100);
                 }
                 if (id == 5) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("HOME"); }, 100);
                }
            }
            function resize() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 38 + "px";
            }
            function pageLoad() {
                resize();
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body onload="resize();" onresize="resize();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ifMoreInfo">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="gray-bg">
            <div class="row">
                <div class="col-lg-12">
                    <div class="tabs-container">
                        <ul class="nav nav-tabs">
                           
                            <li class="" id="idpersonnelmaster" runat="server">
                                <a data-toggle="tab" onclick="return Onclicktab(2);" href="#tab-2">Plan</a>
                            </li>
                            <li class="" id="idtravel" runat="server">
                                <a data-toggle="tab" onclick="return Onclicktab(3);" href="#tab-3">Letter of Intent</a>
                            </li>
                            <li class="" id="Li1" runat="server">
                                <a data-toggle="tab" onclick="return Onclicktab(4);" href="#tab-4">Offer Letter</a>
                            </li>
                             <li class="" id="Home" runat="server">
                                <a data-toggle="tab" onclick="return Onclicktab(5);">Home</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3" class="tab-pane">
            <iframe runat="server" id="ifMoreInfo" frameborder="0" style="width: 100%; height: 100%"></iframe>
        </div>
    </form>
</body>
</html>
