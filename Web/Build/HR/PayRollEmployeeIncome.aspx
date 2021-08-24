<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollEmployeeIncome.aspx.cs" Inherits="PayRoll_PayRollEmployeeIncome" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Income</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

      <%--  <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvEmployerSalary.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>--%>
    </telerik:RadCodeBlock>
    <style>
        .bold {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server" Height="100%">
        <%-- For Popup Relaod --%>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>

        <h2>Salary</h2>
        <eluc:TabStrip ID="gvSalary" runat="server"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvEmployerSalary" Height="30%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="true" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvEmployerSalary_NeedDataSource" 
            OnItemCommand="gvEmployerSalary_ItemCommand"
            OnItemDataBound="gvEmployerSalary_ItemDataBound">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>

                    <telerik:GridTemplateColumn HeaderText='Nature of Salary' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNATUREOFSALARY" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATUREOFSALARY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Description' AllowSorting='true' FooterText="Total" FooterStyle-CssClass="bold">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDESCRIPTION" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Amount' AllowSorting='true' Aggregate='Sum' FooterText="  " DataField="FLDAMOUNT" FooterStyle-CssClass="bold">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAMOUNT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
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
        <br />

        <h2>Perquisite</h2>
        <eluc:TabStrip ID="gvPerquisiteTabStrip" runat="server"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvPerQuisite" Height="30%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="true" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvPerQuisite_NeedDataSource"
            OnItemCommand="gvPerQuisite_ItemCommand"
            OnItemDataBound="gvPerQuisite_ItemDataBound">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>

                    <telerik:GridTemplateColumn HeaderText='Nature Of Perquisite' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNATUREOFSALARY" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATUREOFSALARY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Description' AllowSorting='true' FooterText="Total" FooterStyle-CssClass="bold">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDESCRIPTION" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Amount' AllowSorting='true' Aggregate="Sum" FooterText=" " DataField="FLDAMOUNT" FooterStyle-CssClass="bold">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAMOUNT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50px" AllowSorting='true' HeaderTooltip="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLEMPLOYERPERQUISITEID") %>' ID="cmdEdit"
                                ToolTip="Edit" Width="20PX">
                                  <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLEMPLOYERPERQUISITEID") %>' ID="cmdDelete"
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
        <br />

        <h2>Other Incomes</h2>
        <eluc:TabStrip ID="gvotherIncomeTabStrip" runat="server"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvincome" Height="30%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="true" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvincome_NeedDataSource"
            OnItemCommand="gvincome_ItemCommand"
            OnItemDataBound="gvincome_ItemDataBound">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDPAYROLLINCOMEOTHERSOURCESID">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>
             
                    <telerik:GridTemplateColumn HeaderText='Income Head' AllowSorting='true' FooterText="Total" FooterStyle-CssClass="bold">
                        <HeaderStyle width="50%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblINCOMEHEAD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCOMEHEAD") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Amount' AllowSorting='true' Aggregate="Sum" DataField="FLDAMOUNT" FooterText="  " FooterStyle-CssClass="bold">
                         <HeaderStyle width="30%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAMOUNT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                        
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%" AllowSorting='true' HeaderTooltip="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLINCOMEOTHERSOURCESID") %>' ID="cmdEdit"
                                ToolTip="Edit" Width="20PX">
                                  <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLINCOMEOTHERSOURCESID") %>' ID="cmdDelete"
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
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
