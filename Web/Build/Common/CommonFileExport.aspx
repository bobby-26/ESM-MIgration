<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonFileExport.aspx.cs"
    Inherits="CommonFileExport" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Export</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>      
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuExport" runat="server" OnTabStripCommand="MenuExport_TabStripCommand"></eluc:TabStrip>       
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadLabel ID="lblnote" runat="server" Text="Note: For Export to PDF, Attachment files should be in Image Format "></telerik:RadLabel>
            <br />               
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAttachment" runat="server" Height="85%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvAttachment_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvAttachment_ItemDataBound"
                OnItemCommand="gvAttachment_ItemCommand" ShowFooter="false"
                OnSortCommand="gvAttachment_SortCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="false" HeaderStyle-Width="5%" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:Image ID="imgfiletype" runat="server" Width="14px" Height="14px" />
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Select" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkExport" runat="server" EnableViewState="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File Name" AllowSorting="true" DataField="FLDFILENAME" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Download" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                    Height="14px" ToolTip="Download File">
                                </asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Size(in KB)" AllowSorting="true" DataField="FLDFILESIZE" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileSize" runat="server" Text='<%# string.IsNullOrEmpty(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString()) ? string.Empty : Math.Round(((double.Parse(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString())/1024*100000)/100000.00),2).ToString()%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Synch(Yes/No)" AllowSorting="true" DataField="FLDFILESIZE" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsynchyesno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created By" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreatedBy" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCREATEDBYNAME").ToString()) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
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
