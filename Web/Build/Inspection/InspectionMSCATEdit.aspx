<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMSCATEdit.aspx.cs" Inherits="InspectionMSCATEdit" %>

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
    <form id="frmMSCATEdit" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblCauseList" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuMSCAT" runat="server" OnTabStripCommand="MenuMSCAT_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblCauseSearch" width="100%" visible="false">
                <tr>
                    <td></td>
                    <td>
                        <eluc:Hard ID="ucCategory" runat="server" CssClass="input" OnTextChangedEvent="ucCategory_Changed" Visible="false" AppendDataBoundItems="true"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 208) %>' HardTypeCode="208" AutoPostBack="true" />
                        <%--<eluc:Hard ID="ucAccidentDescription" runat="server" CssClass="input" OnTextChangedEvent="ucAccidentDescription_Changed"
                                        HardList='<%# PhoenixRegistersHard.ListHard(1, 206) %>' HardTypeCode="206" AutoPostBack="true" />--%>
                    </td>
                </tr>
            </table>
            <table id="tblCauseList" width="100%" cellpadding="0" cellspacing="0" border="1">
                <tr>
                    <td>
                        <b>
                            <asp:Literal ID="lblCAR" runat="server" Text="CAR"></asp:Literal></b>
                    </td>
                </tr>
                <tr valign="top">
                    <td width="35%" align="left">
                        <asp:RadioButtonList ID="rblCAR" runat="server" RepeatDirection="Vertical" RepeatColumns="3"></asp:RadioButtonList>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <asp:Literal ID="lblImmediateCause" runat="server" Text="Immediate Causes"></asp:Literal></b>
                    </td>
                </tr>
                <tr valign="top">
                    <td width="25%" align="left">
                        <asp:RadioButtonList ID="rblIC" runat="server" RepeatColumns="3" RepeatDirection="Vertical" AutoPostBack="true" OnSelectedIndexChanged="rblIC_Changed">
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
                            <asp:Literal ID="lblBasicCause" runat="server" Text="Basic Causes"></asp:Literal></b>
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:RadioButtonList ID="rblBC" runat="server" RepeatColumns="3" RepeatDirection="Vertical" AutoPostBack="true" OnSelectedIndexChanged="rblBC_Changed">
                        </asp:RadioButtonList>
                        <br />
                        <asp:Literal ID="lblRemarks2" runat="server" Text="Remarks"></asp:Literal><br />
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
                    <td align="left">
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
