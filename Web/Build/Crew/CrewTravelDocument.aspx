<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelDocument.aspx.cs"
    Inherits="CrewTravelDocument" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="../UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ECNR" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentType" Src="~/UserControls/UserControlDocumentType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTravelDocument" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewPassPort" runat="server" Title="Travel" OnTabStripCommand="CrewPassPort_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNumber" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblPassportDetail" runat="server" Text="Passport Detail"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumber" runat="server" Text="Passport Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassportnumber" runat="server" CssClass="input_mandatory upperCase"></telerik:RadTextBox>
                        <asp:Image ID="imgPPFlag" runat="server" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssue" runat="server" Text="Date of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfIssue" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text="Date of Expiry"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfExpiry" runat="server" CssClass="input_mandatory" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClip" runat="server" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/pdf.png %>" ID="imgPPDF" ToolTip="Export to PDF"
                            runat="server" />
                        <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                            CommandName="Archive" ID="imgPassportArchive" ToolTip="Add" OnClick="OnClickPassportArchive"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblECNR" runat="server" Text="ECNR"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ECNR ID="ucECNR" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMinimum3BlankPages" runat="server" Text="Minimum 3 Blank Pages"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ECNR ID="ucBlankPages" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            ShortNameFilter="S,N" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblpptVerifiedby" runat="server" Text="Verified by"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtpptVerifiedby" Enabled="false" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblpptVerifieddate" runat="server" Text="Verified date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucpptVerifieddate" Enabled="false" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblpptVerifieddyn" runat="server" Text="Verified Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkpptVerifieddyn" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblpptcrosscheckby" runat="server" Text="Cross checked by"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtpptcrosscheckby" Enabled="false" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblpptcrosscheckdate" runat="server" Text="Cross checked date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucpptcrosscheckdate" Enabled="false" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblpptcrosscheck" runat="server" Text="Cross check Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkcrosscheck" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedByPPT" runat="server" Text="Updated By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedByPPT" Enabled="false" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedDatePPT" runat="server" Text="Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucUpdatedDatePPT" Enabled="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblSeamansBookDetail" runat="server" Text="Seaman's Book Detail"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeamansBookNumber" runat="server" Text="Seaman's Book Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanBookNumber" runat="server" MaxLength="50" CssClass="input_mandatory"></telerik:RadTextBox>
                        <asp:Image ID="imgCCFlag" runat="server" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Flag ID="ucSeamanCountry" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <a id="cdcChecker" runat="server" target="_blank">"CDC" Checker</a>
                    </td>
                    <td>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgCCClip" runat="server" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/pdf.png %>" ID="imgCPDF" ToolTip="Export to PDF"
                            runat="server" />
                        <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                            CommandName="Archive" ID="imgSeamanBook" ToolTip="Add" OnClick="OnClickSeamanBookArchive"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssue1" runat="server" Text="Date of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucSeamanDateOfIssue" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiry1" runat="server" Text="Date of Expiry"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucSeamanDateOfExpiry" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue1" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeamanPlaceOfIssue" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text="Verified By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVerifiedBy" runat="server" Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVerifiedOn" runat="server" Text="Verified On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtVerifiedOn" Enabled="false" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVerifiedYN" runat="server" Text="Verified Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkVerifiedYN" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcdccrosscheckedby" runat="server" Text="Cross checked by"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcdccrosscheckedby" Enabled="false" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcdccrosscheckeddate" runat="server" Text="Cross checked date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="uccdccrosscheckeddate" Enabled="false" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcdccrosscheckedyn" runat="server" Text="Cross check Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkcdccrosscheckedyn" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedByCDC" runat="server" Text="Updated By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedByCDC" Enabled="false" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedDateCDC" runat="server" Text="Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucUpdatedDateCDC" Enabled="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                        <b>
                            <telerik:RadLabel ID="lblUSVisa" runat="server" Text="US Visa"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUSVisaType" runat="server" MaxLength="50"></telerik:RadTextBox>
                        <asp:Image ID="imgUSVisa" runat="server" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUSVisaNumber" runat="server" MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIssuedOn" runat="server" Text="Issued On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtUSVisaIssuedOn" runat="server" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgUSVisaClip"
                            runat="server" />
                        <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                            CommandName="Archive" ID="imgUSVisaArchive" ToolTip="Add" OnClick="OnClickUSVisaArchive"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiry2" runat="server" Text="Date of Expiry"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtUSDateofExpiry" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue2" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUSPlaceOfIssue" runat="server" MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedByUS" runat="server" Text="Updated By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedByUS" Enabled="false" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedDateUS" runat="server" Text="Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucUpdatedDateUS" Enabled="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                        <b>
                            <telerik:RadLabel ID="lblMCV" runat="server" Text="MCV(Australia)"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTxNo" runat="server" Text="Tx/No"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMCVNumber" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:Image ID="imgMCV" runat="server" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIssuedOn1" runat="server" Text="Issued On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtMCVIssuedOn" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiry3" runat="server" Text="Date of Expiry"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtMCVDateofExpiry" runat="server" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgMCVClip" runat="server" />
                        <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                            CommandName="Archive" ID="imgMCVArchive" ToolTip="Add" OnClick="OnClickMCVAustraliaArchive"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMCVRemarks" runat="server" MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedByMCV" runat="server" Text="Updated By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedByMCV" Enabled="false" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedDateMCV" runat="server" Text="Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucUpdatedDateMCV" Enabled="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
            </table>
            </hr> <b>
                <telerik:RadLabel ID="lblDocuments" runat="server" Text="Documents"></telerik:RadLabel>
            </b>
            <br />
            <eluc:TabStrip ID="MenuCrewTravelDocument" runat="server" OnTabStripCommand="CrewTravelDocumentMenu_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelDocument" runat="server"  EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnEditCommand="gvTravelDocument_EditCommand"
                OnNeedDataSource="gvTravelDocument_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvTravelDocument_ItemDataBound"
                OnItemCommand="gvTravelDocument_ItemCommand" OnUpdateCommand="gvTravelDocument_UpdateCommand" ShowFooter="True" OnDeleteCommand="gvTravelDocument_DeleteCommand"
                OnSortCommand="gvTravelDocument_SortCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"  HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" >
                    <HeaderStyle Width="140px" />
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
                        <telerik:GridTemplateColumn HeaderText="" AllowSorting="false" UniqueName="FlagIconHeader" ShowSortIcon="true" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document" AllowSorting="true" DataField="FLDDOCUMENTNAME" ShowSortIcon="true" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentTypename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDocumentTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                <eluc:DocumentType ID="ucDocumentTypeEdit" runat="server" DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString()+","+((int)PhoenixDocumentType.OTHER_DOC_WPT).ToString())%>'
                                    SelectedDocumentType='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'
                                    CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged" Width="100%"
                                    AutoPostBack="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:DocumentType ID="ucDocumentTypeAdd" runat="server" DocumentType='<%# ((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString()+","+((int)PhoenixDocumentType.OTHER_DOC_WPT).ToString()%>'
                                    DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString()+","+((int)PhoenixDocumentType.OTHER_DOC_WPT).ToString())%>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged" Width="100%"
                                    AutoPostBack="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" AllowSorting="true" DataField="FLDDOCUMENTNUMBER" ShowSortIcon="true" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltraveldocumentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkNumber" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lbltraveldocumentidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="30" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtNumberAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="30" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Issue" AllowSorting="false" ShowSortIcon="true" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="Label1" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="ucDateOfIssueEdit" CssClass="input_mandatory"
                                    Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE")) %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="ucDateOfIssueAdd" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Valid From" AllowSorting="false" ShowSortIcon="true" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblValidFrom" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDVALIDFROM","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="ucValidFromEdit"
                                    Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDVALIDFROM")) %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="ucValidFromAdd" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date" AllowSorting="true" DataField="FLDDATEOFEXPIRY" ShowSortIcon="true" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDateExpiryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>'
                                    CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="ucDateExpiryAdd" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Entries" AllowSorting="false" ShowSortIcon="true" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoofentry" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNOOFENTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlNoofentryEdit" runat="server" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="1" Text="Single" />
                                        <telerik:RadComboBoxItem Value="2" Text="Multiple" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlNoofentryAdd" runat="server" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="1" Text="Single" />
                                        <telerik:RadComboBoxItem Value="2" Text="Multiple" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue" AllowSorting="false" ShowSortIcon="true" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>' Width="100%"
                                    CssClass="gridinput">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceIssueAdd" runat="server" CssClass="gridinput_mandatory" Width="100%"
                                    MaxLength="200">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality/Flag" AllowSorting="false" ShowSortIcon="true" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Country ID="ddlCountryEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="100%"
                                    CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' SelectedCountry='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYCODE").ToString()%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Country ID="ddlCountryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="100%"
                                    CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Connected" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConnectedToVessel" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDISCONNECTEDTOVESSEL").ToString() == "1" ? "Yes" : "No"%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkConnectedtoVesselEdit" runat="server" Width="100%" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDISCONNECTEDTOVESSEL").ToString() == "1" ? true : false%>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkConnectedToVesselAdd" runat="server" Width="100%"></telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" UniqueName="RemarksHeader" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="70px" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="Remarks" ID="imgRemarks"
                                    ToolTip="Remarks">
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport" AllowSorting="false" ShowSortIcon="true" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpassportno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtpassportnoEdit" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtpassportnoAdd" runat="server" CssClass="gridinput_mandatory" Width="100%" MaxLength="100"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" UniqueName="StatusHeader" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocStatus" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDOCSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="false" SortExpression="" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAtt" ToolTip="Attachment" CommandName="Attachment" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Archive" ID="cmdArchive" CommandName="Archive" ToolTip="Archive" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="Add" ToolTip="Add New" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" EnableNextPrevFrozenColumns="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDocumentsExpired" runat="server" Text="* Documents Expired"></asp:Literal>
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDocumentsExpirinceIn120Days" runat="server" Text="* Documents Expiring in 120 Days"></asp:Literal>
                    </td>
                </tr>
            </table>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
