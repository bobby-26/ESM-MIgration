<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSignOffMovementUpdate.aspx.cs"
    Inherits="AccountsSignOffMovementUpdate" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PopupMenu" Src="~/UserControls/UserControlPopupMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Movement" Src="~/UserControls/UserControlMovement.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Crew" Src="~/UserControls/UserControlCrewContract.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Update Travel Days</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function OpenRadWindow() {
                var oWnd = $find("<%= RadWindowManager1.ClientID %>");
                oWnd.show();
                //Here set the width and height of RadWindow
                oWnd.setSize(400, 100);
                oWnd.minimize();
                oWnd.maximize();
                oWnd.restore();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmmovementupdate" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
      <%--  <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" MaxHeight="330px" MaxWidth="225px" OnClientShow="OpenRadWindow">
        </telerik:RadWindowManager>--%>
          <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecorationZoneID="RadAjaxPanel1" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuMovementUpdate" runat="server" OnTabStripCommand="RegistersMovementUpdate_TabStripCommand"></eluc:TabStrip>

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" CssClass="input" Width="180px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                        </telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input" Width="180px"
                            Enabled="false" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                        </telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" Enabled="false"
                            Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblsignoffdate" runat="server" Text="Signoff Date">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtsignoffdate" runat="server" CssClass="input" Enabled="false" Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmovement" runat="server" Text="Movement">
                        </telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:Movement ID="ddlMovement" runat="server" AppendDataBoundItems="true" CssClass="input" Enabled="false" Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcontract" runat="server" Text="Contract">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Crew ID="CrewContract" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="180px"></eluc:Crew>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromdate" runat="server" Text="From Date">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromdate" runat="server" Width="180px" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTodate" runat="server" Text="To Date">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucTodate" runat="server" Width="180px" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
