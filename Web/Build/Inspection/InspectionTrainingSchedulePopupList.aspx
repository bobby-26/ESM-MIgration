<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionTrainingSchedulePopupList.aspx.cs" Inherits="Inspection_InspectionTrainingSchedulePopupList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Training Due across Vessels</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvTrainingSchedulelist.ClientID %>"));
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
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
          <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
         <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="Trainingschedule_TabStripMenuCommand" TabStrip="true">
            </eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
   <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvTrainingSchedulelist" 
            AutoGenerateColumns="false" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvTrainingSchedulelist_NeedDataSource"
            OnItemDataBound="gvTrainingSchedulelist_ItemDataBound">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDTRAININGONBOARDSCHEDULEID" AutoGenerateColumns="false" EnableColumnsViewState ="false"
                TableLayout="Fixed" ShowHeadersWhenNoRecords="true"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" >
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
                    <telerik:GridTemplateColumn HeaderText="Vessel Name">
                        <HeaderStyle HorizontalAlign="Center" Width="106px" Font-Bold="true"/>
                        <ItemStyle HorizontalAlign="Left"  />                      
                        <ItemTemplate>
                            <telerik:RadLabel ID="RadlblvesselName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Title">
                        <HeaderStyle HorizontalAlign="Center" Width="106px" Font-Bold="true"/>
                        <ItemStyle HorizontalAlign="Left"  />                      
                        <ItemTemplate>
                            <telerik:RadLabel ID="RadlblTrainingName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAININGNAME")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Interval">
                        <HeaderStyle HorizontalAlign="Center"  Width="78px" Font-Bold="true"/>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="RadlblDuration" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCY")+" "+DataBinder.Eval(Container, "DataItem.FLDFREQUENCYTYPE")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Fixed/Variable">
                        <HeaderStyle HorizontalAlign="Center" Width="76px" Font-Bold="true"/>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="Radlblfixedorvariable" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFIXEDORVARIABLE")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Type">
                        <HeaderStyle HorizontalAlign="Center"  Width="111px" Font-Bold="true"/>
                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="Radlbltype" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTYPE")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Due">
                        <HeaderStyle HorizontalAlign="Center" Width="78px" Font-Bold="true"/>
                        <ItemStyle HorizontalAlign="Center"  />                       
                        <ItemTemplate>
                            <telerik:RadLabel ID="RadlblDueDate" runat="server" Text='<%#  General.GetDateTimeToString((DataBinder.Eval(Container, "DataItem.FLDTRAININGONBOARDDUEDATE")).ToString())%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>                         
                    </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done">
                        <HeaderStyle HorizontalAlign="Center" Width="158px" Font-Bold="true"/>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="RadlblLastdonedate" runat="server" Text='<%#  General.GetDateTimeToString((DataBinder.Eval(Container, "DataItem.FLDTRAININGLASTDONEDATE")).ToString())%>'>
                            </telerik:RadLabel>
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
