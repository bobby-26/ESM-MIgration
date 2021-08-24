<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsERMVoucherPrefix.aspx.cs" Inherits="AccountsERMVoucherPrefix" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
     <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
           
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           
            <eluc:TabStrip ID="Menutab" runat="server" TabStrip="false" >
                            </eluc:TabStrip>

                  
                    <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
              
                    <table width="100%">
                        <tr>
                            <td>Company</td>
                            <td><telerik:RadTextBox ID="txtcompany" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            <td>Database</td>
                            <td><telerik:RadTextBox ID="txtDatabase1" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            <td>ERM</td>
                            <td><telerik:RadTextBox ID="txtERM" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                        </tr>
                        <tr>
                            
                            <td>Phoenix</td>
                            <td><telerik:RadTextBox ID="txtPhoenix" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                             <td>Phoenix TRN</td>
                            <td><telerik:RadTextBox ID="txtPhoenixTRN" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            <td>X Access</td>
                            <td><telerik:RadTextBox ID="txtXAccess" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                        </tr>
                        <tr>
                            <td>Z Tiime</td>
                            <td><telerik:RadTextBox ID="txtXTime" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            <td>ZU Time</td>
                            <td><telerik:RadTextBox ID="txtZUTime" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                             <td>ZID</td>
                            <td><telerik:RadTextBox ID="txtZID" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                        </tr>
                        <tr>
                           
                            <td>X Type TRN</td>
                            <td><telerik:RadTextBox ID="txtXtypeTRN" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                             <td>X TRN</td>
                            <td><telerik:RadTextBox ID="txtXTRN" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            <td>X Action</td>
                            <td><telerik:RadTextBox ID="txtXAction" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                        </tr>
                        <tr>
                            <td>X Description</td>
                            <td><telerik:RadTextBox ID="txtXDescription" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            <td>X Number</td>
                            <td><telerik:RadTextBox ID="txtXNumber" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                             <td>X Inc</td>
                            <td><telerik:RadTextBox ID="txtXInc" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                        </tr>
                        <tr>
                           
                            <td>Z Active</td>
                            <td><telerik:RadTextBox ID="txtZActive" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            <td>Column1</td>
                            <td><telerik:RadTextBox ID="txtColumn1" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                            <td>Database2</td>
                            <td><telerik:RadTextBox ID="txtDatabase2" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox></td>
                        </tr>
                    </table>
              
                    <eluc:TabStrip ID="MenuERMVoucherprefix" runat="server" OnTabStripCommand="MenuERMVoucherprefix_TabStripCommand">
                    </eluc:TabStrip>
               
           <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvERMVoucherprefix" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvERMVoucherprefix" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvERMVoucherprefix_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvERMVoucherprefix_SelectedIndexChanging"
                    OnItemDataBound="gvERMVoucherprefix_ItemDataBound" OnItemCommand="gvERMVoucherprefix_ItemCommand"
                    ShowFooter="false" ShowHeader="true" OnSortCommand="gvERMVoucherprefix_SortCommand" Height="96%">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDDTKEY">

        
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Company" >
                                 <HeaderStyle Width="15%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbldtkey" runat="server"  Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkCompany" runat="server" CommandName="Select" CommandArgument='<%# Bind("FLDDTKEY") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANY")  %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Database">
                                   <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               <%-- <HeaderTemplate>
                                    <asp:Label ID="lblDatabase1" runat="server" ForeColor="White">Database&nbsp;</asp:Label>
                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATABASE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ERM">
                                  <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblERM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDERM") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Phoenix">
                                  <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPhoenix" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHOENIX") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Phoenix TRN">
                                  <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPhoenixtrn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHOENIXTRN") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="X Access">
                                  <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblXAccess" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXACCESS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Z Time">
                                   <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                             
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblZTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZTIME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="ZU Time">
                                   <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblZUTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZUTIME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="ZID">
                                   <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblZID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="X Type TRN">
                                   <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblXTypeTRN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXTYPETRN") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="X TRN">
                                   <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblXTRN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXTRN") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="X Action">
                                   <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblXAction" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXACTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="X Description">
                                   <HeaderStyle Width="13%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblXDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXDESC") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="X Num">
                                   <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblXNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXNUM") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="X Inc">
                                   <HeaderStyle Width="7%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblXInc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXINC") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Z Active">
                                   <HeaderStyle Width="10%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblZActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZACTIVE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Column1">
                                   <HeaderStyle Width="10%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblColumn1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMN1") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Databse2">
                                   <HeaderStyle Width="10%" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                             
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDatabse2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATABASE2") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                 <HeaderStyle Width="10%" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                
                                <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>" CommandArgument='<%# Bind("FLDDTKEY") %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                     <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                     
                  </telerik:RadAjaxPanel>
            
            
    </form>
</body>
</html>
             <%--   <div id="divGrid" style="position: relative; z-index: 0; width: 100%; overflow-x:auto; width:100%" >
                    <asp:GridView ID="gvERMVoucherprefix" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvERMVoucherprefix_RowCommand" OnRowDataBound="gvERMVoucherprefix_ItemDataBound"
                        OnRowCancelingEdit="gvERMVoucherprefix_RowCancelingEdit" OnRowDeleting="gvERMVoucherprefix_RowDeleting"
                        AllowSorting="true" OnRowEditing="gvERMVoucherprefix_RowEditing" EnableViewState="false"
                        OnSorting="gvERMVoucherprefix_Sorting" OnSelectedIndexChanging="gvERMVoucherprefix_SelectedIndexChanging"
                        DataKeyNames="FLDDTKEY">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Company">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                <label id="lblcompany" runat="server" ForeColor="White">Company</label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbldtkey" runat="server"  Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkCompany" runat="server" CommandName="Select" CommandArgument='<%# Bind("FLDDTKEY") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANY")  %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Database1">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDatabase1" runat="server" ForeColor="White">Database&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATABASE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ERM">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblERMheader" runat="server" Text="ERM"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblERM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDERM") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phoenix">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPhoenixheader" runat="server" Text="Phoenix"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPhoenix" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHOENIX") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phoenix TRN">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPhoenixtrnheader" runat="server" Text="Phoenix TRN"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPhoenixtrn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHOENIXTRN") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="X Access">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblXAccessheader" runat="server" Text="X Access"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblXAccess" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXACCESS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Z Time">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblZTimeheader" runat="server" Text="Z Time"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblZTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZTIME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="ZU Time">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblZUTimeheader" runat="server" Text="ZU Time"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblZUTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZUTIME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="ZID">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblZIDheader" runat="server" Text="ZID"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblZID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="X Type TRN">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblXTypeTRNheader" runat="server" Text="X Type TRN"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblXTypeTRN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXTYPETRN") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="X TRN">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblXTRNheader" runat="server" Text="X TRN"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblXTRN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXTRN") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="X Action">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblXActionheader" runat="server" Text="X Action"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblXAction" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXACTION") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="X Description">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblXDescriptionheader" runat="server" Text="X Description"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblXDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXDESC") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="X Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblXNumberheader" runat="server" Text="X Num"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblXNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXNUM") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="X Inc">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblXIncheader" runat="server" Text="X Inc"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblXInc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXINC") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Z Active">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblZActiveheader" runat="server" Text="Z Active"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblZActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZACTIVE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Column1">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblColumn1header" runat="server" Text="Column1"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblColumn1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOLUMN1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Databse2">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblDatabse2header" runat="server" Text="Database2"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDatabse2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATABASE2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>" CommandArgument='<%# Bind("FLDDTKEY") %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
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
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>       
    </asp:UpdatePanel>
    </form>
</body>
</html>--%>
