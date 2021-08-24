<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitITVisit.aspx.cs" Inherits="AccountsVesselVisitITVisit" %>

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
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
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
    <form id="frmITVisit" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="Title1" Text="IT Visit" ShowMenu="false" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />
            <eluc:TabStrip ID="MenuITVisit" runat="server" OnTabStripCommand="MenuITVisit_TabStripCommand" Title="IT Visit" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuITVisitSub" runat="server" OnTabStripCommand="MenuITVisitSub_TabStripCommand"></eluc:TabStrip>
            <table id="Table2" width="100%" style="color: Blue">
                <tr>
                    <td>&nbsp;
                    <telerik:RadLabel ID="lblbfd" runat="server" Text="* From/ To Date include Travelling Days" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td width="12%">
                        <telerik:RadLabel ID="lblFormNo" runat="server" Text="Form No"></telerik:RadLabel>
                    </td>
                    <td>
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
                            DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" Width="250px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr style="height: -15px" />
                    </td>
                </tr>
                <tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="Literal1" runat="server" Text="Employee Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <%--  <asp:DropDownList id="ddlSubaccount" runat="server" CssClass="input_mandatory" >
                            </asp:DropDownList>--%>
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

                    </tr>

                    <td />
                    <td />
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
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFromTime" runat="server" Text="Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFromTime" runat="server" CssClass="input_mandatory" Width="50px" />
                        <%--   <ajaxtoolkit:maskededitextender id="txtFromTimeMask" runat="server" acceptampm="false"
                        clearmaskonlostfocus="false" cleartextoninvalid="true" mask="99:99" masktype="Time"
                        targetcontrolid="txtFromTime" usertimeformat="TwentyFourHour" />--%>
                    hrs
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToTime" runat="server" Text="Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtToTime" runat="server" CssClass="input_mandatory" Width="50px" />
                        <%--    <ajaxtoolkit:maskededitextender id="txtToTimeMask" runat="server" acceptampm="false"
                        clearmaskonlostfocus="false" cleartextoninvalid="true" mask="99:99" masktype="Time"
                        targetcontrolid="txtToTime" usertimeformat="TwentyFourHour" />--%>
                    hrs                           
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTravelHours" runat="server" Text="Travel Hours"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtTravelHours" runat="server" CssClass="input_mandatory" DecimalPlace="2" IsPositive="true" />
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
                        <telerik:RadTextBox ID="txtCreatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
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
                        <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPurpose" runat="server" CssClass="input_mandatory" Width="360px" Resize="Both"
                            Height="60px" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCancel" runat="server" Text="Cancellation remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCancelremarks" runat="server" CssClass="input" Width="360px" ReadOnly="true" Resize="Both"
                            Height="60px" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompletedBy" runat="server" Text="Completed By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCompletedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
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
                        <telerik:RadTextBox ID="txtAccountVoucherNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                    <td />
                    <td />
                </tr>
            </table>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
