<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionKPIScoreCriteria.aspx.cs" Inherits="Inspection_InspectionKPIScoreCriteria" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="num" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmkpiscoringcriteria" runat="server">
        <div>

            <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
            <telerik:RadSkinManager ID="rskin1" runat="server"></telerik:RadSkinManager>
        
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

                    <div id="divPart" style="position: relative; z-index: 2">
                        <table id="tblprosperscore" width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCategoryName" runat="server" Text="Event Type"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlsourcetype" AutoPostBack="true" OnSelectedIndexChanged="ddlsourcetype_Changed" runat="server">
                                        <Items>
                                            <%--<telerik:DropDownListItem Text="--Select--" Value="dummy"></telerik:DropDownListItem>--%>
                                            <telerik:DropDownListItem Text="INCIDENT/ACCIDENT" Value="1"></telerik:DropDownListItem>
                                            <telerik:DropDownListItem Text="INSPECTION" Value="2"></telerik:DropDownListItem>
                                            <telerik:DropDownListItem Text="VETTING" Value="3"></telerik:DropDownListItem>
                                        </Items>
                                    </telerik:RadDropDownList>
                                </td>

                            </tr>
                            <%--    <tr><td colspan="6"> </td></tr>--%>
                        </table>
                    </div>

                    <eluc:TabStrip ID="MenuRegistersInspection" runat="server" OnTabStripCommand="RegistersInspection_TabStripCommand"></eluc:TabStrip>

                    <div id="divGrid" style="position: relative; z-index: 0">
                        <%--<asp:GridView ID="gvkpiscoringcriteria" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowFooter="true"
                            ShowHeader="true" EnableViewState="false" AllowSorting="true"
                            OnRowDataBound="gvkpiscoringcriteria_ItemDataBound" OnRowEditing="gvkpiscoringcriteria_RowEditing"
                            OnRowCancelingEdit="gvkpiscoringcriteria_RowCancelingEdit" OnRowCommand="gvkpiscoringcriteria_RowCommand"
                            OnRowDeleting="gvkpiscoringcriteria_RowDeleting" OnRowUpdating="gvkpiscoringcriteria_RowUpdating"
                            DataKeyNames="FLDKPISCORECRITERIAGROUPID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvkpiscoringcriteria" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvkpiscoringcriteria_NeedDataSource"
                            OnItemDataBound="gvkpiscoringcriteria_ItemDataBound1"
                            OnItemCommand="gvkpiscoringcriteria_ItemCommand"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"
                            AutoGenerateColumns="false" ShowFooter="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
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
                                    <telerik:GridTemplateColumn HeaderText="Event">
                                        <ItemStyle Wrap="False" HorizontalAlign="left" Width="50%"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblcriteriaid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKPISCORECRITERIAGROUPID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Category">
                                        <ItemStyle Wrap="False" HorizontalAlign="left" Width="20%"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblincident" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblcatshotcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYSHORTCODE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadDropDownList ID="ddlcategory" runat="server"></telerik:RadDropDownList>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Max Score">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblmaxscore" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXSCORE") %>' CommandName="EDIT">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:num ID="txtmaxscoreedit" runat="server" MaxLength="6"
                                                CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXSCORE") %>' />
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <eluc:num ID="txtmaxscoreadd" runat="server" MaxLength="6" CssClass="gridinput_mandatory" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit"
                                                CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>

                                            </asp:LinkButton>
                                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:LinkButton runat="server" AlternateText="Delete"
                                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                                ToolTip="Delete">
                                                <span class="icon"><i class="fa fa-trash"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Map Category"
                                                CommandName="MAPPING" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMap"
                                                ToolTip="Map Category"><span class="icon"><i class="fas fa-tasks"></i></span>                                                
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Prosper Mapping"
                                                CommandName="KPIMAPPING" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdkpiscore"
                                                ToolTip="Score Criteria">
                                                <span class="icon"><i class="fas fa-calculator"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save"
                                                CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                ToolTip="Save">                                           
                                                            <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                                CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save"
                                                CommandName="ADD" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
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
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>
                </div>
          
        </div>
    </form>
</body>
</html>

