<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollEmployeeDeduction.aspx.cs" Inherits="PayRollEmployeeDeduction" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deduction 80G</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .bold {
            font-weight:bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="Radajaxpanel" runat="server" Height="100%">
            <%-- For Popup Relaod --%>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>

            <%-- <table id="tbldeduction" width="100%" >
            <tr>
                <td width="3%">--%>
            <telerik:RadLabel ID="lblname" runat="server" Text="Name Of Done" Visible="false"></telerik:RadLabel>
            <%--</td>
                <td width="28.3%">--%>
            <telerik:RadTextBox ID="txtname" runat="server" Text="" Visible="false"></telerik:RadTextBox>

            <%--  </td>
            </tr>
        </table>--%>


            <h2>Deduction 80G</h2>
            <eluc:TabStrip ID="gvTabStripGA" runat="server" OnTabStripCommand="gvTabStripGA_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvdeduction80g" Height="30%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" ShowFooter="true" Style="margin-bottom: 0px" EnableViewState="true"
                OnNeedDataSource="gvdeduction80g_NeedDataSource"
                OnItemCommand="gvdeduction80g_ItemCommand"
                OnItemDataBound="gvdeduction80g_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPAYROLLDEDUCTION80GID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText='Name' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNAMEOFDONEE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFDONEE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Address' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblADDRESS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='City' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCITYTOWNDISTRICT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYTOWNDISTRICT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText='Country' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCOUNTRY" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText='State' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSTATE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Pin Code' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPINCODE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPINCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Pan Number' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPANNUMBER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPANNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Eligible Amount' AllowSorting='true' FooterText="Total" FooterStyle-CssClass="bold">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblELIGIBLEAMOUNT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDELIGIBLEAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Amount' AllowSorting='true' FooterText="  " Aggregate="Sum" DataField="FLDAMOUNT" FooterStyle-CssClass="bold">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAMOUNT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Percentage' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPERCENTAGE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting='true' HeaderTooltip="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLDEDUCTION80GID") %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX">
                                  <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLDEDUCTION80GID") %>' ID="cmdDelete"
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


            <h2>Deduction 80GGA</h2>
            <eluc:TabStrip ID="gvTabStripGGA" runat="server" OnTabStripCommand="gvTabStripGGA_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvdeduction80gga" Height="30%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" ShowFooter="true" Style="margin-bottom: 0px" EnableViewState="true"
                OnNeedDataSource="gvdeduction80gga_NeedDataSource"
                OnItemCommand="gvdeduction80gga_ItemCommand"
                OnItemDataBound="gvdeduction80gga_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPAYROLLDEDUCTION80GGAID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText='Name' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNAMEOFDONEE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFDONEE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Address' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblADDRESS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='City' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCITYTOWNDISTRICT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYTOWNDISTRICT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText='Country' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCOUNTRY" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText='State' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSTATE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Pin Code' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPINCODE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPINCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Pan Number' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPANNUMBER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPANNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Eligible Amount' AllowSorting='true' FooterText="Total" FooterStyle-CssClass="bold">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblELIGIBLEAMOUNT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDELIGIBLEAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Amount' AllowSorting='true' FooterText="  " Aggregate="Sum" DataField="FLDAMOUNT" FooterStyle-CssClass="bold">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAMOUNT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Percentage' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPERCENTAGE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting='true' HeaderTooltip="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLDEDUCTION80GGAID") %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX">
                                  <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLDEDUCTION80GGAID") %>' ID="cmdDelete"
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


            <h2>Exempt Income</h2>
            <eluc:TabStrip ID="gvTabStripEXEMPT" runat="server" OnTabStripCommand="gvTabStripEXEMPT_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvexemptincome" Height="30%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" ShowFooter="true" Style="margin-bottom: 0px" EnableViewState="true"
                OnNeedDataSource="gvexemptincome_NeedDataSource"
                OnItemCommand="gvexemptincome_ItemCommand"
                OnItemDataBound="gvexemptincome_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPAYROLLEXEMPTINCOMEID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText='Section' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSECTION" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Head' AllowSorting='true'>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHEAD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEAD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Head Description' AllowSorting='true' FooterText="Total" FooterStyle-CssClass="bold">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHEADDESCRIPTION" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEADDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText='Amount' AllowSorting='true' FooterText="  " FooterStyle-CssClass="bold" Aggregate="Sum" DataField="FLDAMOUNT">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAMOUNT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50px" AllowSorting='true' HeaderTooltip="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLEXEMPTINCOMEID") %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX">
                                  <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLEXEMPTINCOMEID") %>' ID="cmdDelete"
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
