<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionKPIProsperRankMapping.aspx.cs" Inherits="Inspection_InspectionKPIProsperRankMapping" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>KPI & Prosper Rank Mapping</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegistersInspection" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

                <%--  <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="CloseWindow" OKText="Yes"
                    CancelText="No" Visible="false" />--%>
                <div id="divInspectionType" style="position: relative; z-index: 2">
                    <table id="tblInspection" width="100%">
                        <tr>
                            <td width="10%">
                                <telerik:RadLabel ID="lbleventname" runat="server" Text="Event Type"></telerik:RadLabel>
                            </td>
                            <td width="25%">
                                <telerik:RadComboBox ID="ddlsourcetype" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddlsourcetype_Changed" runat="server"
                                    Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select event type">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="dummy"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="INCIDENT/ACCIDENT" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="INSPECTION" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="VETTING" Value="3"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td width="10%">
                                <telerik:RadLabel ID="lblcategory" runat="server" Text="Category"></telerik:RadLabel>
                            </td>
                            <td width="25%">
                                <telerik:RadComboBox ID="ddlcategory" AutoPostBack="true" OnSelectedIndexChanged="ddlcategory_Changed" Width="200px" runat="server"
                                    Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td width="10%">
                                <telerik:RadLabel ID="lblScoretype" runat="server" Text="Score Type"></telerik:RadLabel>
                            </td>
                            <td width="25%">
                                <telerik:RadComboBox ID="ddlscoretype" runat="server" Width="150px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select event type">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="dummy"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="KPI" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="PROSPER" Value="2"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>

                    </table>
                </div>

                <eluc:TabStrip ID="MenuRegistersInspection" runat="server" OnTabStripCommand="RegistersInspection_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 1">
                    <%--<asp:GridView ID="gvkpirankmapping" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3"
                            OnRowDataBound="gvkpirankmapping_ItemDataBound"
                            OnRowCommand="gvkpirankmapping_RowCommand"
                            OnRowEditing="gvkpirankmapping_RowEditing"
                            OnRowUpdating="gvkpirankmapping_RowUpdating"
                            OnRowCancelingEdit="gvkpirankmapping_RowCancelingEdit"
                            OnRowDeleting="gvkpirankmapping_RowDeleting"
                            ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                            <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvkpirankmapping" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvkpirankmapping_NeedDataSource"
                        OnItemCommand="gvkpirankmapping_ItemCommand"
                        OnItemDataBound="gvkpirankmapping_ItemDataBound1"
                        OnSortCommand="gvkpirankmapping_SortCommand"
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

                                <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="20%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblrankmappingid" Visible="false" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPROSPERSCORERANKMAPPING")) %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblcategory" Visible="false" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCATEGORYID")) %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblcategoryname" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sub Category"  HeaderStyle-Width="30%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblsubcategory" Visible="false" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYID")) %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblsubcategoryname" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadComboBox ID="ddlsubcategoryadd" runat="server" Width="100%" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category"></telerik:RadComboBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Applied To"  HeaderStyle-Width="30%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRankApplicable" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>'></telerik:RadLabel>
                                        <%-- <asp:ImageButton Visible="false" ID="ImglblRankApplicable" runat="server" ImageUrl="<%$ PhoenixTheme:images/te_view.png%>"
                                    CommandArgument='<%# Container.DataSetIndex %>'></asp:ImageButton>
                                <eluc:Tooltip ID="ucRankApplicable" runat="server" Width="180px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>' />--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="height: 200px; width: 150px; overflow: auto;" class="input_mandatory">
                                            <asp:CheckBoxList ID="chkRankApplicableEdit" Visible="true" RepeatDirection="Vertical" Enabled="true"
                                                runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <div style="height: 200px; width: 150px; overflow: auto;" class="input_mandatory">
                                            <asp:CheckBoxList ID="chkRankApplicableAdd" RepeatDirection="Vertical" Enabled="true"
                                                runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Score type"  HeaderStyle-Width="10%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblscoretype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORETYPE").ToString()  == "1" ? "KPI" : "Prosper"%>'></telerik:RadLabel>

                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action"  HeaderStyle-Width="10%">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Copy" 
                                            CommandName="COPY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCopy"
                                            ToolTip="Copy" Visible="false">
                                            <span class="icon"><i class="fas fa-copy"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:LinkButton runat="server" AlternateText="Edit" 
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete">
                                             <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>

                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:LinkButton runat="server" AlternateText="Cancel" 
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="ADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
