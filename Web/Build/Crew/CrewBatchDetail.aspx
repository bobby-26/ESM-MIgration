<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBatchDetail.aspx.cs"
    Inherits="CrewBatchDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="InstituteAddress" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Batch Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBatchDetails" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlBatch">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Status runat="server" ID="ucStatusConf" />
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblHeadingBatchDetails" runat="server" Text="Batch Details"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBatchDetails" runat="server" OnTabStripCommand="BatchDetails_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureBatchDetails" cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Course ID="ucCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Enabled="false" />
                            </td>
                            <td>
                                <asp:Literal ID="lblCourseType" runat="server" Text="Course Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard runat="server" ID="ucCourseType" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                    Enabled="false" HardTypeCode="103" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBatchNo" runat="server" Text="Batch No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBatchNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblnote" runat="server" CssClass="guideline_text">Note: For cancelling a batch please select "cancelled" in the "status type" drop down and click "save" </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <b><asp:Literal ID="lblBatchDetails" runat="server" Text="Batch Details"></asp:Literal></b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblStatusType" runat="server" Text="Status Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard runat="server" ID="ucStatus" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    HardTypeCode="152" AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblStatusDate" runat="server" Text="Status Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtStatusDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCommencementDate" runat="server" Text="Commencement Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucStartTime" runat="server" />
                                <asp:Literal ID="lblCompletionDate" runat="server" Text="Completion Date"></asp:Literal>
                                <eluc:Date ID="ucEndTime" runat="server" />
                            </td>
                            <td>
                                <asp:Literal ID="lblStartTime" runat="server" Text="Start Time"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartTime" runat="server" CssClass="input_mandatory" Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="txtTimeMaskEdit" runat="server" TargetControlID="txtStartTime"
                                    Mask="99:99" MaskType="Time" AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" />(hrs)
                                <asp:Literal ID="lblEndTime" runat="server" Text="End Time"></asp:Literal>
                                <asp:TextBox ID="txtEndTime" runat="server" CssClass="input_mandatory" Width="50px" />
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="txtEndTime"
                                    Mask="99:99" MaskType="Time" AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" />(hrs)
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <font color="blue"><asp:Literal ID="lblForenteringtimeEnterPforPMAforAM" runat="server" Text="For entering time-Enter P for PM,A for AM"></asp:Literal></font>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblMinimumParticipants" runat="server" Text="Minimum Participants"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtMinimumParticipant" runat="server" CssClass="input_mandatory txtNumber"
                                    MaxLength="4" IsInteger="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblMaximumParticipants" runat="server" Text="Maximum Participants"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtMaximumParticipant" runat="server" CssClass="input_mandatory txtNumber"
                                    MaxLength="4" IsInteger="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblLanguage" runat="server" Text="Language"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick runat="server" ID="ucLanguage" AppendDataBoundItems="true" QuickTypeCode="22"
                                    CssClass="input" />
                            </td>
                            <td>
                                <asp:Literal ID="lblNotes" runat="server" Text="Notes"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" runat="server" CssClass="input" TextMode="MultiLine" Height="60px"
                                    Width="320px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b><asp:Literal ID="lblSelectDepartment" runat="server" Text="Select Department"></asp:Literal></b>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnSelectedIndexChanged="BindSignatory">
                                    <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblFinalsignatory" runat="server" Text="Final signatory"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSignatory" runat="server" AppendDataBoundItems="true" CssClass="input">
                                    <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="lblCourseOfficer" runat="server" Text="Course Officer"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCourseOfficer" runat="server" AppendDataBoundItems="true"
                                    CssClass="input">
                                    <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <b><asp:Literal ID="lblBatchVenue" runat="server" Text="Batch Venue"></asp:Literal></b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVenue" runat="server" Text="Venue"></asp:Literal>
                            </td>
                            <td colspan="2">
                                <eluc:Address runat="server" ID="ucBatchVenue" CssClass="dropdown_mandatory" AddressType="138"
                                    AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>' AppendDataBoundItems="true"
                                    AutoPostBack="true" OnTextChangedEvent="BindVenueDetails" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblAddress" runat="server" Text="Address"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVenueAddress" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                    Height="60px" Width="320px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblPrimaryContact" runat="server" Text="Primary Contact"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVenuePrimaryContact" runat="server" CssClass="readonlytextbox"
                                    ReadOnly="true" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCity" runat="server" Text="City"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVenueCity" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="220px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblPhoneNo" runat="server" Text="Phone No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVenuePhoneno" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblState" runat="server" Text="State"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVenueState" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="220px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblEMail" runat="server" Text="E-Mail"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVenueEmail" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCountry" runat="server" Text="Country"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVenueCountry" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="220px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblPostalCode" runat="server" Text="Postal Code"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVenuePostalCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
