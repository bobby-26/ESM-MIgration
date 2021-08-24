<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionInspectorDeficiencySummary.aspx.cs" Inherits="InspectionInspectorDeficiencySummary" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register Src="../UserControls/UserControlQuick.ascx" TagName="Quick" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deficiency Summary</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="divInspectionIncidentCriticalFactor" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <%--                        <div id="divHeading">
                            <telerik:RadLabel ID="lblDeficiencySummary" runat="server" Text="Deficiency Summary" Visible="false"></telerik:RadLabel>
                        </div>--%>
            <telerik:RadLabel ID="lblDeficiencies" runat="server" Text="Deficiencies" Font-Bold="true"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDeficiency" runat="server" AutoGenerateColumns="False" OnItemCommand="gvDeficiency_ItemCommand"
                Font-Size="11px" Width="100%" CellPadding="3">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDEFICIENCYID" TableLayout="Fixed">
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
                    <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Ref.Number">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeficiencyid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDEFICIENCYID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>' CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTYPE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="200px"></ItemStyle>
                            <FooterStyle Wrap="True" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDEFICIENCYCATEGORY")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDISSUEDDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>

            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <div id="divFind" style="position: relative; z-index: 2">
                        <b>
                            <telerik:RadLabel ID="lblDeficiencyDetails" runat="server" Text="Deficiency Details"></telerik:RadLabel>

                        </b>
                        <table id="tblInspectionNC" width="100%">
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadRadioButtonList ID="rblDeficiencyType" runat="server" Direction="Horizontal">
                                        <Items>
                                            <telerik:ButtonListItem Text="NC" Value="2"></telerik:ButtonListItem>
                                            <telerik:ButtonListItem Text="Major NC" Value="1"></telerik:ButtonListItem>
                                            <telerik:ButtonListItem Text="Observation" Value="3"></telerik:ButtonListItem>
                                            <telerik:ButtonListItem Text="Hi Risk Observation" Value="4"></telerik:ButtonListItem>
                                        </Items>
                                    </telerik:RadRadioButtonList>
                                </td>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblIssuedDate" runat="server" Text="Issued&nbsp; Date"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Date ID="ucDate" runat="server" DatePicker="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" AssignedVessels="true"
                                        VesselsOnly="true" Width="250px" />
                                </td>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" Width="250px"
                                        HardTypeCode="146" ShortNameFilter="OPN,CLD,CMP,CAD" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblSource" runat="server" Text="Source"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadComboBox ID="ddlSchedule" runat="server" Width="250px"
                                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    </telerik:RadComboBox>
                                </td>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblCheckListReferenceNo" runat="server" Text="CheckList Reference Number"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadTextBox ID="txtChecklistRef" runat="server" CssClass="input" Width="250px"></telerik:RadTextBox>

                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblDeficiencyCategory" runat="server" Text="Deficiency Category"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Quick ID="ucNonConformanceCategory" runat="server" AppendDataBoundItems="true" Width="250px"
                                        QuickTypeCode="47" Visible="true" />
                                    <%--<eluc:Quick ID="ucRiskCategory" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    QuickTypeCode="71" Visible="false" Width="250px"/>--%>
                                </td>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblInspectorComments" runat="server" Text="Inspector Comments"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadTextBox ID="txtInspectorComments" runat="server" CssClass="input" Height="60px" Resize="Both"
                                        TextMode="MultiLine" Width="250px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadTextBox ID="txtDesc" runat="server" CssClass="input" Height="60px" Resize="Both"
                                        TextMode="MultiLine" Width="250px">
                                    </telerik:RadTextBox>
                                </td>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text="Office Remarks"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadTextBox ID="txtOfficeRemarks" runat="server" CssClass="input" Height="60px" Resize="Both"
                                        TextMode="MultiLine" Width="250px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblMastersComments" runat="server" Text="Master's Comments"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadTextBox ID="txtMasterComments" runat="server" CssClass="input" Height="60px" Resize="Both"
                                        TextMode="MultiLine" Width="250px">
                                    </telerik:RadTextBox>
                                </td>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblCompletionDate" runat="server" Text="Completion Date"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Date ID="ucCompletionDate" runat="server" DatePicker="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblRCAnotRequired" runat="server" Text="RCA not required"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadCheckBox ID="chkRCANotrequired" runat="server" />
                                </td>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblRCAisCompleted" runat="server" Text="RCA is Completed"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadCheckBox ID="chkRCAcompleted" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblRCATargetDate" runat="server" Text="RCA Target Date"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Date ID="ucRcaTargetDate" runat="server" DatePicker="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblCloseOutRemarks" runat="server" Text="Close Out Remarks"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadTextBox ID="txtCloseOutRemarks" runat="server" CssClass="input" Height="60px" Resize="Both"
                                        TextMode="MultiLine" Width="250px">
                                    </telerik:RadTextBox>
                                </td>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblCloseOutDate" runat="server" Text="Close Out Date"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Date ID="ucCloseoutDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblCloseOutBy" runat="server" Text="Close Out By"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadTextBox ID="txtCloseOutByName" runat="server" Width="145px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtCloseOutByDesignation" runat="server" Width="100px" CssClass="input"></telerik:RadTextBox>
                                </td>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblCancelReason" runat="server" Text="Cancel Reason"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtCancelReason" runat="server" CssClass="input" Resize="Both"
                                        Height="60px" Rows="4" TextMode="MultiLine" Width="250px">
                                    </telerik:RadTextBox>
                                </td>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblCancelDate" runat="server" Text="Cancel Date"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Date ID="ucCancelDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <telerik:RadLabel ID="lblCancelledBy" runat="server" Text="Cancelled By"></telerik:RadLabel>
                                </td>
                                <td style="width: 35%">
                                    <telerik:RadTextBox ID="txtCancelledByName" runat="server" Width="145px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtCancelledByDesignation" runat="server" Width="100px" CssClass="input"></telerik:RadTextBox>
                                </td>
                                <td colspan="2"></td>
                            </tr>
                        </table>
                    </div>
        </div>
    </form>
</body>
</html>
