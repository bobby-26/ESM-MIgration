<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBatchEnrollment.aspx.cs" Inherits="CrewBatchEnrollment" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlInstituteBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Enrollment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuTitle" runat="server" OnTabStripCommand="MenuTitle_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table id="tblBatchEnrollment" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCourse" CssClass="readonlytextbox" ReadOnly="true" Width="300px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtcourseInstitute" runat="server" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtcourseId" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtInstitute" CssClass="readonlytextbox" ReadOnly="true" Width="300px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtInstituteId" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblBatchNo" runat="server" Text="Batch No"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Batch ID="ddlbatch" runat="server" CssClass="input" AppendDataBoundItems="true" OnTextChangedEvent="ddlbatch_TextChangedEvent"
                            AutoPostBack="true" />
                    </td>
                    <td runat="server" visible="false">

                        <telerik:RadTextBox runat="server" ID="txtBatchNo" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBatchStatus" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStartDate" runat="server" Text="Duration" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtStartDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false" />
                        <telerik:RadLabel ID="lblDash" runat="server" Text="-" Visible="false"></telerik:RadLabel>
                        <eluc:Date ID="txtEndDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false" />
                    </td>
                </tr>
            </table>
            <br />
            <b>
                <telerik:RadLabel ID="lblParticipantList" runat="server" Text="Participant List"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="MenuBatchEnrollment" runat="server" OnTabStripCommand="MenuBatchEnrollment_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvEnrollmentList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvEnrollmentList_ItemCommand" OnNeedDataSource="gvEnrollmentList_NeedDataSource" OnItemDataBound="gvEnrollmentList_ItemDataBound"
                EnableViewState="true" Height="75%" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDCREWBATCHENROLLMENTID">
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
                        <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRowNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="160px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CommandName="EMPCOURSE"></asp:LinkButton>
                                <telerik:RadLabel ID="lblEnrollmentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWBATCHENROLLMENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblReferencedtkey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmpId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAvailableFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatusId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusShort" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSSHORT")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENROLLEDSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="110px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAttachment"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Discontinue"
                                    CommandName="DISCONTINUE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCanel"
                                    ToolTip="Discontinue" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-ban"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Complete" Visible="false"
                                    CommandName="COMPLETED" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdComplete"
                                    ToolTip="Complete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="far fa-check-circle-vi"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="InComplete" Visible="false"
                                    CommandName="INCOMPLETED" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdInComplete"
                                    ToolTip="InComplete">
                                <span class="icon"><i class="fas fa-file-ui"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Move to Personal Record" Visible="false"
                                    CommandName="EMPCOURSE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdMovePersonal"
                                    ToolTip="Move to Personal Record" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-arrow-alt-circle-right"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Remove from Personal Record" Visible="false"
                                    CommandName="REEMPCOURSE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdReMovePersonal"
                                    ToolTip="Remove from Personal Record" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-minus-square"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Plan" Visible="false"
                                    CommandName="PLAN" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdPlan"
                                    ToolTip="Plan" Width="20PX" Height="20PX">
                                <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
