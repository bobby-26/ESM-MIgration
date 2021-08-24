<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRVoyageData.aspx.cs"
    Inherits="CrewOffshoreDMRVoyageData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewOffshore" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPStatus" Src="~/UserControls/UserControlSEPStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOffshoreVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Charterer" Src="~/UserControls/UserControlAddressCharterer.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DMRCharterer" Src="~/UserControls/UserControlDMRVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Voyage Data</title>
    <telerik:RadCodeBlock id="ds" runat="server">
      
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/Phoenix.Telerik.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
     
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        
    </telerik:RadCodeBlock>
   
</head>
<body>
    <form id="frmVoyageData" runat="server">
      
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"/>
       <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
                    <AjaxSettings>
                       
                        <telerik:AjaxSetting AjaxControlID="RadGrid1">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
       
               
                    <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
                   <%-- <div class="subHeader" style="position: relative">
                        <div id="divHeading" class="divFloatLeft">
                            <eluc:Title runat="server" ID="ucTitle" Text="Charter Details" ShowMenu="true"></eluc:Title>
                        </div>
                    </div>--%>
                    <div class="Header">
                        <div style="font-weight: 600; font-size: 12px;" runat="server">
                            <eluc:TabStrip ID="MenuVoyageTap" TabStrip="true" runat="server" OnTabStripCommand="VoyageTap_TabStripCommand"></eluc:TabStrip>
                            <telerik:RadButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                        </div>
                    </div>
                    <div class="subHeader">
                        <div style="font-weight: 600; font-size: 12px;" runat="server">
                            <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="VoyageNewSaveTap_TabStripCommand"></eluc:TabStrip>
                        </div>
                    </div>
                 <br clear="all" />
                    <div>

                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td >
                                <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                                    AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                            </td>
                            <td ></td>
                            <td ></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterer"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Charterer ID="ucCharterer" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    OnTextChangedEvent="ucCharterer_Changed" AutoPostBack="True" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblChartererID" runat="server" Text="Charter ID"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtChartererID" Width="300px" CssClass="readonlytextbox" ReadOnly="true"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCommenced" runat="server" Text="Commenced"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucCommencedDate" runat="server" DatePicker="true" TimeProperty="true"  />
                       
                                                       
                            </td>
                            <td>
                                      
                                <telerik:RadLabel ID="lblCompleted" runat="server" Text="Completed"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucCompletedDate" DatePicker="true" TimeProperty="true"  runat="server"  />
                   
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCommencementAtSea" runat="server" Text="Commencement At Sea">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkCommencementAtSea" runat="server" AutoPostBack="true" OnCheckedChanged="chkCommencementAtSea_OnCheckedChanged" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCompletedAtSea" runat="server" Text="Completed At Sea">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                
                                <telerik:RadCheckBox ID="chkCompletedAtSea" runat="server" AutoPostBack="true" OnCheckedChanged="chkCompletedAtSea_OnCheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCommencedPort" runat="server" Text="Commenced Port"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCommencedLat" runat="server" Text="Commenced Latitude" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:MultiPort ID="ucCommensedPort" runat="server"  Width="30%" ReadOnly="true" />
                                <eluc:Latitude ID="ucCommencedLat" runat="server" CssClass="input_mandatory" Visible="false" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCompletedPort" runat="server" Text="Completed Port"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompletedLat" runat="server" Text="Completed Latitude" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:MultiPort ID="ucCompletedPort" runat="server" Width="300px" />
                                <eluc:Latitude ID="ucCompletedLat" runat="server" CssClass="input_mandatory" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCommencedLong" runat="server" Text="Commenced Longitude" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Longitude ID="ucCommencedLong" runat="server" CssClass="input_mandatory" Visible="false" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCompletedLong" runat="server" Text="Completed Longitude" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Longitude ID="ucCompletedLong" runat="server" CssClass="input_mandatory" Visible="false" />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <telerik:RadLabel ID="lblDisplayVol" runat="server" Text="Display Vol in"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlVolume" runat="server" Width="30%" CssClass="input_mandatory">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="Ltrs" />
                                         <telerik:RadComboBoxItem Value="2" Text="BBL" />
                                         <telerik:RadComboBoxItem Value="3" Text="M3" />

                                    </Items>
                                </telerik:RadComboBox>
                              
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblNextCharter" runat="server" Text="Next Charter"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:DMRCharterer ID="ucDMRCharter" runat="server"  AutoPostBack="true"
                                    AppendDataBoundItems="true" />
                                <%--OnTextChangedEvent="ucDMRCharterer_Changed"--%>
                                <eluc:Charterer ID="ucNextCharterer" Visible="false" runat="server" 
                                    AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr runat="server" visible="false">
                            <td>
                                <telerik:RadLabel ID="lblProposedDate" runat="server" Text="Proposed Completed Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucProposedDate" runat="server"  />
                               
                            <telerik:RadTextBox  RenderMode="Lightweight" ID="txtProposedDate" runat="server"  Width="50px" />
                                <%--<ajaxToolkit:MaskedEditExtender ID="MaskedEditProposedDateExtender" runat="server"
                                    AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                  MaskType="Time" TargetControlID="txtProposedDate" UserTimeFormat="TwentyFourHour" /> --%> 
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblProposedPort" runat="server" Text="Proposed Completed Port"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:MultiPort ID="ucProposedCompletedPort" runat="server"  Width="30%" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblEstimatedCommenceDate" runat="server" Text="Estimated Start of Charter"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucEstimatedCommenceDate" runat="server"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblEstimatedEndDate" runat="server" Text=" Estimated end of Charter"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucEstimatedEndDate" DatePicker="true"  runat="server" />
                           
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtEstimatedDuration" Width="20px" CssClass="readonlytextbox" ReadOnly="true"
                                    runat="server" />
                                Days
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblWorkScope" runat="server" Text="Scope of Work"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtWorkScope" runat="server" Width="300px" >
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblOptions" runat="server" Text="Options"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtOptions" runat="server" Width="300px" >
                                </telerik:RadTextBox>
                            </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblDPCahrtererYN" runat="server" Text="DP Charter"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlDPChartererYN" runat="server" Width="30%" CssClass="input_mandatory"  EmptyMessage="Select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="0" Text="No" />
                                         <telerik:RadComboBoxItem Value="1" Text="Yes" />
                                     

                                    </Items>
                                </telerik:RadComboBox>
                                
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblkickoff" runat="server" Text="Kick Off Meeting - Location"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtkickoff" runat="server" Width="300px" ></telerik:RadTextBox>
                                    
                                </td>
                            </tr>
                     
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblpremobinspection" runat="server" Text="Pre-Mob Inspection"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlpremobinspectionYN" runat="server" Width="30%" CssClass="input_mandatory" EmptyMessage="Select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="0" Text="No" />
                                         <telerik:RadComboBoxItem Value="1" Text="Yes" />
                                    </Items>
                                </telerik:RadComboBox>
                                
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblpremoblocation" runat="server" Text="Pre-Mob Inspection Location"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpremoblocation" runat="server" Width="300px" >
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblworkinglocation" runat="server" Text="Working Location and Field Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtworkinglocation" runat="server" Width="300px" >
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblnameofrig" runat="server" Text="Name of Rig / Platform / Others"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtnameofrig" runat="server" Width="300px" >
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblContractHolder" runat="server" Text="Name Of Contract Holder"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtContractHolder" runat="server" Width="300px" >
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblTradingArea" runat="server" Text="Trading Area"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlTradingArea" runat="server" Width="300px"   EmptyMessage="Select the Trading" Filter="Contains" MarkFirstMatch="true">
                                    <DefaultItem Text="Select the Trading" Value="-1"/>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <telerik:RadLabel ID="lblPlannedCommencedPort" runat="server" Text="Planned On-Hire Port"></telerik:RadLabel>
                            </td>
                            <td >
                                <eluc:MultiPort ID="ucPlannedCommensedPort" runat="server" Width="30%"  />
                            </td>
                            <td style="vertical-align: top;">
                                <telerik:RadLabel ID="lblCharterersVoyage" runat="server" Text="Remarks"></telerik:RadLabel>
                            </td>
                            <td style="vertical-align: top;">
                                <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtChartererInstruction"  TextMode="MultiLine"
                                    Width="300px" Height="70px" Rows="2"></telerik:RadTextBox>
                            </td>
                            
                        </tr>
                        <tr runat="server" visible="false">
                            <td colspan="3">
                                <telerik:RadLabel ID="lblEffectiveDate" runat="server" Text="Effective Date" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="txtEffectiveDate"  runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">Note :
                            <telerik:RadLabel ID="lblSOFNote" runat="server" ForeColor="Red" Text="Master’s SOF is to be generated from this module"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <%--<div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvTradingArea" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvTradingArea_RowCommand" OnRowDataBound="gvTradingArea_ItemDataBound"
                        OnRowCancelingEdit="gvTradingArea_RowCancelingEdit" OnRowDeleting="gvTradingArea_RowDeleting"
                        AllowSorting="true" OnRowEditing="gvTradingArea_RowEditing" OnRowUpdating="gvTradingArea_RowUpdating"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDDMRCHARTERTRADINGAREAID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField FooterText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblSerialHeader" runat="server" Text="S.No."></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblTradingAreaHeader" Text="Trading Area" runat="server">                                        
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTradingArea" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRADINGAREA")%>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblTradingCode" runat="server" Width="10%" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRADINGCODE")%>'></asp:Label>
                                    <asp:DropDownList ID="ddlTradingAreaEdit" runat="server" Width="200px" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTradingArea" runat="server" Width="200px" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblEffectiveDateHeader" Text=" Effective Date " runat="server">                                        
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEffectiveDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEFFECTIVEDATE", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:UserControlDate ID="txtEffectiveDateEdit" CssClass="input_mandatory" runat="server"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDEFFECTIVEDATE", "{0:dd/MM/yyyy}")%>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:UserControlDate ID="txtEffectiveDate" CssClass="input_mandatory" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblTillDateHeader" Text="Till Date" runat="server">                                        
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTillDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTILLDATE", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblTillDateEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTILLDATE", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server" Text="Action"></asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>--%>
                <eluc:Status runat="server" ID="ucStatus" />
             <%--   <div id="divPage" runat="server" visible="false" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">&nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" >
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" 
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                        <tr> <td>
                                <eluc:Date ID="txtDate" runat="server"  DatePicker="true" TimeProperty="true" />
                            </td>
                            <td></td>
                            <td colspan="2"></td>
                        </tr>
                    </table>
                </div>--%>
          
    </form>
</body>
</html>
