<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKPIEdit.aspx.cs" Inherits="Inspection_InspectionShippingPIEdit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Performance Indicator (PI)</title>
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

            <eluc:TabStrip ID="Tabstripspiaddmenu" runat="server" OnTabStripCommand="piaddmenu_TabStripCommand"
                TabStrip="true" />
            <br />
            <table style="margin-left: 20px">
               
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Code" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadTextBox ID="Radpiidentry" runat="server" Width="150px" CssClass="input_mandatory" MaxLength="5">
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
                        <telerik:RadLabel runat="server" Text="Name" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadTextBox ID="pinameentry" runat="server" Width="350px" CssClass="input_mandatory" TextMode="MultiLine" Rows="2">
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
                        <telerik:RadLabel runat="server" Text="Unit" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td >
                        <telerik:RadComboBox ID="radcbpiunitentry" runat="server" CssClass="input_mandatory" Width="150px"
                            EmptyMessage="Type to select PI Unit" AllowCustomText="true" 
                            AutoPostBack="false" />
                    </td>
                      <td>
                           &nbsp &nbsp &nbsp
                      </td>
                      <td>
                        <telerik:RadLabel runat="server" Text="Scope" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td >
                       <telerik:RadComboBox ID="radcbpiscopeentry" runat="server" CssClass="input_mandatory" Width="180px"
                            EmptyMessage="Type to select PI Scope" AllowCustomText="true" 
                            AutoPostBack="false" />
                    </td>

                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Period" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td >
                        <eluc:Hard ID="Radcbpiperiodentry" runat="server" CssClass="input_mandatory" HardTypeCode="272" HardList='<%# PhoenixRegistersHardExtn.ListHardExtn(261,1,null, null) %>' Width="150px"
                                    AutoPostBack="false" />
                    </td>
                      <td>
                           &nbsp &nbsp &nbsp
                      </td>
                      <td>
                        <telerik:RadLabel runat="server" Text="Description" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td >
                       <telerik:RadTextBox ID="RadtbPIdescriptionentry" runat="server" TextMode="MultiLine"  Width="180px"
                             Rows="4"  CssClass="input">
                            </telerik:RadTextBox>
                    </td>

                </tr>
                </table>
             </div>
    </form>
</body>
</html>
