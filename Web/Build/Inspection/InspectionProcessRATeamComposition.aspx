<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionProcessRATeamComposition.aspx.cs" Inherits="InspectionProcessRATeamComposition" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Team Composition</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvOperationalHazard.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmMOC" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuOperational" runat="server" OnTabStripCommand="Operational_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />            
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOperationalHazard" runat="server" Height="91%" AllowCustomPaging="false"
                CellSpacing="0" EnableViewState="true" GridLines="None" Font-Size="11px"
                OnNeedDataSource="gvOperationalHazard_NeedDataSource" OnItemDataBound="gvOperationalHazard_ItemDataBound" OnItemCommand="gvOperationalHazard_ItemCommand" ShowFooter="False" ShowHeader="true" Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDRAACTIVITYTEAMCOMPOSITIONID">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Deparment" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeparment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Function" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFunction" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUNCTIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Group Ranks" HeaderStyle-Width="30%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartmentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblfunctionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUNCTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbllevelid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcombositionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAACTIVITYTEAMCOMPOSITIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPRANKLIST") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankGroupList" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPRANK") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="STCW Level" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%" VerticalAlign="Top"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No.of People" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <eluc:Number ID="txtcount" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERSONCOUNT") %>' MaxLength="3" IsInteger="true" IsPositive="true" DecimalPlace="0" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" >
                            <ItemStyle Wrap="true" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Team Composition" CommandName="GROUPRANKMAPPING" ID="cmdRankGroupMapping" ToolTip="Map Group Rank">
                                                            <span class="icon"><i class="fas fa-user-tie"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="500px" Height="365px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
            VisibleStatusbar="false" KeepInScreenBounds="true">
            <ContentTemplate>
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1" OnAjaxRequest="RadAjaxPanel2_AjaxRequest">
                    <table border="0" style="width: 100%">
                        <tr>
                            <td valign="top" width="10%">
                                <telerik:RadLabel ID="lblequipment" runat="server" Text="Group Rank"></telerik:RadLabel>
                            </td>
                            <td valign="top" width="50%">
                                <telerik:RadCheckBoxList ID="chkRankGroup" runat="server" Columns="2" ></telerik:RadCheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <telerik:RadButton ID="btnCreate" Text="Save" runat="server" OnClick="btnCreate_Click"></telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </telerik:RadAjaxPanel>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>

