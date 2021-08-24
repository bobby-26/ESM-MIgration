<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersPICsupplier.aspx.cs" EnableEventValidation="false"
    Inherits="RegistersPICsupplier" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Mobile" Src="~/UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Phone" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Designation" Src="~/UserControls/UserControlDesignation.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Communication Details</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
     <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegisterVesselCommunication" runat="server">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
             <eluc:TabStrip ID="MenuPicVessel" runat="server" OnTabStripCommand="MenuPicVessel_TabStripCommand">
            </eluc:TabStrip>
      
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                </td>
                 <td width="25%" colspan="2">
                <span id="spnPickListSupMaker">
                   
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false"
                            Width="60px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false"
                            Width="180px"></telerik:RadTextBox>
                        <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                            style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInvoiceType" runat="server" Text="Invoice Type"></telerik:RadLabel>
                </td>
                <td>
                    <%-- <eluc:Hard ID="ddlInvoiceType" runat="server" AppendDataBoundItems="true" AutoPostBack="TRUE"
                                CssClass="input" HardTypeCode="59" Width="300px" />--%>
                     <telerik:RadComboBox RenderMode="Lightweight"  CssClass="input_mandatory" runat="server"  Width="300px" ID="ddlInvoiceType" OnSelectedIndexChanged="ddlInvoiceTypeSelectedindexchange" AutoPostBack="true" EnableLoadOnDemand="true">
                      </telerik:RadComboBox>
                  <%--  <asp:DropDownList ID="ddlInvoiceType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceTypeSelectedindexchange"
                        CssClass="input_mandatory" Width="300px">
                    </asp:DropDownList>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></telerik:RadLabel>
                </td>
                <td>
                        <telerik:RadComboBox RenderMode="Lightweight"  CssClass="input_mandatory" runat="server"  Width="300px" ID="ddlInvoiceStatus"  AutoPostBack="true" EnableLoadOnDemand="true">
                      </telerik:RadComboBox>
                    <%--<asp:DropDownList ID="ddlInvoiceStatus" runat="server" CssClass="input_mandatory" Width="300px" AutoPostBack="true" >
                    </asp:DropDownList>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPIC" runat="server" Text="PIC"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserName ID="ucPIC" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                        UserNameList="<%# PhoenixUser.UserList()%>" Width="300px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblChooseafile" runat="server" Text="Upload File "></telerik:RadLabel>
                </td>
                <td>
                    <asp:FileUpload ID="txtFileUpload1" runat="server" CssClass="input_mandatory" Width="300px" />
                    
                </td>
                  <td>
                
                </td>
                <td>
                   <asp:LinkButton ID="lnkDownloadExcel" runat="server" Text="Download Template" OnClick="lnkDownloadExcel_Click"></asp:LinkButton> 
                </td>
            </tr>
        </table>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" Visible="false" />
   
           
            <eluc:TabStrip ID="MenuPCIAdmin" runat="server" OnTabStripCommand="MenuPCIAdmin_TabStripCommand">
            </eluc:TabStrip>
      
            
             <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvVesselAdminUser" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselAdminUser" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvVesselAdminUser_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" Height="62%" GroupingEnabled="false" OnSelectedIndexChanging="gvVesselAdminUser_SelectedIndexChanging"
                    OnItemDataBound="gvVesselAdminUser_ItemDataBound" OnItemCommand="gvVesselAdminUser_ItemCommand"
                    ShowFooter="false" ShowHeader="true" OnSortCommand="gvVesselAdminUser_SortCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >


                <Columns>
                <%--    <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />--%>
                      <telerik:GridTemplateColumn HeaderText="Supplier Address">
                     
                        <ItemTemplate>
                           <%--<asp:Label ID="lblSupplierid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>--%>
                            <telerik:RadLabel ID="lblSupplierid" runat="server" ></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Supplier Name">
                       
                        <ItemTemplate>
                            <%--<asp:Label ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>--%>
                            <telerik:RadLabel ID="lblSupplierName" runat="server" ></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Supplier Code" >
                       
                        <ItemTemplate>
                            <%--<asp:Label ID="lblSuppliercode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:Label>--%>
                            <telerik:RadLabel ID="lblSuppliercode" runat="server"></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Invoice Type">
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInvoiceTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Invoice Status" >
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInvoiceStautsName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Designation Name" AllowFiltering="true" SortExpression="FLDID">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblVesselIdHeader" runat="server" Visible="true">
                            </telerik:RadLabel>
                            
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDesignationname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblDesignationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDesignationnameedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                        <%--<FooterTemplate>
                                    <eluc:Designation ID="ucDesignation" runat="server" AppendDataBoundItems="true" CssClass="gridinput_mandatory"
                                        DesignationList="<%#PhoenixRegistersDesignation.ListDesignation()%>" />
                                </FooterTemplate>--%>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Person In Charge" AllowSorting="true" SortExpression="FLDSUPPLIERCODE">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                       
                        <ItemTemplate>
                            <telerik:RadLabel ID="lnkSupplierId" runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICUSERNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel Visible="false" ID="lblVesselAdminUserMapCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELADMINUSERMAPCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel Visible="false" ID="lblVesselAdminUserMapCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELADMINUSERMAPCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel Visible="false" ID="lblDesignationInvoiceId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONINVOICEID") %>'></telerik:RadLabel>
                            <eluc:UserName ID="ucPICEdit" runat="server" AppendDataBoundItems="true" CssClass="gridinput_mandatory"
                                SelectedUser='<%# DataBinder.Eval(Container,"DataItem.FLDPICUSERID") %>' UserNameList="<%# PhoenixUser.UserList()%>" />
                        </EditItemTemplate>
                        <%--<FooterTemplate>
                                    <eluc:UserName ID="ucPIC" runat="server" AppendDataBoundItems="true" CssClass="gridinput_mandatory"
                                        UserNameList="<%# PhoenixUser.UserList()%>" />
                                </FooterTemplate>--%>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                     
                        <ItemStyle HorizontalAlign="Center"  Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataSetIndex %>"
                                CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>" ToolTip="Edit" />
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <%--<asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                                CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Delete" />--%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataSetIndex %>"
                                CommandName="Update" ImageUrl="<%$ PhoenixTheme:images/save.png %>" ToolTip="Save" />
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                           <%-- <asp:ImageButton ID="cmdCancel" runat="server" AlternateText="Cancel" CommandArgument="<%# Container.DataItemIndex %>"
                                CommandName="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Cancel" />--%>
                        </EditItemTemplate>
                        <%--<FooterStyle HorizontalAlign="Center" />--%>
                        <%--<FooterTemplate>
                                    <asp:ImageButton ID="cmdAdd" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>" ToolTip="Add New" />
                                </FooterTemplate>--%>
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
