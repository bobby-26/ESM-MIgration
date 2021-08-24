<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListDocumentManagementTreeForAllNodes.aspx.cs" Inherits="Common_CommonPickListDocumentManagementTreeForAllNodes" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeView.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/sitemapstyler.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.4.2.min.js"></script>

    <script type="text/javascript">
        function resizeFrame(obj) {
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 30 + "px";
        }
        function SetSourceURL(type, name, selectednode) {

                if (navigator.userAgent.indexOf("MSIE") != -1) {
                    var e = window.event;
                    if (e.srcElement)
                        name = e.srcElement.innerHTML;
                }
                else {
                    name = arguments.callee.caller.arguments[0].target.innerHTML;
                }

                var args = "function=SetCurrentDMSPickListSelection|selectednode=" + selectednode + "|name=" + name;
                AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);
                fnClosePickList('codehelp1', 'ifMoreInfo');
            }        

        function PrepareTree(parentli) {
            if (parentli == null) parentli = "sitemap";
            var sitemap = document.getElementById(parentli);
            if (sitemap) {

                this.listItem = function (li) {
                    if (li.getElementsByTagName("ul").length > 0) {
                        var ul = li.getElementsByTagName("ul")[0];
                        ul.style.display = "none";
                        var span = document.createElement("span");
                        span.className = "collapsed";
                        span.onclick = function () {
                            ul.style.display = (ul.style.display == "none") ? "block" : "none";
                            this.className = (ul.style.display == "none") ? "collapsed" : "expanded";

                            // To populate the tree on demand

                            if (span.className == "expanded" && li.value != 0) {
                                var status = $('#' + li.id).data("populated");

                                if (status == null)
                                    PopulateTreeOnDemand(li.id, ul.id);
                            }
                        };
                        li.appendChild(span);
                    };
                };

                var items = sitemap.getElementsByTagName("li");
                for (var i = 0; i < items.length; i++) {
                    listItem(items[i]);
                };

            };
        };

        function ExpandFirstLevel() {
            var firsttag = document.getElementById('ul1');
            if (firsttag != null) firsttag.style.display = "block";

            var parentlist = document.getElementById("parentlist");
            if (parentlist != null && parentlist.getElementsByTagName("span").length > 0) {
                var span = parentlist.getElementsByTagName("span")[parentlist.getElementsByTagName("span").length - 1];
                span.className = "expanded";
            }
        }

        function PopulateTreeOnDemand(listid, ulid) {
            var li = document.getElementById(listid);
            var ul = document.getElementById(ulid);

            // To get the content for the tree node

            var args = "function=PopulateDMSTreeOnDemand|selectednode=" + li.id + "|type=" + li.value;
            var arrValue = AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);
            if (arrValue != "")
                $('#' + ul.id).append(arrValue);
            $('#' + li.id).data('populated', '1');

            PrepareTree(li.id);
        }     
    </script>

    <style>
        body
        {
            margin: 0;
            padding: 0; /* background: #f1f1f1; */
            font: 70% Arial, Helvetica, sans-serif;
            color: #555;
            line-height: 150%;
            text-align: left;
        }
        a
        {
            text-decoration: none;
            color: Black;
        }
        a:hover
        {
            text-decoration: none;
            color: Red;
        }
        .select
        {
            font-weight: bold;
            background: #F96611;
            border: 3px solid #FFB380;
        }
        h1
        {
            font-size: 140%;
            margin: 0 20px;
            line-height: 80px;
        }
        #container
        {
            margin: 0 auto;
            width: 680px;
            background: #fff;
            padding-bottom: 20px;
        }
        #content
        {
            margin: 0 20px;
        }
        p
        {
            margin: 0 auto;
            width: 680px;
            padding: 1em 0;
        }
        .style1
        {
            width: 75px;
        }
    </style>
</telerik:RadCodeBlock></head>
<body onload="ExpandFirstLevel();" onresize="resizeFrame(document.getElementById('divDocumentCategory'));">
    <form id="frmDocumentManagement" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div style="position: fixed; top: 0px; margin-left: auto; margin-right: auto; vertical-align: middle;
        width: 100%;" id="topdiv">
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="Documents" ShowMenu="false"></eluc:Title>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuDocument" runat="server" OnTabStripCommand="MenuDocument_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div style="overflow: auto; height: 682px; width: 500px; float: left; clear: right;
            z-index: +2; position: relative;" id="divDocumentCategory" runat="server">
            <br />
            <table cellpadding="0" cellspacing="0" style="float: left; width: 100%;">
                <tr>
                    <td>
                        <div id="container" runat="server">
                            <div id="content" runat="server">
                                <ul id="sitemap" runat="server">
                                </ul>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
