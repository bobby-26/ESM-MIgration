<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementConfigurationNew.aspx.cs" Inherits="DocumentManagement_DocumentManagementConfigurationNew" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand" Title="User Configuration"></eluc:TabStrip>
            <table Width="100%">
                <tr>
                    <td valign="centre" width="10%">Group Rank
                    </td>
                       
                            <td>
                            <telerik:RadCheckBox ID="chkGrouprankAll" Font-Bold="true" runat="server" Text="Check All" AutoPostBack="true"
                                OnCheckedChanged="chkGrouprankAll_CheckedChanged" />
                            <br />
                        </td>
                     </tr>
                        <tr>
                            <td></td>
                    <td valign="top" width="90%">
                        <div id="divGroupRank" runat="server" style="height: 120px; overflow-y: auto; overflow-x: auto; width: 99.9%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <telerik:RadCheckBoxList ID="ddlGroupRank" runat="server" AutoPostBack="false"  Direction="Vertical" Columns="6">
                                </telerik:RadCheckBoxList>
                            <%--<asp:CheckBoxList ID="ddlGroupRank" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="6"
                                AutoPostBack="false">
                            </asp:CheckBoxList>--%>
                        </div>                        
                    </td>
                </tr>
                <tr>
                    <td valign="centre">Function Role
                    </td>
                    <td >
                            <telerik:RadCheckBox ID="chkCheckAll" Font-Bold="true" runat="server" Text="Check All" AutoPostBack="true"
                                OnCheckedChanged="chkCheckAll_CheckedChanged" />
                            <br />
                        </td>
                    </tr>
                        <tr>
                            <td></td>
                    <td valign="top" width="80%">
                        <div id="divDesignation" runat="server" style="height: 320px; overflow-y: auto; overflow-x: auto; width: 99.9%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                             <telerik:RadCheckBoxList ID="ddlDesignation" runat="server" AutoPostBack="false" Direction="Vertical" Columns="4" >
                                </telerik:RadCheckBoxList>
                            <%--<asp:CheckBoxList ID="ddlDesignation" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="4"
                                AutoPostBack="false">
                            </asp:CheckBoxList>--%>
                        </div>
                    </td>
                </tr>                
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
