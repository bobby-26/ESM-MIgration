<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewDateOfAvailability.aspx.cs"
    Inherits="CrewDateOfAvailability" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Relation" Src="../UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Date Of Availabilty</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .checkRtl {
            direction: rtl;
        }

        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewDateOfAvailability" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewDateOfAvailability" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuDO" runat="server" OnTabStripCommand="DOA_TabStripCommand" Title="Date Of Availabilty"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%" CssClass="scrolpan">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True" Enabled="false" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblPresentRank" runat="server" Text="Present Rank"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtPayRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastVessel" runat="server" Text="Last Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastVessel" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignedOff" runat="server" Text="Signed Off"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignedOff" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
            </table>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td colspan="3">
                        <b>
                            <telerik:RadLabel ID="lblPermanentContact" runat="server" Text="Permanent Contact"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="3">
                        <b>
                            <telerik:RadLabel ID="lblLocalContact" runat="server" Text="Local Contact"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPhoneNumber" runat="server" Text="Phone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="txtPhoneNumber" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPhoneNumber2" runat="server" Text="Phone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="txtPhoneNumber2" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalPhoneNumber" runat="server" Text="Phone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="txtlocalPhoneNumber" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalPhoneNumber2" runat="server" Text="Phone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="txtLocalPhoneNumber2" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Mobile"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtMobileNumber" IsMobileNumber="true" runat="server" ReadOnly="True" Enabled="false"
                            CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber2" runat="server" Text="Mobile"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtMobileNumber2" IsMobileNumber="true" runat="server" ReadOnly="True" Enabled="false"
                            CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalMobileNumber" runat="server" Text="Mobile"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtLocalMobileNumber" IsMobileNumber="true" runat="server" ReadOnly="True" Enabled="false"
                            CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalMobileNumber2" runat="server" Text="Mobile"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtLocalMobileNumber2" IsMobileNumber="true" runat="server" ReadOnly="True" Enabled="false"
                            CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber3" runat="server" Text="Mobile"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtMobileNumber3" IsMobileNumber="true" runat="server" ReadOnly="True" Enabled="false"
                            CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPerRelation" runat="server" Text="Relation"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Relation ID="ucPerRelation" runat="server" CssClass="input" AppendDataBoundItems="true"
                            QuickTypeCode="7" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalMobileNumber3" runat="server" Text="Mobile"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtLocalMobileNumber3" IsMobileNumber="true" runat="server" ReadOnly="True" Enabled="false"
                            CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocRelation" runat="server" Text="Relation"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Relation ID="ucLocRelation" runat="server" CssClass="input" AppendDataBoundItems="true"
                            QuickTypeCode="7" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtEmail" runat="server" Width="90%" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td colspan="3"></td>
                </tr>
            </table>
            <hr />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDOAGivenDate" runat="server" Text="DOA Given Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDOAGivenDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDOAMethod" runat="server" Text="DOA Method"></telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:Quick ID="ddlDOAMethod" runat="server" CssClass="input" AppendDataBoundItems="true"
                            QuickTypeCode="56"></eluc:Quick>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateOfTeleconference" runat="server" Text="Date Of Teleconference"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDTOfTelConf" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblDateOfAvailability" runat="server" Text="Date Of Availability"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDOA" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            OnTextChangedEvent='txtDOA_TextChanged' />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStandbyDate" runat="server" Text="Standby Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtStandbyDate" runat="server" CssClass="input" />
                        <telerik:RadTextBox ID="txtHiddenDOAID" runat="server" MaxLength="6" CssClass="input" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFollowUpDate" runat="server" Text="FollowUp Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFollowupDate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblExpectedsalary" runat="server" Text="Expected Salary"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtExpectedsalary" runat="server" CssClass="input txtNumber" MaxLength="10"
                            IsInteger="true" MaskText="##########" IsPositive="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDOACancellation" runat="server" Text="DOA Cancellation"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkDOACancellation" runat="server" OnCheckedChanged="RemarksMandatory_CheckedChanged"
                            AutoPostBack="true"></asp:CheckBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblReasonforCancellation" runat="server" Text="Reason for Cancellation"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="readonlytextbox" ReadOnly="True" Enabled="false"
                            MaxLength="800" TextMode="MultiLine" Height="30px" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDOA" runat="server"
                AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvDOA_NeedDataSource" EnableHeaderContextMenu="true" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="DOA Given Date" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeDAOGivenDate" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOAGIVENDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DOA Method" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDOAMethod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOAMETHOD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Dt. Of TeleConf." AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateOfTeleConf" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFTELCONF","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Standby Date" ColumnGroupName="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStandByDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTANDBYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Of Avail." ColumnGroupName="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDAO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOA","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expected Salary" ColumnGroupName="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpectedSalaryitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPECTEDSALARY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cancellation Date" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCancellationDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELLATIONDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason for Cancellation" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="30%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Width="300px" Style="word-wrap: break-word;"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>'>
                                </telerik:RadLabel>
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
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">

                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
