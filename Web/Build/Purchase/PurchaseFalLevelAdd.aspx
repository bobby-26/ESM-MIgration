<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFalLevelAdd.aspx.cs" Inherits="PurchaseFalLevelAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Group" Src="~/UserControls/UserControlWFGroup.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Target" Src="~/UserControls/UserControlWFTarget.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fal Level Add</title>
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
            <eluc:TabStrip ID="MenuFalLevelAdd" runat="server" OnTabStripCommand="MenuFalLevelAdd_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
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
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Active"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkRequiredYN" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
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
                        <telerik:RadLabel ID="RadLabel8" runat="server" Text="Form Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="UcFormType" runat="server" AutoPostBack="true" Width="180" CssClass="input_mandatory"
                            AppendDataBoundItems="true" QuickTypeCode="152" />

                    </td>
                </tr>


            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
