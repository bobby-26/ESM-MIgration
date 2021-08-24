<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePersonalGeneralOverview.aspx.cs" Inherits="CrewOffshore_CrewOffshorePersonalGeneralOverview" %>

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

        <style type="text/css">
            .mlabel {
                color: #FFFFFF !important;
            }
        </style>


    </telerik:RadCodeBlock>

</head>


<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <asp:Button ID="btnCrewList" runat="server" CssClass="hidden" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="gray-bg">
                <%-- <div class="row page-heading bg-success">

                <div class="col-lg-9">
                    <h2 class="font-bold">Dashboard</h2>
                </div>
                <div class="col-lg-3">
                    <br />
                    <button class="btn btn-primary " type="button"><i class="fa fa-attachments"></i>&nbsp;CV</button>
                    <button class="btn btn-primary " type="button"><i class="fa fa-adjust"></i>&nbsp;Propose</button>                
                </div>
            </div>--%>
                <div class="row">

                    <div class="col-lg-12">


                        <div class="tab-content">
                            <div id="tab-1" class="tab-pane active">
                                <div class="panel-body">

                                    <div class="col-lg-5">

                                        <div class="panel panel-success" style="height: 750px;">
                                            <div class="panel-heading">
                                                <a id="aPersonal" runat="server" class="text-primary" style="color: white; text-decoration: underline">Personal Details</a>
                                                <asp:Button ID="btncv" Text="CV" runat="server" Font-Size="8px" class="btn btn-primary"></asp:Button>
                                                <asp:Button ID="btnpropose" Text="PROPOSE" runat="server" Font-Size="8px" CssClass="btn btn-primary"></asp:Button>
                                                <asp:Button ID="btnupdate" Text="UPDATE" runat="server" Font-Size="8px" CssClass="btn btn-primary" />
                                                <asp:Button ID="btnpdemail" Text="PD Email" runat="server" Font-Size="8px" CssClass="btn btn-primary" />

                                            </div>
                                            <iframe id="personaliframe" runat="server" style="width: 100%; height: 95%; border: none;" scrolling="no"></iframe>
                                        </div>
                                        <div class="panel panel-success" style="height:400px;">
                                            <div class="panel-heading">
                                                <a id="aAdd" runat="server" class="text-primary" style="color: white; text-decoration: underline">Address</a>
                                                <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                                                <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                                                    RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                                                    HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true"
                                                    Text="Note: For landline number, exclude the '0' before the 'Area Code'. For Mobile number, exclude the '0' before the number.use ',' to add more E-Mail Eg: (xx@xx.com, yy@yy.com)">
                                                </telerik:RadToolTip>
                                            </div>
                                         
                                            <iframe id="AddressFrame" runat="server" style="width: 100%; height:85%; border: none;"></iframe>
                                            
                                        </div>
                                    </div>

                                    <div class="col-lg-7">
                                        <div class="panel panel-success" style="height: 450px;">
                                            <div class="panel-heading">
                                                <a id="adoc" runat="server" class="text-primary" style="color: white; text-decoration: underline">Documents</a>
                                            </div>
                                            <div class="panel-body" style="height: 100%">
                                                <iframe id="DocumentsFrame" runat="server" style="width: 100%; height: 95%; border: none;" scrolling="no"></iframe>
                                            </div>
                                        </div>
                                        <div class="panel panel-success" style="height: 500px;">
                                            <div class="panel-heading">
                                                <a id="aAppraisal" runat="server" class="text-primary" style="color: white; text-decoration: underline">Appraisals</a>
                                            </div>
                                            <div class="panel-body" style="height: 100%">
                                                <iframe id="Appraisalframe" runat="server" style="width: 100%; height: 95%; border: none;"></iframe>
                                            </div>
                                        </div>
                                        <div class="panel panel-success" style="height: 180px;">
                                            <div class="panel-heading">
                                                <a id="atraining" runat="server" class="text-primary" style="color: white; text-decoration: underline">Training</a>
                                            </div>
                                            <div class="panel-body" style="height: 100%">
                                                <iframe id="TrainingFrame" runat="server" style="width: 100%; height: 85%; border: none;"></iframe>
                                            </div>
                                        </div>

                                    </div>
                                    <%--  <div class="col-lg-4">
                                        <div class="panel panel-success" style="height: 1000px;">
                                            <div class="panel-heading">
                                                <a id="aCourse" runat="server" class="text-primary" style="color: white; text-decoration: underline">STCW & Other Documents</a>
                                            </div>
                                            <div class="panel-body" style="height: 100%">
                                                <iframe id="CourseFrame" runat="server" style="width: 100%; height: 95%; border: none;"></iframe>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <div class="col-lg-12">
                                        <div class="panel panel-success" style="height: 500px;">
                                            <div class="panel-heading">
                                                <a id="aexp" runat="server" class="text-primary" style="color: white; text-decoration: underline">Experience</a>

                                            </div>
                                            <div class="panel-body" style="height: 100%">
                                                <iframe id="ExpFrame" runat="server" style="width: 100%; height: 95%; border: none;" scrolling="no"></iframe>
                                            </div>
                                        </div>
                                    </div>
                                </div>



                                <%--<div class="col-lg-4">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                Vessel Particulars
                                            </div>

                                            <div class="panel-body">

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

                                    </div>--%>


                                <%--<div class="col-lg-4">

                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                Communication
                                            </div>

                                            <div class="panel-body" style="height: 430px;">

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

                                            <div class="panel-body" style="height: 310px;">

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

                                    </div>--%>
                            </div>
                        </div>



                    </div>
                </div>

            </div>

        </telerik:RadAjaxPanel>
    </form>

    <script type="text/javascript" src="../Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.v3.js"></script>
    <script type="text/javascript" src="../Scripts/dashboard.js"></script>
</body>
</html>
