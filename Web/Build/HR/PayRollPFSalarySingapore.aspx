<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollPFSalarySingapore.aspx.cs" Inherits="PayRoll_PayRollPFSalarySingapore" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pay Roll PF Contribution Singapore</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvPFContributionSingapore.ClientID %>"));
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

          <h2>Salary</h2>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvPFContributionSingapore" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvPFContributionSingapore_NeedDataSource" 
            OnItemCommand="gvPFContributionSingapore_ItemCommand"
            OnItemDataBound="gvPFContributionSingapore_ItemDataBound">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText='Date' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDATE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE", "{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Nature Of Salary' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNATUREOFSALARY" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATUREOFSALARY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


             

                    <telerik:GridTemplateColumn HeaderText='Ordinary Wages' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblORDINARYWAGES" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDINARYWAGES") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Nature Of Employment' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNATUREOFEMPLOYMENT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATUREOFEMPLOYMENT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Employee Status' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEMPLOYEESTATUS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEESTATUS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Active Salary' AllowSorting='true'>
                        <ItemTemplate>

                   <telerik:RadLabel ID="lblISACTIVESALARY" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDISACTIVESALARY").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>




                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50px" AllowSorting='true' HeaderTooltip="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLEMPLOYERSALARYID") %>' ID="cmdEdit"
                                ToolTip="Edit" Width="20PX">
                                  <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLEMPLOYERSALARYID") %>' ID="cmdDelete"
                                ToolTip="Delete" Width="20PX">
                                     <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>

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
