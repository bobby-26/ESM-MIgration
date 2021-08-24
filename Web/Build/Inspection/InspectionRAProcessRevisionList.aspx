<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAProcessRevisionList.aspx.cs" Inherits="InspectionRAProcessRevisionList" %>

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
<head id="Head1" runat="server">
    <title>Process Revisions</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvRiskAssessmentProcessRevision").height(browserHeight - 90);
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
        <eluc:Title runat="server" ID="ucTitle" Text="Process Revisions" ShowMenu="false" Visible="false" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <table cellpadding="1" cellspacing="1" width="100%" id="tdlHazard" runat="server"
            visible="false">
            <tr>
                <td>
                    <asp:Literal ID="lblHazardNo" runat="server" Text="Hazard Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtHazardNo" runat="server" CssClass="input"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="input" AppendDataBoundItems="true"
                        AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" OnSelectedIndexChanged="ddlCategory_Changed">
                        <asp:ListItem Text="--Select--" Value="DUMMY"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlStatus_Changed" CssClass="input">
                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Draft"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Approved"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Issued"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskAssessmentProcessRevision" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true"
            Font-Size="11px" Width="100%" Height="91%" CellPadding="3" ShowHeader="true" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemDataBound="gvRiskAssessmentProcessRevision_ItemDataBound" DataKeyNames="FLDRISKASSESSMENTPROCESSID" OnNeedDataSource="gvRiskAssessmentProcessRevision_NeedDataSource"
            OnItemCommand="gvRiskAssessmentProcessRevision_ItemCommand">
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
                    <telerik:GridTemplateColumn HeaderText="Ref.No">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>' Visible="false"></telerik:RadLabel>
                            <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItem %>"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER")  %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblReferencid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblInstallcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTALLCODE")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"]) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderStyle-Width="198px" HeaderText="Category">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblProcessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSNAME")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Process">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblJobActivity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDJOBACTIVITY").ToString().Length > 40 ? DataBinder.Eval(Container, "DataItem.FLDJOBACTIVITY").ToString().Substring(0, 40) + "..." : DataBinder.Eval(Container, "DataItem.FLDJOBACTIVITY").ToString() %>'></telerik:RadLabel>
                            <eluc:ToolTip ID="ucJobActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBACTIVITY") %>'
                                Width="300px" />
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
                    <telerik:GridTemplateColumn HeaderText="Status">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblActiveyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN")  %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rev.No">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO")  %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Issued">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDISSUEDDATE"])%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false" HeaderText="Edited By">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEditedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEDITEDBY")  %>'></telerik:RadLabel>
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
        <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes"
            CancelText="No" />
    </form>
</body>
</html>
