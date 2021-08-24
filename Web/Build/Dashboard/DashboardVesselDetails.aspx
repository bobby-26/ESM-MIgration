<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardVesselDetails.aspx.cs" Inherits="Dashboard_DashboardVesselDetails" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />
        <script type="text/javascript">
            (function (global, undefined) {
                var demo = {};

                function OnClientFilesUploaded(sender, args) {
                    $find('RadAjaxManager').ajaxRequest();
                }

                
                
                global.OnClientFilesUploaded = OnClientFilesUploaded;
                
            })(window);
            function OnClientAdded(sender, args) {
                var allowedMimeTypes = $telerik.$(sender.get_element()).attr("data-clientFilter");
                $telerik.$(args.get_row()).find(".ruFileInput").attr("accept", allowedMimeTypes);
            }
        </script>
        <style type="text/css">
            .mlabel {
                color: #FFFFFF !important;
            }
            .ruButton.ruBrowse
            {
                height:15px !important;
                width:90px !important;
                font-size:11px !important;
                padding:0px 0px !important;
                background-repeat: no-repeat;
                background-position: 5px 5px;
            }

        </style>


    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <asp:Button ID="btnCrewList" runat="server" CssClass="hidden" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--<telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>--%>
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server" EnablePageHeadUpdate="false">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="imgPhoto" />
                        <telerik:AjaxUpdatedControl ControlID="radImageupload" />
                        
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <div class="gray-bg">
            <%-- <div class="row page-heading bg-success">

                <div class="col-lg-9">
                    <h2 class="font-bold">Dashboard</h2>
                </div>
                <div class="col-lg-3">
                    <br />
                    <button class="btn btn-primary " type="button"><i class="fa fa-anchor"></i>&nbsp;File Management</button>
                    <button class="btn btn-primary " type="button"><i class="fa fa-adjust"></i>&nbsp;External Link</button>
                    <button class="btn btn-primary " type="button"><i class="fa fa-dashboard"></i>&nbsp;KPI'S</button>
                </div>
            </div>--%>
            <div class="row">

                <div class="col-lg-12">
                    <div class="tabs-container">
                        <ul class="nav nav-tabs">
                            <li class="active"><a data-toggle="tab" href="#tab-1">Overview</a></li>
                            <li class=""><a data-toggle="tab" href="#tab-2">Details</a></li>
                        </ul>

                        <div class="tab-content">
                            <div id="tab-1" class="tab-pane active">
                                <div class="panel-body">

                                    <div class="col-lg-4">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                <table style="width:100%">
                                                    <tr>
                                                        <td style="width:75%">
                                                            <telerik:RadLabel ID="lblVesselName" runat="server" CssClass="mlabel" Text=""></telerik:RadLabel>
                                                        </td>
                                                        <td style="text-align:right;">
                                                            <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="radImageupload" MaxFileInputsCount="1"  AllowedFileExtensions="png" 
                                                                CssClass="RadAsyncUpload1" data-clientFilter="image/png" OnClientAdded="OnClientAdded"
                                                                OnFileUploaded="radImageupload_FileUploaded" OnClientFilesUploaded="OnClientFilesUploaded" HideFileInput="true"
                                                                Localization-Select="Upload">
                                                            </telerik:RadAsyncUpload>
                                                        </td>
                                                    </tr>
                                                </table>
                                                
                                                
                                            </div>
                                            <div class="panel-body" style="height: 200px; text-align: center; margin: 0 auto; clear: both;">
                                                <%--<asp:Image ID="imgPhoto" runat="server" Height="180px" Width="380px" />--%>
                                                <telerik:RadImageGallery RenderMode="Lightweight" runat="server" ID="imgPhoto" Height="180px" DisplayAreaMode="Image">
                                                    <Items>
                                                        </Items>
                                                    <ImageAreaSettings ShowDescriptionBox="false" NavigationMode="Button" ResizeMode="Fill" ShowNextPrevImageButtons="false"   />
                                                    <ToolbarSettings ShowSlideshowButton="false" ShowThumbnailsToggleButton="false" ShowItemsCounter="false" Position="BottomInside" />
                                                    <ThumbnailsAreaSettings Mode="ImageSlider"/>
                                                    </telerik:RadImageGallery>
                                                

                                            </div>
                                        </div>
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Key Details
                                            </div>
                                            <div class="panel-body" style="height: 594px;">

                                                <table class="table table-bordered">

                                                    <tbody>

                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Master</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblMasterName" runat="server" Text=""></telerik:RadLabel>
                                                                <%--<telerik:RadLinkButton ID="lnkMasterName" runat="server" Text=""></telerik:RadLinkButton>--%>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Chief Engineer</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblChiefEngineer" runat="server" Text=""></telerik:RadLabel>
                                                                <%--<telerik:RadLinkButton ID="lnkChiefEngineer" runat="server" Text=""></telerik:RadLinkButton>--%>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Last Port</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblLastPort" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Next Port</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblNextPort" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">ETA</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblETA" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Last Reported Position</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblLatLog" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Speed in Kts / Fuel Cons MT</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblKts" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Cargo onboard</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblCargo" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>

                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Last PSC Inspection</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpscinspection" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Last SIRE Inspection</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblsireinspection" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Last Dry Dock</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbldrydock" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Last Export  No / Date</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbllastexport" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Last Import  No/ Date</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbllastimport" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Spare Life Boat Capacity</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblsparelifeboat" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">POB - Crew / Supernumeraries</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblPOB" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>

                                                    </tbody>


                                                </table>

                                            </div>
                                        </div>
                                    </div>


                                    <div class="col-lg-4">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Vessel Particulars
                                            </div>

                                            <div class="panel-body" style="height: 840px;">

                                                <table class="table table-bordered">

                                                    <tbody>

                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Fleet / Fleet Manager</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblfleetmanager" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Technical Suptd</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbltechsuptd" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>

                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Type </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblvesseltype" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Hull No. </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblhullnum" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">IMO Number </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblimonumber" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Official Number</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbloffnum" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Call Sign </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblcallsign" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">MMSI No. </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblmmsino" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Vessel Short Code </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblvesselcode" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Port of Registry </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblregisterport" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Flag </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblflag" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Disponent Owner </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbldisponentowner" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Owner </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblowner" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Charterer </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblCharterer" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Principal </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblprincipal" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Manager  </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblmanager" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">P & I Club</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpni" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">H & M</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblhnm" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Classification </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblclassification" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Class No </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblclassno" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Class Notation  </td>
                                                            <td colspan="3">
                                                                <telerik:RadLabel ID="lblclassnotation" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Keel Laid </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblkeellaid" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Launched . </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbllaunched" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Delivery  </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbldelivery" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Management Type</td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblmgmnttype" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Navigation Area </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblnavarea" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Ice Class </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbliceclass" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Builder  </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblbuilder" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Fitted with Framo </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblfittedframo" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Takeover </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblesmtakeover" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Handover </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblesmhandover" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>


                                                    </tbody>


                                                </table>

                                            </div>
                                        </div>

                                    </div>


                                    <div class="col-lg-4">

                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Communication
                                            </div>

                                            <div class="panel-body" style="height: 400px;">

                                                <table class="table table-bordered">

                                                    <tbody>

                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">SAT B Phone </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblsatbphone" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">SAT C Telex  </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblsatcTelex" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">SAT B Fax  </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblsatbfax" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">VSAT Phone </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblvsatphone" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Fleet77 Phone  </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblfleet77phone" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">VSAT Fax  </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblvsatfax" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">Fleet77 Fax  </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblfleet77fax" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">E-mail </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblemail" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">FBB Phone </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblfbbphone" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Notification E-mail </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblNotifyEmail" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td style="background-color: aliceblue">FBB Fax  </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblfbbfax" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                            <td style="background-color: aliceblue">Mobile Number  </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblmobilenumber" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>


                                                    </tbody>


                                                </table>

                                            </div>
                                        </div>

                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Reports
                                            </div>

                                            <div class="panel-body" style="height: 388px;">

                                                <table class="table table-bordered">


                                                    <tbody>

                                                        <tr class="gradeX">
                                                            <td><a id="btnAEPerformance" runat="server">A/E Performance</a>   </td>
                                                            <td><a id="btnArrivalReport" runat="server">Arrival Report</a></td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td><a id="btncommercialPerformance" runat="server">Commercial Performance</a>   </td>
                                                            <td><a id="btnDepartureReport" runat="server">Departure Report</a></td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td><a id="btnExceptionReport" runat="server">Exception Report</a>   </td>
                                                            <td><a id="btnInitialization" runat="server">Initialization</a>   </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td><a id="btnMachineryParameters" runat="server">Machinery Parameters</a>  </td>
                                                            <td><a id="btnMonthlyreport" runat="server">Monthly report</a>  </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td><a id="btnNoonReport" runat="server">Noon Report</a>  </td>
                                                            <td><a id="btnQuarterlyReport" runat="server">Quarterly Report</a>  </td>
                                                        </tr>
                                                        <tr class="gradeX">
                                                            <td><a id="btnShiftingreport" runat="server">Shifting report</a> </td>
                                                            <td><a id="btnVoyagereport" runat="server">Voyage report</a> </td>
                                                        </tr>
                                                    </tbody>


                                                </table>

                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>

                            <div id="tab-2" class="tab-pane">
                                <div class="panel-body">

                                    <div class="col-lg-12">

                                        <div class="col-lg-4">
                                            <div class="panel panel-success">

                                                <div class="panel-heading">
                                                    Navigation Equipment
                                                </div>

                                                <div class="panel-body" style="height: 945px;overflow-y:auto">

                                                    <table class="table table-bordered">

                                                        <thead>
                                                            <tr>
                                                                <td></td>
                                                                <td>Make</td>
                                                                <td>Model</td>
                                                            </tr>
                                                        </thead>
                                                        <tbody><telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                                            <%
                                                                for (int i = 0; i < navigationdt.Rows.Count; i++)
                                                                {
                                                            %>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue"><%= navigationdt.Rows[i]["FLDCOMPONENTNAME"].ToString() %></td>
                                                                <td><%= navigationdt.Rows[i]["FLDMAKER"].ToString() %></td>
                                                                <td><%=  navigationdt.Rows[i]["FLDTYPE"].ToString() %></td>
                                                            </tr>
                                                            <%
                                                                }
                                                            %>
                                                            </telerik:RadCodeBlock>
                                                        </tbody>
                                                    </table>

                                                </div>
                                            </div>





                                        </div>


                                        <div class="col-lg-4">


                                            <div class="panel panel-success">

                                                <div class="panel-heading">
                                                    Main Engines
                                                </div>

                                                <div class="panel-body" style="height: 450px;overflow-y:auto">

                                                    <table class="table table-bordered">

                                                        <tbody>

                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Qty </td>
                                                                <td><telerik:RadLabel ID="lblmeQty" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">Type </td>
                                                                <td><telerik:RadLabel ID="lblmeType" runat="server"></telerik:RadLabel></td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Make </td>
                                                                <td><telerik:RadLabel ID="lblmeMake" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">Model</td>
                                                                <td><telerik:RadLabel ID="lblmeModel" runat="server"></telerik:RadLabel></td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">NCR BHP </td>
                                                                <td><telerik:RadLabel ID="lblmeNcrBhp" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">NCR KW </td>
                                                                <td><telerik:RadLabel ID="lblmeNcrKw" runat="server"></telerik:RadLabel></td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">NCR F.O. Cons MT/Day </td>
                                                                <td><telerik:RadLabel ID="lblmeNcrfo" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">SFOC g/kWh </td>
                                                                <td><telerik:RadLabel ID="lblmeSfoc" runat="server"></telerik:RadLabel></td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">MCR BHP</td>
                                                                <td><telerik:RadLabel ID="lblmeMcrBhp" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">MCR KW</td>
                                                                <td><telerik:RadLabel ID="lblmeMcrKw" runat="server"></telerik:RadLabel></td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">MCR RPM </td>
                                                                <td><telerik:RadLabel ID="lblmeMcrRpm" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">MCR Speed </td>
                                                                <td><telerik:RadLabel ID="lblmeMcrSpeed" runat="server"></telerik:RadLabel></td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">MCR F.O. Cons MT/Day </td>
                                                                <td><telerik:RadLabel ID="lblmeMcrfo" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">SFOC g/kWh  </td>
                                                                <td><telerik:RadLabel ID="lblmeSfocKw" runat="server"></telerik:RadLabel></td>
                                                            </tr>


                                                        </tbody>


                                                    </table>

                                                </div>
                                            </div>


                                            <div class="panel panel-success">

                                                <div class="panel-heading">
                                                    AUX Engines
                                                </div>

                                                <div class="panel-body" style="height: 250px;overflow-y:auto">

                                                    <table class="table table-bordered">

                                                        <tbody>

                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Qty </td>
                                                                <td><telerik:RadLabel ID="lblaeQty" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">Type </td>
                                                                <td><telerik:RadLabel ID="lblaeType" runat="server"></telerik:RadLabel></td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Make </td>
                                                                <td><telerik:RadLabel ID="lblaeMake" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">Model</td>
                                                                <td><telerik:RadLabel ID="lblaeModel" runat="server"></telerik:RadLabel></td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Output KW </td>
                                                                <td><telerik:RadLabel ID="lblaeOutput" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">RPM </td>
                                                                <td><telerik:RadLabel ID="lblaeRpm" runat="server"></telerik:RadLabel></td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">F.O. Cons MT/Day </td>
                                                                <td><telerik:RadLabel ID="lblaeFoCons" runat="server"></telerik:RadLabel></td>
                                                                <td style="background-color: aliceblue">SFOC g/kWh </td>
                                                                <td><telerik:RadLabel ID="lblaeSfoc" runat="server"></telerik:RadLabel></td>
                                                            </tr>


                                                        </tbody>


                                                    </table>

                                                </div>
                                            </div>
                                            <div class="panel panel-success">

                                                <div class="panel-heading">
                                                    Tonnages
                                                </div>

                                                <div class="panel-body" style="height: 150px;overflow-y:auto">

                                                    <table class="table table-bordered">

                                                        <thead>
                                                            <tr>
                                                                <td></td>
                                                                <td>Register</td>
                                                                <td>Suez</td>
                                                                <td>Panama</td>
                                                            </tr>
                                                        </thead>

                                                        <tbody>

                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Gross Tonnage </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblRegisteredGT" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblSuezGT" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblPanamaGT" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Net Tonnage </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblRegisteredNT" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblSuezNT" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblPanamaNT" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>

                                                        </tbody>


                                                    </table>

                                                </div>
                                            </div>


                                        </div>


                                        <div class="col-lg-4">



                                            <div class="panel panel-success">

                                                <div class="panel-heading">
                                                    Other Equipment
                                                </div>

                                                <div class="panel-body" style="height: 450px;overflow-y:auto">

                                                    <table class="table table-bordered">

                                                        <thead>
                                                            <tr>
                                                                <td></td>
                                                                <td>Make</td>
                                                                <td>Model</td>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <telerik:RadCodeBlock runat="server" ID="RadCodeBlock3">
                                                            <%
                                                                for (int i = 0; i < otherdt.Rows.Count; i++)
                                                                {
                                                                 %>
                                                             <tr class="gradeX">
                                                                <td style="background-color: aliceblue"><%= otherdt.Rows[i]["FLDCOMPONENTNAME"].ToString() %></td>
                                                                <td><%= otherdt.Rows[i]["FLDMAKER"].ToString() %></td>
                                                                <td><%= otherdt.Rows[i]["FLDTYPE"].ToString() %></td>
                                                            </tr>
                                                            <%
                                                                } %>
                                                                </telerik:RadCodeBlock>
                                                        </tbody>
                                                    </table>

                                                </div>
                                            </div>


                                            <div class="panel panel-success">

                                                <div class="panel-heading">
                                                    Load Line
                                                </div>

                                                <div class="panel-body" style="height: 450px;overflow-y:auto">

                                                    <table class="table table-bordered">

                                                        <thead>
                                                            <tr>
                                                                <td></td>
                                                                <td>Freeboard (m)</td>
                                                                <td>Draft (m)</td>
                                                                <td>DWT (mt)</td>
                                                            </tr>
                                                        </thead>

                                                        <tbody>

                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Tropical  </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel ID="lblFreeboardTropical" runat="server" Text=""></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel ID="lblDraftTropical" runat="server" Text=""></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel ID="lblDWTTropical" runat="server" Text=""></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Summer  </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblFreeboardSummer" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblDraftSummer" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblDWTSummer" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Winter  </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblFreeboardWinter" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblDraftWinter" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblDWTWinter" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Lightship </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblFreeboardLightship" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblDraftLightship" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblDWTLightship" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                            <tr class="gradeX">
                                                                <td style="background-color: aliceblue">Ballast Cond </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblFreeboardBallastCond" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblDraftBallastCond" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                                <td style="text-align: right">
                                                                    <telerik:RadLabel runat="server" ID="lblDWTBallastCond" Width="70px" CssClass="input" ReadOnly="true"></telerik:RadLabel>
                                                                </td>
                                                            </tr>

                                                        </tbody>


                                                    </table>

                                                </div>
                                            </div>




                                        </div>



                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        
    </form>
    <script type="text/javascript" src="../Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.v3.js"></script>
    <script type="text/javascript" src="../Scripts/dashboard.js"></script>
</body>
</html>
