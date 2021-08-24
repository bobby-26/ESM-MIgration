<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseRulesConfigAdd.aspx.cs" Inherits="PurchaseRulesConfigAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rules" Src="~/UserControls/UserControlPurchaseRules.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Rules Config Add</title>
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
            <eluc:TabStrip ID="MenuPurchaseRuleConfigAdd" runat="server" OnTabStripCommand="MenuPurchaseRuleConfigAdd_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Rule"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Rules ID="ucRules" runat="server" Width="180px" />
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
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Active"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkRequiredYN" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Next Level Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkNLRequiredYN" runat="server" AutoPostBack="false"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Number Of Quote"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtNumberOfQuote" runat="server" Text="" IsInteger="true" MaxLength="2"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmount" runat="server" Text="" IsInteger="true" DecimalPlace="2" MaxLength="10" Width="100px" />
                    </td>
                </tr>
                          
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"
                            EmptyMessage="Type to select Vessel" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" 
                            EnableCheckAllItemsCheckBox="true" Width="300px" >
                        </telerik:RadComboBox>
                    </td>
                </tr>


            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
