<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionTrainingSchedule.aspx.cs" Inherits="Inspection_InspectionTrainingSchedule" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vessellist" Src="~/UserControls/UserControlVesselCommon.ascx" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Training Schedule</title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       <%-- <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvTrainingSchedulelist.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>--%>
        <script type="text/javascript">
            function confirmCallBackFn(arg) {
               
                if (arg )
                    document.getElementById("btnconfirm").click();
                
            }
            </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
       
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvTrainingSchedulelist,table1" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:TabStrip ID="TabstripTrainingschedule" runat="server" OnTabStripCommand="TabstripTrainingschedulee_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <table id="table1">
                <tbody>
                    <tr>
                        <th>
                            <asp:Label ID="lblvesselname" runat="server" Text="Vessel " />
                        </th>
                        <th>&nbsp
                        </th>
                        <td>
                            <eluc:vessellist ID="ddlvessellist" runat="server" Width="250px" CssClass="input" SyncActiveVesselsOnly="true" ManagementType="FUL"
                                Entitytype="VSL" AutoPostBack="true" ActiveVesselsOnly="true" OnTextChangedEvent="ddlvessellist_textchange" VesselsOnly="true"  AssignedVessels="true"/>
                        </td>
                        <td align="right">
                            Over Due &nbsp
                            
                        </td>
                        <td >
                            <telerik:RadCheckBox ID="radoverdue" runat="server"  AutoPostBack="true" OnCheckedChanged="radoverdue_CheckedChanged"/>
                        </td>
                    </tr>
                </tbody>
            </table>
            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
                EnableShadow="true">
            </telerik:RadWindowManager>
            <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="TabstripMenu_TabStripMenuCommand" TabStrip="true"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvTrainingSchedulelist"  EnableLinqExpressions="false" EnableViewState="true" AllowFilteringByColumn="true"
                AutoGenerateColumns="false" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvTrainingSchedulelist_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true" OnSortCommand="gvTrainingSchedulelist_Sort"
                OnItemDataBound="gvTrainingSchedulelist_ItemDataBound" AllowSorting="true"  Height="88%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" DataKeyNames="FLDTRAININGONBOARDSCHEDULEID" AutoGenerateColumns="false" EnableColumnsViewState="false" AllowNaturalSort="false"
                    TableLayout="Fixed" ShowHeadersWhenNoRecords="true"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage">
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
                        <telerik:GridTemplateColumn HeaderText="" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="3%" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false">
                                 <span class="icon" id="imgFlagcolor"  ><i class="fas fa-star-red"></i></span>      
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="FLDTRAININGNAME" UniqueName="FLDTRAININGNAME" FilterControlWidth="120px" FilterDelay="2000"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains" AllowSorting="false">
                            <HeaderStyle HorizontalAlign="Center" Width="106px" Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <a id="TrainingNameanchor" runat="server" style="text-decoration: none; color: black">
                                    <%# DataBinder.Eval(Container, "DataItem.FLDTRAININGNAME")%></a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Interval" AllowFiltering="false" AllowSorting="false">
                            <HeaderStyle HorizontalAlign="Center" Width="78px" Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="RadlblDuration" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCY")+" "+DataBinder.Eval(Container, "DataItem.FLDFREQUENCYTYPE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fixed/Variable" AllowFiltering="false" AllowSorting="false">
                            <HeaderStyle HorizontalAlign="Center" Width="76px" Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblfixedorvariable" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFIXEDORVARIABLE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" AllowFiltering="false" AllowSorting="false">
                            <HeaderStyle HorizontalAlign="Center" Width="111px" Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlbltype" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTYPE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due" AllowFiltering="false" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDTRAININGONBOARDDUEDATE">
                            <HeaderStyle HorizontalAlign="Center" Width="78px" Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="RadlblDueDate" runat="server" Text='<%#  General.GetDateTimeToString((DataBinder.Eval(Container, "DataItem.FLDTRAININGONBOARDDUEDATE")).ToString())%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done" AllowFiltering="false" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDTRAININGONBOARDLASTDONEDATE">
                            <HeaderStyle HorizontalAlign="Center" Width="158px" Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="RadlblLastdonedate" runat="server" Text='<%#  General.GetDateTimeToString((DataBinder.Eval(Container, "DataItem.FLDTRAININGONBOARDLASTDONEDATE")).ToString())%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false" AllowSorting="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="reportbtn" ToolTip="Report" Visible="false">
                                            <span class="icon"><i class="fas fa-clipboard-list"></i></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" ID="btnhistory" ToolTip="History">
                                            <span class="icon"><i class="fas fa-history"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                  <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit"  Visible="false">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
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
             <asp:Button ID="btnconfirm" runat="server" Text="confirm" OnClick="confirm_Click"  CssClass="hidden"  />
             <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <span class="icon" "><i class="fas fa-star-yellow"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltrainingyettosubmit" runat="server" Text="* Training Yet to be Confirmed"></telerik:RadLabel>
                    </td>
                    <td>
                        <span class="icon" "><i class="fas fa-star-red"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbloverduetraining" runat="server" Text="* Training Over Due"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
             </telerik:RadAjaxPanel>
           
       
    </form>
</body>
</html>
