<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreEmployeeAppointmentHistory.aspx.cs" Inherits="CrewOffshoreEmployeeAppointmentHistory" %>

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
    <title>Crew Appointment Letter History</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
function Resize() {
setTimeout(function () {
TelerikGridResize($find("<%= gvCrewAppointmentLetter.ClientID %>"));
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
    <form id="frmCrewAppointmentLetter" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="CrewQuery" runat="server" Title="Crew Appointment Letter" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>
          


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

            <hr />

            <eluc:TabStrip ID="MenuCrewAppointmentHistory" runat="server" OnTabStripCommand="CrewAppointmentHistory_TabStripCommand"></eluc:TabStrip>


            <%-- <asp:GridView ID="gvCrewAppointmentLetter" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCrewAppointmentLetter_RowCommand"
                    OnRowDataBound="gvCrewAppointmentLetter_RowDataBound"
                    OnRowEditing="gvCrewAppointmentLetter_RowEditing" ShowFooter="false" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true" OnSorting="gvCrewAppointmentLetter_Sorting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <RowStyle Height="10px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewAppointmentLetter" runat="server"  AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewAppointmentLetter_NeedDataSource"
                OnItemCommand="gvCrewAppointmentLetter_ItemCommand"
                OnItemDataBound="gvCrewAppointmentLetter_ItemDataBound"
                OnSortCommand="gvCrewAppointmentLetter_SortCommand"
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
                    
                    <HeaderStyle Width="102px" />
                    <Columns>
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
                        <telerik:GridTemplateColumn HeaderText="Vessel Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="75px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewAppointmentLetterId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPOINTMENTLETTERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkRank" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Engine Type / Model">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEngine" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINETYPEMODEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign On Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDFROMDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign Off Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbToDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTODATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Duration">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuration" runat="server" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Gap">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGap" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDGAP") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employer">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblManningCompanyName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDMANNINGCOMPANY").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucManningCompanyTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANNINGCOMPANY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last drawn salary/day(USD)">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastSalary" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDAILYRATEUSD")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DP Allowance/day (USD)">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastDP" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDPALLOWANCE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <img id="Img7" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" runat="server" />
                                <asp:ImageButton runat="server" CommandArgument="<%# Container.DataSetIndex %>" CommandName="APPOINTMENTLETTERPDF"
                                    ID="cmdAppointmentLetter" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>" ImageAlign="AbsMiddle"
                                    Text=".." ToolTip="Show Contract" />
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
