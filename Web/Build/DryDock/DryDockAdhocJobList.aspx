<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockAdhocJobList.aspx.cs" Inherits="DryDock_DryDockAdhocJobList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.DryDock" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Adhoc Jobs</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvAdhocJob.ClientID %>"));
               }, 200);
           }
           window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
           }
        </script>
    </telerik:RadCodeBlock>
</head>
<body >
    <form id="frmAdhocJobList" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
          <telerik:RadAjaxPanel ID="Panel" runat="server">
            <telerik:RadButton ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

         
                <eluc:TabStrip ID="MenuAdocJob" runat="server" OnTabStripCommand="AdhocJob_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
           
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblproject" runat="server" Text="Project Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlproject" runat="server" CssClass="input" >
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblJobNumber" runat="server" Text="Job Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtJobNumber" runat="server" CssClass="input" Width="220px" MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblJobTitle" runat="server" Text="Job Title"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtJobTitle" runat="server" CssClass="input" Width="220px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlJobType" DefaultMessage="--Select--" runat="server" CssClass="input">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
           
                <eluc:TabStrip ID="MenuJob" runat="server" OnTabStripCommand="Job_TabStripCommand"></eluc:TabStrip>
         
         
                <%-- <asp:GridView ID="gvAdhocJob" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvAdhocJob_RowDataBound" OnRowDataBound="gvAdhocJob_ItemDataBound"
                        OnRowEditing="gvAdhocJob_RowEditing" ShowHeader="true" EnableViewState="false" OnSelectedIndexChanging="gvAdhocJob_SelectIndexChanging"
                        AllowSorting="true" OnSorting="gvAdhocJob_Sorting" OnRowDeleting="gvAdhocJob_RowDeleting" >
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAdhocJob" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                   
                    OnNeedDataSource="gvAdhocJob_NeedDataSource"
                    OnItemDataBound ="gvAdhocJob_ItemDataBound1"
                    OnItemCommand="gvAdhocJob_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDPROJECTID" TableLayout="Fixed" CommandItemDisplay="Top" >
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
                            <telerik:GridTemplateColumn>
                                <itemtemplate>
                                <asp:ImageButton ID="cmdAttachments" runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ToolTip="Attachment">
                                </asp:ImageButton>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Project Name">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                              
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblProject" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblProjectid" runat="server" Visible="false" Text ='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTID") %>'></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Number">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                               
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblJobid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkNumber" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:LinkButton>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Title">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>' />
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Section">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                              
                                <itemtemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDJOBTYPE")%>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Job Description">
                                <itemstyle wrap="true" horizontalalign="Left" width="50%"></itemstyle>
                               
                                <itemtemplate>                                    
                                    <telerik:RadLabel ID="lblJobDesc" runat="server" Text=""></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipJobDesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDESCRIPTION") %>' />
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Job Status">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblJobStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                
                                <itemstyle wrap="False" horizontalalign="Center" width="50px"></itemstyle>
                                <itemtemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"  />
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
