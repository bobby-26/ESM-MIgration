<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewFamilyOtherDocument.aspx.cs" Inherits="CrewFamilyOtherDocument" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ECNR" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentType" Src="~/UserControls/UserControlDocumentType.ascx" %>

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
        <eluc:TabStrip ID="CrewFamilyTabs" runat="server" OnTabStripCommand="CrewFamilyTabs_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="CrewFamilyotherDoc" runat="server" OnTabStripCommand="CrewFamilyotherDoc_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <eluc:TabStrip ID="MenuCrewNewApplicantTravelDocument" runat="server" OnTabStripCommand="CrewNewApplicantTravelDocumentMenu_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvOtherDocument" runat="server" Height="80%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvOtherDocument_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvOtherDocument_ItemDataBound"
                OnItemCommand="gvOtherDocument_ItemCommand" OnUpdateCommand="gvOtherDocument_UpdateCommand" ShowFooter="True" OnDeleteCommand="gvOtherDocument_DeleteCommand"
                OnSortCommand="gvOtherDocument_SortCommand" AutoGenerateColumns="false">
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
                        <telerik:GridButtonColumn Text="Select" CommandName="Select" Visible="false" />
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="3%" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="DocumentNameHeader" HeaderText="Document Name" AllowSorting="true" DataField="FLDDOCUMENTNAME" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDocumentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocumentTypename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDocumentTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                                <eluc:DocumentType ID="ucDocumentTypeEdit" runat="server" DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString(),1)%>'
                                    SelectedDocumentType='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>' Width="100%" Nokyn="true"
                                    CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged"
                                    AutoPostBack="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:DocumentType ID="ucDocumentTypeAdd" runat="server" Nokyn="true" DocumentType='<%# ((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString()%>'
                                    DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString(),1)%>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged" AutoPostBack="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" AllowSorting="true" DataField="FLDDOCUMENTNUMBER" ShowSortIcon="true">
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
                        <telerik:GridTemplateColumn HeaderText="Issued" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateofIssue" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="ucDateOfIssueEdit" CssClass="input_mandatory"
                                    Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE")) %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="ucDateOfIssueAdd" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry" AllowSorting="true" DataField="FLDDATEOFEXPIRY" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblHavingExpiryEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY") %>'></telerik:RadLabel>
                                <eluc:Date ID="ucDateExpiryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>'
                                    CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY").ToString() == "1" ? ("input_mandatory") : ("") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblHavingExpiryAdd" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY") %>'></telerik:RadLabel>
                                <eluc:Date ID="ucDateExpiryAdd" runat="server" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceofIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>' Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceIssueAdd" runat="server" CssClass="gridinput_mandatory" Width="100%"
                                    MaxLength="200">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issuing Authority" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuingAuthority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtIssuingAuthorityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITY") %>' Width="100%" MaxLength="200" ToolTip="Enter Issuing Authority"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtIssuingAuthorityAdd" runat="server" Width="100%" MaxLength="200"
                                    ToolTip="Enter Issuing Authority">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="5%" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="Remarks" ID="imgRemarks"
                                    ToolTip="Remarks">
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
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
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDocumentsExpired" runat="server" Text='* Documents Expired"'></telerik:RadLabel>
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDocumentsExpiringin120Days" runat="server" Text='* Documents Expiring in 120 Days'></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
