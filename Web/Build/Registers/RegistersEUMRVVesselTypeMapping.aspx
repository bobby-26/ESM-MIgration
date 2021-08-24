<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersEUMRVVesselTypeMapping.aspx.cs"
    Inherits="RegistersEUMRVVesselTypeMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Direction </title>
   <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegisterVesselDirection" runat="server">
       <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                      <%--  <eluc:Title runat="server" ID="ucTitle" Text="Vessel Type Mapping" Visible="false"></eluc:Title>--%>
                    <table width="100%">
                        <tr>
                          
                             <td><telerik:RadLabel ID="ltrlCode" runat="server" Text="Code"></telerik:RadLabel></td>
                            <td><telerik:RadTextBox ID="txtCode" runat="server" Width="120px" CssClass="input"></telerik:RadTextBox></td>
                            <td><telerik:RadLabel ID="ltrlEUVesselType" runat="server" Text="EU Vessel Type"></telerik:RadLabel></td>
                            <td><telerik:RadTextBox ID="txtEUVesselType" runat="server" Width="120px" CssClass="input"></telerik:RadTextBox></td>
                            <td><telerik:RadLabel ID="ltrVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel></td>
                            <td><telerik:RadTextBox ID="txtVesselType" runat="server" Width="120px" CssClass="input"></telerik:RadTextBox></td>
                        
                        </tr>
                    </table>
                
                    <eluc:TabStrip ID="MenuRegistersEUVesselType" runat="server" OnTabStripCommand="RegistersEUVesselType_TabStripCommand">
                    </eluc:TabStrip>
                
                        <telerik:RadGrid ID="gvEUVesselType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnItemCommand="gvEUVesselType_ItemCommand" OnItemDataBound="gvEUVesselType_ItemDataBound"
                        
                        AllowSorting="true" OnSorting="gvEUVesselType_Sorting" ShowFooter="true"
                        ShowHeader="true" EnableViewState="false"
                      AllowPaging="true" AllowCustomPaging="true" GridLines="None" 
                        OnNeedDataSource="gvEUVesselType_NeedDataSource" RenderMode="Lightweight"
                        GroupingEnabled="false" EnableHeaderContextMenu="true">
                       
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"    >
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
                            
                            <telerik:GridTemplateColumn FooterText="EU Vessel Type"  AllowSorting="true" SortExpression="FLDCODE" HeaderText="Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="25%" />
                                <ItemTemplate>
                                     <telerik:RadLabel ID="lblEUVesselTypeCode" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                                     <telerik:RadLabel ID="lblEUVesselTypeid" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVVESSELTYPEID") %>'></telerik:RadLabel>
                                     <telerik:RadLabel ID="lblVesselTypeid" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEID") %>'></telerik:RadLabel>
                                     <telerik:RadLabel ID="lblVesselTypeMapid" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEMAPPINGID") %>'></telerik:RadLabel>
                                </ItemTemplate>   
                                 
                                  <EditItemTemplate>
                                   <telerik:RadTextBox ID="txtEUVesselTypeCodeEdit" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>' runat="server" CssClass="gridinput_mandatory" MaxLength="50" />
                                   <telerik:RadLabel ID="lblEUVesselTypeidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVVESSELTYPEID") %>'></telerik:RadLabel>
                                   <telerik:RadLabel ID="lblVesselTypeidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEID") %>'></telerik:RadLabel>
                                   <telerik:RadLabel ID="lblVesselTypeMapidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEMAPPINGID") %>'></telerik:RadLabel>
                                </EditItemTemplate>

                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtEUVesselTypeCodeAdd" runat="server" Width="98%" CssClass="gridinput_mandatory" MaxLength="50"  ToolTip="Enter Code" />    
                                </FooterTemplate>
                                
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn FooterText="New Short"  HeaderText="EU Vessel Type"  AllowSorting="true" SortExpression="FLDVESSELTYPE" >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="25%" />                                                   
                                <ItemTemplate>                                   
                                    <telerik:RadLabel ID="lblEUVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                
                                <EditItemTemplate>
                                    <telerik:RadComboBox ID="ddlEUVesselTypeEdit" runat="server" CssClass="gridinput_mandatory" Width="98%"></telerik:RadComboBox>
                                </EditItemTemplate>
                                
                                <FooterTemplate>
                                        <telerik:RadComboBox ID="ddlEUVesselTypeAdd" runat="server" CssClass="gridinput_mandatory"  Width="100%" ></telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn FooterText="New Short"   AllowSorting="true" SortExpression="FLDTYPEDESCRIPTION" HeaderText="Vessel Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                   <HeaderStyle Width="40%" />
                                <ItemTemplate>                                   
                                    <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                
                                <EditItemTemplate>
                                    <eluc:VesselType ID="ucVesselTypeEdit" runat="server" VesselTypeList='<%#PhoenixRegistersVesselType.ListVesselType(0)%>'
                                        CssClass="gridinput_mandatory" Width="100%" AppendDataBoundItems="true" />
                                </EditItemTemplate>
                                
                                <FooterTemplate>
                                        <eluc:VesselType ID="ucVesselTypeAdd" runat="server" VesselTypeList='<%#PhoenixRegistersVesselType.ListVesselType(0)%>'
                                        CssClass="gridinput_mandatory" Width="100%" AppendDataBoundItems="true" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>

                             <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
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
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
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
                        ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
     
    </telerik:radajaxpanel>
    </form>
</body>
</html>
