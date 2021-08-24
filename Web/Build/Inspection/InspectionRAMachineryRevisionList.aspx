<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAMachineryRevisionList.aspx.cs" Inherits="InspectionRAMachineryRevisionList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Machinery</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvRiskAssessmentMachineryRevision").height(browserHeight - 90);
            });
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="ucTitle" Text="Machinery Revisions" ShowMenu="false" Visible="false" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuMachinery" runat="server" OnTabStripCommand="MenuMachinery_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessmentMachineryRevision" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true"
            Font-Size="11px" Width="100%" Height="91.5%" CellPadding="3" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true"
            OnItemDataBound="gvRiskAssessmentMachineryRevision_ItemDataBound" DataKeyNames="FLDRISKASSESSMENTMACHINERYID"
            OnNeedDataSource="gvRiskAssessmentMachineryRevision_NeedDataSource">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <ColumnGroups>
                    <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                </ColumnGroups>
                <NoRecordsTemplate>
                    <table width="99.9%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Ref.No">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>' Visible="false"></telerik:RadLabel>
                            <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItem %>"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblReferencid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID")  %>'></telerik:RadLabel>
                            <%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"] %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date">
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
                    <telerik:GridTemplateColumn HeaderText="Activity / Conditions">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRiskAssessmentMachineryID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENTMACHINERYID")  %>'
                                Visible="false">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIVITYCONDITIONS").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYCONDITIONS") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Intended Work Date">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDINTENDEDWORKDATE"])%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rev.No">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRevno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStatus" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'></telerik:RadLabel>
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
        <%--        <table width="100%" border="0" class="datagrid_pagestyle">
            <tr>
                <td nowrap="nowrap" align="center">
                    <telerik:RadLabel ID="lblPagenumber" runat="server">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="lblPages" runat="server">
                    </telerik:RadLabel>
                    <telerik:RadLabel ID="lblRecords" runat="server">
                    </telerik:RadLabel>&nbsp;&nbsp;
                </td>
                <td nowrap="nowrap" align="left" width="50px">
                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                </td>
                <td width="20px">&nbsp;
                </td>
                <td nowrap="nowrap" align="right" width="50px">
                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                </td>
                <td nowrap="nowrap" align="center">
                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                    </asp:TextBox>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                        Width="40px"></asp:Button>
                </td>
            </tr>
        </table>--%>
    </form>
</body>
</html>
