S<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardMapFullView.aspx.cs"
    Inherits="Dashboard_DashboardMapFullView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>

    <script type="text/javascript">
        var map_popup;
        function InitializePopUpMap() {
            var mapOptions = {
                zoom: 1,
                center: new google.maps.LatLng(32.842674,32.34375),
                disableDefaultUI: true,
                zoomControl: true,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            map_popup = new google.maps.Map(document.getElementById("map1"),
                                mapOptions);                                                           
            generateMarkers(<%=vesselposition %>);
        }
        
        function generateMarkers(args)
         {
            var infowindow = new google.maps.InfoWindow();

            var marker, i;
         
             for (var i = 0; i < args.length; i++) 
            {
                marker = new google.maps.Marker({
                    position: new google.maps.LatLng(args[i][2], args[i][3]),
                    map_popup: map_popup,
                    title: args[i][0]
                });
                
                 google.maps.event.addListener(marker, 'click', (function(marker, i) {
                    return function() {
                        infowindow.setContent(args[i][4]);
                        infowindow.open(map_popup, marker);
                    }
                })(marker, i));
                
            }
           
            return false;
        }        
    </script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <eluc:Error ID="ucError" runat="server" Visible="false" />
        <div id="map1" style="width: 100%;">
        </div>
        <script type="text/javascript">
            InitializePopUpMap();
        </script>
    </form>
</body>
</html>
