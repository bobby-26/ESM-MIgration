<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSCLIEdit.aspx.cs" Inherits="Dashboard_DashboardBSCLIEdit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Leading Indicator (LI)</title>
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
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="Tabstripspiaddmenu" runat="server" OnTabStripCommand="Tabstripspiaddmenu_TabStripCommand"
                TabStrip="true" />
            <br />
            <table style="margin-left: 20px">


                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Code" />
                    </td>
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox ID="Radliidentry" runat="server" Width="150px" CssClass="input_mandatory" MaxLength="5">
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
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox ID="radtblinameentry" runat="server" Width="350px" CssClass="input_mandatory" TextMode="MultiLine" Rows="2">
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
                        <telerik:RadLabel runat="server" Text="Frequency Type"></telerik:RadLabel>
                    </td>
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <div class="input_mandatory">
                            <telerik:RadRadioButtonList runat="server" ID="RadRadioButtonfrequencytype" Direction="Horizontal" AutoPostBack="false">

                                <Items>
                                    <telerik:ButtonListItem runat="server" Text=" Weeks" Value="Weeks" />

                                    <telerik:ButtonListItem runat="server" Text="Months" Value="Months" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </div>
                    </td>
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Frequency" />
                    </td>
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <eluc:MaskNumber ID="tbfrequencyentry" runat="server" Width="35px" MaskText="##"
                            MaxLength="2" CssClass="input_mandatory" />
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
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <telerik:RadComboBox ID="RadCbliunit" runat="server" CssClass="input_mandatory" Width="150px"
                            EmptyMessage="Type to select LI Unit" AllowCustomText="true"
                            AutoPostBack="false" />
                    </td>
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Scope" />
                    </td>
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td>
                      <div class="input_mandatory" >
                            <telerik:RadRadioButtonList runat="server" ID="RadRBliscope" Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RadRBliscope_SelectedIndexChanged">
                                <Items>
                                    <telerik:ButtonListItem runat="server" Text="Ship" Value="1"  />
                                    <telerik:ButtonListItem runat="server" Text="Office" Value="2" Selected="true" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </div>
                    </td>
                </tr>
                 <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Department" ID="radlbldept" Visible ="false" />
                        <telerik:RadLabel runat="server" Text="Group" ID="radlblrank"  Visible="false"/>
                    </td>
                    <td>
                        &nbsp&nbsp&nbsp
                    </td>
                    <td>
                    <telerik:RadComboBox runat="server" ID="radcbdept" CssClass="input_mandatory" Width="150px"
                            EmptyMessage="Type to select Department" AllowCustomText="true" OnTextChanged="radcbdept_TextChanged"
                            AutoPostBack="true"  Visible="false"/>

                       
                         <eluc:Hard ID="radcbrank" runat="server" AppendDataBoundItems="true" Width="150px"
                                    HardTypeCode="51"    AutoPostBack="true" Visible="false" OnTextChangedEvent="radcbrank_TextChangedEvent" CssClass="input_mandatory"/>

                       
                       
                    </td>
                    <td>
                        &nbsp&nbsp&nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Action By" ID="Radlblappliesto" />
                    </td>
                    <td>
                        &nbsp&nbsp&nbsp
                    </td>
                    <td>
                         <telerik:RadComboBox runat="server" ID="radcbappliesto" CssClass="input_mandatory" Width="250px"
                            EmptyMessage="Type to select to whom LI is to be Assigned" AllowCustomText="true"
                            AutoPostBack="false" Visible="false"/>

                        <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="Radcombodesignationlist" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDDESIGNATIONNAME" DataValueField="FLDDESIGNATIONID" DropDownWidth="300px"
                            Placeholder="Type to select to whom LI is to be Assigned" Filter="Contains" FilterFields="FLDDESIGNATIONNAME, FLDUSERNAME" CssClass="input_mandatory" Visible="false" >
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
                        <telerik:RadLabel runat="server" Text="Description" />
                    </td>
                    <th>&nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadTextBox ID="RadtblIdescriptionentry" runat="server" TextMode="MultiLine" Width="180px"
                            Rows="4" CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
                </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
