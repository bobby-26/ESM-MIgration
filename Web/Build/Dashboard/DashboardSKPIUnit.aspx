<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKPIUnit.aspx.cs" Inherits="Inspection_InspectionPIUnit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Performance Indicator (PI) Unit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/Phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvPIUnit" DecoratedControls="All" EnableRoundedCorners="true" />
         <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
          <%-- <eluc:TabStrip ID="Tabkpi" runat="server" OnTabStripCommand="KPI_TabStripMenuCommand" TabStrip="true"></eluc:TabStrip>--%>
        <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="PIUnit_TabStripMenuCommand" TabStrip="true"></eluc:TabStrip>
                   <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
     
        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvPIUnit" AutoGenerateColumns="false"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvPIUnit_NeedDataSource" Height="88.5%"
            OnItemDataBound="gvPIUnit_ItemDataBound" OnItemCommand="gvPIUnit_ItemCommand" ShowFooter="true">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDUNITID" AutoGenerateColumns="false" 
                TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="false" >
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
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblpiunitcode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUNITCODE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                         <EditItemTemplate>
                              <telerik:RadTextBox ID="Radpiunitcodeedit" runat="server" CssClass="input_mandatory" width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUNITCODE")%>'>
                                </telerik:RadTextBox>
                         </EditItemTemplate>
                            <FooterTemplate>
                                 <telerik:RadTextBox ID="Radpiunitcodeentry" runat="server" CssClass="input_mandatory" width="150px">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblpiunitname" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.FLDUNIT")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <telerik:RadTextBox ID="Radpiunitnameedit" runat="server" CssClass="input_mandatory" width="150px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUNIT")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                 <telerik:RadTextBox ID="Radpiunitnameentry" runat="server" CssClass="input_mandatory" width="150px">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" >
                            <FooterStyle HorizontalAlign="Center" Font-Bold="true"/>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit"  CommandName="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <asp:LinkButton runat="server" ID="btnupdate" ToolTip="Update"  CommandName="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                             &nbsp &nbsp
                              <asp:LinkButton runat="server" ID="btncancel" ToolTip="Cancel"  CommandName="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" ID="btnsave" ToolTip="Add"  CommandName="Add">
                                            <span class="icon"><i class="fas fa-plus-circle"></i></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
