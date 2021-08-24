<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsOtherCrewGeneral.aspx.cs"
    Inherits="VesselAccountsOtherCrewGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Other Crew General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuOtherCrew" runat="server" OnTabStripCommand="MenuOtherCrew_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td style="width: 11%">
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 22%">
                        <telerik:RadTextBox ID="txtFirstName" runat="server" CssClass="input_mandatory" Width="98%"></telerik:RadTextBox>
                    </td>
                    <td style="width: 11%">
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 22%">
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" Width="98%"></telerik:RadTextBox>
                    </td>
                    <td style="width: 11%">
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 22%">
                        <telerik:RadTextBox ID="txtLastName" runat="server" Width="98%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignonDate" runat="server" Text="Sign On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignonDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffDate" runat="server" Text="Sign Off"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignoffDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblACTypeforSeafarers" runat="server" Text="Account Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlAccountType" runat="server" CssClass="input_mandatory" Width="98%">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="" />
                                <telerik:DropDownListItem Text="Staff Account" Value="1" />
                                <telerik:DropDownListItem Text="Owners Account" Value="-1" />
                                <telerik:DropDownListItem Text="Charterers Account" Value="-2" />
                            </Items>
                        </telerik:RadDropDownList>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignonSeaPort" runat="server" Text="Sign On Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ddlSignonport" runat="server" CssClass="dropdown_mandatory" Width="98%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignoffSeaPort" runat="server" Text="Sign Off Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ddlSignoffport" runat="server" Width="98%" />
                    </td>
                </tr>
                <tr runat="server" id="trsignoff1">
                    <td>
                        <telerik:RadLabel ID="lblSignonRemarks" runat="server" Text="Sign On Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignonRemarks" runat="server" TextMode="MultiLine"
                            Width="98%" Height="40px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignoffRemarks" runat="server" Text="Sign Off Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignoffRemarks" CssClass="input" runat="server" TextMode="MultiLine"
                            Width="98%" Height="40px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
