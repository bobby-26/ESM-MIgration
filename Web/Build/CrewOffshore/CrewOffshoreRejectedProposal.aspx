<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreRejectedProposal.aspx.cs" Inherits="CrewOffshoreRejectedProposal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Query Activity</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="PD_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 10; width: 100%;">
                    <%--<asp:GridView ID="gvSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnSelectedIndexChanging="gvSearch_SelectedIndexChanging" Width="100%" CellPadding="3" ShowHeader="true"
                        OnRowDataBound="gvSearch_RowDataBound"
                        EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvSearch" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvSearch_NeedDataSource"
                        OnItemCommand="gvSearch_ItemCommand"
                        OnItemDataBound="gvSearch_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                    
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                            <HeaderStyle Width="102px" />
                            <Columns>
                               
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <HeaderStyle Width="75px" />
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                        Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                    <HeaderStyle Width="150px" />
                                    <itemtemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name">
                                    <HeaderStyle Width="150px" />
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                    <itemtemplate>
                                    <asp:LinkButton ID="lnkemployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Daily Rate (USD)">
                                    <HeaderStyle Width="50px" />
                                    <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblDailyRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAILYRATEUSD")%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Daily DP Allowance (USD)">
                                    <HeaderStyle Width="75px" />
                                    <itemstyle wrap="False" horizontalalign="Right"></itemstyle>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblDPRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPALLOWANCE")%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Planned Relief">
                                    <HeaderStyle Width="75px" />
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                    <headertemplate>
                                    <telerik:RadLabel ID="lblPlannedReliefHeader" runat="server">Planned Relief</telerik:RadLabel>
                                </headertemplate>
                                    <itemtemplate>
                                    <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "")) %>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Off-Signer">
                                    <HeaderStyle Width="150px" />
                                    <itemtemplate>
                                    <asp:LinkButton ID="lnkOffsignerEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblOffsignerName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME")%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                           
                                <telerik:GridTemplateColumn HeaderText="End of Contract">
                                    <itemtemplate>
                                    <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", ""))%>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Proposed By">
                                    <HeaderStyle Width="75px" />
                                    <itemtemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPROPOSEDBY")%>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Proposal Remarks">
                                    <itemstyle wrap="true" width="100px" horizontalalign="Left"></itemstyle>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Width="300px" Style="word-wrap: break-word;"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS")%>'></telerik:RadLabel>
                                    <%--<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS")%>--%>
                                    <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS")%>' />
                                </itemtemplate>
                                    <edititemtemplate>
                                    <telerik:RadLabel ID="lblInterviewId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWID") %>'></telerik:RadLabel>
                                    <asp:TextBox ID="txtMumbaiRemarks" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                </edititemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <HeaderStyle Width="100px" />
                                    <itemtemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <headerstyle horizontalalign="Center" verticalalign="Middle" Width="50px"></headerstyle>
                                  
                                    <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                    <itemtemplate>
                                    <asp:LinkButton runat="server" AlternateText="Approval Rejection Comments" 
                                        CommandName="UPDATECOMMENTS" CommandArgument='<%# Container.DataSetIndex %>' ID="imgComments"
                                        ToolTip="Approval Rejection Comments">
                                        <span class="icon"><i class="fas fa-clipboard"></i></span>
                                    </asp:LinkButton>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

            </div>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
