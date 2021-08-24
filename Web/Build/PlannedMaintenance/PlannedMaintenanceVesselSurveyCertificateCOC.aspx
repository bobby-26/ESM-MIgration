<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateCOC.aspx.cs"
    Inherits="PlannedMaintenanceVesselSurveyCertificateCOC" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCLQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Certificate COC</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="CertificatesMaping" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="radSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager ID="radWindowManager1" runat="server" EnableShadow="true"></telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuCertificatesCOC" runat="server" OnTabStripCommand="MenuCertificatesCOC_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCertificate" runat="server" Text="Certificate"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCertificate" runat="server" Width="250px" CssClass="readonly" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInitialDate" runat="server" Text="Anniversary /Initial Audit Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtInitialDate" runat="server" CssClass="readonly" Width="130" Enabled="false" />
                    </td>
                    <td></td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCategory" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="250px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNumber" runat="server" Width="130px" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIssueAuthority" runat="server" Text="Issued By"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddrType ID="ucIssuingAuthority" runat="server" AddressType="134,137,334,1600" CssClass="readonlytextbox" Enabled="false" Width="250px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIssueDate" runat="server" Text="Issue Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIssueDate" runat="server" CssClass="readonly" Width="130px"
                            Enabled="false" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCPort ID="ddlPort" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox" Enabled="false"
                            SeaportList='<%# PhoenixRegistersSeaport.ListSeaport() %>' Width="250px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExpiryDate" runat="server" Text="Expiry Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucExpiryDate" runat="server" CssClass="readonly" Width="130px"
                            Enabled="false" />
                        &nbsp&nbsp
                    <asp:CheckBox ID="chkNoExpiry" runat="server" Text="No Expiry"
                        Enabled="false" TextAlign="Left" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNotApplicable" runat="server" Text="Not applicable for this vessel"
                            Width="120px">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkNotApplicable" runat="server" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarksType" runat="server" Text="Certificate Status"
                            Width="120px">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCLQuick ID="ucRemarks" runat="server" CssClass="readonlytextbox" Enabled="false" AppendDataBoundItems="true"
                            QuickTypeCode="144" Width="130px" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReason" runat="server" Text="Reason why not applicable"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReason" runat="server" CssClass="readonlytextbox" Width="250px" Height="30px" Enabled="false"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAttachYN" runat="server" Text="Is uploaded Attachment correct"
                            Width="120px">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkAttachYN" runat="server" Enabled="false" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemaks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="250px" Height="30px"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                        <telerik:RadLabel ID="lblLastSurvey" runat="server"><b>Last Audit/Survey</b></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastSurveyType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastSurveyAudit" runat="server" Width="250px" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbllastSurveyDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucLastSurveyDate" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                        <telerik:RadLabel ID="lblNextSurvey" runat="server"><b>Next Audit/Survey</b></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSurveyType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSurveyType" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="250px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlanedDate" runat="server" Text="Planned Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPlanDate" runat="server" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSurveyorName" runat="server" Text="Name of Auditor / Surveyor"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSurveyorName" runat="server" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSurveyDoneDate" runat="server" Text="Completion Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDoneDate" runat="server" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSurveyPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UCPort ID="ddlSurveyPort" runat="server" AppendDataBoundItems="true"
                            SeaportList='<%# PhoenixRegistersSeaport.ListSeaport() %>' Width="250px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlanRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPlanRemarks" runat="server" Width="250px" Height="40px"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
            </table>
            <%--          <tr>
                        <td colspan="5">--%>

            <telerik:RadGrid ID="gvCertificatesCOC" runat="server" AutoGenerateColumns="False" ShowFooter="true" AllowPaging="true"
                Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="true" AllowCustomPaging="true"
                OnItemCommand="gvCertificatesCOC_ItemCommand" OnItemDataBound="gvCertificatesCOC_ItemDataBound"
                OnNeedDataSource="gvCertificatesCOC_NeedDataSource" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#CCE5FF" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="COC">
                            <ItemStyle></ItemStyle>
                            <FooterStyle />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblItem" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDITEM")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCocID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECOCID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtItemEdit" CssClass="input_mandatory" runat="server" Width="100%"
                                    TextMode="MultiLine" Rows="4" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMTOOLTIP") %>'>
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblCocID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCERTIFICATECOCID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtItemAdd" CssClass="input_mandatory" Width="100%" TextMode="MultiLine"
                                    Rows="4" runat="server">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtkey" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDATTACHMENTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDescEdit" CssClass="input_mandatory" runat="server" Width="100%"
                                    TextMode="MultiLine" Rows="4" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTIONTOOLTIP") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescAdd" CssClass="input_mandatory" Width="100%" TextMode="MultiLine"
                                    Rows="4" runat="server">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Due Date">
                            <ItemStyle HorizontalAlign="Left" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDueDateEdit" runat="server" DatePicker="true" CssClass="input_mandatory" Width="100%"
                                    Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucDueDateAdd" runat="server" DatePicker="true" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Status">
                            <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusName" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSDESC") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusName" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSDESC") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Completed Date">
                            <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCompletedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDDATE")) %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="130px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:LinkButton runat="server" CommandName="SURVEYCOC" ID="cmdSvyAtt" ToolTip="Attachment">
                                <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Complete" CmmandName="COMPLETE"
                                    ID="cmdComplete" ToolTip="Complete"><span class="icon"><i class="fas fa-check-double"></i></span></asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                    ToolTip="Delete"><span class="icon"><i class="fas fa-trash-alt"></i></span></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                    ToolTip="Save"><span class="icon"><i class="fas fa-save"></i></i></span></asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                    ToolTip="Cancel"><span class="icon"><i class="fas fa-times-circle"></i></span></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" Width="60px" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="ADD" CommandArgument="<%# Container.DataItem %>" ID="cmdAdd"
                                    ToolTip="Add"><span class="icon"><i class="fas fa-plus-square"></i></i></asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" ScrollHeight="" SaveScrollPosition="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:Confirm ID="ucConfirm" runat="server" Visible="false" CancelText="Cancel" OKText="Ok" OnConfirmMesage="ucConfirm_Click" />
            <%--  </div>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
