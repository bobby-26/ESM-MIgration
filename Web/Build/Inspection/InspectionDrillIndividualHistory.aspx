<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDrillIndividualHistory.aspx.cs" Inherits="Registers_DrillIndividualHistory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Individual Drill History</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvDrillHistorylist.ClientID %>"));
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
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="0%" />
   
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
        DecorationZoneID="gvDrillHistorylist,table1" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
         <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table id="table1">
        <tbody>
            <tr>
                <th>
                    <telerik:RadLabel ID="Radlblfromdate" runat="server" Text="From Date" />
                </th>
                <th>
                    :
                </th>
                <td>
                    <eluc:Date ID="tbfromdateentry" CssClass="input" runat="server" OnTextChangedEvent="datechange" AutoPostBack="true"  />
                </td>
                <th>
                    <telerik:RadLabel ID="radlbltodate" runat="server" Text="To Date" OnTextChangedEvent="datechange" />
                </th>
                <th>
                    :
                </th>
                <td>
                    <eluc:Date ID="tbtodateentry" CssClass="input" runat="server"  AutoPostBack="true" OnTextChangedEvent="datechange"/>
                </td>
            </tr>
        </tbody>
    </table>

    <eluc:TabStrip ID="Tabstripdrillhistorymenu" runat="server" OnTabStripCommand="drillhistorymenu_TabStripCommand"
        TabStrip="true"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvDrillHistorylist" 
            AutoGenerateColumns="false" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvDrillHistorylist_NeedDataSource"
           OnItemDataBound="gvDrillHistorylist_ItemDataBound">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDDRILLSCHEDULEID" AutoGenerateColumns="false"
                TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState ="false"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" >
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                    ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true" ></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                <Columns>
                      
                <telerik:GridTemplateColumn HeaderText= "Title">
                    <HeaderStyle HorizontalAlign="Center"  />
                    <ItemStyle HorizontalAlign="Left"  />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblDrillName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLNAME")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText = "Period">
                    <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                   
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblDuration" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCY")+" "+DataBinder.Eval(Container, "DataItem.FLDFREQUENCYTYPE")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Scenario">
                    <HeaderStyle HorizontalAlign="Center" Width="175px" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblScenario" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSCENARIO")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Description">
                    <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left"  />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="Radlbldescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
               <telerik:GridTemplateColumn HeaderText=" Last Done Date">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="Radlbldonedate" runat="server" Text='<%#General.GetDateTimeToString(( DataBinder.Eval(Container, "DataItem.FLDDRILLLASTDONEDATE")).ToString())%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn HeaderText= "Remarks">
                    <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left"  />
                    <ItemTemplate>
                        <telerik:RadLabel ID="Radlblremarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

               
               <telerik:GridTemplateColumn HeaderText = "Reason For No Attachments">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left" />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="RADLBLREASON" runat="server" Text='<%# General.GetNullableString((DataBinder.Eval(Container, "DataItem.FLDREASON")).ToString())%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                
                    <telerik:GridTemplateColumn HeaderText="Attachments">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                        <ItemTemplate>
                        
                            <asp:LinkButton runat="server"  ID="btnattachments" ToolTip="Attachments">
                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
                                    </asp:LinkButton>
                        </ItemTemplate>
                        
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
