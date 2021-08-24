<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersPostponeMatrix.aspx.cs" Inherits="Registers_RegistersPostponeMatrix" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Postpone Matrix</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPostponematrix" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuPostponematrix" runat="server" OnTabStripCommand="Postponematrix_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPostponematrix" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvPostponematrix_ItemCommand" OnItemDataBound="gvPostponematrix_ItemDataBound"
                OnSortCommand="gvPostponematrix_SortCommand" OnNeedDataSource="gvPostponematrix_NeedDataSource"
                ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table id="Table1" runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Priority" AllowSorting="true" SortExpression="FLDPRIORITY">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkpriority" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPriorityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="First ApprovalBy" AllowSorting="true" SortExpression="FLDFIRSTAPPROVALBY">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkfirstapprovalby" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTAPPROVALBY") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtfirstapprovalbyEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTAPPROVALBY") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="First Extension Office Approval" AllowSorting="true" SortExpression="FLDFIRSTEXTNOFFICEAPPROVAL">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfirstofficeapproval" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNOFFICEAPPROVAL") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkfirstofficeapprovalby" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNOFFICEAPPROVAL") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtfirstofficeapprovalbyEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNOFFICEAPPROVAL") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                                </telerik:RadTextBox>
                                <telerik:RadComboBox runat="server" ID="ddlfirstofficeapproval" DataTextField="FLDROLENAME" DataValueField="FLDFUNCTIONALROLEID" DropDownPosition="Static" AutoPostBack="true" CssClass="dropdown"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="First Extension ship Approval" AllowSorting="true" SortExpression="FLDFIRSTEXTNSHIPAPPROVAL">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfirstshipeapproval" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNSHIPAPPROVAL") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkfirsteapproval" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNSHIPAPPROVAL") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtfirstshipapprovalbyEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNSHIPAPPROVAL") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                                </telerik:RadTextBox>
                                <telerik:RadComboBox runat="server" ID="ddlfirstshipapproval" DataTextField="FLDGROUPRANK" DataValueField="FLDGROUPRANKID" DropDownPosition="Static" AutoPostBack="true" CssClass="dropdown"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="First Extension Days" AllowSorting="true" SortExpression="FLDFIRSTEXTNDAYS">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkfirstextensiondays" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNDAYS") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtfirstextensiondaysEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTEXTNDAYS") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>



                        <telerik:GridTemplateColumn HeaderText="Second ApprovalBy" AllowSorting="true" SortExpression="FLDSECONDAPPROVALBY">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnksecondapprovalby" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDAPPROVALBY") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtsecondapprovalbyEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDAPPROVALBY") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Second Extension Office Approval" AllowSorting="true" SortExpression="FLDSECONDEXTNOFFICEAPPROVAL">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsecondofficeapproval" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNOFFICEAPPROVAL") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnksecodofficeapprovalby" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNOFFICEAPPROVAL") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtsecondofficeapprovalbyEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNOFFICEAPPROVAL") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                                </telerik:RadTextBox>
                                <telerik:RadComboBox runat="server" ID="ddlsecondofficeapproval" DataTextField="FLDROLENAME" DataValueField="FLDFUNCTIONALROLEID" DropDownPosition="Static" AutoPostBack="true" CssClass="dropdown"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Second Extension ship Approval" AllowSorting="true" SortExpression="FLDSECONDEXTNSHIPAPPROVAL">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsecondshipeapproval" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNSHIPAPPROVAL") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnksecondshipapproval" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNSHIPAPPROVAL") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtsecondshipapprovalbyEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNSHIPAPPROVAL") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                                </telerik:RadTextBox>
                                <telerik:RadComboBox runat="server" ID="ddlsecondshipapproval" DataTextField="FLDGROUPRANK" DataValueField="FLDGROUPRANKID" DropDownPosition="Static" AutoPostBack="true" CssClass="dropdown"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="First Extension Days" AllowSorting="true" SortExpression="FLDSECONDEXTNDAYS">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnksecondextensiondays" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNDAYS") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtsecondextensiondaysEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDEXTNDAYS") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
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
