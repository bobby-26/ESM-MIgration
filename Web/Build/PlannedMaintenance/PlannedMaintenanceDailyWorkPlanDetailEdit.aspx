<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDailyWorkPlanDetailEdit.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDailyWorkPlanDetailEdit" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ import Namespace="SouthNests.Phoenix.Registers" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>        
         <script type="text/javascript">             
             function CloseModelWindow(gridid) {
                 if (typeof parent.CloseUrlModelWindow === "function")
                     parent.CloseUrlModelWindow(gridid);
             }
             
                 function PaneResized(sender, args) {
            
            var browserHeight = $telerik.$(window).height();
                 var grid = $find("gvCrewWorkHrs");
                 var height = browserHeight - 250;
                 grid._gridDataDiv.style.height = height + "px";
                 
             }
        function pageLoad() {
                     PaneResized();
        }

            var onCloseJson = {};
            onCloseJson.onClose = function () {
                document.getElementById("cmdHiddenSubmit").click();
            }
            
        </script>
         <style>
             .rgSelectedRow {
                 background-image : none !important;
                 border-color : transparent !important;
                 box-shadow:none !important;
                 background-color:transparent  !important;
             }
             
         </style>
     </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        
        <telerik:RadToolTipManager RenderMode="Lightweight" ID="RadToolTipManager1" runat="server" Position="BottomCenter"
            Animation="Fade" AutoTooltipify="false" Width="300px" RenderInPageRoot="true">
        </telerik:RadToolTipManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
         <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MainMenu_TabStripCommand"></eluc:TabStrip>   
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div id="divEdit" runat="server" >
        
            <table border="0" style="width: 100%" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblElement" runat="server" Text="Process"></telerik:RadLabel>                                                
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtElement" runat="server" ReadOnly="true" Enabled="false"
                            MaxLength="200" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td rowspan="4" style="vertical-align: top">
                        <telerik:RadGrid ID="gvMembers" runat="server"  EnableViewState="true"
            OnNeedDataSource="gvMembers_NeedDataSource" OnItemDataBound="gvMembers_ItemDataBound" OnItemCommand="gvMembers_ItemCommand"
            ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" >
            <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" 
                CommandItemDisplay="None"
                DataKeyNames="FLDMEMBERID"
                 >
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
                     <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="20px">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                           
                            <ItemTemplate>
                               <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false">
                                 <span class="icon" id="imgFlagcolor"  ><i class="fa-exclamation-orange"></i></span>      
                                </asp:LinkButton>
                                <telerik:RadLabel ID="radisedited" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUSEREDITED")%>' Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-HorizontalAlign="Left" >
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblmemberid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMEMBERID")%>' Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblsignoffid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSIGNONOFFID")%>' Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblactivityid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIVITYID")%>' Visible="false">
                                </telerik:RadLabel>
                                 <telerik:RadLabel ID="lblcatid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDUTYCATEGORYID")%>' Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="150Px" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrank" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRANK")%>'>
                                </telerik:RadLabel>
                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Schedule Category" HeaderStyle-Width="290Px" HeaderStyle-HorizontalAlign="Left">
                           
                            <ItemStyle HorizontalAlign="Left"  />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblschedulecategory" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CATNAME")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ddlcat" runat="server"   HardList='<%# PhoenixRegistersHardExtn.ListHardExtn(1,281,0,null)%>'  SelectedHard='<%#(DataBinder.Eval(Container,"DataItem.FLDDUTYCATEGORYID"))%>' HardTypeCode="281" Width="250px" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    
                      <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                           <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit"  CommandName="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                               &nbsp &nbsp
                                <asp:LinkButton runat="server" AlternateText="Timesheet" ID="cmdTimesheet" CommandName="TIME" ToolTip="Timesheet" Visible="false">
                                        <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                    </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <asp:LinkButton runat="server" ID="btnupdate" ToolTip="Update"  CommandName="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                             &nbsp &nbsp
                              <asp:LinkButton runat="server" ID="btncancel" ToolTip="Cancel"  CommandName="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                         
                        </telerik:GridTemplateColumn>

                     </Columns>
            </MasterTableView>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                    AlwaysVisible="true" />
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px" />
            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
                        <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <span class="icon"><i class="fa-exclamation-orange"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblinstruction" runat="server" Text="* This record timings are edited by user"></telerik:RadLabel>
                    </td>
                    
                </tr>
            </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActivity" runat="server" Text="Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtActivity" runat="server" ReadOnly="true" Enabled="false"
                            MaxLength="200" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEstStart" runat="server" Text="Est Start Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlEstStartTime"></telerik:RadComboBox>
                        <telerik:RadLabel ID="radlblstarttime" runat="server"  Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDuration" runat="server" Text=" Est End Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlDuration"></telerik:RadComboBox>
                         <telerik:RadLabel ID="radlblendtime" runat="server"  Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                   <tr>
                       <td>
                           <telerik:RadLabel ID="lblnote" runat="server" Text="Note"></telerik:RadLabel>
                       </td>
                       <td colspan="2">
                           <telerik:RadLabel ID="lblnotes" runat="server" Text="Select team members by clicking on checkboxes below and click the save button ;"  Font-Bold="true"></telerik:RadLabel>
                                                      <telerik:RadLabel ID="RadLabel1" runat="server" Text="Team members cannot be added to the plan if their default workhours are not configured."  Font-Bold="true"></telerik:RadLabel>

                       </td>
                   </tr>
            </table>

            <br />
            
        </div>
   

        <telerik:RadGrid ID="gvCrewWorkHrs" runat="server" MasterTableView-ShowFooter="false"  OnPreRender="gvCrewWorkHrs_PreRender"
                    OnNeedDataSource="gvCrewWorkHrs_NeedDataSource" OnItemDataBound="gvCrewWorkHrs_ItemDataBound" 
                    ShowFooter="false" AllowCustomPaging="false" AllowPaging="false" EnableHeaderContextMenu="true" AllowMultiRowSelection="true">
                    <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" >
                        
                        <Columns>
                             
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridClientSelectColumn>
                            
                           <telerik:GridTemplateColumn Visible="false">
                               <ItemTemplate>
                                   <telerik:RadLabel runat="server" ID="lblsignonoffid" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEESIGNONOFFID")%>'>
                                       </telerik:RadLabel>
                               </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Select Team Member" UniqueName="FLDEMPLOYEENAME" DataField="FLDEMPLOYEENAME" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle Width="15%"  />
                                <ItemStyle Width="15%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Rank" UniqueName="FLDRANKNAME" DataField="FLDRANKNAME" HeaderStyle-HorizontalAlign="Left" >
                                <HeaderStyle Width="7%"   />
                                <ItemStyle Width="7%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="00-01" UniqueName="1" DataField="1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="01-02" UniqueName="2" DataField="2"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="02-03" UniqueName="3" DataField="3"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="03-04" UniqueName="4" DataField="4"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="04-05" UniqueName="5" DataField="5"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="05-06" UniqueName="6" DataField="6"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="06-07" UniqueName="7" DataField="7"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="07-08" UniqueName="8" DataField="8"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="08-09" UniqueName="9" DataField="9"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="09-10" UniqueName="10" DataField="10"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="10-11" UniqueName="11" DataField="11"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="11-12" UniqueName="12" DataField="12"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="12-13" UniqueName="13" DataField="13"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="13-14" UniqueName="14" DataField="14"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="14-15" UniqueName="15" DataField="15"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="15-16" UniqueName="16" DataField="16"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="16-17" UniqueName="17" DataField="17"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="17-18" UniqueName="18" DataField="18"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="18-19" UniqueName="19" DataField="19"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="19-20" UniqueName="20" DataField="20"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="20-21" UniqueName="21" DataField="21"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="21-22" UniqueName="22" DataField="22"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="22-23" UniqueName="23" DataField="23"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="23-24" UniqueName="24" DataField="24"></telerik:GridBoundColumn>

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
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
  </telerik:RadAjaxPanel>
    </form>
</body>
</html>
