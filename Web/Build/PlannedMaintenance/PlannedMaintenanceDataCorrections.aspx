<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDataCorrections.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDataCorrections" %>

<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vessellist" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= grid.ClientID %>"));
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" >
   

            <table id="table1">
        <tbody>
            <tr>
                <th>
                    <telerik:RadLabel ID="lblvesselname" runat="server" Text="Vessel " />
                </th>
                <th>
                 &nbsp
                </th>
                <td>
                     <eluc:vessellist ID="ddlvessellist" runat="server" Width="250px" CssClass="input" SyncActiveVesselsOnly="true"  ManagementType="FUL" 
                      Entitytype="VSL"  AutoPostBack="true" ActiveVesselsOnly="true" VesselsOnly="true" AssignedVessels="true" OnTextChangedEvent="ddlvessellist_TextChangedEvent"/>
                         
                </td>
                <td>
                    Date
                </td>
                <td>

                </td>
                <td>
                    <eluc:Date ID="raddate" runat="server" CssClass="input"/>
                </td>
            </tr>
            </tbody>
                </table>
             <eluc:TabStrip ID="Tabstrip" runat="server" OnTabStripCommand="Tabstrip_TabStripCommand"
        TabStrip="true"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="grid" 
        AutoGenerateColumns="false" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="grid_NeedDataSource"
       AllowMultiRowSelection="true">
        <MasterTableView EditMode="InPlace" DataKeyNames="FLDID" AutoGenerateColumns="false" EnableColumnsViewState ="false"
            TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableViewState="true"
            InsertItemPageIndexAction="ShowItemOnCurrentPage"  >
           
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
                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridClientSelectColumn >
               <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="vesselcolumn" >
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Left"  />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblvesselName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn HeaderText="Date"  >
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Left"  />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="raddate" runat="server" Text='<%# General.GetDateTimeToString((DataBinder.Eval(Container, "DataItem.FLDDATE")).ToString())%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="ID"  >
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Left"  />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="radid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDID")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderText="Type"  >
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Left"  />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="radtype" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTYPE")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                AlwaysVisible="true" />
        </MasterTableView>
         <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true"  />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
    </telerik:RadGrid>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
