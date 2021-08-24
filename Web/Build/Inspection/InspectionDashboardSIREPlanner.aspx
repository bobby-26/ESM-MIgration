<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashboardSIREPlanner.aspx.cs" Inherits="InspectionDashboardSIREPlanner" %>

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
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }

            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvScheduleForCompany").height(browserHeight - 100);
            });
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionScheduleByCompany" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecorationZoneID="tblConfigureAirlines" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="ucTitle" Text="CDI / SIRE Schedule" Visible="false" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"></eluc:TabStrip>
        <table id="tblBudgetGroupAllocationSearch" runat="server" visible="false" width="70%">
            <tr>
                <td>
                    <asp:CheckBox ID="chkShowAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowAll_Changed"
                        Visible="false" />
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuScheduleGroup" runat="server" OnTabStripCommand="BudgetGroup_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvScheduleForCompany" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true" EnableViewState="true"
            Font-Size="11px" Width="100%" CellPadding="3" Height="94%" OnItemCommand="gvScheduleForCompany_ItemCommand" OnNeedDataSource="gvScheduleForCompany_NeedDataSource"
            OnItemDataBound="gvScheduleForCompany_ItemDataBound" ShowFooter="false" ShowHeader="true" OnSortCommand="gvScheduleForCompany_SortCommand"
            AllowSorting="true" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
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
                    <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="imgFlag" runat="server" Visible="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME">Vessel &nbsp;</asp:LinkButton>
                            <img id="FLDVESSELNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkVessel"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'
                                CommandName="SELECT" CommandArgument='<%# Container.DataItem%>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkInspectionNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOMPANYNAME">Company&nbsp;</asp:LinkButton>
                            <img id="FLDCOMPANYNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCompanyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCompanyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblScheduleId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHEDULEID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblScheduleByCompanyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHEDULEBYCOMPANYID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblInspectionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblIsManual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMANUALINSPECTION") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="72px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkInspectionHeader" runat="server" CommandName="Sort" CommandArgument="FLDINSPECTIONSHORTCODE">Type&nbsp;</asp:LinkButton>
                            <img id="FLDINSPECTIONSHORTCODE" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONSHORTCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="3rd Last" HeaderStyle-Width="130px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblthirdInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD3RDINSPECTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="2nd Last" HeaderStyle-Width="130px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblsecondInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD2NDINSPECTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last" HeaderStyle-Width="130px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfirstInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD1STINSPECTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Due" HeaderStyle-Width="150px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkDueHeader" runat="server" CommandName="Sort" CommandArgument="FLDDUEDATE">Due</asp:LinkButton>
                            <img id="FLDDUEDATE" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Date ID="ucDueDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'
                                DatePicker="true" CssClass="input" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblLastDoneHeader" runat="server">Last Done</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastDoneDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblInspectionIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <eluc:Date ID="ucLastDoneDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>'
                                DatePicker="true" CssClass="input" OnTextChangedEvent="ucLastDoneDateEdit_TextChanged"
                                AutoPostBack="true" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblBasisHeader" runat="server">Basis</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBasisDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISDETAILS") %>'
                                CommandName="SHOW" CommandArgument='<%# Container.DataItem%>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblBasisDetails" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISDETAILS") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblBasisId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <span id="spanBasisInspectionSchedule">
                                <asp:TextBox ID="txtCompany" runat="server" CssClass="input" Enabled="false" Width="40px"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISCOMPANYNAME") %>'></asp:TextBox>
                                <asp:TextBox ID="txtBasis" runat="server" CssClass="input" Enabled="false" Width="80px"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISSCHEDULENUMBER") %>'></asp:TextBox>
                                <img id="imgBasis" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                                <asp:TextBox ID="txtBasisScheduleId" runat="server" CssClass="hidden" Width="0px"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISID") %>'></asp:TextBox>
                                <asp:ImageButton runat="server" ID="imgClearBasis" ToolTip="Clear Basis" CommandName="CLEAR"
                                    CommandArgument='<%# Container.DataItem%>' OnClick="ClearBasis" ImageUrl="<%$ PhoenixTheme:images/clear-filter.png %>" />
                            </span>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Planned">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkPlannedHeader" runat="server" CommandName="Sort" CommandArgument="FLDPLANNEDDATE">Planned</asp:LinkButton>
                            <img id="FLDPLANNEDDATE" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPlannedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblPlannedDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'></telerik:RadLabel>
                            <eluc:Date ID="ucPlannedDateEdit" runat="server" Visible="false" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'
                                DatePicker="true" CssClass="input" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblPlannedPortHeader" runat="server">Planned Port</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPlannedPort" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblPlannedPortEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                            <eluc:SeaPort ID="ucSeaportEdit" runat="server" Visible="false" Width="90px" AppendDataBoundItems="true"
                                CssClass="input" SelectedSeaport='<%# DataBinder.Eval(Container,"DataItem.FLDPORTID") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblInspectorHeader" runat="server">Inspector</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspector" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDNAMEOFINSPECTOR").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDNAMEOFINSPECTOR").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblInspectorEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>'></telerik:RadLabel>
                            <asp:TextBox ID="txtInspectorEdit" runat="server" Width="90px" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>'
                                CssClass="input"></asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Active" HeaderStyle-Width="50px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="80px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblScheduleStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULESTATUS") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblStatusId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
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
                            <asp:ImageButton runat="server" AlternateText="NEWSCHEDULE" ImageUrl="<%$ PhoenixTheme:images/new.png %>"
                                CommandName="NEWSCHEDULE" CommandArgument="<%# Container.DataItem %>" Visible="false"
                                ID="imgNewSchedule" ToolTip="New Schedule"></asp:ImageButton>
                            <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:LinkButton runat="server" AlternateText="UNPLAN" CommandName="UNPLAN" Visible="false" ID="imgUnPlan" ToolTip="UnPlan">
                                    <span class="icon"><i class="fas fa-calendar-times"></i></span>
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
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <asp:Button ID="ucConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
    </form>
</body>
</html>

