<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenancesSubWorkOrderReport.aspx.cs"
    Inherits="PlannedMaintenancesSubWorkOrderReport" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%= btnConfirm.UniqueID %>", "");
                }
<%--                else
                {
                    __doPostBack("<%= btnCancel.UniqueID %>", "");
                }--%>

            }
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvWorkOrder.ClientID %>"));
                }, 200);
            }
            function refreshParent() {
                if (typeof parent.CloseUrlModelWindow === "function")
                    parent.CloseUrlModelWindow();
                top.closeTelerikWindow('maintjob', 'maint');
            }
            function deselect(id) {
                var masterTable = $find('<%=gvWorkOrder.ClientID %>').get_masterTableView();
                masterTable.rebind();
            }
        </script>
        <style type="text/css">
            #divWorkorder .RadLabel_Windows7 {
                color: white !important;
                font-weight: bold;
            }

            .RadButton_Windows7.rbCheckBox .rbText {
                color: white !important;
            }

            .tblcollapse table, .tblcollapse td, .tblcollapse th {
                border: 1px solid black;
            }

            .tblcollapse {
                border-collapse: collapse;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body onload="Resize();" onresize="Resize();">
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="radSkinManager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Position="BottomLeft"
            Animation="Fade" AutoTooltipify="true" Width="300px" Font-Size="Large" RenderInPageRoot="true" AutoCloseDelay="80000">           
        </telerik:RadToolTipManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />            
            <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
            <table id="divWorkorder" style="width: 99%; border: 1px; background-color: #1c84c6; padding: 5px; border-color: white; border-bottom-style: solid">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblworkorderNo" Text="WORK ORDER NO:" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" Text="CATEGORY:" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlanDate" Text="PLAN DATE:" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDuration" Text="DURATION:" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblResponsible" Text="ASSIGNED TO:" runat="server"></telerik:RadLabel>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblStatus" Text="STATUS:" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <telerik:RadRadioButtonList ID="rblInProgress" runat="server" OnSelectedIndexChanged="rblPlanned_SelectedIndexChanged" Layout="Flow" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Work Completed" Value="4" />
                            </Items>
                        </telerik:RadRadioButtonList>
                        <telerik:RadRadioButtonList ID="rblPlanned" runat="server" OnSelectedIndexChanged="rblPlanned_SelectedIndexChanged" Layout="Flow" Direction="Horizontal">
                            <Items> 
                                <telerik:ButtonListItem Text="Done" Value="2" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStart" runat="server" Text="Start Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="txtStartDate" runat="server" Enabled="false">
                        </telerik:RadDatePicker>                        
                    </td>    
                     <td >
                        <telerik:RadTimePicker ID="txtStartTime" runat="server" Enabled="false"  TimePopupButton-Visible="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm"  Width="63px">
                        </telerik:RadTimePicker>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompleted" runat="server" Text="End Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="txtCompletedDate" runat="server" Enabled="false">
                        </telerik:RadDatePicker>                        
                    </td>
                    <td>
                        <telerik:RadTimePicker ID="txtCompletedTime" runat="server" TimePopupButton-Visible="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm" Width="63px">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvWorkOrder" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                OnItemCommand="gvWorkOrder_ItemCommand1" EnableViewState="true" OnSortCommand="gvWorkOrder_SortCommand"
                OnDeleteCommand="gvWorkOrder_DeleteCommand" OnItemDataBound="gvWorkOrder_ItemDataBound1" OnPreRender="gvWorkOrder_PreRender"
                OnItemCreated="gvWorkOrder_ItemCreated" AllowMultiRowEdit="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component No. & Name">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>' Visible="false"></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNUMBER") + " - " + DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="285px" HeaderText="Job No, Code and Title" AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDWORKORDERNAME" DataField="FLDWORKORDERNAME" ShowFilterIcon="false" FilterDelay="200">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkorderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkorderGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReportId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERREPORTID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkWorkorderName" runat="server"
                                    Text='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"] + " - " + ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Done" HeaderStyle-Width="150px">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblChk" runat="server" Text="Job Done"></telerik:RadLabel>
                                <br />
                                <telerik:RadCheckBox ID="chkAll" ForeColor="White" runat="server" Text="Check All" OnCheckedChanged="chkAll_CheckedChanged"></telerik:RadCheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadRadioButtonList ID="rblJobDoneStatus" runat="server" OnSelectedIndexChanged="rblJobDoneStatus_SelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="1" Text="Yes"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="2" Text="Defects"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="3" Text="No" Selected="True"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Parameters" HeaderStyle-Width="130px">
                            <ItemStyle Width="100%" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkParameter" runat="server" CommandName="PARAMETER" Text="NA" Enabled="false"></asp:LinkButton>                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Template" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTemplate" runat="server" CommandName="TEMPLATE" Text="NA" Enabled="false"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--                        <telerik:GridTemplateColumn HeaderText="Last Running Hours" HeaderStyle-Width="100px">
                            <ItemTemplate>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadMaskedTextBox ID="txtRunHourEdit" runat="server" Mask="#########.##" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVALUE") %>' Width="100%"></telerik:RadMaskedTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Parts Used" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkpartUsed" runat="server" Text="Show"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Done Date" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <eluc:Date ID="ucDoneDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKDONEDATE") %>' Width="100%" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <telerik:RadTextBox ID="txtCommentEdit" runat="server" TextMode="MultiLine" Width="100%"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAIL") %>'>
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="cmdAtt" ToolTip="Job Attachment" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                                <asp:LinkButton ID="lnkDetails" runat="server" CommandName="cmdDetails" Text="Attachment Reqrd"></asp:LinkButton>
                             </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" Position="Bottom" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table>
            </table>
            <asp:Button ID="btnConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="confirm" OnClick="btnCancel_Click" />
        </telerik:RadAjaxPanel>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                Sys.Application.add_load(function () {
                    setTimeout(function () {
                        TelerikGridResize($find("<%= gvWorkOrder.ClientID %>"));
                    }, 200);
                });
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
