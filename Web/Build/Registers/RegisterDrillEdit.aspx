<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterDrillEdit.aspx.cs" Inherits="Registers_RegisterDrillEdit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Drill</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        
        <div  style="margin-left:0px" >
           
             <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <eluc:TabStrip ID="Tabstripdrilladdmenu" runat="server" OnTabStripCommand="drilladdmenu_TabStripCommand"
                TabStrip="true" />
            <br />
            <table style="margin-left:20px">
                
               
                <tr>

                    <td>

                        <telerik:RadLabel runat="server" Text="Name" />

                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadTextBox ID="Raddrillnameentry" runat="server"  Width="240px" CssClass="input_mandatory">
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
                    <td>: &nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <div class="input_mandatory">
                          <telerik:RadRadioButtonList runat="server" ID="RadRadioButtonfrequencytype"  Direction="Horizontal" AutoPostBack="false">
                                
                                <Items>
                            <telerik:ButtonListItem   runat="server"  Text=" Weeks"  Value="Weeks"/>
                          
                            <telerik:ButtonListItem   runat="server"  Text="Months" Value="Months"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                        </div>
                    </td>
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Frequency" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
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
                        <telerik:RadLabel runat="server" Text="Applies To" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadCheckBoxList runat="server" ID="radcbappliesto" AutoPostBack="false" Columns="5" CssClass="input_mandatory">
                            <DataBindings DataTextField="VESSELTYPE" DataValueField="FLDTYPE" />
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    
                   
                    <td>
                        <telerik:RadLabel runat="server" Text="Type" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td>
                        <div class="input_mandatory">
                            
                             <telerik:RadRadioButtonList runat="server" ID="RadRadioButtontype"  Direction="Horizontal" AutoPostBack="false">
                                
                                <Items>
                            <telerik:ButtonListItem   runat="server"  Text=" Mandatory"  Value="Mandatory"/>
                          
                            <telerik:ButtonListItem   runat="server"  Text="Company Specified" Value="Company Specified"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                        </div>
                    </td>
                     <td>&nbsp &nbsp &nbsp
                    </td>
                    <td >
                        <telerik:RadLabel runat="server" Text="Fixed/Variable" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td>
                        <div class="input_mandatory">
                          <telerik:RadRadioButtonList runat="server" ID="RadRadioButtonfixedorvariable"  Direction="Horizontal" AutoPostBack="false">
                                
                                <Items>
                            <telerik:ButtonListItem   runat="server"  Text=" Fixed"  Value="Fixed"/>
                          
                            <telerik:ButtonListItem   runat="server"  Text="Variable" Value="Variable"/>
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
                        <telerik:RadLabel runat="server" Text="Affected by Crew Change?" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td>
                        <div class="input_mandatory">
                           <telerik:RadRadioButtonList runat="server" ID="RadRadioaffectbycrewynlist"  Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="crewchange">
                                
                                <Items>
                            <telerik:ButtonListItem   runat="server"  Text=" Yes"  Value="Yes"/>
                          
                            <telerik:ButtonListItem   runat="server"  Text="  No" Value="No"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                          
                        </div>
                    </td>
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td >
                        <telerik:RadLabel runat="server" ID="crewchangelabel" Text="Crew Change Percentage"  Visible="false"/>
                    </td>
                    <th > <telerik:RadLabel runat="server"  ID="crewchangecolon" Text=":  &nbsp &nbsp &nbsp "  Visible="false"/>
                    </th>
                    <td id="crewpercententry">
                        <eluc:MaskNumber ID="tbcrewpercententry" runat="server" Width="35px" MaskText="##" Visible="false"
                            MaxLength="2" CssClass="input_mandatory"  />
                         <eluc:MaskNumber ID="dummycrewpercentage" runat="server" Width="35px" MaskText="##" Visible="false"
                            MaxLength="2" CssClass="input_mandatory"  />
                    </td>
                </tr>


                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Photo Mandatory?" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td>
                        <div class="input_mandatory">
                           <telerik:RadRadioButtonList runat="server" ID="RadRadiophotoynlist"  Direction="Horizontal" AutoPostBack="false">
                                
                                <Items>
                            <telerik:ButtonListItem   runat="server"  Text=" Yes"  Value="Yes"/>
                          
                            <telerik:ButtonListItem   runat="server"  Text="  No" Value="No"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                        </div>
                    </td>
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Show in Dashboard?" />
                    </td>
                    <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td>
                        <div class="input_mandatory">
                            <telerik:RadRadioButtonList runat="server" ID="RadRadioButtondashboardynlist"  Direction="Horizontal" AutoPostBack="false">
                                
                                <Items>
                            <telerik:ButtonListItem   runat="server"  Text=" Yes"  Value="Yes"/>
                          
                            <telerik:ButtonListItem   runat="server"  Text="  No" Value="No"/>
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
                        <telerik:RadLabel runat="server" Text="Excluded Vessels" />
                    </td>
                     <th>: &nbsp &nbsp &nbsp
                    </th>
                    <td>
                        <telerik:RadComboBox ID="radexcludedvessels" runat="server" DataValueField="FLDID" DataTextField="FLDNAME" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  Width="100%" AllowCustomText="true" EmptyMessage="Type to select excluded vessels"/>
                    </td>
                    <td>

                    </td>
                    <td></td>
                     <td></td>
                     <td></td>
                </tr>

                 
            </table>
        </div>
    </form>
</body>
</html>
