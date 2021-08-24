<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewContract.aspx.cs" Inherits="CrewContract" ValidateRequest="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeniorityScale" Src="~/UserControls/UserControlSeniorityScale.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlContractCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CrewComponents" Src="~/UserControls/UserControlContractCrew.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Contract</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            } function contractDelete(args) {
                if (args) {
                    __doPostBack("<%=btndelete.UniqueID %>", "");
                }
            } function Unlock(args) {
                if (args) {
                    __doPostBack("<%=btnunlock.UniqueID %>", "");
                }
            }
        </script>
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
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmInActive" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmInActive" runat="server">
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuCrewContract" runat="server" OnTabStripCommand="CrewContract_TabStripCommand" TabStrip="true"></eluc:TabStrip>
           <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%" CssClass="scrolpan">
            <eluc:TabStrip ID="MenuCrewContractSub" runat="server" OnTabStripCommand="CrewContract_TabStripCommand" Title="Contract"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 17%;">
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 31%;">
                        <telerik:RadTextBox ID="txtFirstName" runat="server" CssClass="gridinput readonlytextbox"
                            ReadOnly="true" Enabled="false" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 21%;">
                        <telerik:RadLabel ID="lblRankNationality" runat="server" Text="Rank / Nationality"></telerik:RadLabel>
                    </td>
                    <td style="width: 31%;">
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="input readonlytextbox" ReadOnly="true" Enabled="false" Width="120px"></telerik:RadTextBox>
                        /    
                        <telerik:RadTextBox ID="txtNationality" runat="server" CssClass="input readonlytextbox" ReadOnly="true" Enabled="false" Width="120px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtAddress" runat="server" CssClass="gridinput readonlytextbox"
                            ReadOnly="true" Enabled="false" Width="95%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblSeamanBookNo" runat="server" Text="Seaman Book No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanBook" runat="server" CssClass="input readonlytextbox" Width="240px" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSelecttoViewRevisions" runat="server" Text="View Revision"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlRevision" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Revision" AutoPostBack="true" OnSelectedIndexChanged="ddlRevision_TextChanged" Filter="Contains" MarkFirstMatch="true" DataValueField="FLDCONTRACTID" DataTextField="FLDPAYDATE">
                        </telerik:RadComboBox>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto"
                            Text="Notes :<br/>1. This screen not to be used for generating a revised contract,Instead use the Revise contract screen and then edit the contract from this screen.<br/> 2.Press &quot;save&quot; to save the contract prior viewing the contract<br/>3.Please ensure correct Pool of the seafarer for generating correct format of contract.">
                        </telerik:RadToolTip>
                        <%--<asp:ImageButton ID="imgBtnContractExtension" runat="server" ToolTip="Contract Extension"
                            ImageUrl="<%$ PhoenixTheme:images/edit-info.png%>" />--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblContractingParty" runat="server" Text="Contracting Party"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ddlCompany" CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContractPeriod" runat="server" Text="Contract Period"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtContractPeriod" runat="server" CssClass="input_mandatory" IsInteger="true" />
                        +/-
                            <eluc:Number ID="txtPlusMinusPeriod" runat="server" CssClass="input_mandatory" MaxLength="3" />
                        (Months)
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPortofEngagement" runat="server" Text="Port of Engagement"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ddlSeaPort" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContractPayCommencementDate" runat="server" Text="Contract/ Pay Commencement Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWagesof" runat="server" Text="Wages of"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWage" runat="server" CssClass="input txtNumber readonlytextbox"
                            ReadOnly="true" Enabled="false" Width="90px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtWageCurrency" runat="server" CssClass="input txtNumber readonlytextbox"
                            ReadOnly="true" Enabled="false" Width="90px">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblUSDpermonth" runat="server" Text="per month"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselJoining" runat="server" Text="Vessel Joining"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="input readonlytextbox" ReadOnly="true" Enabled="false" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUnion" runat="server" Text="Applicable CBA / Agreement"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address ID="ddlUnion" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            AddressType="134" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPBCalculationDate" runat="server" Text="PB Calculation Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPBDate" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeniorityWageScale" runat="server" Text="Seniority Wage Scale"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SeniorityScale ID="ddlSeniority" runat="server" AppendDataBoundItems="true" CssClass="input"
                            AutoPostBack="true" OnTextChangedEvent="ddlSeniory_Changed" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankExpCaption" runat="server" Text="Months in Rank "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlRankMonth" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Month in rank" AutoPostBack="true" OnDataBound="ddlRankMonth_DataBound" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncrementDate" runat="server" Text="Next Increment Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtNextIncrementDate" runat="server" />

                        <asp:Button ID="btnGenExpCon" runat="server" Text="Gen Appointment Letter" OnClick="btnGenExpCon_OnClick" />
                    </td>
                </tr>

            </table>
            <table cellpadding="1" cellspacing="1" width="100%" runat="server" id="tblofferletter">
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="Total offer Letter Amount">
                        </telerik:RadLabel>
                    </td>

                    <td>
                        <telerik:RadTextBox ID="txtTotalOfferletter" runat="server" CssClass="input txtNumber readonlytextbox"
                            ReadOnly="true" Enabled="false" Font-Bold="true" Width="90px">
                        </telerik:RadTextBox></td>

                    <td>
                        <telerik:RadLabel ID="lblofferletter" runat="server" Text="Offer Letter Amount ">
                        </telerik:RadLabel>
                    </td>

                    <td>
                        <b>+</b>
                        <telerik:RadTextBox ID="txtofferLetter" Font-Bold="true" runat="server" CssClass="input txtNumber readonlytextbox"
                            ReadOnly="true" Enabled="false" Width="90px">
                        </telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="CBA Components">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <b>-</b>
                        <telerik:RadTextBox ID="txtCBAAmount" runat="server" CssClass="input txtNumber readonlytextbox"
                            ReadOnly="true" Enabled="false" Font-Bold="true" Width="90px">
                        </telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Standard Components Amount">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <b>-</b>
                        <telerik:RadTextBox ID="txtstancomponent" runat="server" CssClass="input txtNumber readonlytextbox"
                            ReadOnly="true" Enabled="false" Font-Bold="true" Width="90px">
                        </telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Other Onboard Monthly wages">
                        </telerik:RadLabel>
                    </td>

                    <td>
                        <b>=</b>
                        <telerik:RadTextBox ID="txtothermonthlyamount" runat="server" CssClass="input txtNumber readonlytextbox"
                            ReadOnly="true" Font-Bold="true" Enabled="false" Width="90px">
                        </telerik:RadTextBox></td>

                </tr>
            </table>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="vertical-align: top; width: 45%;">
                        <telerik:RadDockZone ID="RadDockZone2" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock1" runat="server" Title="CBA Components" EnableDrag="false" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvContract" Width="100%" runat="server" OnItemDataBound="gvCrew_ItemDataBound" OnNeedDataSource="gvContract_NeedDataSource" ShowHeader="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                            AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                            <Columns>
                                                <telerik:GridTemplateColumn>
                                                    <ItemStyle Width="52%" Wrap="false" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblamount" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <ItemStyle Wrap="False" Width="8%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblcurrcode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <ItemStyle Wrap="False" Width="25%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblpay" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE")%>'>
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
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                    <table cellpadding="1" cellspacing="1" width="100%">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblSubTotalCBAMonthlySubTotal" runat="server" Font-Bold="true" Text="Monthly Sub Total"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtSubTotalCBA" runat="server" Font-Bold="true" CssClass="input txtNumber readonlytextbox"
                                                    ReadOnly="true" Enabled="false">
                                                </telerik:RadTextBox>
                                                <telerik:RadLabel ID="lblSubTotalCBAUSD" runat="server" Font-Bold="true" Text=""></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblTotalMonthlyWages" runat="server" Font-Bold="true" Text="Total Monthly Wages"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtMonthlyAmount" runat="server" Font-Bold="true" CssClass="input txtNumber readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                                                <telerik:RadLabel ID="lblbasicCurrencyCode" runat="server" Font-Bold="true" Text=""></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblTotalNonmonthlyWages" Font-Bold="true" runat="server" Text="Total Non-monthly Wages<br/> (Successful Completion of Contract)"></telerik:RadLabel>

                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtNonMonthlyAmount" runat="server" Font-Bold="true" CssClass="input txtNumber readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                                                <telerik:RadLabel ID="lblBasicCurrency" runat="server" Font-Bold="true" Text=""></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                    <td style="vertical-align: top; width: 55%;">
                        <telerik:RadDockZone ID="RadDockZone1" runat="server" FitDocks="true" Orientation="Horizontal" Width="98%" BorderStyle="None">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock2" runat="server" Title="Standard Components" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvESM" runat="server" OnNeedDataSource="gvContract_NeedDataSource" ShowHeader="false">
                                        <MasterTableView AutoGenerateColumns="False" AllowSorting="false">
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="">
                                                    <HeaderStyle Width="40%" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'>
                                                        </telerik:RadLabel>
                                                        <%--<eluc:ToolTip ID="ucToolTipDescription" runat="server" Text='<%#HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME").ToString()) %>' />--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="">
                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="">
                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="">
                                                    <HeaderStyle Width="40%" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE")%>
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
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="true" />
                                        </ClientSettings>
                                        <SelectedItemStyle BackColor="#00ccff" />
                                    </telerik:RadGrid>
                                    <table cellpadding="1" cellspacing="1" width="100%">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblSubToatalESMMonthlySubTotal" Font-Bold="true" runat="server" Text="Monthly Sub Total"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtSubToatalESM" Font-Bold="true" runat="server" CssClass="input txtNumber readonlytextbox"
                                                    ReadOnly="true" Enabled="false">
                                                </telerik:RadTextBox>
                                                <telerik:RadLabel ID="lblSubToatalESMUSD" Font-Bold="true" runat="server" Text=""></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                        <telerik:RadDockZone ID="RadDockZone3" runat="server" FitDocks="true" Orientation="Horizontal" Width="98%" BorderStyle="None">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock3" runat="server" Title="Components Agreed with Crew" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false" EnableDrag="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvCrew" Width="99%" EnableViewState="true" ShowFooter="true" OnItemCommand="gvCrew_ItemCommand" OnItemDataBound="gvCrew_ItemDataBound" runat="server" OnNeedDataSource="gvContract_NeedDataSource" ShowHeader="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                            AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                            <Columns>
                                                <telerik:GridTemplateColumn>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="35%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Left" Width="35%" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblContractCompIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'>
                                                        </telerik:RadLabel>
                                                        <%--<eluc:ToolTip ID="ucToolTipDescription" runat="server" Text='<%#HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString()) %>' />--%>
                                                        <telerik:RadLabel ID="lblShortCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <eluc:CrewComponents ID="ddlCrewComponentsAdd" runat="server" CrewComponentsList='<%#Component()%>' AppendDataBoundItems="true" Width="99%" CssClass="dropdown_mandatory" />
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Right" Width="12%" />
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadLabel ID="lblCompIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadLabel>
                                                        <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>' MaxLength="7" />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory" MaxLength="7" Text="" />
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>

                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Left" Width="12%" />
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Currency ID="ddlCurrencyEdit" runat="server" CssClass="dropdown_mandatory" Width="60px"
                                                            CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' AppendDataBoundItems="true"
                                                            SelectedCurrency='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYID") %>' />

                                                        <telerik:RadLabel ID="lblCurrencyEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <eluc:Currency ID="ddlCurrencyAdd" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" AutoPostBack="false"
                                                            CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' SelectedCurrency="10" Width="60px" />
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="">
                                                    <HeaderStyle Width="18%" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadLabel ID="lblPayableEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE") %>'></telerik:RadLabel>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <telerik:RadLabel ID="lblPayableAdd" runat="server" Text=""></telerik:RadLabel>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                                        <span class="icon"><i class="fas fa-save"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <FooterTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                                                      <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                        </asp:LinkButton>
                                                    </FooterTemplate>
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
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />

                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>

                                    </telerik:RadGrid>
                                    <table cellpadding="1" cellspacing="1" width="100%">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblSubTotalCrewMonthlySubTotal" Font-Bold="true" runat="server" Text="Monthly Sub Total"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtSubTotalCrew" Font-Bold="true" runat="server" CssClass="input txtNumber readonlytextbox"
                                                    ReadOnly="true" Enabled="false">
                                                </telerik:RadTextBox>
                                                <telerik:RadLabel ID="lblSubTotalCrewUSD" Font-Bold="true" runat="server" Text=""></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
            </table>

            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="btndelete" runat="server" Text="contractDelete" OnClick="btndelete_Click" />
            <asp:Button ID="btnconfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" />
            <asp:Button ID="btnunlock" runat="server" Text="confirm" OnClick="btnUnLockConfirm_Click" />

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
