<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanRelieverNewFilter.aspx.cs" Inherits="CrewPlanRelieverNewFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Plan Reliever Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>   
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselPosition" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewRelieverTabs" runat="server" OnTabStripCommand="CrewRelieverTabs_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFileNo" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankName" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRank" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <telerik:RadTextBox ID="txt1" runat="server" Visible="false"></telerik:RadTextBox>
            <telerik:RadTextBox ID="txt2" runat="server" Visible="false"></telerik:RadTextBox>
            <br />
            <table cellpadding="1" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVessel">
                            <telerik:RadTextBox ID="txtVesselName" runat="server" Width="120px"></telerik:RadTextBox>
                            <img runat="server" id="cmdShowVessel" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListVessel', 'codehelp1', '', 'Common/CommonPickListVesselFilter.aspx', true); " />
                            <telerik:RadTextBox ID="txtVesselid" runat="server" Width="20px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Other Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true"
                            Width="200px" />
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="RelieverMenu" runat="server" OnTabStripCommand="RelieverMenu_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRelieverSearch" runat="server" Height="80%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvRelieverSearch_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvRelieverSearch_ItemDataBound"
                OnItemCommand="gvRelieverSearch_ItemCommand" ShowFooter="false"
                OnSortCommand="gvRelieverSearch_SortCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Wrap="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="On-Signer" Name="On-Signer" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Experience" Name="Experience" ParentGroupName="On-Signer" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="3%" ColumnGroupName="On-Signer">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." ColumnGroupName="On-Signer" HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfileno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEECODE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Off-Signer" AllowSorting="true" UniqueName="RELIVERNAME" ColumnGroupName="On-Signer" ShowSortIcon="true" HeaderStyle-Width="9%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkReliever" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblRelieverName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true" UniqueName="RANK" DataField="FLDEMPLOYEERANKNAME" ColumnGroupName="On-Signer" ShowSortIcon="true" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSuitableDoc" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUITABLEDOC") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRelieverId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPlanned" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNED") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblnewappyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWAPPYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsTop4" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISTOP4") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEERANKNAME") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipAddress" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" UniqueName="RANKEXP" AllowSorting="false" ColumnGroupName="Experience" ShowSortIcon="true" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankExp" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type" UniqueName="VSLTYPE" AllowSorting="false" ColumnGroupName="Experience" ShowSortIcon="true" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVslTypExp" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEEXP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-off Date" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="On-Signer" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOff" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFFDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DOA" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="On-Signer" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDOA" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDOA", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="STATUS" AllowSorting="false" ColumnGroupName="On-Signer" ShowSortIcon="true" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEESTATUS")+"/"+DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>'
                                    ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDSTATUSDESCRIPTION")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" ColumnGroupName="On-Signer" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdEdit" ToolTip="Plan Reliever" CommandName="PLANRELIEVER" CommandArgument="<%# Container.DataSetIndex %>">
                                    <span class="icon"><i class="fas fa-calendar-check"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server"
                                    CommandName="SUITABILITYCHECK" CommandArgument="<%# Container.DataSetIndex %>" ID="imgSuitableCheck"
                                    ToolTip="Suitability Check">
                                 <span class="icon"><i class="fas  fa-user-astronaut"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
