<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewEmployeeGrowthList.aspx.cs"
    Inherits="CrewEmployeeGrowthList" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Growth List</title>
       <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvEmployeeGrowthList.ClientID %>"));
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
    <form id="frmEmployeeGrowthList" runat="server" submitdisabledcontrols="true">
    
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
           
                        <table id="tblConfigureEmployeeGrowthList" >
                        <tr>
                            <td style="padding-left:10px;padding-right:15px">
                                <telerik:radlabel ID="lblRecruitedFromDate" runat="server" Text="Recruited From Date"></telerik:radlabel>
                            </td>
                            <td style="padding-right:15px">
                                <eluc:Date ID="ucFromDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                            </td>
                           <td style="padding-right:15px">
                                <telerik:radlabel ID="lblRecruitedToDate" runat="server" Text="Recruited To Date"></telerik:radlabel>
                            </td>
                           <td style="padding-right:15px">
                                <eluc:Date ID="ucToDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
               
                        <eluc:tabstrip id="MenuEmployeeGrowthList" runat="server" ontabstripcommand="EmployeeGrowthList_TabStripCommand">
                    </eluc:tabstrip>
                

                            <telerik:RadGrid ID="gvEmployeeGrowthList" runat="server" AutoGenerateColumns="False" Font-Size="11px" ShowFooter="false"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvEmployeeGrowthList_ItemCommand"
                OnItemDataBound="gvEmployeeGrowthList_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvEmployeeGrowthList_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black"   ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>


                            <telerik:gridtemplatecolumn headertext="Emp No">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:radlabel>
                                    
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                  <telerik:radlabel ID="lblEmpid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:radlabel>
                                    <asp:LinkButton ID="lnkName" runat="server" CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Recruited Rank">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblRecreuitedRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLIEDRANK") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                             <telerik:gridtemplatecolumn headertext="Current Rank">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                
                                 <ItemTemplate>
                                    <telerik:radlabel ID="lblPostedRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTEDRANK") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                              <telerik:gridtemplatecolumn headertext="Batch">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                
                                  <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDBATCH")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                              <telerik:gridtemplatecolumn headertext="1st join date">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                
                                  <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDDATEOFJOINING")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="Nationality">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                              <telerik:gridtemplatecolumn headertext="Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                  <ItemTemplate>
                                    <telerik:radlabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                         </Columns>
                       
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