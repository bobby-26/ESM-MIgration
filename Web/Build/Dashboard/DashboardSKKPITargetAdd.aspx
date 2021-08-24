<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKKPITargetAdd.aspx.cs" Inherits="Dashboard_DashboardSKKPITargetAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>ADD Key Performance Indicators (KPI) Target </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />


        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">


            <div style="margin-left: 0px">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:TabStrip ID="Tabkpi" runat="server" OnTabStripCommand="KPI_TabStripMenuCommand" TabStrip="true"></eluc:TabStrip>
                <br />
                <table style="margin-left: 20px">
                    <tr>
                       

                        <td>
                            <telerik:RadLabel ID="Radlblyear" runat="server" Text="Year" />
                        </td>
                        <td>&nbsp &nbsp &nbsp
                        </td>
                        <td colspan="5">
                            <eluc:Year ID="radcbyear" runat="server" AutoPostBack="True" YearStartFrom="2018" NoofYearFromCurrent="0" OrderByAsc="True" CssClass="input_mandatory" />
                        </td>

                    </tr>

                    <tr>
                        <td colspan="7">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="radlblkpititle" runat="server" Text="KPI" />
                        </td>
                        <td>&nbsp &nbsp &nbsp
                        </td>
                        <td colspan="5">
                            <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="radcobkpi" runat="server" Width="290px" Height="150px" NoWrap="true"
                                DataTextField="FLDKPINAME" DataValueField="FLDKPIID" DropDownWidth="300px" CssClass="input_mandatory"
                                Placeholder="Type to Select the KPI" Filter="Contains" FilterFields="FLDKPICODE, FLDKPINAME">
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
                    <tr>
                        <td colspan="7">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Reference No." />
                        </td>
                        <td>&nbsp &nbsp &nbsp
                        </td>
                        <td>
                            <telerik:RadTextBox ID="radtbkpirefno" runat="server" Width="50px" CssClass="input_mandatory" />
                        </td>
                        <td>&nbsp &nbsp &nbsp
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Objective Owner" />
                        </td>
                        <td>&nbsp &nbsp &nbsp</td>
                        <td>
                             <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="Radcombodesignationlist" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDUSERNAME" DataValueField="FLDUSERCODE" DropDownWidth="300px"
                            Placeholder="Type to select to whom KPI is to be Assigned" Filter="Contains" FilterFields="FLDDESIGNATIONNAME, FLDUSERNAME" CssClass="input_mandatory" >
                            <NoDataTemplate>
                               
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                            </NoDataTemplate>
                            <ColumnsCollection>
                                <telerik:MultiColumnComboBoxColumn Field="FLDDESIGNATIONNAME" Title="Designation" Width="120px" />
                                <telerik:MultiColumnComboBoxColumn Field="FLDUSERNAME" Title="Name" Width="150px" />
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
                        <td>
                            <telerik:RadLabel runat="server" Text="KPI Min Req" />
                        </td>
                        <td>&nbsp &nbsp &nbsp
                        </td>
                        <td>
                            <eluc:Decimal ID="radtbkpiminimumentry" runat="server" Width="150px" CssClass="input_mandatory" MinValue="0" MaxValue="100" DecimalDigits="2"></eluc:Decimal>
                        </td>
                        <td>&nbsp &nbsp &nbsp
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" Text="KPI Target Value" />
                        </td>
                        <th>&nbsp &nbsp &nbsp
                        </th>
                        <td colspan="5">
                            <eluc:Decimal ID="radtbkpitargetvalueentry" runat="server" Width="150px" CssClass="input_mandatory" MinValue="0" MaxValue="100" DecimalDigits="2"></eluc:Decimal>
                        </td>

                    </tr>


                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
