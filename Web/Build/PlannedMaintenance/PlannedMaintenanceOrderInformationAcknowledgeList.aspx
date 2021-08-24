<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceOrderInformationAcknowledgeList.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceOrderInformationAcknowledgeList" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">            
            function Resize() {
                var $ = $telerik.$;
                var height = $(window).height();

                var gvPlanned = $find("<%= gvAckPen.ClientID %>");
                var gvProgress = $find("<%= gvAck.ClientID %>");
                

                var gvPlannedPagerHeight = (gvPlanned.PagerControl) ? gvPlanned.PagerControl.offsetHeight : 0;
                var gvProgressPagerHeight = (gvProgress.PagerControl) ? gvProgress.PagerControl.offsetHeight : 0;
                

                gvPlanned.GridDataDiv.style.height = (Math.round(height / 2) - gvPlannedPagerHeight - 118) + "px";
                gvProgress.GridDataDiv.style.height = (Math.round(height / 2) - gvProgressPagerHeight - 118) + "px";
                
            }
            window.onresize = window.onload = Resize;
            function pageLoad() {
                Resize();
            }
            function refreshParent() {               
                top.closeTelerikWindow('AcknowledgeList', 'maint');
            }  
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuOrderInformationPending" runat="server" OnTabStripCommand="MenuOrderInformation_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvAckPen" runat="server" OnItemCommand="gvAckPen_ItemCommand"
                OnNeedDataSource="gvAckPen_NeedDataSource" OnItemDataBound="gvAckPen_ItemDataBound"
                ShowFooter="false" EnableHeaderContextMenu="true" MasterTableView-ShowFooter="false">
                <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDORDERINFORMATIONID,FLDEMPLOYEEID">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="File No">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDFILENO"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDRANKCODE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                                                                                            
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false" Groupable="false" HeaderStyle-Width="100px">
                            <HeaderStyle />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>                               
                                <asp:LinkButton runat="server" AlternateText="Read & Acknowledge" CommandName="ACK" ID="cmdAck" ToolTip="Read & Acknowledge">
                                    <span class="icon"><i class="fab fa-readme"></i></span>
                                </asp:LinkButton>                              
                            </ItemTemplate>                            
                        </telerik:GridTemplateColumn>
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
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <eluc:TabStrip ID="MenuOrderInformation" runat="server" OnTabStripCommand="MenuOrderInformation_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvAck" runat="server" 
                OnNeedDataSource="gvAck_NeedDataSource"
                ShowFooter="false" EnableHeaderContextMenu="true" MasterTableView-ShowFooter="false">
                <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDORDERINFORATIONACKID">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="File No">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDFILENO"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDRANKCODE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Read On">
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDREADDATE"])%>
                            </ItemTemplate>                            
                        </telerik:GridTemplateColumn>                                                                     
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
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>        
    </form>
</body>
</html>
