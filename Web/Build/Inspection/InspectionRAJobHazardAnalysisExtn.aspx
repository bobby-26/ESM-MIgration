<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAJobHazardAnalysisExtn.aspx.cs" Inherits="InspectionRAJobHazardAnalysisExtn" %>

<!DOCTYPE html>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Hazard Analysis</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            table.Hazard {
                border-collapse: collapse;
            }

                table.Hazard td, table.Hazard th {
                    border: 1px solid;
                    border-color: rgb(30, 57, 91);
                    padding: 5px;
                }
        </style>

        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvHealthSafetyRisk.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>

        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvEnvironmentalRisk.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>

        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvEconomicRisk.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>

        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvevent.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>
        <style type="text/css">
            .checkboxstyle tbody tr td {
                width: 550px;
                vertical-align: top;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigure" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuJobHazardGeneral" runat="server" OnTabStripCommand="MenuJobHazardGeneral_TabStripCommand" Title="Details" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigure">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Process"></telerik:RadLabel>
                    </td>
                    <td width="23%">
                        <telerik:RadComboBox ID="ddlJob" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            DataTextField="FLDNAME" DataValueField="FLDCATEGORYID" Width="360px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblHazardNo" runat="server" Text="Hazard Number"></telerik:RadLabel>
                    </td>
                    <td width="19%">
                        <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtHazid" ReadOnly="true"
                            Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td width="14%">
                        <telerik:RadLabel ID="lblRevisionNo" runat="server" Text="Revision No"></telerik:RadLabel>
                    </td>
                    <td width="39%">
                        <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtRevNO" Width="70px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblJob" runat="server" Text="Job"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtJob" runat="server" CssClass="input_mandatory" Width="360px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPreparedBy" runat="server" Text="Prepared By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtpreparedby" ReadOnly="true"
                            Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucCreatedDate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblApprovedBy" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtApprovedby" ReadOnly="true"
                            Width="360px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate1" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <eluc:Date ID="ucApprovedDate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNoofPeopleInvolvedInActivity" runat="server" Text="No of People who may be affected"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPeopleInvolved" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="360px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIssuedBy" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtIssuedBy" ReadOnly="true"
                            Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate2" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIssuedDate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrequencyofWorkActivity" runat="server" Text="Frequency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlWorkFrequency" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="360px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" Enabled="false" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                        &nbsp;&nbsp;&nbsp;&nbsp 
                        <asp:LinkButton ID="lnkcomment" runat="server" ToolTip="Comments" Visible="false">
                                        <span class="icon"><i class="fa fa-comments"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDurationofWorkActivity" runat="server" Text="Duration"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlWorkDuration" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="360px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtStatus" runat="server" CssClass="readonlytextbox" Width="180px" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupervision" runat="server" Text="Supervision Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlsupervisionlist" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="270px" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblpersons" runat="server" Text="Persons carrying out job"></telerik:RadLabel>
                    </td>
                    <td valign="top" colspan="2">
                        <div id="divpersonsinvolved" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 420px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="Chkpersonsinvolved" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2"
                                AutoPostBack="false">
                            </asp:CheckBoxList>
                        </div>
                        <telerik:RadTextBox ID="txtpersonsinvolved" runat="server" Visible="false" CssClass="readonlytextbox" TextMode="MultiLine" Width="420px" Height="80px"
                            ReadOnly="true" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblConditionsForAdditionalRiskAssessment" runat="server" Text="Conditions for Additional Risk Assessment" Style="text-align: left"></telerik:RadLabel>
                    </td>
                    <td valign="top" colspan="2">
                        <div id="divcbl" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 420px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="cblReason" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2"
                                AutoPostBack="true" OnTextChanged="OtherDetailClick">
                            </asp:CheckBoxList>
                        </div>
                        <telerik:RadTextBox ID="txtcbl" runat="server" Visible="false" CssClass="readonlytextbox" TextMode="MultiLine" Width="420px" Height="80px"
                            ReadOnly="true" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblengcontrol" runat="server" Text="Equipment"></telerik:RadLabel>
                    </td>
                    <td valign="top" colspan="2">
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input"
                                Width="100px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input"
                                Width="300px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowComponents" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                            <asp:LinkButton ID="lnkComponentAdd" runat="server" OnClick="lnkComponentAdd_Click" ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                            </asp:LinkButton>
                            <div id="divComponents" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 420px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                <table id="tblcomponents" runat="server">
                                </table>
                            </div>
                        </span>
                    </td>
                    <td></td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtOtherReason" runat="server" CssClass="readonlytextbox" TextMode="MultiLine" Width="420px" Height="80px"
                            ReadOnly="true" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadDockZone ID="RadDockZone2" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock1" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblaspect" Text="Aspects" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkaspectcomment" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuOperationalHazard" runat="server" TabStrip="false" />
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvRAOperationalHazard" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" EnableViewState="true" OnNeedDataSource="gvRAOperationalHazard_NeedDataSource"
                                        Width="100%" CellPadding="3" OnItemCommand="gvRAOperationalHazard_ItemCommand" OnItemDataBound="gvRAOperationalHazard_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false" EnableHeaderContextMenu="true">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDJHAOPERATIONALHAZARDID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSNO") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Aspect" HeaderStyle-Width="15%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOperationalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHAOPERATIONALHAZARDID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblOperationalHazard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazards / Risks" HeaderStyle-Width="20%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazards" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONALHAZARD") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Controls / Precautions" HeaderStyle-Width="47%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="47%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblControls" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLPRECAUTIONS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="13%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="13%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" CommandName="ASPECTEDIT" ID="cmdAspectEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Map Hazard" CommandName="MAPIMPACT" ID="cmdEdit" ToolTip="Map Hazard">
                                                            <span class="icon"><i class="fas fa-biohazard"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="ASPECTDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadDockZone ID="RadDockZone7" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock7" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblAdditionalControls" Text="Controls" Font-Bold="true"></telerik:RadLabel>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <table width="99%">
                                        <tr>
                                            <td align="center" width="32%" valign="top">
                                                <telerik:RadLabel ID="lblFormscheck" runat="server" Style="background-color: #c6e0b4; width: 99%;">Forms and CheckList</telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvForms" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvForms_ItemCommand" OnNeedDataSource="gvForms_NeedDataSource" OnItemDataBound="gvForms_ItemDataBound" AllowSorting="false" ShowFooter="true" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="90%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="90%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMPOSTERID") %>'></asp:Label>
                                                                    <asp:Label ID="lbltype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                                                    <asp:LinkButton ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                 <FooterTemplate>
                                                                    <span id="spnPickListDocument">
                                                                        <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="90%" Enabled="False" Style="font-weight: bold">
                                                                        </telerik:RadTextBox>
                                                                        <asp:LinkButton ID="btnShowDocuments" runat="server" ToolTip="Select Forms and Checklists">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                                                        </asp:LinkButton>
                                                                        <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                                    </span>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Left" HeaderText="Action"  HeaderStyle-Width="10%">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>

                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="FORMDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                                        CommandName="FORMADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                                                        ToolTip="Add New">
                                         <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td width="2%"></td>
                                            <td align="center" width="32%" valign="top">
                                                <telerik:RadLabel ID="lbldocumentprocedures" runat="server" Style="background-color: #c6e0b4; width: 99%;" Text="Procedures"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvprocedure" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvprocedure_ItemCommand" OnNeedDataSource="gvprocedure_NeedDataSource" OnItemDataBound="gvprocedure_ItemDataBound" AllowSorting="true" ShowFooter="true" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="90%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="90%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMPOSTERID") %>'></asp:Label>
                                                                    <asp:Label ID="lbltype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                                                    <asp:LinkButton ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                               <FooterTemplate>
                                                                    <span id="spnPickListDocument1">
                                                                        <telerik:RadTextBox ID="txtDocumentName1" runat="server" Width="90%" Enabled="False" Style="font-weight: bold">
                                                                        </telerik:RadTextBox>
                                                                        <asp:LinkButton ID="btnShowDocuments1" runat="server" ToolTip="Select Procedures">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                                                        </asp:LinkButton>
                                                                        <telerik:RadTextBox ID="txtDocumentId1" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                                    </span>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Left" HeaderText="Action"  HeaderStyle-Width="10%">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>

                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="PROCEDUREDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                                        CommandName="PROCEDUREADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                                                        ToolTip="Add New">
                                         <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td width="2%"></td>
                                            <td align="center" width="32%" rowspan="2" valign="top">
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="2" width="100%" valign="top" align="center">
                                                             <telerik:RadLabel ID="lblPPE" runat="server" Style="background-color: #fbffe1; width: 99%;" Text="Recommended PPE"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="50%" valign="top" align="center">
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvPPE" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvPPE_NeedDataSource" OnItemDataBound="gvPPE_ItemDataBound" AllowSorting="true" ShowFooter="false" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Icon" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgPhoto" runat="server" Height="30px"
                                                                        Width="40px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="75%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="75%"></ItemStyle>
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                                            </td>                                                       
                                                        <td width="50%" valign="top" align="center">
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvPPE2" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvPPE_NeedDataSource" OnItemDataBound="gvPPE2_ItemDataBound" AllowSorting="true" ShowFooter="false" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Icon" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgPhoto" runat="server" Height="30px"
                                                                        Width="40px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="75%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="75%"></ItemStyle>
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="center" valign="top">
                                                <telerik:RadLabel ID="lblworkpermit" runat="server" Style="background-color: #c6e0b4; width: 99%;" Text="Work Permit"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvworkpermit" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvworkpermit_ItemCommand" OnNeedDataSource="gvworkpermit_NeedDataSource" OnItemDataBound="gvworkpermit_ItemDataBound" AllowSorting="true" ShowFooter="true" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Icon" HeaderStyle-Width="10%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgPhoto" runat="server" Height="30px"
                                                                        Width="40px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="80%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></asp:Label>
                                                                    <asp:Label ID="lbltype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                                                    <asp:LinkButton ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                   <span id="spnPickListDocument5">
                                                                        <telerik:RadTextBox ID="txtDocumentName5" runat="server" Width="90%" Enabled="False" Style="font-weight: bold">
                                                                        </telerik:RadTextBox>
                                                                        <asp:LinkButton ID="btnShowDocuments5" runat="server" ToolTip="Select Work Permit">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                                                        </asp:LinkButton>
                                                                        <telerik:RadTextBox ID="txtDocumentId5" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                                    </span>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Left" HeaderText="Action"  HeaderStyle-Width="10%">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>

                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="WORKPERMITDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                                        CommandName="WORKPERMITADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                                                        ToolTip="Add New">
                                         <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td></td>
                                            <td align="center" valign="top">
                                                <telerik:RadLabel ID="lblEPSS" runat="server" Text="EPSS" Style="background-color: #c6e0b4; width: 99%;"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvEPSS" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvEPSS_ItemCommand" OnNeedDataSource="gvEPSS_NeedDataSource" OnItemDataBound="gvEPSS_ItemDataBound" AllowSorting="true" ShowFooter="true" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>

                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="90%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></asp:Label>
                                                                    <asp:Label ID="lbltype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                                                    <asp:LinkButton ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <span id="spnPickListDocument3">
                                                                        <telerik:RadTextBox ID="txtDocumentName3" runat="server" Width="90%" Enabled="False" Style="font-weight: bold">
                                                                        </telerik:RadTextBox>
                                                                        <asp:LinkButton ID="btnShowDocuments3" runat="server" ToolTip="Select EPSS">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                                                        </asp:LinkButton>
                                                                        <telerik:RadTextBox ID="txtDocumentId3" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                                    </span>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Left" HeaderText="Action"  HeaderStyle-Width="10%">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>

                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="EPSSDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                                        CommandName="EPSSADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                                                        ToolTip="Add New">
                                         <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>

                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadDockZone ID="RadDockZone6" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock6" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblhazards" Text="Hazards and Score Matrix" Font-Bold="true"></telerik:RadLabel>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <table width="99%">
                                        <tr>
                                            <td align="center" width="23%" valign="top">
                                                <telerik:RadLabel ID="lblhealth" runat="server" Style="background-color: #ffe699; width: 99%;" Text="Health and Safety Hazards"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvhazards" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvhazards_NeedDataSource" OnItemDataBound="gvhazards_ItemDataBound" AllowSorting="true" ShowFooter="false" ShowHeader="false" EnableViewState="false">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Icon" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgPhoto" runat="server" Height="30px"
                                                                        Width="40px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="75%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="75%"></ItemStyle>
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td width="2%"></td>
                                            <td align="center" width="23%" valign="top">
                                                <telerik:RadLabel ID="lblenvironmental1" runat="server" Style="background-color: #ffe699; width: 99%;" Text="Environmental Hazards"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvenvironmental" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvenvironmental_NeedDataSource" OnItemDataBound="gvenvironmental_ItemDataBound" AllowSorting="true" ShowFooter="false" ShowHeader="false" EnableViewState="false">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Icon" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgPhoto" runat="server" Height="30px"
                                                                        Width="40px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="75%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="75%"></ItemStyle>
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td width="2%"></td>
                                            <td style="vertical-align: top" width="50%">
                                                <table class="Hazard">
                                                    <tr>
                                                        <td align="center" id="Td" width="24%"></td>
                                                        <td align="center" style="background-color: #ffe699;" width="19%">
                                                            <telerik:RadLabel ID="lblHealthSafety" runat="server">Health and Safety</telerik:RadLabel>
                                                        </td>
                                                        <td align="center" style="background-color: #ffe699;" width="19%">
                                                            <telerik:RadLabel ID="lblEnviormental" runat="server">Environmental</telerik:RadLabel>
                                                        </td>
                                                        <td align="center" style="background-color: #ffe699;" width="19%">
                                                            <telerik:RadLabel ID="lblEconomic" runat="server">Economic/Process Loss</telerik:RadLabel>
                                                        </td>
                                                        <td align="center" style="background-color: #ffe699;" width="19%">
                                                            <telerik:RadLabel ID="lblWorst" runat="server">Worst Case</telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="impact" runat="server">
                                                            <telerik:RadLabel ID="lblimpact" runat="server" Text="Impact"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="impacthealth" runat="server">
                                                            <telerik:RadLabel ID="lblimpacthealth" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="impactenv" runat="server">
                                                            <telerik:RadLabel ID="lblimpactenv" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="impacteco" runat="server">
                                                            <telerik:RadLabel ID="lblimpacteco" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="impactws" runat="server">
                                                            <telerik:RadLabel ID="lblimpactws" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="POO" runat="server">
                                                            <telerik:RadLabel ID="lblPOO" runat="server" Text="POO"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="POOhealth" runat="server">
                                                            <telerik:RadLabel ID="lblPOOhealth" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="POOenv" runat="server">
                                                            <telerik:RadLabel ID="lblPOOenv" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="POOeco" runat="server">
                                                            <telerik:RadLabel ID="lblPOOeco" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="POOws" runat="server">
                                                            <telerik:RadLabel ID="lblPOOws" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="loh" runat="server">
                                                            <telerik:RadLabel ID="lblloh" runat="server" Text="LOH"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="lohhealth" runat="server">
                                                            <telerik:RadLabel ID="lbllohhealth" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="lohenv" runat="server">
                                                            <telerik:RadLabel ID="lbllohenv" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="loheco" runat="server">
                                                            <telerik:RadLabel ID="lblloheco" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="lohws" runat="server">
                                                            <telerik:RadLabel ID="lbllohws" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="tdControls" runat="server">
                                                            <telerik:RadLabel ID="lblControlstxt" runat="server" Text="Controls"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="Controlshealth" runat="server">
                                                            <telerik:RadLabel ID="lblControlshealth" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="Controlsenv" runat="server">
                                                            <telerik:RadLabel ID="lblControlsenv" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="Controlseco" runat="server">
                                                            <telerik:RadLabel ID="lblControlseco" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="Controlsws" runat="server">
                                                            <telerik:RadLabel ID="lblControlsws" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="level" runat="server">
                                                            <telerik:RadLabel ID="lblLevel" runat="server" Text="Risk Level"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="levelofriskhealth" runat="server">
                                                            <telerik:RadLabel ID="lblLevelofRiskHealth" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="levelofriskenv" runat="server">
                                                            <telerik:RadLabel ID="lblLevelofRiskEnv" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="levelofriskeco" runat="server">
                                                            <telerik:RadLabel ID="lblLevelofRiskEconomic" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="levelofriskworst" runat="server">
                                                            <telerik:RadLabel ID="lblLevelofRiskWorst" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="Td1" runat="server">
                                                            <telerik:RadLabel ID="lblcontrols" runat="server" Text="Additonal Controls due to Supervision / Checklist"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdhscontrols" runat="server">
                                                            <telerik:RadLabel ID="lblhscontrols" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdencontrols" runat="server">
                                                            <telerik:RadLabel ID="lblencontrols" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdecocontrols" runat="server">
                                                            <telerik:RadLabel ID="lbleccontrols" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdwscontrols" runat="server">
                                                            <telerik:RadLabel ID="lblwscontrols" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="tdresrisk" runat="server">
                                                            <telerik:RadLabel ID="lblresrisk" runat="server" Text="Residual Risk"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdreshsrisk" runat="server">
                                                            <telerik:RadLabel ID="lblreshsrisk" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdresenrisk" runat="server">
                                                            <telerik:RadLabel ID="lblresenrisk" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdresecorisk" runat="server">
                                                            <telerik:RadLabel ID="lblresecorisk" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdreswsrisk" runat="server">
                                                            <telerik:RadLabel ID="lblreswsrisk" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr valign="top">
                                                        <td align="center" id="Description" runat="server"></td>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="lblHealthDescription" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="lblEnvDescription" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="lblEconomicDescription" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="lblWorstDescription" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="6">
                        <telerik:RadDockZone ID="RadDockZone1" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock2" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblhealthcomments" Text="Health and Safety Risk" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkhealthcomments" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvHealthSafetyRisk" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" EnableViewState="true" OnNeedDataSource="gvHealthSafetyRisk_NeedDataSource"
                                        Width="100%" CellPadding="3" OnItemCommand="gvHealthSafetyRisk_ItemCommand" OnItemDataBound="gvHealthSafetyRisk_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDHEALTHHAZARDID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="4%">
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Aspects" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAspects" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECTS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard / Risk" HeaderStyle-Width="20%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardRisk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDRISK") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard Type" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Undesirable Event" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblUndesirableEvent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNDISIRABLEEVENTLIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHealthid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEALTHHAZARDID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Impact" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Equipment" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEquipment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Operational Control" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOperational" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONALCONTROL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="PPE" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPPE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPPELIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="HMAPPING" ID="cmdEquipment" ToolTip="Controls Mapping">
                                                    <span class="icon"><i class="fas fa-cogs"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="HEALTHSAFETYDELETE" ID="cmddelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadDockZone ID="RadDockZone3" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock3" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblEnvironmental" Text="Environmental Risk" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkEnvironmental" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvEnvironmentalRisk" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" OnNeedDataSource="gvEnvironmentalRisk_NeedDataSource"
                                        CellPadding="3" OnItemCommand="gvEnvironmentalRisk_ItemCommand" OnItemDataBound="gvEnvironmentalRisk_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDHEALTHHAZARDID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="4%">
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Aspects" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>

                                                        <telerik:RadLabel ID="lblAspects" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECTS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard / Risk" HeaderStyle-Width="20%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardRisk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDRISK") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard Type" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Undesirable Event" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblUndesirableEvent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNDISIRABLEEVENTLIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Impact Type" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblImpactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHealthid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEALTHHAZARDID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Impact" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Equipment" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEquipment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Operational Control" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOperational" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONALCONTROL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="ENVMAPPING" ID="cmdEquipment" ToolTip="Controls Mapping">
                                                    <span class="icon"><i class="fas fa-cogs"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="ENVIRONMENTALDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadDockZone ID="RadDockZone4" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock4" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblEconomicrisk" Text="Economic Risk" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkEconomic" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvEconomicRisk" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" OnNeedDataSource="gvEconomicRisk_NeedDataSource"
                                        CellPadding="3" OnItemCommand="gvEconomicRisk_ItemCommand" OnItemDataBound="gvEconomicRisk_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDHEALTHHAZARDID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="5%">
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Aspects" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAspects" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECTS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard / Risk" HeaderStyle-Width="20%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardRisk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDRISK") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard Type" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Undesirable Event" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblUndesirableEvent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNDISIRABLEEVENTLIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazard" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHealthid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEALTHHAZARDID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Impact" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Equipment" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEquipment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Operational Control" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOperational" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONALCONTROL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="9%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="9%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="ECOMAPPING" ID="cmdEquipment" ToolTip="Controls Mapping">
                                                    <span class="icon"><i class="fas fa-cogs"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="ECONOMICDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadDockZone ID="RadDockZone5" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock5" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblUndesireable" Text="Undesirable Event / Worst Case Scenario" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkUndesireable" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvevent" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" Width="100%" EnableViewState="false"
                                        CellPadding="3" OnNeedDataSource="gvevent_NeedDataSource" OnItemCommand="gvevent_ItemCommand" OnItemDataBound="gvevent_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" EnableHeaderContextMenu="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDWORSTCASEID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="5%">
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Undesirable Event">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblevent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Risks">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRisks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKESCALATION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Procedures">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <div id="divProcedures" runat="server" style="width: auto; border-width: 1px; border-style: solid; border: 0px solid #9f9fff">
                                                            <table id="tblProcedures" runat="server">
                                                            </table>
                                                        </div>
                                                        <%--<telerik:RadLabel ID="lblProcedures" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURES") %>'></telerik:RadLabel>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Equipment">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEquipment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="PPE">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPPE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPPELIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Worst Case">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblWorstCaseid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORSTCASEID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblWorstCase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORSTCASENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="EMAPPING" ID="cmdEquipment" ToolTip="Controls Mapping">
                                                    <span class="icon"><i class="fas fa-cogs"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="UDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </telerik:RadAjaxPanel>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="700px" Height="650px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
            VisibleStatusbar="false" KeepInScreenBounds="true">
            <ContentTemplate>
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1" OnAjaxRequest="RadAjaxPanel2_AjaxRequest">
                    <table id="tbl1" border="0" style="width: 100%" runat="server" visible="false" cellspacing="5">
                        <tr>
                            <td valign="top" width="10%">
                                <telerik:RadLabel ID="lblequipnet" runat="server" Text="Equipment"></telerik:RadLabel>
                            </td>
                            <td valign="top" width="50%">
                                <telerik:RadCheckBoxList ID="chkEquipment" runat="server" CssClass="checkboxstyle" DataBindings-DataTextField="FLDCOMPONENTNAME" DataBindings-DataValueField="FLDCOMPONENTID" Style="height: 120px; width: 99%; overflow-y: auto; overflow-x: auto; border-width: 1px; border-style: solid; border: 1px solid #c3cedd"></telerik:RadCheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="10%">
                                <telerik:RadLabel ID="lblEvent" runat="server" Text="Undesirable Event"></telerik:RadLabel>
                            </td>
                            <td valign="top" width="50%">
                                <telerik:RadCheckBoxList ID="chkEvent" runat="server" AutoPostBack="false" CssClass="checkboxstyle" DataBindings-DataTextField="FLDUNDESIRABLEEVENTNAME" DataBindings-DataValueField="FLDUNDESIRABLEEVENTID" Style="height: 120px; width: 99%; overflow-y: auto; overflow-x: auto; border-width: 1px; border-style: solid; border: 1px solid #c3cedd"></telerik:RadCheckBoxList>
                            </td>
                        </tr>

                        <tr id="trPPE" runat="server" visible="false">
                            <td valign="top">
                                <telerik:RadLabel ID="lblPPETEXT" runat="server" Text="PPE"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:CheckBoxList ID="cblRecomendedPPE" runat="server" CssClass="checkboxstyle" RepeatDirection="Vertical" RepeatColumns="3" DataTextField="FLDNAME" DataValueField="FLDMISCELLANEOUSID" Style="height: 120px; width: 99%; overflow-y: auto; overflow-x: auto; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <telerik:RadButton ID="btnCreate" Text="Save" runat="server" OnClick="btnCreate_Click" Font-Bold="true"></telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                    <table id="tbl2" border="0" style="width: 100%" runat="server" visible="false" cellspacing="5">
                        <tr>
                            <td valign="top" width="20%">
                                <telerik:RadLabel ID="lblProcedure" runat="server" Text="Procedure"></telerik:RadLabel>
                            </td>
                            <td width="80%">
                                <span id="spnPickListProcedure">
                                    <telerik:RadTextBox ID="txtProcedure" runat="server" Width="403px" Style="font-weight: bold"
                                        CssClass="input">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="lnkProcedureList" runat="server" ToolTip="Select Procedures, Forms and Checklists">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtProcedureid" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                                <asp:LinkButton ID="lnkProcedureAdd" runat="server" OnClick="lnkProcedureAdd_Click" ToolTip="Add">
                                    <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                </asp:LinkButton>
                                <br />
                                <asp:CheckBoxList ID="cbProcedure" runat="server" DataTextField="FLDPROCEDURENAME" Style="width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd"
                                    DataValueField="FLDPROCEDUREID" AutoPostBack="true" CssClass="checkboxstyle" OnSelectedIndexChanged="cbProcedure_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <telerik:RadLabel ID="lblEquipment" runat="server" Text="Equipment"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickListEquipment" runat="server" visible="false">
                                    <telerik:RadTextBox ID="txtEquipmentCode" runat="server" CssClass="input"
                                        Width="100px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtEquipment" runat="server" CssClass="input"
                                        Width="300px">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="lnkEquipmentList" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtEquipmentid" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                                    <asp:LinkButton ID="lnkEquipmentAdd" runat="server" OnClick="lnkEquipmentAdd_Click" ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </span>
                                <asp:CheckBoxList ID="cbEventEquipment" runat="server" DataTextField="FLDCOMPONENTNAME"
                                    DataValueField="FLDCOMPONENTID" RepeatDirection="Vertical" CssClass="checkboxstyle" RepeatColumns="2" Style="width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <telerik:RadLabel ID="lblEventPPE" runat="server" Text="PPE"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:CheckBoxList ID="cbEventPPE" runat="server" RepeatDirection="Vertical" RepeatColumns="3"
                                    DataTextField="FLDNAME" DataValueField="FLDMISCELLANEOUSID" CssClass="checkboxstyle" Style="width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <telerik:RadLabel ID="lblWorstCase" runat="server" Text="Worst Case"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlWorstCase" runat="server" AppendDataBoundItems="true" EmptyMessage="Type to select" Filter="Contains"
                                    MarkFirstMatch="true" Width="270px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="Center">
                                <telerik:RadButton ID="btnEventsave" Text="Save" runat="server" OnClick="btnEventsave_Click"></telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </telerik:RadAjaxPanel>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
