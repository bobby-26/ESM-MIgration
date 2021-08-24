<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMainFleetRAGenericList.aspx.cs" Inherits="InspectionMainFleetRAGenericList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Generic</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmApprove.UniqueID %>", "");
                }
            }
            function ConfirmIssue(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmIssue.UniqueID %>", "");
                }
            }
            function ConfirmRevision(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRevision.UniqueID %>", "");
                }
            }

            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvRiskAssessmentGeneric").height(browserHeight - 90);
            });

        </script>      
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="ucTitle" Text="Generic" ShowMenu="true" Visible="false" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRAType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlRAType" runat="server" Width="240px"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref.No"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="input" Width="132px"></telerik:RadTextBox>
                </td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblcategory" runat="server" Text="Category"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlCategory" runat="server" AppendDataBoundItems="true"
                        AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" Width="240px"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblIntendedWork" runat="server" Text="Intended Work Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDateIntendedWorkFrom" runat="server" />
                    &nbsp;-&nbsp;
                                    <eluc:Date ID="ucDateIntendedWorkTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Fleet runat="server" ID="ucTechFleet" Width="240px" AppendDataBoundItems="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true" Width="240px"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" Width="240px" AssignedVessels="true" />
                </td>
                <td colspan="2"></td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuGeneric" runat="server" OnTabStripCommand="MenuGeneric_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessmentGeneric" runat="server" AutoGenerateColumns="False" AllowSorting="true" AllowPaging="true" AllowCustomPaging="true"
            Font-Size="11px" Height="78%" CellPadding="3" ShowHeader="true" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemCommand="gvRiskAssessmentGeneric_ItemCommand" OnItemDataBound="gvRiskAssessmentGeneric_ItemDataBound" DataKeyNames="FLDRISKASSESSMENTGENERICID"
            OnNeedDataSource="gvRiskAssessmentGeneric_NeedDataSource" OnSortCommand="gvRiskAssessmentGeneric_SortCommand" GridLines="None">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <ColumnGroups>
                    <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
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
                    <telerik:GridTemplateColumn HeaderStyle-Width="35px">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="imgOfficeFlag" runat="server" Visible="false" />
                            <asp:Image ID="imgFlag" runat="server" Visible="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Ref.No" AllowSorting="true" SortExpression="FLDNUMBER" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblReferencid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowSorting="true" SortExpression="FLDVESSELNAME" ShowSortIcon="true" HeaderText="Vessel" HeaderStyle-Width="135px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblIsCreatedByOffice" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISCREATEDBYOFFICE")  %>'></telerik:RadLabel>
                            <%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"] %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="73px" HeaderText="Prepared" AllowSorting="true" SortExpression="FLDDATE" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"]) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Intended Work">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkintendedWorkDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDINTENDEDWORKDATE">Intended Work</asp:LinkButton>
                            <img id="FLDINTENDEDWORKDATE" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDINTENDEDWORKDATE"])%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblTypeHeader" runat="server">Type</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Activity" HeaderStyle-Width="210px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRiskAssessmentGenericID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTGENERICID")  %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblRevisionno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIVITYCONDITIONS").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYCONDITIONS") %>' TargetControlId="lblWorkActivity" Visible="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rev.No" AllowSorting="true" SortExpression="FLDREVISIONNO" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRevno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="130px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStatus" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblApprovedByHeader" runat="server">Approved by</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblApprovedBy" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAPPROVEDBY").ToString().Length > 25 ? DataBinder.Eval(Container, "DataItem.FLDAPPROVEDBY").ToString().Substring(0, 25) + "..." : DataBinder.Eval(Container, "DataItem.FLDAPPROVEDBY").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucApprovedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDBY") %>' TargetControlId="lblApprovedBy" Visible="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="165px" HeaderText="Action">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITROW" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Approve" CommandName="APPROVE" ID="imgApprove"
                                ToolTip="Approve">
                                <span class="icon"><i class="fas fa-award"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Issue" CommandName="ISSUE" ID="imgIssue"
                                ToolTip="Approve for use">
                                <span class="icon"><i class="fas fa-user-check"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Revision" CommandName="REVISION" ID="imgrevision"
                                ToolTip="Create Revision">
                                <span class="icon"><i class="fas fa-registered"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Generic PDF" CommandName="RAGENERIC" ID="cmdRAGeneric" ToolTip="Show PDF">
                                <span class="icon"><i class="fas fa-chart-bar"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="View Revisions" CommandName="VIEWREVISION" ID="cmdRevision"
                                ToolTip="View Revisions">
                                <span class="icon"><i class="fas fa-eye"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Copy template" CommandName="COPYTEMPLATE" ID="imgCopyTemplate"
                                ToolTip="Copy template">
                                <span class="icon"><i class="fas fa-copy"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Propose template" CommandName="PROPOSETEMPLATE" ID="imgProposeTemplate"
                                ToolTip="Propose Standard Template">
                                  <span class="icon"><i class="fas fa-proposeST"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Comparison" CommandName="COMPARISON" ID="imgComparison"
                                ToolTip="Comparison">
                                  <span class="icon"><i class="fas fa-equals"></i></span>
                            </asp:LinkButton>
                            <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Copy" ImageUrl="<%$ PhoenixTheme:images/vessel.png %>"
                                    CommandName="COPYTOVESSEL" CommandArgument="<%# Container.DataItemIndex %>" ID="imgCopy"
                                    ToolTip="Copy To Vessel"></asp:ImageButton>--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <img id="Img11" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblOverDue" runat="server" Text=" * Overdue"></telerik:RadLabel>
                </td>
                <td>
                    <img id="Img12" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDueWithin2weeks" runat="server" Text=" * Due within 2 Weeks"></telerik:RadLabel>
                </td>
                <td>
                    <img id="Img8" src="<%$ PhoenixTheme:images/green.png%>" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblcopiedST" runat="server" Text=" * Copied from Standard Template"></telerik:RadLabel>
                </td>
            </tr>
        </table>
        <%--        <eluc:Confirm ID="ucConfirmApprove" runat="server" OnConfirmMesage="btnConfirmApprove_Click"
            OKText="Yes" CancelText="No" />
        <eluc:Confirm ID="ucConfirmIssue" runat="server" OnConfirmMesage="btnConfirmApprove_Click"
            OKText="Yes" CancelText="No" />
        <eluc:Confirm ID="ucConfirmRevision" runat="server" OnConfirmMesage="btnConfirmRevision_Click"
            OKText="Yes" CancelText="No" />--%>
        <asp:Button ID="ucConfirmApprove" runat="server" Text="confirm" OnClick="btnConfirmApprove_Click" />
        <asp:Button ID="ucConfirmIssue" runat="server" Text="ConfirmIssue" OnClick="btnConfirmIssue_Click" />
        <asp:Button ID="ucConfirmRevision" runat="server" Text="ConfirmRevision" OnClick="btnConfirmRevision_Click" />
    </form>
</body>
</html>
