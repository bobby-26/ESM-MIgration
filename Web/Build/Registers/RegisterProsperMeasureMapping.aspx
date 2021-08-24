<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterProsperMeasureMapping.aspx.cs" Inherits="Registers_RegisterProsperMeasureMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="num" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Measure Mapping</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmprospercategory" runat="server">
        <div>
            <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxPanel ID="panel1" runat="server">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

                    <div id="divFind" style="position: relative; z-index: 2">
                        <table id="tblprospermm" width="100%">
                            <tr>
                                <td width="20%">
                                    <telerik:RadLabel ID="lblCode" runat="server" Text="Category Name"></telerik:RadLabel>
                                </td>
                                <td width="30%">
                                    <telerik:RadComboBox ID="ddlcategorycode" runat="server" AllowCustomText="true" Width="65%" EmptyMessage="Type to Select">
                                    </telerik:RadComboBox>
                                </td>
                                <td width="20%">
                                    <telerik:RadLabel ID="lblmeasurecode" runat="server" Text="Measure Name"></telerik:RadLabel>
                                </td>
                                <td width="30%">
                                    <telerik:RadComboBox ID="ddlmeasurecode" runat="server" Width="65%" AllowCustomText="true" EmptyMessage="Type to Select">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <eluc:TabStrip ID="MenuRegistersProsper" runat="server" OnTabStripCommand="RegistersProsper_TabStripCommand"></eluc:TabStrip>

                    <div id="divGrid" style="position: relative; z-index: 0">
                        <%--  <asp:GridView ID="gvprospermeasuremap" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowFooter="true"
                            ShowHeader="true" EnableViewState="false" AllowSorting="true"
                            OnRowDataBound="gvprospermeasuremap_RowDataBound" OnRowEditing="gvprospermeasuremap_RowEditing"
                            OnRowCancelingEdit="gvprospermeasuremap_RowCancelingEdit" OnRowCommand="gvprospermeasuremap_RowCommand"
                            OnRowDeleting="gvprospermeasuremap_RowDeleting" OnRowUpdating="gvprospermeasuremap_RowUpdating"
                            OnRowCreated="gvprospermeasuremap_RowCreated"
                            DataKeyNames="FLDMEASUREMAPPINGID" OnSorting="gvprospermeasuremap_Sorting">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvprospermeasuremap" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvprospermeasuremap_NeedDataSource"
                            OnItemCommand="gvprospermeasuremap_ItemCommand"
                            OnItemDataBound="gvprospermeasuremap_ItemDataBound"
                            OnSortCommand="gvprospermeasuremap_SortCommand"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"
                            AutoGenerateColumns="false" ShowFooter="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDMEASUREMAPPINGID">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Category">

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblmeasuremappingid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASUREMAPPINGID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblcategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblcategorycode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYCODE") %>'></telerik:RadLabel>

                                            <telerik:RadLabel ID="lnkcatogoryname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblmeasuremappingidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASUREMAPPINGID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblcategoryidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                            <telerik:RadComboBox ID="ddlcategoryedit" runat="server" CssClass="gridinput_mandatory" AppendDataBoundItems="true" DataTextField="FLDCATEGORYNAME" Width="70%"
                                                AllowCustomText="true" EmptyMessage="Type to Select" DataValueField="FLDCATEGORYID">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddlcategoryadd" runat="server" CssClass="gridinput_mandatory" AppendDataBoundItems="true" DataTextField="FLDCATEGORYNAME" Width="70%"
                                                AllowCustomText="true" Visible="false" EmptyMessage="Type to Select" DataValueField="FLDCATEGORYID">
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Measure">

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblmeasureid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASUREID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lnkmeasurename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblmeasureidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASUREID") %>'></telerik:RadLabel>
                                            <telerik:RadComboBox ID="ddlmeasureedit" runat="server" CssClass="gridinput_mandatory" AppendDataBoundItems="true" DataTextField="FLDMEASURENAME"
                                                AllowCustomText="true" EmptyMessage="Type to Select" Width="70%" DataValueField="FLDMEASUREID">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddlmeasureadd" runat="server" CssClass="gridinput_mandatory" AppendDataBoundItems="true" DataTextField="FLDMEASURENAME"
                                                AllowCustomText="true" EmptyMessage="Type to Select" Width="70%" DataValueField="FLDMEASUREID">
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit"
                                                CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                          
                                            <asp:LinkButton runat="server" AlternateText="Delete" 
                                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                                ToolTip="Delete">
                                                 <span class="icon"><i class="fa fa-trash"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Prosper Mapping"
                                                CommandName="VEESELMAPPING" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdvesselmap"
                                                ToolTip="Map Vessel">
                                                 <span class="icon"><i class="fas fa-tasks"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Prosper Mapping" 
                                                CommandName="PROSPERMAPPING" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdProsperMapping"
                                                ToolTip="Score Criteria">
                                                <span class="icon"><i class="fas fa-calculator"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save"
                                                CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                        
                                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                                CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" 
                                                CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                                ToolTip="Add New">
                                                <span class="icon"><i class="fa fa-plus-circle"></i></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>

                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>

                </div>
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
