<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetVesselFinancialYear.aspx.cs" Inherits="CommonBudgetVesselFinancialYear" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Financial Year Setup</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript">
            function checkSubmit(e) {
                if (e && e.keyCode == 13) {
                    var navigatorVersion = navigator.appVersion;
                    var navigatorAgent = navigator.userAgent;
                    var browserName = navigator.appName;
                    var fullVersionName = '' + parseFloat(navigator.appVersion);
                    var majorVersionName = parseInt(navigator.appVersion, 10);
                    var nameOffset, verOffset, ix;

                    var img = document.getElementById("cmdHiddenSubmit");
                    __doPostBack('img', '');

                    //                if ((verOffset = navigatorAgent.indexOf("MSIE")) != -1) {
                    //                    var img = document.getElementById("cmdHiddenSubmit");
                    //                    __doPostBack('img', '');                    
                    //                }
                    //                else {
                    //                    cmdHiddenSubmit.click();
                    //                }
                    return false;
                }
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselFinancialYearSetup" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlFinancialYearSetup" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuBudgetTab" runat="server" TabStrip="true" OnTabStripCommand="BudgetTab_TabStripCommand"></eluc:TabStrip>
            <table id="tblFinancialYearSetup" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td style="width: 50%">
                        <span id="spnPickListVesselAccount">
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                Width="10%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                MaxLength="50" Width="36%">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowAccount" runat="server" OnClientClick="return showPickList('spnPickListVesselAccount', 'codehelp1', '', '../Common/CommonPickListVesselAndAccountCombined.aspx',true); ">
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVesselId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Financial Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFinancialYear" runat="server" MaxLength="4" CssClass="input"></telerik:RadTextBox>
                        <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedEditFinancialYear" runat="server" AutoComplete="false"
                                    InputDirection="RightToLeft" Mask="9999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtFinancialYear" />--%>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuFinancialYearSetup" runat="server" OnTabStripCommand="FinancialYearSetup_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="dgFinancialYearSetup" runat="server" AutoGenerateColumns="False"
                CellPadding="3" Font-Size="11px" OnItemCommand="dgFinancialYearSetup_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true" Height="80%"
                OnItemDataBound="dgFinancialYearSetup_ItemDataBound" OnDeleteCommand="dgFinancialYearSetup_DeleteCommand" AllowPaging="true" AllowCustomPaging="true"
                OnSortCommand="dgFinancialYearSetup_SortCommand" OnNeedDataSource="dgFinancialYearSetup_NeedDataSource"
                OnRowCreated="dgFinancialYearSetup_RowCreated"
                AllowSorting="true" ShowFooter="True" Style="margin-bottom: 0px" Width="100%"
                EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle" />
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                    <HeaderStyle Width="102px" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFinancialYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALYEAR") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Start" AllowSorting="true" SortExpression="FLDFINANCIALSTARTYEAR" ShowSortIcon="true">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblFinancialStartYearHeader" runat="server" CommandName="Sort"
                                    CommandArgument="FLDFINANCIALSTARTYEAR"></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFinancialStartYear" runat="server" CommandArgument="<%# Container.DataItem %>"
                                    CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALSTARTYEAR","{0:dd/MMM/yyyy}") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:UserControlDate ID="txtFinancialStartYearAdd" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="End" AllowSorting="true"  SortExpression="FLDFINANCIALENDYEAR" ShowSortIcon="true">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblFinancialEndYearHeader" runat="server" CommandName="Sort"
                                    CommandArgument="FLDFINANCIALENDYEAR"></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkFinancialEndYear" runat="server" CommandArgument="<%# Container.DataItem %>"
                                    CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALENDYEAR","{0:dd/MMM/yyyy}") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:UserControlDate ID="txtFinancialEndYearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALENDYEAR","{0:dd/MMM/yyyy}") %>'
                                    CssClass="input_mandatory" />
                                <eluc:UserControlDate ID="txtFinancialStartYearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALSTARTYEAR","{0:dd/MMM/yyyy}") %>'
                                    CssClass="input_mandatory" Visible="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Financial Year">
                            <ItemTemplate>
                                <telerik:RadTextBox ID="txtMapCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPCODE") %>'
                                    Visible="false">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lnkFinancialYear" runat="server" CommandArgument="<%# Container.DataItem %>"
                                    CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsRecentFinancialYear" runat="server" CommandArgument="<%# Container.DataItem %>"
                                    Visible="false" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECENTFINANCIALYEAR") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtMapCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPCODE") %>'
                                    Visible="false">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtFinancialYearEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'
                                    CssClass="input" />
                                <telerik:RadLabel ID="lblIsRecentFinancialYear" runat="server" CommandArgument="<%# Container.DataItem %>"
                                    Visible="false" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECENTFINANCIALYEAR") %>'>
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtFinancialYear" runat="server" CssClass="input"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="88px" />
                            <ItemStyle Width="20px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
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
