<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentNew.aspx.cs" Inherits="DocumentManagement_DocumentManagementDocumentNew" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentTreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/sitemapstyler.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.4.2.min.js"></script>
    
    <script type="text/javascript">
        function resizeFrame(obj) {
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 30 + "px";
        }

<%--        if (document.getElementById('<%=ucVerticalSplit.ClientID %>') != null)
            document.getElementById('<%=ucVerticalSplit.ClientID %>').onmousedown = click;--%>

        function SetSourceURL(type, url, selectednode,designedyn) {

           

            var args = "function=SetCurrentDMSDocumentSelection|selectednode=" + selectednode + "|type=" + type;
            var arrValue = AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);

            if (designedyn != null && designedyn === "Y") 
            {
                openNewWindow('Form', '', url);
            }
            else {
                if (type == 6 && arrValue != "") {
                    openNewWindow('Form', '', url);
                }
                else {
                    document.getElementById('ifMoreInfo').src = url;
                }
            }
            HighlightSelectedNode(selectednode);
        }

        function KeywordSearch() {
            var searchvalue = document.getElementById('<%=txtKeyWord.ClientID%>').value;
            //SetSourceURL('', '../DocumentManagement/DocumentManagementSearchResults.aspx?keyword=' + searchvalue, '');
            document.getElementById('ifMoreInfo').src = '../DocumentManagement/DocumentManagementSearchResultsNew.aspx?keyword=' + searchvalue;
        }

        function checkSubmit(e) {
            if (e && e.keyCode == 13) {
                var navigatorVersion = navigator.appVersion;
                var navigatorAgent = navigator.userAgent;
                var browserName = navigator.appName;
                var fullVersionName = '' + parseFloat(navigator.appVersion);
                var majorVersionName = parseInt(navigator.appVersion, 10);
                var nameOffset, verOffset, ix;

                if ((verOffset = navigatorAgent.indexOf("MSIE")) != -1) {
                    var img = document.getElementById("imgSearch");
                    img.onclick();
                }
                else {
                    imgSearch.click();
                }
                return false;
            }
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
                            span.className = (ul.style.display == "none") ? "collapsed" : "expanded";

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

        function HighlightSelectedNode(listid) {

            var firsttag = document.getElementById(listid);

            if (firsttag != null && firsttag.getElementsByTagName("a").length > 0) {
                //to remove all the previous selections on the hyperlinks.
                $('a').removeClass('select');

                //to set current selection the current hyperlink clicked.               
                var hlink = firsttag.getElementsByTagName("a")[0];
                hlink.className = "select";

                if ($("#" + listid).closest('ul') != null)                  //To set display: block to ul tag in the current li tag (to expand the current node).
                    $("#" + listid).closest('ul').css("display", "block");

                var parentlist = $("#" + listid).parents('li');             //To get all the list of parent li tags
                for (var i = 0; i < parentlist.length; i++) {
                    $(parentlist[i]).closest('ul').css("display", "block"); //find span tag in each parent li tag
                    $("#" + parentlist[i].id + " > span:last-child").attr("className", "expanded"); //To change the symbol to - in the span tag.
                }

                //To scroll the scroll bar to the selected hyperlink
                var container = $('#divDocumentCategory')
                var scrollTo = $("#" + listid);

                container.animate({
                    scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop() - 120
                });
            }
        }

        function PopulateTreeOnDemand(listid, ulid) {
            var li = document.getElementById(listid);
            var ul = document.getElementById(ulid);

            // To get the content for the tree node

            var args = "function=PopulateNewDMSTreeOnDemand|selectednode=" + li.id + "|type=" + li.value;
            var arrValue = AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);
            if (arrValue != "")
                $('#' + ul.id).append(arrValue);
            $('#' + li.id).data('populated', '1');

            PrepareTree(li.id);
        }



        document.addEventListener("DOMContentLoaded", () => {
            document.getElementById("txtKeyWord").addEventListener("keyup", function (event) {
                event.preventDefault();
                if (event.keyCode === 13) {
                    KeywordSearch();
                }
            });
        });

    

    </script>

    <%--this is the style for the tree--%>
    
    <style>
        body {
            margin: 0;
            padding: 0; 
            text-align: left;
            font-family:Arial, Helvetica, sans-serif;
        }

        a {
            text-decoration: none;
            color: #1e395b;
        }

        a:hover {
            text-decoration: none;
        }

        .select {
            text-decoration: none;
            color: Black;
            font-weight: bold;
            background-color: #bbddff;
            border: 3px solid #FFB380;
        }

        h1 {
            font-size: 140%;
            margin: 0 20px;
            line-height: 80px;
        }

        #container {
            margin: 0 auto;
            width: 680px;
            background: #fff;
            padding-bottom: 20px;
        }

        #content {
            margin: 0 20px;
        }

        p {
            margin: 0 auto;
            width: 680px;
            padding: 1em 0;
        }

        .style1 {
            width: 75px;
        }

       .hover-effect a {
           color: #1e395b;
           background-color: #edf4fd;
           background-image: linear-gradient(#f9fbfd,#edf4fd);
           border: 1px solid #bfd8f8;
           padding: 6px;
       }

       #sitemap li span {
            float: left;
            position: absolute;
            top: 8px;
            left: 5px;
            width: 13px;
            height: 13px;
            cursor: auto;
            font-size: 0;
        }

       #sitemap li a {
           padding-left:5px;
            line-height: 22px;
       }

       #sitemap, #sitemap ul {
           list-style: none;
           margin: 0;
           padding: 0;
        } 

        #sitemap ul {
          margin-left: 10px;
        }

       #sitemap li {
          margin: 0;
          padding: 0 7px;
          line-height: 20px;
          color: #369;
          font-weight: normal;
          border-left:1px dotted rgb(100,100,100);
        }

        #sitemap li:before {
           position:relative;
           top:-0.3em;
           height:1em;
           width:12px;
           color:white;
           border-bottom:1px dotted rgb(100,100,100);
           content:"";
           display:inline-block;
           left:-7px;
        }

        #sitemap li:last-child:before {
             border-left:1px dotted rgb(100,100,100);   
        }

        #sitemap li span, #sitemap li span.collapsed  {
            background: url(../css/Theme1/images/arrowDown.png) no-repeat 0 0;
        }

        #sitemap li span, #sitemap li span.expanded {
            background: url(../css/Theme1/images/arrowUp.png) no-repeat 0 0;
        }

        #parentlist  > a {
            font-weight:bold;
        }

    </style>
</telerik:RadCodeBlock></head>
<body onload="ExpandFirstLevel();" onresize="resizeFrame(document.getElementById('ifMoreInfo'));resizeFrame(document.getElementById('divDocumentCategory')); "">
    <form id="form1" runat="server" name="form1">    
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadSplitter1" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="divDocumentCategory">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divDocumentCategory" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:TabStrip ID="MenuDiscussion" runat="server" Title="Document Management" OnTabStripCommand="MenuDiscussion_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" BorderColor="White" Height="94%" Width="99.5%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="25%" Scrolling="None" Height="93%">
                <div style="overflow:auto; height: 800px; width:inherit; float: left; clear: right;
                    z-index: +2; position: relative;" id="divDocumentCategory" runat="server">
                    <table id="tblkeyword" runat="server" cellpadding="0" cellspacing="0" style="float: left; width: 385px;">
                        <tr>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtKeyWord" MaxLength="50" CssClass="input" Width="100%" EmptyMessage="Type to search"></telerik:RadTextBox>&nbsp;                                
                            </td>
                            <td>
                                    <img id="imgSearch" runat="server" src="<%$ PhoenixTheme:images/search.png %>" onclick="KeywordSearch();"
                                    style="display:inline-block;width:30px;cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />                             
                            </td>

                        </tr>
                    </table>
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
                </telerik:RadPane>
            <telerik:RadSplitBar ID="ucVerticalSplit" runat="server" CollapseMode="Forward" Height="100%">
            </telerik:RadSplitBar>         
                <%--<eluc:VerticalSplit runat="server" ID="ucVerticalSplit" TargetControlID="divDocumentCategory" />--%>
            <telerik:RadPane ID="contentPane" runat="server" Width="73%" Scrolling="None" BorderColor="White" Height="93%">
                    <iframe runat="server" id="ifMoreInfo" style="min-height: 450px; position:relative; width: 100%"></iframe>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
