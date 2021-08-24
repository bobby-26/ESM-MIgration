<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportCandidatePerformance.aspx.cs"
    Inherits="CrewReportCandidatePerformance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Faculty" Src="~/UserControls/UserControlFaculty.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlBatch.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Candidate Performance Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCandidatePerformnace" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuCandidatePerformanceReport" runat="server" OnTabStripCommand="MenuCandidatePerformanceReport_TabStripCommand"></eluc:TabStrip>

            <table id="filter" cellpadding="0" cellspacing="0" style="width: 100%; height: 180px;">
                <tr>
                    <td colspan="1" style="width: 200px">
                        <telerik:RadLabel ID="lblCourseName" runat="server" Text="Course Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 300px">
                        <telerik:RadListBox ID="lstCourse" runat="server" AppendDataBoundItems="true" Width="350px" Height="100px"
                            SelectionMode="Multiple">
                        </telerik:RadListBox>


                    </td>
                    <td style="width: 200px;" class="center">
                        <telerik:RadLabel ID="lblBatchNo" Text="Batch No" runat="server"></telerik:RadLabel>
                    </td>
                    <td style="width: 200px">
                        <telerik:RadListBox ID="lstBatch" runat="server" AppendDataBoundItems="true" Width="350px" Height="100px"
                            SelectionMode="Multiple" DataTextField="FLDBATCH"
                            DataValueField="FLDBATCHID">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        <telerik:RadLabel ID="lblSeafarer" Text="Seafarer" runat="server"></telerik:RadLabel>
                    </td>
                    <td style="width: 300px">
                        <span id="spnPickListEmployee" runat="server">
                            <asp:TextBox ID="txtEmployeeName" runat="server" Enabled="false"
                                Width="50%"></asp:TextBox>
                            <asp:ImageButton ID="btnShowEmployee" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListEmployee', 'codehelp1', '', '../Common/CommonPickListEmployee.aspx', true);" />
                            <asp:TextBox ID="txtEmployeeId" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                        </span>
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>

            <br />

            <eluc:TabStrip ID="MenuGridCandidatePerformnace" runat="server" OnTabStripCommand="MenuCandidatePerformnace_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCandidatePerformnace" runat="server" Height="55%"
                EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvCandidatePerformnace_ItemDataBound"
                ShowFooter="False" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />

                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

