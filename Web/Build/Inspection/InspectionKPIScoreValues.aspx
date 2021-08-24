<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionKPIScoreValues.aspx.cs" Inherits="Inspection_InspectionKPIScoreValues" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="num" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>KPI Scoring Criteria</title>
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
            <telerik:RadAjaxPanel ID="panel1" runat="server">
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
                                    <telerik:RadTextBox ID="txteventname" CssClass="readonlytextbox" ReadOnly="true" Width="200px" runat="server" Text=""></telerik:RadTextBox>

                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblincidentid" runat="server" Text="Category Type"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtcategorytype" CssClass="readonlytextbox" ReadOnly="true" Width="200px" runat="server" Text=""></telerik:RadTextBox>
                                </td>

                                <td></td>

                            </tr>
                            <%--    <tr><td colspan="6"> </td></tr>--%>
                        </table>
                    </div>
                    <div class="navSelect" style="position: relative; width: 15px">
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0">
                        <%-- <asp:GridView ID="gvkpiscoringcriteria" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowFooter="true"
                            ShowHeader="true" EnableViewState="false" AllowSorting="true"
                            OnRowDataBound="gvkpiscoringcriteria_ItemDataBound" OnRowEditing="gvkpiscoringcriteria_RowEditing"
                            OnRowCancelingEdit="gvkpiscoringcriteria_RowCancelingEdit" OnRowCommand="gvkpiscoringcriteria_RowCommand"
                            OnRowDeleting="gvkpiscoringcriteria_RowDeleting" OnRowUpdating="gvkpiscoringcriteria_RowUpdating"
                            DataKeyNames="FLDKPISCORINGCRITERIAID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvkpiscoringcriteria" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                            CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvkpiscoringcriteria_NeedDataSource"
                            OnItemCommand="gvkpiscoringcriteria_ItemCommand"
                            OnItemDataBound="gvkpiscoringcriteria_ItemDataBound"
                            GroupingEnabled="false" EnableHeaderContextMenu="true"

                            AutoGenerateColumns="false" ShowFooter="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDKPISCORINGCRITERIAID">
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
                                    <telerik:GridTemplateColumn HeaderText="Measure">
                                        <itemstyle wrap="False" horizontalalign="left" width="60%"></itemstyle>
                                       
                                        <itemtemplate>
                                        <telerik:RadLabel ID="lblcriteriagroupid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKPISCORECRITERIAGROUPID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblcriteriaid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKPISCORINGCRITERIAID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblmeasurecode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURESHORTCODE") %>'></telerik:RadLabel>
                                    </itemtemplate>
                                        <footertemplate>
                                        <telerik:RadDropDownList ID="ddlmeasurecodeadd" runat="server">
                                            <Items>
                                                <telerik:DropDownListItem Text="--SELECT--" Value=""></telerik:DropDownListItem>
                                                <telerik:DropDownListItem Text="A" Value="A"></telerik:DropDownListItem>
                                                <telerik:DropDownListItem Text="B" Value="B"></telerik:DropDownListItem>
                                                <telerik:DropDownListItem Text="C" Value="C"></telerik:DropDownListItem>
                                                <telerik:DropDownListItem Text="Deficiency" Value="DEF"></telerik:DropDownListItem>
                                                <telerik:DropDownListItem Text="Detention" Value="DET"></telerik:DropDownListItem>
                                                <telerik:DropDownListItem Text="Rejection" Value="REJ"></telerik:DropDownListItem>
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </footertemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Minimum Range">
                                        <itemstyle wrap="False" horizontalalign="Right" width="10%"></itemstyle>
                                       
                                        <itemtemplate>

                                        <telerik:RadLabel ID="lblMin" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINVALUE") %>' CommandName="EDIT"></telerik:RadLabel>
                                    </itemtemplate>
                                        <edititemtemplate>

                                        <eluc:num ID="txtminedit" runat="server" MaxLength="6" IsPositive="true" DecimalPlace="5"
                                            CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINVALUE") %>' />
                                    </edititemtemplate>
                                        <footerstyle horizontalalign="Right" />
                                        <footertemplate>

                                        <eluc:num ID="txtminadd" runat="server" MaxLength="6" IsPositive="true" DecimalPlace="5" CssClass="gridinput_mandatory" />
                                    </footertemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Max Range">
                                        <itemstyle wrap="False" horizontalalign="Right" width="10%"></itemstyle>
                                        
                                        <itemtemplate>

                                        <telerik:RadLabel ID="lblMax" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXVALUE") %>' CommandName="EDIT"></telerik:RadLabel>
                                    </itemtemplate>
                                        <edititemtemplate>

                                        <eluc:num ID="txtmaxedit" runat="server" MaxLength="6" IsPositive="true" DecimalPlace="5"
                                            CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXVALUE") %>' />
                                    </edititemtemplate>
                                        <footerstyle horizontalalign="Right" />
                                        <footertemplate>

                                        <eluc:num ID="txtmaxadd" runat="server" MaxLength="6" IsPositive="true" DecimalPlace="5" CssClass="gridinput_mandatory" />
                                    </footertemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Score">
                                        <itemstyle wrap="False" horizontalalign="Right" width="10%"></itemstyle>
                                       
                                        <itemtemplate>

                                        <telerik:RadLabel ID="lblscore" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>' CommandName="EDIT"></telerik:RadLabel>
                                    </itemtemplate>
                                        <edititemtemplate>

                                        <eluc:num ID="txtscoreedit" runat="server" MaxLength="6" IsInteger="true" IsPositive="false"
                                            CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCORE") %>' />
                                    </edititemtemplate>
                                        <footerstyle horizontalalign="Right" />
                                        <footertemplate>

                                        <eluc:num ID="txtscoreadd" runat="server" MaxLength="6" IsInteger="true" IsPositive="false" CssClass="gridinput_mandatory" />
                                    </footertemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <itemstyle wrap="False" horizontalalign="Right" width="10%"></itemstyle>
                                        <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                                       
                                        <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                        <itemtemplate>
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

                                    </itemtemplate>
                                        <edititemtemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Cancel" 
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                        </asp:LinkButton>

                                    </edititemtemplate>
                                        <footerstyle horizontalalign="Center" />
                                        <footertemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="ADD" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></i></span>
                                        </asp:LinkButton>
                                    </footertemplate>
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
        </div>
    </form>
</body>
</html>

