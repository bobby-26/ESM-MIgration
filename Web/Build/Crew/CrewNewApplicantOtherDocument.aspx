<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantOtherDocument.aspx.cs"
    Inherits="CrewNewApplicantOtherDocument" %>

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
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:TabStrip ID="MenuOtherDocument" TabStrip="true" runat="server" OnTabStripCommand="MenuOtherDocument_TabStripCommand"></eluc:TabStrip>

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
                        <td>
                            <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                </table>
                <hr />

                <eluc:TabStrip ID="MenuCrewNewApplicantTravelDocument" runat="server" OnTabStripCommand="CrewNewApplicantTravelDocumentMenu_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvOtherDocument" runat="server"  AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvOtherDocument_NeedDataSource" Height="70%"
                    OnSortCommand="gvOtherDocument_SortCommand"
                    OnItemCommand="gvOtherDocument_ItemCommand"
                    OnItemDataBound="gvOtherDocument_ItemDataBound"
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
                            <telerik:GridTemplateColumn>
                                <HeaderStyle Width="30px" />
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
                                    <eluc:DocumentType ID="ucDocumentTypeEdit" runat="server"
                                        CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged"
                                        AutoPostBack="true" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:DocumentType ID="ucDocumentTypeAdd" runat="server" Pool='<%#SouthNests.Phoenix.Framework.General.GetNullableInteger(ViewState["POOLID"] + "") %>' DocumentType='<%# ((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString()%>'
                                        DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString(), 0, SouthNests.Phoenix.Framework.General.GetNullableInteger(ViewState["POOLID"] + ""))%>'
                                        AppendDataBoundItems="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged"
                                        AutoPostBack="true" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Number">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbltraveldocumentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkNumber" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lbltraveldocumentidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox Width="100%" ID="txtNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'
                                        CssClass="gridinput_mandatory" MaxLength="30">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox Width="100%" ID="txtNumberAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="30"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Date of Issue">
                                <HeaderStyle Width="120px" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblDateofIssueHeader" runat="server">Date of Issue</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDateOfIssue" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date Width="100%" ID="ucDateOfIssueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>'
                                        CssClass="dropdown_mandatory" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date Width="100%" ID="ucDateOfIssueAdd" runat="server" CssClass="dropdown_mandatory" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Expiry Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date Width="100%" ID="ucDateExpiryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>'
                                        CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date Width="100%" ID="ucDateExpiryAdd" runat="server" CssClass="dropdown_mandatory" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Place of Issue">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPlaceOfissue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox Width="100%" ID="txtPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'
                                        CssClass="gridinput" MaxLength="200">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox Width="100%" ID="txtPlaceIssueAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200">
                                    </telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Issuing Authority">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox Width="100%" ID="txtIssuingAuthorityEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITY") %>'
                                        MaxLength="200" ToolTip="Enter Issuing Authority">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox Width="100%" ID="txtIssuingAuthorityAdd" runat="server" CssClass="gridinput" MaxLength="200"
                                        ToolTip="Enter Issuing Authority">
                                    </telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks">
                                <HeaderStyle Width="75px" />
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <asp:LinkButton runat="server" AlternateText="Remarks"
                                        CommandName="REMARKS" CommandArgument="<%# Container.DataSetIndex %>" ID="imgRemarks"
                                        ToolTip="Remarks">
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Attachment"
                                        CommandName="Attachment" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAtt"
                                        ToolTip="Attachment" Width="20PX" Height="20PX">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Archive"
                                        CommandName="Archive" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdArchive"
                                        ToolTip="Archive" Width="20PX" Height="20PX">
                              <span class="icon"><i class="fas fa-archive"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Add"
                                        CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                        ToolTip="Add New" Width="20PX" Height="20PX">
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  />
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
                            <telerik:RadLabel ID="lblDocumentsExpiringin120Days" runat="server" Text=" * Documents Expiring in 120 Days"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
