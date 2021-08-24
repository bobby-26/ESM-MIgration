<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPersonalDetailsBySeafarerEdit.aspx.cs" Inherits="CrewOffshore_CrewPersonalDetailsBySeafarerEdit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlNationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlRank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlZone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlMaritalStatus" Src="~/UserControls/UserControlMaritalStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="../UserControls/UserControlFlag.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew</title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>
        <script type="text/javascript">
            function showBMI() {
                var bmi = document.getElementById("divBMI");
                bmi.style.display = "block";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewPersonal" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewPersonaltab" runat="server" OnTabStripCommand="CrewPersonaltab_TabStripCommand" Title="Personal"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <table id="table2" runat="server" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Personal Detail(Office)"></telerik:RadLabel>
                        </b>
                    </td>

                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Personal Detail(Edit by Seafarer)"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRepFileNo" runat="server" CssClass="input_mandatory upperCase" Width="150px"></telerik:RadTextBox>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true"
                            Text="If present employee, please enter File No. with prefix 'E' . If new employee please enter 'NA'">
                        </telerik:RadToolTip>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPhotograph" runat="server" Text="Photograph" Visible="false"></telerik:RadLabel>
                        <br />
                        <a id="aCrewImg" runat="server">
                            <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                Width="120px" /></a>
                        <br />
                        <%--<asp:FileUpload ID="txtFileUpload" runat="server"  />--%>
                        <telerik:RadUpload ID="txtFileUpload" runat="server" MaxFileInputsCount="1" OverwriteExistingFiles="false"
                            ControlObjectsVisibility="None" Skin="Silk">
                        </telerik:RadUpload>
                    </td>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPhotographemp" runat="server" Text="Photograph" Visible="false"></telerik:RadLabel>
                        <br />
                        <a id="aCrewImgemp" runat="server">
                            <asp:Image ID="imgPhotoemp" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                Width="120px" /></a>
                        <br />
                        <%--<asp:FileUpload ID="txtFileUpload" runat="server"  />--%>
                        <telerik:RadUpload ID="txtFileUploademp" runat="server" MaxFileInputsCount="1" OverwriteExistingFiles="false"
                            ControlObjectsVisibility="None" Skin="Silk">
                        </telerik:RadUpload>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUploadResume" runat="Server" Text="Upload Resume"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:FileUpload ID="UploadResume" runat="server" />
                        <asp:Image ID="imgResume" runat="server" ImageUrl="<%$ PhoenixTheme:images/download_1.png %>" ToolTip="Resume" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblUploadResumeemp" runat="Server" Text="Upload Resume"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:FileUpload ID="UploadResumeemp" runat="server" />
                        <asp:Image ID="imgResumeemp" runat="server" ImageUrl="<%$ PhoenixTheme:images/download_1.png %>" ToolTip="Resume" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstname" runat="server" CssClass="input_mandatory upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFirstNameemp" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstnameemp" runat="server" CssClass="input_mandatory upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" CssClass="input upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleNameemp" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleNameemp" runat="server" CssClass="input upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" CssClass="input upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastNameemp" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastNameemp" runat="server" CssClass="input upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeFileNo" runat="server" Text="Employee/File No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="imgPDForm" AlternateText="PD Form" ToolTip="PD Form" Visible="false">
                            <span class="icon"><i class="fas fa-file"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCivilStatus" runat="server" Text="Civil Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlMaritalStatus ID="ddlMaritialStatus" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCivilStatusemp" runat="server" Text="Civil Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlMaritalStatus ID="ddlMaritialStatusemp" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRankApplied" runat="server" Text="Rank Applied"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankAppliedemp" runat="server" Text="Rank Applied"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRank ID="ddlRankemp" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofBirth" runat="server" Text="Place of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPlaceofBirth" runat="server" CssClass="input upperCase" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofBirthemp" runat="server" Text="Place of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPlaceofBirthemp" runat="server" CssClass="input upperCase" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirth" runat="server" Text="D.O.B."></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtDateofBirth" runat="server" CssClass="input_mandatory"
                            Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirthemp" runat="server" Text="D.O.B."></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtDateofBirthemp" runat="server" CssClass="input_mandatory"
                            Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlNationality ID="ddlNationality" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNationalityemp" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlNationality ID="ddlNationalityemp" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPanNo" runat="server" Text="PAN No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPanNo" runat="server" CssClass="input upperCase" Width="150px"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClipPanNo"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPanNoemp" runat="server" Text="PAN No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPanNoemp" runat="server" CssClass="input upperCase" Width="150px"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClipPanNoemp"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUIDNO" runat="server" Text="UID No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUidNo" runat="server" CssClass="input upperCase" Width="150px"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClipUidNo"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUIDNOemp" runat="server" Text="UID No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUidNoemp" runat="server" CssClass="input upperCase" Width="150px"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClipUidNoemp"
                            runat="server" />
                    </td>
                </tr>
              <%--  <tr>
                    <td>
                        <telerik:RadLabel ID="lblINDOSNo" runat="Server" Text="INDOS No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtINDOsNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblINDOSNoemp" runat="Server" Text="INDOS No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtINDOsNumberemp" runat="server" CssClass="readonlytextbox" ReadOnly="true" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSIMSPreseaBatch" runat="server" Text="PreSea Batch"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Batch ID="ucBatch" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSIMSPreseaBatchemp" runat="server" Text="PreSea Batch"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Batch ID="ucBatchemp" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGender" runat="server" Text="Gender"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlSex" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGenderemp" runat="server" Text="Gender"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlSexemp" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHeight" runat="Server" Text="Height(Cms)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtHeight" runat="server" CssClass="input_mandatory txtNumber" MaxLength="5"
                            IsInteger="true" OnTextChangedEvent="CalculateBMI" AutoPostBack="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHeightemp" runat="Server" Text="Height(Cms)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtHeightemp" runat="server" CssClass="input_mandatory txtNumber" MaxLength="5"
                            IsInteger="true" OnTextChangedEvent="CalculateBMI" AutoPostBack="true" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHairColor" runat="server" Text="Hair Colour"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtHairColor" CssClass="input_mandatory upperCase" runat="server" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHairColoremp" runat="server" Text="Hair Colour"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtHairColoremp" CssClass="input_mandatory upperCase" runat="server" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWeight" runat="server" Text="Weight(Kg)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtWeight" runat="server" MaxLength="6"
                            OnTextChangedEvent="CalculateBMI" AutoPostBack="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWeightemp" runat="server" Text="Weight(Kg)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtWeightemp" runat="server" MaxLength="6"
                            OnTextChangedEvent="CalculateBMI" AutoPostBack="true" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEyeColour" runat="server" Text="Eye Colour"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEyeColor" CssClass="input_mandatory upperCase" runat="server" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEyeColouremp" runat="server" Text="Eye Colour"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEyeColoremp" CssClass="input_mandatory upperCase" runat="server" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDistinguishMark" runat="server" Text="Distinguish Mark"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDistinguishMark" CssClass="input_mandatory upperCase" runat="server" MaxLength="200" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDistinguishMarkemp" runat="server" Text="Distinguish Mark"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDistinguishMarkemp" CssClass="input_mandatory upperCase" runat="server" MaxLength="200" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShoeSize" runat="server" Text="Shoe Size(Inch)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlShoesSize" runat="server" EnableLoadOnDemand="True" Width="150px"
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
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblShoeSizeemp" runat="server" Text="Shoe Size(Inch)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlShoesSizeemp" runat="server" EnableLoadOnDemand="True" Width="150px"
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
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBoilerSuitSize" runat="server" Text="Boiler Suit Size"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBoilerSuitSize" runat="server" EnableLoadOnDemand="True" Width="150px"
                            EmptyMessage="Type to select Shirt Size" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" Selected="true"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="S" Text="S"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="M" Text="M"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="L" Text="L"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XL" Text="XL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XXL" Text="XXL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XXXL" Text="XXXL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XXXXL" Text="XXXXL"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBoilerSuitSizeemp" runat="server" Text="Boiler Suit Size"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBoilerSuitSizeemp" runat="server" EnableLoadOnDemand="True" Width="150px"
                            EmptyMessage="Type to select Shirt Size" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" Selected="true"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="S" Text="S"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="M" Text="M"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="L" Text="L"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XL" Text="XL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XXL" Text="XXL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XXXL" Text="XXXL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="XXXXL" Text="XXXXL"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <table id="tablepassport" runat="server" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="lblPassportDetail" runat="server" Text="Passport Detail(Office)"></telerik:RadLabel>
                        </b>
                    </td>

                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="Passport Detail(Edit by Seafarer)"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumber" runat="server" Text="Passport Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassportnumber" runat="server" CssClass="input_mandatory upperCase"></telerik:RadTextBox>
                        <asp:Image ID="imgPPFlag" runat="server" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumberemp" runat="server" Text="Passport Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassportnumberemp" runat="server" CssClass="input_mandatory upperCase"></telerik:RadTextBox>
                        <asp:Image ID="Image1" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssue" runat="server" Text="Date of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfIssue" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssueemp" runat="server" Text="Date of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfIssueemp" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text="Date of Expiry"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfExpiry" runat="server" CssClass="input_mandatory" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClip" runat="server" />
                       <%-- <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                            CommandName="Archive" ID="imgPassportArchive" ToolTip="Add" OnClick="OnClickPassportArchive"></asp:ImageButton>--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiryemp" runat="server" Text="Date of Expiry"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfExpiryemp" runat="server" CssClass="input_mandatory" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClipemp" runat="server" />
                       <%-- <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                            CommandName="Archive" ID="imgPassportArchiveemp" ToolTip="Add" OnClick="OnClickPassportArchive"></asp:ImageButton>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssueemp" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPlaceOfIssueemp" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblECNR" runat="server" Text="ECNR"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucECNR" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblECNRemp" runat="server" Text="ECNR"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucECNRemp" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMinimum3BlankPages" runat="server" Text="Minimum 3 Blank Pages"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucBlankPages" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            ShortNameFilter="S,N" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblMinimum3BlankPagesemp" runat="server" Text="Minimum 3 Blank Pages"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucBlankPagesemp" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            ShortNameFilter="S,N" />
                    </td>
                </tr>
            </table>
           <table id="tableseamanbook" runat="server" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="RadLabel5" runat="server" Text="Seaman's Book Detail(Office)"></telerik:RadLabel>
                        </b>
                    </td>

                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="RadLabel6" runat="server" Text="Seaman's Book Detail(Edit by seafarer)"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeamansBookNumber" runat="server" Text="Seaman's Book Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanBookNumber" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                        <asp:Image ID="imgCCFlag" runat="server" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSeamansBookNumberemp" runat="server" Text="Seaman's Book Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanBookNumberemp" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                        <asp:Image ID="imgCCFlagemp" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Flag ID="ucSeamanCountry" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFlagemp" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Flag ID="ucSeamanCountryemp" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssue1" runat="server" Text="Date of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucSeamanDateOfIssue" runat="server" CssClass="input_mandatory" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgCCClip" runat="server" />
                     <%--   <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                            CommandName="Archive" ID="imgSeamanBook" ToolTip="Add" OnClick="OnClickSeamanBookArchive"></asp:ImageButton>--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssue1emp" runat="server" Text="Date of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucSeamanDateOfIssueemp" runat="server" CssClass="input_mandatory" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgCCClipemp" runat="server" />
                      <%--  <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                            CommandName="Archive" ID="imgSeamanBookemp" ToolTip="Add" OnClick="OnClickSeamanBookArchive"></asp:ImageButton>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiry1" runat="server" Text="Date of Expiry"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucSeamanDateOfExpiry" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiry1emp" runat="server" Text="Date of Expiry"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucSeamanDateOfExpiryemp" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue1" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanPlaceOfIssue" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue1emp" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanPlaceOfIssueemp" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
