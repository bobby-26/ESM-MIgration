<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCrewWageComponentsList.aspx.cs"
    Inherits="RegistersCrewWageComponentsList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Budget" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="GWC" Src="~/UserControls/UserControlGlobalWageComponent.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Wage Components</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewWageComponents" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewWageComponents" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuCWC" runat="server" OnTabStripCommand="CWC_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortcode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtShortCode" runat="server" CssClass="input_mandatory" MaxLength="3"
                            ToolTip="Enter Code" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="200"
                            ToolTip="Enter Component Name" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCalculationBasis" runat="server" Text="Calculation Basis"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucCalculationBasis" runat="server" HardTypeCode="72" CssClass="input_mandatory"
                            AppendDataBoundItems="true" Width="180px" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblPayableBasis" runat="server" Text="Payable Basis"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucPayableBasis" runat="server" HardTypeCode="72" CssClass="input_mandatory"
                            AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIncludedOnboard" runat="server" Text="Included Onboard"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblIncludedOnboard" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Yes" Value="1" />
                                <telerik:ButtonListItem Text="No" Value="0" />
                            </Items>
                        </telerik:RadRadioButtonList>

                    </td>

                    <td>
                        <telerik:RadLabel ID="lblEarningDeduction" runat="server" Text="Earning/Deduction"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblEarningDeduction" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Earning" Value="1" />
                                <telerik:ButtonListItem Text="Deduction" Value="-1" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPostingBudgetCode" runat="server" Text="Posting Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Budget ID="ucPostingBudget" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            Width="180px" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblChargingBudgetCode" runat="server" Text="Charging Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Budget ID="ucChargingbudget" runat="server" AppendDataBoundItems="true" CssClass="input"
                            Width="180px" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowtoOwner" runat="server" Text="Contract Setting"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlContact" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Contract" Filter="Contains" MarkFirstMatch="true" CssClass="input_mandatory" Width="180px">
                            <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--select--" />
                                <telerik:RadComboBoxItem Value="0" Text="Main" />
                                <telerik:RadComboBoxItem Value="1" Text="SideLetter1" />
                                <telerik:RadComboBoxItem Value="2" Text="SideLetter2" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGlobalWageComponent" runat="server" Text="Global Wage Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:GWC ID="ucGlobalWageComponent" runat="server" AppendDataBoundItems="true" CssClass="input" Width="180px" />
                    </td>                   
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveyn" runat="server" Text="Active ?"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblActiveyn" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Yes" Value="1" />
                                <telerik:ButtonListItem Text="No" Value="0" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblsignonoff" runat="server" Text="Show to Owner at Sign Off"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblShowowneratsignoff" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Yes" Value="1" />
                                <telerik:ButtonListItem Text="No" Value="0" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="input" TextMode="MultiLine"
                            Width="180px">
                        </telerik:RadTextBox>
                    </td>    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Is Offer Letter Check"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblIsCheckOfferLetter" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Yes" Value="1" />
                                <telerik:ButtonListItem Text="No" Value="0" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
