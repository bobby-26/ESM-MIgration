<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentMSCATEdit.aspx.cs" Inherits="InspectionIncidentMSCATEdit" %>

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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" EnableAJAX="false">
            <eluc:TabStrip ID="MenuMSCAT" runat="server" OnTabStripCommand="MenuMSCAT_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblCauseSearch" width="99%">
                <tr>
                    <td>
                        <asp:Literal ID="lblAccidentDescription" runat="server" Text="Accident Description" Visible="false"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Hard ID="ucAccidentDescription" runat="server" CssClass="input" OnTextChangedEvent="ucAccidentDescription_Changed"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 206) %>' HardTypeCode="206" AutoPostBack="true" Visible="false" />
                    </td>
                </tr>
            </table>
            <table id="tblCauseList" width="99.6%" cellpadding="0" cellspacing="0" border="1">
                <tr>
                    <td>
                        <b>
                            <asp:Literal ID="lblFindings" runat="server" Text="Findings"></asp:Literal></b>
                    </td>
                </tr>
                <tr valign="top">
                    <td width="200px" align="left">
                        <asp:RadioButtonList ID="rblFindings" runat="server" AutoPostBack="true" RepeatColumns="3" RepeatDirection="Vertical" OnSelectedIndexChanged="rblFindings_Changed">
                        </asp:RadioButtonList>
                        <asp:Label ID="lblContactTypeId" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <%--<td>
                                  <b>Contact Type</b>
                                </td>--%>
                    <td>
                        <b>
                            <asp:Literal ID="lblImmediateCauses" runat="server" Text="Immediate Causes"></asp:Literal></b>
                    </td>
                </tr>
                <tr>
                    <%--<td width="200px" align="left">                                    
                                    <asp:RadioButtonList ID="rblContactType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblContactType_Changed">
                                    </asp:RadioButtonList>
                                </td>--%>
                    <td width="200px" align="left">
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
                    <td width="200px" align="left">
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
                    <td width="200px" align="left">
                        <asp:RadioButtonList ID="rblCAN" runat="server" AutoPostBack="true" RepeatColumns="3" RepeatDirection="Vertical">
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
