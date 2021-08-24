<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantFamilyMedicalDocument.aspx.cs"
    Inherits="CrewNewApplicantFamilyMedicalDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vaccination" Src="~/UserControls/UserControlDocumentVaccination.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>New Applicant Medical Documents</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewMedical" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="CrewFamilyMedical" runat="server" OnTabStripCommand="CrewFamilyMedical_TabStripCommand" Title="Family NOK / Medical" TabStrip="true"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <b>
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Medical"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="MenuCrewMedical" runat="server" OnTabStripCommand="CrewMedical_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvCrewMedical" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true" ShowHeader="true" EnableViewState="false"
                OnNeedDataSource="gvCrewMedical_NeedDataSource" OnItemDataBound="gvCrewMedical_ItemDataBound" OnItemCommand="gvCrewMedical_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Medical">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"></asp:LinkButton>
                                <telerik:RadLabel ID="lblMedicalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWFLAGMEDICALID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMedicalName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"></asp:LinkButton>
                                <telerik:RadLabel ID="lblMedicalIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWFLAGMEDICALID") %>'></telerik:RadLabel>
                                <eluc:Hard runat="server" ID="ucMedicalEdit" CssClass="dropdown_mandatory" AppendDataBoundItems="true" SelectedHard='<%#DataBinder.Eval(Container,"DataItem.FLDCREWFLAGMEDICALID") %>'
                                    HardTypeCode="95" ShortNameFilter="P&I,UKP,FLM" AutoPostBack="true" OnTextChangedEvent="DisableFlag" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard runat="server" ID="ucMedicalAdd" HardList='<%# PhoenixRegistersHard.ListHard(1, 95) %>'
                                    CssClass="dropdown_mandatory" AppendDataBoundItems="true" HardTypeCode="95" ShortNameFilter="P&I,UKP,FLM" AutoPostBack="true" OnTextChangedEvent="DisableFlag" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueAdd" runat="server" CssClass="input_mandatory" MaxLength="200"
                                    ToolTip="Enter Place of issue">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateAdd" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Flag runat="server" ID="ucFlagEdit" CssClass="input" AppendDataBoundItems="true"
                                    FlagList='<%# PhoenixRegistersFlag.ListFlag(1) %>' MedicalRequiredYN="1" SelectedFlag='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Flag runat="server" ID="ucFlagAdd" CssClass="input" MedicalRequiredYN="1" AppendDataBoundItems="true"
                                    FlagList='<%# PhoenixRegistersFlag.ListFlag(1) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard runat="server" ID="ddlStatus" CssClass="input_mandatory" AppendDataBoundItems="true" HardTypeCode="105"
                                    HardList='<%# PhoenixRegistersHard.ListHard(SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105) %>' SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard runat="server" ID="ddlStatusAdd" CssClass="input_mandatory" AppendDataBoundItems="true" HardTypeCode="105"
                                    HardList='<%# PhoenixRegistersHard.ListHard(SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="MEDICALDELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAtt" ToolTip="Attachment" CommandName="MEDICALATTACHMENT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Archive" ID="cmdArchive" CommandName="MEDICALARCHIVE" ToolTip="Archive" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="MEDICALUPDATE" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="MEDICALCANCEL" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="MEDICALADD" ToolTip="Add New" Width="20px" Height="20px">
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowExpandCollapse="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <b>
                <telerik:RadLabel ID="RadLabel2" runat="server" Text="Medical Test"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="CrewMedicalTest" runat="server" OnTabStripCommand="CrewMedicalTest_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvMedicalTest" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true" ShowHeader="true" EnableViewState="false"
                OnNeedDataSource="gvMedicalTest_NeedDataSource" OnItemDataBound="gvMedicalTest_ItemDataBound" OnItemCommand="gvMedicalTest_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Medical Test">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMedicalTestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWMEDICALTESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMedicalTestName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFMEDICAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"></asp:LinkButton>
                                <telerik:RadLabel ID="lblMedicalTestIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWMEDICALTESTID") %>'></telerik:RadLabel>
                                <eluc:Documents runat="server" ID="ucMedicalTestEdit" CssClass="dropdown_mandatory" AppendDataBoundItems="true" DocumentType="MEDICAL" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Documents runat="server" ID="ucMedicalTestAdd" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" DocumentList='<%# PhoenixRegistersDocumentMedical.ListDocumentMedical() %>'
                                    DocumentType="MEDICAL" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place Of Issue">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueAdd" runat="server" CssClass="input" MaxLength="200"
                                    ToolTip="Enter Place of issue">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateAdd" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="MEDICALTESTDELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAtt" ToolTip="Attachment" CommandName="MEDICALTESTATTACHMENT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Archive" ID="cmdArchive" CommandName="MEDICALTESTARCHIVE" ToolTip="Archive" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="MEDICALTESTUPDATE" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="MEDICALTESTCANCEL" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="MEDICALTESTADD" ToolTip="Add New" Width="20px" Height="20px">
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" Position="Bottom" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowExpandCollapse="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <b>
                <telerik:RadLabel ID="RadLabel3" runat="server" Text="Vaccination"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="CrewVaccination" runat="server" OnTabStripCommand="CrewVaccination_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvVaccination" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true" ShowHeader="true" EnableViewState="false"
                OnNeedDataSource="gvVaccination_NeedDataSource" OnItemDataBound="gvVaccination_ItemDataBound" OnItemCommand="gvVaccination_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vaccination">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCrewVaccinationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWVACCINATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVaccinationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFMEDICAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"></asp:LinkButton>
                                <telerik:RadLabel ID="lblCrewVaccinationIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWVACCINATIONID") %>'></telerik:RadLabel>
                                <eluc:Vaccination runat="server" ID="ucVaccinationEdit" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" DocumentList='<%# PhoenixRegistersDocumentMedical.ListDocumentVaccination() %>'
                                    SelectedDocument='<%# DataBinder.Eval(Container,"DataItem.FLDVACCINATIONID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Vaccination runat="server" ID="ucVaccinationAdd" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" DocumentList='<%# PhoenixRegistersDocumentMedical.ListDocumentVaccination() %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place Of Issue">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueAdd" runat="server" CssClass="input" MaxLength="200"
                                    ToolTip="Enter Place of issue">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblIssueDateHeader" runat="server">Issue Date</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateAdd" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="VACCINATIONDELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAtt" ToolTip="Attachment" CommandName="VACCINATIONATTACHMENT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Archive" ID="cmdArchive" CommandName="VACCINATIONARCHIVE" ToolTip="Archive" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="VACCINATIONUPDATE" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="VACCINATIONCANCEL" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="VACCINATIONADD" ToolTip="Add New" Width="20px" Height="20px">
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" Position="Bottom" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowExpandCollapse="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                    </td>
                    <td>* Documents Expired
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>* Documents Expiring in 120 Days
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
