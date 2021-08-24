<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDailyWorkPlanMemberTimeSheet.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDailyWorkPlanMemberTimeSheet" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
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
                     <telerik:GridTemplateColumn HeaderText="From">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblfrom" runat="server" Text='<%# PadZero((DataBinder.Eval(Container, "DataItem.FLDSTARTTIME")).ToString())%>' Visible="false">
                                </telerik:RadLabel>
                                 <telerik:RadLabel ID="Radlblfromactual" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSTARTTIME"), DateDisplayOption.DateTimeHR24)%>' Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="radid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMEMBERTIMELINEID")%>' Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                         <EditItemTemplate>
                                <telerik:RadComboBox runat="server" ID="Radfromedit" CssClass="input_mandatory" ></telerik:RadComboBox>
                              <telerik:RadLabel ID="radid2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMEMBERTIMELINEID")%>' Visible="false">
                                </telerik:RadLabel>
                         </EditItemTemplate>
                            <FooterTemplate>
                               
                                     <telerik:RadComboBox runat="server" ID="Radfromentry" CssClass="input_mandatory"></telerik:RadComboBox>
                            
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                              
                                <telerik:RadLabel ID="Radlblto" runat="server"  Text='<%# PadZero((DataBinder.Eval(Container, "DataItem.FLDENDTIME").ToString()))%>' Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="radtoactual" runat="server"  Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDENDTIME"), DateDisplayOption.DateTimeHR24)%>' Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox runat="server" ID="Radtoedit" CssClass="input_mandatory" ></telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox runat="server" ID="radtotimeentry" CssClass="input_mandatory"></telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION" >
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
             <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <span class="icon"><i class="fa-exclamation-orange"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblinstruction" runat="server" Text="* This record has been edited by user"></telerik:RadLabel>
                    </td>
                    
                </tr>
            </table>
             </telerik:RadAjaxPanel>
    </form>
</body>
</html>
