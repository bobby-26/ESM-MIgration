<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseAndCertificate.aspx.cs"
    Inherits="CrewCourseAndCertificate" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="../UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Course And Certificate</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewCourseCertificate.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCourseCertificate" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewCourse" runat="server" Title="Course"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <b>
                <telerik:RadLabel ID="lblnote" runat="server" Text="Note: To send mail, please select the courses, whose certificates needs to be attached in the mail"></telerik:RadLabel>
            </b>
            <br />
            <eluc:TabStrip ID="MenuCrewCourseCertificate" runat="server" OnTabStripCommand="CrewCourseCertificate_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCourseCertificate" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCrewCourseCertificate_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvCrewCourseCertificate_ItemDataBound"
                OnItemCommand="gvCrewCourseCertificate_ItemCommand" OnUpdateCommand="gvCrewCourseCertificate_UpdateCommand" ShowFooter="True" OnDeleteCommand="gvCrewCourseCertificate_DeleteCommand"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"  AllowSorting="false" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" Width="100%">
                    
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="42px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="42px">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkChekedEmail" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="107px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course" HeaderStyle-Width="230px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECOURSEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCourseIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECOURSEID") %>'></telerik:RadLabel>
                                <eluc:Course ID="ddlCourseEdit" runat="server" CourseList="<%# PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse() %>"
                                    ListCBTCourse="false" SelectedCourse='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" AutoPostBack="true" Width="100%"
                                    OnTextChangedEvent="ddlDocument_TextChanged" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Course ID="ddlCourseAdd" runat="server" CourseList="<%#PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse()%>"
                                    ListCBTCourse="false" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="100%"
                                    AutoPostBack="true" OnTextChangedEvent="ddlDocument_TextChanged" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="230px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCourseNumber" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCourseNumberEdit" runat="server" CssClass="gridinput_mandatory" MaxLength="200" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCourseNumberAdd" runat="server" CssClass="gridinput_mandatory" Width="100%"
                                    MaxLength="200">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueEdit" runat="server" CssClass="input_mandatory" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueAdd" runat="server" MaxLength="200" CssClass="input_mandatory" Width="100%"
                                    ToolTip="Enter Place of issue">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Width="100%"
                                    Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE")) %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date" HeaderStyle-Width="150px" FooterStyle-Width="150px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateEdit" Width="100%" CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRY").ToString() == "1" ? ("input_mandatory") : ("") %>'
                                    Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY")) %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateAdd" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Institution" HeaderStyle-Width="240px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Address runat="server" ID="ucInstitutionEdit" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>' Width="100%"
                                    SelectedAddress='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Address runat="server" ID="ucInstitutionAdd" CssClass="input_mandatory" AddressType="138" Width="100%"
                                    AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>' AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nationality" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Nationality ID="ddlFlagEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    NationalityList='<%#PhoenixRegistersCountry.ListNationality() %>' SelectedNationality='<%# DataBinder.Eval(Container, "DataItem.FLDCOUNTRYCODE").ToString()%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Nationality ID="ddlFlagAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    NationalityList='<%#PhoenixRegistersCountry.ListNationality() %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issuing Authority" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssuingAuthority" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDAUTHORITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtAuthorityEdit" runat="server" Width="100%"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORITY")%>' MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtAuthorityAdd" runat="server" Width="100%"
                                    MaxLength="200" ToolTip="Enter Issuing Authority">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="72px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="Remarks"
                                    CommandName="INSTRUCTION" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAddInstruction"
                                    ToolTip="Add Remarks">
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="120px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAtt" ToolTip="Attachment" CommandName="CAttachment" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Archive" ID="cmdArchive" CommandName="CArchive" ToolTip="Archive" Width="20px" Height="20px">
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
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="CAdd" ToolTip="Add New" Width="20px" Height="20px">
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
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" EnableNextPrevFrozenColumns="true"/>
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
                        <telerik:RadLabel ID="lblDocumentsExpiringin120Days" runat="server" Text="* Documents Expiring in 120 Days"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
