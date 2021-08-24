<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceHistoryTemplate.aspx.cs" Inherits="PlannedMaintenanceHistoryTemplate" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakeModel" Src="~/UserControls/UserControlEquipmentMakerModel.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Maintenance Form</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvMaintenanceForm.ClientID %>"));
            }, 200);
        }

        function pageLoad() {
            Resize();
        }
        </script>
    </telerik:RadCodeBlock>
    <style>
        .imgbtn-height {
            height: 20px;
        }
    </style>
</head>
<body onresize="Resize()" onload="Resize()">
    <form id="formPMHistoryTemplate" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="ajxpanel" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureHistoryTemplate">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTemplateName" runat="server" Text="Form Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTemplateName" runat="server" CssClass="input" Width="300px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuHistoryTemplate" runat="server" OnTabStripCommand="MenuHistoryTemplate_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvMaintenanceForm" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMaintenanceForm" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvMaintenanceForm_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvMaintenanceForm_ItemDataBound" OnItemCommand="gvMaintenanceForm_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowFooter="true" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" Height="10px">
                    <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form No." HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtFormNo" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtFormNoAdd" runat="server" CssClass="input_mandatory" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFormEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtformname" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="lblFormName" runat="server" CssClass="input_mandatory" Width="200px"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Model" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblModel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:MakeModel ID="ucmodeledit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODEL") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:MakeModel ID="ucModel" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Reported Date" HeaderStyle-Width="15%" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastReportedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTREPORTEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="120px" FooterStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Excel Template" ID="cmdExcelTemplate"
                                    CommandName="EXCEL" CommandArgument='<%# Container.DataItem %>' ToolTip="Excel Template">
                                    <span class="icon"><i class="far fa-file-excel"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="comp job" ID="cmdComjob"
                                    CommandName="COMJOB" CommandArgument='<%# Container.DataItem %>' ToolTip="Component Job">
                                    <span class="icon"><i class="far fa-list-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="reports" ID="cmdReports"
                                    CommandName="REPORTS" CommandArgument='<%# Container.DataItem %>' ToolTip="Reports">
                                    <span class="icon"><i class="fas fa-file-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItem %>' ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ID="cmdAdd"
                                    CommandName="ADD" CommandArgument='<%# Container.DataItem %>' ToolTip="Excel Template">
                                    <span class="icon"><i class="fas fa-plus-square"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
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
