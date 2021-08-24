<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardOffice.aspx.cs"
    Inherits="Dashboard_DashboardOffice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

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
                zoom: 1,
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

            //alert(screen.availHeight);

            var height = ((document.all ? (document.documentElement && document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight) : window.innerHeight)) - 20;
            var width = ((document.all ? (document.documentElement && document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body.clientWidth) : window.innerWidth));

            //            var obj = document.getElementById("ifMoreInfo");
            //            obj.style.height = height / 2 + "px";
            //obj.style.width = width/2 + "px";

            //alert(obj.style.height);

            var obj1 = document.getElementById("divgrid");
            obj1.style.height = (height - 50) / 2 + "px";
            //obj1.style.width = (width - 10) + "px";    

            var obj2 = document.getElementById("map");
            obj2.style.height = height / 2 + "px";
            //obj2.style.width = width / 2 + "px";  

            var obj3 = document.getElementById("divsummary");
            obj3.style.height = height / 2 + "px";
            //obj3.style.width = width / 2 + "px";
        }
    </script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Visible="false" />
    <div id="divVesselList">
        <table id="tblvessellist" width="100%" style="table-layout: fixed">
            <tr>
                <td valign="top" colspan="2">
                    <div style="border-width: 1px; border-style: groove; border-color: Navy; width: 100%;">
                        <div class="subHeader" style="position: relative">
                            <div id="divHeading">
                                <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="true" />
                                <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
                            </div>
                        </div>
                        <div class="navSelect" style="top: 4px; right: 2px; position: absolute;">
                            <eluc:TabStrip ID="MenuVesselList" runat="server" OnTabStripCommand="MenuVesselList_TabStripCommand"
                                TabStrip="true"></eluc:TabStrip>
                        </div>
                        <br />
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
                            </tr>
                        </table>
                        <br />
                        <div id="divgrid" style="width: 100%; overflow-x: auto; overflow-y: auto;">
                            <asp:GridView GridLines="None" ID="gvVesselList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" EnableViewState="false" BorderColor="Transparent"
                                OnRowDataBound="gvVesselList_RowDataBound" OnRowCommand="gvVesselList_RowCommand"
                                ShowHeader="true">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="false" />
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                            <asp:Label ID="lblVesselName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                            <asp:LinkButton ID="lnkVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'
                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="SELECT" Width="10%"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="false" />
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal><br />
                                            <asp:Literal ID="lblCode" runat="server" Text="Code"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>'
                                                Width="5%"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblLatitude" runat="server" Text="Latitude"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLat" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLATITUDE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblLongitude" runat="server" Text="Longitude"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLongitude" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGITUDE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblSpeed" runat="server" Text="Speed"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSpeed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPEED") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblVesselStatus" runat="server" Text="Vessel Status"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVesselStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELSTATUS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblNextPort" runat="server" Text="Next Port"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNextPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTPORTNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblETA" runat="server" Text="ETA"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEta" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETA", "{0:dd-MM-yyyy HH:mm}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="5px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Crew List" ImageUrl="<%$ PhoenixTheme:images/showall.png %>"
                                                CommandName="CREWLIST" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCrewList"
                                                ToolTip="Crew List"></asp:ImageButton>
                                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Summary" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                                CommandName="SUMMARY" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSummary"
                                                ToolTip="Summary"></asp:ImageButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--<div id="divPage" style="position: relative;">
                                    <table width="100%" border="0" class="datagrid_pagestyle">
                                        <tr>
                                            <td nowrap="nowrap" align="center">
                                                <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                                <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                                <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
                                            </td>
                                            <td nowrap="nowrap" align="left" width="50px">
                                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                            </td>
                                            <td width="20px">
                                                &nbsp;
                                            </td>
                                            <td nowrap="nowrap" align="right" width="50px">
                                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                            </td>
                                            <td nowrap="nowrap" align="center">
                                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
                                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                                    Width="40px"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </div>--%>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="50%" valign="top">
                    <div id="map" style="width: 100%;">
                    </div>
                </td>
                <td width="50%" valign="top">
                    <div id="divsummary" style="border-width: 1px; border-style: groove; border-color: Navy;
                        width: 100%; overflow-x: hidden;">
                        <div class="dashboard_section" style="position: relative">
                            <div class="subHeader" style="position: relative">
                                <eluc:Title runat="server" ID="ucSummaryList" Text="Summary" ShowMenu="false" />
                            </div>
                            <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                                <eluc:TabStrip ID="MenuSummary" runat="server" OnTabStripCommand="MenuSummary_TabStripCommand"
                                    TabStrip="true"></eluc:TabStrip>
                            </div>
                            <%--<div class="subHeader" style="position: relative">
                                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                                            <eluc:TabStrip ID="MenuSummary1" runat="server" OnTabStripCommand="MenuSummary1_TabStripCommand">
                                            </eluc:TabStrip>
                                        </div>
                                    </div>--%>
                        </div>
                        <iframe runat="server" id="ifMoreInfo" style="width: 100%; min-height: 325px; overflow-x: hidden;">
                        </iframe>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>
