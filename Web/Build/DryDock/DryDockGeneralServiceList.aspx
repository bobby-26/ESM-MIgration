<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockGeneralServiceList.aspx.cs"
    Inherits="DryDockGeneralServiceList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvGeneralService.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmGeneralServiceList" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
       
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadButton ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

           
                <eluc:TabStrip ID="MenuStandardGeneralService" runat="server" OnTabStripCommand="StandardGeneralService_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
            <table Width="70%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblJobNumber" runat="server" Text="Job Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtJobNumber" runat="server" Width="220px" MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblJobTitle" runat="server" Text="Job Title"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtJobTitle" runat="server" Width="220px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlJobType" runat="server" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select sub category">             </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
          
                <eluc:TabStrip ID="MenuGeneralService" runat="server" OnTabStripCommand="GeneralService_TabStripCommand"></eluc:TabStrip>
           
            <telerik:RadGrid RenderMode="Lightweight" ID="gvGeneralService" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" 
                CellSpacing="0" GridLines="None" 
                OnItemDataBound="gvGeneralService_ItemDataBound"    
                OnNeedDataSource="gvGeneralService_NeedDataSource"
                OnItemCommand="gvGeneralService_ItemCommand"      EnableHeaderContextMenu="true"  GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY" TableLayout="Fixed"   >
                   
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
                        <telerik:GridTemplateColumn HeaderText="Number" ItemStyle-Width ="10Px">
                              <HeaderStyle Width="20%"  Wrap="true" />           
                          
                        
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" ItemStyle-Width ="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              <HeaderStyle Width="20%"  Wrap="true" />           
                            <%-- <HeaderTemplate>
                                    <asp:LinkButton ID="lnkTitle" runat="server" CommandName="Sort" CommandArgument="FLDTITLE"
                                        ForeColor="White">Title</asp:LinkButton>
                                    <img id="FLDTITLE" runat="server" visible="false" />
                                </HeaderTemplate>--%>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTitle" runat="server" CommandName="Select"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Type" ItemStyle-Width ="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              <HeaderStyle Width="20%"   Wrap="true" />           
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDJOBTYPE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Description" ItemStyle-Width ="45%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                              <HeaderStyle Width="20%"  Wrap="true" />           
                            <ItemTemplate>
                                 <telerik:RadLabel ID="lblJobDescription" runat="server" Text='Details' ClientIDMode="AutoID"></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucJobDescription"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDESCRIPTION") %>' TargetControlId="lblJobDescription" />
                              
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Select" ItemStyle-Width ="5%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="5%"></ItemStyle>
                              <HeaderStyle Width="20%"  Wrap="true" />           
                            <ItemTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkSelectedYN" Text="" BackColor="Transparent" CommandName="SELECTJOB" />
                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                                      <HeaderStyle Width="10%"  HorizontalAlign="center" Wrap="true" />           
                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                    CommandName="EDIT" ID="cmdEdit" 
                                    ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                               
                                <asp:LinkButton runat="server" AlternateText="Distribute" 
                                    CommandName="DISTRIBUTE" ID="cmdDistribute" Width ="20PX" Height="20PX"
                                    ToolTip="Distribute">
                                    <span class="icon"><i class="fas fa-shipping-fast"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"  />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
         </telerik:RadAjaxPanel>
        <%--   <Triggers>
                <asp:PostBackTrigger ControlID="gvGeneralService" />
            </Triggers>--%>
    </form>
</body>
</html>
