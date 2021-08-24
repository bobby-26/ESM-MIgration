<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreBudgetAddEdit.aspx.cs" Inherits="CrewOffshore_CrewOffshoreBudgetAddEdit" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
             <eluc:TabStrip ID="MenuCrewBudget" runat="server" OnTabStripCommand="MenuCrewBudget_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div>
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblffectiveDate" runat="server" Text="Effective Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date  ID="ucEffectiveDate" runat="server" Text="" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text="Effective Date"></telerik:RadLabel>
                        </td>
                        
                        <td><eluc:Currency  ID="ucCurrency" runat="server" CssClass="input" /></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOverlapWage" runat="server" Text="Overlap Wage"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number  ID="ucOverlapWage" runat="server" DecimalPlace="2" CssClass="input" Text="" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTankCleanAllowance" runat="server" Text="Tank Clean Allowance"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number  ID="ucTankCleanAllowance" runat="server" DecimalPlace="2" CssClass="input" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDPAllowance" runat="server" Text="DP Allowance"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number  ID="ucDPAllowance" runat="server" DecimalPlace="2" CssClass="input" Text="" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOtherAllowance" runat="server" Text="Other Allowance"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number  ID="ucOtherAllowance" runat="server" DecimalPlace="2" CssClass="input" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblappremarks" runat="server" Text="Appointment Remarks"></telerik:RadLabel>

                        </td>
                        <td colspan="3">
                            <telerik:RadEditor ID="txtRemarks" runat="server" Width="99%" EmptyMessage="" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                            </telerik:RadEditor>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblofferremark" runat="server" Text="Appointment Remarks"></telerik:RadLabel>

                        </td>
                        <td colspan="3">
                            <telerik:RadEditor ID="txtOfferRemarks" runat="server" Width="99%" EmptyMessage="" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                            </telerik:RadEditor>
                        </td>
                    </tr>
                </table>

            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
