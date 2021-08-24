<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderGroupToolBoxMeet.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderGroupToolBoxMeet" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CrewList" Src="~/UserControls/UserControlCrewList.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow;
                else if (window.frameElement && window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow;
                return oWindow;
            }
            function CloseModal() {
                GetRadWindow().close();
                parent.location.reload();
            }
        </script>

        <script type="text/javascript">
            function Resize() {

                var $ = $telerik.$;
                var height = $(window).height();
                var gvWaiver = $find("<%= gvWaiver.ClientID %>");
                var gvToolBoxList = $find("<%= gvToolBoxList.ClientID %>");

                var gvWaiverPagerHeight = (gvWaiver.PagerControl) ? gvWaiver.PagerControl.offsetHeight : 0;
                var gvToolBoxListPagerHeight = (gvToolBoxList.PagerControl) ? gvToolBoxList.PagerControl.offsetHeight : 0;
                if (gvWaiverPagerHeight.GridDataDiv != null) {
                    gvWaiverPagerHeight.GridDataDiv.style.height = (Math.round(height / 3) - gvWaiverPagerHeight - 19) + "px";
                    gvToolBoxListPagerHeight.GridDataDiv.style.height = (Math.round(height / 3) - gvToolBoxListPagerHeight - 19) + "px";
                }

            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
            function refreshParent(keepOpen) {
                if (typeof parent.CloseUrlModelWindow === "function")
                    parent.CloseUrlModelWindow(keepOpen);
            }
        </script>
        <style type="text/css">
            #table2 {
                border-collapse: collapse;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderRequisition" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="radAjaxPanel1" runat="server" EnableAJAX="true">
            <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuToolBoxMeet" runat="server" OnTabStripCommand="MenuToolBoxMeet_TabStripCommand" />
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWorkorderNumber" runat="server" Text="Work Order No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWorkorderNumber" runat="server" CssClass="readOnly" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date & Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="txtToolBoxMeet" runat="server" />
                        <telerik:RadTimePicker ID="txtToolBoxMeetTime" runat="server" RenderMode="Lightweight"></telerik:RadTimePicker>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPIC" runat="server" Text="PIC"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPersonIncharge" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID"
                            EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true" Width="300px">
                        </telerik:RadComboBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOtherMembers" runat="server" Text="Other Members"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlOtherMembers" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID"
                            EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="300px">
                        </telerik:RadComboBox>
                    </td>

                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="Menutoolboxprint" runat="server" TabStrip="false" OnTabStripCommand="Menutoolboxprint_TabStripCommand" />
            <telerik:RadGrid ID="gvWaiver" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvWaiver_NeedDataSource"
                OnItemDataBound="gvWaiver_ItemDataBound" OnItemCommand="gvWaiver_ItemCommand"
                EnableHeaderContextMenu="true" AllowMultiRowSelection="true" AllowPaging="true" AllowCustomPaging="true" EnableViewState="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Requirement Met?" Name="groupHeader1" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Comp. No." HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcomponentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID")%>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lblComponentNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNO").ToString()%>' CommandName="COMPONENT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comp. Name" HeaderStyle-Width="160px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME").ToString()%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Code and Title">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorderid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID")%>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lblWorkordername" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME").ToString()%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE").ToString())%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Risk Assessment" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="90px"
                            ItemStyle-HorizontalAlign="Center" ColumnGroupName="groupHeader1">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRaMet" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISRAMET").ToString()=="0" ? "No": "Yes" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Parts Required" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="90px"
                            ItemStyle-HorizontalAlign="Center" ColumnGroupName="groupHeader1">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPartsMet" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISPARTSREQUIREDMET").ToString()=="0" ? "No": "Yes" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PTW" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="90px"
                            ItemStyle-HorizontalAlign="Center" ColumnGroupName="groupHeader1">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPtwMet" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISPTWMET").ToString()=="0" ? "No": "Yes" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="99%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="160px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <telerik:RadGrid ID="gvToolBoxList" runat="server" RenderMode="Lightweight" OnNeedDataSource="gvToolBoxList_NeedDataSource"
                OnItemCommand="gvToolBoxList_ItemCommand" Width="100%"
                EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDTOOLBOXMEETID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date & Time" HeaderStyle-Width="76px" AllowSorting="true" ShowFilterIcon="false"
                            ShowSortIcon="true" SortExpression="FLDDATEANDTIME" DataField="FLDDATEANDTIME" FilterDelay="200">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDatetime" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDDATEANDTIME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PIC">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPicId" runat="server" Text='<%# DataBinder.Eval(Container,"Dataitem.FLDPERSONINCHARGEID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPicName" runat="server" Text='<%# DataBinder.Eval(Container,"Dataitem.FLDPERSONINCHARGENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlPicListEdit" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID"
                                    EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Others">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDOTHERMEMBERSNAME").ToString().Trim(',') %>
                                <telerik:RadLabel ID="lblOthersId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERMEMBERS")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlCrewListEdit" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID"
                                    EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="lnkPrint" runat="server" CommandName="PRINT" ToolTip="Print" ImageUrl="<%$ PhoenixTheme:images/pdf.png %>" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="80px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
