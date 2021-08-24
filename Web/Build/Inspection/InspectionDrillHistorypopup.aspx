<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDrillHistorypopup.aspx.cs" Inherits="Registers_Historypopup" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vessellist" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Drill History Across all Vessels</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvDrillHistorypopuplist.ClientID %>"));
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
    
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
        DecorationZoneID="gvDrillHistorylist,table1" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
     <eluc:TabStrip ID="Tabstripdrillhistoy" runat="server" OnTabStripCommand="drillhistory_TabStripCommand"
        TabStrip="true"></eluc:TabStrip>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true"/>
         <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvDrillHistorypopuplist" 
        AutoGenerateColumns="false" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvDrillHistorypopuplist_NeedDataSource"
        OnItemDataBound="gvDrillHistorypopuplist_ItemDataBound" >
        <MasterTableView EditMode="InPlace" DataKeyNames="FLDDRILLSCHEDULEID" AutoGenerateColumns="false" EnableColumnsViewState ="false"
            TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true"
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
                <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="vesselcolumn">
                    <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left"  />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblvesselName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText= "Title">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
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
                    <HeaderStyle HorizontalAlign="Center" Width="175px" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblScenario" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSCENARIO")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
             
                     <telerik:GridTemplateColumn HeaderText="Description">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Left"  />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="Radlbldescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'>
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

       
           
                     <telerik:GridTemplateColumn HeaderText="Last Done ">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="Radlbldonedate" runat="server" Text='<%#General.GetDateTimeToString(( DataBinder.Eval(Container, "DataItem.FLDDONEDATE")).ToString())%>'>
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
                        <asp:LinkButton runat="server" ID="btnattachments" ToolTip="Attachments">
                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                AlwaysVisible="true" />
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
