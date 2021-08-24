<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCTypeofChangeCategory.aspx.cs"
    Inherits="InspectionMOCTypeofChangeCategory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MOC Category</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:radcodeblock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:radscriptmanager id="radscript1" runat="server"></telerik:radscriptmanager>
        <telerik:radajaxpanel id="panel1" runat="server" height="100%">

             <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
                DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divnote">
            </telerik:RadFormDecorator>
            
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblShortCode" runat="server" Text="Code"></telerik:RadLabel>
                        </td>
                        <td style="padding-right:40px"> 
                            <telerik:RadTextBox ID="txtShortCode" MaxLength="20" runat="server" Width="180px" ></telerik:RadTextBox>
                        </td>
                        
                           <td  >
                            <telerik:RadLabel ID="lblCategory" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCategory" runat="server" Width="180px"  ></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>

            <eluc:TabStrip ID="MenuMOCCategory" runat="server" OnTabStripCommand="MenuMOCCategory_TabStripCommand">
            </eluc:TabStrip>
            <telerik:RadGrid ID="gvMOCCategory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
              Width="100%" CellPadding="3" OnItemCommand="gvMOCCategory_ItemCommand"
                OnItemDataBound="gvMOCCategory_ItemDataBound" ShowHeader="true" EnableViewState="false"
                AllowSorting="true" OnSorting="gvMOCCategory_Sorting"       Height="88%"
                 AllowPaging="true" AllowCustomPaging="true" GridLines="None" 
                        OnNeedDataSource="gvMOCCategory_NeedDataSource" RenderMode="Lightweight"
                        GroupingEnabled="false" EnableHeaderContextMenu="true">
                       
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"   >
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                  <Columns>
                
                    <telerik:GridTemplateColumn HeaderText="Code">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20px"></ItemStyle>
                         <HeaderStyle     Width="35%"     HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'
                                Width="50px"></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtShortCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'
                                CssClass="gridinput_mandatory" Visible="false"></telerik:RadTextBox>
                            <telerik:RadLabel ID="lblShortCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                      
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name"   AllowSorting="true" SortExpression="FLDMOCCATEGORYNAME">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                         <HeaderStyle     Width="55%"     HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                        <%--<HeaderTemplate>
                            <telerik:RadLabel ID="lblCategoryHeader" Visible="true" runat="server">
                                <asp:ImageButton runat="server" ID="cmdCategory" OnClick="cmdSearch_Click" CommandName="FLDMOCCATEGORYNAME"
                                    ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                            </telerik:RadLabel>
                           
                        </HeaderTemplate>--%>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCCATEGORYNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCategoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCCATEGORYID") %>'
                                Visible="false"></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtCategoryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCCATEGORYNAME") %>'
                                CssClass="gridinput_mandatory"    Width="100%"    ></telerik:RadTextBox>
                            <telerik:RadLabel ID="lblCategoryIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCCATEGORYID") %>'
                                Visible="false"></telerik:RadLabel>
                        </EditItemTemplate>
                   
                    </telerik:GridTemplateColumn>
               
                         <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle     Width="10%"     HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" />

                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                <ItemTemplate>
                                     <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                                
                                </ItemTemplate>
                                 <EditItemTemplate>
                              
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                          
                        </telerik:GridTemplateColumn>

                        </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
     
    </telerik:radajaxpanel>
    </form>
</body>
</html>
