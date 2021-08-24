<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCrewApprovalSubCategory.aspx.cs" Inherits="RegisterCrewApprovalSubCategory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <%-- <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvsubcategory.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="aj1" runat="server" Height="91%">
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td> <telerik:RadLabel ID="lblapprovalcat" runat="server" Text="Approval Category"></telerik:RadLabel></td>
                    <td> <telerik:RadTextBox ID="lblapprovalcatname" Width="180PX" runat="server" Text=""></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 40px">
                        <telerik:RadTextBox ID="txtShortCode" MaxLength="20" runat="server" Width="180px"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Sub Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCategory" runat="server" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="gvsubcategoryTab" runat="server" OnTabStripCommand="gvsubcategoryTab_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvsubcategory" runat="server" AllowCustomPaging="true"
                AllowSorting="true" AllowPaging="true" Height="95%"
                CellSpacing="0" GridLines="None" ShowFooter="false" EnableViewState="true" OnSortCommand="gvsubcategory_SortCommand"
                OnNeedDataSource="gvsubcategory_NeedDataSource" OnItemCommand="gvsubcategory_ItemCommand" OnItemDataBound="gvsubcategory_ItemDataBound">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
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
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <HeaderStyle Width="35%" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsubCategoryid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSUBCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSUBCATEGORYCODE") %>'
                                    Width="50px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtcodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSUBCATEGORYCODE") %>'
                                    CssClass="gridinput_mandatory" Width="100%">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblsubcatIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSUBCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtShortCodeAdd" runat="server" Text="" Width="100%" CssClass="gridinput_mandatory">
                                </telerik:RadTextBox>
                            </FooterTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Category" AllowSorting="true" SortExpression="FLDAPPROVALSUBCATEGORYNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="55%" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSUBCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCategoryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSUBCATEGORYNAME") %>'
                                    CssClass="gridinput_mandatory" Width="100%">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblCategoryIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALCATEGORYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCategorynameAdd" runat="server" Text="" Width="100%" CssClass="gridinput_mandatory">
                                </telerik:RadTextBox>
                            </FooterTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Applies To" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="55%" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "Registers/RegsiterCrewApprovalLevelRankList.aspx?rankcategorylist=" + ","+DataBinder.Eval(Container,"DataItem.FLDRANKLIST").ToString()+"," %>' />
                                <telerik:RadLabel ID="lblranklist" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKLIST") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="height: 200px; width: 150px; overflow: auto;" class="input_mandatory">
                                    <asp:CheckBoxList ID="chkRankApplicableEdit" RepeatDirection="Vertical" Enabled="true"
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

                        <telerik:GridTemplateColumn HeaderText="Action" FooterStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDITS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="SAVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/add.png %>"
                                    CommandName="ADD" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria" PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
