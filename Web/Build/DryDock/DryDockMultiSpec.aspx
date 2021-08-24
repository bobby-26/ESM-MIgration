<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockMultiSpec.aspx.cs"
    Inherits="DryDockMultiSpec" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Additional Specifications</title>
   
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
      
     <telerik:RadAjaxPanel ID="panel1" runat="server"   Height="100%">

           <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table id="tblCatalog" >
                    <tr>
                        <td style="padding-right:20px">
                            <telerik:RadLabel ID="lblRegister" runat="server" Text="Register"></telerik:RadLabel>
                        </td>
                        <td >
                            <telerik:RadComboBox ID="ddlCatalogList" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="ddlCatalogList_SelectedIndexChanged"  
                              Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category"  >
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            
            <%--  <asp:GridView ID="gvAdditionalSpec" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvAdditionalSpec_RowCommand"
                        OnRowDataBound="gvAdditionalSpec_DataBound"
                        OnRowDeleting="gvAdditionalSpec_RowDeleting"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" 
                        AllowSorting="true" onrowediting="gvAdditionalSpec_RowEditing" 
                        onrowcancelingedit="gvAdditionalSpec_RowCancelingEdit" 
                        onrowcreated="gvAdditionalSpec_RowCreated"
                        >
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAdditionalSpec" runat="server" AllowCustomPaging="true"  AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    OnNeedDataSource="gvAdditionalSpec_NeedDataSource"
                    OnItemDataBound="gvAdditionalSpec_ItemDataBound"   
                    OnItemCommand="gvAdditionalSpec_ItemCommand"
                    OnUpdateCommand="gvAdditionalSpec_UpdateCommand"    EnableHeaderContextMenu="true" GroupingEnabled="false" Height="94%">
                   
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDMULTISPECID" TableLayout="Fixed"  >
                        
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

                           <%--<telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="ShortCode">
                             
                                    <HeaderStyle Width="30%" Wrap="true"  />
                                     <ItemStyle HorizontalAlign="Left" Wrap="true"  />
                                   
                                <Itemtemplate>
                                    
                                    <telerik:RadLabel ID="lblMultispecId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMULTISPECID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                </Itemtemplate>
                                <EditItemtemplate>
                                    <telerik:RadLabel ID="lblMultispecIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMULTISPECID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtShortCodeEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'
                                        MaxLength="5"   Width="100%"></telerik:RadTextBox>
                                </EditItemtemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtShortCodeAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="5"    Width="100%"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name">
                               
                                    <HeaderStyle Width="30%" Wrap="true"  />
                                     <ItemStyle HorizontalAlign="Left" Wrap="true"  />
                               
                                <Itemtemplate>
                                    <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </Itemtemplate>
                                <EditItemtemplate>
                                    <telerik:RadLabel ID="lblTypeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtNameEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                        MaxLength="200" ToolTip="Enter Name"    Width="100%"    ></telerik:RadTextBox>
                                </EditItemtemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtNameAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" ToolTip="Enter Name"      Width="100%"    ></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Description">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <Itemtemplate>
                                    <telerik:RadLabel ID="lblDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </Itemtemplate>
                                <EditItemtemplate>
                                    <telerik:RadLabel ID="lblDtkeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                        MaxLength="200" ToolTip="Enter Description" Width="100%"></telerik:RadTextBox>
                                </EditItemtemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="gridinput"
                                        MaxLength="200" ToolTip="Enter Description" Width="100%"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Action" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
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
                                         <span class="icon"><i class="fas fa-trash-alt"></i></span>
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
                 
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>

            </telerik:RadGrid>
     
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>