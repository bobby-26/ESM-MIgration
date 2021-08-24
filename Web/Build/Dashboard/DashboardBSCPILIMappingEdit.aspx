<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSCPILIMappingEdit.aspx.cs" Inherits="Dashboard_DashboardBSCPILIMappingEdit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit PI-LI Mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
       <div style="margin-left: 0px">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="Tabstripspiaddmenu" runat="server" OnTabStripCommand="piaddmenu_TabStripCommand"
                TabStrip="true" />
            <br />
            <table style="margin-left: 20px">
                  <tr>
                    <th>
                        <telerik:RadLabel runat="server" Text="PI" />
                    </th>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                      <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="Radcombopilist" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDPINAME" DataValueField="FLDPIID" DropDownWidth="300px"
                            Placeholder="Type to Select the PI" Filter="Contains" FilterFields="FLDPICODE, FLDPINAME" CssClass="input_mandatory"
                            >
                            <NoDataTemplate>
                               
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                            </NoDataTemplate>
                            <ColumnsCollection>
                                <telerik:MultiColumnComboBoxColumn Field="FLDPICODE" Title="Code" Width="70px" />
                                <telerik:MultiColumnComboBoxColumn Field="FLDPINAME" Title="Name" Width="200px" />
                            </ColumnsCollection>
                        </telerik:RadMultiColumnComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>

                <tr>
                    <th>
                        <telerik:RadLabel runat="server" Text="LI" />
                    </th>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                       <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="radcobLi" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDLINAME" DataValueField="FLDLIID" DropDownWidth="300px"
                            Placeholder="Type to Select the LI" Filter="Contains" FilterFields="FLDLICODE, FLDLINAME" CssClass="input_mandatory"
                            >
                            <NoDataTemplate>
                               
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                            </NoDataTemplate>
                            <ColumnsCollection>
                                <telerik:MultiColumnComboBoxColumn Field="FLDLICODE" Title="Code" Width="70px" />
                                <telerik:MultiColumnComboBoxColumn Field="FLDLINAME" Title="Name" Width="200px" />
                            </ColumnsCollection>
                        </telerik:RadMultiColumnComboBox>
                    </td>
                </tr>
               
               
                </table>
             </div>
    </form>
</body>
</html>
