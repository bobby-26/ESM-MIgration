<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDMRSeaCondition.aspx.cs" Inherits="Registers_RegistersDMRSeaCondition" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sea Condition </title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegisterSEACONDITION" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
        
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                
                        
                    <eluc:TabStrip ID="MenuRegistersSeaCondition" runat="server" OnTabStripCommand="RegistersSeaCondition_TabStripCommand">
                    </eluc:TabStrip>
                
                    <telerik:RadGrid ID="gvSeaCondition" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnItemCommand="gvSeaCondition_ItemCommand" OnItemDataBound="gvSeaCondition_ItemDataBound"
                        AllowSorting="true" OnSorting="gvSeaCondition_Sorting" ShowFooter="true" OnNeedDataSource="gvSeaCondition_NeedDataSource"
                        ShowHeader="true" EnableViewState="false" AllowCustomPaging="true" AllowPaging="true" GroupingEnabled="false" EnableHeaderContextMenu="true">
                        <%--<FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
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
                            
                            <telerik:GridTemplateColumn HeaderText="Short name"  FooterText="New Cargo">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                <ItemTemplate>
                                     <telerik:RadLabel ID="lblConditionShortName" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>   
                                 
                                  <EditItemTemplate>
                                   <telerik:RadTextBox ID="txtConditionShortNameEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' runat="server" CssClass="gridinput_mandatory" MaxLength="200" />
                                </EditItemTemplate>

                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtConditionShortNameAdd" runat="server"  CssClass="gridinput_mandatory" MaxLength="200"  ToolTip="Enter Condition Short Name" />    
                                </FooterTemplate>
                                   
                         </telerik:GridTemplateColumn>
                            
                                <telerik:GridTemplateColumn HeaderText=Description FooterText="New Short">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="40%"></ItemStyle>
                                                        
                                <ItemTemplate>                                   
                                    <telerik:RadLabel ID="lblConditionCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEACONDITIONCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblConditionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEACONDITIONNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblConditionCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEACONDITIONCODE") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtConditionNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEACONDITIONNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></telerik:RadTextBox>
                                </EditItemTemplate>
                                
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtConditionNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                        ToolTip="Enter Condition Name"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn >
                            
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
                                    <asp:LinkButton runat="server" AlternateText="Delete" 
                                        CommandName="DELETE" ID="cmdDelete"
                                        ToolTip="Delete">
                                        <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                </ItemTemplate>
                                
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" 
                                        CommandName="Update" ID="cmdSave"
                                        ToolTip="Save">
                                         <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:LinkButton runat="server" AlternateText="Cancel" 
                                        CommandName="Cancel"  ID="cmdCancel" ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                </EditItemTemplate>
                                
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" 
                                        CommandName="Add"  ID="cmdAdd" ToolTip="Add New">
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
