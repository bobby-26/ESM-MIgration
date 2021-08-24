<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHWorkCalender.aspx.cs"
    Inherits="VesselAccountsRHWorkCalender" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work and Rest Hour Records</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function ConfirmReconcile(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmReconcile.UniqueID %>", "");
                }
            }
            function ConfirmVerify(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmWorkHours.UniqueID %>", "");
                }
            }
            function ConfirmUnlock(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmUnlock.UniqueID %>", "");
                }
            }
            function DashboardValidation() {
               
                    document.getElementById("cmddashboardactivityvalidation").click();
               
            }
        </script>
        <script type="text/javascript">

            function pageLoad() {
                PaneResized();
            }

            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvWorkCalender");
                grid._gridDataDiv.style.height = (browserHeight - 390) + "px";
            }
        </script>
        <%--<style>
            .rbToggleCheckbox {
                float: right !important;
                }
            .rbToggleCheckboxChecked {
                float: right !important;
            }
            /*.rbVerticalList button {
                text-aligh: right;
            }*/
        </style>--%>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRestHourWOrkCalender" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmddashboardactivityvalidation" runat="server" Text="cmddashboardactivityvalidation" OnClick="cmddashboardactivityvalidation_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuRHGeneral" runat="server" Visible="false" OnTabStripCommand="RHGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuRHUnlock" runat="server" OnTabStripCommand="MenuRHUnlock_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="Label4" Font-Bold="true" runat="server" EnableViewState="false" Text="Notes :" BorderStyle="None" ForeColor="Blue"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbl1" runat="server" Text="1." ForeColor="Blue"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblToAddaReportingDayClickonthe" runat="server" Text="Reporting Day Will be a dded automatically with Default Work hours configured." ForeColor="Blue"></telerik:RadLabel>
                                    <%--<i class="fa fa-plus-circle"></i>
                                    <telerik:RadLabel ID="lblbuttonplacedontheScrollList" runat="server" Text="' button, placed on the Scroll List." ForeColor="Blue"></telerik:RadLabel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbl2" runat="server" Text="2." ForeColor="Blue"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTorecordWorkHourClick" runat="server" ForeColor="Blue" Text="To verify Work Hour Click '"></telerik:RadLabel>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/completed.png %>" />
                                    <telerik:RadLabel ID="lblandrecordworkhours" runat="server" ForeColor="Blue" Text="' and verify work hours."></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbl3" runat="server" Text="3." ForeColor="Blue"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTorecordNatureofWorkClick" ForeColor="Blue" runat="server" Text="Click '"></telerik:RadLabel>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/red.png %>" />
                                    <telerik:RadLabel ID="lblthenselectNatureofWorkandsave" ForeColor="Blue" runat="server" Text="', Review the “Reason for NC”, identify the “System Causes” and “Corrective actions” and save."></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="4." ForeColor="Blue"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel2" runat="server" ForeColor="Blue" Text="Click on the '"></telerik:RadLabel>
                                    <i class="fas fa-calendar-alt"></i>
                                    <telerik:RadLabel ID="RadLabel3" runat="server" ForeColor="Blue" Text="' button, to reconcile the crew work hours."></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
            <table style ="width:100%;" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmpName" runat="server" CssClass="input" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStartDate" runat="server" Text="Start Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtStartDate" runat="server" CssClass="input" Enabled="false" Width="90px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" AutoPostBack="true" Filter="Contains" Width="250px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input"
                            Enabled="false" Width="250px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNoofDays" runat="server" Text="No.of Days"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtNOdays" CssClass="input" Enabled="false" runat="server" Text="0" Width="90px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadlblUnlockStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="RadtxtUnlockStatus" runat="server" Enabled="false" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                 </tr>
                </table>
            <table style="width:100%">
                <tr>

                    <td style="width:50%">
                        <telerik:RadCheckBoxList ID="rdLevel" runat="server" RenderMode="Lightweight" Direction="Vertical" Width="90%" >
                            <DataBindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" />
                        </telerik:RadCheckBoxList>
                    </td>
                    <td style="width:50%">
                        <telerik:RadTextBox ID="txtRemarks" runat="server" RenderMode="Lightweight" TextMode="MultiLine" Rows="3" EmptyMessage="Enter Remarks" Width="90%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuWorkHour" runat="server" OnTabStripCommand="MenuWorkHour_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvWorkCalender" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None" Width="100%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvWorkCalender_ItemCommand"
                EnableViewState="false" OnItemDataBound="gvWorkCalender_ItemDataBound" OnNeedDataSource="gvWorkCalender_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" Width="1%" />
                            <HeaderTemplate>
                                <img id="ImgFlagHeader" runat="server" alt="" src="<%$ PhoenixTheme:images/Red.png %>"
                                    title="Non Compliance" />
                                <img id="ImgFlagNWHeader" runat="server" visible="false" alt="" src="<%$ PhoenixTheme:images/green.png %>"
                                    title="Nature of Work" />
                                &nbsp;
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="1%"></ItemStyle>
                            <ItemTemplate>
                                <%--<img id="ImgFlag" runat="server" alt="" src="<%$ PhoenixTheme:images/Red.png %>"
                                    visible="false" />--%>
                                <asp:ImageButton runat="server" ID="ImgFlag" ImageUrl="<%$ PhoenixTheme:images/Red.png %>" Visible="false" />
                                <eluc:ToolTip ID="ucToolTipNC" runat="server" />
                                <%--<img id="ImgFlagNW" runat="server" alt="" src="<%$ PhoenixTheme:images/green.png %>"
                                    visible="false" />--%>
                                <asp:ImageButton runat="server" ID="ImgFlagNW" ImageUrl="<%$ PhoenixTheme:images/green.png %>" Visible="false" />
                                <eluc:ToolTip ID="ucToolTipNW" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reasons for NC">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReasons" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREASONTEXT"]%>'></telerik:RadLabel>
                                <%--<eluc:ToolTip ID="ucToolTipReason" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREASONTEXT"]%>' />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="System Causes">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSystemcauses" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSYSTEMCAUSESTEXT"]%>'></telerik:RadLabel>
                                <%--<eluc:ToolTip ID="ucToolTipSystemcauses" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSYSTEMCAUSESTEXT"]%>' />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Corrective Action">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCORRECTIVEACTIONTEXT"]%>'></telerik:RadLabel>
                                <%--<eluc:ToolTip ID="ucToolTipCorrectiveAction" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCORRECTIVEACTIONTEXT"]%>' />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nature of Work">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNatureOfWork" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNATUREOFWORKTEXT"]%>'></telerik:RadLabel>
                                <%--<eluc:ToolTip ID="ucToolTipNatureOfWork" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNATUREOFWORKTEXT"]%>' />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Day">
                            <HeaderStyle HorizontalAlign="Right" Width="2%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpendingactivity" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISACTIVITYPENDING") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCalenderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOURCALENDARID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblShipCalenderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCALENDARID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMonthId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReportingDay" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTINGDAY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <HeaderStyle HorizontalAlign="Left" Width="3%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sea / Port">
                            <HeaderStyle HorizontalAlign="Left" Width="2%" />
                            <ItemStyle HorizontalAlign="Left" Width="2%"/>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeaPort" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLACENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hours">
                            <HeaderStyle HorizontalAlign="Right" Width="2%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHours" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOURS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="IDL">
                            <HeaderStyle HorizontalAlign="Left" Width="2%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRetardAdvance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOCKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Adv/Ret">
                            <HeaderStyle HorizontalAlign="Left" Width="2%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClock" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCERETARDNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAdvanceRetardHour" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCERETARDHOUR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work">
                            <HeaderStyle HorizontalAlign="Right" Width="2%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalHour" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALHOURS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rest">
                            <HeaderStyle HorizontalAlign="Right" Width="2%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRestHour" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOURS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No.of NC" Visible="false">
                            <HeaderStyle HorizontalAlign="Right" Width="1%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblnoofNonCompliance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFCOMPLIANCES") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="2%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Verify Work Hours" ID="cmdWorkingHourAdd" ImageUrl="<%$ PhoenixTheme:images/completed.png %>"
                                    ToolTip="Verify Work Hours" CommandName="Add" Visible="false"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="ucConfirmReconcile" runat="server" OnClick="ucConfirmReconcile_Click" CssClass="hidden" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:Button ID="ucConfirmWorkHours" runat="server" Text="cmdConfirmWorkHours" OnClick="ucConfirmWorkHours_Click" CssClass="hidden" />
            <asp:Button ID="ucConfirmUnlock" runat="server" Text="cmdConfirmUnlock" OnClick="ucConfirmUnlock_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
