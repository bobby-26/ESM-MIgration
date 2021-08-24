<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCommonOffice.aspx.cs"
    Inherits="DashboardCommonOffice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixdashboard.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>

    <script type="text/javascript">

        // Define the overlay, derived from google.maps.OverlayView
        function Label(opt_options) {
         // Initialization
         this.setValues(opt_options);

         // Label specific
         var span = this.span_ = document.createElement('span');
         span.style.cssText = 'position: relative; left: -12px; top: 0px; ' +
                              'white-space: nowrap; border: 1px solid blue; ' +
                              'padding: 2px; background-color: white;';

         var div = this.div_ = document.createElement('div');
         div.appendChild(span);
         div.style.cssText = 'position: absolute; display: none';
        };
        Label.prototype = new google.maps.OverlayView;

        // Implement onAdd
        Label.prototype.onAdd = function() {
         var pane = this.getPanes().overlayLayer;
         pane.appendChild(this.div_);

         // Ensures the label is redrawn if the text or position is changed.
         var me = this;
         this.listeners_ = [
           google.maps.event.addListener(this, 'position_changed',
               function() { me.draw(); }),
           google.maps.event.addListener(this, 'text_changed',
               function() { me.draw(); })
         ];
        };

        // Implement draw
        Label.prototype.draw = function() {
         var projection = this.getProjection();
         var position = projection.fromLatLngToDivPixel(this.get('position'));

         var div = this.div_;
         div.style.left = position.x + 'px';
         div.style.top = position.y + 'px';
         div.style.display = 'block';

         this.span_.innerHTML = this.get('text').toString();
        };


        var map;
        function InitializeMap() {
            var mapOptions = {
                zoom: 2,
                center: new google.maps.LatLng(32.842674,32.34375),
                disableDefaultUI: true,
                zoomControl: true,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            map = new google.maps.Map(document.getElementById("map"),
                                mapOptions);                                                           
            generateMarkers(<%=vesselposition %>);
            resize();
        }
        
        function generateMarkers(args)
         {
            var infowindow = new google.maps.InfoWindow();

            var marker, i;
            
            var label;
         
             for (var i = 0; i < args.length; i++) 
            {
                marker = new google.maps.Marker({
                    position: new google.maps.LatLng(args[i][2], args[i][3]),
                    map: map,
                    title: args[i][0]
                });
                 
                  var label = new Label({
               map: map
          });
                label.set('zIndex', 1234);
                label.bindTo('position', marker, 'position');
                label.set('text', args[i][5]);
          
                 google.maps.event.addListener(marker, 'click', (function(marker, i) {
                    return function() {
                        infowindow.setContent(args[i][4]);
                        infowindow.open(map, marker);
                    }
                })(marker, i));
                
            }
           
            return false;
        }        
    </script>

    <script type="text/javascript">
        function resize() {
            var height = ((document.all ? (document.documentElement && document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight) : window.innerHeight)) - 20;
            var width = ((document.all ? (document.documentElement && document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body.clientWidth) : window.innerWidth));

            var obj2 = document.getElementById("map");
            obj2.style.height = height / 1 + "px"; 
        }
    </script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="db_container">
        <eluc:DashboardMenu runat="server" ID="ucDashboardMenu" />
        <div class="db_results">
            <eluc:Error ID="ucError" runat="server" Visible="false" />
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 99.9%;">
                <table id="tblvessellist" width="100%">
                    <tr>
                        <td valign="top">
                            <div style="width: 100%;">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Fleet ID="ucFleet" runat="server" CssClass="input" AppendDataBoundItems="true"
                                                AutoPostBack="true" OnTextChangedEvent="ucFleet_Changed" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlVessel" runat="server" CssClass="input" AutoPostBack="true"
                                                OnSelectedIndexChanged="ucVessel_Changed">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblNavication" runat="server" AppendDataBoundItems="false"
                                                RepeatDirection="Horizontal" AutoPostBack="true" RepeatLayout="Table" OnSelectedIndexChanged="SetNavication"
                                                CssClass="readonlytextbox">
                                                <asp:ListItem Text="Map" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Map With Vessel List" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <div id="map" style="width: 100%;">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <eluc:Status runat="server" ID="ucStatus" />
    <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
    </form>
</body>
</html>
