<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselStatutoryDuesVesselWiseNoofCrew.aspx.cs"
    Inherits="AccountsVesselStatutoryDuesVesselWiseNoofCrew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Statutory Dues For Period</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvStock.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuStatoryDuesMain" runat="server" OnTabStripCommand="MenuStatoryDuesMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>



            <eluc:TabStrip ID="MenuStatoryDues" runat="server" OnTabStripCommand="MenuStatoryDues_TabStripCommand"></eluc:TabStrip>

            <table width="75%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Literal ID="lblFromDate" runat="server" Text="From"></asp:Literal></td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <asp:Literal ID="lblToDate" runat="server" Text="To"></asp:Literal></td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblCBA" runat="server" Text="CBA"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Address ID="ddlUnion" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            AutoPostBack="true" OnTextChangedEvent="ddlUnion_Changed" AddressType="134" Width="250px" />
                    </td>
                    <td>
                        <asp:Literal ID="lblComponent" runat="server" Text="Component"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlComponent" runat="server" CssClass="dropdown_mandatory" Width="250px" Filter="Contains" EmptyMessage="Type to select"></telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <br />
          
                <eluc:TabStrip ID="MenuStock" runat="server" OnTabStripCommand="MenuStock_TabStripCommand"></eluc:TabStrip>
          
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="true" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvStock_RowDataBound"
                    ShowHeader="true" ShowFooter="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                </asp:GridView>
            </div>
        
    </form>
</body>
</html>
