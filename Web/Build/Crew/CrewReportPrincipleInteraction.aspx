<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportPrincipleInteraction.aspx.cs"
    Inherits="CrewReportPrincipleInteraction" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselList" Src="../UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Principle Interaction Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvInteractionList.ClientID %>"));
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
    <form id="frmPrincipleInteraction" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReportNotRelievedOnTime"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="PrincipleInteraction" runat="server" OnTabStripCommand="PrincipleInteraction_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td style="width: 39%;">
                                    <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" CssClass="dropdown_mandatory"
                                        AutoPostBack="true" OnTextChangedEvent="ucPrincipal_TextChangedEvent" AppendDataBoundItems="true"
                                        Width="80%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblApprovalRequired" runat="server" Text="Approval Required">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlApproval" runat="server" AppendDataBoundItems="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="1" Text="Yes"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="0" Text="No"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="dvVessel" class="input" style="overflow: auto; width: 80%; height: 80px">
                            <asp:CheckBoxList runat="server" ID="cblVessel" Height="100%" RepeatColumns="1" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </div>


                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblContactDetails" runat="server" Text="Contact Details for Approval">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContactDetails" runat="server" TextMode="MultiLine"
                            Width="320px" Height="40px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBriefingRequired" runat="server" Text="Briefing Required">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBriefingReq" runat="server" TextMode="MultiLine"
                            Width="320px" Height="40px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMonthlyReporting" runat="server" Text="Monthly Reporting">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMonthlyReporting" runat="server" TextMode="MultiLine"
                            Width="320px" Height="40px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAnyOtherRequirement" runat="server" Text="Any other Requirement">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOtherReq" runat="server" TextMode="MultiLine"
                            Width="320px" Height="40px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuInteractionList" runat="server" OnTabStripCommand="MenuInteractionList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvInteractionList" runat="server"
                EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnEditCommand="gvInteractionList_EditCommand"
                CellSpacing="0" OnNeedDataSource="gvInteractionList_NeedDataSource" OnItemCommand="gvInteractionList_ItemCommand" OnDeleteCommand="gvInteractionList_DeleteCommand"
                GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" OnSortCommand="gvInteractionList_SortCommand"
                OnItemDataBound="gvInteractionList_ItemDataBound" ShowFooter="False" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Sr No" AllowSorting="false" HeaderStyle-Width="40px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Principle" AllowSorting="false" HeaderStyle-Width="120px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPrincipleName" runat="server"
                                    Text='<%#DataBinder.Eval(Container,"DataItem.FLDPRINCIPLENAME") %>' CommandName="EDIT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessels" AllowSorting="false" HeaderStyle-Width="120px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInteractionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINTERACTIONIID") %>'></telerik:RadLabel>
                                <%#DataBinder.Eval(Container, "DataItem.FLDVESSELSHORTCODE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approval" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApproval" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAPPROVALYN").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Contact" AllowSorting="false" HeaderStyle-Width="110px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblContactDetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTACTDETAILS").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDCONTACTDETAILS").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDCONTACTDETAILS").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipContactDetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTACTDETAILS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Briefing" AllowSorting="false" HeaderStyle-Width="120px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBriefing" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBRIEFINGREQUIRED").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDBRIEFINGREQUIRED").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDBRIEFINGREQUIRED").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipBriefing" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBRIEFINGREQUIRED") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Monthly Reporting" AllowSorting="false" HeaderStyle-Width="90px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMonthlyReporting" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHLYREPORTING").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDMONTHLYREPORTING").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDMONTHLYREPORTING").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipMonthlyReporting" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHLYREPORTING") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Any other Specific req" AllowSorting="false" HeaderStyle-Width="90px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOther" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHER").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDOTHER").ToString().Substring(0, 10)+ "..." : DataBinder.Eval(Container, "DataItem.FLDOTHER").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipOther" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last edited by" AllowSorting="false" HeaderStyle-Width="90px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDMODIFIEDBY")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDMODIFIEDDATE","{0:dd/MMM/yyyy}"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT"
                                    Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE"
                                    ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
