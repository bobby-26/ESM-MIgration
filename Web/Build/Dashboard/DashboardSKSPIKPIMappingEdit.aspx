 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKSPIKPIMappingEdit.aspx.cs" Inherits="Inspection_ShippingSPIKPIMappingEdit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit SPI-KPI Mapping</title>
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
                    <th>
                        <telerik:RadLabel runat="server" Text="SPI" />
                    </th>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                       <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="Radcombospilist" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDSPITITLE" DataValueField="FLDSHIPPINGSPIID" DropDownWidth="300px"
                            Placeholder="Type to Select the SPI" Filter="Contains" FilterFields="FLDSPIID, FLDSPITITLE" CssClass="input_mandatory"
                            >
                            <NoDataTemplate>
                               
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                            </NoDataTemplate>
                            <ColumnsCollection>
                                <telerik:MultiColumnComboBoxColumn Field="FLDSPIID" Title="Code" Width="70px" />
                                <telerik:MultiColumnComboBoxColumn Field="FLDSPITITLE" Title="Name" Width="200px"  />
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
                        <telerik:RadLabel runat="server" Text="KPI" />
                    </th>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="radcobkpi" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDKPINAME" DataValueField="FLDKPIID" DropDownWidth="300px"
                            Placeholder="Type to Select the KPI" Filter="Contains" FilterFields="FLDKPICODE, FLDKPINAME" CssClass="input_mandatory"
                            >
                            <NoDataTemplate>
                               
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                            </NoDataTemplate>
                            <ColumnsCollection>
                                <telerik:MultiColumnComboBoxColumn Field="FLDKPICODE" Title="Code" Width="70px" />
                                <telerik:MultiColumnComboBoxColumn Field="FLDKPINAME" Title="Name" Width="200px" />
                            </ColumnsCollection>
                        </telerik:RadMultiColumnComboBox>
                    </td>
                </tr>
               
               
                </table>
             </div>
    </form>
</body>
</html>
