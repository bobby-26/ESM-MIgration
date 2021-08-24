<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitSuperintendentVisit.aspx.cs" Inherits="AccountsVesselVisitSuperintendentVisit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="../UserControls/UserControlDepartment.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IT Visit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSupVisit" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="Title1" Text="Vessel Visit" ShowMenu="false" Visible="false"></eluc:Title>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
            CssClass="hidden" />
        <eluc:TabStrip ID="MenuSupVisit" runat="server" OnTabStripCommand="MenuSupVisit_TabStripCommand" Title="Vessel Visit" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuITVisitSub" runat="server" OnTabStripCommand="MenuITVisitSub_TabStripCommand"></eluc:TabStrip>
        <table id="Table2" width="100%" style="color: Blue">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblfd" runat="server" Text=" * From/ To Date include Travelling Days" Font-Bold="true"></telerik:RadLabel>
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="1" style="width: 100%">
            <tr>
                <td width="12%">
                    <telerik:RadLabel ID="lblFormNo" runat="server" Text="Form No"></telerik:RadLabel>
                </td>
                <td width="38%">
                    <telerik:RadTextBox ID="txtFormNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                </td>
                <td width="10%" />
                <td width="40%" />
            </tr>
            <tr>
                <td colspan="4">
                    <hr style="height: -15px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>

                </td>
                <td>
                    <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" Width="50%"
                        AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlVessel_Changed" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" OnDataBound="ddlAccountDetails_DataBound"
                        DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" Width="50%">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr style="height: -15px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="Literal1" runat="server" Text="Employee Name"></telerik:RadLabel>
                </td>
                <td>
                    <%--  <asp:DropDownList ID ="ddlSubaccount" runat="server"  
                                CssClass="input_mandatory">
                            </asp:DropDownList>
                    --%>
                    <span id="spnPickListFleetManager">
                        <telerik:RadTextBox ID="txtMentorName" runat="server" CssClass="input_mandatory" MaxLength="100"
                            Width="80%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtuserDesignation" runat="server" CssClass="hidden" Enabled="false"
                            MaxLength="30" Width="5px" ReadOnly="true">
                        </telerik:RadTextBox>

                        <asp:ImageButton runat="server" ID="imguser" Style="cursor: pointer; vertical-align: top"
                            ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListFleetManager', 'codehelp1', '', '../Common/CommonPickListEmployeeAccount.aspx', true); "
                            ToolTip="Select Employee" />
                        <telerik:RadTextBox runat="server" ID="txtuserid" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtuserEmailHidden" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                    </span>


                </td>
                <td>
                    <telerik:RadLabel ID="lblBudgetedVisit" runat="server" Text="Visit Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlBudgetedVisit" runat="server" CssClass="input_mandatory" Width="50%">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Budgeted" Value="1"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Dry Dock" Value="3"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Predelivery" Value="4"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Non - Budgeted" Value="2"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Retrofit" Value="5"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>


                <td>
                    <telerik:RadLabel ID="lblDepartment" runat="server" Text="Department"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlDepartment" runat="server" DataTextField="FLDDEPARTMENTNAME" CssClass="input_mandatory" OnDataBound="ddlDepartment_DataBound"
                        DataValueField="FLDDEPARTMENTID" Width="50%">
                    </telerik:RadComboBox>
                </td>


            </tr>
            <tr>
                <td colspan="4">
                    <hr style="height: -15px" />
                </td>
            </tr>

            <tr>
                <td>
                    <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                </td>
                <td style="z-index: 2;">
                    <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input_mandatory" Width="400px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Country ID="ddlcountrylist" runat="server" AppendDataBoundItems="true" Width="50%" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVisitStartDate" runat="server" Text="Visit Start Date"></telerik:RadLabel>
                </td>
                <td>

                    <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblVisitEndDate" runat="server" Text="Visit End Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="Onboard Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucVisitStartDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>


                <td>
                    <telerik:RadLabel ID="lblToDate" runat="server" Text="Disembarkation Date"></telerik:RadLabel>

                </td>
                <td>
                    <eluc:Date ID="ucVisitEndDate" runat="server" CssClass="input_mandatory" DatePicker="true" />

                </td>


                <td>
                    <telerik:RadTextBox ID="txtToTime" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50px" Visible="false" />
                    <%-- <ajaxtoolkit:maskededitextender id="txtToTimeMask" runat="server" acceptampm="false"
                        clearmaskonlostfocus="false" cleartextoninvalid="true" mask="99:99" masktype="Time"
                        targetcontrolid="txtToTime" usertimeformat="TwentyFourHour" />--%>


                </td>


                <%--    <td>
                                <%--  <telerik:RadLabel ID="lblFromTime" runat="server" Text="Time"></telerik:RadLabel>
                            </td>--%>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox ID="txtPurpose" runat="server" CssClass="input_mandatory" Width="360px" Resize="Both"
                        Height="60px" TextMode="MultiLine">
                    </telerik:RadTextBox>

                    <telerik:RadTextBox ID="txtFromTime" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50px" Visible="false" />
                    <%--    <ajaxtoolkit:maskededitextender id="txtFromTimeMask" runat="server" acceptampm="false"
                        clearmaskonlostfocus="false" cleartextoninvalid="true" mask="99:99" masktype="Time"
                        targetcontrolid="txtFromTime" usertimeformat="TwentyFourHour" />--%>

                </td>
                      <td>
                    <telerik:RadLabel ID="lbreim" runat="server" Text="Reimbursement currency  </br>
                                Claim amount </br>
                                Reimbursement amount"></telerik:RadLabel>                    
                </td>
                <td>
                    <telerik:RadTextBox ID="txtClaimCurrencycode" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="25%"></telerik:RadTextBox></br>
                                  <telerik:RadTextBox ID="txtClaimamount" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="25%"></telerik:RadTextBox></br>
                                  <telerik:RadTextBox ID="txtReimbursementamount" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="25%"></telerik:RadTextBox>

                </td>
            </tr>
            <tr>
                <td>
                    <%-- <telerik:RadLabel ID="lblTravelHours" runat="server" Text="Travel Hours"></telerik:RadLabel>--%>

                </td>
                <td>
                    <eluc:Number ID="txtTravelHours" runat="server" CssClass="readonlytextbox" ReadOnly="true" DecimalPlace="2" IsPositive="true" Visible="false" />
                </td>
                <td />
                <td />
            </tr>
            <tr>
                <td colspan="4">
                    <hr style="height: -15px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCreatedBy" runat="server" Text="Created By"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCreatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblCreatedDate" runat="server" Text="Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucCreatedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblcm" runat="server" Text="Cancellation remarks"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCancelremarks" runat="server" Width="360px" ReadOnly="true" Resize="Both"
                        Height="60px" TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCompletedBy" runat="server" Text="Completed By"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox ID="txtCompletedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblCompletedDate" runat="server" Text="Date"></telerik:RadLabel>

                </td>
                <td>
                    <eluc:Date ID="ucCompletedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAccountVoucherNo" runat="server" Text="Account Voucher Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtAccountVoucherNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="50%" />
                </td>
                <td>
                    <telerik:RadLabel ID="Literal2" runat="server" Text="PIC Name"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListFleetManager1">
                        <telerik:RadTextBox ID="txtPICName" runat="server" MaxLength="100"
                            Width="60%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txttemp" runat="server" CssClass="hidden" Enabled="false"
                            MaxLength="30" Width="5px" ReadOnly="true">
                        </telerik:RadTextBox>

                        <asp:ImageButton runat="server" ID="imgPIC" Style="cursor: pointer; vertical-align: top"
                            ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListFleetManager1', 'codehelp1', '', '../Common/CommonPickListEmployeeAccount.aspx', true); "
                            ToolTip="Select PIC" />
                        <telerik:RadTextBox runat="server" ID="txtpicid" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtmail" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
        </table>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
