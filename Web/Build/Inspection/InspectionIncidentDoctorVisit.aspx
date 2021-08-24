<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentDoctorVisit.aspx.cs"
    Inherits="InspectionIncidentDoctorVisit" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doctor Visit</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>


</head>
<body>
    <form id="frmIncidentDoctorVisitGeneral" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <%--<eluc:status runat="server" id="ucStatus"></eluc:status>--%>



                <eluc:TabStrip ID="MenuIncidentDoctorVisit" TabStrip="true" runat="server" OnTabStripCommand="MenuIncidentDoctorVisit_TabStripCommand"></eluc:TabStrip>

                <eluc:TabStrip ID="MenuIncidentDoctorVisitGeneral" runat="server" OnTabStripCommand="MenuIncidentDoctorVisitGeneral_TabStripCommand"></eluc:TabStrip>


                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblIncidentDoctorVisit" width="100%">
                        <tr>
                            <td>Reference No.
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" Width="200px"
                                    ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                            <td>Doctor Visit
                            </td>
                            <td>
                                <eluc:Date ID="ucDoctorVisitDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>Vessel
                            </td>
                            <td>
                                <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" Width="200px"
                                    CssClass="input_mandatory" VesselsOnly="true" AutoPostBack="true" />
                            </td>
                            <td>Name
                            </td>
                            <td>
                                <span id="spnCrewInCharge">
                                    <telerik:RadTextBox ID="txtCrewName" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="50" Width="244px">
                                    </telerik:RadTextBox>
                                    <img runat="server" id="imgShowCrewInCharge" style="cursor: pointer; vertical-align: top"
                                        src="<%$ PhoenixTheme:images/picklist.png %>" />
                                    <telerik:RadTextBox ID="txtCrewRank" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="50" Width="60px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtCrewId" runat="server" CssClass="input" MaxLength="20" Width="1px"></telerik:RadTextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">Port
                            </td>
                            <td style="width: 30%">
                                <eluc:MultiPort ID="ucPort" runat="server" CssClass="dropdown_mandatory" Width="200px" />
                            </td>
                            <td style="width: 20%">Port Agent
                            </td>
                            <td style="width: 30%">
                                <span id="spnPickListAgent">
                                    <telerik:RadTextBox ID="txtAgentNumber" runat="server" Width="60px" CssClass="input_mandatory"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAgentName" runat="server" Width="180px" CssClass="input_mandatory"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton runat="server" ID="cmdShowAgent" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle"
                                        Text=".." />
                                    <telerik:RadTextBox ID="txtAgent" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">Illness On
                            </td>
                            <td style="width: 30%">
                                <eluc:Date ID="ucDateIllness" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                            <td style="width: 20%">Illness Description
                            </td>
                            <td style="width: 30%">
                                <telerik:RadTextBox ID="txtIllnessDescription" runat="server" CssClass="input" Height="50px"
                                    Width="250px" TextMode="MultiLine" Rows="3">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>

                    <eluc:TabStrip ID="MenuDeficiency" runat="server" OnTabStripCommand="Deficiency_TabStripCommand"></eluc:TabStrip>

                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;" runat="server">
                        <%-- <asp:GridView ID="gvDeficiency" runat="server" AutoGenerateColumns="False" OnRowCommand="gvDeficiency_RowCommand"
                                Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvDeficiency_ItemDataBound"
                                OnRowDeleting="gvDeficiency_RowDeleting" ShowHeader="true" EnableViewState="false"
                                OnSelectedIndexChanging="gvDeficiency_SelectIndexChanging" AllowSorting="true"
                                OnSorting="gvDeficiency_Sorting" DataKeyNames="FLDPNIMEDICALCASEID" OnRowEditing="gvDeficiency_RowEditing">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvDeficiency" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" OnNeedDataSource="gvDeficiency_NeedDataSource"
                            OnItemCommand="gvDeficiency_ItemCommand"
                            OnSortCommand="gvDeficiency_SortCommand"
                            OnItemDataBound="gvDeficiency_ItemDataBound"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"
                            AutoGenerateColumns="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Reference No.">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPNIMedicalCaseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNIMEDICALCASEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                            <asp:LinkButton ID="lnkRefNumber" runat="server" CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vessel">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkVessel" runat="server" CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Name">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCrewName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Rank">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCrewRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWRANK") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Illness Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblIllnessDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFILLNESS"))%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Doctor Visit">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDoctorVisitDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDOCTORVISITDATE"))%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Sickness Report"
                                                CommandName="SICKNESSREPORT" CommandArgument='<%# Container.DataSetIndex %>'
                                                ID="cmdSicknessReport" ToolTip="Sickness Report">
                                                <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="PNI Checklist" CommandName="CHECKLIST"
                                                ID="cmdChkList" ToolTip="PNI Checklist">
                                                <span class="icon"><i class="fas fa-tasks"></i></span>
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

                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
