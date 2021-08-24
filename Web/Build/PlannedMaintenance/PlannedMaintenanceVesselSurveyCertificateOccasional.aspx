<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateOccasional.aspx.cs"
    Inherits="PlannedMaintenanceVesselSurveyCertificateOccasional" %>

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
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Occasional Audit/Survey Detail</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="CertificatesRenewal" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="radSkinManager1" runat="server"></telerik:RadSkinManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvSurvey">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvSurvey" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvSurveyCOC">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvSurveyCOC"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuCertificatesRenewal" runat="server" OnTabStripCommand="MenuCertificatesRenewal_TabStripCommand"></eluc:TabStrip>
            </div>
            <table cellpadding="1px" width="100%">
                <tr>
                    <td></td>
                    <td>
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
                        <telerik:RadLabel ID="lblPlannedDate" runat="server" Text="Planned Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPlanDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSurveyPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCPort ID="ddlSurveyPort" runat="server" AppendDataBoundItems="true" 
                            SeaportList='<%# PhoenixRegistersSeaport.ListSeaport() %>' Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSurveyorName" runat="server" Text="Name of Auditor / Surveyor"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSurveyorName" runat="server" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemaks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRemarks" runat="server" Width="250px" Height="40px"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvSurvey" RenderMode="Lightweight" runat="server" AutoGenerateColumns="False" EnableHeaderContextMenu="true"
                Width="100%" CellPadding="3" OnItemDataBound="gvSurvey_ItemDataBound" OnNeedDataSource="gvSurvey_NeedDataSource">
                <SortingSettings SortedBackColor="#CCE5FF" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Certificate">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>
                                <telerik:RadLabel ID="lblCertificateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number">
                            <ItemTemplate>
                                <telerik:RadTextBox ID="txtNumber" runat="server" Width="100px" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'></telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date">
                            <ItemTemplate>
                                <eluc:Date ID="txtIssueDate" runat="server" CssClass="input" Width="100px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date">
                            <ItemTemplate>
                                <eluc:Date ID="txtExpiryDate" runat="server" CssClass="input" Width="100px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Certificate Status">
                            <ItemTemplate>
                                <eluc:UCLQuick ID="ucRemarks" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickList='<%#PhoenixRegistersQuick.ListQuick(1, 144) %>' QuickTypeCode="144" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    Width="100px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Certificate" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="SURVEYCERTIFICATE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdSvyAtt" ToolTip="Certificate"></asp:ImageButton>
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
            <telerik:RadGrid ID="gvSurveyCOC"  RenderMode="Lightweight" runat="server" AutoGenerateColumns="False" Width="100%" CellPadding="3" EnableHeaderContextMenu="true"
                OnItemDataBound="gvSurveyCOC_ItemDataBound" OnItemCommand="gvSurveyCOC_ItemCommand1" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvSurveyCOC_NeedDataSource">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Survey Type">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSURVEYTYPENAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Certificates">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>
                                <telerik:RadLabel ID="lblCertificateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="COC">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEM") %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucCOCToolTip" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDITEMTOOLTIP").ToString() %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucDescriptionToolTip" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDESCRIPTIONTOOLTIP").ToString() %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSDESC") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed Date">
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE")) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtDoneDate" runat="server" CssClass="input_mandatory"  />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed Remarks">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDREMARKS") %>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucCompletedRemarksToolTip" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDREMARKSTOOLTIP").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCOCId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECOCID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtCompletionRemarks" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="40px"></ItemStyle>
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
        <%--</telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
