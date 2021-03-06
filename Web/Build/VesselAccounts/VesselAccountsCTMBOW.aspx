<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCTMBOW.aspx.cs"
    Inherits="VesselAccountsCTMBOW" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew BOW</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuCTMMain" runat="server" OnTabStripCommand="MenuCTMMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCTM" Height="90%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCTM_ItemCommand" OnItemDataBound="gvCTM_ItemDataBound" EnableHeaderContextMenu="true"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvCTM_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDFILENO"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblbowId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDBOWID"] %>'></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDRANKCODE"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Sign On">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsignondate" runat="server"
                                    Text='<%#string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDSIGNONDATE"]) %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned Relief Due">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDRELIEFDUEDATE"]) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSignonoffid" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDSIGNONOFFID"] %>'></telerik:RadLabel>
                                <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" Text='<%#((DataRowView)Container.DataItem)["FLDFINISHDATE"] %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Literal ID="lblTotal" runat="server" Text="Total"></asp:Literal>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDAMOUNT"] %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdDetails" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>

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
