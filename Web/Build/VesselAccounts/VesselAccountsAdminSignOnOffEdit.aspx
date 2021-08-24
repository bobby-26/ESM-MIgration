<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsAdminSignOnOffEdit.aspx.cs" Inherits="VesselAccountsAdminSignOnOffEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sign on/off</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuMainList" runat="server" OnTabStripCommand="MenuMainList_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfileno" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFile" runat="server" ReadOnly="true" Enabled="false" Text="" Width="250px" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblname" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" ReadOnly="true" Enabled="false" Text="" Width="250px" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblpassportno" runat="server" Text="Passport No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtpassportno" runat="server" ReadOnly="true" Enabled="false" Text="" Width="250px" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcdc" runat="server" Text="CDC No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCDC" runat="server" ReadOnly="true" Enabled="false" Text="" Width="250px" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblrank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" RankList="<%#PhoenixRegistersRank.ListRank() %>" Width="250px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblsignon" runat="server" Text="Sign On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOnDate" runat="server" CssClass="input_mandatory" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblrelief" runat="server" Text="Relief Due"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtReliefDueDate" runat="server" CssClass="input_mandatory" Text="" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBTo" runat="server" Text="BTO Date "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtBtoDate" runat="server" CssClass="input" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEto" runat="server" Text="ETO Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtEtoDate" runat="server" CssClass="input" Text="" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblsignoff" runat="server" Text="Sign Off"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOffDate" runat="server" CssClass="input" Text="" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignoffport" runat="server" Text="Sign Off Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ddlSeaPort" runat="server" CssClass="input" Width="250px" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignoffreason" runat="server" Text="Sign Off Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignOffReason runat="server" ID="ddlReason" CssClass="input" AppendDataBoundItems="true" Width="250px"
                            SignOffReasonList='<%#SouthNests.Phoenix.Registers.PhoenixRegistersreasonssignoff.Listreasonssignoff() %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblstatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlSignOnOffStatus" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select"  Filter="Contains" MarkFirstMatch="true" Width="250px">
                            <Items>
                                <telerik:RadComboBoxItem Value="0" Text="Sign Off" />
                                <telerik:RadComboBoxItem Value="1" Text="Sign On" />
                            </Items>
                        </telerik:RadComboBox>
                        <eluc:Status ID="Ucsstats" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
