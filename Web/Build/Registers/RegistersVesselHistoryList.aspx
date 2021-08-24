<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselHistoryList.aspx.cs" Inherits="RegistersVesselHistoryList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeniorityScale" Src="~/UserControls/UserControlSeniorityScale.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel History List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmVesselHistoryList" DecoratedControls="All" />
    <form id="frmVesselHistoryList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuVesselHistoryList" runat="server" OnTabStripCommand="VesselHistoryList_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 100px">
                        <telerik:RadLabel ID="lblFieldName" runat="server" Text="Field Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucHistoryModification" AppendDataBoundItems="true"
                            CssClass="input_mandatory" AutoPostBack="true" HardTypeCode="110" OnTextChangedEvent="History_Changed" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblValue" Text="Value" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVesselName" CssClass="input_mandatory" Width="240px"></telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtVesselCode" CssClass="input_mandatory" MaxLength="10" Width="240px"></telerik:RadTextBox>
                        <eluc:Address runat="server" ID="ucOwner" AddressType="" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" />
                        <eluc:Address runat="server" ID="ucPrincipal" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            AddressType="" Width="240px" />
                        <eluc:Address runat="server" ID="ucManager" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            AddressType="" Width="240px" />
                        <eluc:Flag runat="server" ID="ucFlag" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="240px" />
                        <telerik:RadDropDownList runat="server" ID="drpActiveYN" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            AutoPostBack="true" OnSelectedIndexChanged="drpActiveYN_Changed" Width="240px">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="" />
                                <telerik:DropDownListItem Text="In-Active" Value="0" />
                                <telerik:DropDownListItem Text="Active" Value="1" />
                            </Items>
                        </telerik:RadDropDownList>
                        <eluc:Hard runat="server" ID="ucManagementType" AppendDataBoundItems="true" Width="240px"
                            CssClass="input_mandatory" AutoPostBack="true" HardTypeCode="31" />

                        <table width="100%" runat="server" id="tblWageScale">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblOfficer" runat="server" Text="Officer"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Address runat="server" ID="ucOfficerWageScale" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                        AddressType="" Width="240px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblRatings" runat="server" Text="Ratings"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Address runat="server" ID="ucRatingsWageScale" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                        AddressType="" Width="240px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblSeniority" runat="server" Text="Seniority"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:SeniorityScale runat="server" ID="ucSeniorityWageScale" CssClass="input" AppendDataBoundItems="true" Width="240px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblStandardWageComponents" runat="server" Text="Standard Wage Components"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Hard runat="server" ID="ucESMStdWage" AppendDataBoundItems="true"
                                        CssClass="input" AutoPostBack="true" HardTypeCode="156" Width="240px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblESMHandOverDate" Text="Hand Over Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtESMHandOverDate" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWEF" runat="server" Text="W.E.F"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtWEF" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPortofChange" runat="server" Text="Port of Change"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ucPortRegistered" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" Width="240px" CssClass="input_mandatory"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
