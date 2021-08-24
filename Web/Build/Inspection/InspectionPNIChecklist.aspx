<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNIChecklist.aspx.cs" Inherits="InspectionPNIChecklist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PNI Checklist</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmPNIChecklist" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
               
                <eluc:TabStrip ID="MenuPNICheckListMain" TabStrip="true" runat="server"
                    OnTabStripCommand="MenuPNICheckListMain_TabStripCommand"></eluc:TabStrip>
                 <telerik:RadLabel ID="ucTitle" runat="server" Font-Bold="true"></telerik:RadLabel>
                <br /> <br /> 
                <eluc:TabStrip ID="MenuPNIChecklist" runat="server" OnTabStripCommand="MenuPNIChecklist_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                    <%--<asp:GridView ID="gvPNIChecklist" runat="server" AutoGenerateColumns="False" OnRowCommand="gvPNIChecklist_RowCommand"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvPNIChecklist_ItemDataBound" OnRowCancelingEdit="gvPNIChecklist_RowCancelingEdit"
                        ShowHeader="true" EnableViewState="false" OnRowEditing="gvPNIChecklist_RowEditing" OnRowUpdating="gvPNIChecklist_RowUpdating"
                        OnSelectedIndexChanging="gvPNIChecklist_SelectIndexChanging"
                        DataKeyNames="FLDPNICHECKLISTID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPNIChecklist" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvPNIChecklist_NeedDataSource"
                        OnItemCommand="gvPNIChecklist_ItemCommand"
                        OnItemDataBound="gvPNIChecklist_ItemDataBound"
                        OnSortCommand="gvPNIChecklist_SortCommand"
                        EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                            AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldName="FLDDEPARTMENT" FieldAlias="Details" SortOrder="Ascending" />
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="FLDDEPARTMENT" SortOrder="Ascending" />
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn Visible="false" UniqueName="FLDDEPARTMENT" DataField="FLDDEPARTMENT">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Checklist">

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPNIChecklistid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNICHECKLISTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblPNICaseid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNICASEID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDepartmentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENT") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblQuestionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKLISTQUESTIONID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                             
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                        <eluc:ToolTip ID="ucToolTipStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Hard runat="server" ID="ucStatus" CssClass="dropdown_mandatory" HardTypeCode="36" AppendDataBoundItems="true"
                                            HardList='<%# PhoenixRegistersHard.ListHard(1, 36) %>' ShortNameFilter="YES,NO,NA" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks">
                                    
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                 
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                            ToolTip="Medical Unfit Report"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                            ToolTip="Medical Unfit Report"></asp:ImageButton>
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
                                             <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
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
    </form>
</body>
</html>
