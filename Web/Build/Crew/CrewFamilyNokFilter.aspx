<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewFamilyNokFilter.aspx.cs"
    Inherits="CrewFamilyNokFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Src="../UserControls/UserControlNationalityList.ascx" TagName="UserControlNationalityList" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRankList.ascx" TagName="UserControlRankList" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlZone.ascx" TagName="UserControlZone" TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlRank.ascx" TagName="UserControlRank" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Family/NOK Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuActivityFilterMain" runat="server" OnTabStripCommand="NewApplicantFilterMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Visible="false" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblNote" Font-Bold="true" Text="Note: For embeded search, use '%' symbol. (Eg. Name: %xxxx)" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNOK" Text="Name (Family/NOK)" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNOKName" runat="server" Width="240px" MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" Text="Name" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Width="240px" MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFileNumber" Text="File No." runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNumber" runat="server" Width="240px" MaxLength="10"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumber" Text="Passport" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassortNo" runat="server" Width="240px" MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCDCNumber" Text="CDC No." runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanbookNo" runat="server" Width="240px" MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblZone" Text="Zone" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlZone ID="ddlZone" runat="server" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                    <td colspan="1">
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td colspan="1">
                        <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblNationality" Text="Nationality" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlNationalityList ID="lstNationality" runat="server"
                            Width="240px" AppendDataBoundItems="true" />
                        <br />
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPresentRank" Text="Present Rank" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRankList ID="lstRank" runat="server" AppendDataBoundItems="true"
                            Width="240px" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveInActive" Text="Active / In-Active" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlInActive" AutoPostBack="true" EnableLoadOnDemand="true" Filter="Contains" MarkFirstMatch="true"
                            Width="240px" OnSelectedIndexChanged="rblInActive_SelectedIndex">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="All" />
                                <telerik:RadComboBoxItem Value="1" Text="Active" />
                                <telerik:RadComboBoxItem Value="0" Text="In-Active" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" Text="Status" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadListBox ID="lstStatus" runat="server" SelectionMode="Multiple" 
                            Height="80px" Width="240px" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"></telerik:RadListBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
