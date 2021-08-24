<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreActivityLog.aspx.cs" Inherits="CrewOffshoreActivityLog" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOnReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Activity</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmActivity" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <eluc:TabStrip ID="MenuActivity" Title="Activity Log" runat="server" OnTabStripCommand="Activity_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="Employee Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                            ReadOnly="True" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPresentRank" runat="server" Text="Present Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPayRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <%--  <tr>
                    
                    <td>
                        Last Vessel
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastVessel" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True"></telerik:RadTextBox>
                    </td>
                    <td>
                        Signed Off
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtSignedOff" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True"></telerik:RadTextBox>
                    </td>                 
                </tr>--%>
            </table>
            <hr />
            <table cellpadding="1" cellspacing="1" width="103%" runat="server" visible="false">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActivity" runat="server" Text="Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlActivity" runat="server" HardTypeCode="77" AppendDataBoundItems="true" ShortNameFilter="SI,STL,UPL,SEC,CRB,TT,TF,SS,LV,AME,VTO,COR,NOT,NOR"
                            CssClass="dropdown_mandatory" AutoPostBack="true" OnTextChangedEvent="ddlActivity_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            OnTextChangedEvent="txtFromDate_TextChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" CssClass="input"
                            VesselsOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReasonOn" runat="server" Text="Reason On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignOnReason ID="ddlSignOnReason" runat="server" AppendDataBoundItems="true"
                            CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOnPort" runat="server" Text="Sign On Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ddlSignOnPort" runat="server" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffReason" runat="server" Text="Sign Off Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignOffReason ID="ddlSignOffReason" runat="server" AppendDataBoundItems="true"
                            CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffPort" runat="server" Text="Sign Off Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ddlSignOffPort" runat="server" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Course ID="ucCourse" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCourseVenue" runat="server" Text="Course Venue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCourseVenue" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCourseCountry" runat="server" Text="Course Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ucCountry" runat="server" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="200px" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuCrewActivityLog" runat="server" OnTabStripCommand="MenuCrewActivityLog_TabStripCommand"></eluc:TabStrip>

            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%-- <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvActivity_RowDataBound" OnRowEditing="gvActiviy_RowEditing"
                    OnRowDeleting="gvActivity_RowDeleting" ShowHeader="true" EnableViewState="false"
                    AllowSorting="true" OnSorting="gvActivity_Sorting">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvActivity" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvActivity_NeedDataSource"
                    OnItemCommand="gvActivity_ItemCommand"
                    OnSortCommand="gvActivity_SortCommand"
                    OnItemDataBound="gvActivity_ItemDataBound"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
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
                           
                            <telerik:GridTemplateColumn HeaderText="Activity Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblActivityLogId" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDACTIVITYLOGID") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblSystemGen" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSYSTEMGEN") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblOverLap" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOVERLAP") %>'></asp:Label>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDACTIVITYCODE") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Activity Name">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDACTIVITYNAME") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="From Date">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="To Date">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Center" Visible="false">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" Visible="false" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
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

            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <table>
                            <tr class="rowoverlap">
                                <td width="5px" height="10px"></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOverlappingActivities" runat="server" Text="Overlapping Activities"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:Confirm ID="ucConfirm" runat="server" Visible="false" OnConfirmMesage="InsertCrewActivity" />
        </div>

    </form>
</body>
</html>
