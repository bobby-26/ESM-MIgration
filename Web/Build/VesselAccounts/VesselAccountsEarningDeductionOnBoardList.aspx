<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsEarningDeductionOnBoardList.aspx.cs"
    Inherits="VesselAccounts_VesselAccountsEarningDeductionOnBoardList" EnableEventValidation="True" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EntryType" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VesselEarningDeductionBoardList</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvCrewSearch").height(browserHeight - 40);
            });
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuEarDedGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuEarDedGeneral_TabStripCommand" Title="Earnings/Deductions"></eluc:TabStrip>
        <br />
        <table width="60%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox RenderMode="Lightweight" ID="ddlType" runat="server" CssClass="input_mandatory" EmptyMessage="Type or select Type" MarkFirstMatch="true" AppendDataBoundItems="true"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="Onboard Earnings/Deductions" Selected="true" />
                            <telerik:RadComboBoxItem Value="3" Text="Cash Advance" />
                            <telerik:RadComboBoxItem Value="4" Text="Allotment" />
                            <telerik:RadComboBoxItem Value="5" Text="Radio Log" />
                            <telerik:RadComboBoxItem Value="7" Text="Special Allotment" />
                        </Items>
                    </telerik:RadComboBox>
                    <asp:ImageButton ID="imgClip" runat="server" OnClick="imgClip_onClick"
                        ToolTip="Upload Attachment" Style="color: #FFFFFF" />
                </td>

                <td>
                    <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" AutoPostBack="true" EmptyMessage="Type or select Type" MarkFirstMatch="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input_mandatory" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="true"></telerik:RadComboBox>
                </td>
            </tr>
        </table>
        <br />
        <eluc:TabStrip ID="MenuBondIssue" runat="server" OnTabStripCommand="MenuBondIssue_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="false"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true"
            OnNeedDataSource="gvCrewSearch_NeedDataSource" OnItemDataBound="gvCrewSearch_ItemDataBound" OnItemCommand="gvCrewSearch_ItemCommand">
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                <%--<HeaderStyle Width="120px" />--%>
                <Columns>
                    <telerik:GridTemplateColumn ItemStyle-Width="40px" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="File No.">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDFILENO"] %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDNAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDRANKCODE"] %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Email">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDEMAIL"] %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" Position="Bottom" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" ScrollHeight="350px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
