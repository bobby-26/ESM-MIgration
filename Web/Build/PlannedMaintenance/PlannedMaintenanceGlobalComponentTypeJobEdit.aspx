<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalComponentTypeJobEdit.aspx.cs"
    Inherits="PlannedMaintenanceGlobalComponentTypeJobEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TypeJobs" Src="~/UserControls/UserControlMultiColumnComponentTypeJob.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PTW" Src="~/UserControls/UserControlMultiColumnPMSPTWA.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RA" Src="~/UserControls/UserControlMultiColumnPMSRA.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized() {
            var sender = $find('pgGeneral');
            var browserHeight = $telerik.$(window).height();
            sender.set_height(browserHeight - 80);
            var sender1 = $find('pgHistory');
            var browserHeight = $telerik.$(window).height();
            sender1.set_height(browserHeight - 100);

            //var sender2 = $find('pgManuals');
            // sender2.set_height(browserHeight - 80);
            //$telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 150);
        }

        (function (global, undefined) {
            var textBox = null;

            function textBoxLoad(sender) {
                textBox = sender;
            }

            function OpenFileExplorerDialog() {
                global.radopen("PlannedMaintenaneManualsExplorer.aspx", "PMS Manual");
            }

            //This function is called from a code declared on the Explorer.aspx page
            function OnFileSelected(fileSelected) {
                if (textBox) {
                    textBox.set_value(fileSelected);
                }
            }

            global.OpenFileExplorerDialog = OpenFileExplorerDialog;
            global.OnFileSelected = OnFileSelected;
            global.textBoxLoad = textBoxLoad;
        })(window);

    </script>
</head>
<body onresize="PaneResized();" onload="PaneResized();">
    <form id="frmPlannedMaintenanceJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server" />
      <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
               
                <telerik:AjaxSetting AjaxControlID="gvHistoryTemplateList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvHistoryTemplateList" UpdatePanelHeight="95%"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvIncludeJobs">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvIncludeJobs" UpdatePanelHeight="80%"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvManual">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvManual" UpdatePanelHeight="85%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError" ></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ucTypeJobs">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ucTypeJobs"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="txtTemplateName">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvHistoryTemplateList" UpdatePanelHeight="95%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="txtTemplateName"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand"></eluc:TabStrip>
            </div>

            <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
                <telerik:RadPageView runat="server" Height="100%" ID="pgGeneral">
                    <div style="font-weight: 600; font-size: 12px;" runat="server">
                        <eluc:TabStrip ID="MenuPlannedMaintenance" runat="server" OnTabStripCommand="PlannedMaintenance_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <telerik:RadAjaxPanel ID="RadPanel1" runat="server">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblMake" runat="server" Text="Make"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtMake" Enabled="false" Width="180px"></telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtType" runat="server" Enabled="false" Width="180px"></telerik:RadTextBox>
                                    <asp:HiddenField ID="hdnModel"  runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtNumber" runat="server" Enabled="false" Width="180px"></telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtName" runat="server" Enabled="false" Width="180px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtCode" runat="server" Enabled="false" Width="180px"></telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtTitle" runat="server" Enabled="false" Width="180px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblJobClass" runat="server" Text="Class"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Quick ID="ucJobClass" runat="server" Width="180px" Enabled="false"
                                        QuickTypeCode="34" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblJobCategory" runat="server" Text="Category"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Quick ID="ucCategory" runat="server" Width="180px" Enabled="false"
                                        QuickTypeCode="165" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <hr />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblActive" RenderMode="Lightweight" Text="Active"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox runat="server" ID="chkActive" RenderMode="Lightweight"></telerik:RadCheckBox>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <hr />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtFrequency" RenderMode="Lightweight" InputType="Number" runat="server" Width="88px"></telerik:RadTextBox>
                                    <eluc:Hard ID="ucFrequency" runat="server" HardTypeCode="7" Width="88px" />
                                </td>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblCounter" runat="server" Text="Counter"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtCounter" RenderMode="Lightweight" InputType="Number" runat="server" Width="88px"></telerik:RadTextBox>
                                    <eluc:Hard ID="ucCounter" runat="server" HardTypeCode="111" Width="88px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblHistory" runat="server" Text="History Mandatory"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkHistory" runat="server" RenderMode="Lightweight"></telerik:RadCheckBox>
                                </td>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblResponsibility" runat="server" Text="Responsiblity"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Discipline ID="ucResponsibility" runat="server" AppendDataBoundItems="true" Width="180px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblRAMandatory" runat="server" Text="Risk Assessment"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkRA" runat="server" RenderMode="Lightweight"></telerik:RadCheckBox>
                                    <eluc:RA ID="ucRA" runat="server" Vessel="0" Width="140px" DropDownWidth="600" Visible="false" />
                                </td>
                                <td colspan="2">
                                    
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPanningMethod" runat="server" Text="Planning Method"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Hard ID="ucPlanningMethod" runat="server" HardTypeCode="5" Width="180px" />
                                </td>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblWindow" runat="server" Text="Window (Days)"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtWindow" RenderMode="Lightweight" runat="server" InputType="Number" Width="180px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblReference" runat="server" Text="Reference to User Manual"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtReference" Width="180px"></telerik:RadTextBox>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </telerik:RadAjaxPanel>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" Height="100%" ID="pgHistory">
                    <telerik:RadAjaxPanel ID="pnlHistory" runat="server">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblFormName" Text="Form Name"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtTemplateName" Width="180px" OnTextChanged="txtTemplateName_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvHistoryTemplateList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Width="100%" Height="480px"
                                CellSpacing="0" GridLines="None" OnItemCommand="gvHistoryTemplateList_ItemCommand" OnNeedDataSource="gvHistoryTemplateList_NeedDataSource" OnItemDataBound="gvHistoryTemplateList_ItemDataBound">
                                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                                        <telerik:GridTemplateColumn HeaderText="S No.">
                                            <HeaderStyle Width="57px" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblserialnoitem" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Template Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDFORMNAME">
                                            <HeaderStyle Width="600px" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblFormID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblFormType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                                                <asp:LinkButton ID="lnkTemplateName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'
                                                    CommandName="EDIT" CommandArgument="<%# Container.DataItem %>"></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox ID="txtFormIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadTextBox>
                                                <telerik:RadLabel ID="lblTemplateNameEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></telerik:RadLabel>
                                                <%--<asp:TextBox ID="txtTemplateNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'
                                CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>--%>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Include">
                                            <HeaderStyle Width="127px" />
                                            <ItemStyle Width="20px" />
                                            <ItemTemplate>
                                                <center>
                                <telerik:RadCheckBox ID="chkVerified" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? true:false%>'
                                    Enabled="false" />
                            </center>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <center>
                                <telerik:RadCheckBox ID="chkVerifiedEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? true : false%>'
                                    Enabled="true"></telerik:RadCheckBox>
                            </center>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Edit"
                                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Save"
                                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                                </asp:LinkButton>
                                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                                </asp:LinkButton>
                                            </EditItemTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                </telerik:RadPageView>
                    <telerik:RadPageView runat="server" Height="100%" ID="pgInclude">
                    <telerik:RadAjaxPanel ID="pnlInclude" runat="server">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblIJobCode" Text="Job Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" Enabled="false" ReadOnly="true" ID="txtICode" runat="server"></telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblIJobTitle" Text="Title"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" Enabled="false" ReadOnly="true" ID="txtITitle" Width="100%"></telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblIFrequency" Text="Frequency"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtIFrequency" Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                            <br clear="all" />
                            <table style="width:100%">
                                <tr>
                                    <td style="width:10%">
                                        <telerik:RadLabel RenderMode="Lightweight" Text="Job" runat="server" ID="lblJob"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:TypeJobs runat="server" ID="ucTypeJob" CssClass="input_mandatory" Width="100%" DropDownWidth="800"/>
                                    </td>
                                    <td style="width:10%;text-align:center;">
                                        <telerik:RadFormDecorator ID="radFormDecorator" DecoratedControls="All" CssClass="rgRow" DecorationZoneID="cmdAdd" runat="server" RenderMode="Lightweight" />
                                        <asp:LinkButton runat="server" AlternateText="Add" ID="cmdAdd"
                                                    ToolTip="Add" Width="20px" Height="20px" OnClick="cmdAdd_Click">
                                                    <span class="icon"><i class="fas fa-plus"></i></span>
                                                </asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvIncludeJobs" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" Width="100%" Height="430px"
                                CellSpacing="0" GridLines="None" OnItemCommand="gvIncludeJobs_ItemCommand" OnNeedDataSource="gvIncludeJobs_NeedDataSource" OnItemDataBound="gvIncludeJobs_ItemDataBound">
                                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="None">
                                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="true" AddNewRecordText="Include Job" ShowExportToPdfButton="false" />
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Code" UniqueName="CODE" >
                                            <HeaderStyle Width="20%" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                                <asp:LinkButton ID="lnkJobCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'
                                                    CommandName="SELECT" CommandArgument="<%# Container.DataItem %>"></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:TypeJobs runat="server" ID="ucTypeJobs" />
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Title" UniqueName="TITLE">
                                            <HeaderStyle Width="40%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblTitle" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn HeaderText="Frequency" UniqueName="FREQUENCY">
                                            <HeaderStyle Width="20%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblFrequency" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCY") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Delete"
                                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                                    ToolTip="Delete" Width="20px" Height="20px">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="pgManuals" Height="100%">
                    <telerik:RadAjaxPanel ID="pnlManuals" runat="server">
                        <div style="font-weight: 600; font-size: 12px;" runat="server">
                            <eluc:TabStrip ID="MenuPMSManual" runat="server" OnTabStripCommand="MenuPMSManual_TabStripCommand"></eluc:TabStrip>
                        </div>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvManual" runat="server" Height="460px" OnItemDataBound="gvManual_ItemDataBound"
                            OnNeedDataSource="gvManual_NeedDataSource" OnItemCommand="gvManual_ItemCommand" OnSortCommand="gvManual_SortCommand"
                            ShowFooter="true" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
                            <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDMANUALID">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Name">
                                        <HeaderStyle Width="50%" />
                                        <ItemStyle Width="50%" />
                                        <ItemTemplate>
                                            <%#((DataRowView)Container.DataItem)["FLDMANUALNAME"]%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="fileName" runat="server" Width="325px">
                                                <ClientEvents OnLoad="textBoxLoad" />
                                            </telerik:RadTextBox>
                                            <telerik:RadButton ID="selectFile" OnClientClicked="OpenFileExplorerDialog" AutoPostBack="false" Text="Open" runat="server">
                                            </telerik:RadButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Path">
                                        <HeaderStyle Width="40%" />
                                        <ItemStyle Width="40%" />
                                        <ItemTemplate>
                                            <%#((DataRowView)Container.DataItem)["FLDMANUALPATH"]%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Wrap="false" Width="10%" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                        
                    </telerik:RadAjaxPanel>
                </telerik:RadPageView>
                <telerik:RadPageView ID="pgPTW" runat="server" Height="100%">
                    <telerik:RadAjaxPanel ID="pnlPTW" runat="server">
                        <table style="width: 100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblPTWJobCode" Text="Job Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" Enabled="false" ReadOnly="true" ID="txtPTWJobCode" runat="server"></telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblPTWTitle" Text="Title"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" Enabled="false" ReadOnly="true" ID="txtPTWTitle" Width="100%"></telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblPTWFrequency" Text="Frequency"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtPTWFrequency" Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        <br clear="all" />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvPTW" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" Width="100%" Height="430px"
                                CellSpacing="0" GridLines="None" ShowFooter="true" OnItemCommand="gvPTW_ItemCommand" OnNeedDataSource="gvPTW_NeedDataSource" OnItemDataBound="gvPTW_ItemDataBound">
                                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="None">
                                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="true" AddNewRecordText="Include Job" ShowExportToPdfButton="false" />
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Form No" UniqueName="FORMNO" >
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPPINGID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Category" UniqueName="CATEGORY">
                                            <HeaderStyle Width="30%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblCategory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                                            <HeaderStyle Width="40%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel RenderMode="Lightweight" runat="server" Visible="false" ID="lblPTWId" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPTWID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <eluc:PTW runat="server" ID="ucPTW" Type="1" DropDownWidth="500" Width="100%" ExpandDirection="Up" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Remarks" UniqueName="REMARKS">
                                            <HeaderStyle Width="20%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblRemarks" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Delete"
                                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                                    ToolTip="Delete" Width="20px" Height="20px">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Add"
                                                    CommandName="ADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                                    ToolTip="Add" Width="20px" Height="20px">
                                                    <span class="icon"><i class="fas fa-plus"></i></span>
                                                </asp:LinkButton>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <telerik:RadWindowManager ID="theWindowManager" runat="server">
            <Windows>
                <telerik:RadWindow runat="server" Width="700px" Height="500px" VisibleStatusbar="false"
                    ShowContentDuringLoad="false" NavigateUrl="PlannedMaintenaneManualsExplorer.aspx" ID="ExplorerWindow"
                    Modal="true" Behaviors="Close,Move">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        </div>
    </form>
</body>
</html>
