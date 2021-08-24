<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCRequestAdd.aspx.cs"
    Inherits="InspectionMOCRequestAdd" MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

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
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
            function ConfirmRevision(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRevision.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function fnConfirmTelerik(sender, msg) {
                var callBackFn = function (shouldSubmit) {
                    if (shouldSubmit) {
                        //sender.click();
                        //if (Telerik.Web.Browser.ff) {
                        //    sender.get_element().click();
                        //}
                        eval(sender.target.parentElement.parentElement.href);
                    }
                    else {
                        if (e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                var confirm;

                if (msg == null)
                    confirm = radconfirm("Are you sure you want to delete this record?", callBackFn);
                else
                    confirm = radconfirm(msg, callBackFn);

                return false;
            }
        </script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvMOCSupportItemRequired.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>
        <style type="text/css">
            table.Hazard {
                border-collapse: collapse;
            }

                table.Hazard td, table.Hazard th {
                    border: 1px solid black;
                    padding: 5px;
                }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server"
            DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuMOCStatus" runat="server" OnTabStripCommand="MenuMOCStatus_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuMOCSubmit" runat="server" OnTabStripCommand="MenuMOCSubmit_TabStripCommand"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table id="Tablemoc" runat="server">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblOfficeShip" runat="server" Text="Office/Ship">
                        </telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <eluc:Vessel ID="ddlvessel" runat="server" CssClass="input" AppendDataBoundItems="true"
                            Width="270px" AssignedVessels="true" Enabled="false" />
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company">
                        </telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <eluc:Company ID="ucCompany" runat="server" Enabled="false" AppendDataBoundItems="true"
                            Readonly="false" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProposerName" runat="server" Text="Proposer (Name/Rank)">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPersonInCharge" runat="server">
                            <telerik:RadTextBox ID="txtCrewName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="150px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtCrewRank" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="120px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtCrewId" runat="server" CssClass="input" MaxLength="20"
                                Width="10px">
                            </telerik:RadTextBox>
                        </span><span id="spnPersonInChargeOffice" runat="server">
                            <telerik:RadTextBox ID="txtOfficePersonName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="150px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtOfficePersonDesignation" runat="server" CssClass="input"
                                Enabled="false" MaxLength="50" Width="120px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtPersonInChargeOfficeId" CssClass="input"
                                Width="0px" MaxLength="20">
                            </telerik:RadTextBox>
                        </span>
                        <telerik:RadLabel ID="lblDtkey1" runat="server" Visible="false">
                        </telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbltitle" runat="server" Text="Title">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtmoctitle" runat="server" CssClass="input" MaxLength="50"
                            Width="270px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblMOCDate" runat="server" Text="Date">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucMOCDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                            AutoPostBack="true" Enabled="false" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOwner" runat="server" Text="Name of Owner">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAddrOwner" MaxLength="20" Style="text-align: left;"
                            Width="270px" ReadOnly="true" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblclass" runat="server" Text="Name of the Class">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtClassName" MaxLength="20" Style="text-align: left;"
                            Width="270px" ReadOnly="true" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Name of Flag">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFlag" MaxLength="20" Style="text-align: left;"
                            Width="270px" ReadOnly="true" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterer">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCharterer" MaxLength="20" Style="text-align: left;"
                            Width="270px" ReadOnly="true" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type of Vessel">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVesselType" MaxLength="20" Style="text-align: left;"
                            Width="270px" ReadOnly="true" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblstatus" runat="server" Text="Status">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="ddlstatus" runat="server" Width="200px" CssClass="input">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblstatusid" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <b>
                            <telerik:RadLabel ID="lblrequestchangeid" runat="server" Text="Section A: Request for Change (By Proposer)">
                            </telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr style="height: -12px" />
                        <b>
                            <telerik:RadLabel ID="lbltypeofchange" runat="server" Text="Type of Change">
                            </telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" Width="270px" runat="server" Filter="Contains" CssClass="input_mandatory"
                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubCategory" runat="server" Text="Sub-Category">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubCategory" Width="270px" runat="server" Filter="Contains"
                            EmptyMessage="Type to Select" CssClass="input_mandatory">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNatureofChange" runat="server" Text="Nature of Change">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList RenderMode="Lightweight" ID="ddlNatureofChange" runat="server"
                            DropDownHeight="90px" OnSelectedIndexChanged="ddlNatureofChange_SelectedIndexChanged"
                            AutoPostBack="true">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="Dummy" />
                                <telerik:DropDownListItem Text="Permanent" Value="1"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="Temporary" Value="2"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTargetDateofimplementation" runat="server" Text="Target Date of implementation">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucTargetDateofimplementation" runat="server" CssClass="input_mandatory"
                            DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofrestoration" runat="server" Text="Date of restoration to original state">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateofrestoration" runat="server" CssClass="input_mandatory" DatePicker="true"
                            AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblnewregulationreq" runat="server" Text="Is this change due to a New Regulation?">
                        </telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rdbNewRegulation" runat="server"
                            OnSelectedIndexChanged="rdbNewRegulation_SelectedIndexChanged" Direction="Horizontal"
                            DropDownHeight="80px" AutoPostBack="true">
                            <Items>
                                <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="No" Value="2" Selected="true"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblnewRegulationTitle" runat="server" Text="New Regulation Title"
                            Visible="false">
                        </telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtNewRegulationTitle" runat="server" Enabled="false" Width="50%"
                            Visible="false" ToolTip="New Regulation Status"/>
                        <telerik:RadTextBox ID="txtNewRegStatus" runat="server"  Width="15%" ReadOnly="true"
                            Visible="false" />
                        <asp:LinkButton ID="lnkNewRegulation" runat="server" OnClick="lnkNewRegulation_OnClick"
                            Text="New Regulation" Visible="false"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkNRStatus" Visible="false"><span class="icon"><i class="fas fa-file-alt"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNRissueddate" runat="server" Text="New Regulation Issued Date"
                            Visible="false">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="dteNRIssue" runat="server" CssClass="input_mandatory" DatePicker="false"
                            Visible="false" AutoPostBack="true" Enabled="false" ReadOnly="true" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <b>
                            <telerik:RadLabel ID="lblDetailsoftheProposedChange" runat="server" Text="Details of the Proposed Change :">
                            </telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtDetailsoftheProposedChange" runat="server" Height="80px"
                            Resize="Both" Rows="6" CssClass="input_mandatory" TextMode="MultiLine" Width="95%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel ID="lblOptionsconsidered" runat="server" Text="Options considered for the change (What options were considered before selecting this particular option?)">
                            </telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtOptionsconsidered" runat="server" Height="80px" Rows="6"
                            Resize="Both" TextMode="MultiLine" CssClass="input_mandatory" Width="95%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel ID="lblJustificationforchange" runat="server" Text="Justification for change (Provide reason for proposed change safer operations, cost benefit, etc.)">
                            </telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtJustificationforchange" runat="server" Height="80px" Rows="6"
                            Resize="Both" CssClass="input_mandatory" TextMode="MultiLine" Width="95%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone7" runat="server" FitDocks="true" Orientation="Horizontal" Width="98%" BorderStyle="None">
                            <telerik:RadDock Width="98%" RenderMode="Lightweight" ID="RadDock7" runat="server" EnableAnimation="true" EnableDrag="false"
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
                                                    <asp:LinkButton runat="server" ID="imgShowRA" OnClick="imgShowRA_Click"><span class="icon"><i class="fas fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadTextBox ID="txtRAId" runat="server" RenderMode="Lightweight" CssClass="hidden"
                                                        MaxLength="20" Width="0px" Text="">
                                                    </telerik:RadTextBox>
                                                    <telerik:RadTextBox ID="txtRaType" runat="server" RenderMode="Lightweight" CssClass="hidden"
                                                        MaxLength="2" Width="0px" Text=''>
                                                    </telerik:RadTextBox>
                                                </span>&nbsp
                    <asp:ImageButton runat="server" AlternateText="Show RA Details" ImageUrl="<%$ PhoenixTheme:images/BarChart.png %>"
                        ID="ImgRA" ToolTip="Show PDF" OnClick="cmdRA_Click" Visible="false" />
                                                <asp:LinkButton ID="lnkCreateRA" runat="server" OnClick="lnkCreateRA_OnClick"></asp:LinkButton>
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
                                                        <td align="center" style="background-color: rgb(255,230,110);"width="20%">
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
                <tr>
                    <td colspan="4">
                        <br />
                        <b>
                            <telerik:RadLabel ID="lblrequestchangesid" runat="server" Text="Section B: Request for Change (By Proposer)">
                            </telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone1" runat="server" FitDocks="true" Orientation="Horizontal"
                            Width="98%" BorderStyle="None">
                            <telerik:RadDock Width="98%" RenderMode="Lightweight" ID="RadDock2" runat="server"
                                Title="<b>Support Required (By Office/ Superintendent, assistance by external parties such as workshop/technician etc)</b>"
                                EnableAnimation="true" EnableDrag="false" EnableRoundedCorners="true" Resizable="true"
                                CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuMOCSupportRequired" runat="server" />
                                    <telerik:RadGrid ID="gvMOCSupportRequired" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvMOCSupportRequired_RowCommand"
                                        OnItemDataBound="gvMOCSupportRequired_ItemDataBound" ShowHeader="true" EnableViewState="false"
                                        OnNeedDataSource="gvMOCSupportRequired_NeedDataSource" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                            AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDMOCSUPPORTREQUIREDID">
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <NoRecordsTemplate>
                                                <table id="Table2" runat="server" width="100%" border="0">
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
                                                <telerik:GridTemplateColumn HeaderText="From Whom?">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblquestionname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPORTREQUIREDQUESTION") %>'>
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCSupportRequiredid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSUPPORTREQUIREDID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblquestionid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPORTREQUIREDQUESTIONID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblotherdetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERDETAIL") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblhardcodeID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCHARDCODE") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Required" Visible="false">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblRequiredYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSUPPORTREQUIREDYN").ToString().Equals("1"))?"Yes":"No" %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="What type of support required?">
                                                    <HeaderStyle Width="50%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPORTREQUIREDDETAILS") %>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="Edit"
                                                            Width="20px" Height="20px"><span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE"
                                                            ToolTip="Delete" Width="20px" Height="20px"><span class="icon"><i class="fas fa-trash"></i></span>
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
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone2" runat="server" FitDocks="true" Orientation="Horizontal"
                            Width="98%" BorderStyle="None">
                            <telerik:RadDock Width="98%" RenderMode="Lightweight" ID="RadDock1" runat="server"
                                Title="<b>Identify any external approvals required (Regulatory Authorities / Classification Society/Owner )</b>"
                                EnableAnimation="true" EnableDrag="false" EnableRoundedCorners="true" Resizable="true"
                                CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuMOCExternalApproval" runat="server" />
                                    <telerik:RadGrid ID="gvMOCExternalApproval" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvMOCExternalApproval_RowCommand"
                                        OnItemDataBound="gvMOCExternalApproval_ItemDataBound" ShowHeader="true" EnableViewState="false"
                                        OnNeedDataSource="gvMOCExternalApproval_NeedDataSource" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                            AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDMOCEXTERNALAPPROVALREQUIREDID">
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <NoRecordsTemplate>
                                                <table id="Table2" runat="server" width="100%" border="0">
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
                                                <telerik:GridTemplateColumn HeaderText="From Whom?">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblquestionname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALAPPROVALQUESTION") %>'>
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCExternalApprovalid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCEXTERNALAPPROVALREQUIREDID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblquestionid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALAPPROVALQUESTIONID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblotherdetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERDETAIL") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Required" Visible="false">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblRequiredYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDEXTERNALAPPROVALYN").ToString().Equals("1"))?"Yes":"No" %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Details of Approval required?">
                                                    <HeaderStyle Width="50%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTERNALAPPROVALDETAILS") %>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="Edit"
                                                            Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE"
                                                            ToolTip="Delete" Width="20px" Height="20px">
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
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone3" runat="server" FitDocks="true" Orientation="Horizontal"
                            Width="98%" BorderStyle="None">
                            <telerik:RadDock Width="98%" RenderMode="Lightweight" ID="RadDock3" runat="server"
                                Title="<b>Identify any Equipment, Stores and Spares, Man Power, Document,  Manuals, Drawings/ Plans, Phoenix Modules, etc that are required to be changed or provided</b>"
                                EnableAnimation="true" EnableDrag="false" EnableRoundedCorners="true" Resizable="true"
                                ToolTip="Identify any Equipment, Stores and Spares, Man Power, Document,  Manuals, Drawings/ Plans, Phoenix Modules, etc that are required to be changed or provided"
                                CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuMOCSupportItemRequired" runat="server" />
                                    <telerik:RadGrid ID="gvMOCSupportItemRequired" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvMOCSupportItemRequired_RowCommand"
                                        OnItemDataBound="gvMOCSupportItemRequired_ItemDataBound" ShowHeader="true" EnableViewState="false"
                                        OnNeedDataSource="gvMOCSupportItemRequired_NeedDataSource" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                            AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDMOCSUPPORTITEMREQUIREDID">
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <NoRecordsTemplate>
                                                <table id="Table2" runat="server" width="100%" border="0">
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
                                                <telerik:GridTemplateColumn HeaderText="Item">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblquestionname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSUPPORTITEMQUESTION") %>'>
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCSupportItemRequiredid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSUPPORTITEMREQUIREDID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblquestionid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSUPPORTITEMQUESTIONID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblotherdetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERDETAIL") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Required" Visible="false">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblRequiredYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDMOCSUPPORTITEMREQUIREDYN").ToString().Equals("1"))?"Yes":"No" %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Details of Change required?">
                                                    <HeaderStyle Width="50%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSUPPORTITEMREQUIREDDETAILS") %>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Items">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblItemsHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMS") %>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="HMAPPING" ID="cmdEquipment"
                                                            Visible="false" ToolTip="Equipment Mapping">
                                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="HSEQADoc" CommandName="HSEQUA" ID="cmdHSEQUA"
                                                            Visible="false" ToolTip="HSEQA Documents">
                                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDITD"
                                                            Visible="false" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE"
                                                            ToolTip="Delete" Width="20px" Height="20px">
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
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone4" runat="server" FitDocks="true" Orientation="Horizontal"
                            Width="98%" BorderStyle="None">
                            <telerik:RadDock Width="98%" RenderMode="Lightweight" ID="RadDock4" runat="server"
                                Title="<b>Shipboard Personnel affected by change</b>" EnableAnimation="true"
                                EnableDrag="false" EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex"
                                Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuMOCShipboard" runat="server" />
                                    <telerik:RadGrid ID="gvMOCShipboard" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="3" OnItemCommand="gvMOCShipboard_RowCommand" OnItemDataBound="gvMOCShipboard_ItemDataBound"
                                        ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvMOCShipboard_NeedDataSource"
                                        ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                            AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDMOCSHIPBOARDPERSONNELAFFECTEDID">
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <NoRecordsTemplate>
                                                <table id="Table2" runat="server" width="100%" border="0">
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
                                                <telerik:GridTemplateColumn HeaderText="Dept">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblquestionname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSBPERSONNELAFFECTEDQUESTION") %>'>
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCShipboardid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSHIPBOARDPERSONNELAFFECTEDID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblquestionid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSBPERSONNELAFFECTEDQUESTIONID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblotherdetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERDETAIL") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Affected?" Visible="false">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblRequiredYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSBPERSONNELAFFECTEDYN").ToString().Equals("1"))?"Yes":"No" %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Details of how they will be notified of the change?">
                                                    <HeaderStyle Width="50%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSBPERSONNELAFFECTEDDETAILS") %>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="Edit"
                                                            Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE"
                                                            ToolTip="Delete" Width="20px" Height="20px">
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
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone5" runat="server" FitDocks="true" Orientation="Horizontal"
                            Width="98%" BorderStyle="None">
                            <telerik:RadDock Width="98%" RenderMode="Lightweight" ID="RadDock5" runat="server"
                                Title="<b>Shore Based Personnel affected by change</b>" EnableAnimation="true"
                                EnableDrag="false" EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex"
                                Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuMOCShoreBased" runat="server" />
                                    <telerik:RadGrid ID="gvMOCShoreBased" runat="server" AutoGenerateColumns="False"
                                        RenderMode="Lightweight" Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvMOCShoreBased_RowCommand"
                                        OnItemDataBound="gvMOCShoreBased_ItemDataBound" ShowHeader="true" EnableViewState="false"
                                        OnNeedDataSource="gvMOCShoreBased_NeedDataSource" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                            AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDMOCSHOREBASEDPERSONNELAFFECTEDID">
                                            <NoRecordsTemplate>
                                                <table id="Table2" runat="server" width="100%" border="0">
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
                                                <telerik:GridTemplateColumn HeaderText="Dept / Entity">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblquestionname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSBPERSONNELAFFECTEDQUESTION") %>'>
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCShoreBasedid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCSHOREBASEDPERSONNELAFFECTEDID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblquestionid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSBPERSONNELAFFECTEDQUESTIONID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblotherdetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERDETAIL") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Affected?" Visible="false">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblRequiredYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSBPERSONNELAFFECTEDYN").ToString().Equals("1"))?"Yes":"No" %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Details of how they will be notified of the change?">
                                                    <HeaderStyle Width="50%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSBPERSONNELAFFECTEDDETAILS") %>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="Edit"
                                                            Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE"
                                                            ToolTip="Delete" Width="20px" Height="20px">
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
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone6" runat="server" FitDocks="true" Orientation="Horizontal"
                            Width="98%" BorderStyle="None">
                            <telerik:RadDock Width="98%" RenderMode="Lightweight" ID="RadDock6" runat="server"
                                Title="<b>Training Required (Identify the training needs &amp; persons to be trained, with PIC &amp; Target date to complete training. Specific ranks or departments to be mentioned, if applicable)</b>"
                                EnableAnimation="true" EnableDrag="false" EnableRoundedCorners="true" Resizable="true"
                                CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuTrainingRequired" runat="server" />
                                    <telerik:RadGrid ID="gvMOCTrainingRequired" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvMOCTrainingRequired_RowCommand"
                                        OnItemDataBound="gvMOCTrainingRequired_ItemDataBound" ShowHeader="true" EnableViewState="false"
                                        OnNeedDataSource="gvMOCTrainingRequired_NeedDataSource" ShowFooter="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                                            ShowFooter="false" AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDMOCTRAININGREQUIREDID">
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <NoRecordsTemplate>
                                                <table id="Table1" runat="server" width="100%" border="0">
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
                                                <telerik:GridTemplateColumn HeaderText="Ship/Shore">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblShipOrShoreid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNAME") %>'>
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCTrainingRequiredid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCTRAININGREQUIREDID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblMOCid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Training Required">
                                                    <HeaderStyle Width="15%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblTrainingRequired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGREQUIRED") %>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Dept./Persons to be trained">
                                                    <HeaderStyle Width="15%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDepartmentOrPersonToBeObtainted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTORPERSONTOBETRAINED") %>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                                
                                                <telerik:GridTemplateColumn HeaderText="Department">
                                                    <HeaderStyle Width="15%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDepart" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Person In Charge">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'>
                                                        </telerik:RadLabel>
                                                        -
                                                    <telerik:RadLabel ID="lblPICRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICRANK") %>'>
                                                    </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Target date">
                                                    <HeaderStyle Width="15%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblTargetdate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT"
                                                            Width="20px" Height="20px">
                                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE"
                                                            ToolTip="Delete" Width="20px" Height="20px">
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
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>
        <telerik:RadFormDecorator ID="RadFormDecorator2" DecorationZoneID="RadAjaxPanel2"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindow ID="modalPopup" runat="server" Width="500px" Height="365px" Modal="true"
            OnClientClose="CloseWindow" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true">
            <ContentTemplate>
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1"
                    OnAjaxRequest="RadAjaxPanel2_AjaxRequest">
                    <table border="0" style="width: 100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblsupport" runat="server" Text="Item">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSectionName" runat="server" ReadOnly="true" Enabled="false"
                                    Width="90%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trequipment" visible="false">
                            <td>
                                <telerik:RadLabel ID="lblengcontrol" runat="server" Text="Equipment">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickListComponent">
                                    <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input" Width="80px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input" Width="250px">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowComponents" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="0px">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="lnkComponentAdd" runat="server" OnClick="lnkComponentAdd_Click"
                                        ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                    <div id="divComponents" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 330px; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                                        <table id="tblcomponents" runat="server">
                                        </table>
                                    </div>
                                </span>
                            </td>
                        </tr>
                        <tr runat="server" id="trDocuments" visible="false">
                            <td>
                                <telerik:RadLabel ID="lblDocuments" runat="server" Text="Procedures, Forms and Checklists">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnShowDocuments">
                                    <telerik:RadTextBox ID="txtDocumentname" runat="server" CssClass="input" Width="280px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtDocumentId" runat="server" CssClass="hidden" Width="0px">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowDocuments" runat="server" ToolTip="Select Documents">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkDocumentAdd" runat="server" OnClick="lnkDocumentAdd_Click"
                                        ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                    <div id="divHSEQUA" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 330px; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                                        <table id="tblDocuments" runat="server">
                                        </table>
                                    </div>
                                </span>
                            </td>
                        </tr>
                        <tr runat="server" id="trDescription" visible="false">
                            <td>
                                <telerik:RadLabel ID="lblDescriptionSupport" runat="server" Text="Description">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDescriptionSupport" runat="server" Width="100%" Rows="4"
                                    Height="50px" Resize="Both" TextMode="MultiLine">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <telerik:RadButton ID="btnClose" Text="Save" runat="server" OnClick="btnClose_Click">
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                     <asp:Button ID="ucConfirmRevision" runat="server" Text="confirmRevision" OnClick="ucConfirmRevision_Click" />
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
