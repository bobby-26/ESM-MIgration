<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreProposalHistory.aspx.cs" Inherits="CrewOffshoreProposalHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Proposal History</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmProposalHistory" runat="server" submitdisabledcontrols="true">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <eluc:TabStrip ID="CrewCompExpTab" Title="Proposal List" runat="server"></eluc:TabStrip>

                <div id="divProposalHistory" style="position: relative; z-index: +1">
                    <table id="tblProposalHistory" width="100%">
                        <tr>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />

                <eluc:TabStrip ID="MenuProposalHistory" runat="server" OnTabStripCommand="ProposalHistory_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: +1">
                    <%-- <asp:GridView ID="gvProposalHistory" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="false" ShowHeader="true" OnRowCommand="gvProposalHistory_RowCommand"
                    EnableViewState="false" OnRowDataBound="gvProposalHistory_RowDataBound">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvProposalHistory" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvProposalHistory_NeedDataSource"
                        OnItemCommand="gvProposalHistory_ItemCommand"
                        OnItemDataBound="gvProposalHistory_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="Comments" Name="Comments" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
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
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Waived" ColumnGroupName="Comments">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="50px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblWD" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWAIVEDDOCUMENTLIST").ToString() %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="imgWD" runat="server" CommandArgument='<%# Container.DataSetIndex %>'>
                                        <span class="icon"><i class="fas fa-glasses"></i></span>
                                        </asp:LinkButton>
                                        <eluc:ToolTip ID="ucToolTipWD" Width="450px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDWAIVEDDOCUMENTLIST") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rejection" ColumnGroupName="Comments">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgRC" runat="server" CommandName="NAVIGATE" CommandArgument='<%# Container.DataSetIndex %>'>
                                         <span class="icon"><i class="fas fa-glasses"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblVesselHeader" runat="server" Text="Vessel"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblCrewPlanID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblCrewPlanDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="200px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Updated By">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblUpdatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUPDATEDBY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Updated Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblUpdatedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Document Checklist" HeaderStyle-Width="5px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Document Checklist"
                                            CommandName="DOCUMENTCHECKLIST" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDocChecklist"
                                            ToolTip="Documents Checklist">
                                        <span class="icon"> <i class="fas fa-tasks"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

                <%--            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblWD" Visible="" runat="server" Text=" * WD - Waived Documents"></telerik:RadLabel>
                    </td>
                </tr>
            </table>--%>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
