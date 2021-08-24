<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulation.aspx.cs" Inherits="Inspection_InspectionRegulation" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Regulation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvNewRegulations.ClientID %>"));
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
      <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCourseListEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

  <telerik:RadAjaxPanel runat="server" ID="pnlCourseListEntry"> 


        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvNewRegulations" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvNewRegulations_NeedDataSource" Width="100%" 
                OnItemDataBound="gvNewRegulations_ItemDataBound" OnItemCommand="gvNewRegulations_ItemCommand" EnableHeaderContextMenu="true" GroupingEnabled="false">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDREGULATIONID">
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

                        <telerik:GridTemplateColumn HeaderText="Issued Date" AllowSorting="true" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="85px" />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE","{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date  ID="txtIssuedDateEdit"  runat="server"  DatePicker="true"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE","{0:dd-MM-yyyy}") %>'/> 
                                <telerik:RadLabel ID="lbldtkeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Issued By" AllowSorting="true">
                            <HeaderStyle Width="118px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                              <EditItemTemplate>
                                <telerik:RadTextBox ID="txtIssuedByEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDBYNAME") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="MOC Ref.No" AllowSorting="true" HeaderStyle-Wrap="false">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMOCTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCREFERENCENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                              <EditItemTemplate>
                                <telerik:RadTextBox ID="txtMOCTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCREFERENCENO") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Title" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDTITLE">
                            <HeaderStyle Width="118px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRegulationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGULATIONID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDREGULATIONID") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtTitleEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Due Date" AllowSorting="true" ShowSortIcon="true" UniqueName="Duedate"   SortExpression="FLDDUEDATE">
                            <HeaderStyle Width="85px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuedate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDUEDATE", "{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                               <EditItemTemplate>
                                <telerik:RadDatePicker ID="txtDuedateEdit" runat="server" SelectedDate='<%# DataBinder.Eval(Container,"DataItem.FLDDUEDATE") %>'></telerik:RadDatePicker>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vessel Type" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDVESSELTYPE">
                            <HeaderStyle Width="110px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesseltype" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                               <EditItemTemplate>
                                  <eluc:VesselType   runat="server" ID="chkvesselListEdit" />
                                  <telerik:RadLabel ID="lblVesselTypeEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Description" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDDESCRIPTION">
                            <HeaderStyle Width="118px" Wrap="true" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                               <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action Required" Visible="true" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDACTIONREQUIRED">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActionRequied" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONREQUIRED") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONREQUIRED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtActionRequiredEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONREQUIRED") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Remarks" Visible="true" AllowSorting="true" ShowSortIcon="true" >
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="true" ShowSortIcon="true" >
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>

                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDREGULATIONID") %>' ID="cmdDelete"
                                    ToolTip="Delete Regulation"  >
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attach"
                                    CommandName="ATTACHMENT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDREGULATIONID") %>' ID="cmdAttachment"
                                    ToolTip="Regulation Attachment"  >
                                        <span class="icon"><i class="fa fa-paperclip"></i></span>
                                </asp:LinkButton>
                                 <asp:LinkButton runat="server" AlternateText="Compliance Status"
                                     CommandName="VIEW" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDREGULATIONID") %>' ID="cmdComplianceStatus"
                                     ToolTip="Compliance Status" >
                                     <span class="icon"><i class="fas fa-file-alt"></i></span>
                                 </asp:LinkButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="UPDATE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDREGULATIONID") %>' ToolTip="Save" Width="20px" >
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="CANCEL" ToolTip="Cancel" Width="20px" >
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
