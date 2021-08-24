<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDocumentsList.aspx.cs"
    Inherits="CrewOffshoreDocumentsList" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="../UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentType" Src="~/UserControls/UserControlDocumentType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew OffShore Documents</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewOffShoreDocuments" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="tskin" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlDocument" Height="100%">
            <%--            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:Title runat="server" ID="TravelDocument" Text="Documents" ShowMenu="false" Visible="false"></eluc:Title>

            <eluc:TabStrip ID="CrewPassPort" runat="server" OnTabStripCommand="CrewPassPort_TabStripCommand" Title="Documents"></eluc:TabStrip>
            <%--                </div>--%>
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
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
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
                        <eluc:Hard ID="ucECNR" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMinimum3BlankPages" runat="server" Text="Minimum 3 Blank Pages"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucBlankPages" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
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
                        <eluc:Date ID="ucpptVerifieddate" Enabled="false" runat="server" CssClass="readonlytextbox" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblpptVerifieddyn" runat="server" Text="Verified Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkpptVerifieddyn" runat="server" />
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
                        <telerik:RadCheckBox ID="chkcrosscheck" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedByPPT" runat="server" Text="Updated by"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedByPPT" Enabled="false" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedDatePPT" runat="server" Text="Updated date"></telerik:RadLabel>
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
                        <telerik:RadTextBox ID="txtSeamanBookNumber" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                        <asp:Image ID="imgCCFlag" runat="server" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Flag ID="ucSeamanCountry" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssue1" runat="server" Text="Date of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucSeamanDateOfIssue" runat="server" CssClass="input_mandatory" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgCCClip" runat="server" />
                        <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                            CommandName="Archive" ID="imgSeamanBook" ToolTip="Add" OnClick="OnClickSeamanBookArchive"></asp:ImageButton>
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
                        <telerik:RadTextBox ID="txtSeamanPlaceOfIssue" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text="Verified by"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVerifiedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVerifiedOn" runat="server" Text="Verified date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtVerifiedOn" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVerifiedYN" runat="server" Text="Verified Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkVerifiedYN" runat="server" />
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
                        <telerik:RadCheckBox ID="chkcdccrosscheckedyn" runat="server" />
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
                    </td>
                </tr>
            </table>
            <table id="tblFilter" runat="server">
                <tr>
                    <td>
                        <telerik:RadCheckBox ID="chkPendingAuthentication" runat="server" AutoPostBack="true" />
                        <telerik:RadLabel ID="lblPendingAuthenticationYN" runat="server" Text="Pending Authentication"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkPendingCrosscheck" runat="server" AutoPostBack="true" />
                        <telerik:RadLabel ID="lblPendingCrosscheckYN" runat="server" Text="Pending Cross check"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <b>
                <telerik:RadLabel ID="lblDocuments" runat="server" Text=" Travel Documents"></telerik:RadLabel>
            </b>
            <br />
            <eluc:TabStrip ID="MenuCrewTravelDocument" runat="server" OnTabStripCommand="CrewTravelDocumentMenu_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelDocument" runat="server" AutoGenerateColumns="False" GroupingEnabled="false" EnableHeaderContextMenu="true" Font-Size="11px"
                CellPadding="3" OnItemCommand="gvTravelDocument_ItemCommand" OnItemDataBound="gvTravelDocument_ItemDataBound"
                OnDeleteCommand="gvTravelDocument_DeleteCommand" OnNeedDataSource="gvTravelDocument_NeedDataSource"
                OnUpdateCommand="gvTravelDocument_UpdateCommand" AllowPaging="true" AllowCustomPaging="true" Height=""
                ShowFooter="True" EnableViewState="false" AllowSorting="true" OnSortCommand="gvTravelDocument_SortCommand">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <%--                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>--%>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <%--                            <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="40px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText="Document Name" SortExpression="FLDDOCUMENTNAME">

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
                                    CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged"
                                    AutoPostBack="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:DocumentType ID="ucDocumentTypeAdd" runat="server" Width="100%" DocumentType='<%# (((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + (int)PhoenixDocumentType.OTHER_DOC_CDC).ToString()+","+((int)PhoenixDocumentType.OTHER_DOC_WPT).ToString()%>'
                                    DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString()+","+((int)PhoenixDocumentType.OTHER_DOC_WPT).ToString())%>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FooterText="Number" HeaderStyle-Width="75px" HeaderText="Number" SortExpression="FLDDOCUMENTNUMBER">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltraveldocumentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblNumber" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkNumber" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItem %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lbltraveldocumentidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="30">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtNumberAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="30" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString()%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Issue" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateOfIssue" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDateOfIssueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>'
                                    CssClass="dropdown_mandatory" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucDateOfIssueAdd" runat="server" CssClass="dropdown_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Valid From" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblValidFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALIDFROM","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucValidFromEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALIDFROM") %>'
                                    CssClass="input" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucValidFromAdd" runat="server" CssClass="input" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="150px" HeaderText="Expiry Date" SortAscImageUrl="FLDDATEOFEXPIRY">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDateExpiryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>'
                                    CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucDateExpiryAdd" runat="server" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No of entries" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoofentry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFENTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlNoofentryEdit" runat="server" CssClass="input" AppendDataBoundItems="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="1" Text="Single"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="2" Text="Multiple"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlNoofentryAdd" runat="server" CssClass="input" Width="100%" AppendDataBoundItems="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="1" Text="Single"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="2" Text="Multiple"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfissue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'
                                    CssClass="gridinput">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceIssueAdd" runat="server" Width="100%" CssClass="gridinput_mandatory"
                                    MaxLength="200">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality/Flag" HeaderStyle-Width="135px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYNAME").ToString()%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Country ID="ddlCountryEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' SelectedCountry='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYCODE").ToString()%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Country ID="ddlCountryAdd" runat="server" CssClass="input_mandatory" Width="100%" AppendDataBoundItems="true"
                                    CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Connected to Vessel" HeaderStyle-Width="85px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConnectedToVessel" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISCONNECTEDTOVESSEL").ToString() == "1" ? "Yes" : "No"%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkConnectedtoVesselEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDISCONNECTEDTOVESSEL").ToString() == "1" ? true : false%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkConnectedToVesselAdd" Width="100%" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="95px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <img id="imgRemarks" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/te_view.png %>"
                                    onmousedown="javascript:closeMoreInformation()" />
                            </ItemTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FooterText="Number" HeaderText="Passport No" HeaderStyle-Width="115px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpassportno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtpassportnoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'
                                    CssClass="gridinput" MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtpassportnoAdd" runat="server" CssClass="gridinput" Width="100%" MaxLength="100"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Doc Status" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDOCSTATUS")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Full Term YN" Visible="false">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFULLTERM")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlFullTermEdit" runat="server" CssClass="input">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="0" Text="CRA"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="1" Text="Full Term"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlFullTermAdd" runat="server" Width="100%" CssClass="input">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="0" Text="CRA"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="1" Text="Full Term"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated Date" HeaderStyle-Width="105px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDON")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated By" HeaderStyle-Width="105px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked Date" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked by" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Verificatiom Method" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVerificationMethodByHeader" runat="server">
                                    Verification Method
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerificationMethod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVRIMETHODNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAuthenticationRequYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelLockYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLOCKYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucVerificationMethodEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="131"></eluc:Quick>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucVerificationMethodAdd" runat="server" CssClass="input" Width="100%" AppendDataBoundItems="true"
                                    QuickTypeCode="131"></eluc:Quick>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Seafarer" UniqueName="ISADDEDBYSEAFARER" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblisbyseafarer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISADDEDBYSEAFARER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                                <%--  <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="Attachment" CommandArgument='<%# Container.DataItem %>' ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>--%>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="Attachment" CommandArgument="<%# Container.DataItem %>" ID="cmdAtt" ToolTip="Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <%--                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />--%>
                                <%--     <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                    CommandName="Archive" CommandArgument='<%# Container.DataItem %>' ID="cmdArchive"
                                    ToolTip="Archive"></asp:ImageButton>--%>
                                <asp:LinkButton runat="server" AlternateText="Archive" CommandName="Archive" CommandArgument="<%# Container.DataItem %>" ID="cmdArchive" ToolTip="Archive">
                                    <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>
                                <%--                                <img id="Img8" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />--%>
                                <asp:ImageButton runat="server" AlternateText="Unlock" ImageUrl="<%$ PhoenixTheme:images/unlock.png %>"
                                    CommandName="UNLOCKTRAVELDOC" Visible="false" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdUnlock" ToolTip="Unlock"></asp:ImageButton>
                                <img id="Img9" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Authentication" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    CommandName="AUTHENTICATIONTRAVELDOC" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdAuthenticate" ToolTip="Authentication"></asp:ImageButton>
                                <img id="Img10" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="CrossCheck" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                    CommandName="CROSSCHECKTRAVELDOC" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdCrossCheck" ToolTip="Cross Check"></asp:ImageButton>
                                <%--   <asp:LinkButton runat="server" CommandName="CROSSCHECKTRAVELDOC" ID="cmdCrossCheck" ToolTip="Cross Check">
                                        <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                    CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img alt="" src="../css/Theme1/images/spacer.png" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <%--  <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/te_check.png%>'
                                    CommandName="Add" CommandArgument="<%# Container.DataItem %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>--%>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--                </div>--%>

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
            <br />
            <br />
            <b>
                <telerik:RadLabel ID="lblNationalDocuments" runat="server" Text="Licence - National Documents"></telerik:RadLabel>
            </b>
            <br />
            <eluc:TabStrip ID="MenuCrewLicence" runat="server" OnTabStripCommand="CrewLicence_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewLicence" runat="server" AutoGenerateColumns="False" GroupingEnabled="false" EnableHeaderContextMenu="true" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvCrewLicence_ItemCommand" OnItemDataBound="gvCrewLicence_ItemDataBound"
                OnDeleteCommand="gvCrewLicence_DeleteCommand" OnNeedDataSource="gvCrewLicence_NeedDataSource"
                OnSelectedIndexChanging="gvCrewLicence_SelectedIndexChanging" AllowPaging="true" AllowCustomPaging="true"
                OnUpdateCommand="gvCrewLicence_RowUpdating" ShowFooter="true" ShowHeader="true">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>

                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <%--                        <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Licence" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLicenceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEELICENCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWatchkeepingyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHKEEPINGYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLicenceName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblLicenceIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEELICENCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLicenceNameEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCENAME") %>'></telerik:RadLabel>
                                <eluc:Documents ID="ddlLicenceEdit" runat="server" DocumentList="<%# PhoenixRegistersDocumentLicence.ListDocumentLicence(null,null,1,null,null) %>"
                                    SelectedDocument='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCE") %>' AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" DocumentType="LICENCE" AutoPostBack="true" OnTextChangedEvent="ddlDocumentLIC_TextChanged"
                                    Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? false : true%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Documents ID="ddlLicenceAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="100%"
                                    DocumentType="LICENCE" AutoPostBack="true" DocumentList="<%# PhoenixRegistersDocumentLicence.ListDocumentLicence(null,null,1,null,null) %>"
                                    OnTextChangedEvent="ddlDocumentLIC_TextChanged" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="135px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString()%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOCUMENTTYPENAME").ToString()%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Licence Number" FooterText="Number" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNumber" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCENUMBER") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkLicenceName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItem %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCENUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtLicenceNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCENUMBER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="50" Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? false : true%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtLicenceNumberAdd" runat="server" Width="100%" CssClass="gridinput_mandatory"
                                    MaxLength="50" ToolTip="">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue" HeaderStyle-Width="130px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueEdit" runat="server" CssClass="gridinput_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>' Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? false : true%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueAdd" runat="server" Width="100%" CssClass="gridinput_mandatory"
                                    MaxLength="200" ToolTip="Enter Place of issue">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateAdd" Width="100%" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateEdit" CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateAdd" Width="100%" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFLAGNAME").ToString()%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Country ID="ddlFlagEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? false : true%>'
                                    CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' SelectedCountry='<%# DataBinder.Eval(Container, "DataItem.FLDFLAGID").ToString()%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Country ID="ddlFlagAdd" runat="server" CssClass="input_mandatory" Width="100%" AppendDataBoundItems="true"
                                    CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated Date" HeaderStyle-Width="115px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDON")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated By" HeaderStyle-Width="115px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked Date" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked by" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Verificatiom Method" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerificationMethod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVRIMETHODNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLicenceLockYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLOCKYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucVerificationMethodEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="131"></eluc:Quick>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucVerificationMethodAdd" runat="server" Width="100%" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="131"></eluc:Quick>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issuing Authority" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDISSUEDBY") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtIssuedByEdit" runat="server" Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? false : true%>'
                                    CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDBY")%>'
                                    MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtIssuedByAdd" runat="server" Width="100%" CssClass="gridinput" MaxLength="200"
                                    ToolTip="Enter Issuing Authority">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Limitation Remarks" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <img id="imgRemarks" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/te_view.png %>"
                                    onmousedown="javascript:closeMoreInformation()" />
                                <telerik:RadLabel ID="lblAuthenticationRequYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRemarksAdd" runat="server" Width="100%" CssClass="gridinput"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="true" HeaderText="Seafarer" UniqueName="ISADDEDBYSEAFARER" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblisbyseafarer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISADDEDBYSEAFARER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="Edit" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="Attachment" CommandArgument='<%# Container.DataItem %>' ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                    CommandName="Archive" CommandArgument='<%# Container.DataItem %>' ID="cmdArchive"
                                    ToolTip="Archive"></asp:ImageButton>
                                <img id="Img8" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Unlock" ImageUrl="<%$ PhoenixTheme:images/unlock.png %>"
                                    CommandName="UNLOCKLICENCE" Visible="false" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdUnlock" ToolTip="Unlock"></asp:ImageButton>
                                <img id="Img9" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Authentication" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    CommandName="AUTHENTICATIONLICENCE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdAuthenticate" ToolTip="Authentication"></asp:ImageButton>
                                <img id="Img10" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="CrossCheck" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                    CommandName="CROSSCHECKLICENCE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdCrossCheck" ToolTip="Cross Check"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItem %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItem %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <%--  <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItem %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>--%>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--            </div>--%>
            <br />
            <br />
            <b>
                <telerik:RadLabel ID="lblMedical" runat="server" Text="Medical"></telerik:RadLabel>
            </b>
            <br />
            <eluc:TabStrip ID="MenuCrewMedical" runat="server" OnTabStripCommand="CrewMedical_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewMedical" runat="server" EnableHeaderContextMenu="true" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvCrewMedical_ItemCommand" OnItemDataBound="gvCrewMedical_ItemDataBound"
                OnDeleteCommand="gvCrewMedical_DeleteCommand" OnNeedDataSource="gvCrewMedical_NeedDataSource"
                OnSelectedIndexChanging="gvCrewMedical_SelectedIndexChanging"
                ShowFooter="false" ShowHeader="true" EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>

                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <%--                        <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Medical" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblMedicalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWFLAGMEDICALID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMedicalName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMedicalLockYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLOCKYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date" HeaderStyle-Width="145px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date" HeaderStyle-Width="145px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag" HeaderStyle-Width="130px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated Date" HeaderStyle-Width="145px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDON")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated By" HeaderStyle-Width="140px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Doctor Name" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoctorName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCTORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked Date" HeaderStyle-Width="145px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked by" HeaderStyle-Width="145px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Verificatiom Method" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerificationMethod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVRIMETHODNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="true" HeaderText="Seafarer" UniqueName="ISADDEDBYSEAFARER" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblisbyseafarer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISADDEDBYSEAFARER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="Edit" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="MEDICALDELETE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdDelete" ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="MEDICALATTACHMENT" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdAtt" ToolTip="Attachment"></asp:ImageButton>
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                    CommandName="MEDICALARCHIVE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdArchive" ToolTip="Archive"></asp:ImageButton>
                                <img id="Img8" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Unlock" ImageUrl="<%$ PhoenixTheme:images/unlock.png %>"
                                    CommandName="UNLOCKMEDICAL" Visible="false" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdUnlock" ToolTip="Unlock"></asp:ImageButton>
                                <img id="Img9" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Authentication" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    CommandName="AUTHENTICATIONMEDICAL" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdAuthenticate" ToolTip="Authentication"></asp:ImageButton>
                                <img id="Img10" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="CrossCheck" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                    CommandName="CROSSCHECKMEDICAL" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdCrossCheck" ToolTip="Cross Check"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdSave" ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdCancel" ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="MEDICALADD" CommandArgument='<%# Container.DataItem %>' ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                                <%-- <asp:LinkButton runat="server" AlternateText="Save" CommandName="MEDICALADD" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>--%>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <br />
            <b>
                <asp:Literal ID="lblMedicalTest" runat="server" Text="Medical Test"></asp:Literal></b>
            <br />
            <eluc:TabStrip ID="CrewMedicalTest" runat="server" OnTabStripCommand="CrewMedicalTest_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvMedicalTest" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemDataBound="gvMedicalTest_ItemDataBound" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvMedicalTest_NeedDataSource" OnItemCommand="gvMedicalTest_ItemCommand"               
                ShowFooter="true" ShowHeader="true"
                EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>

                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <%--                        <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />--%>
                        <telerik:GridTemplateColumn>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Medical Test" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblMedicalTestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWMEDICALTESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMedicalTestName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFMEDICAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblMedicalTestIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWMEDICALTESTID") %>'></telerik:RadLabel>
                                <eluc:Documents runat="server" ID="ucMedicalTestEdit" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" DocumentList='<%# PhoenixRegistersDocumentMedical.ListDocumentMedical() %>'
                                    DocumentType="MEDICAL" SelectedDocument='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALTESTID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Documents runat="server" ID="ucMedicalTestAdd" CssClass="dropdown_mandatory" Width="100%"
                                    AppendDataBoundItems="true" DocumentList='<%# PhoenixRegistersDocumentMedical.ListDocumentMedical() %>'
                                    DocumentType="MEDICAL" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueAdd" runat="server" CssClass="gridinput" MaxLength="200" Width="100%"
                                    ToolTip="Enter Place of issue">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date" HeaderStyle-Width="145px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date" HeaderStyle-Width="145px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateAdd" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard runat="server" ID="ddlStatusEdit" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnTextChangedEvent="Status_OnTextChangedEvent" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard runat="server" ID="ddlStatusAdd" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    HardTypeCode="105" AutoPostBack="true" HardList='<%# PhoenixRegistersHard.ListHard(SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105) %>'
                                    OnTextChangedEvent="Status_OnTextChangedEvent" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="220px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarksEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRemarksAdd" runat="server" CssClass="gridinput" MaxLength="200" Width="100%"
                                    ToolTip="Enter remarks">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated Date" HeaderStyle-Width="145px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDON")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated By" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Doctor Name" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDoctorName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCTORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked Date" HeaderStyle-Width="145px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked by" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Verificatiom Method" Visible="false" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerificationMethod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVRIMETHODNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAuthenticationRequYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblMedicalTestLockYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLOCKYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucVerificationMethodEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="131"></eluc:Quick>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucVerificationMethodAdd" runat="server" CssClass="input" AppendDataBoundItems="true" Width="100%"
                                    QuickTypeCode="131"></eluc:Quick>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="true" HeaderText="Seafarer" UniqueName="ISADDEDBYSEAFARER" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblisbyseafarer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISADDEDBYSEAFARER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdEdit" ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="MEDICALTESTDELETE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdDelete" ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="MEDICALTESTATTACHMENT" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdAtt" ToolTip="Attachment"></asp:ImageButton>
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                    CommandName="MEDICALTESTARCHIVE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdArchive" ToolTip="Archive"></asp:ImageButton>
                                <img id="Img8" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Unlock" ImageUrl="<%$ PhoenixTheme:images/unlock.png %>"
                                    CommandName="UNLOCKMEDICALTEST" Visible="false" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdUnlock" ToolTip="Unlock"></asp:ImageButton>
                                <img id="Img9" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Authentication" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    CommandName="AUTHENTICATIONMEDICALTEST" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdAuthenticate" ToolTip="Authentication"></asp:ImageButton>
                                <img id="Img10" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="CrossCheck" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                    CommandName="CROSSCHECKMEDICALTEST" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdCrossCheck" ToolTip="Cross Check"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="UPDATE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdSave" ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdCancel" ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <%-- <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="MEDICALTESTADD" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdAdd" ToolTip="Add New"></asp:ImageButton>--%>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="MEDICALTESTADD" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--            </div>--%>
            <br />
            <br />
            <b>
                <telerik:RadLabel ID="lblCourses" runat="server" Text="Courses"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="MenuCrewCourseCertificate" runat="server" OnTabStripCommand="CrewCourseCertificate_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCourseCertificate" runat="server" AutoGenerateColumns="False" EnableHeaderContextMenu="true"
                Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvCrewCourseCertificate_ItemCommand" OnNeedDataSource="gvCrewCourseCertificate_NeedDataSource"
                OnItemDataBound="gvCrewCourseCertificate_ItemDataBound"
                AllowCustomPaging="true" AllowPaging="true"               
                ShowFooter="true" ShowHeader="true" EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <%--                        <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECOURSEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblCourseIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECOURSEID") %>'></telerik:RadLabel>
                                <eluc:Course ID="ddlCourseEdit" runat="server" CourseList="<%# PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse() %>"
                                    ListCBTCourse="false" SelectedCourse='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" AutoPostBack="true"
                                    OnTextChangedEvent="ddlDocument_TextChanged" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Course ID="ddlCourseAdd" runat="server" CourseList="<%#PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse()%>"
                                    ListCBTCourse="false" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="100%"
                                    AutoPostBack="true" OnTextChangedEvent="ddlDocument_TextChanged" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="90px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="90px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Certificate Number" FooterText="Number" HeaderStyle-Width="85px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNumber" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkCourseNumber" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItem %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCourseNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="30">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCourseNumberAdd" runat="server" CssClass="gridinput_mandatory" Width="100%"
                                    MaxLength="30" ToolTip="">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue" HeaderStyle-Width="82px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueAdd" runat="server" CssClass="input_mandatory" MaxLength="200" Width="100%"
                                    ToolTip="Enter Place of issue">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateEdit" CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateAdd" CssClass="input" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Institution" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Address runat="server" ID="ucInstitutionEdit" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                    SelectedAddress='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Address runat="server" ID="ucInstitutionAdd" CssClass="input_mandatory" AddressType="138" Width="100%"
                                    AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>' AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDNATIONALITY").ToString()%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Nationality ID="ddlFlagEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    NationalityList='<%#PhoenixRegistersCountry.ListNationality() %>' SelectedNationality='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYCODE").ToString()%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Nationality ID="ddlFlagAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="100%"
                                    NationalityList='<%#PhoenixRegistersCountry.ListNationality() %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issuing Authority" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDAUTHORITY")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtAuthorityEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORITY")%>'
                                    MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtAuthorityAdd" runat="server" CssClass="gridinput" MaxLength="100" Width="100%"
                                    ToolTip="Enter Authority">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="85px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <img id="imgRemarks" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/te_view.png %>"
                                    onmousedown="javascript:closeMoreInformation()" />
                            </ItemTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRemarksAdd" runat="server" Width="100%" CssClass="gridinput"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated Date" HeaderStyle-Width="115px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDON")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated By" HeaderStyle-Width="115px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked Date" HeaderStyle-Width="105px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked by" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Verification Method" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerificationMethod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVRIMETHODNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAuthenticationRequYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseLockYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLOCKYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucVerificationMethodEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="131"></eluc:Quick>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucVerificationMethodAdd" runat="server" CssClass="input" AppendDataBoundItems="true" Width="100%"
                                    QuickTypeCode="131"></eluc:Quick>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Seafarer" UniqueName="ISADDEDBYSEAFARER" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblisbyseafarer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISADDEDBYSEAFARER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="CDELETE" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="CAttachment" CommandArgument='<%# Container.DataItem %>' ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                    CommandName="CArchive" CommandArgument='<%# Container.DataItem %>' ID="cmdArchive"
                                    ToolTip="Archive"></asp:ImageButton>
                                <img id="Img8" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Unlock" ImageUrl="<%$PhoenixTheme:images/unlock.png %>"
                                    CommandName="UNLOCKCOURSE" Visible="false" CommandArgument='<%#Container.DataItem %>'
                                    ID="cmdUnlock" ToolTip="Unlock"></asp:ImageButton>
                                <img id="Img9" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Authentication" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    CommandName="AUTHENTICATIONCOURSE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdAuthenticate" ToolTip="Authentication"></asp:ImageButton>
                                <img id="Img10" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="CrossCheck" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                    CommandName="CROSSCHECKCOURSE" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdCrossCheck" ToolTip="Cross Check"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItem %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItem %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <%--  <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="CAdd" CommandArgument='<%# Container.DataItem %>' ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>--%>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="CAdd" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--            </div>--%>

            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img4" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="* Documents Expired"></telerik:RadLabel>

                    </td>
                    <td>
                        <img id="Img5" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="* Documents Expiring in 120 Days"></telerik:RadLabel>

                    </td>
                </tr>
            </table>
            <br />
            <br />
            <b>
                <telerik:RadLabel ID="lblOtherDocuments" runat="server" Text="Other Documents"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="MenuCrewOtherDocument" runat="server" OnTabStripCommand="CrewOtherDocumentMenu_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOtherDocument" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvOtherDocument_ItemCommand" OnItemDataBound="gvOtherDocument_ItemDataBound"
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvOtherDocument_NeedDataSource"              
                ShowFooter="True"
                EnableViewState="false" AllowSorting="true" OnSortCommand="gvOtherDocument_SortCommand">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <%--                    <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document Name" HeaderStyle-Width="150px" SortExpression="FLDDOCUMENTNAME" AllowSorting="true">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentTypename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDocumentTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                <eluc:DocumentType ID="ucDocumentTypeEdit" runat="server" DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString())%>'
                                    SelectedDocumentType='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'
                                    CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType1_TextChanged"
                                    AutoPostBack="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:DocumentType ID="ucDocumentTypeAdd" runat="server" DocumentType='<%# ((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString()%>'
                                    DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString())%>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType1_TextChanged"
                                    AutoPostBack="true" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME").ToString()%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FooterText="Number" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTNUMBER">Number&nbsp;</asp:LinkButton>
                                <img id="FLDDOCUMENTNUMBER" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltraveldocumentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblNumber" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkNumber" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItem %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lbltraveldocumentidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="txtNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="30">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtNumberAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="30" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Issue" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateOfIssue" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDateOfIssueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>'
                                    CssClass="dropdown_mandatory" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucDateOfIssueAdd" runat="server" CssClass="dropdown_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date" AllowSorting="true" SortExpression="FLDDATEOFEXPIRY" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDateExpiryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>'
                                    CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucDateExpiryAdd" runat="server" CssClass="dropdown_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfissue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'
                                    CssClass="gridinput" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceIssueAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issuing Authority" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtIssuingAuthorityEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITY") %>'
                                    MaxLength="200" ToolTip="Enter Issuing Authority">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtIssuingAuthorityAdd" runat="server" CssClass="gridinput" MaxLength="200"
                                    ToolTip="Enter Issuing Authority" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <img id="imgRemarks" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/te_view.png %>"
                                    onmousedown="javascript:closeMoreInformation()" />
                            </ItemTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated Date" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDON")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Authenticated By" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuthenticatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked Date" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cross checked by" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerifiedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Verificatiom Method" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerificationMethod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVRIMETHODNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAuthenticationRequYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAUTHENTICATIONREQYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblOtherLockYN" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLOCKYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Quick ID="ucVerificationMethodEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="131"></eluc:Quick>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucVerificationMethodAdd" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="131"></eluc:Quick>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Seafarer" UniqueName="ISADDEDBYSEAFARER" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblisbyseafarer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISADDEDBYSEAFARER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img alt="" src="../css/Theme1/images/spacer.png" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="Attachment" CommandArgument='<%# Container.DataItem %>' ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                    CommandName="Archive" CommandArgument='<%# Container.DataItem %>' ID="cmdArchive"
                                    ToolTip="Archive"></asp:ImageButton>
                                <img id="Img8" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Unlock" ImageUrl="<%$ PhoenixTheme:images/unlock.png %>"
                                    CommandName="UNLOCKOTHERDOC" Visible="false" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdUnlock" ToolTip="Unlock"></asp:ImageButton>
                                <img id="Img9" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Authentication" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    CommandName="AUTHENTICATIONOTHERDOC" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdAuthenticate" ToolTip="Authentication"></asp:ImageButton>
                                <img id="Img10" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="CrossCheck" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                    CommandName="CROSSCHECKOTHERDOC" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdCrossCheck" ToolTip="Cross Check"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                    CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img alt="" src="../css/Theme1/images/spacer.png" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>

                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight=""  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img6" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblddd" Text="* Documents Expired" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <img id="Img7" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" Text="* Documents Expiring in 120 Days" runat="server"></telerik:RadLabel>

                    </td>
                </tr>
            </table>

            <telerik:RadLabel ID="Literal1" runat="server" Font-Bold="true" Text="Drug & Alcohol Test"></telerik:RadLabel>
            <eluc:TabStrip ID="MenuDAT" runat="server" OnTabStripCommand="MenuDAT_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAlcoholTest" runat="server" Height="37%" EnableViewState="true"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvAlcoholTest_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvAlcoholTest_ItemDataBound"
                OnItemCommand="gvAlcoholTest_ItemCommand" ShowFooter="false"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Test Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTestDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDALCOHOLTESTDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Inspector" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Company" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                      
                        <telerik:GridTemplateColumn HeaderText="Result" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblResult" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESULT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSample" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSAMPLETAKEN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAtt" ToolTip="Attachment" CommandName="DATATTACHMENT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
