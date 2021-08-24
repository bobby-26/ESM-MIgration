<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionS3Report.aspx.cs" Inherits="InspectionS3Report" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>S3 Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAuditSummary" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" Visible="false" runat="server" Text="" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Title Text="S3 - Safety Performance Report Data" ID="ucTitle" runat="server" Visible="false"
                    ShowMenu="true" />
                <table id="tblWI" width="60%" runat="server">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlFrommonth" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadComboBox ID="ddlFromYear" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlTomonth" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadComboBox ID="ddlToYear" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
                <br />
                <eluc:TabStrip ID="MenuS3Report" runat="server" OnTabStripCommand="MenuS3Report_TabStripCommand"></eluc:TabStrip>
                <div>
                    <br />
                    <b>
                        <telerik:RadLabel ID="lblIncidentComparisionTitle" runat="server" Text="A) Incident Comparison"></telerik:RadLabel>
                    </b>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvIncidentComparision" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            </ColumnGroups>
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
                            <Columns>
                                <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                                <telerik:GridTemplateColumn HeaderText="Incident Comparison">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIncidentComparision" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIODNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Personal Injury">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPersonalInjury" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERSONALINJURY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Environment Release">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEnvRelease" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENVRELEASE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Near Miss">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNearMiss" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEARMISS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Property Damage">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPropertyDamage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPERTYDAMAGE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Process Loss">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblProcessLoss" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSLOSS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Security">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSecurity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECURITY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Total">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <br />
                    <b>
                        <telerik:RadLabel ID="lblExposureHoursTitle" runat="server" Text="Exposure Hours"></telerik:RadLabel>
                    </b>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvExposureHours" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            </ColumnGroups>
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
                            <Columns>
                                <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                                <telerik:GridTemplateColumn HeaderText="Period">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIOD") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Exposure Hours">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblExposureHour" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPOSUREHOUR") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <br />
                    <b>
                        <telerik:RadLabel ID="lblIncidentstatisticsTitle" runat="server" Text="B) Incident Statistics"></telerik:RadLabel>
                    </b>
                    <br />
                    <b>
                        <telerik:RadLabel ID="lblVslTypestatisticsTitle" runat="server" Text="1) Vessel Type Wise Incident Statistics:"></telerik:RadLabel>
                    </b>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvVslTypeStatistics" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            </ColumnGroups>
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
                            <Columns>
                                <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                                <telerik:GridTemplateColumn HeaderText="Vessel Type Wise Incident Statistics">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Personal Injury">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPersonalInjury" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERSONALINJURY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Environment Release">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEnvRelease" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENVRELEASE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Near Miss">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNearMiss" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEARMISS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Property Damage">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPropertyDamage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPERTYDAMAGE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Process Loss">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblProcessLoss" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSLOSS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Security">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSecurity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECURITY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Total">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <br />
                    <b>
                        <telerik:RadLabel ID="lblCategorystatisticsTitle" runat="server" Text="2) Category Wise Incident Statistics"></telerik:RadLabel>
                    </b>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCategoryStatistics" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            </ColumnGroups>
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
                            <Columns>
                                <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCategoryHeader" runat="server" Text="Category Wise Incident Statistics"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblPersonalInjuryHeader" runat="server" Text="Personal Injury"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPersonalInjury" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERSONALINJURY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblEnvReleaseHeader" runat="server" Text="Environment Release"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEnvRelease" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENVRELEASE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblNearMissHeader" runat="server" Text="Near Miss"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNearMiss" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEARMISS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblPropertyDamageHeader" runat="server" Text="Property Damage"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPropertyDamage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPERTYDAMAGE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblProcessLossHeader" runat="server" Text="Process Loss"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblProcessLoss" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSLOSS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblSecurityHeader" runat="server" Text="Security"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSecurity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECURITY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTotalHeader" runat="server" Text="Total"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <br />
                    <b>
                        <telerik:RadLabel ID="lblPersonalInjuryTitle" runat="server" Text="3) Personal Injuries"></telerik:RadLabel>
                    </b>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPersonalInjury" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false" OnItemDataBound="gvPersonalInjury_ItemDataBound"
                        ShowHeader="true" EnableViewState="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            </ColumnGroups>
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
                            <Columns>
                                <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblPersonalInjuryHeader" runat="server" Text="Injury Category"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPersonalInjury" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblPrev4QHeader" runat="server" Text="4Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPrev4Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD4QPREVYEAR") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl1QHeader" runat="server" Text="1Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl1Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD1Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl2QHeader" runat="server" Text="2Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl2Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD2Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl3QHeader" runat="server" Text="3Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl3Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD3Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl4QHeader" runat="server" Text="4Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl4Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD4Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <br />
                    <b>
                        <telerik:RadLabel ID="lblInjuryTypeComparisionTitle" runat="server" Text="4) Injury Type Comparison"></telerik:RadLabel>
                    </b>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvInjuryType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false" OnItemDataBound="gvInjuryType_ItemDataBound"
                        ShowHeader="true" EnableViewState="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            </ColumnGroups>
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
                            <Columns>
                                <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblInjuryTypeHeader" runat="server" Text="Injury Type"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblInjuryType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINJURYTYPE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblPrev4QHeader" runat="server" Text="4Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPrev4Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD4QPREVYEAR") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl1QHeader" runat="server" Text="1Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl1Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD1Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl2QHeader" runat="server" Text="2Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl2Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD2Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl3QHeader" runat="server" Text="3Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl3Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD3Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl4QHeader" runat="server" Text="4Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl4Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD4Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <br />
                    <b>
                        <telerik:RadLabel ID="lblQ5CategoryWiseTitle" runat="server" Text="5) Q5-Categorywise"></telerik:RadLabel>
                    </b>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvQ5" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false" OnItemDataBound="gvQ5_ItemDataBound"
                        ShowHeader="true" EnableViewState="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            </ColumnGroups>
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
                            <Columns>
                                <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblQ5CategoryHeader" runat="server" Text="Q5 Category"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblQ5Category" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblPrev4QHeader" runat="server" Text="4Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPrev4Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD4QPREVYEAR") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl1QHeader" runat="server" Text="1Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl1Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD1Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl2QHeader" runat="server" Text="2Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl2Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD2Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl3QHeader" runat="server" Text="3Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl3Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD3Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbl4QHeader" runat="server" Text="4Q"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbl4Q" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD4Q") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <br />
                    <b>
                        <telerik:RadLabel ID="lblPartOfBodyHeader" runat="server" Text="6) Part of the Body"></telerik:RadLabel>
                    </b>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPartOfBody" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false">
                            <ColumnGroups>
                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            </ColumnGroups>
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
                            <Columns>
                                <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblPartofBodyHeader" runat="server" Text="Part of the body"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPartofBody" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTOFBODY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCountHeader" runat="server" Text="Count"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
