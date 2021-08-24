<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSCIssue.aspx.cs" Inherits="Dashboard_DashboardBSCIssue" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlMonth.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
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
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="TabstripMenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <div style="margin-left: 0px">
            <br />
            <table style="margin-left: 20px; width:90%">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Month" />
                    </td>
                    <td>&nbsp&nbsp
                    </td>
                    <td>
                        <eluc:Month runat="server" ID="radcbmonth" width="150px" CssClass="input_mandatory" />
                    </td>
                    <td>&nbsp&nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Year" />
                    </td>
                    <td>&nbsp
                    </td>
                    <td>
                        <eluc:Year runat="server" YearStartFrom="2018" NoofYearFromCurrent="0" ID="radcbyear" width="150px" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="PI" />
                    </td>
                    <td>&nbsp&nbsp
                    </td>
                    <td colspan="5">
                        <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="Radcombopilist" runat="server" width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDPINAME" DataValueField="FLDPIID" DropDownwidth="300px"
                            Placeholder="Type to Select tde PI" Filter="Contains" FilterFields="FLDPICODE, FLDPINAME" CssClass="input_mandatory">
                            <NoDataTemplate>
                               
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                            </NoDataTemplate>
                            <ColumnsCollection>
                                <telerik:MultiColumnComboBoxColumn Field="FLDPICODE" Title="Code" width="70px" />
                                <telerik:MultiColumnComboBoxColumn Field="FLDPINAME" Title="Name" width="200px" />
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
                        <telerik:RadLabel runat="server" Text="Issue" />
                    </td>
                    <td>&nbsp&nbsp
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="radtbissueentry" CssClass="input_mandatory" TextMode="MultiLine" Rows="3" width="290px" />
                    </td>
                    <td>&nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Implication" />
                    </td>
                    <td>&nbsp&nbsp
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="radtbimplicationentry" CssClass="input_mandatory" TextMode="MultiLine" Rows="3" width="150px" />
                    </td>

                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Action" />
                    </td>
                    <td>&nbsp&nbsp
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox runat="server" ID="radtbactionentry" CssClass="input_mandatory" TextMode="MultiLine" Rows="3"  width="290px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Assigned To" />
                    </td>

                    <td>&nbsp&nbsp
                    </td>
                    <td>
                         <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="Radcombodesignationlist" runat="server" width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDUSERNAME" DataValueField="FLDUSERCODE" DropDownwidth="300px"
                            Placeholder="Type to select to whom Issue is to be Assigned" Filter="Contains" FilterFields="FLDDESIGNATIONNAME, FLDUSERNAME" CssClass="input_mandatory" >
                            <NoDataTemplate>
                               
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                            </NoDataTemplate>
                            <ColumnsCollection>
                                <telerik:MultiColumnComboBoxColumn Field="FLDDESIGNATIONNAME" Title="Designation" width="120px" />
                                <telerik:MultiColumnComboBoxColumn Field="FLDUSERNAME" Title="Name" width="150px" />
                            </ColumnsCollection>
                         </telerik:RadMultiColumnComboBox>
                    </td>
                    <td>
                        &nbsp &nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Target date" />
                    </td>
                   <td>
                       &nbsp&nbsp
                   </td>
                    <td>
                          <eluc:Date ID="radtargetdate" CssClass="input_mandatory" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
