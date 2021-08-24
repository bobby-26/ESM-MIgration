<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreCourseCost.aspx.cs" Inherits="CrewOffshoreCourseCost" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Course Cost</title>
   
<telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCourseCost" runat="server">
    
   <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                        <eluc:Title runat="server" ID="ucTitle" Text="Course Cost" Visible="false" />
                    <eluc:TabStrip ID="MnuCourseCost" runat="server" OnTabStripCommand="CourseCost_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
               
                  <table id="tblCourseCost" >
                          <tr>
                              <td>
                                <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                            </td>
                            <td>
                              <telerik:RadComboBox ID="ddlCourse" AutoPostBack="true" runat="server" AllowCustomText="true" EmptyMessage="Type to Select" AppendDataBoundItems="true"  CssClass="input_mandatory" Width="300px">
                                    </telerik:RadComboBox>
                               <%--  <eluc:Address runat="server" ID="ucInstitution" CssClass="dropdown_mandatory"
                                        AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                        AppendDataBoundItems="true" AutoPostBack="true" Width="300px" />        --%>
                            </td>                            
                        </tr>
                    </table>
              
                <br />
                <%--<b><telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel></b>--%>
              
                    <eluc:TabStrip ID="MenuCourseCost" runat="server" OnTabStripCommand="MenuCourseCost_TabStripCommand"    />
              
                   <telerik:RadGrid ID="gvCourseCost" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnItemCommand="gvCourseCost_ItemCommand" OnItemDataBound="gvCourseCost_ItemDataBound"
                        AllowPaging="true" AllowCustomPaging="true" 
                        ShowFooter="true" ShowHeader="true" OnSorting="gvCourseCost_Sorting" AllowSorting="true"
                        EnableViewState="false" OnNeedDataSource="gvCourseCost_NeedDataSource" RenderMode="Lightweight" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"    >                        
                      
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"   >
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                       
                        <Columns>
                        
                            <telerik:GridTemplateColumn   HeaderText="Institute">
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="30%" />
                                        
                                <ItemStyle Wrap="true" HorizontalAlign="Left"  ></ItemStyle>
                               <FooterStyle Wrap="true" HorizontalAlign="Left" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCourseCostId" runat="server" Visible="false" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSECOSTID") %>'></telerik:RadLabel>                                
                                    <asp:LinkButton ID="lnkInstituteName" runat="server" CommandName="EDIT" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTENAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblCourseCostIdEdit" runat="server" Visible="false" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSECOSTID") %>'></telerik:RadLabel>                                   
                                   <eluc:Address runat="server" ID="ucInstitutionEdit" CssClass="dropdown_mandatory"
                                        AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                        AppendDataBoundItems="true" Width="100%" /> 
                                </EditItemTemplate>
                                <FooterTemplate> 
                                 <eluc:Address runat="server" ID="ucInstitutionAdd" CssClass="dropdown_mandatory"
                                        AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                        AppendDataBoundItems="true"  Width="100%" />                                          
                                   <%-- <asp:DropDownList ID="ddlCourseAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="200px">
                                    </asp:DropDownList>--%>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Currency">
                               <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="15%" />
                                        
                                <ItemStyle Wrap="true" HorizontalAlign="Left"  ></ItemStyle>
                               <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrencyName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME")  %>'></telerik:RadLabel>                                  
                                </ItemTemplate>
                                <EditItemTemplate>                                
                                    <telerik:RadLabel ID="lblCurrencyIdEdit" runat="server" Visible="false" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'></telerik:RadLabel>                                   
                                 <eluc:Currency runat="server" ID="ucCurrencyEdit" ActiveCurrency="true" AppendDataBoundItems="true"
                                     CssClass="dropdown_mandatory" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>                                          
                                  <eluc:Currency runat="server" ID="ucCurrencyAdd" ActiveCurrency="true" AppendDataBoundItems="true"
                                     CssClass="dropdown_mandatory" Width="100%" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Cost">
                                 <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="15%" />
                                        
                                <ItemStyle Wrap="true" HorizontalAlign="Left"  ></ItemStyle>
                               <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOST") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number runat="server" ID="txtCost" CssClass="input_mandatory"
                                         Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOST") %>' Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number runat="server" ID="txtCostAdd" CssClass="input_mandatory"
                                        Width="100%"  />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn    HeaderText="Duration (In Days)">
                                 <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="15%" />
                                        
                                <ItemStyle Wrap="true" HorizontalAlign="Left"  ></ItemStyle>
                               <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDuation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number runat="server" ID="txtDuration"  IsInteger="true"
                                         Text='<%# DataBinder.Eval(Container, "DataItem.FLDDURATION") %>' Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number runat="server" ID="txtDurationAdd"            Width="100%"  IsInteger="true"/>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Action"  >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></HeaderStyle>
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
                         />  
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
     
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>