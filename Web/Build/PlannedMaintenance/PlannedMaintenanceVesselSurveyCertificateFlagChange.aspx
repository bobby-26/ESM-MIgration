<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateFlagChange.aspx.cs"
    Inherits="PlannedMaintenanceVesselSurveyCertificateFlagChange" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCLQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Flag Audit/Survey Detail</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="CertificatesRenewal" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuCertificatesRenewal" runat="server" OnTabStripCommand="MenuCertificatesRenewal_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td width="15%"></td>
                    <td width="85%">
                        <asp:RadioButtonList ID="rdlAuditSurvey" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rdlAuditSurvey_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="Survey" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Audit"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAudit" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAuditSurvey" runat="server" CssClass="input_mandatory">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDueDate" runat="server" Text="Due Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDueDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlannedDate" runat="server" Text="Planned Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPlanDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSurveyPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCPort ID="ddlSurveyPort" runat="server" AppendDataBoundItems="true"
                            SeaportList='<%# PhoenixRegistersSeaport.ListSeaport() %>' Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSurveyorName" runat="server" Text="Name of Auditor / Surveyor"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSurveyorName" runat="server" CssClass="input" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSurveyDoneDate" runat="server" Text="Date of Completion"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDoneDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemaks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRemarks" runat="server" Width="270px" Height="30px"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvSurvey" runat="server" RenderMode="Lightweight" AutoGenerateColumns="False" OnItemDataBound="gvSurvey_ItemDataBound" 
                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvSurvey_NeedDataSource"
                EnableHeaderContextMenu="true">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#CCE5FF" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Certificate">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>
                                <telerik:RadLabel ID="lblCertificateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadTextBox ID="txtNumber" runat="server" Width="100px" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'></telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <eluc:Date ID="txtIssueDate" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <eluc:Date ID="txtExpiryDate" runat="server" Width="100px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Certificate Status">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <eluc:UCLQuick ID="ucRemarks" runat="server" AppendDataBoundItems="true"
                                    QuickList='<%#PhoenixRegistersQuick.ListQuick(1, 144) %>' QuickTypeCode="144" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    Width="100px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="SURVEYCERTIFICATE" ID="cmdSvyAtt" ToolTip="Certificate">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
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
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadGrid ID="gvSurveyCOC" runat="server" RenderMode="Lightweight"  AutoGenerateColumns="False" Font-Size="11px"  OnNeedDataSource="gvSurveyCOC_NeedDataSource"
                Width="100%" CellPadding="3" OnItemDataBound="gvSurveyCOC_ItemDataBound" OnItemCommand="gvSurveyCOC_ItemCommand"
                ShowHeader="true" EnableViewState="false" AllowPaging="true" AllowCustomPaging="true" EnableHeaderContextMenu="true">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#CCE5FF" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Fixed" GroupHeaderItemStyle-Font-Bold="true">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Survey Type" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSURVEYTYPENAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Certificates">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>
                                <telerik:RadLabel ID="lblCertificateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due Date" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COC" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEM") %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucCOCToolTip" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDITEMTOOLTIP").ToString() %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucDescriptionToolTip" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDESCRIPTIONTOOLTIP").ToString() %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSDESC") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed Date" HeaderStyle-Width="13%">
                            <ItemStyle HorizontalAlign="Left" Width="13%"></ItemStyle>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE")) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtDoneDate" runat="server" CssClass="input_mandatory" Width="100px" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed Remarks" HeaderStyle-Width="13%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="13%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDREMARKS") %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucCompletedRemarksToolTip" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDREMARKSTOOLTIP").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCOCId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECOCID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtCompletionRemarks" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave"
                                    ToolTip="Save"><span class="icon"><i class="fas fa-save"></i></span></asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel"><span class="icon"><i class="fas fa-times-circle"></i></span></asp:LinkButton>
                            </EditItemTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>

</body>
</html>
