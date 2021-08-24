<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDMRAnchorHandlingType.aspx.cs"
    Inherits="RegistersDMRAnchorHandlingType" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DMRRov Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersAnchorHandlingType" autocomplete="off" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <table id="tblAnchorHandlingType">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAnchorHandlingTypeName" runat="server" Text="Description"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtAnchorHandlingType" runat="server" MaxLength="200" CssClass="input"
                        Width="240px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuRegistersAnchorHandlingType" runat="server" OnTabStripCommand="RegistersDMRRovType_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid ID="gvAnchorHandlingType" runat="server" AutoGenerateColumns="False"
            Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvAnchorHandlingType_ItemCommand"
            OnItemDataBound="gvAnchorHandlingType_ItemDataBound" ShowFooter="true" ShowHeader="true"
            OnSorting="gvAnchorHandlingType_Sorting" AllowSorting="true" AllowPaging="true"
            AllowCustomPaging="true" OnNeedDataSource="gvAnchorHandlingType_NeedDataSource"
            RenderMode="Lightweight" GridLines="None" EnableViewState="false" GroupingEnabled="false"
            EnableHeaderContextMenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <NoRecordsTemplate>
                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                        Font-Bold="true">
                    </telerik:RadLabel>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <Columns>
                    
                    <telerik:GridTemplateColumn HeaderText="Short code" FooterText="DMRRowType" AllowSorting="true" SortExpression="FLDANCHORHANDLINGTYPECODE">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                       
                        <ItemTemplate>
        <telerik:RadLabel ID="lblAnchorHandlingType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANCHORHANDLINGTYPEID") %>' ></telerik:RadLabel>
        <asp:LinkButton ID="lnkAnchorHandlingTypeCode" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANCHORHANDLINGTYPECODE") %>'></asp:LinkButton>
    </ItemTemplate>
                        <EditItemTemplate>
        <telerik:RadLabel ID="lblAnchorHandlingIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANCHORHANDLINGTYPEID") %>'   ></telerik:RadLabel>
        <telerik:RadTextBox ID="txtAnchorHandlingTypeIDEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANCHORHANDLINGTYPECODE") %>'
            CssClass="gridinput_mandatory" MaxLength="200"  ></telerik:RadTextBox>
    </EditItemTemplate>
                        <FooterTemplate>
        <telerik:RadTextBox ID="txtAnchorHandlingTypeCodeAdd" runat="server" CssClass="gridinput_mandatory"
            MaxLength="4" ToolTip="Enter DMR Rov Type Code" ></telerik:RadTextBox>
    </FooterTemplate>
                 </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn    HeaderText="Description" AllowSorting="true" SortExpression="FLDANCHORHANDLINGTYPENAME">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"  />
                        
                        <ItemTemplate>
        <telerik:RadLabel ID="lbAnchorHandlingType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANCHORHANDLINGTYPENAME") %>'    ></telerik:RadLabel>
    </ItemTemplate>
                        <EditItemTemplate>
        <telerik:RadTextBox ID="txtAnchorHandlingTypeNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANCHORHANDLINGTYPENAME") %>'
            CssClass="gridinput_mandatory" MaxLength="200"  ></telerik:RadTextBox>
                </EditItemTemplate>
                        <FooterTemplate>
         <telerik:RadTextBox ID="txtAnchorHandlingTypeNameAdd" runat="server" CssClass="gridinput_mandatory"
             MaxLength="200"    ></telerik:RadTextBox>
     </FooterTemplate>
                  </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
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
                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave"
                                ToolTip="Save">
        <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel"
                                ToolTip="Cancel">
                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Add New" CommandName="Add"
                                ID="cmdAdd">
                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                    ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
