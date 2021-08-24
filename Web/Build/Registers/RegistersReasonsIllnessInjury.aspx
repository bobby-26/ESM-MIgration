﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersReasonsIllnessInjury.aspx.cs"
    Inherits="RegistersReasonsIllnesInjury" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reasons Illness Injury</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersReasonsIllnesInjury" autocomplete="off" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Illness/Injury"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuRegistersReasonsIllnessInjury" runat="server" OnTabStripCommand="RegistersReasonIllnessInjury_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvReasonIllnessInjury" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvReasonIllnessInjury_ItemCommand" OnNeedDataSource="gvReasonIllnessInjury_NeedDataSource" Height="85%"
                OnItemDataBound="gvReasonIllnessInjury_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
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
                        <telerik:GridTemplateColumn HeaderText="Reason" HeaderStyle-Width="300px" SortExpression="FLDREASON" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReasonID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASONID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkReason" runat="server" CommandName="EDIT" 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblReasonIDEDIT" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASONID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtReasonEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>' Width="98%"
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtReasonAdd" runat="server" CssClass="gridinput_mandatory" Width="98%" MaxLength="200"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="300px" SortExpression="FLDCATEGORY" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCategoryEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtCategoryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>' Width="98%"
                                    CssClass="gridinput_mandatory" MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCategoryAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="100" Width="98%"
                                    ToolTip="Enter Category">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Illness/Injury Case" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" />                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActiveYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDILLNESS").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkIllnessEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDILLNESS").ToString().Equals("1"))?true:false %>'>
                                </telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkIllnessAdd" runat="server"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50px">                            
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