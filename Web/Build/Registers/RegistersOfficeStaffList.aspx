<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOfficeStaffList.aspx.cs"
    Inherits="OfficeStaffList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="../UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Designation" Src="../UserControls/UserControlDesignation.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuOfficestaffMain" runat="server" OnTabStripCommand="MenuOfficestaffMain_TabStripCommand" TabStrip="true" ></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuSecurityOfficeStaffList" runat="server" OnTabStripCommand="SecurityOfficeStaffList_TabStripCommand"
                 Title="Office Staff"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="ltSalutation" runat="server" Text="Salutation"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ddlsalutation" runat="server" QuickTypeCode="160" AppendDataBoundItems="true"
                            CssClass="input_mandatory" Width="250px" />

                        <telerik:RadTextBox runat="server" ID="txtSalutation" MaxLength="10" CssClass="input" Visible="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUsername" runat="server" Text="Username"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtUsername" Width="250px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" MaxLength="100" CssClass="input_mandatory" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" MaxLength="100" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastname" MaxLength="100" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirth" runat="server" Text="D.O.B"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateOfBirth" runat="server" CssClass="input_mandatory"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeFileNumber" runat="server" Text="Employee/ File Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" MaxLength="50" CssClass="input_mandatory" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="ltLocation" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDesignation" runat="server" Text="Designation"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Designation ID="ucDesignation" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="250px" />
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblDesignationHRMS" runat="server" Text="HRMS Designation"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txthrmsdesignation" ReadOnly="true" CssClass="readonlytextbox" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepartment" runat="server" Text="Department"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Department runat="server" ID="ucDepartment" AppendDataBoundItems="true" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmail" MaxLength="100" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumber" runat="server" Text="Passport Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPassportNo" MaxLength="100" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPlaceOfIssue" MaxLength="100" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssue" runat="server" Text="Date of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateOfIssue" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblExpiryDate" runat="server" Text="Expiry Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtExpiryDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblisactive" runat="server" Text="Is Active"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkisactive" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
