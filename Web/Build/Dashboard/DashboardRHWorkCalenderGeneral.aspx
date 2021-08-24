<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardRHWorkCalenderGeneral.aspx.cs"
    Inherits="DashboardRHWorkCalenderGeneral" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ShipCalendar" Src="~/UserControls/UserControlMultiColumnShipCalendar.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Day</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComment" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
        <eluc:TabStrip ID="MenuWorkHour" runat="server" OnTabStripCommand="MenuWorkHour_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="Label4" runat="server" EnableViewState="false" Text="<b> Notes :</b> System accepts Advance/Retard by 10/20/40/60 minutes" BorderStyle="None" ForeColor="Blue"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCalendar" runat="server" Text="Ship Calendar"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:ShipCalendar ID="ucCalendar" runat="server" Width="180px" OnTextChangedEvent="ucCalendar_TextChangedEvent"
                            CssClass="input_mandatory" AutoPostBack="true" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAdvanceRetard" runat="server" Text="Clock"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<eluc:Date ID="txtNextDate" runat="server" CssClass="input" TimeProperty="true" Width="180px" />--%>
                        <telerik:RadDatePicker runat="server" ID="txtNextDate" Width="90px" AutoPostBack="true"
                            OnSelectedDateChanged="txtNextDate_SelectedDateChanged" Enabled="false">
                        </telerik:RadDatePicker>
                        <telerik:RadTimePicker runat="server" ID="txtNextDateTime" Enabled="true" Width="90px"></telerik:RadTimePicker>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMinutes" runat="server" Text="Advance/Retard by"></telerik:RadLabel>
                    </td>
                    <td>

                        <%--<telerik:RadTextBox ID="txtMinutes" runat="server"  Width="40px" ncopy="return false" onpaste="return false" oncut="return false" MaxLength="3">
                    <ClientEvents OnKeyPress="keyPress" />
                </telerik:RadTextBox >Mins
                    <telerik:RadCodeBlock runat="server">
                        <script type="text/javascript">

                            function keyPress(sender, args) {
                                var text = sender.get_value() + args.get_keyCharacter();
                                if (!text.match('^[0-9]+$'))
                                    args.set_cancel(true);
                            }

                        </script>
                    </telerik:RadCodeBlock>--%>
                        <telerik:RadDropDownList ID="ddlMinutes" runat="server" Width="90px">
                            <Items>
                                <telerik:DropDownListItem Value="Dummy" Text="--Select--" />
                                <telerik:DropDownListItem Value="10" Text="10" />
                                <telerik:DropDownListItem Value="20" Text="20" />
                                <telerik:DropDownListItem Value="30" Text="30" />
                                <telerik:DropDownListItem Value="40" Text="40" />
                                <telerik:DropDownListItem Value="60" Text="60" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rbnhourchange" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Advance" Value="1" />
                                <telerik:ButtonListItem Text="Retard" Value="2" />
                                <telerik:ButtonListItem Text="Reset" Value="0" Selected="true" />
                            </Items>
                        </telerik:RadRadioButtonList>

                    </td>

                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselStatus" runat="server" Text="Vessel Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rbtnworkplace" runat="server" Direction="Horizontal" Enabled="false">
                            <Items>
                                <telerik:ButtonListItem Text="In Port" Value="1" />
                                <telerik:ButtonListItem Text="At Sea" Value="2" />
                                <telerik:ButtonListItem Text="Sea/Port" Value="3" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIDL" runat="server" Text="IDL"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="rbtnadvanceretard" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="IDL W-E" Value="1" />
                                <telerik:ButtonListItem Text="IDL E-W" Value="2" />
                                <telerik:ButtonListItem Text="None" Value="0" />
                            </Items>
                        </telerik:RadRadioButtonList>

                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShip" runat="server" Text="Ship Mean Time" Visible="false"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadDropDownList runat="server" ID="ddlShipMean" Width="40px" Visible="false">
                            <Items>
                                <telerik:DropDownListItem Value="1" Text="+" />
                                <telerik:DropDownListItem Value="-1" Text="-" />
                            </Items>
                        </telerik:RadDropDownList>
                        <telerik:RadNumericTextBox ID="txtShipMean" runat="server" Width="50px" MaxValue="14" MinValue="0" Visible="false">
                            <IncrementSettings InterceptArrowKeys="false" InterceptMouseWheel="false" />
                            <NumberFormat DecimalDigits="1" />
                        </telerik:RadNumericTextBox>

                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
