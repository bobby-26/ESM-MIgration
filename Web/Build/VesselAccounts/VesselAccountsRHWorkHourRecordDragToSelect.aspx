<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHWorkHourRecordDragToSelect.aspx.cs" Inherits="VesselAccountsRHWorkHourRecordDragToSelect" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkSheet" Src="~/UserControls/UserControlRHTimeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function ConfirmReconcile(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmReconcile.UniqueID %>", "");
                }
            }
        </script>

        <style>
            .RadRadioButtonList.rbHorizontalList .RadRadioButton {
                /*min-width: 70px;*/
                text-align: left;
            }

            .legend {
                width: 10px;
                height: 10px;
                display: inline-block;
                border: 1px solid black;
                border-radius: 3px;
            }

            .legend-container {
                display: inline;
                padding-right: 2px;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRestHourWOrkCalender" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">--%>
            <asp:Button ID="ucConfirmReconcile" runat="server" OnClick="ucConfirmReconcile_Click" CssClass="hidden" />
            <telerik:RadAjaxManager runat="server" ID="RadAjaxManager" UpdateInitiatorPanelsOnly="false">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="MenuWorkHour">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="MenuWorkHour" />
                            <telerik:AjaxUpdatedControl ControlID="gvAttandence" UpdatePanelHeight="80px" />
                            <telerik:AjaxUpdatedControl ControlID="gvWorkHourRecord" />
                            <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                            <telerik:AjaxUpdatedControl ControlID="ucError" />
                            <telerik:AjaxUpdatedControl ControlID="gvMember" />
                            <telerik:AjaxUpdatedControl ControlID="lnkReasonNC" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="gvAttandence">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="MenuWorkHour" />
                            <telerik:AjaxUpdatedControl ControlID="gvAttandence" UpdatePanelHeight="80px" />
                            <telerik:AjaxUpdatedControl ControlID="gvWorkHourRecord" />
                            <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                            <telerik:AjaxUpdatedControl ControlID="ucError" />
                            <telerik:AjaxUpdatedControl ControlID="gvMember" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="gvMember">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="MenuWorkHour" />
                            <telerik:AjaxUpdatedControl ControlID="gvAttandence" UpdatePanelHeight="80px" />
                            <telerik:AjaxUpdatedControl ControlID="gvWorkHourRecord" />
                            <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                            <telerik:AjaxUpdatedControl ControlID="ucError" />
                            <telerik:AjaxUpdatedControl ControlID="gvMember" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="MenuWorkHour" runat="server" OnTabStripCommand="MenuWorkHour_TabStripCommand" Title="Rest Work Hour"></eluc:TabStrip>
            <table runat="server" width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td width="70%">
                        <telerik:RadLabel ID="lblNotes" Font-Bold="true" runat="server" Text="Notes:"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl1Clickon"
                            runat="server" ForeColor="Blue" Text="1. Work Hours are recorded automatically based on the watch and non-watch hours set for the seafarer and any Operation or Maintenance activity he was involved in.">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl2Theapportioned" ForeColor="Blue"
                            runat="server" Text="2. Seafarer is required to confirm that his work hours are correctly recorded and identify the “Reason for NC” if applicable, from the multiple check box.">
                        </telerik:RadLabel>
                    </td>
                    <td width="15%">
                        <asp:LinkButton ID="lnkReasonNC" Font-Underline="true"
                            runat="server" Text="Reason for NC" ForeColor="Blue" Font-Bold="true">
                        </asp:LinkButton>
                    </td>
                    <td width="15%">
                        <asp:LinkButton ID="lnkNatureofwork" Font-Underline="true"
                            runat="server" Text="Nature of Work" ForeColor="Blue" Font-Bold="true">
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl3Thetotalworkhoursandresthoursareshownattheendofthetabstrip"
                            runat="server" Text="3. Seafarer can only edit or confirm his work hours after the day is over."
                            ForeColor="Blue">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl4"
                            runat="server" Text="4. If the recorded hours are correct, click “Confirm”. If the recorded hours are in-correct, click “Edit”."
                            ForeColor="Blue">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl5"
                            runat="server" Text="5. Click on the time strip hours strip to amend the work hours recorded. Click once to record 1.0 hour, click again to change it to 0.5. The third click will reset the cell to 0.0.<br/> To save the changes click “Save”."
                            ForeColor="Blue">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl6"
                            runat="server" Text="6. “Remarks” are mandatory if any changes are made and “Reason for NC” is mandatory in case of an NC."
                            ForeColor="Blue">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl7"
                            runat="server" Text="7. If the recorded hours are correct, click “Confirm”."
                            ForeColor="Blue">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl8"
                            runat="server" Text="8. The apportioned time will be displayed, if the clock is advanced or retarded during the hour."
                            ForeColor="Blue">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbl9"
                            runat="server" Text="9. The total work hours and rest hours are shown at the end of the tab strip."
                            ForeColor="Blue" Font-Bold="true">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmpName" runat="server" Enabled="false" Width="210px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Enabled="false" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHour" runat="server" Text="Hour"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtHour" runat="server" CssClass="input txtNumber" MaxLength="2"
                            Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportingDay" runat="server" Text="Reporting Day"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtReportingday" runat="server" CssClass="input txtNumber" Enabled="false" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadRadioButtonList ID="rbtnadvanceretard" runat="server" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="IDL W-E" Value="1" />
                                <telerik:ButtonListItem Text="IDL E-W" Value="2" />
                                <telerik:ButtonListItem Text="None" Value="0" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rbtnworkplace" runat="server" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Work at Port" Value="1" />
                                <telerik:ButtonListItem Text="Work at Sea" Value="2" />
                                <telerik:ButtonListItem Text="Work at Sea/Port" Value="3" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadRadioButtonList ID="rbnhourchange" runat="server" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Advance" Value="1" />
                                <telerik:ButtonListItem Text="Retard" Value="2" />
                                <telerik:ButtonListItem Text="Reset" Value="0" Selected="True" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rbnhourvalue" runat="server" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="0.5 Hour" Value="1" />
                                <telerik:ButtonListItem Text="1.0 Hour" Value="2" />
                                <telerik:ButtonListItem Text="2.0 Hour" Value="3" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        <telerik:RadTextBox runat="server" ID="txtRemarks" CssClass="input_mandatory" TextMode="MultiLine"
                            Height="80px" Width="60%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        <telerik:RadGrid ID="gvAttandence" runat="server" RenderMode="Lightweight" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" Height="80px" CellSpacing="0" GridLines="None"
            OnNeedDataSource="gvAttandence_NeedDataSource" OnItemDataBound="gvAttandence_ItemDataBound" OnItemCommand="gvAttandence_ItemCommand">
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDREPORTINGDAY,FLDRESTHOURSTARTID,FLDRESTHOURCALENDARID,FLDVESSELID" CommandItemDisplay="None">
                <Columns>
                </Columns>
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" CellSelectionMode="MultiCell" UseClientSelectColumnOnly="true" />
                <%--<ClientEvents OnCellSelected="cellSelected" />--%>
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <table style="width: 100%">
            <tr>
                <td style="text-align: center">
                    <div class="legend-container">
                        <span class="legend" style="background-color: yellow"></span>
                    </div>
                    <asp:LinkButton ID="lnk05" runat="server" Enabled="false" Text="0.5 Hrs" OnClick="lnk05_Click"></asp:LinkButton>
                </td>
                <td style="text-align: center">
                    <div class="legend-container">
                        <span class="legend" style="background-color: gray"></span>
                    </div>
                    <asp:LinkButton ID="lnk10" runat="server" Enabled="false" Text="1.0 Hrs" OnClick="lnk10_Click"></asp:LinkButton>
                </td>
                <td style="text-align: center">
                    <div class="legend-container">
                        <span class="legend"></span>
                    </div>
                    <asp:LinkButton ID="lnk00" runat="server" Enabled="false" Text="0.0 Hrs" OnClick="lnk00_Click"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <telerik:RadGrid ID="gvWorkHourRecord" runat="server" Height="300px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false"
            Width="100%" CellPadding="3" EnableViewState="true" OnItemDataBound="gvWorkHourRecord_ItemDataBound" OnNeedDataSource="gvWorkHourRecord_NeedDataSource">
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
                    <telerik:GridTemplateColumn HeaderText="Hour">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblHour" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTINGHOUR") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Non Compliance">
                        <HeaderStyle HorizontalAlign="Left" Width="70%" />
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblException" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONCOMPLIANCE") %>'
                                ForeColor="Red">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Legends">
                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="imgS1" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                Width="16" Height="16" />
                            <eluc:ToolTip ID="ucToolTipnc1" runat="server" />
                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                height="16" />
                            <asp:Image ID="imgS2" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                Width="16" Height="16" />
                            <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                height="16" />
                            <eluc:ToolTip ID="ucToolTipnc2" runat="server" />
                            <asp:Image ID="imgS3" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                Width="16" Height="16" />
                            <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                height="16" />
                            <eluc:ToolTip ID="ucToolTipnc3" runat="server" />
                            <asp:Image ID="imgS4" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                Width="16" Height="16" />
                            <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                height="16" />
                            <eluc:ToolTip ID="ucToolTipnc4" runat="server" />
                            <asp:Image ID="imgS5" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                Width="16" Height="16" />
                            <img id="Img5" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                height="16" />
                            <eluc:ToolTip ID="ucToolTipnc5" runat="server" />
                            <asp:Image ID="imgO1" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                Width="16" Height="16" />
                            <img id="Img6" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="16"
                                height="16" />
                            <eluc:ToolTip ID="ucToolTipnc6" runat="server" />
                            <asp:Image ID="imgO2" runat="server" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                Width="16" Height="16" />
                            <eluc:ToolTip ID="ucToolTipnc7" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvMember" AutoGenerateColumns="false" EnableHeaderContextMenu="true"
            AllowPaging="false" AllowCustomPaging="false" OnNeedDataSource="gvMember_NeedDataSource"
            OnItemDataBound="gvMember_ItemDataBound">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDMEMBERID" AutoGenerateColumns="false"
                TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="false">
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="20px">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />

                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false">
                                     <span class="icon" id="imgFlagcolor"  ><i class="fa-exclamation-orange"></i></span>      
                            </asp:LinkButton>

                        </ItemTemplate>

                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Activity/Maintainance">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblact" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ACTITIVITYNAME")%>'>
                            </telerik:RadLabel>
                            
                            <telerik:RadLabel ID="lblmemberid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMEMBERID")%>' Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblcatid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDUTYCATEGORYID")%>' Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblactivityid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIVITYID")%>' Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lbluseredited" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUSEREDITED")%>' Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lbltype" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTYPE")%>' Visible="false">
                            </telerik:RadLabel>
                        </ItemTemplate>

                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Start">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSTARTTIME"), DateDisplayOption.DateTimeHR24)%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>

                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="End">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" />

                        <ItemTemplate>
                           <telerik:RadLabel ID="RadLabel2" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDTIME"), DateDisplayOption.DateTimeHR24)%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>

                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Schedule Catergory">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcat" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CATNAME")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>

                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">

                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Timesheet" ID="cmdTimesheet" CommandName="TIME" ToolTip="Timesheet" Visible="false">
                                            <span class="icon"><i class="far fa-calendar-alt"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>

                    </telerik:GridTemplateColumn>
                </Columns>
              
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="150px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>

        </telerik:RadGrid>
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <span class="icon"><i class="fa-exclamation-orange"></i></span>
                </td>
                <td>
                    <telerik:RadLabel ID="lblinstruction" runat="server" Text="* This record timings are edited by user"></telerik:RadLabel>
                </td>

            </tr>
        </table>
        <%--</telerik:RadAjaxPanel>--%>
    </form>
