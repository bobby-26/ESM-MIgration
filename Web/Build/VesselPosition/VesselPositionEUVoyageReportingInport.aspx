<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUVoyageReportingInport.aspx.cs" Inherits="VesselPositionEUVoyageReportingInport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Departure Report..</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id = "div1" runat ="server" >
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
        <asp:UpdatePanel runat="server" ID="panel1">
            <ContentTemplate>
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="EU Voyages " ShowMenu="true">
                        </eluc:Title>
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <eluc:Status runat="server" ID="ucStatus" />
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuDepartureReport" runat="server" OnTabStripCommand="MenuDepartureReport_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                </div>
                <br />
                <table width ="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblmVesselName" runat="server" Text="Vessel"></asp:Literal>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <eluc:Vessel ID="UcVessel" runat="server" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="UcVessel_OnTextChangedEvent" SyncActiveVesselsOnly="True" VesselsOnly="true" />
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                           <%-- <eluc:Port ID="ucPortEdit" runat="server" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" SeaportList='<%#PhoenixRegistersSeaport.ListSeaport()%>'
                                    Width="260px" />--%>
                            <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input" Width="300px" IsEUPort="1"  />
                        </td>
                        <td>
                            <asp:Literal ID="lblArrivalFromDate" runat="server" Text="Arrival From - To"></asp:Literal>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtArrivalFrom" CssClass="input" DatePicker="true" />
                            <asp:Literal ID="lblArrivalFromTo" runat="server" Text=" - "></asp:Literal>
                            <eluc:Date runat="server" ID="txtArrivalTo" CssClass="input" DatePicker="true" />
                        </td>
                        
                    </tr>
                     <tr>
                        <td><asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal></td>
                        <td>
                          <eluc:Fleet runat="server" ID="ucFleet" Width="60%" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucFleet_OnTextChangedEvent" />
                        </td>
                        <td>
                            <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                        </td>
                        <td><eluc:Address runat="server" ID="ucOwner" CssClass="input" AddressType="128" Width="200" AutoPostBack="true"  OnTextChangedEvent="ucOwner_OnTextChangedEvent"
                               AppendDataBoundItems="true" Enabled="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDepartureFrom" runat="server" Text="Departure From - To" Visible="false"></asp:Literal>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtDepartureFrom" CssClass="input" DatePicker="true"  Visible="false"/>
                            <asp:Literal ID="lblDepartureTo" runat="server" Text=" - " Visible="false"></asp:Literal>
                            <eluc:Date runat="server" ID="txtDepartureTo" CssClass="input" DatePicker="true" Visible="false" />
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:Literal runat="server" ID="lblShowNonEU" Text="Show All Voyages"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkShowNonEU" />
                        </td>
                         <td></td>
                         <td></td>
                         <td></td>
                         <td></td>
                    </tr>
                </table>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                                <eluc:TabStrip ID="MenuCrewCourseList" runat="server" OnTabStripCommand="CrewCourseList_TabStripCommand">
                            </eluc:TabStrip>
                </div>
                    <table width="100%">
                        <tr>
                            <td>
                           <div class="navSelect" style="position: relative; width: 15px">
                                <eluc:TabStrip ID="TabStrip1" runat="server" OnTabStripCommand="CrewCourseList_TabStripCommand">
                            </eluc:TabStrip>
                            </div>
                                <div id="divGrid" style="position: relative; z-index: 0">
                                    <asp:GridView ID="gvConsumption" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnRowCreated="gvConsumption_RowCreated"
                                        Width="100%" CellPadding="3" AllowSorting="true" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                                         OnRowCommand="gvConsumption_RowCommand" OnRowDataBound="gvConsumption_ItemDataBound">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Vessel">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblVesselHeader" runat="server" Text="Vessel"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVesselItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>
                                                    <asp:Label ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField >
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblVoyNoHeader" runat="server" Text="Voy No"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVoyNoItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOYAGENO") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblFromHeader" runat="server" Text="Port"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFromtem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTO") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCommencedHeader" runat="server" Text="Arrival"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCommencedItem" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETED")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMPLETED", "{0:HH:mm}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCompletedHeader" runat="server" Text="Departure"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompletedItem" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCURRENTSBE")) + " " + DataBinder.Eval(Container,"DataItem.FLDCURRENTSBE", "{0:HH:mm}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblTimeInportHeader" runat="server" Text="Time In Port"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTimeInportItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMEINPORT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblDistanceHeader" runat="server" Text="On Arrival"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDistanceItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOQTYONARRIVAL") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblTimeAtSeaHeader" runat="server" Text="Loaded"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTimeAtSeaItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOQTYLOADED") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCargoQtyHeader" runat="server" Text="Discharged"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCargoQtyItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOQTYDISC") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblTransportWorkHeader" runat="server" Text="On Departure"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransportWorkItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOQTYONDEPT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblHFOHeader" runat="server" Text="HFO Cons (MT)"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHFOItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFOCONSINPORT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblMDOHeader" runat="server" Text="MDO/MGO Cons (MT)"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMDOItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOCONSINPORT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblAggCo2EmittedHeader" runat="server" Text="Agg CO₂ Emitted (T-CO₂)"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAggCo2EmittedItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCO2EMISSIONINPORT") %>'></asp:Label>
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
                                        <asp:ImageButton runat="server" AlternateText="Send Mail" ImageUrl="<%$ PhoenixTheme:images/48.png %>"
                                            CommandName="EMAIL" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdmail"
                                            ToolTip="Send Mail"></asp:ImageButton>
                                    </ItemTemplate>
                                   
                                </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                        
                                </div>
                            </td>
                        </tr>
                    </table>
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
                                        <eluc:Number ID ="txtnopage" runat="server" CssClass = "input" MaxLength="9" Width="20px" IsInteger="true" />
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                            Width="40px"></asp:Button>
                                </td>
                                </tr>
                                    <eluc:Status runat="server" ID="Status1" />
                            </table>
                        </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
