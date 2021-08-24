<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormFBformsList.aspx.cs"
    Inherits="StandardFormFBformsList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplitter" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Form List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
        function PaneResized() {
//            var sender = $find('RadSplitter1');
//            var browserHeight = $telerik.$(window).height();
//            sender.set_height(browserHeight);
//            $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 72);
        }
    </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
    </telerik:RadScriptManager>
<%--    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />--%>
<%--    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>--%>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="tvwCategory">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecorationZoneID="tblConfigureAirlines"
        DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
      <%--  <eluc:TabStrip ID="MenuStoreItemInOutTransaction" runat="server" OnTabStripCommand="StoreItemInOutTransaction_TabStripCommand">
        </eluc:TabStrip>--%>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Height="100%" >
            <telerik:RadPane ID="navigationPane" runat="server" Width="200px" Height="95%">
                <eluc:TreeView ID="tvwCategory" runat="server" OnNodeClickEvent="ucTree_SelectNodeEvent"/>
                <asp:Label runat="server" ID="lblSelectedNode"></asp:Label>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Height="95%">
                <%--  <div id="Datadiv" style="position: relative; overflow: auto; clear: right; height: 538px">--%>
                <table cellpadding="1" cellspacing="1" style="width: 100%;">
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFormName" runat="server" Text="Name"></asp:Literal>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFormName" runat="server" CssClass="input">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblActive" runat="server" Text="Active(Y/N)"></asp:Literal>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlActive" runat="server" CssClass="input_mandatory"
                                OnSelectedIndexChanged="ddlActive_SelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <telerik:DropDownListItem Text="Yes" Value="1" />
                                    <telerik:DropDownListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>--%>
                </table>
                <eluc:TabStrip ID="MenuGridItem" runat="server" TabStrip="false" OnTabStripCommand="MenuGridItem_TabStripCommand">
                </eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight"  ID="gvFormList" runat="server" AllowCustomPaging="true" Height="87%"
                    AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvFormList_ItemCommand"
                    OnItemDataBound="gvFormList_ItemDataBound" OnUpdateCommand="gvFormList_UpdateCommand" EnableViewState="false"
                    ShowFooter="True" OnNeedDataSource="gvFormList_NeedDataSource"
                    OnSortCommand="gvFormList_SortCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                        AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDFORMID">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                            ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="S.no" AllowSorting="true" SortExpression="FLDAIRLINESCODE">
                                <HeaderStyle Width="30px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                   <%# Container.DataSetIndex + 1 %>
                                    <asp:Label ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></asp:Label>
                                    <asp:Label ID="lblEditable" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEDITABLE") %>'></asp:Label>
                                    <asp:Label ID="lblFormType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblSnoAdd" runat="server"></asp:Label>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" >
                                <HeaderStyle Width="100px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lnkFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtFormNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'
                                        CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFormNameAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="true" SortExpression="">
                                <HeaderStyle Width="70px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblStatusId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSID") %>'></asp:Label>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Is Active" AllowSorting="true" SortExpression="">
                                <HeaderStyle Width="50px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblActiveYn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString()=="1"?"Yes":"No" %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString()=="1"? true:false %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="chkActiveAdd" runat="server"></asp:Label>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Published">
                              <HeaderStyle HorizontalAlign="Left" Width="90px"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblPublishedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucPublishedDateEdit" runat="server" CssClass="gridinput_mandatory"
                                        Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Revision">
                              <HeaderStyle HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRevisionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISION") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="Edit"
                                        Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE"
                                        ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Design" ID="cmdDesign" CommandName="CREATE"
                                        ToolTip="Design" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-pen-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Publish" ID="cmdPublish" CommandName="PUBLISH"
                                        ToolTip="Publish" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-upload"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Change category" ID="cmdMove" CommandName="MOVE"
                                        ToolTip="Change category" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-exchange-alt"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Revision" ID="cmdRevision" CommandName="REVISION"
                                        ToolTip="Revision" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="distrubute" ID="cmdDistribute" CommandName="DISTRUBUTE"
                                        ToolTip="Distrubute" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-share-alt"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update"
                                        ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel"
                                        ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="Add"
                                        ToolTip="Add New" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                            AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                        AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
<%-- <telerik:RadGrid ID="gvFormList" RenderMode="Lightweight" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowCommand="gvFormList_RowCommand" OnRowDataBound="gvFormList_ItemDataBound"
                                OnRowCancelingEdit="gvFormList_RowCancelingEdit" OnRowDeleting="gvFormList_RowDeleting"
                                OnSorting="gvFormList_Sorting" OnRowEditing="gvFormList_RowEditing" AllowSorting="true"
                                ShowFooter="true" ShowHeader="true" EnableViewState="false" GridLines="None" OnUpdateCommand="gvFormList_RowUpdating"
                                OnSelectedIndexChanging="gvFormList_SelectedIndexChanging">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                    <asp:TemplateField ItemStyle-Width="40px">
                                        <ItemStyle Wrap="False" HorizontalAlign="right"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblsno" runat="server">S.no</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                            <asp:Label ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></asp:Label>
                                            <asp:Label ID="lblEditable" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEDITABLE") %>'></asp:Label>
                                            <asp:Label ID="lblFormType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblSnoAdd" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblheaderFormName" runat="server" CommandName="Sort" CommandArgument="FLDFORMNAME">Name</asp:LinkButton>
                                            <img id="FLDFORMNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lnkFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFormNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>' CssClass="input"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFormNameAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblheaderstatus" runat="server" Text="Status"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatusId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSID") %>'></asp:Label>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="40px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblActive" runat="server">Is Active</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblActiveYn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString()=="1"?"Yes":"No" %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEditActive" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString()=="1"? true:false %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="chkActiveAdd" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Published Date" HeaderStyle-Width="60px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkpurchaseDate" runat="server" Text="Published" CommandName="Sort" CommandArgument="FLDPUBLISHEDDATE"></asp:LinkButton>
                                            <img id="FLDPUBLISHEDDATE" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPublishedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Date ID="ucPublishedDateEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Revision Number" HeaderStyle-Width="60px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            Revision
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRevisionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISION") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblActionHeader" runat="server">Action </asp:Label>
                                        </HeaderTemplate>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                                ToolTip="Edit"></asp:ImageButton>
                                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Create" ImageUrl="<%$ PhoenixTheme:images/view_gallery.png %>"
                                                CommandName="CREATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCreate"
                                                ToolTip="Design Form"></asp:ImageButton>
                                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Publish" ImageUrl="<%$ PhoenixTheme:images/publish-document.png %>"
                                                CommandName="PUBLISH" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdPublish"
                                                ToolTip="Publish Form"></asp:ImageButton>
                                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Revised" ImageUrl="<%$ PhoenixTheme:images/copy-requisition.png %>"
                                                CommandName="REVISION" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdRevision"
                                                ToolTip="Make Revision"></asp:ImageButton>
                                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                                ToolTip="Delete"></asp:ImageButton>
                                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Move" ImageUrl="<%$ PhoenixTheme:images/move_items.png %>"
                                                CommandName="MOVE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdMove"
                                                ToolTip="Change Category"></asp:ImageButton>
                                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Move" ImageUrl="<%$ PhoenixTheme:images/move_items.png %>"
                                                CommandName="DISTRIBUTE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDistribute"
                                                ToolTip="Distribute to Vessel"></asp:ImageButton>

                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdUpdate"
                                                ToolTip="Update"></asp:ImageButton>
                                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                                ToolTip="Cancel"></asp:ImageButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                                ToolTip="Add New"></asp:ImageButton>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                                <tr>
                                    <td nowrap align="center">
                                        <asp:Label ID="lblPagenumber" runat="server">
                                        </asp:Label>
                                        <asp:Label ID="lblPages" runat="server">
                                        </asp:Label>
                                        <asp:Label ID="lblRecords" runat="server">
                                        </asp:Label>&nbsp;&nbsp;
                                    </td>
                                    <td nowrap align="left" width="50px">
                                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                    </td>
                                    <td width="20px">&nbsp;
                                    </td>
                                    <td nowrap align="right" width="50px">
                                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                    </td>
                                    <td nowrap align="center">
                                        <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                                        </asp:TextBox>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                            Width="40px"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
              

                
            
         
       
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
--%>