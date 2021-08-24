<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersMiscellaneousAppraisalOccasion.aspx.cs"
    Inherits="RegistersMiscellaneousAppraisalOccasion" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occasion" Src="~/UserControls/UserControlAppraisalOccasion.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Appraisal Occasions</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersMiscellaneousAppraisalOccasion" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Rank Group Mapping"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblAppraisalOccasssion" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategorySearch" runat="server" Text="Rank Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucCategory" runat="server" HardTypeCode="90" ShortNameFilter="SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO") %>'
                            AppendDataBoundItems="true" Width="300px" AutoPostBack="true" OnTextChangedEvent="TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOccasionSearch" runat="server" Text="Occasion"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Occasion ID="ucOccasion" runat="server" AutoPostBack="true"
                            Width="450px" AppendDataBoundItems="true" OnTextChangedEvent="TextChangedEvent"></eluc:Occasion>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersMiscellaneousAppraisalOccasion" runat="server" OnTabStripCommand="RegistersMiscellaneousAppraisalOccasion_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="dgMiscellaneousAppraisalOccasion" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="dgMiscellaneousAppraisalOccasion_ItemCommand" OnNeedDataSource="dgMiscellaneousAppraisalOccasion_NeedDataSource" Height="85%"
                OnItemDataBound="dgMiscellaneousAppraisalOccasion_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                     DataKeyNames="FLDOCCASIONID">
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
                        <telerik:GridTemplateColumn HeaderText="Rank Group" HeaderStyle-Width="300px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCategoryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcategoryIdedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucCategoryAdd" runat="server" CssClass="dropdown_mandatory" HardTypeCode="90"
                                    ShortNameFilter="SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO" HardList='<%# PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO") %>'
                                    AppendDataBoundItems="true" Width="300px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Rank List" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server"
                                    Screen='<%# "Registers/RegistersToolTipAppraisalRankList.aspx?rankcategorylist=" + ","+DataBinder.Eval(Container,"DataItem.FLDCATEGORY").ToString()+"," %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Occasion" HeaderStyle-Width="200px" SortExpression="FLDOCCASION" AllowSorting="true">                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkOccasion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCCASION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOccasionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCCASION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOccasionrefIdedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEOCCASIONID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Occasion ID="ucOccasionInsert" runat="server" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" Width="300px"></eluc:Occasion>
                                <%-- <asp:TextBox ID="txtOccasioninsert" runat="server" CssClass="gridinput_mandatory"
                                            MaxLength="200" ToolTip="Enter Occasion"></asp:TextBox>--%>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Occasion Description" Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOccasionDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCCASIONDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOccasionDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCCASIONDESCRIPTION") %>'
                                    MaxLength="200">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <%-- <asp:TextBox ID="txtOccasionDescriptioninsert" runat="server" CssClass="gridinput_mandatory"
                                            MaxLength="200" ToolTip="Enter Country Name"></asp:TextBox>--%>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Appraisal on Signoff" HeaderStyle-Width="130px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAppraisalSignoff" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALONSIGNOFF").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkAppraisalSignoffEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDAPPRAISALONSIGNOFF").ToString().Equals("Yes"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkAppraisalSignoffAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Active YN" HeaderStyle-Width="70px">                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYNItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCCASIONACTIVEYN").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDOCCASIONACTIVEYN").ToString().Equals("Yes"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">                            
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
