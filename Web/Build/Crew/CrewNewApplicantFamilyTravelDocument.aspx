<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantFamilyTravelDocument.aspx.cs" Inherits="CrewNewApplicantTravelDocument" %>

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
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTravelDocument" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
            <eluc:TabStrip ID="CrewFamilyMedical" runat="server" OnTabStripCommand="CrewFamilyMedical_TabStripCommand" Title="Family NOK / Travel Document"></eluc:TabStrip>
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
                    <td colspan="6">
                        <hr />
                        <b>
                            <telerik:RadLabel ID="Radlabel1" runat="server" Text="Passport Details"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassportNo" runat="server" Text="Passport No"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassportnumber" runat="server" MaxLength="15" CssClass="input upperCase"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssue" runat="server" Text="Issued"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfIssue" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text="Expiry"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfExpiry" runat="server" CssClass="input" />
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClip" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblECNR" runat="server" Text="ECNR"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:ECNR ID="ucECNR" runat="server" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" CssClass="input" MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMinimum4BlankPages" runat="server" Text="Minimum 4 Blank Pages"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:ECNR ID="ucBlankPages" runat="server" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                </tr>
            </table>
            <br />
            <b>
                <telerik:RadLabel ID="Documents" runat="server" Text="Documents"></telerik:RadLabel>
            </b>
            <br />
            <div>
                <eluc:TabStrip ID="MenuCrewNewApplicantTravelDocument" runat="server" OnTabStripCommand="CrewNewApplicantTravelDocumentMenu_TabStripCommand"></eluc:TabStrip>
            </div>
            <telerik:RadGrid ID="gvTravelDocument" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true" ShowHeader="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvTravelDocument_NeedDataSource" OnItemDataBound="gvTravelDocument_ItemDataBound" OnItemCommand="gvTravelDocument_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle Width="120px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentTypename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDocumentTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                <eluc:DocumentType ID="ucDocumentTypeEdit" runat="server"
                                    CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged" AutoPostBack="true" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:DocumentType ID="ucDocumentTypeAdd" runat="server" DocumentType='<%# ((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString()%>' DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString())%>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged" AutoPostBack="true" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number">
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
                        <telerik:GridTemplateColumn HeaderText="Issued on">
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
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblValidFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALIDFROM","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucValidFromEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALIDFROM") %>'
                                    CssClass="input" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucValidFromAdd" runat="server" CssClass="input" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDateExpiryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>'
                                    CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucDateExpiryAdd" runat="server" CssClass="dropdown_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No of entries">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoofentry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFENTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlNoofentryEdit" runat="server" AppendDataBoundItems="true" Width="100%" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="1" Text="Single" />
                                        <telerik:RadComboBoxItem Value="2" Text="Multiple" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlNoofentryAdd" runat="server" AppendDataBoundItems="true" Width="100%" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="1" Text="Single" />
                                        <telerik:RadComboBoxItem Value="2" Text="Multiple" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfissue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'
                                    CssClass="gridinput" MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceIssueAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality/Flag">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYNAME").ToString()%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Country ID="ddlCountryEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="100%"
                                    CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' SelectedCountry='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYCODE").ToString()%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Country ID="ddlCountryAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="100%" CountryList='<%#PhoenixRegistersCountry.ListCountry(null) %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="Remarks" ID="imgRemarks"
                                    ToolTip="Remarks">
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
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
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="radlabel2" runat="server" Text="* Documents Expired"></telerik:RadLabel>
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="* Documents Expiring in 120 Days"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
