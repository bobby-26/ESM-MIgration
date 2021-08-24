<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKKPIAdd.aspx.cs" Inherits="Inspection_InspectionShippingKPIAdd" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Key Performance Indicator (KPI)</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
                         <telerik:RadLabel runat="server" Text="Level" />
                    </td>
                    <td>
                         &nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <div class="input_mandatory">
                            <telerik:RadRadioButtonList runat="server" ID="RadRadioButtonkpilevel" Direction="Horizontal" AutoPostBack="false">

                                <Items>
                                    <telerik:ButtonListItem runat="server" Text="Corporate" Value="Corporate" />

                                    <telerik:ButtonListItem runat="server" Text="Department" Value="Department" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp 
                    </td>
                </tr>
                  <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="ID" />
                    </td>
                    <th> &nbsp &nbsp &nbsp
                    </th>
                    <td >
                        <telerik:RadTextBox ID="Radtbkpicodeentry" runat="server" Width="100px" CssClass="input_mandatory" MaxLength="6" >
                        </telerik:RadTextBox>
                    </td>
                       <th> &nbsp &nbsp &nbsp
                    </th>
                       <td>
                        <telerik:RadLabel runat="server" Text="Department" ID="radlbldept"  />
                        
                    </td>
                        <th> &nbsp &nbsp &nbsp
                    </th>
                      <td>
                           <telerik:RadComboBox runat="server" ID="radcbdept" CssClass="input_mandatory" Width="180px"
                            EmptyMessage="Type to select Department" AllowCustomText="true" 
                              />
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
                    <th> &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadTextBox ID="Radtbkpinameentry" runat="server" Width="350px" CssClass="input_mandatory"  TextMode="MultiLine" Rows="2">
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
                    <th> &nbsp &nbsp &nbsp
                    </th>
                    <td >
                        <telerik:RadComboBox ID="radcbkpiunitentry" runat="server" CssClass="input_mandatory" Width="150px"
                            EmptyMessage="Type to select KPI Unit" AllowCustomText="true" 
                            AutoPostBack="false" />
                    </td>
                      <td>
                           &nbsp &nbsp &nbsp
                      </td>
                      <td>
                        <telerik:RadLabel runat="server" Text="Scope" />
                    </td>
                    <th> &nbsp &nbsp &nbsp
                    </th>
                    <td >
                       <telerik:RadComboBox ID="radcbkpiscopeentry" runat="server" CssClass="input_mandatory" Width="180px"
                            EmptyMessage="Type to select KPI Scope" AllowCustomText="true" 
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
                    <th> &nbsp &nbsp &nbsp
                    </th>
                    <td >
                        <eluc:Hard ID="Radcbkpiperiodentry" runat="server" CssClass="input_mandatory" HardTypeCode="272" HardList='<%# PhoenixRegistersHardExtn.ListHardExtn(261,1,null, null) %>' Width="150px"
                                    AutoPostBack="false" />
                    </td>
                      <td>
                           &nbsp &nbsp &nbsp
                      </td>
                      <td>
                        <telerik:RadLabel runat="server" Text="Description" />
                    </td>
                    <th> &nbsp &nbsp &nbsp
                    </th>
                    <td >
                       <telerik:RadTextBox ID="RadtbkPIdescriptionentry" runat="server" TextMode="MultiLine"  Width="180px"
                                Rows="4" CssClass="input">
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
