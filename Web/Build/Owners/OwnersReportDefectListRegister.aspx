<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersReportDefectListRegister.aspx.cs" Inherits="OwnersReportDefectListRegister" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvDefectList.ClientID %>").get_masterTableView();
                masterTable.rebind();
                Resize();
            }

            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvDefectList.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
            function pageLoad() {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDefectWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="status" />
            <table style="width: 100%;" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 10%;">
                        <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Component Number "></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtComponentNumber" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                    <td style="width: 10%;">
                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name "></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtComponentName" runat="server" Width="100%"></telerik:RadTextBox>
                    </td>
                    <td style="width: 10%;">
                        <telerik:RadLabel ID="lblResponsibilitysearch" runat="server" Text="Responsibility"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <eluc:Discipline ID="ucDisciplineResponsibility" runat="server" Width="100%" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" HardTypeCode="258" ID="ddlStatus" Width="100%" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Text="" ID="txtTitle" Width="100%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlType" runat="server" Width="100%">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="Dummy" />
                                <telerik:DropDownListItem Text="Defect" Value="1" />
                                <telerik:DropDownListItem Text="Non-Routine Job" Value="2" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>

            </table>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvDefectList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvDefectList_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="true" EnableHeaderContextMenu="true" OnItemCommand="gvDefectList_ItemCommand" Width="100%"
                ShowFooter="false" ShowHeader="true" OnItemDataBound="gvDefectList_ItemDataBound" OnSortCommand="gvDefectList_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDEFECTJOBID,FLDVESSELID">
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Defect No">
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldefectno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFECTNO")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldefectjobid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFECTJOBID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Component">
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNUMBER") + " - " + DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Defect Details">
                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                            <ItemStyle Width="250px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldetailsofdefect" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Job No">
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobNo" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDJOBNO")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Date of Issue" AllowSorting="false">
                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                            <ItemStyle Width="90px" />
                            <ItemTemplate>
                               <%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due" AllowSorting="true" SortExpression="FLDDUEDATE">
                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                            <ItemStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Responsibility">
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResponsibility" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDRESPONSIBILITY")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        

                        <telerik:GridTemplateColumn HeaderText="WO No">
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkOrderNo" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDWORKGROUPNO")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Reqn No">
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkReqNo" CommandName="REQUISITION" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO")) %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="RA No">
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRANo" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDRANUMBER")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Source">
                            <HeaderStyle HorizontalAlign="Left" Width="120px" />
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIncident" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSOURCENAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Significant Defect">
                            <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            <ItemStyle Width="70px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignificant" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAFFECTNAVIGATION").ToString()=="1"?"Yes":"No") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="DD Job">
                            <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            <ItemStyle Width="70px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDDJob" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDDJOB").ToString()=="1"?"Yes":"No") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="70px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkOrderReq" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERREQUIRED") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" ID="cmdEdit" ToolTip="Edit" Visible="false"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Postpone" ImageUrl="<%$ PhoenixTheme:images/31.png %>"
                                    CommandName="POSTPONE" ID="cmdPostpone" ToolTip="Postpone"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Work order" ImageUrl="<%$ PhoenixTheme:images/Jobs.png %>"
                                    CommandName="VERIFY" ID="cmdverify" ToolTip="Review" Visible="false"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Work order" ImageUrl="<%$ PhoenixTheme:images/Jobs.png %>"
                                    CommandName="WORKORDER" ID="cmdWorkorder" ToolTip="Create Job" Visible="false"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Complete" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                    CommandName="COMPLETE" ID="cmdComplete" ToolTip="Complete" Visible="false"></asp:ImageButton>
                                <asp:LinkButton runat="server" AlternateText="Communication"
                                    CommandName="COMMUNICATION" ID="lnkCommunication" ToolTip="Comments">
                                <span class="icon"><i class="fas fa-postcomment"></i></span>
                                </asp:LinkButton>
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" ID="cmdDelete" ToolTip="Delete"></asp:ImageButton>
                                <asp:ImageButton runat="server" ID="cmdAtt" ToolTip="Attachment" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>

        <telerik:RadWindow ID="modalPopup" runat="server" Width="700px" Height="450px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
            VisibleStatusbar="false" KeepInScreenBounds="true" NavigateUrl="PlannedMaintenanceDefectListAdd.aspx" ReloadOnShow="true" >
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
