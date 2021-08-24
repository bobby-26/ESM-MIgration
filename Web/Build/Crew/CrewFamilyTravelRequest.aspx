<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewFamilyTravelRequest.aspx.cs" Inherits="CrewFamilyTravelRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="~/UserControls/UserControlAirport.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Change Travel</title>
    
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
    
</head>
<body>
    <form id="frmCrewChangeTravel" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewCompanyExperienceListEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <eluc:Title runat="server" ID="Title1" Text="Travel Request" ShowMenu="false"></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuGenerateTravel" runat="server" OnTabStripCommand="GenerateTravel_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                
                <table cellspacing="1" cellpadding="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblDateofCrewChange" runat="server" Text="Date of Crew Change"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDateOfCrewChange" runat="server" ReadOnly="true" CssClass="readonlytextbox" />
                        </td>
                        <td>
                            <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Port ID="ddlPort" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
                <hr />
                
                <hr />
                <br />
                <b><asp:Literal ID="lblTravelPlan" runat="server" Text="Travel Plan"></asp:Literal></b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuBreakUpAssign" runat="server" OnTabStripCommand="BreakUpAssign_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvCCT" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowDeleting="gvCCT_RowDeleting" OnRowDataBound="gvCCT_RowDataBound" OnRowEditing="gvCCT_RowEditing"
                    OnRowCancelingEdit="gvCCT_RowCancelingEdit" OnRowUpdating="gvCCT_RowUpdating"
                    OnRowCommand="gvCCT_RowCommand" OnSelectedIndexChanging="gvCCT_SelectedIndexChanging"
                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                    
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    
                    <Columns>
                        <asp:TemplateField HeaderText="S.No.">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Copy To">
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkAssignedTo" />
                                <asp:Label ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblOnSignerYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblCrewChangePort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWCHANGEPORT") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblDateOfCrewChange" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFCREWCHANGE") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblDTKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblTravelReqNo" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="On/Off-Signer">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYESNO") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="D.O.B.">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="PP No.">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDPASSPORTNUMBER")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="VISA Details">
                            <ItemTemplate>
                                <asp:Label ID="lblOtherVisa" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOTHERVISADETAILS").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDOTHERVISADETAILS").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDOTHERVISADETAILS").ToString() %>'></asp:Label>
                                <eluc:ToolTip ID="ucOtherVisaTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERVISADETAILS") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                      <asp:TemplateField HeaderText="Origin">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListAirportEdit">
                                    <asp:TextBox ID="txtAirportNameEdit" runat="server" Width="80%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'
                                        Enabled="False" CssClass="input_mandatory"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowAirportEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>"
                                         OnClientClick="return showPickList('spnPickListAirportEdit', 'codehelp1', '', 'Common/CommonPickListAirport.aspx', true); " />
                                    <asp:TextBox ID="txtoriginIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'></asp:TextBox>
                                </span>
                            </EditItemTemplate>                           
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Destination">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListAirportdestinationedit">
                                    <asp:TextBox ID="txtDestinsationNameedit" runat="server" Width="80%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'
                                        Enabled="False" CssClass="input_mandatory"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowDestinationedit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>" 
                                        OnClientClick="return showPickList('spnPickListAirportdestinationedit', 'codehelp1', '', 'Common/CommonPickListAirport.aspx', true); " />
                                    <asp:TextBox ID="txtDestinationIdedit" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'></asp:TextBox>
                                </span>
                            </EditItemTemplate>                           
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Departure Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE","{0:dd/MMM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDate" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE")%>'>
                                </eluc:Date>
                            </EditItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Arrival Date">
                            <ItemTemplate>                                
                                 <asp:Label ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE")%>'>
                                </eluc:Date>
                            </EditItemTemplate>
                        </asp:TemplateField>
                           <asp:TemplateField HeaderText="More Info">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>                                  
                               <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>' Visible="false"></asp:Label>
                                    <img ID="imgRemarks" runat="server"  alt="More Info" style="cursor:hand" src="<%$ PhoenixTheme:images/te_view.png %>"  onmousedown="javascript:closeMoreInformation()" />
                                </ItemTemplate>                                
                            </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                            Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Instruction" ImageUrl="<%$ PhoenixTheme:images/add-instruction.png %>"
                                    CommandName="INSTRUCTION" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAddInstruction"
                                    ToolTip="Add Instruction"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div id="divPage" style="position: relative;">
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
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <b><asp:Literal ID="lblBreakJourneyDetails" runat="server" Text="Break Journey Details"></asp:Literal></b>
                <asp:GridView ID="gvCTBreakUp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowDeleting="gvCTBreakUp_RowDeleting" OnRowDataBound="gvCTBreakUp_RowDataBound"
                    OnRowEditing="gvCTBreakUp_RowEditing" OnRowCancelingEdit="gvCTBreakUp_RowCancelingEdit"
                    OnRowUpdating="gvCTBreakUp_RowUpdating" Width="100%" CellPadding="3" ShowHeader="true"
                    EnableViewState="false" OnRowCommand="gvCTBreakUp_RowCommand">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.No.">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTravelReqNo" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></asp:Label>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSERIALNO")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Origin">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblOnSignerYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblBreakUpId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBREAKUPID") %>'
                                    Visible="false"></asp:Label>
                               <%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblEmployeeIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblTravelRequestIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblVesselIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblOnSignerYNEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblBreakUpIdEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBREAKUPID") %>'
                                    Visible="false"></asp:Label>
                                    
                                <span id="spnPickListOriginOldbreakup">
                                    <asp:TextBox ID="txtOriginNameOldBreakup" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'></asp:TextBox>
                                    <asp:ImageButton ID="btnShowOriginoldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>"
                                        OnClientClick="return showPickList('spnPickListOriginOldbreakup', 'codehelp1', '', 'Common/CommonPickListAirport.aspx', true); " />
                                    <asp:TextBox ID="txtOriginIdOldBreakup" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'></asp:TextBox>
                                </span>
                                <br />
                                <br />
                                <span id="spnPickListOriginbreakup">
                                    <asp:TextBox ID="txtOriginNameBreakup" runat="server" Width="80%" Enabled="False" CssClass="input_mandatory"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowOriginbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>" 
                                        OnClientClick="return showPickList('spnPickListOriginbreakup', 'codehelp1', '', 'Common/CommonPickListAirport.aspx', true); " />
                                    <asp:TextBox ID="txtOriginIdBreakup" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                </span>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Destination">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListDestinationOldbreakup">
                                    <asp:TextBox ID="txtDestinationNameOldBreakup" runat="server" Width="80%" Enabled="False" CssClass="input_mandatory"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowDestinationOldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>" 
                                        OnClientClick="return showPickList('spnPickListDestinationOldbreakup', 'codehelp1', '', 'Common/CommonPickListAirport.aspx', true); " />
                                    <asp:TextBox ID="txtDestinationIdOldBreakup" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                </span>
                                <br />
                                <br />
                                <span id="spnPickListDestinationbreakup">
                                    <asp:TextBox ID="txtDestinationNameBreakup" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'></asp:TextBox>
                                    <asp:ImageButton ID="btnShowDestinationbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>" 
                                        OnClientClick="return showPickList('spnPickListDestinationbreakup', 'codehelp1', '', 'Common/CommonPickListAirport.aspx', true); " />
                                    <asp:TextBox ID="txtDestinationIdBreakup" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'></asp:TextBox>
                                </span>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Departure Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDateOld" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE") %>'>
                                </eluc:Date>
                                <br />
                                <br />
                                <eluc:Date runat="server" ID="txtDepartureDate" CssClass="input_mandatory"></eluc:Date>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Arrival Date">
                            <ItemTemplate>
                                <asp:Label ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy}")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtArrivalDateOld" CssClass="input_mandatory"></eluc:Date>
                                <br />
                                <br />
                                <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE") %>'>
                                </eluc:Date>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Purpose">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPurpose" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSENAME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick runat="server" ID="ucPurposeOld" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    QuickTypeCode="59" QuickList='<%# PhoenixRegistersQuick.ListQuick(1, 59) %>' />
                                <br />
                                <br />
                                <eluc:Quick runat="server" ID="ucPurpose" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    QuickTypeCode="59" QuickList='<%# PhoenixRegistersQuick.ListQuick(1, 59) %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                            Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDITROW" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/travel-breakup.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdTravelBreakUp"
                                    ToolTip="Add Break Up"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                    ToolTip="Save Break Up"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Save" Visible="false" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Save" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdRowSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <eluc:Status ID="ucStatus" runat="server" />
                <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnApprove_Click" OKText="Yes"
                    CancelText="No" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvCCT" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
