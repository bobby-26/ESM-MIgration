<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePD.aspx.cs" Inherits="CrewOffshorePD" %>

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
           
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="PD_TabStripCommand"></eluc:TabStrip>

              
                    <%--<asp:GridView ID="gvSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnSelectedIndexChanging="gvSearch_SelectedIndexChanging" Width="100%" CellPadding="3" ShowHeader="true"
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

                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblRank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkemployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Curr ency">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblcCurrencyid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Daily Rate">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDailyRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAILYRATEUSD")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblbudgetedwage" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD1STTEARSCALE")%>'></telerik:RadLabel>

                                        <eluc:ToolTip ID="ucToolTipNW" TargetControlId="lblDailyRate" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="EPF %">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblepfper" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEPFCONTRIBUTION")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="EPF Amt">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDOA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEPFAMOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Daily DP Allowance">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="RadLabel4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDPALLOWANCE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                  <telerik:GridTemplateColumn HeaderText="Planned Relief">
                                    <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                       <telerik:RadLabel ID="lblPlanned" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Off-Signer">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkOffsignerEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblOffsignerName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                  <telerik:GridTemplateColumn HeaderText="End of Contract">
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="RadLabel6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Proposed By">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDPROPOSEDBY")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Proposal Remarks">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Wrap="true" Width="100px" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                         <telerik:RadLabel ID="lblRemarks" runat="server" 
                                             Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALREMARKS").ToString().Length > 30 ? 
                                                 DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString().Substring(0, 30) + "..." 
                                                 : DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString() %>'></telerik:RadLabel>                                       
                                        <%--<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS")%>--%>
                                        <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS")%>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblInterviewId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINTERVIEWID") %>'></telerik:RadLabel>
                                        <asp:TextBox ID="txtMumbaiRemarks" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75px"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Approve Proposal"
                                            CommandName="APPROVE" CommandArgument='<%# Container.DataSetIndex %>' ID="imgApprove"
                                            ToolTip="Approve Proposal">
                                        <span class="icon"><i class="fas fa-award"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Reject"
                                            CommandName="REJECT" CommandArgument='<%# Container.DataSetIndex %>' ID="imgReject"
                                            ToolTip="Reject Proposal">
                                         <span class="icon"><i class="fas fa-times-circle-cancel"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <%--<NestedViewTemplate>
                                <table style="font-size: 11px;">
                                    <tr>

                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="EPF % :"></telerik:RadLabel>

                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblepfper" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEPFCONTRIBUTION")%>'></telerik:RadLabel>
                                        </td>
                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="EPF Amount :"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblDOA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEPFAMOUNT") %>'></telerik:RadLabel>
                                        </td>
                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="Daily DP Allowance:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDPALLOWANCE") %>'></telerik:RadLabel>
                                        </td>
                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel5" runat="server" Text="Planned Relief:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>

                                        </td>
                                        <td style="font-weight: 700">
                                            <telerik:RadLabel ID="RadLabel7" runat="server" Text="End of contract:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>

                                        </td>

                                    </tr>

                                </table>
                            </NestedViewTemplate>--%>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
               
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
