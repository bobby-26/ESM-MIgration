<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsEmployeePersonal.aspx.cs"
    Inherits="VesselAccountsEmployeePersonal" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
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
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewMainPersonel" DecoratedControls="All" EnableRoundedCorners="true" />

    <form id="frmCrewMainPersonel" runat="server" autocomplete="off">
        
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
         
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:TabStrip ID="CrewMainPersonal" runat="server" Title="Personal"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table  id="tblid" cellpadding="2" cellspacing="2" width="100%" runat="server">
                <tr>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblEmployeeFileNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtFirstname" runat="server" CssClass="
                            readonlytextbox"
                            ReadOnly="true" Width="180px"
                            MaxLength="200">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtMiddlename" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="180px"
                            MaxLength="200">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastname" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGender" runat="server" Text="Gender"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtGender" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirth" runat="server" Text="D.O.B"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateofBirth" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofBirth" runat="server" Text="Place of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPlaceofBirth" runat="server" CssClass="readonlytextbox" Width="180px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNationality" runat="server" CssClass="readonlytextbox" Width="180px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAge" runat="server" Text="Age"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAge" CssClass="readonlytextbox txtNumber" ReadOnly="true"
                            Width="60px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeamansBookNumber" runat="server" Text="CDC No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamenBookNumber" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumber" runat="server" Text="Passport No. "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassport" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td colspan="2" rowspan="4">
                        <a id="aCrewImg" runat="server">
                            <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                Width="120px" /></a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRankApplied" runat="server" Text="Rank Applied"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRankApplied" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankPosted" runat="server" Text="Rank Posted"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRankPosted" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRecrOffZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtZone" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPool" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblINDOSNumber" runat="server" Text="INDOS No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtINDOsNumber" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDtFirstJoin" runat="server" Text="First Joined"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateofJoin" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHeightCms" runat="server" Text="Height(Cms)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtHeight" runat="server" CssClass="readonlytextbox txtNumber" ReadOnly="true"
                            Width="60px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWeightKg" runat="server" Text="Weight(Kg)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWeight" runat="server" CssClass="readonlytextbox txtNumber" ReadOnly="true"
                            Width="60px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBMI" runat="server" Text="BMI"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBMI" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            MaxLength="4" Width="60px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShoeSizeInch" runat="server" Text="Shoe Size(Inch)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtShoes" runat="server" CssClass="readonlytextbox txtNumber" ReadOnly="true"
                            Width="60px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCivilStatus" runat="server" Text=" Civil Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMaritialStatus" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastVessel" runat="server" Text="Last Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastVessel" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHairColour" runat="server" Text="Hair Colour"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtHairColor" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEyeColour" runat="server" Text="Eye Colour"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEyeColor" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDistinguishMark" runat="server" Text="Distinguish Mark"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDistinguishMark" runat="server" Width="180px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
