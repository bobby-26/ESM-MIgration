<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDirectNonConformityMSCATAdd.aspx.cs" Inherits="InspectionDirectNonConformityMSCATAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
        .CheckBoxList {
            width: 99% !important;
        }

        .RadioButton {
            width: 99% !important;
        }
        .rbText {
            text-align: left;
            width: 89% !important;
        }

        .rbVerticalList {
            width: 32% !important;
        }
    </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmMSCATAdd" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblCauseList" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
            <eluc:TabStrip ID="MenuMSCAT" runat="server" OnTabStripCommand="MenuMSCAT_TabStripCommand" Title="Add"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblCauseSearch" width="99.5%" visible="false">
                <tr>
                    <td></td>
                    <td>
                        <eluc:Hard ID="ucCategory" runat="server" CssClass="input" OnTextChangedEvent="BindIC" AppendDataBoundItems="true"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 208) %>' HardTypeCode="208" AutoPostBack="true" Visible="false" />

                    </td>
                </tr>
            </table>
            <table width="99.5%" id="tblCauseList" cellpadding="0" cellspacing="0" border="1">
                <tr>
                    <td>
                        <b>
                            <asp:Literal ID="lblCAR" runat="server" Text="CAR"></asp:Literal></b>
                    </td>
                </tr>
                <tr valign="top">
                    <td width="35%" align="left">
                        <asp:CheckBoxList ID="chkCAR" runat="server" RepeatDirection="Vertical" RepeatColumns="3"></asp:CheckBoxList>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <asp:Literal ID="lblImmediateCauses" runat="server" Text="Immediate Causes"></asp:Literal></b>
                    </td>
                </tr>
                <tr valign="top">
                    <td width="35%" align="left">
                        <asp:RadioButtonList ID="rblIC" runat="server" AutoPostBack="true" RepeatColumns="3" RepeatDirection="Vertical" OnSelectedIndexChanged="rblIC_Changed">
                        </asp:RadioButtonList>
                        <br />
                        <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal><br />
                        <asp:TextBox ID="txtICRemarks" runat="server" CssClass="input" TextMode="MultiLine" Rows="4" Width="800px"></asp:TextBox>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <asp:Literal ID="lblBasicCauses" runat="server" Text="Basic Causes"></asp:Literal></b>
                    </td>
                </tr>
                <tr>
                    <td width="35%" align="left">
                        <asp:RadioButtonList ID="rblBC" runat="server" AutoPostBack="true" RepeatColumns="3" RepeatDirection="Vertical" OnSelectedIndexChanged="rblBC_Changed">
                        </asp:RadioButtonList>
                        <br />
                        <asp:Literal ID="lblRemark" runat="server" Text="Remarks"></asp:Literal><br />
                        <asp:TextBox ID="txtBCRemarks" runat="server" CssClass="input" TextMode="MultiLine" Rows="4" Width="800px"></asp:TextBox>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <asp:Literal ID="lblControlActionNeeds" runat="server" Text="Control Action Needs"></asp:Literal></b>
                    </td>
                </tr>
                <tr>
                    <td width="30%" align="left">
                        <asp:RadioButtonList ID="rblCAN" runat="server" RepeatColumns="3" RepeatDirection="Vertical" AutoPostBack="true">
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
