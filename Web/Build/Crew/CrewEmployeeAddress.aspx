<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewEmployeeAddress.aspx.cs"
    Inherits="CrewEmployeeAddress" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="~/UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Institution" Src="~/UserControls/UserControlInstitution.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
        <title>Employee Address</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewEmployeeAddress" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlEmployeeAddressEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader" style="position: relative">
                <div id="divHeading">
                    <eluc:Title runat="server" ID="ucTitle" Text="Employee Address" />
                </div>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                <eluc:TabStrip ID="MenuEmployeeAddress" runat="server" OnTabStripCommand="MenuEmployeeAddress_TabStripCommand">
                </eluc:TabStrip>
                <asp:Label ID="lblEmployeeAddressid" runat="server" Visible="false"></asp:Label>
            </div>
            <div id="divFind" style="position: relative; z-index: 0; width: 100%">
                <table id="tblEmployeeAddress" width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Literal ID="lblEmployeeNo" runat="server" Text="Employee No"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeNo" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRank" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSurname" runat="server" Text="Surname"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurname" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblFirstname" runat="server" Text="Firstname"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstname" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMiddlename" runat="server" Text="Middlename"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMiddlename" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Literal ID="lblPermanentAddress" runat="server" Text="Permanent Address"></asp:Literal>
                        </td>
                        <td colspan="4">
                            <asp:Literal ID="lblLocalAddress" runat="server" Text="Local Address"></asp:Literal>
                            <asp:CheckBox ID="chkCopyPermanent" runat="server" OnCheckedChanged="CopyPermanentAddress"
                                AutoPostBack="true" Text="Copy Permanent Address" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:TextBox ID="txtAddressPermanent" runat="server" CssClass="input_mandatory" Height="60px"
                                MaxLength="100" TextMode="MultiLine" Width="370px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtAddressLocal" runat="server" CssClass="input_mandatory" Height="60px"
                                MaxLength="100" TextMode="MultiLine" Width="370px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPermanentCityHomeTown" runat="server" Text="City/Home Town"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCityPermanent" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPermanentCountry" runat="server" Text="Country"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Country ID="ucCountryPermanent" runat="server" AppendDataBoundItems="true"
                                CountryList="<%# PhoenixRegistersCountry.ListCountry(null)%>" />
                        </td>
                        <td>
                            <asp:Literal ID="lblLocalCityHomeTown" runat="server" Text="City/Home Town"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCityLocal" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblLocalCountry" runat="server" Text="Country"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Country ID="ucCountryLocal" runat="server" AppendDataBoundItems="true" CountryList="<%# PhoenixRegistersCountry.ListCountry(null)%>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPermanentPinZipCode" runat="server" Text="Pin/Zip Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPinPermanent" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPermanentState" runat="server" Text="State"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStatePermanent" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblLocalPinZipCode" runat="server" Text="Pin/Zip Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPinLocal" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblLocalState" runat="server" Text="State"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStateLocal" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPermanentPhoneNumber" runat="server" Text="Phone Number"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtPhonenoPermanent" runat="server" CssClass="input txtNumber" MaxLength="10"
                                IsInteger="true" />
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Literal ID="lblLocalPhoneNumber" runat="server" Text="Phone Number"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtPhonenoLocal" runat="server" CssClass="input txtNumber" MaxLength="10"
                                IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPermanentMobileNumber" runat="server" Text="Mobile Number"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtMobilenoPermanent" runat="server" CssClass="input txtNumber"
                                MaxLength="10" IsInteger="true" />
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Literal ID="lblLocalMobileNumber" runat="server" Text="Mobile Number"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtMobilenoLocal" runat="server" CssClass="input txtNumber" MaxLength="10"
                                IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPermanentFaxNo" runat="server" Text="Fax No"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtFaxNoPermanent" runat="server" CssClass="input txtNumber" MaxLength="10"
                                IsInteger="true" />
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Literal ID="lblLocalFaxNo" runat="server" Text="Fax No"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtFaxnoLocal" runat="server" CssClass="input txtNumber" MaxLength="10"
                                IsInteger="true" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <hr />
            <div>
                <table cellpadding="1" cellspacing="1" width="60%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblNearestInternationalAirport" runat="server" Text="Nearest International Airport"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Airport ID="ucAirport" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPrefferedTrainingInstitute" runat="server" Text="Preffered Training Institute"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Institution ID="ucInstitution" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
