<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewContractRevision.aspx.cs" Inherits="CrewContractRevision" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeniorityScale" Src="~/UserControls/UserControlSeniorityScale.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlContractCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CrewComponents" Src="~/UserControls/UserControlContractCrew.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Contract Revision</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmInActive" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmInActive" runat="server">
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <eluc:TabStrip ID="MenuCrewContract" runat="server" OnTabStripCommand="CrewContract_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuCrewContractSub" runat="server" OnTabStripCommand="CrewContract_TabStripCommand" Title="Revise Contract"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%" CssClass="scrolpan">
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
                    <td><span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="400px" ShowEvent="onmouseover" CssClass="fon"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="BottomCenter" EnableRoundedCorners="true" ContentScrolling="Auto" VisibleOnPageLoad="true"
                            Text="*Note:<br/>please use the Revise contract feature only in case of changes in the wage structure,to change the pay commencement date,<br/>please use the unlock feature to correct the main contract.">
                        </telerik:RadToolTip>
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
                            <eluc:Number ID="txtPlusMinusPeriod" runat="server" CssClass="input_mandatory" IsInteger="true" />
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
                            ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblUSDpermonth" runat="server" Text=""></telerik:RadLabel>
                        ,per month
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
                            AddressType="134" Width="240px" AutoPostBack="true" OnTextChangedEvent="ddlUnion_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCBARevision" runat="server" Text="CBA Revision"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox CssClass="input_mandatory" DropDownPosition="Static" ID="ddlRevision" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Revision" Width="240px" DataTextField="FLDNAME" DataValueField="FLDREVISIONID" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
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
                </tr>

            </table>
            <table cellpadding="1" cellspacing="1" width="99%">
                <tr>
                    <td style="vertical-align: top; width: 45%;">
                        <telerik:RadDockZone ID="RadDockZone2" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock1" runat="server" Title="CBA Components" EnableAnimation="true"
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
                        <telerik:RadDockZone ID="RadDockZone1" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock2" runat="server" Title="Standard Components"
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
                        <telerik:RadDockZone ID="RadDockZone3" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock3" runat="server" Title="Components Agreed with Crew" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
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
                                                        <eluc:CrewComponents ID="ddlCrewComponentsAdd" runat="server" AppendDataBoundItems="true" Width="99%" CssClass="dropdown_mandatory" />
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
                                                        <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>' MaxLength="8" />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory" MaxLength="8" Text="" />
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

            <%--    <table cellpadding="1" cellspacing="1" width="100%">
            
                <tr>
                    <td valign="top" colspan="2">
                        <asp:GridView ID="gvContract" runat="server" AutoGenerateColumns="False" Width="100%"
                            OnRowDataBound="gvContract_RowDataBound" CellPadding="3" ShowFooter="false" ShowHeader="false"
                            EnableViewState="false">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:GridView ID="gvESM" runat="server" AutoGenerateColumns="False" Width="100%"
                            OnRowDataBound="gvContract_RowDataBound" CellPadding="3" ShowFooter="false" ShowHeader="false"
                            EnableViewState="false">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="False" Width="100%"
                            OnRowDataBound="gvContract_RowDataBound" CellPadding="3" ShowFooter="true" ShowHeader="false" OnRowCommand="gvCrew_RowCommand"
                            OnRowEditing="gvCrew_RowEditing" OnRowCancelingEdit="gvCrew_RowCancelingEdit" OnRowUpdating="gvCrew_RowUpdating"
                            EnableViewState="false">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblContractCompId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadLabel>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                                        <telerik:RadLabel ID="lblShortCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblContractCompIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblCrewComponentsEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:CrewComponents ID="ddlCrewComponentsAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblCompIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTID")%>'></telerik:RadLabel>
                                        <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>' MaxLength="8" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory" MaxLength="8" Text="" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblCurrencyEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Currency ID="ddlCurrencyAdd" runat="server" CssClass="dropdown_mandatory" SelectedCurrency="10" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblPayableEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPAYABLE") %>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadLabel ID="lblPayableAdd" runat="server" Text=""></telerik:RadLabel>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            Action
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr class="datagrid_footerstyle">
                    <td>
                        <telerik:RadLabel ID="lblSubTotalCBAMonthlySubTotal" runat="server" Text="Monthly Sub Total"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubTotalCBA" runat="server" CssClass="input txtNumber readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblSubTotalCBAUSD" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubToatalESMMonthlySubTotal" runat="server" Text="Monthly Sub Total"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubToatalESM" runat="server" CssClass="input txtNumber readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblSubToatalESMUSD" runat="server" Text=""></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubTotalCrewMonthlySubTotal" runat="server" Text="Monthly Sub Total"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubTotalCrew" runat="server" CssClass="input txtNumber readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblSubTotalCrewUSD" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTotalMonthlyWages" runat="server" Text="Total Monthly Wages"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtMonthlyAmount" runat="server" CssClass="input txtNumber readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadLabel ID="lblMonthlyUSD" runat="server" Text=""></telerik:RadLabel>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTotalNonmonthlyWagesSuccessfulCompletionofContract" runat="server" Text="Total Non-monthly Wages (Successful Completion of Contract)"></telerik:RadLabel>

                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtNonMonthlyAmount" runat="server" CssClass="input txtNumber readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadLabel ID="lblNonMonthlyUSD" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
            </table>--%>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:Confirm ID="ucConfirm" runat="server" YesButtonVisible="false" CancelText="Ok" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
