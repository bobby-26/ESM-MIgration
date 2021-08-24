<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionLongTermActionOfficeList.aspx.cs"
    Inherits="InspectionLongTermActionOfficeList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Long Term Action List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvShipBoardTasks").height(browserHeight - 150);
            });
        </script>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvLongTermAction.ClientID %>"));
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
    <form id="frmRegistersBudgetBillingList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Title runat="server" ID="Title1" Text="Office Task List" Visible="false" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <div id="divInspectionType" style="position: relative; z-index: 2" runat="server"
                visible="false">
                <table width="100%">
                    <tr>
                        <td>
                            <font color="blue" size="0"><b>
                                <asp:Literal ID="lblTaskStatusChange" runat="server" Text="Task Status Change:"></asp:Literal></b>
                                <li>Open >> Accepted >> Completed.</li>
                                <li>Open >> Accepted >> Cancelled.</li>
                                <li>Cancelled >> Open >> Accepted >> Completed.</li>
                            </font>
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:TabStrip ID="MenuLongTermAction" runat="server" OnTabStripCommand="MenuLongTermAction_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLongTermAction" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                OnSorting="gvLongTermAction_Sorting" Width="100%" CellPadding="3" OnItemCommand="gvLongTermAction_ItemCommand" OnItemDataBound="gvLongTermAction_ItemDataBound"
                OnRowCancelingEdit="gvLongTermAction_RowCancelingEdit" OnNeedDataSource="gvLongTermAction_NeedDataSource"
                AllowSorting="true" ShowFooter="false" GroupingEnabled="false" EnableHeaderContextMenu="true"
                ShowHeader="true" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDINSPECTIONPREVENTIVEACTIONID">
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="35px">
                            <HeaderTemplate>
                                <telerik:RadCheckBox ID="chkAllRemittance" runat="server" Text="" AutoPostBack="true"
                                    OnPreRender="CheckAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues" AutoPostBack="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="100px" AllowSorting="true" SortExpression="FLDVESSELNAME" ShowSortIcon="true">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME">Vessel</asp:LinkButton>
                                <img id="FLDVESSELNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSourceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNONCONFORMITYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSourceType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCETYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVessel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Source" HeaderStyle-Width="145px" AllowSorting="true" SortExpression="FLDCREATEDFROMNAME" ShowSortIcon="true">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROM") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkTaskSource" runat="server" CommandName="SHOWSOURCE" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROMNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblCreatedFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROMNAME") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="45px" AllowSorting="true" SortExpression="FLDTYPE" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Task" HeaderStyle-Width="145px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="250"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLongTermActionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPREVENTIVEACTIONID") %>'></telerik:RadLabel>
                                <asp:CheckBox ID="chkmanualtask" runat="server" Visible="false" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDMANUALGENERATETASKSYN").ToString().Equals("1")?true:false %>' />
                                <asp:LinkButton ID="lnkTask" runat="server" CommandName="TASK" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION").ToString() %>'></asp:LinkButton>
                                <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION")%>' TargetControlId="lnkTask" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="160px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkCategoryHeader" runat="server" CommandName="Sort" CommandArgument="FLDCATEGORY">Category</asp:LinkButton>
                                <img id="FLDCATEGORY" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Width="100" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKCATEGORYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCategoryShortCode" Visible="false" runat="server" Width="100" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKCATEGORYSHORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Category">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="120"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" Width="120" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkSubCategory" Width="120" runat="server" Visible="false" CommandName="FILEEDIT" CommandArgument='<%# Container.DataItem %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKSUBCATEGORYNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblsectionno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSECTIONNUMBER")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblsectionid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSECTIONID")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOCUMENTID")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblformid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMID")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblformno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMNO")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWREVISIONNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="86px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Department" HeaderStyle-Width="95px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkDepartmentHeader" runat="server" CommandName="Sort" CommandArgument="FLDDEPARTMENTNAME">Department</asp:LinkButton>
                                <img id="FLDDEPARTMENTNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAssignedDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Department" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBDEPARTMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Accepted by" HeaderStyle-Width="100px" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAcceptedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCEPTEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Acceptance Status" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAcceptedYNName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCEPTEDSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAcceptedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCEPTEDBY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDTARGETDATE" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTargetDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed" HeaderStyle-Width="78px" AllowSorting="true" SortExpression="FLDCOMPLETIONDATE" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Order Number" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblWorkOrderNumberHeader" runat="server"> Work <br /> Order Number</asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkWorkOrderNumber" runat="server" CommandName="SHOWWORKORDER"
                                    CommandArgument='<%# Container.DataItem %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDWONUMBER") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblWorkOrderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWONUMBER") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="75px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="2%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Show Pending Tasks in Vessels" CommandName="SHOWSTATUS" ID="cmdShowStatus"
                                    ToolTip="Show Pending Tasks in Vessels">
                                    <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt"
                                    ToolTip="Upload Evidence" Visible="false">
                                     <span class="icon"><i class="fas fa-paperclip"></i></span>
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
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                     <table>
                         <tr style="background-color:red">
                             <td width="5px" height="10px"></td>
                         </tr>
                     </table>                     
                </td>
                <td>
                    <b>
                    <telerik:RadLabel ID="lblOverdue" runat="server" Text=" - Overdue"></telerik:RadLabel></b>
                </td>
                <td>
                     <table>
                         <tr style="background-color:darkviolet">
                             <td width="5px" height="10px"></td>
                         </tr>
                     </table>                     
                </td>
                <td>
                    <b><telerik:RadLabel ID="lblPostponed" runat="server" Text=" - Postponed"></telerik:RadLabel></b>
                </td>
            </tr>
        </table>
               </div>
        <triggers>
                <asp:PostBackTrigger ControlID="gvLongTermAction" />
            </triggers>
    </form>
</body>
</html>
