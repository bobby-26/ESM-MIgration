<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJobsList.aspx.cs" Inherits="PlannedMaintenanceComponentJobsList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manual" Src="~/UserControls/UserControlPMSManual.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component Jobs</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .imgbtn-height {
            height: 20px;
        }
    </style>
</head>
<body>
    <form id="frmComponentJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Position="BottomCenter"
            Animation="Fade" AutoTooltipify="false" Width="300px" Font-Size="Large" RenderInPageRoot="true" AutoCloseDelay="80000">
            <TargetControls>
            </TargetControls>
        </telerik:RadToolTipManager>
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" Height="100%">
            <eluc:TabStrip ID="MenuDivComponentJob" runat="server" OnTabStripCommand="MenuDivComponentJob_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="Status" runat="server" />
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvComponentJob" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvComponentJob" Height="91%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvComponentJob_NeedDataSource" OnPreRender="gvComponentJob_PreRender" OnSortCommand="gvComponentJob_SortCommand"
                OnItemDataBound="gvComponentJob_ItemDataBound" OnItemCommand="gvComponentJob_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true" EnableLinqExpressions="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCOMPONENTJOBID" TableLayout="Fixed" AllowFilteringByColumn="true">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false">
                            <HeaderStyle Width="30px" />
                            <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOverDueYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDWORKSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadBinaryImage ID="imgFlag" ToolTip="OverDue" ImageUrl="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component No." FilterDelay="2000" FilterControlWidth="100%" ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDCOMPONENTNUMBER" DataField="FLDCOMPONENTNUMBER">
                            <HeaderStyle Width="70px" HorizontalAlign="Left" />
                            <ItemStyle Width="70px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Name" FilterDelay="2000" FilterControlWidth="100%" ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDCOMPONENTNAME" DataField="FLDCOMPONENTNAME">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Code" FilterDelay="2000" FilterControlWidth="100%" ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDJOBCODE" DataField="FLDJOBCODE">
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJobID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFunctionCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Title" FilterDelay="2000" FilterControlWidth="100%" ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDJOBTITLE" DataField="FLDJOBTITLE">
                            <HeaderStyle Width="200px" HorizontalAlign="Left" />
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Category" AllowSorting="true" UniqueName="FLDJOBCATEGORY" ShowSortIcon="false" FilterControlWidth="99%"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo">
                            <HeaderStyle Width="120px" HorizontalAlign="Left" />
                            <ItemStyle Width="120px" HorizontalAlign="Left" />
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblJobCategory" runat="server" OnDataBinding="cblJobCategory_DataBinding" AppendDataBoundItems="true"
                                    SelectedValue='<%# ViewState["FLDJOBCATEGORY"].ToString() %>' OnClientSelectedIndexChanged="CategoryIndexChanged" Width="98%">
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function CategoryIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            tableView.filter("FLDJOBCATEGORY", args.get_item().get_value(), "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCATEGORY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Priority" FilterDelay="2000" FilterControlWidth="100%" ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDPRIORITY" DataField="FLDPRIORITY">
                            <HeaderStyle Width="60px" HorizontalAlign="Center" />
                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPriority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Responsibility" ColumnGroupName="ResDescipline" HeaderStyle-Width="150px" UniqueName="FLDDISCIPLINENAME"
                            AllowSorting="true" SortExpression="FLDDISCIPLINENAME" FilterControlWidth="80px" FilterDelay="2000"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlResponsibility" OnDataBinding="ddlResponsibility_DataBinding" AppendDataBoundItems="true"
                                    SelectedValue='<%# ViewState["FLDDISCIPLINENAME"].ToString() %>' OnClientSelectedIndexChanged="ResponsibilityIndexChanged"
                                    runat="server">
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function ResponsibilityIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            tableView.filter("FLDDISCIPLINENAME", args.get_item().get_value(), "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCIPLINENAME") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDiscipline" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDISCIPLINE") %>' Visible="false" />
                                <eluc:Discipline ID="ucDiscipline" runat="server" CssClass="input" AppendDataBoundItems="true" DisciplineList='<%# PhoenixRegistersDiscipline.ListDiscipline() %>'
                                    SelectedDiscipline='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDISCIPLINE")  %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency" ColumnGroupName="Frequency" AllowFiltering="true" UniqueName="FLDFREQUENCYTYPE"
                            AutoPostBackOnFilter="false" FilterDelay="2000">
                            <HeaderStyle Width="140px" HorizontalAlign="Left" />
                            <FilterTemplate>
                                <telerik:RadTextBox ID="txtFrequency" runat="server" Width="40%" Text='<%# ViewState["FLDFREQUENCY"] %>'></telerik:RadTextBox>
                                <telerik:RadComboBox ID="cblFrequencyType" runat="server" OnDataBinding="cblFrequencyType_DataBinding" AutoPostBack="false" Width="60%" AppendDataBoundItems="true"
                                    OnClientSelectedIndexChanged="FrequencyIndexChanged" SelectedValue='<%# ViewState["FLDFREQUENCYTYPE"] %>'>
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function FrequencyIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var frequency = $find('<%# ((GridItem)Container).FindControl("txtFrequency").ClientID %>');
                                            var freqtype = args.get_item().get_value();
                                            tableView.filter("FLDFREQUENCYTYPE", frequency.get_value() + "~" + freqtype, "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemStyle Wrap="false" Width="140px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Manuals Mapped" AllowFiltering="false" UniqueName="FLDMANUALEXIST">
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cmbManual" runat="server" RenderMode="Lightweight" AutoPostBack="false" Width="100%"
                                    SelectedValue='<%# ViewState["FLDMANUALEXIST"] %>'
                                    OnClientSelectedIndexChanged="ManualIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function ManualIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var freqtype = args.get_item().get_value();
                                            tableView.filter("FLDMANUALEXIST", freqtype, "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <eluc:Manual ID="ucManual" runat="server" ComponentJobId='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="RA Required" AllowFiltering="true" UniqueName="FLDISRAREQUIRED">
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                            <HeaderStyle HorizontalAlign="Center" Width="60px" />
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cmbRa" runat="server" RenderMode="Lightweight" AutoPostBack="false" Width="100%"
                                    SelectedValue='<%# ViewState["FLDISRAREQUIRED"] %>'
                                    OnClientSelectedIndexChanged="RaIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function RaIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var freqtype = args.get_item().get_value();
                                            tableView.filter("FLDISRAREQUIRED", freqtype, "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lblRARequired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAREQUIRED").ToString() =="0"?"No":"Yes" %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Attachment Required" AllowFiltering="true" UniqueName="FLDATTACHMENTREQYN">
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                            <HeaderStyle HorizontalAlign="Center" Width="60px" />
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cmbAttach" runat="server" RenderMode="Lightweight" AutoPostBack="false" Width="100%"
                                    SelectedValue='<%# ViewState["FLDATTACHMENTREQYN"] %>'
                                    OnClientSelectedIndexChanged="AttachIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function AttachIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var freqtype = args.get_item().get_value();
                                            tableView.filter("FLDATTACHMENTREQYN", freqtype, "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lblAttachmentRequired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTREQYN").ToString() =="0"?"No":"Yes" %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reporting Template" AllowFiltering="true" UniqueName="FLDISTEMPLATEMAPPED">
                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                            <HeaderStyle HorizontalAlign="Center" Width="150px" />
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cmbTemplate" runat="server" RenderMode="Lightweight" AutoPostBack="false" Width="100%"
                                    SelectedValue='<%# ViewState["FLDISTEMPLATEMAPPED"] %>'
                                    OnClientSelectedIndexChanged="TemplateIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function TemplateIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var freqtype = args.get_item().get_value();
                                            tableView.filter("FLDISTEMPLATEMAPPED", freqtype, "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lblReportingTemplate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATENAMES")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Verification Level" AllowFiltering="true" UniqueName="FLDVERIFICATIONLEVEL">
                            <ItemStyle HorizontalAlign="Left" Width="90px" />
                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cmbverification" runat="server" RenderMode="Lightweight" AutoPostBack="false" Width="100%"
                                    SelectedValue='<%# ViewState["FLDVERIFICATIONLEVEL"] %>'
                                    OnClientSelectedIndexChanged="verifyIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                        <telerik:RadComboBoxItem Text="Vessel" Value="1" />
                                        <telerik:RadComboBoxItem Text="Office" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function verifyIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var freqtype = args.get_item().get_value();
                                            tableView.filter("FLDVERIFICATIONLEVEL", freqtype, "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerificationLevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFICATIONLEVEL")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remaining Frequency" AllowFiltering="false">
                            <HeaderStyle Width="90px" HorizontalAlign="Left" />
                            <ItemStyle Wrap="false" Width="90px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemainingFreq" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMAININGFREQUENCY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done Date" ColumnGroupName="LastDoneDate" SortExpression="FLDJOBLASTDONEDATE" AllowFiltering="false">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastDonedate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBLASTDONEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtLastDonedate" runat="server" Width="100%" MaxLength="10" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBLASTDONEDATE")) %>'></eluc:Date>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done Hour" ColumnGroupName="LastDoneHour" AllowFiltering="false">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastDoneHour" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLASTDONEHOURS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtLastDoneHour" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTDONEHOURS") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due Date" ColumnGroupName="DueDate" SortExpression="FLDPLANNINGDUEDATE" AllowFiltering="false">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action" AllowFiltering="false">
                            <HeaderStyle Width="145px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="JOBDESCRIPTION" CommandArgument="<%# Container.DataSetIndex %>" ID="btnJobDesc"
                                    ToolTip="Job Description" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="AUDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="btnAudit"
                                    ToolTip="Audit Trial" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fa fa-file"></i></span>
                                </asp:LinkButton>
                                <%--<asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="MAINFORM" CommandArgument="<%# Container.DataSetIndex %>" ID="btnMainForm"
                                    ToolTip="Maintenance Form" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-file-alt"></i></span>
                                </asp:LinkButton>--%>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="ATTACHMENT" CommandArgument="<%# Container.DataSetIndex %>" ID="btnAttachment"
                                    ToolTip="Job Attachment" Width="20PX" Height="20PX">
                                <span class="icon"> <i class="fa fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <span>
                <img id="Img2" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                &nbsp
                <telerik:RadLabel ID="lblOverdue" runat="server" Text="* Overdue"></telerik:RadLabel>
            </span>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

