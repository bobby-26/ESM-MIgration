<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselvisitAdminPage.aspx.cs" Inherits="AccountsVesselvisitAdminPage" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
        <style type="text/css">
            .input {
            }
        </style>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoiceAdminPage" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="Attachment" Text="Vessel Visit Admin" Visible="false"></eluc:Title>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand" Title="Vessel Visit Admin"
                TabStrip="true"></eluc:TabStrip>
            <table width="75%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblformnubmer" runat="server" Text="Form number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFormNo" runat="server" CssClass="input" Width="295px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Employee name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNameSearch" runat="server" CssClass="input" Width="295px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Vessel name"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chkVesselList" runat="server" Height="100%" OnSelectedIndexChanged="chkVesselList_Changed" CssClass="input"
                            RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow" Style="overflow: auto; width: 40%; height: 90px">
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuAdminPageSub" runat="server" OnTabStripCommand="MenuAdminPageSub_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvAttachment_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false"
                Height="64%" OnItemCommand="gvAttachment_ItemCommand" OnItemDataBound="gvAttachment_ItemDataBound" AllowSorting="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Form Number" HeaderStyle-Width="30%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormNumber" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDFORMNUMBER")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Employee Name" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpname" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEFULLNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Vessel Name" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Visit Status" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVisitStatus" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDHARDNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblvisitidEdit" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVISITID")) %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:Hard ID="ucHard" runat="server" AppendDataBoundItems="true" HardTypeCode="252" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-VerticalAlign="Middle">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit" ImageAlign="Middle"
                                    ToolTip="Edit"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="SAVE" CommandArgument='<%# Container.DataItem %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataItem %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
