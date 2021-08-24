<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBatchAttendance.aspx.cs" Inherits="CrewBatchAttendance" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlInstituteBatchList.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Attendance</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuBatchAttendance" runat="server" OnTabStripCommand="MenuBatchAttendance_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCourse" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="300px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtcourseId" runat="server" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtcourseInstitute" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInstituteMappingId" runat="server" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtInstitute" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="300px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBatchNo" runat="server" Text="Batch No"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Batch ID="ddlbatch" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" OnTextChangedEvent="ddlbatch_TextChangedEvent"
                            AutoPostBack="true" />
                    </td>
                    <td runat="server" visible="false">
                        <telerik:RadTextBox ID="txtBatchNo" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true" Width="150px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtbatchId" runat="server" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBatchStatus" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblAttendeanceMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="dropdown_mandatory"
                            AutoPostBack="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAttendeanceYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAllPresent" runat="server" Visible="false"></telerik:RadLabel>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBatchStartDate" runat="server" Text="Duration" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtBatchStartDate" runat="server" CssClass="readonlytextbox" Visible="false" />
                        <telerik:RadLabel ID="lblDash1" runat="server" Text="-" Visible="false"></telerik:RadLabel>
                        <eluc:Date ID="txtBatchEndDate" runat="server" CssClass="readonlytextbox" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAllPresent" runat="server" OnCheckedChanged="chkAllPresent_CheckedChanged"
                            AutoPostBack="true" Visible="false" />
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />

            <br />
            <div id="divGuidance" runat="server">
                <table style="color: Blue">
                    <tr>
                        <td>
                            <asp:Literal ID="lblToviewtheGuidelinesplacethemouseonthe" runat="server" Text="To view the Guidelines, place the mouse on the"></asp:Literal>
                        </td>
                        <td>
                            <telerik:RadToolTip RenderMode="Lightweight" ID="btnTooltipHelp" runat="server" Height="100%" TargetControlID="btnHelp" HideDelay="10000">
                            </telerik:RadToolTip>
                            <asp:LinkButton runat="server" AlternateText="Guide" ID="btnHelp" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-info-circle"></i></span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>

            <eluc:TabStrip ID="MenuAttendanceList" runat="server" OnTabStripCommand="MenuAttendanceList_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuGrid" runat="server" OnTabStripCommand="MenuGrid_TabStripCommand" Visible="false"></eluc:TabStrip>
            <b>
                <telerik:RadLabel ID="lblTitle" runat="server" Text="Attendance"></telerik:RadLabel>
            </b>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAttendance" runat="server" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvAttendance_NeedDataSource" OnItemDataBound="gvAttendance_ItemDataBound"
                EnableViewState="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="File No." HeaderStyle-Width="65px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEECODE"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="90px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDRANKNAME"].ToString()%>
                                <telerik:RadLabel ID="lblEnrollmentId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWBATCHENROLLMENTID") %>'></telerik:RadLabel>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
