<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsComponentTaxTypeMapAdd.aspx.cs" Inherits="VesselAccounts_VesselAccountsComponentTaxTypeMapAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EntryType" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component Tax Type Map</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentTaxTypeMap" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuComponentTaxTypeMap" runat="server" OnTabStripCommand="MenuComponentTaxTypeMap_TabStripCommand"></eluc:TabStrip>
        <table id="tblComponentTaxTypeMap" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox RenderMode="Lightweight" ID="ddlType" runat="server" EmptyMessage="Type or select Type" MarkFirstMatch="true" AppendDataBoundItems="true" OnTextChanged="ddlType_TextChanged"
                        Width="180px" CssClass="input_mandatory" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="CBA" Selected="true" />
                            <telerik:RadComboBoxItem Value="2" Text="Standard Wage Component" />
                            <telerik:RadComboBoxItem Value="3" Text="Component Agreed With Crew" />

                        </Items>
                    </telerik:RadComboBox>

                </td>
                <td>
                    <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Country ID="ddlCountry" runat="server" CssClass="input_mandatory" Width="180px" AppendDataBoundItems="true"></eluc:Country>
                </td>
                <td>
                    <telerik:RadLabel ID="lblWageComponents" runat="server" Text="Wage Components"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucWageComponents" runat="server" HardTypeCode="156" AppendDataBoundItems="true" CssClass="input_mandatory"
                        AutoPostBack="true" OnTextChangedEvent="ucWageComponents_Changed" Width="180px" />
                </td>
            </tr>

            <tr>
                <td>
                    <telerik:RadLabel ID="lblUnion" runat="server" Text="Union"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Address ID="ddlUnion" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                        AutoPostBack="true" OnTextChangedEvent="ddlUnion_TextChangedEvent" AddressType="134" Width="180px" />

                </td>
                <td>
                    <telerik:RadLabel ID="lblCBARevision" runat="server" Text="CBA Revision"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox DropDownPosition="Static" ID="ddlRevision" runat="server" EnableLoadOnDemand="True" CssClass="input_mandatory" OnTextChanged="ddlRevision_TextChangedEvent" AutoPostBack="true"
                        EmptyMessage="Type to select Revision" Width="180px" DataTextField="FLDNAME" DataValueField="FLDREVISIONID" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                </td>
                <td>

                    <telerik:RadComboBox ID="ddlComponent" DropDownPosition="Static" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" CssClass="input_mandatory"
                      EnableLoadOnDemand="True" DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTID"  Width="180px">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblComponentType" runat="server" Text="Component Type"></telerik:RadLabel>
                </td>
                <td>

                    <telerik:RadComboBox ID="ddlComponentType" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" CssClass="input_mandatory"
                        Width="180px">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active YN"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkActiveYN" runat="server" AutoPostBack="false" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

