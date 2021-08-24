<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMocExtentionAdd.aspx.cs" Inherits="InspectionMocExtentionAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MOC Extention</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function ConfirmRevision(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRevision.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        table.Hazard {
            border-collapse: collapse;
        }

            table.Hazard td, table.Hazard th {
                border: 1px solid black;
                padding: 5px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
             <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MOCExtentionAdd" runat="server" OnTabStripCommand="MOCExtentionAdd_TabStripCommand" Title="Extention (By: Responsible person)" />
            <table width="100%">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblOTDate" runat="server" Text="Original target date"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <eluc:Date ID="OriginalDate" runat="server" Enabled="false" />
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblRTDate" runat="server" Text="Revised target date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="RevisedDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblExtention" runat="server" Text="1. Is there any need to change the agreed target date & process"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rblMOCExtetion" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Table">
                            <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblHeadingRAReview" runat="server" Text="2. Risk Assessment review" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblRAReview" runat="server" Text="Risk Assessment has been reviewed and found adequate"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rblRAReview" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Table">
                            <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone7" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock7" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblRA" Text="RiskAssessment" Font-Bold="true"></telerik:RadLabel>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblRAType" runat="server" Text="Risk Assessment">
                                                </telerik:RadLabel>
                                            </td>
                                            <td>
                                                <span id="spnRA">
                                                    <telerik:RadTextBox ID="txtRANumber" runat="server" RenderMode="Lightweight" CssClass="readonlytextbox"
                                                        MaxLength="50" Width="150px" Text="">
                                                    </telerik:RadTextBox>
                                                    <telerik:RadTextBox ID="txtRA" runat="server" RenderMode="Lightweight" CssClass="readonlytextbox"
                                                        MaxLength="50" Width="250px" Text="">
                                                    </telerik:RadTextBox>
                                                    <telerik:RadTextBox ID="txtRAId" runat="server" RenderMode="Lightweight" CssClass="hidden"
                                                        MaxLength="20" Width="0px" Text="">
                                                    </telerik:RadTextBox>
                                                    <telerik:RadTextBox ID="txtRaType" runat="server" RenderMode="Lightweight" CssClass="hidden"
                                                        MaxLength="2" Width="0px" Text=''>
                                                    </telerik:RadTextBox>
                                                </span>&nbsp
                    <asp:ImageButton runat="server" AlternateText="Show RA Details" ImageUrl="<%$ PhoenixTheme:images/BarChart.png %>"
                        ID="ImgRA" ToolTip="Show PDF" />
                                                <asp:LinkButton ID="lnkEditRA" runat="server" OnClick=" lnkEditRA_Click" Text="Edit/View"></asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="imgrevision" ToolTip="Revision" OnClick="imgrevision_Click">
                                    <span class="icon"><i class="fas fa-registered"></i></span>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table class="Hazard" width="100%">
                                                    <tr>
                                                        <td align="center" id="Td" width="20%"></td>
                                                        <td align="center" style="background-color: rgb(255,230,110);" width="20%">
                                                            <telerik:RadLabel ID="lblHealthSafety" runat="server">Health and Safety</telerik:RadLabel>
                                                        </td>
                                                        <td align="center" style="background-color: rgb(155,255,166);" width="20%">
                                                            <telerik:RadLabel ID="lblEnviormental" runat="server">Environmental</telerik:RadLabel>
                                                        </td>
                                                        <td align="center" style="background-color: rgb(251,255,225);" width="20%">
                                                            <telerik:RadLabel ID="lblEconomic" runat="server">Economic/Process Loss</telerik:RadLabel>
                                                        </td>
                                                        <td align="center" style="background-color: rgb(255,216,44);" width="20%">
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
                                                            <telerik:RadLabel ID="lblLevel" runat="server" Text="Risk Levels from Sections A & B"></telerik:RadLabel>
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
                                                            <telerik:RadLabel ID="lblcontrols" runat="server" Text="Controls due to Supervision / Checklist"></telerik:RadLabel>
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
            </table>
            <table width="100%">
                <tr>
                    <td width="40%">
                        <telerik:RadLabel ID="lblActionPlan" runat="server" Text="3. Action Plan has been reviewed and found adequate"></telerik:RadLabel>
                    </td>
                    <td width="25%">
                        <asp:RadioButtonList ID="rblActionPlan" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Table">
                            <asp:ListItem Selected="True" Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <eluc:TabStrip ID="MOCActionPlan" runat="server" />
                        <telerik:RadGrid ID="gvMOCActionPlan" runat="server" AutoGenerateColumns="False" OnNeedDataSource="gvMOCActionPlan_NeedDataSource" Font-Size="11px"
                            CellPadding="3" OnItemCommand="gvMOCActionPlan_ItemCommand" AllowPaging="false" AllowCustomPaging="false" OnItemDataBound="gvMOCActionPlan_ItemDataBound"
                            ShowFooter="false" EnableHeaderContextMenu="true" GroupingEnabled="false" ShowHeader="true" EnableViewState="true" AllowSorting="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDMOCACTIONPLANID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
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
                                    <telerik:GridTemplateColumn HeaderText="Actions to be taken">
                                        <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblMOCActionPlanid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCACTIONPLANID") %>'>
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblActionToBeTaken" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDACTIONTOBETAKEN") %>'>
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblMOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Person In Charge">
                                        <HeaderStyle Width="20%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'>
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblPICRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICRANK") %>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Target">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTargetDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'
                                                Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Completion">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompletionDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'
                                                Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Closed">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblClosedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE")) %>'
                                                Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Status">
                                        <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Task" CommandName="EDITTASK" ID="lnkTaskEdit" Visible="false"
                                                ToolTip="Task Edit">
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITROW" ID="cmdEdit"
                                                ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                                ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMOCCommitteInvoledExtention" runat="server" Text="4. MOC Committee Involved">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblMOCCommitteInvoledExtention" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Table" OnSelectedIndexChanged="rblMOCCommitteInvoledExtention_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMOCMeetingExtention" runat="server" Text="Date of MOC Meeting">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtMOCMeetingExtention" runat="server" CssClass="gridinput" DatePicker="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMOCcommitteeExtention" runat="server" Text="Name and Designation of other MOC Committee Members">
                        </telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtMOCcommitteeExtention" runat="server" CssClass="gridinput" Enabled="false"
                            Width="23%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblapprover" runat="server" Text="Approving Authority (Name & Rank)*">
                        </telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <span id="Spnextentionoffice" runat="server">
                            <telerik:RadTextBox ID="txtextentionofficename" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="23%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtextentionofficerank" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="23%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgextentionoffice" Visible="false"><span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtextentionofficeid" runat="server" CssClass="input" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtextentionofficemail" runat="server" CssClass="input" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                        </span>
                        <telerik:RadLabel ID="lblextentiondtkey" runat="server" Visible="false">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <asp:Button ID="ucConfirmRevision" runat="server" Text="confirmRevision" OnClick="ucConfirmRevision_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
