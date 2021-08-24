<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsEmployeeQuery.aspx.cs"
    Inherits="VesselAccountsEmployeeQuery" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew List / Rest Hour</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">            
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvCrewSearch.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
            <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" Height="99%" runat="server" EnableViewState="true"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCrewSearch_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvCrewSearch_ItemDataBound"
                OnItemCommand="gvCrewSearch_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="true" ShowSortIcon="true">
                            <HeaderStyle Width="5%" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDROWNUMBER"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrhempid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOUREMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRHstartid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOURSTARTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblShipCalendarId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCALENDARID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEployeeName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>' CommandName="EDIT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSIGNONRANKCODE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="12%" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDPASSPORTNO"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CDC No." AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="12%" />
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDSEAMANBOOKNO"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign On" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Relief Due" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action">
                            <HeaderStyle Width="150px" HorizontalAlign="Center" />
                            <ItemStyle Width="150px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton runat="server" AlternateText="View Contract"
                                    CommandName="CONTRACT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdGenContract"
                                    ToolTip="View Contract">
                                <span class="icon"><i class="fas fa-file"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Reset" CommandName="WORKHOURS" ID="cmdWorkHours" ToolTip="Scheduled Work Hours">
                                    <span runat="server" class="icon"><i class="fas fa-clock" runat="server" id="workhourIcon" ></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attendance" CommandName="CREWATTENDANCE" ID="cmdCrewAttendance" ToolTip="Work and Rest Hour Records">
                                    <span class="icon"><i class="fas fa-user-clock"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="CR6B Report" CommandName="CR6BREPORT" ID="cmdReport" ToolTip="CR6B Report">
                                    <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="CR6C Report" CommandName="CR6CREPORT" ID="cmdCR6CReport" ToolTip="CR6C Report" Visible="false">
                                    <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Allow Sign Off" CommandName="SIGNOFF" ID="cmdSignOff" ToolTip="Allow Review Before Month End">
                                    <span runat="server" class="icon"><i class="fas fa-award" runat="server" id="I1" ></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Apply Validations" CommandName="SIGNON" ID="cmdSignOn" ToolTip="Do not allow to Review Before Month End">
                                    <span runat="server" class="icon"><i class="fas fa-times-circle-cancel" runat="server" id="I2" ></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="800px" Height="600px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
             VisibleStatusbar="false" KeepInScreenBounds="true" NavigateUrl="../VesselAccounts/VesselAccountsRHDesignation.aspx" ReloadOnShow="true" ShowContentDuringLoad="false">            
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
