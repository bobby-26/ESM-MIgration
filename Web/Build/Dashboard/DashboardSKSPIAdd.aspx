<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKSPIAdd.aspx.cs" Inherits="Inspection_InspectionSPIAdd" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create SPI</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/Phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <div style="margin-left: 0px">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="Tabstripspiaddmenu" runat="server" OnTabStripCommand="spiaddmenu_TabStripCommand"
                TabStrip="true" />
            <br />
              <table style="margin-left: 20px">


                <tr>
                    <td>

                        <telerik:RadLabel runat="server" Text="ID" />

                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadTextBox ID="Radspiidentry" runat="server" Width="120px" CssClass="input_mandatory" MaxLength="7">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>

                        <telerik:RadLabel runat="server" Text="Short Code" />

                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadTextBox ID="Radspishortcodeentry" runat="server" Width="120px" CssClass="input_mandatory" MaxLength="3">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>

                        <telerik:RadLabel runat="server" Text="Title" />

                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadTextBox ID="Radspinameentry" runat="server" Width="240px" CssClass="input_mandatory" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                   <tr>
                    <td>

                        <telerik:RadLabel runat="server" Text="Description" />

                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadTextBox ID="Radspidescriptionentry" runat="server" Width="240px" CssClass="input" TextMode="MultiLine" Rows="6">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
