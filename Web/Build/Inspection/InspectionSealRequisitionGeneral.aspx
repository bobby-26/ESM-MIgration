<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealRequisitionGeneral.aspx.cs" Inherits="InspectionSealRequisitionGeneral" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Requisition</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmSealReqGeneral" runat="server">
            
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
         <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
         <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuSealReq" runat="server" OnTabStripCommand="MenuSealReq_TabStripCommand"></eluc:TabStrip>

        </div>
       
      
       <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:Status ID="ucStatus" runat="server" />
            <table width="100%" >
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Text="<%#SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.VesselName %>">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" Width="150px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
              
              
                    <td>
                        <telerik:RadLabel ID="lblRequestDate" runat="server" Text="Request Date"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtRequestDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <font color="blue" size="0"><b>
                          <telerik:RadLabel ID="lblNotes" runat="server" Text="Notes:1. To issue seals from office, switch to 'Office'."></telerik:RadLabel>
                                       </b>        
                        </font>
                    </td>
                </tr>
            </table>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSealReqLineItem" runat="server" OnTabStripCommand="MenuSealReqLineItem_TabStripCommand"></eluc:TabStrip>
            </div>

            <%-- <asp:GridView ID="gvSealReqLine" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvSealReqLine_RowDataBound" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true" ShowFooter="true" OnRowEditing="gvSealReqLine_RowEditing"
                        OnSorting="gvSealReqLine_Sorting" OnRowCancelingEdit="gvSealReqLine_RowCancelingEdit"
                        OnRowDeleting="gvSealReqLine_RowDeleting" DataKeyNames="FLDREQUESTLINEID" OnRowCommand="gvSealReqLine_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSealReqLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                OnNeedDataSource="gvSealReqLine_NeedDataSource"
                OnItemCommand="gvSealReqLine_ItemCommand"
                OnItemDataBound="gvSealReqLine_ItemDataBound"
                GroupingEnabled="false" EnableHeaderContextMenu="true"  
              >
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDREQUESTLINEID" TableLayout="Fixed"  Height="10px">
                    <HeaderStyle Width="102px" />
                   <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Seal Type">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                       
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequestId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestlineId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTLINEID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTSTATUSID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatus" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTSTATUS"] %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkSealType" runat="server" CommandName="select"><%# ((DataRowView)Container.DataItem)["FLDSEALTYPENAME"]%></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRequestLineIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTLINEID"] %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTSTATUSID"] %>'></telerik:RadLabel>
                                <eluc:Quick ID="ddlSealTypeEdit" runat="server" AppendDataBoundItems="true" QuickList="<%# PhoenixRegistersQuick.ListQuick(1,87) %>"
                                    QuickTypeCode="87" SelectedQuick='<%# ((DataRowView)Container.DataItem)["FLDSEALTYPE"] %>' CssClass="input_mandatory"
                                    Width="200px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ddlSealTypeAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    Width="200px" QuickTypeCode="87" QuickList="<%# PhoenixRegistersQuick.ListQuick(1,87) %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Requested Qty">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="80px"></ItemStyle>
                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuantity" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDQUANTITY"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtQuantityEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDQUANTITY"]%>'
                                    CssClass="input_mandatory" Width="90px" IsInteger="true" MaskText="####" IsPositive="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtQuantity" runat="server" CssClass="input_mandatory" DefaultZero="false"
                                    Width="90px" IsInteger="true" IsPositive="true" MaskText="####" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Issued Qty from Office">
                            <ItemStyle Wrap="true" HorizontalAlign="Right" Width="80px"></ItemStyle>
                         
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuedQty" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUEDQUANTITY"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Received Qty">
                            <ItemStyle Wrap="true" HorizontalAlign="Right" Width="80px"></ItemStyle>
                         
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceivedQty" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDRECEIVEDQTY"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cancelled Qty">
                            <ItemStyle Wrap="true" HorizontalAlign="Right" Width="80px"></ItemStyle>
                        
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDamagedQty" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCANCELLEDQTY"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received Date">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceivedDate" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDRECEIVEDDATE"]) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtReceivedDateEdit" runat="server" CssClass="input_mandatory" DatePicker="true" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDRECEIVEDDATE"]) %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                               
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"> <span class="icon"><i class="fa fa-trash"></i></span></asp:LinkButton>
                                
                                <asp:LinkButton Width="20px" Height="20px" ID="cmdIssue" runat="server" CommandName="RECORD"
                                    ToolTip="Issue Seals" CommandArgument="<%# Container.DataSetIndex %>" AlternateText="Issue Seals">
                                     <span class="icon"><i class="fas fa-share-alt-square"></i></i></span>
                                </asp:LinkButton>
                               
                                <asp:LinkButton Width="20px" Height="20px" ID="cmdReceive" runat="server" CommandName="RECEIPT"
                                    ToolTip="Receive Seals" CommandArgument="<%# Container.DataSetIndex %>" AlternateText="Receive Seals">
                                     <span class="icon"><i class="fas fa-people-carry"></i></span>
                                </asp:LinkButton>
                                <%--<asp:ImageButton runat="server" AlternateText="Confirm" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                        CommandName="CONFIRM" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdConfirm"
                                        ToolTip="Confirm Receipt"></asp:ImageButton>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                    CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                               
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                    CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                    ToolTip="Add New">
                                     <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

             <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes"
            CancelText="No" />


        </telerik:RadAjaxPanel>

      

    </form>
</body>
</html>
