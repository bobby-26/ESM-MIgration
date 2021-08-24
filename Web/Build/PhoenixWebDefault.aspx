<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhoenixWebDefault.aspx.cs" Inherits="PhoenixWebDefault" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Application["softwarename"].ToString() %></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EDGE,10,9,8" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="shortcut icon" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/images/favicon.png" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.telerik.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        
        <script type="text/javascript">
            function itemOpened(sender, args) {    
                var parentHeight = $(window).height();
                var item = args.get_item();
                var childCount = item.get_items().get_count();
                var scrollWrapElement = args.get_item()._getScrollWrapElement();
                var groupHeight = item.get_groupSettings().get_height();
                var itemHeight = item.get_element().offsetHeight;
                itemHeight = childCount * itemHeight;
                //if (itemHeight < parseInt(groupHeight.replace("px",""))) {
                //    scrollWrapElement.style.height = itemHeight + "px";
                //}
                var height = itemHeight + 10;
                if (itemHeight > parentHeight)
                    height = (parentHeight - 50)
                scrollWrapElement.style.height = height + "px";                
            }   
            function PanelBarExpandCollapse(sender, args) {
                var btn = $find("BtnExpandCollapse");//document.getElementById("BtnExpandCollapse");                
                var isexpand = btn.get_text() == "+" ? true : false;
                var panel = $find("<%= radMenuTree.ClientID %>");
                for (var i = 0; i < panel.get_allItems().length; i++) {
                    if (isexpand)
                        panel.get_allItems()[i].expand();
                    else
                        panel.get_allItems()[i].collapse();
                }
               
                if (isexpand) {
                    btn.set_text("-");
                    btn.set_toolTip("Collapse All");
                }
                else {
                    btn.set_text("+");
                    btn.set_toolTip("Expand All");
                }
                sender.set_cancel(true);
            }
            
        </script>
        <style type="text/css">
            #BtnExpandCollapse {
                min-width: 0;
            }

            .searchtxt {
                margin-left: 1px;
            }

            .rnvLink .radIcon {
                line-height: 1.75em !important;
            }

            .RadNavigation {
                line-height: 2em !important;
            }
            .rwIconDisable:before {
              content: none !important;
            }
             /* classic render mode */
			div.RadWindow .rwControlButtons li a.customprintbutton
			{
				background: url(classic-sprite.png) no-repeat;
			}

			div.RadWindow .rwControlButtons li a.customprintbutton:hover
			{
				background: url(classic-sprite.png) 0 -20px no-repeat;
			}
			
			/* ensure the before pseudoelement is not visible. If you will be using custom font icons, tweak this accordingly */
			div.RadWindow .rwCommandButton.rwHelpButton::before
			{
				content: "\e402";/* this is where you can set your own custom font icon. You will also need to set the appropriate font name */
			}
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="Radscriptmanager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                <asp:ScriptReference Path="~/js/phoenixPopup.js" />
                <asp:ScriptReference Path="~/js/phoenix.js" />                
                <asp:ScriptReference Path="~/js/phoenix.telerik.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <asp:HiddenField runat="server" ID="nav" />
        <asp:HiddenField runat="server" ID="hdnModule" />
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <asp:Button runat="server" ID="cmdApplication" OnClick="cmdApplication_Click" CssClass="hidden" />
        <asp:Button runat="server" ID="cmdVessel" OnClick="cmdVessel_Click" CssClass="hidden" />
        <div id="about" runat="server"  class="slider">
            <iframe width="0" height="0" id="ifrKeepAlive" src="PhoenixKeepAlive.aspx" style="display: none"></iframe>
            <header class="tm-header" id="logo"></header>
            <h2 class="logo" style="cursor: pointer" onclick="javascript:document.getElementById('cmdApplication').click()">
                <img runat="server" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>" alt="Phoenix" height="20" width="30" />
            </h2>            
            <telerik:RadToolBar ID="mainToolBar" runat="server" Width="0px" Height="0px"
                EnableRoundedCorners="true" EnableShadows="true">
            </telerik:RadToolBar>            
            <telerik:RadTextBox ClientEvents-OnLoad="telerikNav.clientTreeSearch" ID="txtMenuSearch" runat="server" CssClass="searchtxt" Width="80%" EmptyMessage="Type to search" AutoPostBack="false" />
            <telerik:RadButton ID="BtnExpandCollapse" runat="server" Text="+" ToolTip="Expand All" OnClientClicked="PanelBarExpandCollapse">                
            </telerik:RadButton>
            <telerik:RadPanelBar ID="radMenuTree" runat="server" Width="100%"
                DataSourceID="menuXML" DataTextField="NAME" DataNavigateUrlField="URL" Height="500px"
                ExpandMode="MultipleExpandedItems" OnItemDataBound="radMenuTree_ItemDataBound" OnItemClick="radMenuTree_ItemClick">
            </telerik:RadPanelBar>
            <telerik:RadMenu ID="PhoenixModule" runat="server" Width="100%" OnItemClick="PhoenixModule_ItemClick" OnClientItemOpened="itemOpened" DefaultGroupSettings-ExpandDirection="Up">             
            </telerik:RadMenu>
            <asp:XmlDataSource runat="server" ID="XmlDataSourceModule" XPath="//DIRECTORY" />
        </div>

        <div class="page">

            <div class="main-header clear">
                <telerik:RadNavigation ID="mainNavigation" runat="server" CssClass="header-info" Width="100%" Height="37px" OnClientNodeClicked="onNavigationClicked" OnClientNodeMouseEnter="onNavigationMouseEnter">
                    <Nodes>
                        <telerik:NavigationNode>
                            <NodeTemplate>
                                <div class="home-btn" id="menubar" runat="server" style="padding-left: 5px;">
                                    <span class="icon"><i class="fas fa-bars"></i></span>
                                    <telerik:RadLabel runat="server" ID="lblTitle" CssClass="text"></telerik:RadLabel>
                                </div>
                            </NodeTemplate>
                        </telerik:NavigationNode>
                        <telerik:NavigationNode Text="" CssClass="user right" SpriteCssClass="icon fas fa-sort-down">
                        </telerik:NavigationNode>
                        <%--<telerik:NavigationNode Text="Skin" runat="server" CssClass="right">
                            <NodeTemplate>                                
                                <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" OnSkinChanged="RadSkinManager1_SkinChanged"/>
                            </NodeTemplate>
                        </telerik:NavigationNode> --%>
                        <telerik:NavigationNode Text="Vessel" runat="server" CssClass="right">
                            <NodeTemplate>
                                <telerik:RadComboBox ID="ddlVessel" DropDownCssClass="drpdwn" runat="server" OnItemDataBound="ddlVessel_ItemDataBound" OnSelectedIndexChanged="ddlVessel_TextChanged" Width="180" EnableLoadOnDemand="True" AutoPostBack="true"
                                    DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" EmptyMessage="Type to select vessel" Filter="Contains" MarkFirstMatch="true" EnableTextSelection="true">
                                </telerik:RadComboBox>
                            </NodeTemplate>
                        </telerik:NavigationNode>
                        <telerik:NavigationNode Text="Company" runat="server" CssClass="right">
                            <NodeTemplate>
                                <telerik:RadComboBox ID="ddlCompany" runat="server" OnItemDataBound="ddlCompany_ItemDataBound" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" Width="120" EnableLoadOnDemand="True" AutoPostBack="true"
                                    DataValueField="FLDCOMPANYID" DataTextField="FLDSHORTCODE" EmptyMessage="Type to select Company" Filter="Contains" MarkFirstMatch="true" EnableTextSelection="true">
                                </telerik:RadComboBox>
                            </NodeTemplate>
                        </telerik:NavigationNode>
                       <telerik:NavigationNode Text="FMS" CssClass="right" ID="lnkFMSSearch" runat="server" >
                            <Nodes>
                                <telerik:NavigationNode Text="Drawings and Plans" Target="fraPhoenixApplication" NavigateUrl="~/DocumentManagement/DocumentManagementFMSDrawingList.aspx" ID="lnkDrawings" runat="server"></telerik:NavigationNode>
                                <telerik:NavigationNode Text="E Filing" Target="fraPhoenixApplication" NavigateUrl="~/DocumentManagement/DocumentFMSFileNoList.aspx" ID="lnkEFiling" runat="server"></telerik:NavigationNode>
                                <telerik:NavigationNode Text="E Mails" Target="fraPhoenixApplication" NavigateUrl="~/DocumentManagement/DocumentManagementFMSMailList.aspx" ID="lnkEMails" runat="server"></telerik:NavigationNode>
                                <telerik:NavigationNode Text="Equipment Manuals" Target="fraPhoenixApplication" NavigateUrl="~/DocumentManagement/DocumentManagementFMSEquipmentManuals.aspx" ID="lnkEquipManuals" runat="server"></telerik:NavigationNode>
                                <telerik:NavigationNode Text="Maintenance Forms" Target="fraPhoenixApplication" NavigateUrl="~/DocumentManagement/DocumentManagementFMSMaintenanceHistoryTemplate.aspx" ID="lnkMainForms" runat="server"></telerik:NavigationNode>
                                <telerik:NavigationNode Text="Office Forms" Target="fraPhoenixApplication" NavigateUrl="~/DocumentManagement/DocumentManagementFMSOfficeFormList.aspx?CATEGORYNO=16&Callfrom=1" ID="lnkOfficeForms" runat="server"></telerik:NavigationNode>
                                <telerik:NavigationNode Text="Plans – Class approved & others" Target="fraPhoenixApplication" NavigateUrl="~/DocumentManagement/DocumentManagementFMSManualList.aspx" ID="lnkPlansManuals" runat="server"></telerik:NavigationNode>
                                <telerik:NavigationNode Text="Shipboard Forms" Target="fraPhoenixApplication" NavigateUrl="~/DocumentManagement/DocumentManagementFMSShipboardFormList.aspx?CATEGORYNO=2" ID="lnkShipboardForms" runat="server"></telerik:NavigationNode>
                            </Nodes>
                        </telerik:NavigationNode>
                        <telerik:NavigationNode Text="Help" CssClass="right" ID="lnkHelp" runat="server">
                            <Nodes>
                                <telerik:NavigationNode Text="Help" Target="_blank" NavigateUrl="~/PHOENIXHELP/index.html" ID="lnkPhoenixHelp" runat="server"></telerik:NavigationNode>
                                <telerik:NavigationNode Text="EPSS" Target="_blank" NavigateUrl="~/EPSSHELP/index.html" ID="lnkEPSS" runat="server"></telerik:NavigationNode>
                            </Nodes>
                        </telerik:NavigationNode>
                        <telerik:NavigationNode  ID="lnkCrewSearch" Text="Crew Search" runat="server" CssClass="searchWrapper right" Target="fraPhoenixApplication" NavigateUrl="~/Dashboard/DashboardSearch.aspx">
                        </telerik:NavigationNode>
                        <telerik:NavigationNode  ID="lnkDMSSearch" Text="DMS Search" runat="server" CssClass="searchWrapper right" Target="fraPhoenixApplication" NavigateUrl="~/DocumentManagement/DocumentManagementNewDocumentTree.aspx">
                        </telerik:NavigationNode>
                        <telerik:NavigationNode CssClass="eventWrapper right" ID="lnkFeedback" runat="server" Visible="false">
                            <NodeTemplate>
                                <a class="rnvRootLink rnvLink" onclick="javascript:openNewWindow('EventFeedback', '', 'Inspection/InspectionSupdtEventFeedbackBanner.aspx?sourcefrom=0');" style="padding-top: 0px !important;">
                                    <span class="rnvText">Feedback</span>
                                </a>
                           </NodeTemplate>
                        </telerik:NavigationNode>
                        <%--<telerik:NavigationNode Text="Office" runat="server" CssClass="officeWrapper right" ID="lnkOffice" >
                            <NodeTemplate>
                                <a class="rnvRootLink rnvLink" runat="server" onserverclick="lnkOffice_ServerClick" style="padding-top: 0px !important;">
                                    <span class="rnvText" id="spnOffice" runat="server">Office</span>
                                </a>
                           </NodeTemplate>
                        </telerik:NavigationNode>--%>
                        <telerik:NavigationNode Text="Phoenix" runat="server" CssClass="logoWrapper right">
                            <NodeTemplate>
                                <a class="rnvRootLink rnvLink" id="lnkApplication" runat="server" onserverclick="lnkApplication_ServerClick" style="padding-top: 0px !important;">
                                    <span class="rnvText" id="spnApplication" runat="server">Phoenix</span>
                                </a>
                           </NodeTemplate>
                        </telerik:NavigationNode>
                    </Nodes>
                </telerik:RadNavigation>
            </div>

            <div class="container clear">
                <div class="sidebar" id="sidebar" style="display: none">
                </div>

                <div class="section bottom">
                    <iframe id="fraPhoenixApplication" name="fraPhoenixApplication" width="100%" height="100%" frameborder="0" runat="server"></iframe>
                </div>
            </div>

            <div class="tm-click-overlay"></div>

            <telerik:RadToolTip runat="server" ID="UserProfile" IsClientID="true" EnableShadow="true" HideEvent="FromCode" Visible="true" TargetControlID="aa">
                <img src="css/Theme1/images/Blank.png" alt="" />
                <div class="content">
                    <span style="display:none"><%= PhoenixSecurityContext.CurrentSecurityContext.Email %></span>
                    <span class="addBtn" id="spnfeedback" runat="server" visible="false">
                        <span onclick="javascript:openNewWindow('EventFeedback', '', '<%=Session["sitepath"] %>/Inspection/InspectionSupdtEventFeedbackBanner.aspx?sourcefrom=0');">
                            <span class="icon"><i class="far fa-comment"></i></span>
                            <span class="text">Event Feedback</span>
                        </span>
                        <br />
                        <%-- <span onclick="javascript:document.getElementById('fraPhoenixApplication').src='Dashboard/Dashboard.aspx';">
                            <span class="icon"><i class="fas fa-ship"></i></span>
                            <span class="text">Dashboard</span>
                        </span>
                        <br />
                        <span>
                        <span class="icon"><i class="fas fa-ship"></i></span>
                        <span class="text" id="spnTimeout"></span>
                        </span>--%>
                    </span>
                    <telerik:RadButton runat="server" ID="SignOut" ButtonType="LinkButton" Text="Sign Out" PostBackUrl="PhoenixLogout.aspx"></telerik:RadButton>
                </div>
            </telerik:RadToolTip>
        </div>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="OnRequestStart" ClientEvents-OnResponseEnd="OnResponseEnd">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ddlVessel">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radMenuTree" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="fraPhoenixApplication" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlCompany">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radMenuTree" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="fraPhoenixApplication" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radMenuTree">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radMenuTree"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ddlVessel"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ddlCompany"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="lblTitle"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="fraPhoenixApplication" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>                
                <telerik:AjaxSetting AjaxControlID="cmdApplication">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="fraPhoenixApplication" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="lblTitle" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                 <telerik:AjaxSetting AjaxControlID="lnkApplication">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="fraPhoenixApplication" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="lblTitle" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>  
                <telerik:AjaxSetting AjaxControlID="PhoenixModule">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radMenuTree" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                 <telerik:AjaxSetting AjaxControlID="cmdVessel">
                    <UpdatedControls>                        
                         <telerik:AjaxUpdatedControl ControlID="ddlVessel"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>

        <telerik:RadWindowManager ID="phoenixPopup" runat="server" EnableShadow="true" RenderMode="Lightweight" ShowContentDuringLoad="true"
            Behaviors="Close, Move, Resize, Maximize, Minimize" DestroyOnClose="true" Opacity="99" OnClientShow="OnClientShow"
            Width="450" Height="400" Modal="true" VisibleStatusbar="false">
        </telerik:RadWindowManager>
        <asp:XmlDataSource runat="server" ID="menuXML" XPath="/RECORDSET/DIRECTORY"></asp:XmlDataSource>
        <div id="dragBG"></div>
        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
            <script type="text/javascript">
                var worker;
                var page;
                var nav = $get("<%= nav.ClientID %>").value;
                var currModule = $get("<%= hdnModule.ClientID %>");
                function pageLoad() {
                    if (nav === "desktop") {
                        worker = desktop;
                    } else {
                        worker = mobile;
                    }
                    if ($(".user .fa-user-circle").length == 0) {
                        $(".user .rnvText").after("<i class=\"fas fa-user-circle\" style=\"font-size:2em;padding-left: 5px;\"></i>");
                    }
                    $('#mainToolBar').hide();
                    if (typeof desktop === 'object')
                        desktop.initialize(nav);
                    if (typeof initializeInteractions === 'function')
                        initializeInteractions();
                    if (typeof mobile === 'object')
                        mobile.initialize(nav);
                    clear();
                    $("#about").attr("class", "slider RadToolBar_<%=Session["skin"]%>");
                    if ($(".logo span").length == 0) {
                        $(".logo").append("<span><%=Application["softwarename"].ToString().ToLower() %><span>");
                    }
                    resizeTree();
                    $telerik.$(window).resize(function () { resizeTree(); });
                    //$telerik.$(window).load(function () { resizeTree(); });                        
                }

                function initializeInteractions() {
                    $(".user").on("click", function (e) {
                        toggleToolTip(e);
                    })
                }

                function toggleToolTip(e) {
                    var tooltip = $find("<%=UserProfile.ClientID %>");

                    if (!tooltip.isVisible() == true) {
                        var $node = $(e.currentTarget);
                        var arrowPosition = $node.offset();

                        tooltip.show();
                        var popup = tooltip.get_popupElement();

                        positionX = arrowPosition.left - $(popup).outerWidth(true) + $node.outerWidth(true) + 5;
                        positionY = arrowPosition.top + $node.outerHeight(true) + 3;

                        setTimeout(function () {
                            $telerik.setLocation(popup, { x: (positionX - 5), y: (positionY - 10) });
                        }, 20)

                        $("html").on("click", function (e) {
                            tooltip.hide();
                        })
                    }
                    else {
                        tooltip.hide();
                    }
                }
                function FocusItem(menuCode) {
                    var panelbar1 = $find('mnuNavigationBar');
                    var myitem = panelbar1.findItemByValue(menuCode);
                    if (myitem) {
                        if (myitem.get_level() != 0) {
                            var parentItem = myitem;

                            for (var i = 0; parentItem.get_level() > 0; i++) {
                                parentItem = parentItem.get_parent();
                                parentItem.set_expanded(true);
                            }
                        }
                        myitem.get_linkElement().scrollIntoView({ behavior: "smooth", block: "center", inline: "nearest" });
                        myitem.select();
                    }
                }



                //if (window.name == "") { 
                window.name = "";
                ValidateTab("");
                //}

                function ValidateTab(tabid) {
                    var tabid = window.name ? window.name : "";
                    var data = "function=VaidateTab|tabid=" + tabid;
                    $.ajax({
                        type: 'POST',
                        url: SitePath + "PhoenixWebFunctions.aspx",
                        data: data,
                        success: function (response) {
                            if (response != "" && response.toLowerCase().indexOf("tab_") > -1) {
                                window.name = response;
                            } else if (response != "" && response.toLowerCase().indexOf("phoenixbrowsingrestriction") > -1) {
                                top.location.href = response;
                            }
                        },
                        async: false
                    });
                }
                setInterval(function () { ValidateTab() }, 3000);

                function changeRenderMode() {
                    var browserHeight = $telerik.$(window).height();
                    $find('radMenuTree').ajaxRequest(browserHeight - 100);
                }
                function resizeTree() {
                    var treeDiv = document.getElementById("<%=radMenuTree.ClientID%>");
                    var browserHeight = $telerik.$(window).height();
                    treeDiv.style.height = (browserHeight - 100) + "px";
                }
                (function (global, undefined) {
                    var telerikNav = global.telerikNav = {};
                    var treeSearchTimer = null;
                    telerikNav.clientTreeSearch = function (sender) {
                        $telerik.$(".riTextBox", sender.get_element().parentNode).bind("keydown", telerikNav.valueChanging);
                    };
                    telerikNav.valueChanging = function (sender, eventArgs) {
                        if (treeSearchTimer) {
                            clearTimeout(treeSearchTimer);
                        }

                        treeSearchTimer = setTimeout(function () {
                            var tree = $find("radMenuTree");
                            var textbox = $find("txtMenuSearch");
                            var searchString = textbox.get_element().value;      
                            if (searchString == "Type to search") return;
                            for (var i = 0; i < tree.get_items().get_count(); i++) {
                                telerikNav.findNodes(tree.get_items().getItem(i), searchString);
                            }
                        }, 200);
                    };
                    telerikNav.findNodes = function (node, searchString) {
                        node.set_expanded(true);
                        node.expand();
                        var hasFoundChildren = false;
                        for (var i = 0; i < node.get_items().get_count(); i++) {
                            hasFoundChildren = telerikNav.findNodes(node.get_items().getItem(i), searchString) || hasFoundChildren;
                        }

                        if (hasFoundChildren || node.get_text().toLowerCase().indexOf(searchString.toLowerCase()) != -1) {
                            node.set_visible(true);
                            return true;
                        }
                        else {
                            node.set_visible(false);
                            return false;
                        }
                    };
                })(window);                
                var yPos;
                function SaveScrollPosition() {
                    yPos = document.getElementById("<%= radMenuTree.ClientID %>").scrollTop;                    
                }

                function ReturnPos() {
                    document.getElementById("<%= radMenuTree.ClientID %>").scrollTop = yPos;
                }


                function OnResponseEnd(sender, eventArgs) {
                    ReturnPos();                    
                }

                function OnRequestStart(sender, eventArgs) {
                    SaveScrollPosition();                    
                }
                function SetHelp(sender, args) {
                    if (sender._helpWindowURL == null) return;
                    var TitleBar = sender.GetTitlebar();
                    var parent = TitleBar.parentNode;
                    var oUL = parent.getElementsByTagName('UL')[0];
                    if (!(oUL.firstChild.id == "customhelpbuttonID")) // Check if the element is already added 
                    {
                        // If not - create and add the custom button 
                        var oLI = document.createElement("LI");
                        oLI.id = "customhelpbuttonID"

                        //create the actual button
                        var customBtn;
                        //set the proper custom button class for decoration
                        var customBtnClass = "rwHelpButton";
                        //choose the handler for the button
                        var desiredHandler = function (e) {
                            var helpURL = sender._helpWindowURL;
                            openNewWindow('help', 'Help', helpURL);
                            if (!e)
                                e = window.event;
                            return $telerik.cancelRawEvent(e);
                        };
                        switch (sender._renderMode) {
                            case Telerik.Web.UI.RenderMode.Lite: {//lightweight
                                customBtn = document.createElement("span");
                                customBtn.className = "rwCommandButton ";
                                oLI.className = "rwListItem";
                                break;
                            }
                            case Telerik.Web.UI.RenderMode.Classic: {//classic
                                customBtn = document.createElement("a");
                                customBtn.href = "javascript:void(0)";
                                //if the titlebar does not appear OK you may need to tweak its width
                                //oUL.style.width = "192px";
                                break;
                            }
                            default: {
                                if (console && console.log)
                                    console.log("Unknown render mode. Examine the HTML to see what elements to use");
                            }
                        }

                        customBtn.className = customBtn.className + customBtnClass;
                        customBtn.title = "Help";

                        //add the required handler for your custom functionality
                        if (Telerik.Web.UI.EventType) { //Q3 2015 versions and later - modern IE versions supported (e.g., Edge, 11)
                            var NS = ".wndCustomButton";
                            var DOWN_NS = Telerik.Web.UI.EventType.Down + NS;
                            $telerik.$(customBtn).onEvent(DOWN_NS, desiredHandler);
                        }
                        else { //old Telerik.Web.UI (pre Q2 2015)
                            customBtn.onmousedown = desiredHandler;
                        }

                        //add the custom button to the RadWindow
                        oLI.appendChild(customBtn);
                        oUL.insertBefore(oLI, oUL.firstChild);
                        //setTimeout(function () { oUL.insertBefore(oLI, oUL.firstChild); }, 200);
                        //console.log(oUL);
                        //if RadWindow does not display its titlebar properly after being modified, uncomment this code:
                        //sender._updateTitleWidth();
                    }
                }
                function OnClientShow(sender, args) {
                    setTimeout(function () { SetHelp(sender, args); }, 200);
                }
            </script>

            <%--<script type="text/javascript">
            var timeout = '<%= Session.Timeout * 60 * 1000 %>';
            var timer = setInterval(function () {
                timeout -= 1000;
                document.getElementById('spnTimeout').innerHTML = "Your Session will timeout in " + time(timeout) + " minutes.";
                if (timeout == 0) {
                    clearInterval(timer);
                    alert('Your Session has expired. You will be redirected to another page.!');
                    //window.location.replace("SessionLogoutPage.aspx");
                }
            }, 1000);

            function two(x) {
                return ((x > 9) ? "" : "0") + x
            }

            function time(ms) {
                var t = '';
                var sec = Math.floor(ms / 1000);
                ms = ms % 1000

                var min = Math.floor(sec / 60);
                sec = sec % 60;
                t = two(sec);

                var hr = Math.floor(min / 60);
                min = min % 60;
                t = two(min) + ":" + t;
                return t;
            }
        </script>--%>
        </telerik:RadCodeBlock>
        <style type="text/css">
            .drpdwn img {
                height: 25px;
                width: 25px;
                border-radius: 50%;
            }
        </style>
    </form>
</body>
</html>
