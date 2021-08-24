<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPersonal.aspx.cs" Inherits="CrewPersonal" %>

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
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
    </telerik:RadCodeBlock>

</head>
<body onload="blink();">
    <form id="frmCrewMainPersonel" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewMainPersonal" runat="server" OnTabStripCommand="CrewMainPersonal_TabStripCommand" Title="Personal"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <table id="table1" runat="server" cellpadding="10" cellspacing="10">
                <tr>
                    <td colspan="2">
                        <a id="aCrewImg" runat="server">
                            <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                Width="120px" />
                        </a>
                        <asp:ImageButton ID="imgIDCard" runat="server" ImageUrl="<%$ PhoenixTheme:images/id-card.png %>"
                            ToolTip="ID Card" />
                        <br />
                        <telerik:RadUpload ID="RadUpload1" runat="server" MaxFileInputsCount="1" OverwriteExistingFiles="false"
                            ControlObjectsVisibility="None" Skin="Silk">
                        </telerik:RadUpload>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtFirstname" runat="server" EmptyMessage="First Name" CssClass="input_mandatory upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtMiddlename" runat="server" CssClass="upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtLastname" runat="server" CssClass="upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCreatedBy" runat="server" Text="Created By"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtCreatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" MaxLength="200" Width="150px"></telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table id="table2" runat="server" cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" EmptyMessage="File No." CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
                        <asp:ImageButton runat="server" ID="imgPDForm" AlternateText="PD Form" ToolTip="PD Form"
                            ImageUrl="<%$ PhoenixTheme:images/pr.png %>" Visible="false" />
                        <asp:LinkButton runat="server" AlternateText="Email" Visible="false"
                            ID="cmdEmail" ToolTip="Pdform Email" Width="20PX" Height="20PX">                            
                            <span class="icon"><i class="fas fa-envelope"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeStatus" runat="server" EmptyMessage="Status" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCivilStatus" runat="server" Text="Civil Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MaritalStatus ID="ucMaritialStatus" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRankApplied" runat="server" Text="Rank Applied"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            Enabled="false" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAppliedOn" runat="server" Text="Applied On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtAppliedOn" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankPosted" runat="server" Text="Rank Posted"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRankPosted" runat="server" AppendDataBoundItems="true" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastOnboardVessel" runat="server" Text="Last/Onboard Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastVessel" runat="server" EmptyMessage="Last Vessel" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDtFirstJoin" runat="server" Text="First Join Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateofJoin" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDirectSignOn" runat="server" Text="Direct Sign-On"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkDirectSignon" runat="server" AutoPostBack="true" OnCheckedChanged="DirectSignOnfn" Value="0"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofBirth" runat="server" Text="Place of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPlaceofBirth" runat="server" EmptyMessage="Place of Birth" CssClass="upperCase" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirth" runat="server" Text="Date of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateofBirth" runat="server" CssClass="input_mandatory" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAge" runat="server" Text="Age"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAge" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumber" runat="server" Text="Passport No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassport" runat="server" MaxLength="200" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSeamans" runat="server" Text="CDC No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamenBookNumber" runat="server" MaxLength="200" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPanNo" runat="server" Text="Pan No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPanNo" runat="server" CssClass="upperCase" Width="150px"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClipPanNo"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUIDNo" runat="server" Text="UID No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUidNo" runat="server" CssClass="input upperCase" Width="150px"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClipUidNo"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblINDOSNo" runat="server" Text="INDOS No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtINDOsNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSIMSPreSeaBatch" runat="server" Text="PreSea Batch"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                            Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="150px" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>" ID="cmdeditpool" runat="server"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRecrOff" runat="server" Text="Manning Office"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGender" runat="server" Text="Gender"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Sex ID="ucSex" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHeight" runat="server" Text="Height(Cms)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtHeight" runat="server" CssClass="input_mandatory txtNumber" MaskText="###"
                            IsInteger="true" OnTextChangedEvent="CalculateBMI" AutoPostBack="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWeight" runat="server" Text="Weight(Kg)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtWeight" runat="server" MaskText="###"
                            ReadOnly="true" IsInteger="true" OnTextChangedEvent="CalculateBMI" AutoPostBack="true"
                            Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHairColour" runat="server" Text="Hair Colour"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtHairColor" runat="server" CssClass="input_mandatory upperCase" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEyeColour" runat="server" Text="Eye Colour"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEyeColor" runat="server" CssClass="input_mandatory upperCase" MaxLength="50" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDistinguishMark" runat="server" Text="Distinguish Mark"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDistinguishMark" runat="server" CssClass="input_mandatory upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
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
                        <telerik:RadLabel ID="lblShirtSize" runat="server" Text="Shirt Size"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlShirtSize" runat="server" EnableLoadOnDemand="True" Width="150px"
                            EmptyMessage="Type to select Shirt Size" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" Selected="true" />
                                <telerik:RadComboBoxItem Value="S" Text="S" />
                                <telerik:RadComboBoxItem Value="M" Text="M" />
                                <telerik:RadComboBoxItem Value="L" Text="L" />
                                <telerik:RadComboBoxItem Value="XL" Text="XL" />
                                <telerik:RadComboBoxItem Value="XXL" Text="XXL" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBMI" runat="server" Text="BMI"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBMI" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            MaxLength="4" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMentor" runat="server" Text="Mentor"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <span id="spnPickListFleetManager">
                            <telerik:RadTextBox ID="txtMentorName" runat="server" MaxLength="100" Width="150px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtuserDesignation" runat="server" CssClass="hidden" Enabled="false" MaxLength="30" Width="5px" ReadOnly="true"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imguser" Style="cursor: pointer; vertical-align: top"
                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListFleetManager', 'codehelp1', '', 'Common/CommonPickListUser.aspx?framename=ifMoreInfo', true); "
                                ToolTip="Select Mentor"  />
                            <telerik:RadTextBox ID="txtuserid" runat="server" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtuserEmailHidden" runat="server" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:LinkButton ID="lnkResume" runat="server" Text="View Resume"></asp:LinkButton>
                    </td>
                </tr>
                <tr id="compact" runat="server" >
                    <td>
                        <telerik:RadPushButton ID="lnkIncompatibility" runat="server" Text="Compatibility Check"></telerik:RadPushButton>
                    </td>
                    <td colspan="2">
                        <asp:LinkButton ID="lnkImportantRemarks" runat="server" ForeColor="Red" Font-Bold="true" OnClick="lnkImportantRemarks_Click" Text="Please see the Important Remarks"></asp:LinkButton>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtNTBRStatus" runat="server" MaxLength="200" Visible="false" Width="150px" Height="15px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="Tr1" visible="false" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblBoilerSuitSize" runat="server" Text="Boiler Suit Size"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlBoilerSuitSize" runat="server" EnableLoadOnDemand="True" Width="150px"
                            EmptyMessage="Type to select Shirt Size" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" Selected="true" />
                                <telerik:RadComboBoxItem Value="S" Text="S" />
                                <telerik:RadComboBoxItem Value="M" Text="M" />
                                <telerik:RadComboBoxItem Value="L" Text="L" />
                                <telerik:RadComboBoxItem Value="XL" Text="XL" />
                                <telerik:RadComboBoxItem Value="XXL" Text="XXL" />
                                <telerik:RadComboBoxItem Value="XXXL" Text="XXXL" />
                                <telerik:RadComboBoxItem Value="XXXXL" Text="XXXXL" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPassportName" runat="server" Text="Name as per Passport"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassportName" runat="server" CssClass="upperCase" MaxLength="200" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <div id="divBMI" runat="server" style="position: absolute; display: none; top: 0px; left: 0px; z-index: 10; background-color: White"
                onmouseout="javascript:this.style.display='none';">
            </div>
            <eluc:Confirmation ID="ucConfirm" runat="server" OnConfirmMesage="UpdateCrewMainPersonalInformation" />
            <%--            </div>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
