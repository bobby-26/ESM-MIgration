<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreEmployeeExperience.aspx.cs"
    Inherits="CrewOffshoreEmployeeExperience" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Company Experience</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
function Resize() {
setTimeout(function () {
TelerikGridResize($find("<%= gvCrewCompanyExperience.ClientID %>"));
}, 200);
}
window.onresize = window.onload = Resize;

function pageLoad(sender, eventArgs) {
Resize();
fade('statusmessage');
}
</script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewCompanyExperience" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


            <eluc:TabStrip ID="CrewQuery" runat="server" Title="Crew Experience" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>


                <table id="tblCrewCompanyExperience" width="100%">
                    <tr>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
  
            <eluc:TabStrip ID="MenuCrewCompanyExperience" runat="server" OnTabStripCommand="CrewCompanyExperience_TabStripCommand"></eluc:TabStrip>


            <%-- <asp:GridView ID="gvCrewCompanyExperience" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3"
                    OnRowDataBound="gvCrewCompanyExperience_RowDataBound"
                    ShowFooter="false" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true" OnSorting="gvCrewCompanyExperience_Sorting"
                    OnRowCreated="gvCrewCompanyExperience_RowCreated">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" Wrap="False" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <RowStyle Height="10px" />
                    <RowStyle Height="10px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCompanyExperience" runat="server"  AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewCompanyExperience_NeedDataSource"
                OnItemCommand="gvCrewCompanyExperience_ItemCommand"
                OnItemDataBound="gvCrewCompanyExperience_ItemDataBound"
                OnSortCommand="gvCrewCompanyExperience_SortCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" >
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Engine" Name="Engine" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="No. of days of this operation" Name="Operation" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Total" Name="Total" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <HeaderStyle Width="102px" />
                    
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Company">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblManningCompanyName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucManningCompanyTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANNINGCOMPANY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewCompanyExperienceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExperienceFlag" runat="server" Visible="false" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCEFLAG") %>'>
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkRank" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DP 1/2 /3">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDPClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPCLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DP Make and Model">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="200px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDPMakeandModel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPMAKEANDMODEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="GRT">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGrtNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELGT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Make" ColumnGroupName="Engine">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEngineMake" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Model" ColumnGroupName="Engine">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEngineModel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINETYPEMODEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="BHP">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBHP" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBHP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Propulsion">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPropulsion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPULSIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Operating Voltage">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoltage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOLTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supply" ColumnGroupName="Operation">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupply" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLYCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Anchor Handling" ColumnGroupName="Operation">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAnchorHandling" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANCHORHANDLING") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Towing" ColumnGroupName="Operation">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTowing" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOWINGCOUTN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Dive Support" ColumnGroupName="Operation">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDIVESUPPORTCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ROV" ColumnGroupName="Operation">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblROV" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROVCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flotel" ColumnGroupName="Operation">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlotel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLOTELCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type of Installation Served">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstallationServed" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINSTALLATIONSERVED").ToString().Length>40 ? DataBinder.Eval(Container, "DataItem.FLDINSTALLATIONSERVED").ToString().Substring(0, 40) + "..." : DataBinder.Eval(Container, "DataItem.FLDINSTALLATIONSERVED").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucInstallationServed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLATIONSERVED") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Operating Area">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOperatingArea" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOPERATINGAREA").ToString().Length>40 ? DataBinder.Eval(Container, "DataItem.FLDOPERATINGAREA").ToString().Substring(0, 40) + "..." : DataBinder.Eval(Container, "DataItem.FLDOPERATINGAREA").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucOperatingArea" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATINGAREA") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Charterer">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCharterer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCHARTERER").ToString().Length>40 ? DataBinder.Eval(Container, "DataItem.FLDCHARTERER").ToString().Substring(0, 40) + "..." : DataBinder.Eval(Container, "DataItem.FLDCHARTERER").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucCharterer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARTERER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDFROMDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbToDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTODATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Duration" ColumnGroupName="Total">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuration" runat="server" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign Off Reason">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOffReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFREASONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
</telerik:RadAjaxPanel>
    </form>
</body>
</html>
