<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSCLI.aspx.cs" Inherits="Dashboard_DashboardBSCLI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Leading Indicators (LI)</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                  <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvPI" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
              <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="Tabli" runat="server" OnTabStripCommand="LI_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="LI_TabStripMenuCommand" TabStrip="true"></eluc:TabStrip>
           <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvLI" AutoGenerateColumns="false"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvLI_NeedDataSource" Height="88.5%" 
            OnItemDataBound="gvLI_ItemDataBound" OnItemCommand="gvLI_ItemCommand" ShowFooter="false">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDLIID" AutoGenerateColumns="false" 
                TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" >
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
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblliid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLICODE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblliname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLINAME")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Unit">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="108px" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblliunit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUNIT")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>  
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Action By">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="150px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblliactionby" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIONBY")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>  
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Frequency">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"  width="150px"/>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblfrequency" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCY")%>'>
                                </telerik:RadLabel>
                                &nbsp&nbsp&nbsp
                                <telerik:RadLabel ID="Radlblfrequencytype" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCYTYPE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Description">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Justify" Wrap="true" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="RadlblPIdescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75px" Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit" >
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                  &nbsp&nbsp
                                <asp:LinkButton runat="server" ID="btncolor" ToolTip="Grading Configuration" >
                                            <span class="icon"><i class="fas fa-sliders-h"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>   
                        </telerik:GridTemplateColumn>
                  </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                    AlwaysVisible="true" />
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
