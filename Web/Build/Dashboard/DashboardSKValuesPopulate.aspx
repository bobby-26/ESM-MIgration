<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKValuesPopulate.aspx.cs" Inherits="Dashboard_DashboardSKValuesPopulate" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <div>
        <telerik:RadAjaxPanel runat="server" ID="radajax" >
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadNotification ID="ucNotification" RenderMode="Lightweight" runat="server" AutoCloseDelay="1500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false"></telerik:RadNotification>
    <table>
        <tr>
            <td>
                Year
            </td>
            <td>
                <eluc:Year runat="server" ID="year" YearStartFrom="2010" NoofYearFromCurrent="2" /> 
            </td>
            <td>
                Quarter
            </td>
            <td>
                <telerik:RadComboBox ID="quarter" runat="server" >
                    <Items>
                        <telerik:RadComboBoxItem Text="Q1" Value="Q1" />
                         <telerik:RadComboBoxItem Text="Q2" Value="Q2" />
                         <telerik:RadComboBoxItem Text="Q3" Value="Q3" />
                         <telerik:RadComboBoxItem Text="Q4" Value="Q4" />
                    </Items>
                </telerik:RadComboBox>
            </td>
            <td>
                <telerik:RadButton runat="server" ID="changetimeline" OnClick="changetimeline_Click" Text="Change" />
            </td>
        </tr>
        <tr>
            <td>
                PI
            </td>
            <td colspan="2">

            </td>
            <td>
               <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="Radcombopilist" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDPINAME" DataValueField="FLDPICODE" DropDownWidth="300px"
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
            <td>
                <telerik:RadButton ID="picaluclate" runat="server" OnClick="picaluclate_Click"  Text="Calculate"/>
            </td>
        </tr>
        <tr>
            <td>
                KPI
            </td>
            <td colspan="2">

            </td>
            <td>
                <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="RadComboKpilist" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDKPINAME" DataValueField="FLDKPICODE" DropDownWidth="300px"
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
            <td>
                 <telerik:RadButton ID="kpicalculate" runat="server" OnClick="kpicalculate_Click" Text="Calculate"/>
            </td>
        </tr>
        <tr>
            <td>
                SPI
            </td>
            <td colspan="2">

            </td>
            <td>
                <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="Radcombospilist" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDSPITITLE" DataValueField="FLDSPIID" DropDownWidth="300px"
                            Placeholder="Type to Select the SPI" Filter="Contains" FilterFields="FLDSPIID, FLDSPITITLE" CssClass="input_mandatory"
                            >
                            <NoDataTemplate>
                               
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                            </NoDataTemplate>
                            <ColumnsCollection>
                                <telerik:MultiColumnComboBoxColumn Field="FLDSPIID" Title="Code" Width="70px" />
                                <telerik:MultiColumnComboBoxColumn Field="FLDSPITITLE" Title="Name" Width="200px" />
                            </ColumnsCollection>
                        </telerik:RadMultiColumnComboBox>
            </td>
            <td>
                <telerik:RadButton ID="spicalculate" runat="server" OnClick="spicalculate_Click" Text="Calculate"/>
            </td>
        </tr>
    </table>
            </telerik:RadAjaxPanel>
    </div>
    </form>
</body>
</html>
