<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersSupplierApproval.aspx.cs" Inherits="RegistersSupplierApproval" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>--%>
<%--<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supplier Approval</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegistersSupplierApproval" autocomplete="off" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSupplierApproval" runat="server" OnTabStripCommand="MenuSupplierApproval_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            </div>

            <div id="divFind" style="position: relative; z-index: 2">
                <table>
                    <tr>
                        <td colspan="2" style="text-align: center">
                            <b>
                                <telerik:RadLabel ID="lblSHIPSTORESSPARESSUPPLIERWORKSHOPINITIALAPPROVALFORM" runat="server" Text="SHIP STORES/SPARES SUPPLIER/WORKSHOP INITIAL APPROVAL FORM"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            <telerik:RadLabel ID="lblNameOftheCompany" runat="server" Text="Name Of the Company"></telerik:RadLabel>
                        </td>
                        <td style="width: 80%">
                            <telerik:RadTextBox ID="txtCompanyName" runat="server" ReadOnly="true" Width="600px" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAddress" runat="server" ReadOnly="true" CssClass="input" Width="600px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTelephone" runat="server" Text="Telephone"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTelephone" runat="server" ReadOnly="true" CssClass="input" Width="600px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFax" runat="server" Text="Fax"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFax" runat="server" ReadOnly="true" CssClass="input" Width="600px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEmail" runat="server" Text="Email"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmail" runat="server" ReadOnly="true" CssClass="input" Width="600px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPIC" runat="server" Text="PIC"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPIC" runat="server" ReadOnly="true" CssClass="input" Width="600px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <%-- <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                FadeTransitions="false" FramesPerSecond="40" TransitionDuration="50" AutoSize="None"
                                RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                                <Panes>
                                    <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                                        <Header>
                                            <a href="" class="accordionLink">Department</a>
                                        </Header>
                                        <Content>
                                            <asp:CheckBoxList runat="server" ID="cblAddressDepartment" Height="26px" RepeatColumns="7"
                                                RepeatDirection="Horizontal" RepeatLayout="Table">
                                            </asp:CheckBoxList>
                                        </Content>
                                    </ajaxToolkit:AccordionPane>
                                    <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                                        <Header>
                                            <a href="" class="accordionLink">Address Type</a>
                                        </Header>
                                        <Content>
                                            <asp:CheckBoxList runat="server" ID="cblAddressType" Height="26px" RepeatColumns="5"
                                                RepeatDirection="Horizontal" RepeatLayout="Table">
                                            </asp:CheckBoxList>
                                        </Content>
                                    </ajaxToolkit:AccordionPane>
                                <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                                <Header>
                                    <a href="" class="accordionLink">Product/Services</a>
                                </Header>
                                <Content>
                                    <p>
                                        <asp:Literal ID="lblSelecttheProductServicesyouoffer" runat="server" Text="Select the Product/Services you offer"></asp:Literal>
                                    </p>
                                    <asp:CheckBoxList runat="server" ID="cblProduct" Height="26px" RepeatColumns="7"
                                        RepeatDirection="Horizontal" RepeatLayout="Table">
                                    </asp:CheckBoxList>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                            </Panes>
                            </ajaxToolkit:Accordion>--%>
                            <telerik:RadPanelBar RenderMode="Lightweight" ID="MyAccordion" runat="server" Width="100%">
                                <Items>
                                    <telerik:RadPanelItem Text="Department" Width="100%">
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Department"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:CheckBoxList runat="server" ID="cblAddressDepartment" Height="26px" RepeatColumns="7"
                                                RepeatDirection="Horizontal" RepeatLayout="Table">
                                            </asp:CheckBoxList>
                                        </ContentTemplate>
                                    </telerik:RadPanelItem>
                                    <telerik:RadPanelItem Text="Address Type" Width="100%">
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Address Type"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:CheckBoxList runat="server" ID="cblAddressType" Height="26px" RepeatColumns="5"
                                                RepeatDirection="Horizontal" RepeatLayout="Table">
                                            </asp:CheckBoxList>
                                        </ContentTemplate>
                                    </telerik:RadPanelItem>
                                    <telerik:RadPanelItem Text="Product/Services" Width="100%">
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblSelecttheProductServicesyouoffer" runat="server" Text="Select the Product/Services you offer"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:CheckBoxList runat="server" ID="cblProduct" Height="26px" RepeatColumns="7"
                                                RepeatDirection="Horizontal" RepeatLayout="Table">
                                            </asp:CheckBoxList>
                                        </ContentTemplate>
                                    </telerik:RadPanelItem>
                                </Items>
                            </telerik:RadPanelBar>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b>
                                <telerik:RadLabel ID="lblPleasecompletethebelowQuestionnaire" runat="server" Text="Please complete the below Questionnaire"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status:"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucSupplierStatus" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel runat="server" ID="ucVessel" VesselsOnly="true" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblProposedBy" runat="server" Text="Proposed By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtProposedBy" runat="server" ReadOnly="true" CssClass="input" Width="600px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblHSCQuestionnairefilledYN" runat="server" Text="HSC Questionnaire filled Y/N"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="optHSCQuestionnaire" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="0">NO</asp:ListItem>
                                <asp:ListItem Value="1">YES</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblReasonforintroducing" runat="server" Text="Reason for introducing"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtIntroducingReason" runat="server" CssClass="input" Height="30px" Width="600px" TextMode="MultiLine"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOtherAlternativesifany" runat="server" Text="Other Alternatives if any"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOtherAlternatives" runat="server" CssClass="input" Height="30px" Width="600px" TextMode="MultiLine"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRiskAssociatedifany" runat="server" Text="Risk Associated if any"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRiskAssociated" runat="server" CssClass="input" Height="30px" Width="600px" TextMode="MultiLine"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRemarksbySuperintendentFleetManager" runat="server" Text="Remarks by (Superintendent/Fleet Manager)"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSuperintendentRemarks" runat="server" CssClass="input" Height="30px" Width="600px" TextMode="MultiLine"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPurchaseExecutive" runat="server" Text="Purchase Executive"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtExecutiveApprovalDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                                        <telerik:RadButton ID="btnExecutiveApprove" runat="server" Text="Approve" CssClass="input"
                                            OnClick="btnExecutiveApprove_Click" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblPurchaseTechnicalSuperintendent" runat="server" Text="Purchase/Technical Superintendent"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtSuperintendentApprovalDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                                        <telerik:RadButton ID="btnSuperintendentApprove" runat="server" Text="Approve" CssClass="input"
                                            OnClick="btnSuperintendentApprove_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblFleetManager" runat="server" Text="Fleet Manager"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtFleetManagerApprovalDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                                        <telerik:RadButton ID="btnFleetManagerApprove" runat="server" Text="Approve" CssClass="input"
                                            OnClick="btnFleetManagerApprove_Click" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblDirector" runat="server" Text="Director"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtDirectoreApprovalDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                                        <telerik:RadButton ID="btnDirectorApprove" runat="server" Text="Approve" CssClass="input"
                                            OnClick="btnDirectorApprove_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                </table>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvSupplierApprove" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnSortCommand="gvSupplierApprove_SortCommand" OnNeedDataSource="gvSupplierApprove_NeedDataSource"
                    OnItemDataBound="gvSupplierApprove_ItemDataBound" OnItemCommand="gvSupplierApprove_ItemCommand" AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDSUPPLIERAPPROVALID" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Request Date" HeaderStyle-Width="80px" AllowSorting="true" SortExpression="FLDREQUESTDATE">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRequestDate" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDATE" ,"{0:dd/MMM/yyyy}") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Proposed By" HeaderStyle-Width="80px" AllowSorting="true" SortExpression="FLDREQUESTDATE">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSupplierApprovalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERAPPROVALID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblProposedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDBYUSER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel Name" HeaderStyle-Width="80px" AllowSorting="true" SortExpression="FLDREQUESTDATE">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
