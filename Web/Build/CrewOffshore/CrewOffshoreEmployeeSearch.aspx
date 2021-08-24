<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreEmployeeSearch.aspx.cs"
    Inherits="CrewOffshoreEmployeeSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RankList" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ZoneList" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselTypeList" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="../UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EngineType" Src="~/UserControls/UserControlEngineType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentType" Src="~/UserControls/UserControlDocumentType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Query Activity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function resize() {
                var obj = document.getElementById("divContainer");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 60 + "px";

            }
            function radioMe(e) {
                if (!e) e = window.event;
                var sender = e.target || e.srcElement;

                if (sender.nodeName != 'INPUT') return;
                var checker = sender;
                var chkBox = document.getElementById('<%= cblHasMissing.ClientID %>');
                var chks = chkBox.getElementsByTagName('INPUT');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i] != checker)
                        chks[i].checked = false;
                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body onload="resize()" onresize="resize()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


                <eluc:TabStrip ID="CrewQuery" runat="server" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>


                <eluc:TabStrip ID="CrewQueryGeneral" runat="server" OnTabStripCommand="CrewQueryGeneral_TabStripCommand"></eluc:TabStrip>

                <div id="divContainer">
                    <table width="80%" cellpadding="2" cellspacing="2">
                        <tr>
                            <td colspan="2">
                                <b>
                                    <telerik:RadLabel ID="lblNameSearch" runat="server" Text="Name Search"></telerik:RadLabel>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFullName1" runat="server" Text="Full Name Contains(1st)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFullName1" runat="server" MaxLength="200" Width="250px"></telerik:RadTextBox>
                            </td>                  
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFullName2" runat="server" Text="Full Name Contains(2nd)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFullName2" runat="server" MaxLength="200" Width="250px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <b>
                                    <telerik:RadLabel ID="lblSaveSearch" runat="server" Text="Save Search Criteria"></telerik:RadLabel>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSearchName" runat="server" Text="Search Title"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSearchName" runat="server" MaxLength="200" Width="250px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSavedSearch" runat="server" Text="Saved Search"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlSavedSearch" runat="server" DataTextField="FLDSEARCHNAME" Filter="Contains"
                                    EmptyMessage="Type to select saved name" MarkFirstMatch="true"
                                    DataValueField="FLDSEARCHID" AutoPostBack="true" OnTextChanged="ddlSavedSearch_TextChanged" Width="250px">
                                </telerik:RadComboBox>
                                <telerik:RadButton ID="btnSearchDelete" runat="server" Text="Delete Search" OnClick="btnSearchDelete_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <b>
                                    <telerik:RadLabel ID="lblOtherSearch" runat="server" Text="Other Search Criteria"></telerik:RadLabel>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblColumn" runat="server" Text="Display Column"></telerik:RadLabel>
                            </td>
                            <td>                         
                                <telerik:RadComboBox ID="ddlColumnlist" runat="server" EmptyMessage="Type to select column" Filter="Contains" 
                                        MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="250px" >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Name" Value="Name" />
                                        <telerik:RadComboBoxItem Text="Rank" Value="Rank" />
                                        <telerik:RadComboBoxItem Text="File No." Value="FileNo" />
                                        <telerik:RadComboBoxItem Text="Status" Value="Status" />
                                        <telerik:RadComboBoxItem Text="Present Vessel" Value="PresentVessel" />
                                        <telerik:RadComboBoxItem Text="Planned Vessel" Value="PlnVessel" />
                                        <telerik:RadComboBoxItem Text="Last Vessel" Value="LstVessel" />
                                        <telerik:RadComboBoxItem Text="Last Sign-Off Date" Value="LstSignOffDate" />
                                        <telerik:RadComboBoxItem Text="DOA" Value="DOA" />
                                      <%--  <telerik:RadComboBoxItem Text="DOB" Value="DOB" />                                        
                                        <telerik:RadComboBoxItem Text="Reporting Office" Value="Zone" />--%>
                                    </Items>
                                  </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPassportNo" runat="server" Text="Passport No. Contains"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPassportNo" runat="server" MaxLength="50" Width="250px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCurrentRank" runat="server" Text="Current Rank"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:RankList ID="lstRank" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCurrentZone" runat="server" Text="Reporting Office"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:ZoneList ID="lstZone" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCurrentStatus" runat="server" Text="Current Status"></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<asp:DropDownList ID="ddlStatus" runat="server"  
                                DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" Width="250px"></asp:DropDownList>      --%>
                                <eluc:Hard ID="ucCurrentStatus" runat="server" AppendDataBoundItems="true"
                                    HardTypeCode="54" ShortNameFilter="ONB,ONL,APR,REJ,TCL,NEW,TLG,RFS" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCurrentStage" runat="server" Text="Current Stage"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard ID="ucCurrentStage" runat="server" AppendDataBoundItems="true"
                                    HardTypeCode="99" ShortNameFilter="AWA,APV,AFT,AFS,TLG" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Current Ship"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Current Vessel Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:VesselType ID="ddlVesselType" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCurrentTechnicalGroup" runat="server" Text="Current Technical Group"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Fleet runat="server" ID="ucTechFleet" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPlannedShipOwnedBy" runat="server" Text="Current/ Planned Ship Owned By"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:AddressType runat="server" ID="ddlPlannedShipOwnedBy" AddressType="128" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPlannedShipManagedBy" runat="server" Text="Current/ Planned Ship Managed By"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:AddressType runat="server" ID="ddlPlannedShipManagedBy" AddressType="126" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPlannedShip" runat="server" Text="Planned for Ship"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ddlPlannedVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPlannedVesselType" runat="server" Text="Planned for Vessel Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:VesselType ID="ddlPlannedVesselType" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text="Employee ID"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFileNo" runat="server" MaxLength="50" Width="250px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAvailableFrom" runat="server" Text="Available From"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtAvailableFrom" runat="server" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Nationality ID="ddlNationality" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag of Current Ship"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Flag ID="ddlFlag" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblExperienceShipType" runat="server" Text="Experience - Ship Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:VesselTypeList ID="lstVesselType" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblExperienceRank" runat="server" Text="Experience - Rank"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:RankList ID="lstRankList" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblExperienceShip" runat="server" Text="Experience - Ship"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ddlExperienceShip" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblExperienceFrom" runat="server" Text="Experience - From"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtExperienceFrom" runat="server" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblExperienceTo" runat="server" Text="Experience - To"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtExperienceTo" runat="server" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblExperienceEngine" runat="server" Text="Experience - Engine"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:EngineType ID="ddlEngineType" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblStartingRank" runat="server" Text="Starting Rank (with company)"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSignedOffShip" runat="server" Text="Signed Off - Ship"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ddlSignedOff" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblShipOwnedBy" runat="server" Text="Signed Off - Ship Owned By"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:AddressType runat="server" ID="ddlSignedOffShipOwnedBy" AddressType="128" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSignOffTechGroup" runat="server" Text="Signed Off Ship Technical Group"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Fleet runat="server" ID="ucSignOffTechGroup" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSignedOffReason" runat="server" Text="Signed Off - Reason"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:SignOffReason ID="ddlSignOffReason" runat="server" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSignedOffMonths" runat="server" Text="Signed Off - Months Since Leaving Ship"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlSignedOffMonths" runat="server" Width="250px" MarkFirstMatch="true" Filter="Contains" EmptyMessage="Type to select Signed Off - Months">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="1" Text="1" />
                                        <telerik:RadComboBoxItem Value="2" Text="2" />
                                        <telerik:RadComboBoxItem Value="3" Text="3" />
                                        <telerik:RadComboBoxItem Value="4" Text="4" />
                                        <telerik:RadComboBoxItem Value="5" Text="5" />
                                        <telerik:RadComboBoxItem Value="6" Text="6" />
                                        <telerik:RadComboBoxItem Value="7" Text="7" />
                                        <telerik:RadComboBoxItem Value="8" Text="8" />
                                        <telerik:RadComboBoxItem Value="9" Text="9" />
                                        <telerik:RadComboBoxItem Value="10" Text="10" />
                                    </Items>

                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTrainingCourse" runat="server" Text="Pre-Sea Training Course"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Course ID="ddlCourse" runat="server" CourseList="<%# PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse() %>"
                                    ListCBTCourse="false" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTrainingInstitute" runat="server" Text="Pre-Sea Training Institute"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:AddressType runat="server" ID="ucInstitution" AppendDataBoundItems="true"
                                    AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>' Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCOC" runat="server" Text="COC"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Documents ID="ddlLicence" runat="server" DocumentList="<%# PhoenixRegistersDocumentLicence.ListDocumentLicence(null,null,1,null,null) %>"
                                    AppendDataBoundItems="true" DocumentType="LICENCE" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFlagofCOC" runat="server" Text="Flag of COC"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Country ID="ddlFlagcoc" runat="server" AppendDataBoundItems="true"
                                    CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblNewapplicantID" runat="server" Text="New applicant ID"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtNewapplicantID" runat="server" Width="250px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAddress" runat="server" Text="Address Name Contains"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAddress" runat="server" Width="250px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPhoneNumber" runat="server" Text="Phone Number Contains"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPhoneNumber" runat="server" Width="250px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Mobile Number Contains"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtMobileNumber" runat="server" Width="250px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPreviousShip" runat="server" Text="Previous Ship Contains"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPreviousShip" runat="server" Width="250px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOwnerName" runat="server" Text="Owner Name Contains"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtOwnerName" runat="server" Width="250px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVisaheld" runat="server" Text="Visa held"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Country ID="ucCountryVisaheld" runat="server" AppendDataBoundItems="true" />
                                <%--  <eluc:DocumentType ID="ucDocumentType" runat="server" DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString())%>'
                                         AppendDataBoundItems="true" Width="250px" />    --%>    
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbleocnod" runat="server" Text="Expiry of Contract - No Of Days"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddleocnod" runat="server" Width="250px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select no.of days">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="5" Text="5" />
                                        <telerik:RadComboBoxItem Value="10" Text="10" />
                                        <telerik:RadComboBoxItem Value="15" Text="15" />
                                        <telerik:RadComboBoxItem Value="20" Text="20" />
                                        <telerik:RadComboBoxItem Value="25" Text="25" />
                                        <telerik:RadComboBoxItem Value="30" Text="30" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblExpiryCOC" runat="server" Text="Expiry COC"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Documents ID="ddlExpiryCOC" runat="server" DocumentList="<%# PhoenixRegistersDocumentLicence.ListDocumentLicence(null,null,1,null,null) %>"
                                    AppendDataBoundItems="true" DocumentType="LICENCE" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblExpiryCourse" runat="server" Text="Expiry Training Course"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Course ID="ddlExpiryCourse" runat="server" CourseList="<%# PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse() %>"
                                    ListCBTCourse="false" AppendDataBoundItems="true" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblExpiryDocument" runat="server" Text="Expiry Document"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:DocumentType ID="ucExpiryDocument" runat="server" AppendDataBoundItems="true" DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString())%>'
                                    Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMinimumAge" runat="server" Text="Minimum Age"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="txtMinimumAge" runat="server" Width="80px" IsInteger="true" IsPositive="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMaximumAge" runat="server" Text="Maximum Age"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="txtMaximumAge" runat="server" Width="80px" IsInteger="true" IsPositive="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTrainingNeeds" runat="server" Text="Training Needs"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Course ID="ddlTrainingNeeds" runat="server" CourseList="<%# PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse() %>"
                                    ListCBTCourse="false" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="Literal1" runat="server" Text="Has Outstanding Training Needs"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkTrainingNeeds" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMedicalTest" runat="server" Text="Drug & Alcohol Test"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Documents runat="server" ID="ddlMedicalTest"
                                    AppendDataBoundItems="true" DocumentList='<%# PhoenixRegistersDocumentMedical.ListDocumentMedical() %>'
                                    DocumentType="MEDICAL" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMedicalTestFrom" runat="server" Text="Drug & Alcohol Test From"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtMedicalTestFrom" runat="server" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMedicalTestTo" runat="server" Text="Drug & Alcohol Test To"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtMedicalTestTo" runat="server" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblHasEmailAddress" runat="server" Text="Has Email Address"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkHasEmailAddress" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblGeneralRemark" runat="server" Text="Has General Remark"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkGeneralRemark" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblIsNotActive" runat="server" Text="Is Not Active"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkIsNotActive" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblNotActiveFrom" runat="server" Text="Not Active From"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtNotActiveFrom" runat="server" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblNotActiveReason" runat="server" Text="Not Active Reason"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard ID="ddlInactiveReason" runat="server" AppendDataBoundItems="true" HardTypeCode="53"
                                    ShortNameFilter="LFT,DTH,EXM,TSP,TTO" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblIsNTBE" runat="server" Text="Is NTBE"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkIsNTBE" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblShowRecord" runat="server" Text="Show Record Number in List"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="txtShowRecord" runat="server" Width="80px" IsInteger="true" IsPositive="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOrderby1" runat="server" Text="Order by (1st)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlOrderby1" runat="server" Width="250px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Order by (1st)">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="FLDDOA" Text="Available Date" />
                                        <telerik:RadComboBoxItem Value="FLDRANKNAME" Text="Rank Name" />
                                        <telerik:RadComboBoxItem Value="FLDNAME" Text="Name" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOrderby2" runat="server" Text="Order by (2nd)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlOrderby2" runat="server" Width="250px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Order by (2nd)">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="FLDDOA" Text="Available Date" />
                                        <telerik:RadComboBoxItem Value="FLDRANKNAME" Text="Rank Name" />
                                        <telerik:RadComboBoxItem Value="FLDNAME" Text="Name" />
                                    </Items>


                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSearchfor" runat="server" Text="Search For"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadListBox ID="cblHasMissing" CheckBoxes="true" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal">
                                    <Items>
                                        <telerik:RadListBoxItem Value="1" Text="Has" />
                                        <telerik:RadListBoxItem Value="2" Text="Missing" />
                                    </Items>

                                </telerik:RadListBox>
                            </td>
                        </tr>
                    </table>
                    <%--<asp:GridView ID="gvDocument" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" OnRowDataBound="gvDocument_RowDataBound"
                                    AllowSorting="true" ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDDOCUMENTID">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvDocument" runat="server" Height="" Width="100%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvDocument_NeedDataSource"
                        OnItemDataBound="gvDocument_ItemDataBound"
                        OnItemCommand="gvDocument_ItemCommand"
                        EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView DataKeyNames="FLDDOCUMENTID" EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                            AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldName="FLDDOCUMENTTYPENAME" FieldAlias="Details" SortOrder="Ascending" />
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="FLDDOCUMENTTYPENAME" SortOrder="Ascending" />
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn Visible="false" UniqueName="Data" DataField="FLDDOCUMENTTYPENAME">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDOCUMENTTYPENAME"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle Width="5%" />
                                    <ItemTemplate>
                                        <telerik:RadCheckBox ID="chkDocumentSelect" runat="server" AutoPostBack="true" EnableViewState="true" OnCheckedChanged="SaveCheckedDocumentValues" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Course">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCategoryHeader" runat="server" Text="Category"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentType" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOCUMENTTYPE")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentTypeName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOCUMENTTYPENAME")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDocumentCategoryName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOCUMENTCATEGORYNAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Course">
                                    <HeaderStyle Width="60%" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCourseHeader" runat="server" Text="Documents"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOCUMENTNAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>


                </div>
            </div>

        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
