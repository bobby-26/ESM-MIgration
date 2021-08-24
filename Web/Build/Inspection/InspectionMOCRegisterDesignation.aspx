<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCRegisterDesignation.aspx.cs"
    Inherits="InspectionMOCRegisterDesignation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Role Designation</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersDesignation" runat="server" autocomplete="off">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table id="tblConfiguremoccategory">
            <tr>
                <td style="padding-right: 20px">
                    <telerik:RadLabel ID="lblApproverRole" runat="server" Text="User Role">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlApproverRole" runat="server" Width="180px" CssClass="input_mandatory"
                        OnSelectedIndexChanged="ddlApproverRole_SelectedIndexChanged" AppendDataBoundItems="true"
                        AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Approver Role" />
                </td>
            </tr>
            <tr>
                <td style="padding-right: 20px">
                    <telerik:RadLabel ID="lblCode" runat="server" Text="Code">
                    </telerik:RadLabel>
                </td>
                <td style="padding-right: 40px">
                    <telerik:RadTextBox ID="txtCode" runat="server" MaxLength="10" Width="180px">
                    </telerik:RadTextBox>
                </td>
                <td style="padding-right: 20px">
                    <telerik:RadLabel ID="lblName" runat="server" Text="Name">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtName" runat="server" MaxLength="100" Width="180px">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuRegistersDesignation" runat="server" OnTabStripCommand="RegistersDesignation_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid ID="gvMOCDesignation" runat="server" AutoGenerateColumns="False"
            Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvMOCDesignation_ItemCommand"
            OnItemDataBound="gvMOCDesignation_ItemDataBound" AllowSorting="true" OnSorting="gvMOCDesignation_Sorting"
            ShowFooter="false" Height="30%" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvMOCDesignation_NeedDataSource"
            EnableHeaderContextMenu="true" GroupingEnabled="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger"
                                    Font-Bold="true">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false"
                    ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Code" HeaderStyle-Width="40%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONCODE") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONCODE") %>'
                                CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="40%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDesignationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONID") %>'>
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblDesignationIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONID") %>'>
                            </telerik:RadLabel>
                            <telerik:RadTextBox ID="txtNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'
                                CssClass="gridinput_mandatory" MaxLength="100" Width="100%">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="De-Select" CommandName="DESELECT" ID="cmdDeSelect"
                                ToolTip="De-Select">
                                       <span class="icon"><i class="fas fa-times-circle"></i></span>

                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <br />
        <br />
        <telerik:RadLabel ID="lblMOCNameHeader" runat="server" Text='Add Designation' Font-Size="Small">
        </telerik:RadLabel>
        <br />
        <telerik:RadGrid ID="gvDesignation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Height="45%" Width="100%" CellPadding="3" OnItemCommand="gvDesignation_ItemCommand"
            OnItemDataBound="gvDesignation_ItemDataBound" EnableHeaderContextMenu="true"
            GroupingEnabled="false" OnNeedDataSource="gvDesignation_NeedDataSource" AllowSorting="true"
            OnSorting="gvDesignation_Sorting" AllowPaging="true" AllowCustomPaging="true"
            ShowHeader="true" EnableViewState="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="RadLabel2" runat="server" Text="No Records Found" Font-Size="Larger"
                                    Font-Bold="true">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false"
                    ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridTemplateColumn SortExpression="FLDDESIGNATIONCODE" AllowSorting="true"
                        HeaderText="Code" HeaderStyle-Width="40%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblCodeHeader" Visible="true" runat="server">
                                <asp:LinkButton runat="server" ID="cmdCode" OnClick="cmdSearch_Click" CommandName="FLDDESIGNATIONCODE"></asp:LinkButton>
                            </telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONCODE") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONCODE") %>'
                                CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" SortExpression="FLDDESIGNATIONNAME"
                        AllowSorting="true" HeaderStyle-Width="40%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDesignationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONID") %>'>
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtDesignationIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONID") %>'
                                Visible="false">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'
                                CssClass="gridinput_mandatory" MaxLength="100" Width="100%">
                            </telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Select" CommandName="SELECT" ID="cmdSelect"
                                ToolTip="Select">
                                          <span class="icon"><i class="fa fa-check-circle"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <eluc:Status ID="ucStatus" runat="server" />
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
