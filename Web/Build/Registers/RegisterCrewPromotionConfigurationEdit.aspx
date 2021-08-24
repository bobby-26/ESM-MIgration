<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCrewPromotionConfigurationEdit.aspx.cs" Inherits="RegisterCrewPromotionConfigurationEdit" %>

<!DOCTYPE html>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Promotion Copy</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>

<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%" />
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="CrewPromotionMenu" runat="server" OnTabStripCommand="CrewPromotionMenu_TabStripCommand"></eluc:TabStrip>
            <table id="table1" rules="server" cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td colspan="6">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblRankFrom" runat="server" Text="Rank From"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Rank ID="ucRankFrom" runat="server" Width="200px" AutoPostBack="true" AppendDataBoundItems="true" OnTextChangedEvent="ucRankFrom_TextChangedEvent"
                                        CssClass="input_mandatory" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRankTo" runat="server" Text="Rank To"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox DropDownPosition="Static" CssClass="dropdown_mandatory" ID="ucRankTo" runat="server" EnableLoadOnDemand="True" OnTextChanged="ucRankTo_TextChangedEvent"
                                        EmptyMessage="Type to select Rank" Filter="Contains" Width="200px" AppendDataBoundItems="true" 
                                        AutoPostBack="true" MarkFirstMatch="true">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblAppraisal" runat="server" Text="Recommended Appraisal"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtAppraisal" runat="server" Width="25%" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblLicence" runat="server" Text="Certificate of Competency" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadListBox ID="chkListLicence" RepeatDirection="Vertical" runat="server" Height="155px" CheckBoxes="true" Width="90%">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblTasks" runat="server" Text="Task" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadListBox ID="chkListTasks" RepeatDirection="Vertical" runat="server" Height="155px" CheckBoxes="true" Width="90%">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblRankExpTitle" runat="server" Text="Experience in Rank" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td colspan="6">
                        <table>
                            <tr>
                                <td style="width: 30%">
                                    <telerik:RadLabel ID="lblAvailable" runat="server" Text="Available" Font-Bold="true"></telerik:RadLabel>
                                </td>
                                <td style="width: 5%"></td>
                                <td style="width: 30%">
                                    <telerik:RadLabel ID="lblSelected" runat="server" Text="Selected" Font-Bold="true"></telerik:RadLabel>
                                </td>
                                <td style="width: 5%"></td>
                                <td style="width: 30%"></td>
                            </tr>
                            <tr>
                                <td style="width: 30%">
                                    <telerik:RadListBox ID="lstExpinRank" runat="server" Width="100%" CheckBoxes="true" Height="90px" SelectionMode="Multiple"></telerik:RadListBox>
                                </td>
                                <td style="width: 5%">
                                    <asp:Button ID="btnExpinRankSelect" runat="server" Text=">>" OnClick="btnExpinRankSelect_Click" />
                                    <br />
                                    <br />
                                    <asp:Button ID="btnExpinRankDeselect" runat="server" Text="<<" OnClick="btnExpinRankDeselect_Click" />
                                </td>
                                <td style="width: 30%">
                                    <telerik:RadListBox ID="lstSelectedExpinRank" runat="server" Width="100%" CheckBoxes="true" Height="90px"
                                        SelectionMode="Multiple">
                                    </telerik:RadListBox>
                                </td>
                                <td style="width: 5%">
                                    <eluc:Number ID="txtRankExp" runat="server" Width="100%" />
                                </td>
                                <td style="width: 30%">
                                    <telerik:RadLabel ID="lblRankexp" runat="server" Text="months experience in ANY of these ranks"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6"></td>
                </tr>
                <%--  <tr>
                    <td rowspan="2" style="width: 20%">
                        <telerik:RadLabel ID="lblExpinVesseltype" runat="server" Text="Experience on Vessel Type"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%">
                        <asp:ListBox ID="lstExpinVesseltype" runat="server" Width="200px"
                            SelectionMode="Multiple"></asp:ListBox>
                    </td>
                    <td style="width: 5%">
                        <asp:Button ID="btnExpinVTSelect" runat="server" Text=">>" OnClick="btnExpinVTSelect_Click" />
                        <br />
                        <br />
                        <asp:Button ID="btnExpinVTDeselect" runat="server" Text="<<" OnClick="btnExpinVTDeselect_Click" />
                    </td>
                    <td style="width: 20%">
                        <asp:ListBox ID="lstSelectedExpinVesseltype" runat="server" Width="200px"
                            SelectionMode="Multiple"></asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <eluc:Number ID="txtVesseltypeExp" runat="server" Width="30px" />
                        <telerik:RadLabel ID="lblVesseltypeExp" runat="server" Text="months experience in ANY of these vessel types"></telerik:RadLabel>
                    </td>
                </tr>--%>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
