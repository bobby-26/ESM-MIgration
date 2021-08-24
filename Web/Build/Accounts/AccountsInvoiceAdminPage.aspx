<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceAdminPage.aspx.cs"
    Inherits="Accounts_AccountsInvoiceAdminPage" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Vessel Sign-On</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoiceAdminPage" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
       <%-- <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuAdminPage" runat="server" OnTabStripCommand="MenuAdminPage_OnTabStripCommand"></eluc:TabStrip>
            <table width="75%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ddlYearlist" runat="server" QuickTypeCode="55" AppendDataBoundItems="true"
                            AutoPostBack="true" CssClass="input_mandatory" OnTextChangedEvent="ucMonthHard_Changed" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblmonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucMonthHard" runat="server" HardTypeCode="55" Width="50%" AppendDataBoundItems="true"
                            AutoPostBack="true" CssClass="input_mandatory" SortByShortName="true" OnTextChangedEvent="ucMonthHard_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceReceivedFromDate" runat="server" Text="Invoice Received From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceReceivedToDate" runat="server" Text="Invoice Received To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceReferenceNumber" runat="server" Text="Invoice Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceRefNumber" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Choose a file"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:FileUpload ID="FileUpload" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucHard" runat="server" AppendDataBoundItems="true" CssClass="input"
                            HardTypeCode="60" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPIC" runat="server" Text="PIC"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserName ID="ddlUser" runat="server" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuAdminPageSub" runat="server" OnTabStripCommand="MenuAdminPageSub_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" Height="52%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvAttachment_ItemCommand" OnItemDataBound="gvAttachment_ItemDataBound" OnNeedDataSource="gvAttachment_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Invoice Code" AllowSorting="true">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceCode" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDINVOICECODE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Number" AllowSorting="true">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Status" AllowSorting="true">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDHARDNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceCodeEdit" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDINVOICECODE")) %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:Hard ID="ucHard" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    HardTypeCode="60" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PIC" AllowSorting="true">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPic" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPICNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:UserName ID="ddlUser" runat="server" AppendDataBoundItems="true" CssClass="input" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" visible="false" />
                                <asp:ImageButton runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                    ID="cmdVesselList" ImageUrl="<%$ PhoenixTheme:images/vessel.png%>" ImageAlign="AbsMiddle"
                                    CommandName="VESSELLIST" Text=".." ToolTip="Vessel List" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="SAVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
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
       <%-- </telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
