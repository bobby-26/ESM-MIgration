<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreWageTracker.aspx.cs"
    Inherits="CrewOffshoreWageTracker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Wage Tracker</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
            }
        }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWageTracker" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="confirm" runat="server" OnClick="btnConfirm_Click" />

            <eluc:TabStrip ID="CrewMenuGeneral" runat="server" OnTabStripCommand="CrewMenuGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>


            <eluc:TabStrip ID="CrewTrainingMenu" runat="server" OnTabStripCommand="CrewTrainingMenu_TabStripCommand"></eluc:TabStrip>

            <div>
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblReportfortheMonthof" runat="server" Text="Report for the Month of :"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                Filter="Contains" EmptyMessage="Type to select the month" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="January" Value="1" />
                                    <telerik:RadComboBoxItem Text="February" Value="2" />
                                    <telerik:RadComboBoxItem Text="March" Value="3" />
                                    <telerik:RadComboBoxItem Text="April" Value="4" />
                                    <telerik:RadComboBoxItem Text="May" Value="5" />
                                    <telerik:RadComboBoxItem Text="June" Value="6" />
                                    <telerik:RadComboBoxItem Text="July" Value="7" />
                                    <telerik:RadComboBoxItem Text="August" Value="8" />
                                    <telerik:RadComboBoxItem Text="September" Value="9" />
                                    <telerik:RadComboBoxItem Text="October" Value="10" />
                                    <telerik:RadComboBoxItem Text="November" Value="11" />
                                    <telerik:RadComboBoxItem Text="December" Value="12" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblYear" runat="server" Text="Year :"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                Filter="Contains" EmptyMessage="Type to select the month" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                        <%-- <td>
                                <telerik:RadLabel ID="lblFromdate" runat="server" Text="Opening Date "></telerik:RadLabel>
                            </td>--%>
                        <td>
                            <eluc:Date ID="txtfromdate" Visible="false" runat="server" CssClass="input_mandatory" />
                        </td>

                        <td>
                            <telerik:RadLabel ID="lblDate" runat="server" Text="Closing Date "></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtAsOnDate" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnTextChangedEvent="txtAsOnDate_TextChanged" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblWageTrackerHistory" runat="server" Text="Wage Tracker History"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlHistory" runat="server" DataTextField="FLDPBCLOSGINDATE"
                                DataValueField="FLDWAGETRACKERID" AutoPostBack="true" OnSelectedIndexChanged="ddlHistory_SelectedIndexChanged"
                                Filter="Contains" EmptyMessage="Type to select the month" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                            <telerik:RadLabel ID="lblcurrency" runat="server" Visible="false"></telerik:RadLabel>
                        </td>
                        <%--<telerik:RadLabel ID="lblReportingCurrency" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALWAGESINRPTCURRENCY") %>'></telerik:RadLabel>--%>
                        <%--<td>
                            <telerik:RadLabel ID="lbllastlockeddate" runat="server" Text="Last Locked Date :"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtlastlockeddate" runat="server" ReadOnly="true"/>
                        </td>--%>
                    </tr>
                </table>
            </div>

            <eluc:TabStrip ID="MenuOffshoreWageTracker" runat="server" OnTabStripCommand="MenuOffshoreWageTracker_TabStripCommand"></eluc:TabStrip>

            <div>
                <table id="tblTable" runat="server" width="100%">
                    <tr>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblMonthlyBudget" runat="server" Text="Monthly Budget"></telerik:RadLabel>
                            </b>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMonthlyBudget" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>

                            <%--<asp:ImageButton ID="btndetail" ToolTip="detail" runat="server" OnClick="ImgPOPickList_Click" />--%>
                            <%--<eluc:CommonToolTip ID="ucCommonToolTip"  runat="server" Screen="" />           --%>                 
                        </td>
                        <td>
                            <eluc:TabStrip ID="btndetail" runat="server" OnTabStripCommand="ImgPOPickList_Click"
                                TabStrip="true"></eluc:TabStrip>
                        </td>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblActual" runat="server" Text="Actual"></telerik:RadLabel>
                            </b>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtActual" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblBudget" runat="server" Text="Budget"></telerik:RadLabel>
                            </b>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtBudget" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblVariance" runat="server" Text="Variance"></telerik:RadLabel>
                            </b>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVariance" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0">
                <%-- <asp:GridView ID="gvWageTracker" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCreated="gvWageTracker_RowCreated" OnRowCommand="gvWageTracker_RowCommand"
                    ShowHeader="true" ShowFooter="true" EnableViewState="false" OnRowDataBound="gvWageTracker_RowDataBound"
                    OnRowEditing="gvWageTracker_RowEditing" OnRowCancelingEdit="gvWageTracker_RowCancelingEdit"
                    OnRowUpdating="gvWageTracker_RowUpdating" DataKeyNames="FLDSIGNONOFFID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvWageTracker" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvWageTracker_NeedDataSource"
                    OnItemCommand="gvWageTracker_ItemCommand"
                    OnItemDataBound="gvWageTracker_ItemDataBound"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true" CommandItemDisplay="Top">
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
                        <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Daily Rate" Name="daily" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Contract" Name="Contract" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Date Signed" Name="sign" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Onboard" Name="Onboard" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Travel" Name="Travel" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="No">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSignonoffID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWageTrackerID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGETRACKERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblwagetrackeremployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGETRACKEREMPLOYEEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblcrewplanid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblNo" runat="server" Text='<%# Container.DataSetIndex + 1 %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="E.mail ID">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmail" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRank" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Currency">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrencyitem" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblcurrencyid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Budgeted" ColumnGroupName="daily">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle Width="70px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetedWage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETEDWAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Actual" ColumnGroupName="daily">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle Width="70px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActualWages" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALWAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblActualWagesEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALWAGE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="txtSignonoffID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="txtWageTrackerID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGETRACKERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="txtwagetrackeremployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGETRACKEREMPLOYEEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWageTrackerExtnIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGETRACKEREXTNID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblcrewplanidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></telerik:RadLabel>
                                    <eluc:Number ID="txtActualWages" runat="server" IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALWAGE") %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="DP Allowance" ColumnGroupName="daily">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle Width="70px" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDailyDPAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAILYDPALLOWANCE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Start" ColumnGroupName="Contract">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblContractDate" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCONTRACTCOMMENCEMENTDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="End" ColumnGroupName="Contract">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblContractCancelDate" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCONTRACTCANCELDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="On" ColumnGroupName="sign">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSignOnDate" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Off" ColumnGroupName="sign">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSignOffDate" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Days for Travel">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <%--<FooterStyle Wrap="false" HorizontalAlign="Right" />--%>
                                <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTravelDays" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYSFORTRAVEL") %>'></telerik:RadLabel>
                                </ItemTemplate>

                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Wages for Travel">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTravelWages" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGESFORTRAVEL") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:RadLabel ID="lblTravelWagesTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELWAGESTOTAL") %>'></telerik:RadLabel>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Days" ColumnGroupName="Onboard">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOnboardDays" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYSONBOARD") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:RadLabel ID="lblOnboardDaysTotal" runat="server"></telerik:RadLabel>
                                        <%=OnboardDaysTotal%></b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Wages" ColumnGroupName="Onboard">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOnboardWages" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGESONBOARD") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:RadLabel ID="lblOnboardWagesTotal" runat="server"></telerik:RadLabel>
                                        <%=OnboardWagesTotal%></b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Estimated" ColumnGroupName="Travel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTravelEndDate" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDESTIMATEDTRAVELENDDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucTravelEndDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDESTIMATEDTRAVELENDDATE")) %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Days for sign off" ColumnGroupName="Travel">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <HeaderStyle Width="70px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSignoffTravelDays" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDAYSSIGNOFF") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:RadLabel ID="lblSignoffTravelDaysTotal" runat="server"></telerik:RadLabel>
                                        <%=SignoffTravelDaysTotal%></b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Allowance" ColumnGroupName="Travel">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <HeaderStyle Width="70px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTravelAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELALLOWANCE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:RadLabel ID="lblTravelAllowanceTotal" runat="server"></telerik:RadLabel>
                                        <%=TravelAllowanceTotal%></b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Total DP Allowance">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDPAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPALLOWANCE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtDPAllowance" runat="server" IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPALLOWANCE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:RadLabel ID="lblDPAllowanceTotal" runat="server"></telerik:RadLabel>
                                        <%=DPAllowanceTotal%></b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Earnings">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReimbursements" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREIMBURSEMENTS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:RadLabel ID="lblReimbursementsTotal" runat="server"></telerik:RadLabel>
                                        <%=ReimbursementsTotal%></b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Deductions">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDeductions" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEDUCTIONS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:RadLabel ID="lblDeductionsTotal" runat="server"></telerik:RadLabel>
                                        <%=DeductionsTotal%></b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Total Wages">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTotalWages" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALWAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:RadLabel ID="lblWagesTotal" runat="server"></telerik:RadLabel>
                                        <%=WagesTotal%></b>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Remarks">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderStyle Width="100px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                        ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
            <%--  <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes"
                CancelText="No" />--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
