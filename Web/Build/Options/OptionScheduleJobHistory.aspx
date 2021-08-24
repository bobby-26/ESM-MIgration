<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionScheduleJobHistory.aspx.cs" Inherits="OptionScheduleJobHistory" %>

<!DOCTYPE html>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Schedule Jobs</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
               <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
         <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server" Height="100%">
         <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <eluc:TabStrip ID="Menujobhistory" runat="server" OnTabStripCommand="Menujobhistory_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvSchedulejobHistory" Height="94%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvSchedulejobHistory_NeedDataSource"
            OnItemDataBound="gvSchedulejobHistory_ItemDataBound">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>
                    
                       <telerik:GridTemplateColumn HeaderText='Name'>
                           <HeaderStyle Width="60%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbljobid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.JOB_ID") %>'></telerik:RadLabel>
                         <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.NAME") %>'></telerik:RadLabel>

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText='Enabled (Y/N)'>
                        <HeaderStyle Width="20%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblenable" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.ENABLED").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%" AllowSorting='true' HeaderTooltip="Action">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/WRH-Default-Work-Hours.png %>"
                                CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.JOB_ID") %>' ID="cmdEdit"
                                ToolTip="Job History" Width="15PX">
                                  
                            </asp:ImageButton>
                             <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/multiselect-item.png %>"
                                CommandName="EDIT1" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.JOB_ID") %>' ID="cmdEdit1"
                                ToolTip="Job Server" Width="15PX">
                                    
                            </asp:ImageButton>

                        </ItemTemplate>
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
     