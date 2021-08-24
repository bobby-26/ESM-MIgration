<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHRTravelPassengerApprovalList.aspx.cs" Inherits="CrewHRTravelPassengerApprovalList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Passenger Approval List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>       

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewHrPassenger" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlTravelPassenger">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <eluc:Title runat="server" ID="Title1" Text="Details" ShowMenu="true">
                    </eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuTravelPassengerMain" runat="server" OnTabStripCommand="TravelPassengerMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative">  
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuTravelPassengerSub" runat="server" TabStrip="false" OnTabStripCommand="TravelPassengerSub_TabStripCommand">
                        </eluc:TabStrip>                   
                    </span>
                    <eluc:Title runat="server" ID="Title2" Text="" ShowMenu ="false"></eluc:Title>
                </div>
                
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblvessel" runat="server" Text="Vessel" ></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" AssignedVessels="true" 
                                Width="120px" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />                  
                
                <asp:Label id="lblTravelBreakUp" runat="server" Text="Breakup Details" Font-Bold="true" ></asp:Label>
                
                <div id="div1" style="position: relative; z-index: 1; width: 100%;">
                    <asp:GridView ID="gvTravelRequestBreakup" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowDataBound="gvTravelRequestBreakup_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                        OnRowCommand="gvTravelRequestBreakup_RowCommand" OnSelectedIndexChanging="gvTravelRequestBreakup_SelectedIndexChanging"
                        OnRowDeleting="gvTravelRequestBreakup_RowDeleting" EnableViewState="false" DataKeyNames="FLDTRAVELBREAKUPID"
                        OnRowUpdating="gvTravelRequestBreakup_RowUpdating" OnRowCancelingEdit="gvTravelRequestBreakup_RowCancelingEdit" 
                        OnRowEditing="gvTravelRequestBreakup_RowEditing" ShowFooter="true" >
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="S.No.">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblRequestNoHeader" runat="server"> S.No.</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBREAKUPROWNUMBER") %>' ></asp:Label>
                                    <asp:Label ID="lblTravelRequestId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELREQUESTID") %>'
                                     Visible="false" ></asp:Label>
                                     <asp:Label ID="lblTravelBreakupId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELBREAKUPID") %>'
                                     Visible="false" ></asp:Label>
                                     <asp:Label ID="lblPersonalInfosn" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPERSONALINFOSN") %>'
                                     Visible="false" ></asp:Label>
                                     <asp:Label ID="lblOfficeStaffId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOFFICESTAFFID") %>'
                                     Visible="false" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                  
                            
                            <asp:TemplateField HeaderText="Departure">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblNameHeader" runat="server">Depature</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDepatureCityName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDEPATURECITY") %>'
                                        CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                    <asp:Label ID="lblDepatureCityName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURECITY") %>' ></asp:Label>
                                    <asp:Label ID="lblDepatureCityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURECITYID") %>' 
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickListDeparturebreakup">
                                        <asp:TextBox ID="txtDepatureBreakupEdit" runat="server" Width="80%" Enabled="False"
                                            CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURECITY") %>'></asp:TextBox>
                                        <asp:ImageButton ID="btnShowDepaturebreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="Top" Text=".." CommandName="DEPATUREBREAKUP" CommandArgument="<%# Container.DataItemIndex %>"
                                            OnClientClick="return showPickList('spnPickListDeparturebreakup', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                        <asp:TextBox ID="txtDepatureIdBreakupEdit" runat="server" Width="0px" CssClass="hidden"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURECITYID") %>'></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnPickListDeparturebreakupAdd">
                                        <asp:TextBox ID="txtDepatureBreakupAdd" runat="server" Width="80%" Enabled="False"
                                            CssClass="input_mandatory" ></asp:TextBox>
                                        <asp:ImageButton ID="btnShowDepaturebreakupAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="Top" Text=".." CommandName="DEPATUREBREAKUPADD" CommandArgument="<%# Container.DataItemIndex %>"
                                            OnClientClick="return showPickList('spnPickListDeparturebreakupAdd', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                        <asp:TextBox ID="txtDepatureIdBreakupAdd" runat="server" Width="0px" CssClass="hidden" ></asp:TextBox>
                                    </span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="DepartureDate">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblDepartureDateHeader" runat="server">Depature Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                           
                                    <asp:Label ID="lblDepartureDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPATUREDATE")) %>'></asp:Label>
                                    <asp:Label ID="lblDepartureTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATURETIMEAMPM") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date runat="server" ID="txtDepartureDateEdit" CssClass="input_mandatory" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDEPATUREDATE")) %>'>
                                    </eluc:Date>
                                    <asp:DropDownList ID="ddlDepartureTimeEdit" runat="server" CssClass="dropdown_mandatory">
                                        <asp:ListItem Text="AM" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="PM" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date runat="server" ID="txtDepartureDateAdd" CssClass="input_mandatory">
                                    </eluc:Date>
                                    <asp:DropDownList ID="ddlDepartureTimeAdd" runat="server" CssClass="dropdown_mandatory">
                                        <asp:ListItem Text="AM" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="PM" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Destination">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblNameHeader" runat="server">Destination</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDestinationCityName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITY") %>'
                                        CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                    <asp:Label ID="lblDestinationCityName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITY") %>' ></asp:Label>
                                    <asp:Label ID="lblDestinationCityId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITYID") %>' 
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickListDestinationBreakup">
                                        <asp:TextBox ID="txtDestinationBreakupEdit" runat="server" Width="80%" Enabled="False"
                                            CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITY") %>'></asp:TextBox>
                                        <asp:ImageButton ID="btnShowDestinationbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="Top" Text=".." CommandName="DESTINATIONBREAKUP" CommandArgument="<%# Container.DataItemIndex %>"
                                            OnClientClick="return showPickList('spnPickListDestinationBreakup', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                        <asp:TextBox ID="txtDestinationIdBreakupEdit" runat="server" Width="0px" CssClass="hidden"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONCITYID") %>'></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnPickListDestinationBreakupAdd">
                                        <asp:TextBox ID="txtDestinationBreakupAdd" runat="server" Width="80%" Enabled="False"
                                            CssClass="input_mandatory"></asp:TextBox>
                                        <asp:ImageButton ID="btnShowDestinationBreakupAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="Top" Text=".." CommandName="DESTINATIONBREAKUPADD" CommandArgument="<%# Container.DataItemIndex %>"
                                            OnClientClick="return showPickList('spnPickListDestinationBreakupAdd', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                        <asp:TextBox ID="txtDestinationIdBreakupAdd" runat="server" Width="0px" CssClass="hidden" ></asp:TextBox>
                                    </span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="ArrivalDate">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblArrivalDateHeader" runat="server">Arrival Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                           
                                    <asp:Label ID="lblArrivalDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE")) %>'></asp:Label>
                                    <asp:Label ID="lblArrivalTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALTIMEAMPM") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date runat="server" ID="txtArrivalDateEdit" CssClass="input_mandatory" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE")) %>'>
                                    </eluc:Date>
                                    <asp:DropDownList ID="ddlArrivalTimeEdit" runat="server" CssClass="dropdown_mandatory">
                                        <asp:ListItem Text="AM" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="PM" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date runat="server" ID="txtArrivalDateAdd" CssClass="input_mandatory">
                                    </eluc:Date>
                                    <asp:DropDownList ID="ddlArrivalTimeAdd" runat="server" CssClass="dropdown_mandatory">
                                        <asp:ListItem Text="AM" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="PM" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Class">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblClassHeader" runat="server">Class</asp:Label></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblClass" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELCLASSNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Hard ID="ucClassEdit" runat="server" CssClass="input" HardList="<%# PhoenixRegistersHard.ListHard(1,227) %>" 
                                        HardTypeCode="227" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELCLASSID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Hard ID="ucClassAdd" runat="server" CssClass="input" HardList="<%# PhoenixRegistersHard.ListHard(1,227) %>" 
                                        HardTypeCode="227" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Center" Width="100px" />
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdRowSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                    ToolTip="Add"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                 </div>
                <br />
                <br />
               
                <asp:Label id="lblPassengerDetails" runat="server" Text="Passenger Details" Font-Bold="true" ></asp:Label>
                
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuTravelPassenger" runat="server" OnTabStripCommand="TravelPassenger_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                <asp:GridView ID="gvTravelPassenger" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowDataBound="gvTravelPassenger_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                        OnRowCommand="gvTravelPassenger_RowCommand" OnSelectedIndexChanging="gvTravelPassenger_SelectedIndexChanging"
                        OnRowDeleting="gvTravelPassenger_RowDeleting" EnableViewState="false" DataKeyNames="FLDTRAVELPASSENGERID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="S.No.">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblRequestNoHeader" runat="server"> S.No.</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' ></asp:Label>
                                    <asp:Label ID="lblTravelRequestId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELREQUESTID") %>'
                                     Visible="false" ></asp:Label>
                                     <asp:Label ID="lblTravelPassengerId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELPASSENGERID") %>'
                                     Visible="false" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                  
                            
                            <asp:TemplateField HeaderText="Name">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblNameHeader" runat="server">Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                        CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                    <asp:Label ID="lblpersonalinfosn" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPERSONALINFOSN") %>'
                                     Visible="false" ></asp:Label>
                                    <asp:Label ID="lblFamilymembersn" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFAMILYMEMBERSN") %>'
                                     Visible="false" ></asp:Label>
                                    <asp:Label ID="lblOfficeStaffId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOFFICESTAFFID") %>'
                                     Visible="false" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="DOB">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblDOBHeader" runat="server"> DOB</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>                           
                                    <asp:Label ID="lblDOB" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH")) %>'></asp:Label>
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
                                   <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                 </div>
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
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
