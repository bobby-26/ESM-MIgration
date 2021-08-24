<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReviewPlanner.aspx.cs"
    Inherits="InspectionReviewPlanner" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DepartmentType" Src="~/UserControls/UserControlDepartmentType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inspection Review Planner</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvPlanner").height(browserHeight - 60);
            });
        </script>

        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            } function ConfirmDelete(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }

        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvPlanner.ClientID %>"));
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
    <form id="frmReviewPlanner" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecorationZoneID="tblConfigureAirlines" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Audit / Inspection Schedule" Visible="false" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuScheduleGroup" runat="server" TabStrip="true" OnTabStripCommand="BudgetGroup_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPlanner" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" OnNeedDataSource="gvPlanner_NeedDataSource" OnItemCommand="gvPlanner_ItemCommand" OnItemDataBound="gvPlanner_ItemDataBound" EnableViewState="true"
                OnSortCommand="gvPlanner_SortCommand" AllowSorting="true" OnSelectedIndexChanging="gvPlanner_SelectedIndexChanging" GridLines="None" ShowFooter="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
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
                        <%--<telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="imgFlag" runat="server" Visible="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="110px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME">Vessel</asp:LinkButton>
                                <img id="FLDVESSELNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblInspectingCompanyid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANINSPECTINGCOMPANYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="M/C" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <%--    <HeaderTemplate>
                                <telerik:RadLabel ID="lblMCHeader" runat="server">M/C</telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblManual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMANUAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" HeaderText="Audit/Inspection">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkInspectionNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDINSPECTIONSHORTCODE">Audit/Inspection</asp:LinkButton>
                                <img id="FLDINSPECTIONSHORTCODE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInspectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONSHORTCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInspectionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblScheduleId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblPlannerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWPLANNERID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsManual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMANUALINSPECTION") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="63px" HeaderText="Category">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCATEGORY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCategoryId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCATEGORYID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Last Done">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastDoneDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblInspectionIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:Date ID="ucLastDoneDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>'
                                    DatePicker="true" OnTextChangedEvent="ucLastDoneDateEdit_TextChanged" Width="150px"
                                    AutoPostBack="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Due">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkDueHeader" runat="server" CommandName="Sort" CommandArgument="FLDDUEDATE">Due</asp:LinkButton>
                                <img id="FLDDUEDATE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipDueDate" runat="server" Text='' TargetControlId="lblDueDate" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDueDateEdit" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'
                                    DatePicker="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Planned">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkPlannedHeader" runat="server" CommandName="Sort" CommandArgument="FLDPLANNEDDATE">Planned</asp:LinkButton>
                                <img id="FLDPLANNEDDATE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlannedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblPlannedDateEdit" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'></telerik:RadLabel>
                                <eluc:Date ID="ucPlannedDateEdit" runat="server" Visible="false" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'
                                    DatePicker="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="From" ColumnGroupName="FromToPort">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromPort" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMPORT").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDFROMPORT").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDFROMPORT").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipFromPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMPORT") %>' TargetControlId="lblFromPort" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFromPortEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMPORT") %>'></telerik:RadLabel>
                                <eluc:SeaPort ID="ucFromPortEdit" runat="server" Visible="false" Width="98%" AppendDataBoundItems="true"
                                    SelectedSeaport='<%# DataBinder.Eval(Container,"DataItem.FLDFROMPORTID") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="To" ColumnGroupName="FromToPort">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblToPort" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPORT").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDTOPORT").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDTOPORT").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipToPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPORT") %>' TargetControlId="lblToPort" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblToPortEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPORT") %>'></telerik:RadLabel>
                                <eluc:SeaPort ID="ucToPortEdit" runat="server" Visible="false" Width="98%" AppendDataBoundItems="true"
                                    SelectedSeaport='<%# DataBinder.Eval(Container,"DataItem.FLDTOPORTID") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" HeaderText="Attending Supt">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInspector" runat="server" Width="110px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTENDINGSUPT").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTENDINGSUPT") %>' TargetControlId="lblInspector" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblInspectorEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTENDINGSUPT") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlInternalInspectorEdit" Width="98%" runat="server"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" HeaderText="External Auditor">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExternalAuditor" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALINSPECTORNAME").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDEXTERNALINSPECTORNAME").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDEXTERNALINSPECTORNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipExternalAuditor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALINSPECTORNAME") %>' TargetControlId="lblExternalAuditor" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblExternalAuditorEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALINSPECTORNAME") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtExternalAuditorEdit" runat="server" Width="98%" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALINSPECTORNAME") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" HeaderText="Organization">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrganisation" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALINSPECTORORGANISATION").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDEXTERNALINSPECTORORGANISATION").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDEXTERNALINSPECTORORGANISATION").ToString()%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipOrganisation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALINSPECTORORGANISATION") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOrganisationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALINSPECTORORGANISATION") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtOrganisationEdit" runat="server" Width="98%" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALINSPECTORORGANISATION") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="60px" HeaderText="Status">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblScheduleStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Plan" CommandName="CREATESCHEDULE" ID="imgCreateSchedule" ToolTip="Plan">
                                     <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Report" CommandName="REPORT" ID="cmdReport" ToolTip="Report">
                                    <span class="icon"><i class="fas fa-clipboard"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="InProgress" CommandName="INPROGRESS" Visible="false" ID="cmdInProgress" ToolTip="InProgress">
                                    <span class="icon"><i class="fas fa-spare-move"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="UNPLAN" CommandName="UNPLAN" Visible="false" ID="imgUnPlan" ToolTip="UnPlan">
                                    <span class="icon"><i class="fas fa-calendar-times"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" Visible="true" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>                                
                                <asp:LinkButton runat="server" AlternateText="AUDITPLAN" CommandName="AUDITPLAN" ID="imgauditplan" Visible="false" ToolTip="Audit Plan">
                                    <span class="icon"><i class="fas fa-Inspector-plan"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
            <asp:Button ID="ucConfirmDelete" runat="server" Text="ConfirmDelete" OnClick="btnConfirmDelete_Click" />
        </telerik:RadAjaxPanel>
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <table>
                        <tr style="background-color: red">
                            <td width="5px" height="10px"></td>
                        </tr>
                    </table>
                </td>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblOverdue" runat="server" Text=" - Overdue"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <table>
                        <tr style="background-color: darkorange">
                            <td width="5px" height="10px"></td>
                        </tr>
                    </table>
                </td>
                <td>
                    <b>
                        <telerik:RadLabel ID="lbl30days" runat="server" Text=" - Due within 30 days"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <table>
                        <tr style="background-color: green">
                            <td width="5px" height="10px"></td>
                        </tr>
                    </table>
                </td>
                <td>
                    <b>
                        <telerik:RadLabel ID="lbl60days" runat="server" Text=" - Due within 60 days"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
