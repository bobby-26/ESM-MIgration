<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewIncidents.aspx.cs" Inherits="CrewIncidents" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incidents</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmIncidents" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">


            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <img src="<%$ PhoenixTheme:images/attachment.png %>" id="imgClip" runat="server" style="float: left; top: 5px; left: 10%; position: absolute" visible="false" />


            <eluc:TabStrip ID="MenuIncidents" runat="server" OnTabStripCommand="Incidents_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="dropdown_mandatory" VesselsOnly="true" AppendDataBoundItems="true" AutoPostBack="true"
                            Entitytype="VSL" ActiveVesselsOnly="true" AssignedVessels="true" OnTextChangedEvent="BindSeafarer" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblVoyageNumber" runat="server" Text="Voyage Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoyageNumber" runat="server" CssClass="input" MaxLength="50" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPortLocation" runat="server" Text="Port/Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnDLocation">
                            <telerik:RadTextBox ID="txtSeaportCode" runat="server" Width="0px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtSeaPortName" runat="server" Width="140px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="cmdShowLocation" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="showPickList('spnDLocation', 'codehelp1', '', '../Common/CommonPickListDeliveryLocation.aspx', true); return false;"
                                Text=".." />
                            <telerik:RadTextBox ID="txtSeaPortId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIncidentDate" runat="server" Text="Incident Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtIncidentDate" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnTextChangedEvent="BindSeafarer" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblReportedBy" runat="server" Text="Reported By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReportedBy" runat="server" CssClass="input_mandatory" MaxLength="100" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportedDate" runat="server" Text="Reported Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtReportedDate" runat="server" CssClass="input" />
                    </td>

                </tr>



                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTypeofIncident" runat="server" Text="Type of Incident"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ddlIncidentType" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="BindSeafarer"
                            HardTypeCode="192" />
                        <%--<eluc:Quick ID="ddlIncidentType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankofPerson" runat="server" Text="Rank of Person"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeafarer" runat="server" Text="Crew List"></telerik:RadLabel>
                    </td>
                 

                    <td colspan="5">
                        <div style="height: 150px; overflow-y: auto; width: 100%" class="input_mandatory">
                            <telerik:RadCheckBoxList ID="lstCrewList" runat="server" Width="100%">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblDescriptionofEvent" runat="server" Text="Description of Event"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadEditor ID="txtRemarks" runat="server" Height="250px" Width="100%" activemode="Design">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>

                    </td>
                </tr>
            </table>
            <hr />
            <br />

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%-- <asp:GridView ID="gvIncidents" runat="server" AutoGenerateColumns="False" Width="100%"
                        OnRowEditing="gvIncidents_RowEditing" OnRowDataBound="gvIncidents_RowDataBound"
                        AllowSorting="true" OnSorting="gvIncidents_Sorting" OnRowDeleting="gvIncidents_RowDeleting"
                        CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvIncidents" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvIncidents_NeedDataSource"
                    OnItemDataBound="gvIncidents_ItemDataBound"
                    OnItemCommand="gvIncidents_ItemCommand"
                    OnSortCommand="gvIncidents_SortCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <telerik:GridTemplateColumn HeaderText="Vessel">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIncidentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkVessel" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>' CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Incident Date">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />

                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDINCIDENTDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblReportedBy" runat="server" Text="Reported By"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDREPORTEDBY")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblComplimentsComplaints" runat="server" Text="Compliments/Complaints"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCORRESPONDENCETYPE")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblActionHeader" runat="server">
                                        Action
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Delete"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Crew List"
                                        ID="cmdCrewList"
                                        ToolTip="Crew List">
                                            <span class="icon"><i class="fas fa-user-alt"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>

                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </div>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
