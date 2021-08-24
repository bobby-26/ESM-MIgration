<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePersonalDetailsFrame.aspx.cs" Inherits="CrewOffshore_CrewOffshorePersonalDetailsFrame" %>

<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="../UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Sex" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaritalStatus" Src="../UserControls/UserControlMaritalStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OwnerPool" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirmation" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />
        <script type="text/javascript">
            function showBMI() {
                var bmi = document.getElementById("divBMI");
                bmi.style.display = "block";
            }
            function blink() {
                setInterval(function () {
                    var link = document.getElementById("lnkImportantRemarks");
                    if (link != null)
                        link.style.display = "none";
                    setTimeout(function () {
                        var link = document.getElementById("lnkImportantRemarks");
                        if (link != null)
                            link.style.display = "block";
                    }, 700);
                }, 1400);
            }

        </script>
        <style type="text/css">
            .buttonclass {
                width: 50px;
                min-width: 30px; /*this is used for minimum width to occupy*/
            }
        </style>
    </telerik:RadCodeBlock>

</head>
<body style="background-color:white">
    <form id="frmCrewMainPersonel" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />       
        <%--<eluc:TabStrip ID="CrewMainPersonal" runat="server" OnTabStripCommand="CrewMainPersonal_TabStripCommand" Title="Personal"></eluc:TabStrip>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" Font-Size="9px">
            <div class="gray-bg">
                <div class="row">
                    <div class="col-lg-12">
                        <table class="table table-bordered">                         
                           
                            <tr>
                                <td colspan="2">
                                    <table id="ti" class="table table-bordered" cellspacing="10" runat="server" >
                                          <tr class="gradeX" >
                                              <td colspan="2" rowspan="4" style="height: 100px; text-align: center; margin: 0 auto; clear: both; ">
                                                   <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                        Width="85px" />
                                              </td>
                                              <td style="background-color: aliceblue">First Name
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtFirstname" runat="server" EmptyMessage="First Name"></telerik:RadLabel>
                                            </td>
                                             
                                              </tr>
                                        <tr class="gradeX">
                                            
                                             <td style="background-color: aliceblue">Middle Name
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtMiddlename" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                         <tr class="gradeX">
                                             
                                            <td style="background-color: aliceblue">Last Name
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtLastname" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                       <%-- <tr class="gradeX">
                                             <td style="background-color: aliceblue">Last Contacted
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lbllastcontacted" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>--%>
                                         <tr class="gradeX">                                           
                                            <td style="background-color: aliceblue">Availability
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblavailability" runat="server"></telerik:RadLabel>
                                            </td>

                                        </tr>
                                        <tr class="gradeX">

                                            <td style="background-color: aliceblue">Created By
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtCreatedBy" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">Last Updated By
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtlastupdatedby" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>                                       
                                        <tr class="gradeX">

                                            <td style="background-color: aliceblue">File no
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtEmployeeCode" runat="server" EmptyMessage="File No."></telerik:RadLabel>

                                            </td>
                                            <td style="background-color: aliceblue">Pan No
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtPanNo" runat="server" EmptyMessage="Pan No"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">UID No
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtUidNo" runat="server" EmptyMessage="UID No"></telerik:RadLabel>

                                            </td>
                                            <td style="background-color: aliceblue">SIMS Batch
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="ucBatch" runat="server" EmptyMessage="UID No"></telerik:RadLabel>

                                              <%--  <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                                    Width="150px" />--%>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">INDOS No
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtINDOsNumber" runat="server" EmptyMessage="UID No"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">Manning Office
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="ucZone" runat="server" EmptyMessage=""></telerik:RadLabel>
                                                
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Rank
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="ddlRankPosted" runat="server" EmptyMessage="Status"></telerik:RadLabel>

                                                <%--<eluc:Rank ID="ddlRankPosted" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                                    Enabled="false" Width="150px" />  --%>                                              
                                              
                                            </td>
                                            <td style="background-color: aliceblue">Gender
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="ucSex" runat="server" EmptyMessage="Status"></telerik:RadLabel>

<%--                                                <eluc:Sex ID="ucSex" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                                    Width="150px" />--%>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Status
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtEmployeeStatus" runat="server" EmptyMessage="Status"></telerik:RadLabel>

                                            </td>
                                            <td style="background-color: aliceblue">Distinguish Mark
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtDistinguishMark" runat="server" EmptyMessage="Status"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">On board
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtLastVessel" runat="server" EmptyMessage="Last Vessel"></telerik:RadLabel>

                                            </td>
                                            <td style="background-color: aliceblue">Height(Cms)
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtHeight" runat="server" EmptyMessage="Last Vessel"></telerik:RadLabel>

                                               <%-- <eluc:Number ID="txtHeight" runat="server" CssClass="input_mandatory txtNumber" MaskText="###"
                                                    IsInteger="true" OnTextChangedEvent="CalculateBMI" AutoPostBack="true" Width="150px" />--%>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Signed on
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtdatesignon" runat="server" Text="Date Signed on"></telerik:RadLabel>

                                            </td>
                                            <td style="background-color: aliceblue">Weight(Kg)
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtWeight" runat="server" EmptyMessage="Last Vessel"></telerik:RadLabel>

                                              <%--  <eluc:Number ID="txtWeight" runat="server" MaskText="###"
                                                    ReadOnly="true" IsInteger="true" OnTextChangedEvent="CalculateBMI" AutoPostBack="true"
                                                    Width="150px" />--%>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Applied
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="ucRank" runat="server" EmptyMessage="Last Vessel"></telerik:RadLabel>

                                                <%-- <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                                    Enabled="false" Width="150px" />--%>

                                            </td>
                                            <td style="background-color: aliceblue">Eye Colour
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtEyeColor" runat="server"></telerik:RadLabel>

                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Applied on
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtAppliedOn" runat="server" />
                                            </td>
                                            <td style="background-color: aliceblue">Hair Colour
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtHairColor" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">First Join
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtDateofJoin" runat="server" />
                                            </td>
                                            <td style="background-color: aliceblue">Shoe Size(Inch)
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="ddlShoesSize" runat="server" />

                                             <%--   <telerik:RadComboBox DropDownPosition="Static" ID="ddlShoesSize" runat="server" EnableLoadOnDemand="True" Width="150px"
                                                    EmptyMessage="Type to select Shoe Size" Filter="Contains" MarkFirstMatch="true">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Value="" Text="--Select--" Selected="true" />
                                                        <telerik:RadComboBoxItem Value="6.00" Text="6" />
                                                        <telerik:RadComboBoxItem Value="7.00" Text="7" />
                                                        <telerik:RadComboBoxItem Value="8.00" Text="8" />
                                                        <telerik:RadComboBoxItem Value="9.00" Text="9" />
                                                        <telerik:RadComboBoxItem Value="10.00" Text="10" />
                                                        <telerik:RadComboBoxItem Value="11.00" Text="11" />
                                                        <telerik:RadComboBoxItem Value="12.00" Text="12" />
                                                    </Items>
                                                </telerik:RadComboBox>--%>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Civil Status
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="ucMaritialStatus" runat="server" />

                                             <%--   <eluc:MaritalStatus ID="ucMaritialStatus" runat="server" AppendDataBoundItems="true"
                                                    CssClass="dropdown_mandatory" Width="150px" />--%>
                                            </td>
                                            <td style="background-color: aliceblue">Shirt Size
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="ddlShirtSize" runat="server" />

                                              <%--  <telerik:RadComboBox DropDownPosition="Static" ID="ddlShirtSize" runat="server" EnableLoadOnDemand="True" Width="150px"
                                                    EmptyMessage="Type to select Shirt Size" Filter="Contains" MarkFirstMatch="true">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Value="" Text="--Select--" Selected="true" />
                                                        <telerik:RadComboBoxItem Value="S" Text="S" />
                                                        <telerik:RadComboBoxItem Value="M" Text="M" />
                                                        <telerik:RadComboBoxItem Value="L" Text="L" />
                                                        <telerik:RadComboBoxItem Value="XL" Text="XL" />
                                                        <telerik:RadComboBoxItem Value="XXL" Text="XXL" />
                                                    </Items>
                                                </telerik:RadComboBox>--%>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">DOB
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtDateofBirth" runat="server" />

                                            </td>
                                            <td style="background-color: aliceblue">BMI
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtBMI" runat="server" >
                                                </telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Age
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtAge" runat="server"></telerik:RadLabel>


                                            </td>
                                            <td style="background-color: aliceblue">Mentor
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtMentorName" runat="server"></telerik:RadLabel>

                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Place of Birth
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtPlaceofBirth" runat="server" EmptyMessage="Place of Birth"></telerik:RadLabel>



                                            </td>
                                            <td style="background-color: aliceblue">Nearest Airport
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtairport" Text="New col" runat="server"></telerik:RadLabel>

                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Nationality
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="ucNationality" Text="New col" runat="server"></telerik:RadLabel>

                                             <%--   <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                                    Width="150px" />--%>

                                            </td>
                                            <td style="background-color: aliceblue">Port of Engagement
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtportofEngage" Text="New col" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Religion
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtreligion" Text="New col" runat="server"></telerik:RadLabel>

                                            </td>
                                            <td style="background-color: aliceblue">Pool
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="ucPool" Text="New col" runat="server"></telerik:RadLabel>
                                              
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>                       

                     
                        <div id="divBMI" runat="server" style="position: absolute; display: none; top: 0px; left: 0px; z-index: 10; background-color: White"
                            onmouseout="javascript:this.style.display='none';">
                        </div>
                    </div>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
   <%-- <script type="text/javascript" src="../Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.v3.js"></script>
    <script type="text/javascript" src="../Scripts/dashboard.js"></script>--%>
</body>
</html>
