<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportCrewReliever.aspx.cs" Inherits="OwnersMonthlyReportCrewReliever" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvReleif.ClientID %>"));
               }, 200);
           }
        </script>  
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
            <telerik:RadGrid ID="gvReleif" runat="server" ShowHeader="true" ShowFooter="false" AutoGenerateColumns="false"
                GridLines="None" BorderStyle="None" ClientIDMode="AutoID"
                OnNeedDataSource="gvReleif_NeedDataSource" OnItemDataBound="gvReleif_ItemDataBound">
                <MasterTableView ShowHeader="true" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Off-Signer">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRelievee" CommandName="SELECTROW" CommandArgument='<%# Container.DataSetIndex %>'
                                    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                <asp:Label ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></asp:Label>
                                <asp:Label ID="lblName" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblVesseltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblTrainingmatrixid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGMATRIXID") %>'></asp:Label>
                                <asp:Label ID="lblJoinDate" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="lnkRank" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblOffsignerNationality" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNATIONALITY") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Wages">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDPRate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONBOARDEMPWAGES")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign on Date">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblDateJoined" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERJOINDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="End of Contract">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblReliefDue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reliever">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblRelieverId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERID") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:LinkButton ID="lnkReliever" CommandName="SELECTROW" CommandArgument='<%# Container.DataSetIndex %>'
                                    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' Visible="false" Enabled="false"></asp:LinkButton>
                                <asp:Label ID="lblRelieverName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblRelieverNationality" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNATIONALITY") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblRelieverRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERRANKID") %>'
                                    Visible="false">
                                </asp:Label>
                                <asp:Label ID="RadLabel14" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERRANK") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Wages">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblDPRate1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTOTALAMOUNT")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned Date">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblReliefDue1" Visible="false" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE")) %>'></asp:Label>
                                <asp:Label ID="lblJoiningDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE")) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Joining Port">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblPDStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUSID")%>'></asp:Label>
                                <asp:Label ID="lblRemarks" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString().Length>20 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString()) %>'></asp:Label>
                                <asp:LinkButton ID="imgRemarks" runat="server" CommandArgument='<%# Container.DataSetIndex %>'>
                                                           <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                                <eluc:ToolTip ID="ucToolTipAddress" Width="200px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee Status">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lblPDStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
