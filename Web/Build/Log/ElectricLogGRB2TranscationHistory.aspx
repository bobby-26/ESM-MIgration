<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogGRB2TranscationHistory.aspx.cs" Inherits="Log_ElectricLogGRB2TranscationHistory" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Transaction History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvTranscationHistory.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>


        <%-- For Popup Relaod --%>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>

        <br />
        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" DatePicker="true" />
                </td>
            </tr>
        </table>
        <br />

        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>


        <telerik:RadGrid RenderMode="Lightweight" ID="gvTranscationHistory" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true" Height="92%"
            OnNeedDataSource="gvTranscationHistory_NeedDataSource"
            OnItemCommand="gvTranscationHistory_ItemCommand"
            OnItemDataBound="gvTranscationHistory_ItemDataBound">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>

                    <telerik:GridTemplateColumn HeaderText='Entry' AllowSorting='true' ItemStyle-Width="50">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDISPLAYID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPLAYID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Datetime' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE", "{0:dd-MM-yyyy hh:mm tt}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Type' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>



                    <telerik:GridTemplateColumn HeaderText='User Id' AllowSorting='true' ItemStyle-Width="50px">
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblUSERID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Rank' AllowSorting='true'>
                        <ItemStyle Width="50px" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRANKID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKSHORTCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Method' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMethod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMETHOD") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Category' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Quantity' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Facility / Vessel' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFacility" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTPOSITION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Receipt Ref' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Attachment"
                                CommandName="ATTACHMENT" ID="lnkAttachment" CssClass="ui-attachment"
                                ToolTip="Attachment" Width="20PX" Height="20PX">
                                <span class='icon'><i runat="server" id="attachmentIcon" class='fas fa-paperclip-na'></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Status' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStatus" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSSHORTCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Verified' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVerified" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIED") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Re-Verified' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblReverified" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVERIFIED") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Modifying' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMODIFYING" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFYING") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <%--<telerik:GridTemplateColumn HeaderText='Reason' AllowSorting='true'>
                        <ItemStyle Width="50px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblREASON" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
