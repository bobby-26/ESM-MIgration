<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionManagementOfChangeList.aspx.cs"
    Inherits="InspectionManagementOfChangeList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MOC List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">

                function ConfirmRevision(args) {
                    if (args) {
                        __doPostBack("<%=ucConfirmRevision.UniqueID %>", "");
                    }
                }

            </script>
            <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvMOC.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmMOC" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            EnableAJAX="false" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuMOC" runat="server" OnTabStripCommand="MenuMOC_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMOC" runat="server" AllowMultiRowSelection="true" AllowFilteringByColumn="true" FilterType="CheckList"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" EnableHeaderContextMenu="true"
                GridLines="None" OnItemCommand="gvMOC_RowCommand" OnItemDataBound="gvMOC_ItemDataBound"
                OnNeedDataSource="gvMOC_NeedDataSource" ShowFooter="false" EnableViewState="true" Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" ClientDataKeyNames="FLDMOCID"
                    AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDMOCID" AllowFilteringByColumn="true">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <NoRecordsTemplate>
                        <table id="Table2" runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Office/Ship" ShowFilterIcon="false" FilterControlWidth="99%" UniqueName="FLDVESSELID" DataField="FLDVESSELID">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ucVessel" runat="server" OnDataBinding="ucVessel_DataBinding" AppendDataBoundItems="true" AutoPostBack="true"
                                    Width="100%" OnSelectedIndexChanged="ucVessel_SelectedIndexChanged" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"  >
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmocid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblvesselid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblvessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblmocintermediateverificationid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERMEDIATEVERIFICATIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" AllowFiltering="false" DataField="FLDMOCTITLE" UniqueName="FLDMOCTITLE" FilterDelay="2000" FilterControlWidth="99%"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle  Width="18%" />
                            <ItemStyle Wrap="False"  HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmoctitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCTITLE") %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="moctitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCTITLE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proposed" AllowFiltering="false">
                            <HeaderStyle Width="9%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMOCDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDMOCDATE")) %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref No" ShowFilterIcon="false" FilterControlWidth="99%" DataField="FLDMOCREFERENCENO"
                            UniqueName="FLDMOCREFERENCENO" FilterDelay="2000" HeaderStyle-Width="12%" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmocrequestid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkRefNumber" runat="server" CommandName="select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCREFERENCENO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" ShowFilterIcon="false" FilterControlWidth="99%" DataField="FLDMOCCATEGORYID" UniqueName="FLDMOCCATEGORYID">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlCategory" OnDataBinding="ddlCategory_DataBinding" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    Width="100%" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlCategory"] %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChangeCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="Category" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Category" ShowFilterIcon="false" FilterControlWidth="99%" UniqueName="FLDMOCSUBCATEGORYID" DataField="FLDMOCSUBCATEGORYID">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlSubCategory" OnDataBinding="ddlSubCategory_DataBinding" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    Width="100%" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlSubCategory"] %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" Width="8%" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORY") %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ChangeCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nature of Change" ShowFilterIcon="false" FilterControlWidth="99%" DataField="FLDNATUREOFCHANGE" UniqueName="FLDNATUREOFCHANGE">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlNatureofChange" runat="server" AppendDataBoundItems="true" Width="99%" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlNatureofChange_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlNChange"] %>'>
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                                        <telerik:RadComboBoxItem Text="Permanent" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Temporary" Value="2"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNatureofChange" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATUREOFCHANGE") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target Date" ShowFilterIcon="false" FilterControlWidth="99%"
                            FilterDelay="2000" AllowSorting="true" SortExpression="FLDIMPLEMENTATIONDATE" ShowSortIcon="true" DataField="FLDIMPLEMENTATIONDATE" UniqueName="FLDIMPLEMENTATIONDATE">
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["FDATE"].ToString() %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["TDATE"].ToString() %>' />
                                <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDIMPLEMENTATIONDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(FromPicker);
                                            var toDate = FormatSelectedDate(sender);

                                            tableView.filter("FLDIMPLEMENTATIONDATE", fromDate + "~" + toDate, "Between");
                                        }
                                        function FormatSelectedDate(picker) {
                                            var date = picker.get_selectedDate();
                                            var dateInput = picker.get_dateInput();
                                            var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());

                                            return formattedDate;
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTargetDateofImplementation" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDIMPLEMENTATIONDATE")) %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn HeaderText="Status" ShowFilterIcon="false" FilterControlWidth="99%" UniqueName="FLDMOCSTATUSID">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlstatus" OnDataBinding="ddlstatus_DataBinding" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    Width="100%" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlStatus"] %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSID") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rev.No" AllowFiltering="false">
                        <HeaderStyle Width="4%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRevisionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNUMBER") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Next Int.Date" AllowFiltering="false">
                        <HeaderStyle Width="10%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNextIntermediateVerificationDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDNEXTINTERIMDUEDATE")) %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completion" AllowFiltering="false">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" HeaderStyle-Width="15%" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" AlternateText="Revision" CommandName="REVISION" ID="imgrevision"
                                    ToolTip="Create Revision" Visible="false">
                                 <span class="icon"><i class="fas fa-registered"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="ViewRevision" CommandName="VIEWREVISION" ID="cmdViewRevisions"
                                    ToolTip="View Revisions" >
                                <span class="icon"><i class="fas fa-eye"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EditrequestExtension"
                                    ID="cmdEdit" ToolTip="Edit" Visible="false">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EditActionPlan"
                                    ID="cmdActionPlanEdit" ToolTip="Action Plan">
                                    <span class="icon"><i class="fas fa-list"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="Editintverification"
                                    ID="cmdintverificationEdit" ToolTip="Intermediate verification">
                                    <span class="icon"><i class="fas fa-clipboard-check"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Copy Approval" CommandName="EditApprovalofMOC"
                                    ID="imgApproval" ToolTip="Approval" Visible="false">
                                    <span class="icon"><i class="fas fa-user-check"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Copy template" CommandName="COPYTEMPLATE"
                                    ID="imgCopyTemplate" ToolTip="Copy template">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <asp:Button ID="ucConfirmRevision" runat="server" Text="ConfirmRevision" OnClick="ucConfirmRevision_Click" />
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
    </form>
</body>
</html>
