<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantTravelDocument.aspx.cs"
    Inherits="CrewNewApplicantTravelDocument" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="../UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ECNR" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentType" Src="~/UserControls/UserControlDocumentType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTravelDocument" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


                <eluc:TabStrip ID="CrewPassPort" Title="Travel Document" runat="server" OnTabStripCommand="CrewPassPort_TabStripCommand"></eluc:TabStrip>

                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmployeeFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmployeeMiddleName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmployeeLastName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td colspan="4" align="left">
                            <a id="cdcChecker" runat="server" target="_blank">"Indian CDC" Checker</a>
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
                            <telerik:RadLabel ID="lblPassportNo" runat="server" Text="Passport Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPassportnumber" runat="server" MaxLength="15" CssClass="input_mandatory upperCase">
                            </telerik:RadTextBox>

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
                            <asp:LinkButton runat="server" AlternateText="Archive"
                                CommandName="Archive" ID="imgPassportArchive" ToolTip="Add" OnClick="OnClickPassportArchive">
                                <span class="icon"><i style="color:skyblue" class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPlaceofIssue" runat="Server" Text="Place of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" CssClass="input_mandatory" MaxLength="200"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblECNR" runat="server" Text="ECNR"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:ECNR ID="ucECNR" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMinimum3BlackPages" runat="server" Text="Minimum 3 Blank Pages"></telerik:RadLabel>
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
                            <telerik:RadTextBox ID="txtpptVerifiedby" Enabled="false" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblpptVerifieddate" runat="server" Text="Verified date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucpptVerifieddate" Enabled="false" runat="server" CssClass="readonlytextbox"/>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblpptVerifieddyn" runat="server" Text="Verified Y/N"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkpptVerifieddyn" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblpptcrosscheckby" runat="server" Text="Cross checked by"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtpptcrosscheckby" Enabled="false" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblpptcrosscheckdate" runat="server" Text="Cross checked date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucpptcrosscheckdate" Enabled="false" runat="server" CssClass="readonlytextbox" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblpptcrosscheck" runat="server" Text="Cross check Y/N"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkcrosscheck" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblUpdatedByPPT" runat="server" Text="Updated By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtUpdatedByPPT" Enabled="false" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblUpdatedDatePPT" runat="server" Text="Updated Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucUpdatedDatePPT" Enabled="false" runat="server" CssClass="readonlytextbox" />
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
                            <telerik:RadTextBox ID="txtSeamanBookNumber" runat="server" MaxLength="15" CssClass="input_mandatory"></telerik:RadTextBox>

                            <asp:Image ID="imgCCFlag" runat="server" Visible="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Flag ID="ucSeamanCountry" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDateofIssue3" runat="server" Text="Date of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucSeamanDateOfIssue" runat="server" CssClass="input_mandatory" />
                            <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgCCClip" runat="server" />
                            <asp:LinkButton runat="server" AlternateText="Archive"
                                CommandName="Archive" ID="imgSeamanBook" ToolTip="Add" OnClick="OnClickSeamanBookArchive">
                                <span class="icon"><i style="color:skyblue" class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
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
                            <telerik:RadTextBox ID="txtSeamanPlaceOfIssue" runat="server" CssClass="input_mandatory"
                                MaxLength="200">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text="Verified By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVerifiedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVerifiedOn" runat="server" Text="Verified On"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtVerifiedOn" Enabled="false" runat="server" CssClass="readonlytextbox" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVerifiedYN" runat="server" Text="Verified Y/N"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkVerifiedYN" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblcdccrosscheckedby" runat="server" Text="Cross checked by"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtcdccrosscheckedby" Enabled="false" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblcdccrosscheckeddate" runat="server" Text="Cross checked date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="uccdccrosscheckeddate" Enabled="false" runat="server" CssClass="readonlytextbox" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblcdccrosscheckedyn" runat="server" Text="Cross check Y/N"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkcdccrosscheckedyn" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblUpdatedByCDC" runat="server" Text="Updated By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtUpdatedByCDC" Enabled="false" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblUpdatedDateCDC" runat="server" Text="Updated Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucUpdatedDateCDC" Enabled="false" runat="server" CssClass="readonlytextbox" />
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
                            <telerik:RadTextBox ID="txtUSVisaType" runat="server" MaxLength="50" CssClass="input"></telerik:RadTextBox>
                            <asp:Image ID="imgUSVisa" runat="server" Visible="false" />

                        </td>
                        <td>
                            <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtUSVisaNumber" runat="server" CssClass="input" MaxLength="50"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblIssuedOn" runat="server" Text="Issued On"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtUSVisaIssuedOn" runat="server" CssClass="input" />
                            <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgUSVisaClip"
                                runat="server" />
                            <asp:LinkButton runat="server" AlternateText="Archive"
                                CommandName="Archive" ID="imgUSVisaArchive" ToolTip="Add" OnClick="OnClickUSVisaArchive">
                                <span class="icon"><i style="color:skyblue" class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDateofExpiry2" runat="server" Text="Date of Expiry"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtUSDateofExpiry" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPlaceofIssue2" runat="server" Text="Place of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtUSPlaceOfIssue" runat="server" CssClass="input" MaxLength="200"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblUpdatedByUS" runat="server" Text="Updated By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtUpdatedByUS" Enabled="false" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblUpdatedDateUS" runat="server" Text="Updated Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucUpdatedDateUS" Enabled="false" runat="server" CssClass="readonlytextbox" />
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
                            <telerik:RadLabel ID="lblTxNo" runat="server" Text=" Tx/No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMCVNumber" runat="server" CssClass="input" MaxLength="50"></telerik:RadTextBox>

                            <asp:Image ID="imgMCV" runat="server" Visible="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblIssuedOn1" runat="server" Text="Issued On"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtMCVIssuedOn" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDateofExpiry3" runat="server" Text="Date of Expiry"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtMCVDateofExpiry" runat="server" CssClass="input" />
                            <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgMCVClip" runat="server" />
                            <asp:LinkButton runat="server" AlternateText="Archive"
                                CommandName="Archive" ID="imgMCVArchive" ToolTip="Add" OnClick="OnClickMCVAustraliaArchive">
                                <span class="icon"><i style="color:skyblue" class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMCVRemarks" runat="server" CssClass="input" MaxLength="200"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblUpdatedByMCV" runat="server" Text="Updated By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtUpdatedByMCV" Enabled="false" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblUpdatedDateMCV" runat="server" Text="Updated Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucUpdatedDateMCV" Enabled="false" runat="server" CssClass="readonlytextbox" />
                        </td>
                    </tr>
                </table>
                <hr />
                <b>
                    <telerik:RadLabel ID="lblDocuments" runat="server" Text="Documents"></telerik:RadLabel>
                </b>
                <br />

                <eluc:TabStrip ID="MenuCrewNewApplicantTravelDocument" runat="server" OnTabStripCommand="CrewNewApplicantTravelDocumentMenu_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelDocument" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvTravelDocument_NeedDataSource"
                    OnItemCommand="gvTravelDocument_ItemCommand"
                    OnItemDataBound="gvTravelDocument_ItemDataBound"
                    OnSortCommand="gvTravelDocument_SortCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
                        <HeaderStyle Width="102px" />
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
                            <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Document Name">
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDocumentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDocumentTypename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblDocumentTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                    <eluc:DocumentType ID="ucDocumentTypeEdit" runat="server" DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString()+ "," + ((int)PhoenixDocumentType.OTHER_DOC_WPT).ToString())%>'
                                        SelectedDocumentType='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'
                                        CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged"
                                        AutoPostBack="true" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:DocumentType ID="ucDocumentTypeAdd" runat="server" DocumentType='<%# ((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString()+ "," + ((int)PhoenixDocumentType.OTHER_DOC_WPT).ToString()%>'
                                        DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString()+ "," + ((int)PhoenixDocumentType.OTHER_DOC_WPT).ToString())%>'
                                        AppendDataBoundItems="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged"
                                        AutoPostBack="true" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FooterText="Number" HeaderText="Number">
                                <HeaderStyle Width="100px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbltraveldocumentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkNumber" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>"
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
                            <telerik:GridTemplateColumn HeaderText="Date of Issue">
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDateOfIssue" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucDateOfIssueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>'
                                        CssClass="dropdown_mandatory" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="ucDateOfIssueAdd" runat="server" CssClass="dropdown_mandatory" Width="100%" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Valid From">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblValidFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALIDFROM","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucValidFromEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALIDFROM") %>'
                                        Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="ucValidFromAdd" runat="server" Width="100%" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Expiry Date">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucDateExpiryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>' Width="100%"
                                        CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="ucDateExpiryAdd" runat="server" CssClass="dropdown_mandatory" Width="100%" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="No of entries">
                                <HeaderStyle Width="100px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNoofentry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFENTRYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox ID="ddlNoofentryEdit" runat="server" AppendDataBoundItems="true" Width="100%" Filter="Contains" EmptyMessage="Type to select status" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                            <telerik:RadComboBoxItem Value="1" Text="Single" />
                                            <telerik:RadComboBoxItem Value="2" Text="Multiple" />

                                        </Items>

                                    </telerik:RadComboBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="ddlNoofentryAdd" runat="server" AppendDataBoundItems="true" Width="100%" Filter="Contains" EmptyMessage="Type to select status" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                            <telerik:RadComboBoxItem Value="1" Text="Single" />
                                            <telerik:RadComboBoxItem Value="2" Text="Multiple" />

                                        </Items>
                                    </telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Place of Issue">
                                <HeaderStyle Width="100px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPlaceOfissue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'
                                        MaxLength="200" Width="100%">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtPlaceIssueAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" Width="100%">
                                    </telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Nationality/Flag">
                                <HeaderStyle Width="100px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYNAME").ToString()%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Country ID="ddlCountryEdit" runat="server" AppendDataBoundItems="true" Width="100%"
                                        CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' SelectedCountry='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYCODE").ToString()%>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Country ID="ddlCountryAdd" runat="server" AppendDataBoundItems="true" Width="100%"
                                        CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks">
                                <HeaderStyle Width="75px" />
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblremarktext" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lblRemarks" runat="server" CommandArgument="<%# Container.DataSetIndex %>">                                       
                                       <span class="icon"><i class="fas fa-glasses"></i></span>
                                    </asp:LinkButton>

                                </ItemTemplate>
                                <EditItemTemplate>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Passport No">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblpassportno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtpassportnoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'
                                        MaxLength="100" Width="100%">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtpassportnoAdd" runat="server" CssClass="gridinput" MaxLength="100" Width="100%"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Doc Status">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDDOCSTATUS")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox ID="ddlDocStatusEdit" runat="server" Width="100%" Filter="Contains" EmptyMessage="Type to select status" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                            <telerik:RadComboBoxItem Value="0" Text="Not Valid" />
                                            <telerik:RadComboBoxItem Value="1" Text="Valid" />
                                        </Items>

                                    </telerik:RadComboBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="ddlDocStatusAdd" runat="server" Width="100%" Filter="Contains" EmptyMessage="Type to select status" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                            <telerik:RadComboBoxItem Value="0" Text="Not Valid" />
                                            <telerik:RadComboBoxItem Value="1" Text="Valid" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Full Term YN" Visible="false">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDFULLTERM")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle Width="100Px" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Delete"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete">
                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Attachment"
                                        CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                        ToolTip="Attachment">
                                     <span class="icon"><i class="fas fa-paperclip"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Archive"
                                        CommandName="Archive" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdArchive"
                                        ToolTip="Archive">
                                    <span class="icon"><i class="fas fa-archive"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                        ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="CANCEL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                        ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </FooterTemplate>
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

                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDocumentsExpired" runat="server" Text="* Documents Expired"></telerik:RadLabel>
                        </td>
                        <td>
                            <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDocumentsExpirinceIn120Days" runat="server" Text="* Documents Expiring in 120 Days"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
