<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsTickler.aspx.cs"
    Inherits="OptionsTickler" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function pageLoad() {
                PaneResized();
            }
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvTickler");
                grid._gridDataDiv.style.height = (browserHeight - 410) + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPhoenixBroadcast" runat="server" OnTabStripCommand="PhoenixBroadcast_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUser" runat="server" Text="User"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListUsers">
                            <telerik:RadTextBox CssClass="input_mandatory" runat="server" ID="txtUser" MaxLength="200" Width="80%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="lblUserCode" runat="server" Width="0px"></telerik:RadTextBox>
                        </span>
                        <asp:ImageButton ID="cmdShowUsers" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemindOn" runat="server" Text="Remind On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtReminderDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        &nbsp;
                            <telerik:RadLabel ID="lblTime" runat="server" Text="Time"></telerik:RadLabel>
                        &nbsp;
                            <telerik:RadTimePicker ID="txtTime" runat="server" CssClass="input_mandatory" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMessage" runat="server" Text="Message"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox CssClass="input_mandatory" runat="server" ID="txtMessage" TextMode="MultiLine"
                            Rows="5" Columns="80" MaxLength="500" Width="80%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <table width="80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReminderFromDate" runat="server" Text="Reminder From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReminderToDate" runat="server" Text="Reminder To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSentRecieved" runat="server" Text="Sent / Recieved"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlStatus" runat="server">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="--Select--" />
                                <telerik:DropDownListItem Value="1" Text="Sent" />
                                <telerik:DropDownListItem Value="0" Text="Received" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompleted" runat="server" Text="Completed"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlCompleted" runat="server">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="--Select--" />
                                <telerik:DropDownListItem Value="1" Text="Yes" />
                                <telerik:DropDownListItem Value="0" Text="No" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuTickler" runat="server" OnTabStripCommand="MenuTickler_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTickler" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false" Width="100%"
                OnItemCommand="gvTickler_ItemCommand" OnNeedDataSource="gvTickler_NeedDataSource" OnItemDataBound="gvTickler_ItemDataBound"
                OnUpdateCommand="gvTickler_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDTICKLERID">
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
                        <telerik:GridTemplateColumn HeaderText="Posted By">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblTicklerID" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTICKLERID")%>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPOSTEDBYNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reminder Date">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" />
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDREMINDERDATE").ToString() != string.Empty ? 
                                        DateTime.Parse(DataBinder.Eval(Container, "DataItem.FLDREMINDERDATE").ToString()).ToLocalTime().ToString("g")
                                        : ""%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblReminderDate" runat="server" Text=' <%#DataBinder.Eval(Container, "DataItem.FLDREMINDERDATE").ToString() != string.Empty ? 
                                        DateTime.Parse(DataBinder.Eval(Container, "DataItem.FLDREMINDERDATE").ToString()).ToLocalTime().ToString("g"): ""%>'>
                                </telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comments">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucRemarksTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKREMARKS") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRemarksEdit" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDTASKREMARKS").ToString() %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sent To">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" />
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDTOUSER")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ReScheduled Date">
                            <HeaderStyle HorizontalAlign="Left" Width="13%" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE").ToString() != string.Empty ?
                                       DateTime.Parse(DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE").ToString()).ToLocalTime().ToString("g"): ""%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtRescheduleDate" runat="server" Enabled="true" CssClass="input_mandatory" Width="93%" DatePicker="true" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE")%>' />
                                <%--<telerik:RadTextBox ID="txtReScheduleTime" runat="server" CssClass="input_mandatory" Width="80%" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE").ToString() != string.Empty ?
                                       DateTime.Parse(DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE").ToString()).ToLocalTime().ToString("t"): ""%>' />--%>
                                <telerik:RadTimePicker ID="txtReScheduleTime" runat="server" Enabled="true" Width="80px" DbSelectedDate='<%#DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE").ToString() != string.Empty ?
                                       DateTime.Parse(DataBinder.Eval(Container, "DataItem.FLDSCHEDULEDATE").ToString()).ToLocalTime().ToString("t"): ""%>'>
                                </telerik:RadTimePicker>
                                hrs
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDYN").ToString() == "0" ? "" : "Yes"%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkDoneYN" runat="server" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action Taken">
                            <HeaderStyle HorizontalAlign="Left" Width="17%" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoneRemarks" runat="server" Width="100%"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDREMARKS").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDREMARKS").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDREMARKS").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucDoneRemarksTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDREMARKS") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDoneRemarks" Enabled="true" runat="server" Width="100%" CssClass="gridinput_mandatory"
                                    TextMode="MultiLine" Resize="Both">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action By">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" />
                            <ItemTemplate>
                                <%# Eval("FLDCOMPLETEDBYNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action Date">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCOMPLETEDDATE", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sent / Received">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSENTYN").ToString() == "1" ? "Sent" : "Received"%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" ID="cmdEdit" ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Snooze" ImageUrl="<%$ PhoenixTheme:images/dashboard.png %>"
                                    CommandName="SNOOZE" ID="cmdSnooze" ToolTip="Snooze"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" ID="cmdSave" ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
