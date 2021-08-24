<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreAppraisalDetailsFrame.aspx.cs" Inherits="CrewOffshore_CrewOffshoreAppraisalDetailsFrame" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <telerik:RadSkinManager ID="RadSkinManager1" runat="server"></telerik:RadSkinManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" Font-Size="Smaller">
                <div>
                    <telerik:RadGrid ID="gvCrewAppraisal" runat="server" AutoGenerateColumns="false"  Height="420px"
                        AllowSorting="false" GroupingEnabled="true" BorderStyle="None"
                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvCrewAppraisal_NeedDataSource"
                        OnItemDataBound="gvCrewAppraisal_ItemDataBound">
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                            AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="true" GroupLoadMode="Client"
                            GroupHeaderItemStyle-CssClass="center">
                            <NoRecordsTemplate>
                                <table runat="server" width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Appraisal" HeaderStyle-Width="75px">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentIssue" runat="server" Text='<%# General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDAPPRAISALDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Score" HeaderStyle-Width="60px">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblscore" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAPPRAISAL") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Promotion Y/N" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIsRecommendPromo" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROMOTIONYESNO")%>'></telerik:RadLabel>

                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Re-Employ Y/N" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIsreemploy" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRECOMMENDEDSTATUSNAME")%>'></telerik:RadLabel>

                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <%--<telerik:RadLabel ID="lblIsreempssldoy" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT")%>'></telerik:RadLabel>--%>

                                        <telerik:RadLabel ID="lblPlannedVessel" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT").ToString().Length> 30 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT").ToString()).ToString().Substring(0, 30)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDHEADDEPTCOMMENT").ToString()) %>'></telerik:RadLabel>
                                        <eluc:tooltip id = "ucToolTipPlannedVessel" runat = "server"  targetcontrolid = "lblPlannedVessel" />


                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                            </Columns>
                        </MasterTableView>
                          <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
