<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreToBeDoneByConfiguration.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreToBeDoneByConfiguration" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>To be Done by Configuration</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <%--<eluc:TabStrip ID="MenuTitle" runat="server" Title="To be Done by Configuration"></eluc:TabStrip>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="MenuShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvConfig" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvConfig_ItemCommand" OnNeedDataSource="gvConfig_NeedDataSource" Height="99%"
                OnItemDataBound="gvConfig_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
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
                        <telerik:GridTemplateColumn HeaderText="To be Done by" HeaderStyle-Width="300px">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblNameHeader" runat="server" Text="To be Done by"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTOBEDONENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblNameEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTOBEDONENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblShortNameEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblQuickCodeEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTOBEDONECODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Before/After Joining" HeaderStyle-Width="250px">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTypeHeader" runat="server" Text="Before/After Joining"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTYPEDESC") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTypeId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddltype" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddltype_SelectedIndexChanged" Width="100px"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="" />
                                        <telerik:RadComboBoxItem Text="Before Joining" Value="1" />
                                        <telerik:RadComboBoxItem Text="After Joining" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                                <telerik:RadLabel ID="lblTypeId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days" HeaderStyle-Width="100px">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDaysHeader" runat="server" Text="Days"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDays" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDAYS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucDays" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDAYS") %>'
                                    Width="60px" MaxLength="3" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CBT/TrainingCourse" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTypeofTrainingHeader" runat="server" Text="CBT/TrainingCourse"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTypeofTraining" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCBTORTCNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucTypeofTrainingEdit" runat="server" CssClass="input_mandatory" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDCBTORTC") %>'
                                    AppendDataBoundItems="true" QuickTypeCode="129" />
                                <telerik:RadLabel ID="lblcbtortc" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCBTORTC") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
