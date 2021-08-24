<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsLeaveOpeningBalance.aspx.cs"
    Inherits="AccountsLeaveOpeningBalance" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Opening Balance</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function pageLoad() {
                PaneResized();
            }
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("");
                grid._gridDataDiv.style.height = (browserHeight - 130) + "px";
            }
        </script>
        <script type="text/javascript">
            function ConfirmLock(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmLock.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLeaveOpening" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuLeaveOpeningBalance" runat="server" OnTabStripCommand="MenuLeaveOpeningBalance_TabStripCommand"></eluc:TabStrip>
        <table Width="100%">
            <tr>
                <td width="11.2%">
                    <telerik:RadLabel ID="lblSeafarerFileNumber" runat="server" Text="Seafarer File Number"></telerik:RadLabel>
                </td>
                <td colspan="5">
                    <telerik:RadTextBox runat="server" ID="txtFileNo" Width="20%" CssClass="input_mandatory"></telerik:RadTextBox>
                    <asp:ImageButton ID="ImgBtnValidFileno" runat="server" ImageAlign="AbsBottom" ImageUrl="<%$ PhoenixTheme:images/tableviewobservation.png %>"
                        ToolTip="Verify Entered File Number" OnClick="ImgBtnValidFileno_Click" />
                    <font color="Blue"><asp:Label ID="lblNote" runat="server" Text="Note: Please verify the entered file number by clicking search icon next to the File number textbox"></asp:Label></font>
                </td>
            </tr>
            <tr>
                <td width="11.2%">
                    <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                </td>
                <td width="22.6%">
                    <telerik:RadTextBox runat="server" ID="txtFirstName" Width="80%" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td width="10.6%">
                    <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                </td>
                <td width="22.6%">
                    <telerik:RadTextBox runat="server" ID="txtMiddleName" Width="80%" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td width="10.6%">
                    <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                </td>
                <td width="22.6%">
                    <telerik:RadTextBox runat="server" ID="txtLastName" Width="80%" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td width="11.2%">
                    <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>
                </td>
                <td width="22.6%">
                    <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" Width="80%"  CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td width="10.6%">
                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                </td>
                <td width="22.6%">
                    <telerik:RadTextBox runat="server" ID="txtRank" Width="80%" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td width="10.6%">

                </td>
                <td width="22.6%">

                </td>
            </tr>
            <%--<tr>
                <td colspan="6">&nbsp;
                </td>
            </tr>--%>
            <tr>
                <td width="11.2%">
                    <telerik:RadLabel ID="lblVesselSailed" runat="server" Text="Vessel Sailed"></telerik:RadLabel>
                </td>
                <td width="22.6%">
                    <telerik:RadDropDownList runat="server" ID="ddlVesselSailed" CssClass="input_mandatory" AppendDataBoundItems="true" AutoPostBack="false"
                        DataValueField="FLDSAILEDHISTORYID" DataTextField="FLDSAILEDHISTORYNAME" Width="80%">
                    </telerik:RadDropDownList>
                </td>
                <td width="10.6%">
                    <telerik:RadLabel ID="lblLeaveUnPaidfortheselectedSignOn" runat="server" Text="Leave UnPaid (for the selected Sign On)"></telerik:RadLabel>
                </td>
                <td width="22.6%">
                    <eluc:Number runat="server" ID="txtLeaveUnPaid" CssClass="input_mandatory" DecimalPlace="1" />
                </td>
                <td width="10.6%">
                    <telerik:RadLabel ID="lblBTBUnPaidfortheselectedSignOn" runat="server" Text="BTB UnPaid (for the selected Sign On)"></telerik:RadLabel>
                </td>
                <td width="22.6%">
                    <eluc:Number runat="server" ID="txtBTBUnPaid" CssClass="input_mandatory" DecimalPlace="1" />
                </td>
            </tr>
            <tr id="trSeniority" runat="server">
                <td width="11.2%">
                    <telerik:RadLabel ID="lblSeniorityYear" runat="server" Text="Seniority Year"></telerik:RadLabel>
                </td>
                <td width="22.6%">
                    <telerik:RadDropDownList runat="server" ID="ddlSeniority" CssClass="input_mandatory" Width="80%">
                    </telerik:RadDropDownList>
                </td>
                <td width="10.6%">
                    <telerik:RadLabel ID="lblMonthlyWages" runat="server" Text="Monthly Wages"></telerik:RadLabel>
                </td>
                <td width="22.6%">
                    <eluc:Number runat="server" ID="txtMonthlyWages" CssClass="input_mandatory" DecimalPlace="2" />
                </td>
                <td width="10.6%">

                </td>
                <td width="22.6%">

                </td>
            </tr>
        </table>
        <br />
        <telerik:RadGrid ID="gvLVP" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="">
                <NoRecordsTemplate>
                    <table runat="server" Width="50%" border="0">
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
                    <telerik:GridTemplateColumn HeaderText="Vessel">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="BTod/Sign On Date">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# string.Format("{0:dd/MMM/yyyy}",DataBinder.Eval(Container, "DataItem.FLDBTOD"))%> - <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sign Off /ETod Date">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# string.Format("{0:dd/MMM/yyyy}",DataBinder.Eval(Container, "DataItem.FLDSIGNOFFDATE"))%> - <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDETOD"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="From Date">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDFROMDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="To Date">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDTODATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Leave Earned">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDLEAVEDAYS")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Leave UnPaid">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDUNPAIDLEAVE")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="BTB Earned">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDBTBDAYS")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="BTB UnPaid">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDUNPAIDBTB")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <eluc:Status ID="ucStatus" runat="server" />
        <asp:Button ID="ucConfirmLock" runat="server" OnClick="ucConfirmLock_Click" CssClass="hidden" />
        <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnRequest_Click" OKText="Yes" CancelText="No" Visible="false" />
    </form>
</body>
</html>
