<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterTrainingScenarioAdd.aspx.cs" Inherits="Registers_RegisterTrainingScenarioAdd" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create a Training-Scenario</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
     <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <div style="margin-left:0px">
     <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <eluc:TabStrip ID="Tabstripscenarioaddmenu" runat="server" OnTabStripCommand="drillscenarioaddmenu_TabStripCommand"
                TabStrip="true" />
            <br />
        <table style="margin-left:20px">
             <tbody>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadlblTrainingname" runat="server" Text="Training " />
                    </td>
                    <th>
                         &nbsp&nbsp&nbsp
                    </th>
                    <td>
                        <telerik:RadComboBox ID="radcombotraining" runat="server" CssClass="input_mandatory"
                            EmptyMessage="Type to select Training" AllowCustomText="true"  Width="250px"
                             />
                    </td>
                </tr>
                 <tr>
                     <td colspan="3" >
                         <br />
                     </td>
                 </tr>
                 <tr>
                     <td>
                        <telerik:RadLabel  runat="server" Text="Scenario " />
                    </td>
                     <th>
                        : &nbsp&nbsp&nbsp
                    </th>
                     <td>
                         <telerik:RadTextBox ID="Radtxtscenario" runat="server" CssClass="input_mandatory" 
                                Width="250px" TextMode="MultiLine" Rows="2">
                             </telerik:RadTextBox>
                     </td>
                 </tr>
                 <tr>
                     <td colspan="3" >
                         <br />
                     </td>
                 </tr>
                 <tr>
                     <td>
                        <telerik:RadLabel  runat="server" Text="Description " />
                    </td>
                     <th>
                         &nbsp&nbsp&nbsp
                    </th>
                     <td>
                         <telerik:RadTextBox ID="radtbdescription" runat="server" CssClass="input" 
                                Width="250px" TextMode="MultiLine" Rows="2">
                             </telerik:RadTextBox>
                     </td>
                 </tr>
            </tbody>
            </table>
    </div>
    </form>
</body>
</html>
