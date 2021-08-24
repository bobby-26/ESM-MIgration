<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersPICvessel.aspx.cs"  EnableEventValidation="false"
    Inherits="RegistersPICvessel" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="70%">

                <eluc:TabStrip ID="MenuPicVessel" runat="server" OnTabStripCommand="MenuPicVessel_TabStripCommand">
            </eluc:TabStrip>
       
           
  
        <table cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel Name"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" VesselsOnly="true" AppendDataBoundItems="true"
                        Width="300px" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInvoiceType" runat="server" Text="Invoice Type"></telerik:RadLabel>
                </td>
                <td>
                  
                     <telerik:RadComboBox RenderMode="Lightweight"  CssClass="input_mandatory" runat="server"  Width="300px" ID="ddlInvoiceType" OnSelectedIndexChanged="ddlInvoiceTypeSelectedindexchange" AutoPostBack="true" EnableLoadOnDemand="true">
                      </telerik:RadComboBox>
                 <%--   <asp:DropDownList ID="ddlInvoiceType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceTypeSelectedindexchange"
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
                   <%-- <asp:DropDownList ID="ddlInvoiceStatus" runat="server" CssClass="input_mandatory" Width="300px" AutoPostBack="true">
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
                     <asp:LinkButton ID="lnkDownloadExcel" runat="server" Text="Download Template" OnClick="lnkDownloadExcel_Click"></asp:LinkButton>
                </td>
                  <td>
               
                </td>
                <td>
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
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvVesselAdminUser_SelectedIndexChanging"
                    OnItemDataBound="gvVesselAdminUser_ItemDataBound" OnItemCommand="gvVesselAdminUser_ItemCommand" Height="84%"
                    ShowFooter="false" ShowHeader="true" OnSortCommand="gvVesselAdminUser_SortCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >

                  <Columns>
                  <%--  <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />--%>
                    <telerik:GridTemplateColumn HeaderText="Vessel ID">
                        
                        <ItemTemplate>
                         
                            <telerik:RadLabel ID="lblvesselid" runat="server"></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name">
                       
                        <ItemTemplate>
                         
                           <telerik:RadLabel ID="lblvesselName" runat="server" ></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Code">
                        
                        <ItemTemplate>
                         
                            <telerik:RadLabel ID="lblvesselcode" runat="server"></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Invoice Type">
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInvoiceTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                   <telerik:GridTemplateColumn HeaderText="Invoice Status">
                        
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInvoiceStautsName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Designation Name" AllowSorting="true" SortExpression="FLDID">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderTemplate>
                            <asp:Label ID="lblVesselIdHeader" runat="server" Visible="true">
                            </asp:Label>
                           <%-- <asp:Label ID="lnkVesselIdHeader" runat="server" CommandArgument="FLDID" CommandName="Sort"
                                ForeColor="White">Designation Name&nbsp;</asp:Label>
                            <img id="VesselAbbreviation" runat="server" visible="false" />--%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDesignationnames" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblDesignationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDesignationname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    
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
                     
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataSetIndex %>"
                                CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>" ToolTip="Edit" />
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                       
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataSetIndex %>"
                                CommandName="Update" ImageUrl="<%$ PhoenixTheme:images/save.png %>" ToolTip="Save" />
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                        
                        </EditItemTemplate>
                    
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



