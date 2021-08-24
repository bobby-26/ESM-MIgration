<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardV2Map.aspx.cs" Inherits="Dashboard_DashboardV2Map" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>   
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <a id="lnkOtis" class="text-primary pull-right" style="text-decoration: underline" href="javascript: top.openNewWindow('otis', 'OTIS', 'https://system.stratumfive.com/otis/index.html', true);"> OTIS</a>
        <telerik:RadMap runat="server" ID="RadMap1" Zoom="2" OnItemDataBound="RadMap1_ItemDataBound" Height="600px">
            <CenterSettings Latitude="23" Longitude="10" />
            <DataBindings>
                <MarkerBinding DataTitleField="FLDVESSELNAME" DataLocationLatitudeField="FLDDECIMALLAT" DataLocationLongitudeField="FLDDECIMALLONG" />
            </DataBindings>
            <LayersCollection>
                <telerik:MapLayer Type="Tile" Subdomains="a,b,c"
                    UrlTemplate="https://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png"
                    Attribution="&copy; <a href='http://osm.org/copyright' title='OpenStreetMap contributors' target='_blank'>OpenStreetMap contributors</a>.">
                </telerik:MapLayer>
            </LayersCollection>
        </telerik:RadMap>
    </form>
</body>
</html>
