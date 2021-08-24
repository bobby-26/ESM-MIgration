<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewQueryActivityFilter.aspx.cs"
    Inherits="Crew_CrewQueryActivityFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register Src="../UserControls/UserControlNationalityList.ascx" TagName="UserControlNationalityList" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRankList.ascx" TagName="UserControlRankList" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlZone.ascx" TagName="UserControlZone" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRank.ascx" TagName="UserControlRank" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlVesselTypeList.ascx" TagName="UserControlVesselTypeList" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlEngineType.ascx" TagName="UserControlEngineType" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlDocuments.ascx" TagName="UserControlDocuments" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlDate.ascx" TagName="UserControlDate" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="OtherCompany" Src="~/UserControls/UserControlOtherCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register Src="~/UserControls/UserControlPoolList.ascx" TagName="UserControlPool" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlVesselCommon.ascx" TagName="UserControlVessel" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="BatchList" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function OnClientSelectedIndexChanged(sender, args) {
                args.get_item().set_checked(args.get_item().get_selected());
            }
        </script>

        <script type="text/javascript">
            function OnClientItemChecked(sender, args) {
                args.get_item().set_selected(args.get_item().get_checked());
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="110%">
            <eluc:TabStrip ID="MenuActivityFilterMain" runat="server" OnTabStripCommand="NewApplicantFilterMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td colspan="4">
                        <telerik:RadButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <telerik:RadLabel ID="lblnote" runat="server" Font-Bold="true" Text="Note:"></telerik:RadLabel>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Font-Bold="true" Text="For embeded search, use '%' symbol. (Eg. Name: %xxxx)"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtName" MaxLength="100" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblFileNumber" runat="server" Text="File No"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtFileNumber" MaxLength="100" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumber" runat="server" Text="Passport"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPassortNo" MaxLength="100" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCDCNumber" runat="server" Text="CDC No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSeamanbookNo" MaxLength="100" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAppliedBetween" runat="server" Text="Applied Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtAppliedStartDate" runat="server" />
                        <eluc:UserControlDate ID="txtAppliedEndDate" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofAcailability" runat="server" Text="D.O.A. Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtDOAStartDate" runat="server" />
                        <eluc:UserControlDate ID="txtDOAEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselSailed" runat="server" Text="Vessel Sailed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlVessel ID="ddlVessel" runat="server" AppendDataBoundItems="true"
                            Width="240px" VesselsOnly="true" Entitytype="VSL" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblZone" Text="Zone" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlZone ID="ddlZone" runat="server" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPool" Text="Pool" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlPool ID="ddlPool" runat="server" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblNationality" Text="Nationality" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlNationalityList ID="lstNationality" runat="server" Width="240px"
                            AppendDataBoundItems="true" />

                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPresentRank" Text="Present Rank" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRankList ID="lstRank" runat="server" AppendDataBoundItems="true" Width="240px" />
                        <br />
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblBatch" Text="Batch" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:BatchList ID="lstBatch" AppendDataBoundItems="true" runat="server" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCountry" Text="Country" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ddlCountry" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                            OnTextChangedEvent="ddlCountry_TextChanged" Width="240px" />
                    </td>
                    <td colspan="1">
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td colspan="1">
                        <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblState" Text="State" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:State ID="ddlState" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCity" Text="City" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:City ID="ddlCity" runat="server" AppendDataBoundItems="true" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirth" Text="D.O.B. Between" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtNOKName" MaxLength="100" Width="345px" Visible="false"></telerik:RadTextBox>
                        <eluc:UserControlDate ID="txtDOBStartDate" runat="server" />
                        <eluc:UserControlDate ID="txtDOBEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                        <b>
                            <telerik:RadLabel ID="lblExperience" Text="Experience" runat="server"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSailedRank" Text="Sailed Rank" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRank ID="ddlSailedRank" runat="server" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" Text="Vessel Type" runat="server"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">
                        <eluc:UserControlVesselTypeList ID="ddlVesselType" runat="server" AppendDataBoundItems="true" Width="240px" />
                        <br />
                        <asp:CheckBox ID="chkIncludepastexp" runat="server" Text="Include past experience" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEngineType" Text="Engine Type" runat="server"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:UserControlEngineType ID="ddlEngineType" runat="server" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" Font-Bold="true" Text="Note: If checked, the filter will search for vessel type experience
                                in"
                            runat="server">
                        </telerik:RadLabel>
                        <br />
                        <telerik:RadLabel ID="RadLabel3" Font-Bold="true" Text="full past experience,if not will check only last vessel/onboard vessel" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                        <b>
                            <telerik:RadLabel ID="lblDocuments" Text="Documents" runat="server"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" Text="Course" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDocuments ID="ddlCourse" runat="server" DocumentType="COURSE" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLicenses" Text="License" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDocuments ID="ddlLicences" runat="server" DocumentType="LICENCE"
                            AppendDataBoundItems="true" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVisa" Text="Visa" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ddlVisa" runat="server" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFlag" Text="Flag" runat="server" Visible="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Flag ID="ddlFlag" runat="server" Visible="true" AppendDataBoundItems="true"
                            Width="240px" FlagList='<%#PhoenixRegistersFlag.ListFlag() %>' />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPreviousCompany" Text="Previous Company" runat="server"></telerik:RadLabel>
                    </td>
                    <td colspan="1">
                        <eluc:OtherCompany ID="ddlPrevCompany" runat="server" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblprincipalntbr" Text="Principal NTBR" runat="server"></telerik:RadLabel>
                    </td>
                    <td colspan="1">
                        <eluc:Principal ID="ucPrincipalntbr" runat="server" AddressType="128" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveInActive" Text="Active / In-Active" runat="server"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlInActive" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select record" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="All" />
                                <telerik:RadComboBoxItem Value="1" Text="Active" />
                                <telerik:RadComboBoxItem Value="0" Text="In-Active" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" Text="Status" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadListBox CheckBoxes="true" ID="lstStatus" runat="server" Width="240px" Height="80px" SelectionMode="Multiple" OnClientItemChecked="OnClientItemChecked" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"
                            RepeatDirection="Vertical" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE">
                        </telerik:RadListBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
