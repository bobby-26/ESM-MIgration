<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantCourseAndCertificate.aspx.cs"
    Inherits="CrewNewApplicantCourseAndCertificate" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="../UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Course And Certificate</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
    <form id="frmNewApplicantCourseCertificate" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewCourse" runat="server" Title="Course"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%" cellpadding="1" cellspacing="1">
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
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <hr />
            <telerik:RadLabel ID="lblnote" runat="server" Text="Note:To send mail,please select the courses,whose certificates needs to be attached in the mail">
            </telerik:RadLabel>
            &nbsp;
                <br />
            <b>Courses</b>
            <eluc:TabStrip ID="MenuCrewCourseCertificate" runat="server" OnTabStripCommand="CrewCourseCertificate_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCourseCertificate" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCrewCourseCertificate_ItemCommand" OnNeedDataSource="gvCrewCourseCertificate_NeedDataSource"
                EnableViewState="false" OnItemDataBound="gvCrewCourseCertificate_ItemDataBound" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkChekedEmail" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course" HeaderStyle-Width="150px">
                            <%--   <HeaderTemplate>
                                    <telerik:RadLabel ID="lblCourseHeader" runat="server">Course &nbsp;                                        
                                    </telerik:RadLabel>
                                </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECOURSEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCourseIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECOURSEID") %>'></telerik:RadLabel>
                                <eluc:Course ID="ddlCourseEdit" runat="server" ListCBTCourse="false" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    AutoPostBack="true" OnTextChangedEvent="ddlDocument_TextChanged" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Course ID="ddlCourseAdd" runat="server" CourseList='<%# PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse()%>' ListCBTCourse="false"
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" AutoPostBack="true" OnTextChangedEvent="ddlDocument_TextChanged" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Certificate Number" HeaderStyle-Width="90px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <%-- <HeaderTemplate>
                                    <telerik:RadLabel ID="lblCourseNumberHeader" runat="server">Certificate Number&nbsp;
                                    </telerik:RadLabel>
                                </HeaderTemplate>--%>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCourseNumber" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCourseNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCourseNumberAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200" ToolTip="">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Issue" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtPlaceOfIssueAdd" runat="server" CssClass="input_mandatory"
                                    MaxLength="200" ToolTip="Enter Place of issue">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date" HeaderStyle-Width="150px">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblIssueDateHeader" runat="server">Issue Date&nbsp;</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateEdit" CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtExpiryDateAdd" CssClass="input" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Institution" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Address runat="server" ID="ucInstitutionEdit" CssClass="input_mandatory" AppendDataBoundItems="true" AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                    SelectedAddress='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Address runat="server" ID="ucInstitutionAdd" CssClass="input_mandatory" AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                    AppendDataBoundItems="true" />
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
                                <eluc:Nationality ID="ddlFlagAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    NationalityList='<%#PhoenixRegistersCountry.ListNationality() %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issuing Authority" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDAUTHORITY")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtAuthorityEdit" runat="server" CssClass="input"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORITY")%>' MaxLength="100">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtAuthorityAdd" runat="server" CssClass="gridinput"
                                    MaxLength="100" ToolTip="Enter Authority">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Verified" Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerified" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? "Yes" : "No"%>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVerifiedby" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDBY").ToString()%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipCertificates" runat="server" Text='<%# "Verified By : " + DataBinder.Eval(Container,"DataItem.FLDVERIFIEDBY") +"<br/> Verified On :  " + General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDVERIFIEDDATE" ))%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkVerifiedEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? true : false%>'
                                    Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDYN").ToString() == "1" ? false : true%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkVerifiedAdd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="Remarks"
                                    CommandName="REMARKS" CommandArgument="<%# Container.DataSetIndex %>" ID="imgRemarks"
                                    ToolTip="Remarks">
                                <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                                <%--<img ID="imgRemarks" runat="server" style="cursor:hand" src="<%$ PhoenixTheme:images/te_view.png %>"  onmousedown="javascript:closeMoreInformation()" />--%>
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRemarksAdd" runat="server" CssClass="gridinput"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="COURSESDELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="COURSESATTACHMENT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAtt"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Archive"
                                    CommandName="COURSESARCHIVE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdArchive"
                                    ToolTip="Archive" Width="20PX" Height="20PX">
                              <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                    CommandName="COURSESADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                    ToolTip="Add New" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img4" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                    </td>
                    <td>* Documents Expired
                    </td>
                    <td>
                        <img id="Img5" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                    </td>
                    <td>* Documents Expiring in 120 Days
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
