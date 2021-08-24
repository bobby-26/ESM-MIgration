<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanRelievee.aspx.cs" Inherits="CrewPlanRelievee" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Relief Plan</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvSearch.ClientID %>"));
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewRelieverTabs" runat="server" OnTabStripCommand="CrewRelieverTabs_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="QueryMenu" runat="server" OnTabStripCommand="QueryMenu_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSearch" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvSearch_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvSearch_ItemDataBound"
                OnItemCommand="gvSearch_ItemCommand" ShowFooter="false" OnDeleteCommand="gvSearch_DeleteCommand"
                OnSortCommand="gvSearch_SortCommand" AutoGenerateColumns="false">
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
                        <telerik:GridColumnGroup HeaderText="Off-Signer" Name="Off-Signer" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="On-Signer(Reliever)" Name="On-Signer" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Experience" Name="Experience" ParentGroupName="Off-Signer" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Contract" Name="Contract" ParentGroupName="Off-Signer" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Relief" Name="Relief" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" ColumnGroupName="Off-Signer" HeaderStyle-Width="3%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true" UniqueName="RANK" DataField="FLDRANKNAME" ShowSortIcon="true" ColumnGroupName="Off-Signer" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsignonoffid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesseltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentsReq" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTSREQUIRED") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAppType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALTYPE") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSupt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPT") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsTop4" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISTOP4") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJoinDate" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" UniqueName="VESSELNAME" DataField="FLDVESSELNAME" ShowSortIcon="true" ColumnGroupName="Off-Signer" HeaderStyle-Width="9%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" UniqueName="NAME" DataField="FLDNAME" ShowSortIcon="true" ColumnGroupName="Off-Signer" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblName" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkRelievee" CommandName="SELECTROW" CommandArgument='<%# Container.DataSetIndex %>' runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone" AllowSorting="false" UniqueName="ZONE" ShowSortIcon="true" ColumnGroupName="Off-Signer" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOffsignerZone" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERZONE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" UniqueName="RANKEXP" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="Experience" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankExp" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERDECIMALEXPERIENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vsl.Typ" UniqueName="VSLTYPE" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="Experience" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVslTypExp" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDRELIVEEVSLTYPEDECIMALEXPERIENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry On" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="Contract" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpOn" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDEXPRIEDDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due" UniqueName="DUEDATE" AllowSorting="true" DataField="FLDRELIEFDUEDATE" ShowSortIcon="true" ColumnGroupName="Relief" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReliefDue" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned" UniqueName="PLANNED" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="Relief" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlanned" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" UniqueName="RELIEVERNAME" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="On-Signer" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRelieverId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERID") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkReliever" CommandName="SELECTROW" CommandArgument='<%# Container.DataSetIndex %>' runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" UniqueName="RELIEVERRANK" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="On-Signer" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRelieverRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERRANKID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRelieverRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERRANK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank Exp" HeaderStyle-Wrap="true" UniqueName="RELIEVERRANKEXP" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="On-Signer" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRelieverRankExp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERRANKID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReliever" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERDECIMALEXPERIENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PD Status" UniqueName="PDSTATUS" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="On-Signer" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPDStatus" runat="server" CssClass="tooltip" ClientIDMode="AutoID"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblPDStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUSID")%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipAddress" TargetControlId="lblPDStatus" runat="server" Text='<%#HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDAPPROVALREMARKS").ToString()) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned Port" HeaderStyle-Wrap="true" UniqueName="PLANNEDPORT" AllowSorting="false" ShowSortIcon="true" ColumnGroupName="On-Signer" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlannedPort" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" ColumnGroupName="On-Signer" HeaderStyle-Width="13%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDITPLAN" CommandArgument="<%# Container.DataSetIndex %>">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="Delete" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="De-Plan">
                                    <span class="icon"><i class="fas fa-calendar-times"></i></span> 
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Show"
                                    CommandName="SHOW" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdShow"
                                    ToolTip="Show Reliever">
                                <span class="icon"><i class="fas fa-user-circle"></i></span>
                                </asp:LinkButton>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" />
                                <asp:LinkButton runat="server" AlternateText="PD Form"
                                    CommandName="PDFORM" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdPDForm"
                                    ToolTip="PD Form">
                                <span class="icon"><i class="fas fa-file"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Checklist"
                                    CommandName="CHECKLIST" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdChkList"
                                    ToolTip="Checklist">
                                <span class="icon"><i class="fas fa-list-ul"></i></span>
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
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>                      
                        <img id="Img2" src="<%$ PhoenixTheme:images/blue.png%>" runat="server" />
                    </td>
                    <td>* OffSigners Planned
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
