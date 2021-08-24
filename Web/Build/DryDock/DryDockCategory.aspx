<%@ Page Language="C#" AutoEventWireup="True" CodeFile="DryDockCategory.aspx.cs" Inherits="DryDockCategory"
    MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quick</title>
   <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDryDockCategory" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
     
            <table cellpadding="1" cellspacing="1" >
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td style="padding-right : 20px">
                        <telerik:RadTextBox ID="txtCode" runat="server" MaxLength="20"  Width="180px"    ></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>

                    </td>
                    <td style="padding-right : 20px">
                        <telerik:RadTextBox ID="txtName" runat="server" MaxLength="200"  Width="180px"     ></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblActive" runat="server" Text="Active"></telerik:RadLabel>

                    </td>
                    <td style="padding-right : 20px">
                        <telerik:RadComboBox ID="ddlActive" runat="server" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category"   Width="180px"     >
                            <Items>
                                <telerik:RadComboBoxItem  Value="Dummy" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Yes" />
                                <telerik:RadComboBoxItem Value="0" Text="No" />
                            </Items>

                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            
           
                <eluc:TabStrip ID="MenuDryDockCategory" runat="server" OnTabStripCommand="DryDockCategory_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCategory" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
               EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnNeedDataSource="gvCategory_NeedDataSource"
                OnItemCommand="gvCategory_ItemCommand"  Height="88.5%"    
                OnItemDataBound="gvCategory_ItemDataBound"
                OnUpdateCommand ="gvCategory_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCATEGORYID" TableLayout="Fixed" >
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
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                      
                        <telerik:GridTemplateColumn FooterText="New Category" HeaderText="Category Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <HeaderStyle    Wrap="true"  Width="15%" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCode" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYCODE") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYCODE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="20">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCodeAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="20"
                                    ToolTip="Enter Category Name">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FooterText="New Category" HeaderText="Category Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               <HeaderStyle    Wrap="true"  Width="35%" />
                            <ItemTemplate>
                              <%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>
                                 
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'
                                    CssClass="gridinput_mandatory"   Width="98%" >
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtNameAdd" runat="server" CssClass="gridinput_mandatory"   Width="98%" 
                                    ToolTip="Enter Category Name">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FooterText="New Category" HeaderText="Frequency (Months)">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               <HeaderStyle    Wrap="true" Width="20%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCY") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtFrequencyEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCY") %>'   Width="98%"    
                                    CssClass="input" MaxLength="2" IsPositive="true"></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtFrequencyAdd" runat="server" CssClass="input" MaxLength="2" IsPositive="true"     Width="98%" ></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Active Y/N">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <HeaderStyle Wrap="true"  Width="20%" />
                            
                            <ItemTemplate>
                                <%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?"Yes":"No" %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYN" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIVEYN").ToString().Equals("1"))?true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                       
                         <telerik:GridTemplateColumn HeaderText="Action" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="LinkButton1"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="LinkButton2"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="LinkButton3"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="LinkButton4"
                                    ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Add New" CommandName="Add"
                                    ID="LinkButton5">
                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
     
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>             