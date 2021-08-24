<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCrewCostEvaluationSection.aspx.cs"
    Inherits="RegistersCrewCostEvaluationSection" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SectionType" Src="~/UserControls/UserControlCrewCostEvaluationSectionType.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <title>Section</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersSection" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table cellpadding="1" cellspacing="1" width="70%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSectionType" runat="server" Text="Section Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SectionType ID="ddlSectionType" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSection" runat="server" Text="Section"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSectionName" runat="server"></telerik:RadTextBox>
                    </td>
                     <td>
                        <telerik:RadLabel ID="lblActiveYn" runat="server" Text="Active"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlActive" runat="server" AppendDataBoundItems="false"
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Yes" />
                                <telerik:RadComboBoxItem Value="0" Text="No" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuSection" runat="server" OnTabStripCommand="MenuSection_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvSection" runat="server" AutoGenerateColumns="False" EnableViewState="false" Height="90%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="true" ShowFooter="true"
                OnNeedDataSource="gvSection_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvSection_ItemCommand"
                OnItemDataBound="gvSection_ItemDataBound1" OnDeleteCommand="gvSection_DeleteCommand" OnUpdateCommand="gvSection_UpdateCommand">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
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
                        <telerik:GridTemplateColumn HeaderText="Section Type"  HeaderStyle-Width="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSectionTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSectionTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblSectionIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONID") %>'></telerik:RadLabel>
                                <eluc:SectionType ID="ucSectionTypeEdit" runat="server" CssClass="dropdown_mandatory"
                                    SelectionTypeList="<%#PhoenixRegistersCrewCostEvaluationSectionType.ListCrewCostEvaluationSectionType(null) %>" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:SectionType ID="ucSectionTypeAdd" runat="server" CssClass="dropdown_mandatory" Width="100%"
                                    SelectionTypeList="<%#PhoenixRegistersCrewCostEvaluationSectionType.ListCrewCostEvaluationSectionType(null) %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Section"  >
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSectionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSectionNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="80%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSectionNameAdd" runat="server" CssClass="gridinput_mandatory" Width="80%"
                                    MaxLength="200">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EDIT"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">                                   
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DELETE"
                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">                                   
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave" ToolTip="Save">                                   
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel" ToolTip="Cancel">                                   
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd" ToolTip="Add New">                                    
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
