<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationAdd.aspx.cs" Inherits="Inspection_InspectionRegulationAdd" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Regulation Add</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <style>
        .even {
            background-color: #dde8f6 !important;
        }

        .odd {
            background-color: #f4f8fa !important;
        }

        .vesselType-container {
            height: 300px;
            overflow: auto;
            border: 1px;
            border-style: groove;
            width: 98%;
        }

        .table-container {
            margin-top: 20px;
            width: 300px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="110%">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="NewRegulation" runat="server" OnTabStripCommand="NewRegulation_TabStripCommand"></eluc:TabStrip>

            <table style="width: 100%;">

                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtTitle" runat="server" Enabled="true" TextMode="MultiLine" Rows="2" Resize="Both" Width="98%"></telerik:RadTextBox>
                    </td>

                    <td style="width: 10%">
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblRegulationId" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblIssuedDate" runat="server" Text="Issued Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 40%">
                        <eluc:Date runat="server" ID="txtIssuedDate" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblIssuedBy" runat="server" Text="Issued By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtIssuedBy" runat="server" TextMode="MultiLine" Rows="1" Enabled="true" Resize="Both" Width="98%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDueDate" runat="server" Text="Due Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtregulationDueDate" />
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblActionRequired" runat="server" Text="Action Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" TextMode="MultiLine" ID="txtActionRequired" runat="server" Width="98%" Rows="5" Enabled="true" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescription" runat="server" TextMode="Multiline" Enabled="true" Width="98%" Rows="5" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" TextMode="MultiLine" ID="txtRemarks" runat="server" Width="98%" Rows="5" Enabled="true" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <div id="divRule" runat="server">
                <eluc:TabStrip ID="ruleTabstrip" runat="server" OnTabStripCommand="ruleTabstrip_TabStripCommand"></eluc:TabStrip>
                <table width="100%" runat="server" id="tblRule" visible="false">
                    <tr class="even">
                        <td class="even">
                            <b>
                                <telerik:RadLabel runat="server" ID="lblRuleName" Text="Rule Name"></telerik:RadLabel>
                            </b>
                        </td>
                        <td class="even">
                            <b>
                                <telerik:RadLabel runat="server" ID="txtRuleName" Text="Rule 1"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr class="odd">
                        <td class="odd">
                            <telerik:RadLabel runat="server" ID="lblApply" Text="Apply"></telerik:RadLabel>
                        </td>
                        <td class="odd">
                            <telerik:RadCheckBox runat="server" ID="chkApply" AutoPostBack="false"></telerik:RadCheckBox>
                        </td>
                    </tr>
                    <tr class="even">
                        <td class="even">
                            <telerik:RadLabel runat="server" ID="lblRuleOrder" Text="Rule Order"></telerik:RadLabel>
                        </td>
                        <td class="even">
                            <eluc:Number ID="txtRuleOrder" runat="server" Width="122px" />
                        </td>
                    </tr>
                    <tr class="odd">
                        <td class="odd">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                        </td>
                        <td class="odd">
                            <telerik:RadComboBox runat="server" ID="ddlVesselType" Filter="Contains" EmptyMessage="Select Vessel Type"></telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr class="even">
                        <td class="even">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblDateBuilt" runat="server" Text="Date Built"></telerik:RadLabel>
                        </td>
                        <td class="even">
                            <eluc:Date runat="server" ID="txtDateBuilt" />
                        </td>
                    </tr>
                    <tr>
                        <td class="odd">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblBeforeAfter" runat="server" Text="Before | After"></telerik:RadLabel>
                        </td>
                        <td class="odd">
                            <telerik:RadRadioButtonList ID="chkBeforeAfter" runat="server" Direction="Horizontal" AutoPostBack="false">
                                <Items>
                                    <telerik:ButtonListItem Text="Before" Value="<" />
                                    <telerik:ButtonListItem Text="After" Value=">" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr class="odd">
                        <td class="odd">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblAttribute" runat="server" Text="Attribute"></telerik:RadLabel>
                        </td>
                        <td class="odd">
                            <telerik:RadComboBox ID="ddlFieldNameAdd" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameAdd_SelectedIndexChanged" ExpandDirection="Up" runat="server" RenderMode="Lightweight" Filter="Contains" EmptyMessage="Select Vessel Type"></telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="even">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblValue" runat="server" Text="Value"></telerik:RadLabel>
                        </td>
                        <td class="even">
                            <eluc:Number runat="server" ID="txtValue" DecimalPlace="0" Width="150px" />
                            <eluc:Date runat="server" ID="ucValue" DatePicker="true" />
                            <telerik:RadComboBox runat="server" ID="ddlValue" RenderMode="Lightweight" Filter="Contains" EmptyMessage="Select Flag"></telerik:RadComboBox>
                            <%--<telerik:RadTextBox runat="server" ID="txtValue" Width="122px"></telerik:RadTextBox>--%>
                        </td>
                    </tr>
                    <tr class="odd">
                        <td class="odd">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lbllesserGrater" runat="server" Text="Lesser | Greater"></telerik:RadLabel>
                        </td>
                        <td class="odd">
                            <telerik:RadRadioButtonList ID="chkGreaterLesser" runat="server" Direction="Horizontal" AutoPostBack="false">
                                <Items>
                                    <telerik:ButtonListItem Text="Lesser" Value="<" />
                                    <telerik:ButtonListItem Text="Greater" Value=">" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr class="even">
                        <td class="even">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblDueDates" runat="server" Text="Due Date"></telerik:RadLabel>
                        </td>
                        <td class="even">
                            <eluc:Date runat="server" ID="txtDuedate" />
                        </td>
                    </tr>
                    <tr class="odd">
                        <td class="odd">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblSurvey" runat="server" Text="Survey"></telerik:RadLabel>
                        </td>
                        <td class="odd">

                            <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlCertificate" Width="200" Height="200"
                                DataTextField="FLDCERTIFICATENAME"
                                Filter="Contains" MarkFirstMatch="true"
                                EmptyMessage="select Certificate" DropDownAutoWidth="Enabled"
                                ShowMoreResultsBox="false" HighlightTemplatedItems="true"
                                DataValueField="FLDCERTIFICATEID">
                            </telerik:RadComboBox>

                            <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddSurveyType" Width="200" Height="200"
                                DataTextField="FLDSURVEYTYPENAME"
                                Filter="Contains" MarkFirstMatch="true"
                                EmptyMessage="select Survey" DropDownAutoWidth="Enabled"
                                ShowMoreResultsBox="false" HighlightTemplatedItems="true"
                                DataValueField="FLDSURVEYTYPEID">
                            </telerik:RadComboBox>


                        </td>

                    </tr>
                    <tr class="even">
                        <td class="even">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblEarlierLater" runat="server" Text="Earlier | Later"></telerik:RadLabel>
                        </td>
                        <td class="even">
                            <telerik:RadRadioButtonList ID="chkEarlierLater" runat="server" Direction="Horizontal" AutoPostBack="false">
                                <Items>
                                    <telerik:ButtonListItem Text="Earlier" Value="<" />
                                    <telerik:ButtonListItem Text="Later" Value=">" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                </table>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvRule" runat="server" AutoGenerateColumns="False" OnNeedDataSource="gvRule_NeedDataSource"
                    OnItemCommand="gvRule_ItemCommand" OnItemDataBound="gvRule_ItemDataBound" ShowHeader="false" ShowFooter="false" PageSize="100">
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false">
                        <HeaderStyle Width="102px" />
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Rules" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel runat="server" ID="lblRuleID" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRULEID") %>' Visible="false"></telerik:RadLabel>

                                    <telerik:RadLabel runat="server" ID="lblRegulationId" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGULATIONID") %>' Visible="false"></telerik:RadLabel>
                                    <table width="100%">
                                        <tr class="even">
                                            <td class="even">
                                                <b>
                                                    <telerik:RadLabel runat="server" ID="lblRuleName" Text="Rule Name"></telerik:RadLabel>
                                                </b>
                                            </td>
                                            <td class="even" colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <b>
                                                                <telerik:RadLabel runat="server" ID="txtRuleName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRULENAME") %>'></telerik:RadLabel>
                                                            </b>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                                            <span class="icon"><i class="fas fa-save"></i></span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                                            </asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="odd">
                                            <td class="odd">
                                                <telerik:RadLabel runat="server" ID="lblApply" Text="Apply"></telerik:RadLabel>
                                            </td>
                                            <td class="odd">
                                                <telerik:RadCheckBox runat="server" ID="chkApply"></telerik:RadCheckBox>
                                            </td>
                                        </tr>
                                        <tr class="even">
                                            <td class="even">
                                                <telerik:RadLabel runat="server" ID="lblRuleOrder" Text="Rule Order"></telerik:RadLabel>
                                            </td>
                                            <td class="even">
                                                <eluc:Number ID="txtRuleOrder" runat="server" Width="122px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRULEORDER") %>' />
                                            </td>
                                        </tr>
                                        <tr class="odd">
                                            <td class="odd">
                                                <telerik:RadLabel RenderMode="Lightweight" ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                                            </td>
                                            <td class="odd">
                                                <telerik:RadComboBox runat="server" ID="ddlVesselType"></telerik:RadComboBox>

                                            </td>
                                        </tr>
                                        <tr class="even">
                                            <td class="even">
                                                <telerik:RadLabel RenderMode="Lightweight" ID="lblDateBuilt" runat="server" Text="Date Built"></telerik:RadLabel>
                                            </td>
                                            <td class="even">
                                                <eluc:Date runat="server" ID="txtDateBuilt" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEBUILT")) %>' />
                                            </td>
                                        </tr>

                                        <tr class="odd">
                                            <td class="odd">
                                                <telerik:RadLabel RenderMode="Lightweight" ID="lblBeforeAfter" runat="server" Text="Before | After"></telerik:RadLabel>
                                            </td>
                                            <td class="odd">
                                                <telerik:RadRadioButtonList ID="chkBeforeAfter" runat="server" Direction="Horizontal" AutoPostBack="false">
                                                    <Items>
                                                        <telerik:ButtonListItem Text="Before" Value="<" />
                                                        <telerik:ButtonListItem Text="After" Value=">" />
                                                    </Items>
                                                </telerik:RadRadioButtonList>
                                            </td>
                                        </tr>
                                        <tr class="even">
                                            <td class="even">
                                                <telerik:RadLabel RenderMode="Lightweight" ID="lblAttribute" runat="server" Text="Attribute"></telerik:RadLabel>
                                            </td>
                                            <td class="even">
                                                <telerik:RadComboBox ID="ddlFieldNameAdd" AutoPostBack="true" OnSelectedIndexChanged="ddlFieldNameAdd_SelectedIndexChangedGrid" ExpandDirection="Up" runat="server" RenderMode="Lightweight"></telerik:RadComboBox>
                                            </td>

                                        </tr>
                                        <tr class="odd">
                                            <td class="odd">
                                                <telerik:RadLabel RenderMode="Lightweight" ID="lblValue" runat="server" Text="Value"></telerik:RadLabel>
                                            </td>
                                            <td class="odd">
                                                <eluc:Number runat="server" ID="txtValue" DecimalPlace="0" Width="150px" />
                                                <eluc:Date runat="server" ID="ucValue" DatePicker="true" />
                                                <telerik:RadComboBox runat="server" ID="ddlValue" RenderMode="Lightweight" Filter="Contains" EmptyMessage="Select Flag"></telerik:RadComboBox>
                                                <telerik:RadLabel runat="server" ID="valuetype" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRULEORDER") %>'></telerik:RadLabel>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr class="even">
                                            <td class="even">
                                                <telerik:RadLabel RenderMode="Lightweight" ID="RadLabel1" runat="server" Text="Lesser | Greater"></telerik:RadLabel>
                                            </td>
                                            <td class="even">
                                                <telerik:RadRadioButtonList ID="chkGreaterLesser" runat="server" Direction="Horizontal" AutoPostBack="false">
                                                    <Items>
                                                        <telerik:ButtonListItem Text="Lesser" Value="<" />
                                                        <telerik:ButtonListItem Text="Greater" Value=">" />
                                                    </Items>
                                                </telerik:RadRadioButtonList>
                                            </td>
                                        </tr>
                                        <tr class="odd">
                                            <td class="odd">
                                                <telerik:RadLabel RenderMode="Lightweight" ID="lblDueDates" runat="server" Text="Due Date"></telerik:RadLabel>
                                            </td>
                                            <td class="odd">
                                                <eluc:Date runat="server" ID="txtDuedate" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDUEDATE")) %>' />
                                            </td>
                                        </tr>
                                        <tr class="even">
                                            <td class="even">
                                                <telerik:RadLabel RenderMode="Lightweight" ID="lblSurvey" runat="server" Text="Survey"></telerik:RadLabel>
                                            </td>
                                            <td class="even">

                                                <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlCertificate" Width="200" Height="200"
                                                    DataTextField="FLDCERTIFICATENAME"
                                                    Filter="Contains" MarkFirstMatch="true"
                                                    EmptyMessage="select Certificate" DropDownAutoWidth="Enabled"
                                                    ShowMoreResultsBox="false" HighlightTemplatedItems="true"
                                                    DataValueField="FLDCERTIFICATEID">
                                                </telerik:RadComboBox>

                                                <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddSurveyType" Width="200" Height="200"
                                                    DataTextField="FLDSURVEYTYPENAME"
                                                    Filter="Contains" MarkFirstMatch="true"
                                                    EmptyMessage="select Survey" DropDownAutoWidth="Enabled"
                                                    ShowMoreResultsBox="false" HighlightTemplatedItems="true"
                                                    DataValueField="FLDSURVEYTYPEID">
                                                </telerik:RadComboBox>

                                            </td>
                                        </tr>
                                        <tr class="odd">
                                            <td class="odd">
                                                <telerik:RadLabel RenderMode="Lightweight" ID="lblEarlierLater" runat="server" Text="Earlier | Later"></telerik:RadLabel>
                                            </td>
                                            <td class="odd">
                                                <telerik:RadRadioButtonList ID="chkEarlierLater" runat="server" Direction="Horizontal" AutoPostBack="false">
                                                    <Items>
                                                        <telerik:ButtonListItem Text="Earlier" Value="<" />
                                                        <telerik:ButtonListItem Text="Later" Value=">" />
                                                    </Items>
                                                </telerik:RadRadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                    <hr />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
