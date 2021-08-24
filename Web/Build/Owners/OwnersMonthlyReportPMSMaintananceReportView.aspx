<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportPMSMaintananceReportView.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="OwnersMonthlyReportPMSMaintananceReportView" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvMaintenance.ClientID %>"));
                }, 200);
           }
        </script>    

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

                    <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvMaintenance" runat="server" GridLines="None" OnNeedDataSource="gvMaintenance_NeedDataSource" EnableLinqExpressions="false"
            AutoGenerateColumns="false" Width="100%" OnItemDataBound="gvMaintenance_ItemDataBound" OnItemCommand="gvMaintenance_ItemCommand" OnPreRender="gvMaintenance_PreRender"
            GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false" AllowFilteringByColumn="true"
            ShowFooter="false" ShowHeader="true" AllowCustomPaging="false">
            <MasterTableView TableLayout="Fixed" FilterExpression="">
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Form No." HeaderStyle-Width="5%" AllowFiltering="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Form Name" 
                        UniqueName="FLDFORMNAME"
                        AllowSorting="false" FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains" HeaderStyle-Width="25%">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFormID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblworkorderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                             <telerik:RadLabel ID="lblFormType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                             <asp:LinkButton ID="lblFormName" runat="server" CommandName="DOWNLOAD" CommandArgument='<%# Container.DataItem %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></asp:LinkButton>
                             <telerik:RadLabel ID="lblReportId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Jobcode & Title"
                        UniqueName="FLDWORKORDERNAME"
                        AllowSorting="false" FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains"  HeaderStyle-Width="25%">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblWoname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component Number"
                        UniqueName="FLDCOMPONENTNUMBER"
                        AllowSorting="false" FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains" HeaderStyle-Width="10%">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcono" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component Name"
                        UniqueName="FLDCOMPONENTNAME"
                        AllowSorting="false" FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains" HeaderStyle-Width="25%">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblconame" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lbljsonyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJSONREPORTYN") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="10%" ItemStyle-Width="10%" AllowFiltering="false">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastReportedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTREPORTEDDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" Visible="false" HeaderStyle-Width="20%" FooterStyle-HorizontalAlign="Center" AllowFiltering="false">
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
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
        </telerik:RadGrid>

        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    </form>
</body>
</html>
