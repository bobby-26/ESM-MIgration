<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPSCMOUShipRiskProfile.aspx.cs" Inherits="Inspection_InspectionPSCMOUShipRiskProfile" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
    <title>Ship Risk Calculator</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            table.Hazard {
                border-collapse: collapse;
            }

                table.Hazard td, table.Hazard th {
                    border: 1px solid;
                    border-color: rgb(30, 57, 91);
                    padding: 5px;
                }
        </style>
        <style type="text/css">
            .checkboxstyle tbody tr td {
                width: 550px;
                vertical-align:top;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigure" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        
        <eluc:TabStrip ID="MenuShipRiskProfile" runat="server" OnTabStripCommand="MenuShipRiskProfile_TabStripCommand" Title="Ship Risk Calculator" />
<%--            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />--%>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <br />
            <table cellpadding="3" cellspacing="3" width="100%" id="shipprofile">                       
                <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblport" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                            <eluc:MultiPort ID="ucFromPort" runat="server" CssClass="readonlytextbox" Width="200px"
                                />
                    </td>
                    <td width="25%" runat="server" visible="false">
                        <telerik:RadLabel ID="RadLabel4" runat="server" Font-Bold="true" Text="Weighting points to high risk profile"></telerik:RadLabel>
                    </td>
                    <td width="25%" runat="server" visible="false">
                        <telerik:RadLabel ID="RadLabel5" runat="server" Font-Bold="true" Text="Eligibility to low risk profile"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <eluc:VesselByOwner runat="server" ID="ucVessel" OnTextChangedEvent="ucVessel_TextChangedEvent" AutoPostBack="true" AppendDataBoundItems="true" Width="200px" VesselsOnly="true" />
                    </td>
                    <td width="25%" runat="server" visible="false">
                        <telerik:RadLabel ID="RadLabel2" runat="server" Font-Bold="true" Text="Weighting points to high risk profile"></telerik:RadLabel>
                    </td>
                    <td width="25%" runat="server" visible="false">
                        <telerik:RadLabel ID="RadLabel3" runat="server" Font-Bold="true" Text="Eligibility to low risk profile"></telerik:RadLabel>
                    </td>                    
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadLabel ID="RadLabel1" runat="server" Font-Size="Larger" Font-Bold="true" Text="Generic Parameters"></telerik:RadLabel>
                        <hr />
                    </td>                        
                </tr>
                <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblpscmou" runat="server" Text="PSC MOU"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadComboBox ID="ddlCompany" runat="server" Width="200px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" CssClass="input" >                                
                        </telerik:RadComboBox>                        
                    </td>
                    <td width="25%" runat="server">
                        <telerik:RadLabel ID="lblhrship" runat="server" Font-Bold="true" Text="Weighting points to high risk profile"></telerik:RadLabel>
                    </td>
                    <td width="25%" runat="server">
                        <telerik:RadLabel ID="lblrship" runat="server" Font-Bold="true" Text="Eligibility to low risk profile"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblshiptype" runat="server" Text="Ship Type"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="200px" HardList='<%# PhoenixRegistersHard.ListHard(1, 81) %>'
                            HardTypeCode="81"/>                    
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrshiptype" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lrshiptype" runat="server" Text="All types"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblshipage" runat="server" Text="Ship Age"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadComboBox runat="server" ID="ddlshipage" RenderMode="Lightweight" Width="200px" AppendDataBoundItems="true" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" CssClass="input">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="< 12 Years" Value="0"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text=">= 12 Years" Value="1"></telerik:RadComboBoxItem>
                        </Items>    
                        </telerik:RadComboBox>                  
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrshipage" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllrshipage" runat="server" Text="All ages"></telerik:RadLabel>
                    </td>
                </tr>

               <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblflag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadTextBox ID="ucFlag" runat="server" Width="200px" Text=""></telerik:RadTextBox>
                         <%--<eluc:Flag ID="ucFlag" runat="server" Width="200px" AppendDataBoundItems="true" />   --%>

                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrflag" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllrflag" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>

               <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblflagperformance" runat="server" Text="Flag Performance"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                         <telerik:RadComboBox ID="ddlflagperf" runat="server" Width="200px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" ></telerik:RadComboBox>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrflagperf" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllrflagperf" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                </tr>

               <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblIsIMOAudited" runat="server" Text="Flag Is IMO Audited"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                            <asp:RadioButtonList ID="rblIMOAudit" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="200px" AutoPostBack="true">
                                <asp:ListItem Text="Yes" Value="1" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>     
                            </asp:RadioButtonList>
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="hrimoaudit" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lrimoaudit" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                </tr>
 
              <tr runat="server" visible="false" >
                    <td width="25%">
                        <telerik:RadLabel ID="lblcertissue" runat="server" Text="All Certificates Issued by Flag"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                            <asp:RadioButtonList ID="rblcertissue" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="200px" AutoPostBack="true">
                                <asp:ListItem Text="Yes" Value="1" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>     
                            </asp:RadioButtonList>
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrcertissue" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllrcertissue" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                </tr>

              <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblclasssociety" runat="server" Text="Class Society"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                            <eluc:MultiAddress runat="server" ID="ucClassName" AddressType='<%# ((int)PhoenixAddressType.CLASSIFICATIONSOCIETY).ToString() %>'  />
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrclasssociety" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllrclasssociety" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>

              <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblclassperf" runat="server" Text="Class Society Performance"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadComboBox ID="ddlclassperf" runat="server" Width="200px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" ></telerik:RadComboBox>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrclassperf" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllrclassperf" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                </tr>

 
              <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lbliseurecog" runat="server" Text="Is EU Recognized"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                            <asp:RadioButtonList ID="rblEURecog" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="200px" AutoPostBack="true">
                                <asp:ListItem Text="Yes" Value="1" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>     
                            </asp:RadioButtonList>
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrEURecog" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllrEURecog" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                </tr>

               <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblcompperf" runat="server" Text="Company Performance"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadComboBox ID="ddlcompperf" runat="server" Width="200px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrcompperf" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllrcompperf" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadLabel ID="lblsubhead" runat="server" Font-Size="Larger" Font-Bold="true" Text="Historic Parameters from the last 36 Months"></telerik:RadLabel>
                        <hr />
                    </td>                        
                </tr>
               <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhisinsp" runat="server" Text="Atleast One inspection"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                            <asp:RadioButtonList ID="rbloneinsp" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="200px" AutoPostBack="true">
                                <asp:ListItem Text="Yes" Value="1" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>     
                            </asp:RadioButtonList>
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhroneinsp" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllroneinsp" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                </tr>
               <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="All inspections with 5 or less deficiencies"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                            <asp:RadioButtonList ID="rbllessdef" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="200px" AutoPostBack="true">
                                <asp:ListItem Text="Yes" Value="1" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>     
                            </asp:RadioButtonList>
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrlessdef" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllrlessdef" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                </tr>
               <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblnodetention" runat="server" Text="No of Dentention"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                            <asp:RadioButtonList ID="rblnodetention" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="200px" AutoPostBack="true">
                                <asp:ListItem Text="None" Value="1" Selected="False"></asp:ListItem>            
                                <asp:ListItem Text="One" Value="2" Selected="False"></asp:ListItem>                         
                                <asp:ListItem Text="Two or More" Value="0" Selected="False"></asp:ListItem>     
                            </asp:RadioButtonList>
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhrnodetention" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllrnodetention" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadLabel ID="RadLabel7" runat="server" Font-Size="Larger" Font-Bold="true" Text="Result"></telerik:RadLabel>
                        <hr />
                    </td>                        
                </tr>
              <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lbltotalweightage" runat="server" Text="Total weighting point to high risk profile"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbltotalscore" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">                        
                    </td>
                </tr>
              <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhighriskprofile" runat="server" Text="Eligibility to high risk profile (>=5)"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblhighriskscore" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">                        
                    </td>
                </tr>
              <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllowriskprofile" runat="server" Text="Eligibility to low risk profile"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lbllowriskriskscore" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">                        
                    </td>
                </tr>
              <tr>
                    <td width="25%">
                        <telerik:RadLabel ID="lblshiprisk" runat="server" Text="Ship Risk Profile"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        </td>
                    <td width="25%">
                        <telerik:RadLabel ID="lblshipriskresult" runat="server" Text="-"></telerik:RadLabel>
                    </td>
                    <td width="25%">                        
                    </td>
                </tr>                
                <tr>
                    <td colspan="5">
                        <hr />
                    </td>                        
                </tr>

            </table>

    </form>
</body>
</html>
