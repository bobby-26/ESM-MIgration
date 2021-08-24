<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVesselDrillOverDue.aspx.cs" Inherits="Inspection_InspectionVesselDrillOverDue" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Over Due Drills</title>
  <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvDrillvsVessels.ClientID %>"));
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
        DecorationZoneID="gvDrillvsVessels" DecoratedControls="All" EnableRoundedCorners="true" />
  
   
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
         <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" >
              <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
    <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="drillvsvessels_TabStripMenuCommand" TabStrip="true">
            </eluc:TabStrip>
    <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvDrillvsVessels" AutoGenerateColumns="false"
        AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvDrillvsVessels_NeedDataSource"
        OnItemDataBound="gvDrillvsVessels_ItemDataBound"  >
        <MasterTableView EditMode="InPlace" DataKeyNames="FLDDRILLSCHEDULEID" AutoGenerateColumns="false"
            TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState ="false"
            InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="true">
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
                <telerik:GridTemplateColumn HeaderText="Drill ">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblDrillName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLNAME")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText=" Due on ">
                    <HeaderStyle HorizontalAlign="Center"  Width="100PX" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="Radlblduedate" runat="server" Text='<%#General.GetDateTimeToString((DataBinder.Eval(Container, "DataItem.FLDDRILLDUEDATE")).ToString())%>'>
                        </telerik:RadLabel>
                      
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderText=" Overdue by ">
                    <HeaderStyle HorizontalAlign="Center" Width="338px" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                      
                        <a id="Radlblduein" runat="server" >
                           <%# DataBinder.Eval(Container, "DataItem.DUEIN")%></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

            </Columns>
            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                AlwaysVisible="true" />
        </MasterTableView>
        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"  ColumnsReorderMethod="Reorder">
                    
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="370px"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
    </telerik:RadGrid>
             </telerik:RadAjaxPanel>
   
    </form>
</body>
</html>
