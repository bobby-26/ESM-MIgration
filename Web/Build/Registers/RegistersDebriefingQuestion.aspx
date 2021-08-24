<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDebriefingQuestion.aspx.cs" Inherits="Registers_RegistersDebriefingQuestion" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Debriefing  Question</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersDebriefingQuestion" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="De-Briefing Question"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureDebrefingQuestion" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategoryid" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlcategory" AutoPostBack="true" runat="server" CssClass="input_mandatory" AllowCustomText="true" EmptyMessage="Type to Select"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblQuestionName" runat="server" Text="Question"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtQuestionName" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRankApplicable" runat="server" Text="Applicable Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="250px" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuRegistersDebriefingQuestion" runat="server" OnTabStripCommand="RegistersgvDebriefingQuestion_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvDebriefingQuestion" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvDebriefingQuestion_ItemCommand" OnNeedDataSource="gvDebriefingQuestion_NeedDataSource" Height="85%"
                OnItemDataBound="gvDebriefingQuestion_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true"
                OnSortCommand="gvDebriefingQuestion_SortCommand">
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
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuestionId" Visible="false" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDQUESTIONID")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtQuestionIdEdit" Visible="false" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Category">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCodeHeader" Visible="true" runat="server">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkCategoryHeader" runat="server" CommandName="Sort" CommandArgument="FLDCATEGORYNAME">
                                    Category</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlCategoryEdit" Width="98%" runat="server" DataSource='<%# PhoenixRegistersDebriefingQuestion.CategoryList() %>'
                                    DataValueField="FLDCATEGORYID" DataTextField="FLDCATEGORYNAME" AllowCustomText="true" EmptyMessage="Type to Select" CssClass="dropdown_mandatory">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <%--<asp:DropDownList ID="ddlCategoryAdd" Width="150px" runat="server" CssClass="dropdown_mandatory" DataSource='<%# PhoenixRegistersDebriefingQuestion.CategoryList() %>'
                                    DataValueField="FLDCATEGORYID" DataTextField="FLDCATEGORYNAME">
                                </asp:DropDownList>--%>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Question">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblquestionheader" runat="server" Text="Question"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuestionName" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDQUESTIONNAME")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtQuestionNameEdit" TextMode="MultiLine" runat="server" Width="98%" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONNAME")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtQuestionNameAdd" TextMode="MultiLine" runat="server" Width="98%" CssClass="input_mandatory"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="210px" HeaderText="Applicable Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRankApplicableHeader" runat="server" Text="Applicable Rank"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                 <telerik:RadLabel ID="lblRankApplicable" Width="98%" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANK")%>' CssClass="tooltip" ClientIDMode="AutoID"></telerik:RadLabel>
                                <eluc:ToolTip ID="ucRankApplicable" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>' TargetControlId="lblRankApplicable" />
                                <%--<telerik:RadLabel ID="lblRankApplicable" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKAPPLICABLE") %>'></telerik:RadLabel>
                               <asp:LinkButton runat="server" AlternateText="View"  Width="20PX" Height="20PX" ID="ImglblRankApplicable" ClientIDMode="AutoID">
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                                <eluc:ToolTip ID="ucRankApplicable" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKAPPLICABLE") %>' TargetControlId="lblRankApplicable" /> --%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="height: 100px; width: 230px; overflow: auto;" class="input_mandatory">
                                    <telerik:RadCheckBoxList ID="chkRankApplicableEdit" Visible="true" RepeatDirection="Vertical" Enabled="true"
                                        runat="server">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div style="height: 100px; width: 230px; overflow: auto;" class="input_mandatory">
                                    <telerik:RadCheckBoxList ID="chkRankApplicableAdd" Direction="Vertical" Enabled="true" runat="server">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Sort Order">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSortOrderHeader" runat="server" Text="Sort Order"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSortOrder" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSORTORDER")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtSortOrderEdit" runat="server" CssClass="input_mandatory" MaxLength="2"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtSortOrderAdd" runat="server" Width="100%" CssClass="input_mandatory" MaxLength="2"
                                    IsInteger="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Require Remark Y/N" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRequireRemarkHeader" runat="server">
                                    Require Remark?
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequireRemark" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDREQUIREREMARK").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="ChkRequireRemarkEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDREQUIREREMARK").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkRequireRemarkrAdd" runat="server" MaxLength="10"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActiveHeader" runat="server">
                                    Active?
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActive" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
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
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Question Option" ToolTip="Question Option" Width="20PX" Height="20PX"
                                    CommandName="SELECT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdselect">
                                <span class="icon"><i class="fas fa-list-alt"></i></span>
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
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
