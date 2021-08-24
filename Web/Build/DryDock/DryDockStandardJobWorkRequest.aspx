<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockStandardJobWorkRequest.aspx.cs"
    Inherits="DryDockStandardJobWorkRequest" %>

<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Order Done History</title>
    <telerik:RadCodeBlock ID="rad1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
          <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        

                <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand" TabStrip="true"></eluc:TabStrip>
       
                <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
            
                <%--<asp:GridView ID="gvWorkOrder" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvWorkOrder_ItemDataBound"
                        OnRowUpdating="gvWorkOrder_RowUpdating" OnRowCancelingEdit="gvWorkOrder_RowCancelingEdit"
                        OnRowEditing="gvWorkOrder_RowEditing" EnableViewState="false" DataKeyNames="FLDWORKORDERID">
                        <FooterStyle CssClass="datagrid_FooterStyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>                    --%>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                     OnNeedDataSource="gvWorkOrder_NeedDataSource"
                    OnItemDataBound="gvWorkOrder_ItemDataBound1"
                    OnItemCommand ="gvWorkOrder_ItemCommand"    Height="86%"
                    OnUpdateCommand="gvWorkOrder_UpdateCommand" EnableHeaderContextMenu="true"  GroupingEnabled="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView  EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" TableLayout="Fixed" >
                       <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel3" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                        
                        
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Work Order Number"  >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Work Order Title"  >
                                <HeaderStyle Width="200px" />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Component"  >
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%> - <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Priority"   >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Due Date"           >  
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDPLANNINGDUEDATE"])%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status"     >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDSTATUSNAME"] %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadDropDownList ID="ddlStatus" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                        
                                       <Items>
                                           <telerik:DropDownListItem Value="1" Text="Active" />
                                           <telerik:DropDownListItem  Value="0" Text="Cancelled" />
                                           <telerik:DropDownListItem Value="2" Text="Postponed" />
                                       </Items>
                                      
                                    </telerik:RadDropDownList>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDSTATUS"] %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblJobRegister" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDJOBREGISTER"] %>'></telerik:RadLabel>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reason"     >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDREASON"] %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtReason" CssClass="gridinput_mandatory" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDREASON"] %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PostPoned Date" >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                   <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDPOSTPONEDDATE"])%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date id="txtPostponedDate" runat="server" DatePicker="true" Text='<%#((DataRowView)Container.DataItem)["FLDPOSTPONEDDATE"]%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--<asp:TemplateField HeaderText="Discipline">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Discipline ID="ddlDiscipline" runat="server" CssClass="input" AppendDataBoundItems="true" 
                                     DisciplineList='<%#SouthNests.Phoenix.Registers.PhoenixRegistersDiscipline.ListDiscipline()%>'/> 
                                </EditItemTemplate>
                            </asp:TemplateField>--%>
                            <telerik:GridTemplateColumn HeaderText="Completed Satisfactorily"   >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDCOMPSATISFACTORILYNAME"]%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadDropDownList  ID="ddlCompletedStatisfactorilyYN" runat="server" >
                                        <Items>
                                            <telerik:DropDownListItem Value="" Text="--Select--" />
                                            <telerik:DropDownListItem  Value="1" Text="Yes" />
                                            <telerik:DropDownListItem  Value="0" Text="No" />
                                        </Items>
                                                                     
                                    </telerik:RadDropDownList>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Done Date"  >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDWORKDONEDATE"])%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Done By"    >   
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDWORKDONEBY"] %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" Width="3" />                                    
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                        CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                        ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" Width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel" 
                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:"  CssClass=" RadGrid_Default rgPagerTextBox"  />
                    </MasterTableView>
                  <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                 
                </telerik:RadGrid>
         
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
              </telerik:RadAjaxPanel>
    </form>
</body>
</html>
