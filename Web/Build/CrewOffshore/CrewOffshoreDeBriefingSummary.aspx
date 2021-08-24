<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDeBriefingSummary.aspx.cs" Inherits="CrewOffshoreDeBriefingSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew De-Briefing Summary</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="PD_TabStripCommand"></eluc:TabStrip>


                    <%-- <asp:GridView ID="gvSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCommand="gvSearch_RowCommand" OnSelectedIndexChanging="gvSearch_SelectedIndexChanging"
                        Width="100%" CellPadding="3" ShowHeader="true"
                        OnRowDataBound="gvSearch_RowDataBound"
                        EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvSearch" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
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

                                <telerik:GridTemplateColumn HeaderText="Sent">
                                    <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSentdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENTDATE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Received">
                                    <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSignonoffId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblreceiveddate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBRIEFINGDATE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblcomplaintid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWCOMPLAINTID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="File No">
                                    <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%--<asp:LinkButton ID="lnkemployee"  CommandArgument="<%# Container.DataSetIndex %>" CommandName="EDITROW" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>--%>
                                        <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                        <telerik:RadLabel ID="lblempname" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>

                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="From">
                                    <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To">
                                    <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblToDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNOFFDATE", "")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblstatusid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Email sent">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEmailcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAILCOUN") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle Width="100px" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Export XL" 
                                            CommandName="GETEMPLOYEE" CommandArgument='<%# Container.DataSetIndex %>'
                                            ID="cmdCrewExport2XL" ToolTip="View De-Briefing">
                                            <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                        </asp:LinkButton>
                                       
                                        <asp:LinkButton runat="server" AlternateText="Review" 
                                            CommandName="APPROVE" CommandArgument='<%# Container.DataSetIndex %>' ID="imgApprove"
                                            ToolTip="Review Done">
                                            <span class="icon"><i class="fas fa-award"></i></span>
                                        </asp:LinkButton>
                                       
                                        <asp:LinkButton runat="server" AlternateText="Crew Complaints"
                                            CommandName="CREWCOMPLIENT" CommandArgument='<%# Container.DataSetIndex %>' ID="imgCrewComplaints"
                                            ToolTip="Crew Complaints">
                                            <span class="icon"><i class="fas fa-user-edit"></i></span>
                                        </asp:LinkButton>
                                      
                                        <asp:LinkButton runat="server" AlternateText="sent mail"
                                            CommandName="SENDMAILTOCREW" CommandArgument='<%# Container.DataSetIndex %>'
                                            ID="cmdsendmail" ToolTip="Resend email">
                                            <span class="icon"><i class="fas fa-envelope"></i></span>
                                        </asp:LinkButton>

                                    </ItemTemplate>
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

         
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
