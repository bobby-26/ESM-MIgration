<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFalApproveAdd.aspx.cs" Inherits="PurchaseFalApproveAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Group" Src="~/UserControls/UserControlWFGroup.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Target" Src="~/UserControls/UserControlWFTarget.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Fal Approve Add</title>
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
            <eluc:TabStrip ID="MenuPurchaseFalApproveAdd" runat="server" OnTabStripCommand="MenuPurchaseFalApproveAdd_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblstocktye" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlStockType" Width="180px" CssClass="input_mandatory">
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
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="lblLevel" runat="server" Text="" IsInteger="true" IsPositive="true" MaxLength="3" CssClass="input_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Level Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLevelName" runat="server" Text="" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkRequiredYN" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                    </td>
                </tr>
              
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Maximum"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="lblMaximum" runat="server" Text="" DecimalPlace="2" IsPositive="true" MaxLength="10" Width="100px" CssClass="input_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Group ID="UcGroup" runat="server" AutoPostBack="true" Width="180px" CssClass="input_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel7" runat="server" Text="Target"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Target ID="UcTarget" runat="server" AutoPostBack="true" Width="180px" CssClass="input_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel8" runat="server" Text="Applier To"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"
                            EmptyMessage="Type to select Vessel" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true"
                            EnableCheckAllItemsCheckBox="true" Width="180px" >
                        </telerik:RadComboBox>

                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
