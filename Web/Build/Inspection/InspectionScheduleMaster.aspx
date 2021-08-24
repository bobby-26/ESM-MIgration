<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionScheduleMaster.aspx.cs"
    Inherits="InspectionScheduleMaster" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITopic" Src="~/UserControls/UserControlInspectionTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Checklist" Src="~/UserControls/UserControlInspectionChecklist.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvInspectionSchedule.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionScheduleMaster" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:Title runat="server" ID="frmTitle" Text="Vetting Log" Visible="false"></eluc:Title>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuInspectionScheduleSearch" runat="server" OnTabStripCommand="InspectionScheduleSearch_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvInspectionSchedule" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true"
            Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvInspectionSchedule_ItemCommand" OnItemDataBound="gvInspectionSchedule_ItemDataBound"
            OnSortCommand="gvInspectionSchedule_SortCommand" OnNeedDataSource="gvInspectionSchedule_NeedDataSource" AllowSorting="true" GridLines="None" ShowFooter="false" GroupingEnabled="false"
            EnableHeaderContextMenu="true" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDINSPECTIONSCHEDULEID">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <ColumnGroups>
                    <telerik:GridColumnGroup Name="DeficiencyCount" HeaderText="Def Count" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
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
                    <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="96px" AllowSorting="true" SortExpression="FLDVESSELNAME" ShowSortIcon="true">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkVesselCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"></asp:LinkButton>
                            <img id="FLDVESSELNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblDTkey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--<telerik:GridTemplateColumn>
                                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                            <HeaderTemplate>
                                                                M/C
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblManual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMANUAL") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>--%>
                    <telerik:GridTemplateColumn HeaderText="Ref.No." HeaderStyle-Width="100px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkInspectionRefNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDREFERENCENUMBER">Ref.No.&nbsp;</asp:LinkButton>
                            <img id="FLDREFERENCENUMBER" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspectionRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--<telerik:GridTemplateColumn HeaderText="Inspection Type">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="lnkInspectionTypeHeader" runat="server" CommandName="Sort" CommandArgument="FLDINSPECTIONTYPENAME"
                                                                    ForeColor="White">Type &nbsp;</asp:LinkButton>
                                                                <img id="FLDINSPECTIONTYPENAME" runat="server" visible="false" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <telerik:RadLabel ID="lblInspectionTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONTYPENAME") %>'></telerik:RadLabel>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>--%>
                    <telerik:GridTemplateColumn HeaderText="Inspection Category" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkInspectionCategoryHeader" runat="server" CommandName="Sort"
                                CommandArgument="FLDINSPECTIONCATEGORY">Category &nbsp;</asp:LinkButton>
                            <img id="FLDINSPECTIONCATEGORY" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspectionCategoryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCATEGORY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="70px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspectionCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCOMPANYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vetting" HeaderStyle-Width="55px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkInspectionHeader" runat="server" CommandName="Sort" CommandArgument="FLDINSPECTIONNAME">Vetting&nbsp;</asp:LinkButton>
                            <img id="FLDINSPECTIONNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspectionScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONSCHEDULEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblInspectionDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkInspection" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItem %>"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="I/S" HeaderStyle-Width="33px">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspectionScreening" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONSCREENING") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Basis" HeaderStyle-Width="110px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBasisId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkBasis" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItem %>"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASISREFERENCENUMBER") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Done" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDVETTINGCOMPLETIONDATE" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkLastDoneDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDVETTINGCOMPLETIONDATE"></asp:LinkButton>
                            <img id="FLDVETTINGCOMPLETIONDATE" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastDoneDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Port" HeaderStyle-Width="102px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Inspector">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Attending Supt" HeaderStyle-Width="77px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAttendingSupt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTENDINGSUPT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="78px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStatus" runat="server" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME").ToString().Substring(0, 15)+ "..." : DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME").ToString()  %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucToolTipStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>' TargetControlId="lblStatus" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Total" ColumnGroupName="DeficiencyCount">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDeficiencyCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYCOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="OBS" HeaderStyle-Width="40px" ColumnGroupName="DeficiencyCount">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOBSCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBSCOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="40px" HeaderText="HR OBS" ColumnGroupName="DeficiencyCount">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblHROBSCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHIRISKOBSCOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="60px">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt"
                                ToolTip="Attachment">
                                      <span class="icon"><i class="fas fa-paperclip"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Summary" CommandName="DEFICIENCYSUMMARY"
                                ID="cmdDeficiencySummary" ToolTip="View Deficiency Summary">
                                     <span class="icon"><i class="fas fa-eye"></i></span>
                            </asp:LinkButton>
                            <%--<asp:LinkButton runat="server" AlternateText="Communication"
                                CommandName="COMMUNICATION" ID="lnkCommunication" ToolTip="Communication">
                                <span class="icon"><i class="fas fa-postcomment"></i></span>
                            </asp:LinkButton>--%>
                        </ItemTemplate>
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
        </telerik:RadAjaxPanel>
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblInspection" runat="server" Text="* I - Inspection"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblScreening" runat="server" Text="* S - Screening"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                     <table>
                         <tr style="background-color:red">
                             <td width="5px" height="10px"></td>
                         </tr>
                     </table>                     
                </td>
                <td>
                    <b>
                    <telerik:RadLabel ID="lblOverdue" runat="server" Text=" - Overdue for Review"></telerik:RadLabel></b>
                </td>
                <td>
                     <table>
                         <tr style="background-color:darkorange">
                             <td width="5px" height="10px"></td>
                         </tr>
                     </table>                     
                </td>
                <td>
                    <b><telerik:RadLabel ID="RadLabel1" runat="server" Text=" - Overdue for Closure"></telerik:RadLabel></b>
                </td>
            </tr>
        </table>
        <eluc:Confirm ID="ucConfirmComplete" runat="server" OnConfirmMesage="btnComplete_Click"
            OKText="Yes" CancelText="No" />
    </form>
</body>
</html>
