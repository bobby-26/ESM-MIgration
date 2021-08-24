<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReimbursementFilter.aspx.cs"
    Inherits="CrewReimbursementFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register Src="../UserControls/UserControlRank.ascx" TagName="Rank" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reimbursements/Recoveries Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:TabStrip ID="MenuPD" runat="server" OnTabStripCommand="PD_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td style="width: 18%;">
                        <telerik:RadLabel ID="lblFileno" Text="File No" runat="server"></telerik:RadLabel>
                    </td>
                    <td style="width: 32%;">
                        <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="input" Width="270px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 18%;">
                        <telerik:RadLabel ID="lblName" Text="Name" runat="server"></telerik:RadLabel>
                    </td>
                    <td style="width: 32%;">
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" Text="Rank" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="input" Width="270px" DataBoundItemName="--All--" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReimbursementRecovery" Text="Reimbursement/ Recovery" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" CssClass="input" ID="ddlEarDed" runat="server" EnableLoadOnDemand="True" OnSelectedIndexChanged="ddlEarDed_SelectedIndexChanged" Width="270px"
                            EmptyMessage="Type to select Reimbursement/ Recovery" Filter="Contains" AutoPostBack="true" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--All--" Value="" />
                                <telerik:RadComboBoxItem Text="Reimbursement(B.O.C)" Value="1" />
                                <telerik:RadComboBoxItem Text="Reimbursement(Monthly)" Value="2" />
                                <telerik:RadComboBoxItem Text="Reimbursement(E.O.C)" Value="3" />
                                <telerik:RadComboBoxItem Text="Recovery(B.O.C)" Value="-1" />
                                <telerik:RadComboBoxItem Text="Recovery(Monthly)" Value="-2" />
                                <telerik:RadComboBoxItem Text="Recovery(E.O.C)" Value="-3" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPurpose" Text="Purpose" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlPurpose" runat="server" CssClass="input" AppendDataBoundItems="true"
                            HardTypeCode="128" ShortNameFilter="TRV,USV,AFR,EBG,CFE,LFE,MEF" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblApproved" Text="Approved/Not Approved" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" CssClass="input" ID="ddlApproved" runat="server" EnableLoadOnDemand="True" Width="270px"
                            EmptyMessage="Type to select Approved/Not Approved" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--All--" Value="" />
                                <telerik:RadComboBoxItem Text="Approved" Value="1" />
                                <telerik:RadComboBoxItem Text="Not Approved" Value="2" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrom" Text="Date Between" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" DatePicker="true" CssClass="input" />
                        <eluc:Date ID="txtToDate" runat="server" DatePicker="true" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateapproved" Text="Approved Between" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtApprovedDateFrom" runat="server" CssClass="input" />
                        <eluc:Date ID="txtApprovedDateTo" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr id="trAccountOf" runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblAccountof" Text="Account of" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVesselAccountof" runat="server" CssClass="input" AppendDataBoundItems="true"
                            VesselsOnly="true" Width="270px" Entitytype="VSL" ActiveVessels="true" AssignedVessels="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblChargeableVessel" Text="Chargeable" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlChargeableVessel" runat="server" CssClass="input" AppendDataBoundItems="true"
                            VesselsOnly="true" Width="270px" Entitytype="VSL" ActiveVessels="true" AssignedVessels="true" />
                    </td>
                </tr>


                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPaymentMode" Text="Payment Mode" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlPaymentMode" runat="server" CssClass="input" AppendDataBoundItems="true"
                            HardTypeCode="142" Width="270px" DataBoundItemName="--All--" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblActive" Text="Status" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" CssClass="input" ID="ddlStatus" runat="server" EnableLoadOnDemand="True" Width="270px"
                            EmptyMessage="Type to select Approved/Not Approved" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--All--" Value="" />
                                <telerik:RadComboBoxItem Text="Active" Value="1" />
                                <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>

            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
