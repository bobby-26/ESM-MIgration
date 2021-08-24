<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewFamilySignOn.aspx.cs" Inherits="CrewFamilySignOn" EnableEventValidation="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOnReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmCrew" Src="~/UserControls/UserControlConfirmMessageCrew.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Sign-On</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    document.getElementById("txtremarks").value = args;
                    __doPostBack("<%=confirm.UniqueID %>", "");

                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSignOn" runat="server">

        <eluc:TabStrip ID="CrewFamilyTabs" runat="server" OnTabStripCommand="CrewFamilyTabs_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuSignOn" runat="server" OnTabStripCommand="CrewSignOn_TabStripCommand"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        <asp:HiddenField ID="txtremarks" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="80%">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSignonVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ucCountry" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Width="50%" OnTextChangedEvent="FilterSeaport" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOnPort" runat="server" Text="Sign-On Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ddlPort" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="Sign-On Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOnDate" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            OnTextChangedEvent="CalculateReliefDue" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDurationMonths" runat="server" Text="Duration (Months)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDuration" runat="server" CssClass="input_mandatory txtNumber" OnTextChangedEvent="CalculateReliefDue" Width="50%"
                            AutoPostBack="true" MaxLength="3" IsInteger="true" MaskText="###" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReliefDue" runat="server" Text="Relief Due"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtReliefDueDate" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnTextChangedEvent="CalculateReliefDue" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOnReason" runat="server" Text="Sign-On Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignOnReason ID="ddlSignOnReason" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOnRemarks" runat="server" Text="Sign-On Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtSignonRemarks" runat="server" TextMode="MultiLine" MaxLength="200" Width="300px"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr id="trSG1" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblSignOffDate" runat="server" Text="Sign-Off Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOffDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffPort" runat="server" Text="Sign-Off Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ddlSignOffPort" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffReason" runat="server" Text="Sign-Off Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignOffReason ID="ddlSignOffReason" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr id="trSG2" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblSignOffRemarks" runat="server" Text="Sign-Off Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtSignOffRemarks" runat="server" TextMode="MultiLine"
                            Width="300px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadLabel ID="lblnote" runat="server" CssClass="guideline_text" Text="Note: For Adding previous sailing experience on ESM vessels ,click on the Add Button"></telerik:RadLabel>
            <eluc:TabStrip ID="MenuCrewFamilySignOn" runat="server" OnTabStripCommand="CrewFamilySignOn_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSignOn" runat="server" EnableViewState="false" Height="55%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnEditCommand="gvCrewSignOn_EditCommand"
                OnNeedDataSource="gvCrewSignOn_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvCrewSignOn_ItemDataBound"
                OnItemCommand="gvCrewSignOn_ItemCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONREMARKS") %>' />
                                <telerik:RadLabel ID="lblVessel" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="llnkVessel" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONRANK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-On Reason" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONREASON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-On Date" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-Off Date" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOffDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-Off Reason" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOffReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFREASON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>

        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
