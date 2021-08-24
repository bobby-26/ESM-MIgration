<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDailyWorkPlanMemberActualTimeSheet.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDailyWorkPlanMemberActualTimeSheet" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>     

          <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvMemberTimeSheet.ClientID %>"));
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
    
       
         <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
             
        <table>
            <tr>
                <td>
                    Name
                </td>
                <td>
                    <telerik:RadLabel runat="server" ID="radlblname" />
                    <telerik:RadLabel runat="server" ID="radactlblstarttime"  Visible="false"/>
                    <telerik:RadLabel runat="server" ID="radactlblendtime"  Visible="false"/>
                </td>
            </tr>
        </table>
              <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
     <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvMemberTimeSheet" AutoGenerateColumns="false" EnableHeaderContextMenu ="true"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvMemberTimeSheet_NeedDataSource" 
            OnItemDataBound="gvMemberTimeSheet_ItemDataBound" OnItemCommand="gvMemberTimeSheet_ItemCommand" ShowFooter="true">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDMEMBERTIMELINEID" AutoGenerateColumns="false" 
                TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="false" >
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                 <Columns>
                     <telerik:GridTemplateColumn HeaderText="From">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblfrom" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTARTTIME")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="radid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMEMBERTIMELINEID")%>' Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        <EditItemTemplate>
                                <telerik:RadDateTimePicker ID="txtstartDateTimeedit" runat="server" Width="200px"  AutoPostBackControl="Calendar" SelectedDate='<%# DataBinder.Eval(Container, "DataItem.FLDSTARTTIME")%>'>
                                    
                    </telerik:RadDateTimePicker>
                             <telerik:RadLabel ID="radid2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMEMBERTIMELINEID")%>' Visible="false">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                 <telerik:RadDateTimePicker ID="txtstartDateTimeentry" runat="server" Width="200px"  AutoPostBackControl="Calendar" PopupDirection="TopRight" >
                    </telerik:RadDateTimePicker>
                                </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtendDateTime" runat="server"  Text='<%# DataBinder.Eval(Container, "DataItem.FLDENDTIME")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDateTimePicker ID="txtendDateTimeedit" runat="server" Width="200px"  AutoPostBackControl="Calendar" SelectedDate='<%# DataBinder.Eval(Container, "DataItem.FLDENDTIME")%>'>
                    </telerik:RadDateTimePicker>
                            </EditItemTemplate>
                            <FooterTemplate>
                                 <telerik:RadDateTimePicker ID="txtendDateTimeentry" runat="server" Width="200px"  AutoPostBackControl="Calendar" PopupDirection="TopRight">
                    </telerik:RadDateTimePicker>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" >
                            <FooterStyle HorizontalAlign="Center" Font-Bold="true"/>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit"  CommandName="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                 &nbsp &nbsp
                              <asp:LinkButton runat="server" ID="btndelete" ToolTip="Delete"  CommandName="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
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
                            <FooterTemplate>
                                <asp:LinkButton runat="server" ID="btnsave" ToolTip="Add"  CommandName="Add">
                                            <span class="icon"><i class="fas fa-plus-circle"></i></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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