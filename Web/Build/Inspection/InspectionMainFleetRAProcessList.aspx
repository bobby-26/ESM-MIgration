<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMainFleetRAProcessList.aspx.cs" Inherits="InspectionMainFleetRAProcessList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Process</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
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
                $("#gvPlanner").height(browserHeight - 40);
            });

        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Title runat="server" ID="ucTitle" Text="Process" ShowMenu="true" Visible="false" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref.No"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtHazardNo" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblRAType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlRAType" runat="server" AutoPostBack="true" Width="180px" OnSelectedIndexChanged="ddlRAType_Changed"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlCategory" runat="server" AppendDataBoundItems="true" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" Width="240px"
                        AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" OnSelectedIndexChanged="ddlCategory_Changed">
                        <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true" AutoPostBack="true" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"
                        OnSelectedIndexChanged="ddlStatus_Changed">
                        <Items>
                            <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="1" Text="Draft"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="2" Text="Pending Approval"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="3" Text="Approved for Use"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessmentProcess" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true"
            Font-Size="11px" Width="100%" Height="88.5%" CellPadding="3" ShowHeader="true" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemDataBound="gvRiskAssessmentProcess_ItemDataBound" OnItemCommand="gvRiskAssessmentProcess_ItemCommand" DataKeyNames="FLDRISKASSESSMENTPROCESSID"
            OnNeedDataSource="gvRiskAssessmentProcess_NeedDataSource" AllowSorting="true">
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
                    <telerik:GridTemplateColumn HeaderText="Ref.No" HeaderStyle-Width="90px" AllowSorting="true" SortExpression="FLDNUMBER" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblReferencid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="74px" AllowSorting="true" SortExpression="FLDDATE" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"]) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="220px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSNAME")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Process" HeaderStyle-Width="220px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblJobActivity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDJOBACTIVITY").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucJobActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBACTIVITY") %>' TargetControlId="hidden" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Risks/Aspects" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRiskAssessmentProcessID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTPROCESSID")  %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKACTIVITY")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="105px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblActiveyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rev.No" HeaderStyle-Width="63px" AllowSorting="true" SortExpression="FLDREVISIONNO" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Issued" HeaderStyle-Width="74px" AllowSorting="true" SortExpression="FLDISSUEDDATE" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDISSUEDDATE"])%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Edited By" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEditedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEDITEDBY")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="190px" HeaderStyle-HorizontalAlign="Center">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITROW" ID="cmdEdit"
                                ToolTip="Edit">
                             <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Approve" CommandName="APPROVE" ID="imgApprove"
                                ToolTip="Request for Approval">
                                <span class="icon"><i class="fas fa-award"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Issue" CommandName="ISSUE" ID="imgIssue"
                                ToolTip="Issue">
                                 <span class="icon"><i class="fas fa-user-check"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Revision" CommandName="REVISION" ID="imgrevision"
                                ToolTip="Create Revision">
                                 <span class="icon"><i class="fas fa-registered"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Process PDF" CommandName="RAPROCESS"
                                ID="cmdRAProcess" ToolTip="Show PDF">
                                 <span class="icon"><i class="fas fa-chart-bar"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Revision" CommandName="VIEWREVISION" ID="cmdRevisions"
                                ToolTip="View Revisions">
                                <span class="icon"><i class="fas fa-eye"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Copy" CommandName="COPY" ID="imgCopy"
                                ToolTip="Copy">
                                <span class="icon"><i class="fas fa-copy"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Copy JHA to Company" CommandName="OFFICECOPY" ID="imgOfficeCopy"
                                ToolTip="Copy RA to Company">
                                 <span class="icon"><i class="fas fa-file-import"></i></span>
                            </asp:LinkButton>
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
        <asp:Button ID="ucConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
        <asp:Button ID="ucConfirmIssue" runat="server" Text="ConfirmIssue" OnClick="btnConfirmIssue_Click" />
        <asp:Button ID="ucConfirmRevision" runat="server" Text="ConfirmRevision" OnClick="btnConfirmRevision_Click" />
    </form>
</body>
</html>

