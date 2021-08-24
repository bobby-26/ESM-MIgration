<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFalLevelMappingEdit.aspx.cs" Inherits="PurchaseFalLevelMappingEdit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FAL Mapping Edit</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuFalLevelMappingEdit" runat="server" OnTabStripCommand="MenuFalLevelMappingEdit_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Level Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="txtLevelName" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblstocktye" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlStockType" Width="180px">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="" />
                                <telerik:DropDownListItem Text="Spares" Value="SPARE" />
                                <telerik:DropDownListItem Text="Stores" Value="STORE" />
                                <telerik:DropDownListItem Text="Service" Value="SERVICE" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                        <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="Within Budget Minimum"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="lblInMinimum" runat="server" Text="" DecimalPlace="2" IsPositive="true" MaxLength="18" Width="100px" CssClass="input_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Within Budget Maximum"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="lblInMaximum" runat="server" Text="" DecimalPlace="2" IsPositive="true" MaxLength="18" Width="100px" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel9" runat="server" Text="Exceed Budget Minimum"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="lblExcMinimum" runat="server" Text="" DecimalPlace="2" IsPositive="true" MaxLength="18" Width="100px" CssClass="input_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel10" runat="server" Text="Exceed Budget Maximum"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="lblExcMaximum" runat="server" Text="" DecimalPlace="2" IsPositive="true" MaxLength="18" Width="100px" CssClass="input_mandatory" />
                    </td>
                </tr>
        


                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel8" runat="server" Text="Vessel List"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"
                            EmptyMessage="Type to select Vessel" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true"
                            EnableCheckAllItemsCheckBox="true" Width="180px">
                        </telerik:RadComboBox>

                    </td>
                </tr>
           
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
