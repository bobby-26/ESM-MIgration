<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetProposalRevision.aspx.cs"
    Inherits="OwnerBudgetProposalRevision" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.OwnerBudget" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
             <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        

    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("ifMoreInfo");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight)-400 + "px";

        }
    </script>

</telerik:RadCodeBlock></head>
<body >
    <form id="frmProposalRevision" runat="server">
    <telerik:RadScriptManager  ID="ToolkitScriptManager1"
        runat="server">
    </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
    <telerik:RadAjaxPanel runat="server" ID="pnlProposalRevision">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <eluc:TabStrip ID="MenuProposalRevision" runat="server" OnTabStripCommand="MenuProposalRevision_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                <table width="100%" cellspacing="1" cellpadding="1">
                    <tr>
                        <td >
                            <iframe runat="server" id="ifMoreInfo"  style="width: 99.8%" scrolling="no" height="260px" ></iframe>
                        </td>
                    </tr>
                    <tr>
                        <td>
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetProposal" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="99.8%" CellPadding="3" OnItemCommand="gvBudgetProposal_ItemCommand" OnItemDataBound="gvBudgetProposal_ItemDataBound"
                                    OnDeleteCommand="gvBudgetProposal_DeleteCommand"  AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                                   OnNeedDataSource="gvBudgetProposal_NeedDataSource" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                         <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
                   <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                                    <Columns>
                                        <telerik:GridTemplateColumn  HeaderText="Revision.No" HeaderStyle-Width="160px" >
                                            <ItemStyle Wrap="False" Width="5%" HorizontalAlign="Left"></ItemStyle>
                                          
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblRevisionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNUMBER") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  HeaderText="Vessel Name" HeaderStyle-Width="160px">
                                            <ItemStyle Wrap="False" Width="30%" HorizontalAlign="Left"></ItemStyle>
                                         
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText=" Proposal Title" HeaderStyle-Width="160px">
                                            <ItemStyle Wrap="False" Width="40%" HorizontalAlign="Left"></ItemStyle>
                                              <ItemTemplate>
                                                <telerik:RadLabel ID="lblProposalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblRevisionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblProposalTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALTITLE") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Proposal Date" HeaderStyle-Width="160px">
                                            <ItemStyle Wrap="False" Width="20%" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblProposalDate" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALDATE", "{0:dd-MMM-yyyy}")%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="160px">
                                            <ItemStyle Wrap="False" Width="20%" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblStatus" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.STATUS")%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText=" Action" HeaderStyle-Width="90px">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                           
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                            
                                                   <asp:LinkButton runat="server" CommandName="APPROVE" ID="cmdApprove" ToolTip="Approve">
                                                   <span class="icon"><i class="fas fa-user-check"></i></span> 
                                                 </asp:LinkButton>
                                               
                                                   <asp:LinkButton runat="server" CommandName="ADD" ID="cmdAdd" ToolTip="Revision">
                                                   <span class="icon"><i class="fas fa-copy"></i></span> 
                                                 </asp:LinkButton>

                                                    
                                                  <asp:LinkButton runat="server" AlternateText="Edit" 
                                                    CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>

                                              
                                                <asp:LinkButton runat="server" AlternateText="Delete" 
                                                    CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                                    ToolTip="Delete">  <span class="icon"><i class="fas fa-trash"></i></span> </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                   
                             </MasterTableView>
                                      <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling  SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                                </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
         
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
