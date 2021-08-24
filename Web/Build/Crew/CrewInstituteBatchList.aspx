<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInstituteBatchList.aspx.cs" Inherits="CrewInstituteBatchList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Width="0" />
            <table cellpadding="1" cellspacing="2" width="100%">               
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListInstitute">
                            <telerik:RadTextBox ID="txtInstituteId" runat="server" Width="0"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" CssClass="readonlytextbox" ReadOnly="true" ></telerik:RadTextBox>
                             <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top" ID="btnShowInstitute" ToolTip="Select Institute">
                                <span class="icon"><i class="fas fa-tasks-picklist"></i></span>
                            </asp:LinkButton>
                        </span>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBatchNo" runat="server" Text="Batch No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtBatchNoSearch" CssClass="input"></telerik:RadTextBox>
                         <telerik:RadTextBox ID="txtcourseName" runat="server" CssClass="input" Width="0" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCourseId" runat="server" Width="0" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="Literal3" runat="server" Text="Duration"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtStartDate" runat="server"  />
                        <telerik:RadLabel ID="lblDash" runat="server" Text="-"></telerik:RadLabel>
                        <eluc:Date ID="txtEndDate" runat="server"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucBatchStatus" runat="server" AppendDataBoundItems="true"
                            CssClass="input" HardTypeCode="152" ShortNameFilter="OPN,CNL" />
                        <asp:HiddenField ID="hdncnlStatus" runat="server" />
                        <asp:HiddenField ID="hdnErcStatus" runat="server" />
                        <asp:HiddenField ID="hdnOpnStatus" runat="server" />
                        <asp:HiddenField ID="hdncmpStatus" runat="server" />
                    </td>
                </tr>
            </table>            
            <%--<telerik:RadLabel ID="lbld" runat="server" ForeColor="Blue" Text="Note : By default batches will show only the open status."></telerik:RadLabel>--%>
            <br />            
            <b>
                <telerik:RadLabel ID="lblTitle" runat="server" Text="Batch List"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="MenuCrewBatchList" runat="server" OnTabStripCommand="MenuCrewBatchList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBatchList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvBatchList_ItemCommand" OnNeedDataSource="gvBatchList_NeedDataSource"
                EnableViewState="false" Height="82%" GroupingEnabled="false" EnableHeaderContextMenu="true" OnItemDataBound="gvBatchList_ItemDataBound">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDCREWINSTITUTEBATCHID">
                    <HeaderStyle Width="102px" />
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
                        <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-Width="30px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRowNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Batch No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBatchNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Institute">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstitute" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="110px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Batch Plan"
                                    CommandName="PLAN" CommandArgument='<%# Container.DataSetIndex %>' ID="imgAddPlan"
                                    ToolTip="Batch Plan" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-calendar-plus"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Enrollment"
                                    CommandName="ENROLL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEnrollment"
                                    ToolTip="Enrollment" Width="20PX" Height="20PX" Visible="false">
                                <span class="icon"><i class="fal fa-file-signature"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel Batch"
                                    CommandName="CANCELBATCH" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancelBatch"
                                    ToolTip="Cancel Batch" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-exclamation"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attendance" Visible="false"
                                    CommandName="ATTEND" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttendance"
                                    ToolTip="Attendance" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fal fa-bars"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Enrollment Complete" Visible="false"
                                    CommandName="ENROLLMENT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEnrollmentCompleted"
                                    ToolTip="Enrollment Complete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-clipboard-check"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>            
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
