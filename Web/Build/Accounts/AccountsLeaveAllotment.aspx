<%@ Page Language="C#" AutoEventWireup="True" CodeFile="AccountsLeaveAllotment.aspx.cs"
    Inherits="AccountsLeaveAllotment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rank</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function ConfirmSend(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmSend.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="LeaveAllotment" runat="server" OnTabStripCommand="LeaveAllotment_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPayTill" runat="server" Text="Pay Till"></telerik:RadLabel></td>
                <td>
                    <eluc:Date ID="txtPayTill" runat="server" CssClass="input_mandatory" />
                </td>
                <td>
                    <asp:ImageButton runat="server" AlternateText="View Allotment Request" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                        CommandName="SEARCH" ID="cmdSearch" OnClick="cmdSearch_Click" ToolTip="View Allotment Request"></asp:ImageButton>
                </td>
            </tr>
        </table>
        <telerik:RadGrid ID="gvLVP" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnNeedDataSource="gvLVP_NeedDataSource"
            Width="100%" OnItemDataBound="gvLVP_ItemDataBound" EnableViewState="false" DataKeyNames="" ShowHeader="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDEMPLOYEEID">
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
                    <telerik:GridTemplateColumn HeaderText="Rank">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Crew Name">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Contract Commence Date">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDCOCDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Paid From Date">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDFROMDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Paid Till Date">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDTODATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Days Deducted">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDLEAVEDEDUCTION")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Monthly Leave Wages Amount">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDMONTHLYWAGES")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Leave Amount">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDLEAVEAMOUNT")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <eluc:Status ID="ucStatus" runat="server" />
        <%--<eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnRequest_Click" OKText="Yes" CancelText="No" Visible="false" />--%>
        <asp:Button ID="ucConfirmSend" runat="server" OnClick="ucConfirmSend_Click" CssClass="hidden" />
    </form>
</body>
</html>
