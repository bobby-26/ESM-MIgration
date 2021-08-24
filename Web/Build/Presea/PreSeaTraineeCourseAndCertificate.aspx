<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaTraineeCourseAndCertificate.aspx.cs" Inherits="PreSeaTraineeCourseAndCertificate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="../UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course And Certificate</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaTraineeCourseCertificate" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaTraineeCourseCertificateEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" id="divHeading">
                        <span style="float: left">
                            <eluc:Title runat="server" ID="ucTitle" Text="Crew Course" ShowMenu="false" />
                        </span>
                        <img src="<%$ PhoenixTheme:images/attachment.png %>" id="imgClip" runat="server"
                            style="padding-top: 3px" />
                        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewCourse" runat="server"></eluc:TabStrip>
                </div>
                <div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                First Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                Middle Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                Last Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                Employee Number
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>                       
                    </table>
                </div>
                <hr />
                <asp:Label ID="lblnote" runat="server" Text="Note:To send mail,please select the courses,whose certificates needs to be attached in the mail"
                    CssClass="guideline_text" ></asp:Label>
                &nbsp;
                <br />
                <b>Courses</b>
                <div style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaTraineeCourseCertificate" runat="server" OnTabStripCommand="PreSeaTraineeCourseCertificate_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvTraineeCourseCertificate" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvTraineeCourseCertificate_RowCommand"
                        OnRowDataBound="gvTraineeCourseCertificate_RowDataBound" OnRowDeleting="gvTraineeCourseCertificate_RowDeleting"  OnSorting="gvTraineeCourseCertificate_Sorting"
                        OnRowCancelingEdit="gvTraineeCourseCertificate_RowCancelingEdit" OnSelectedIndexChanging="gvTraineeCourseCertificate_SelectedIndexChanging"
                        OnRowEditing="gvTraineeCourseCertificate_RowEditing" OnRowUpdating="gvTraineeCourseCertificate_RowUpdating"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnRowCreated="gvTraineeCourseCertificate_RowCreated">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkChekedEmail" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                     <asp:LinkButton ID="lnkCourseType" runat="server" CommandName="Sort" CommandArgument="FLDCOURSETYPE"
                                        ForeColor="White">Type&nbsp;</asp:LinkButton>
                                    <img id="FLDCOURSETYPE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCourseHeader" runat="server">Course &nbsp;                                        
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                    <asp:Label ID="lblTraineeCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEECOURSEID") %>'></asp:Label>
                                    <asp:Label ID="lbldocid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></asp:Label>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:Label ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></asp:Label>
                                    <asp:Label ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                        CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                    <asp:Label ID="lblTraineeCourseIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAINEECOURSEID") %>'></asp:Label>
                                    <eluc:Course ID="ddlCourseEdit" runat="server" CourseList="<%# PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse() %>"
                                        ListCBTCourse="false" SelectedCourse='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'
                                        AppendDataBoundItems="true" CssClass="dropdown_mandatory" AutoPostBack="true"
                                        OnTextChangedEvent="ddlDocument_TextChanged" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Course ID="ddlCourseAdd" runat="server" CourseList="<%#PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse()%>"
                                        ListCBTCourse="false" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        AutoPostBack="true" OnTextChangedEvent="ddlDocument_TextChanged" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCourseNumberHeader" runat="server">Certificate Number&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCourseNumber" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCourseNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENUMBER") %>'
                                        CssClass="gridinput_mandatory" MaxLength="30"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtCourseNumberAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="30" ToolTip=""></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                          
                            <asp:TemplateField HeaderText="Issue Date">
                                <HeaderTemplate>
                                 <asp:LinkButton ID="lnkIssueDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDDATEOFISSUE"
                                        ForeColor="White">Issued&nbsp;</asp:LinkButton>
                                    <img id="FLDDATEOFISSUE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Expiry
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date runat="server" ID="txtExpiryDateEdit" CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date runat="server" ID="txtExpiryDateAdd" CssClass="input" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Institution
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Address runat="server" ID="ucInstitutionEdit" CssClass="input_mandatory" AppendDataBoundItems="true"
                                        AddressType="138" AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>'
                                        SelectedAddress='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Address runat="server" ID="ucInstitutionAdd" CssClass="input_mandatory" AddressType="138"
                                        AddressList='<%# PhoenixRegistersAddress.ListAddress("138") %>' AppendDataBoundItems="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Place
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPlaceOfIssueEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'>
                                    </asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPlaceOfIssueAdd" runat="server" CssClass="input_mandatory" MaxLength="200"
                                        ToolTip="Enter Place of issue"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Country">
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
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issuing Authority">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDAUTHORITY")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAuthorityEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUTHORITY")%>'
                                        MaxLength="100">
                                    </asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtAuthorityAdd" runat="server" CssClass="gridinput" MaxLength="100"
                                        ToolTip="Enter Authority"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                        Visible="false"></asp:Label>
                                    <img id="imgRemarks" runat="server" style="cursor: hand" src="<%$ PhoenixTheme:images/te_view.png %>"
                                        onmousedown="javascript:closeMoreInformation()" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtRemarksAdd" runat="server" CssClass="gridinput"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                        Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="CEDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CDELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="CAttachment" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Attachment"></asp:ImageButton>
                                    <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                        CommandName="CArchive" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdArchive"
                                        ToolTip="Archive"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="CUpdate" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CCancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="CAdd" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img4" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                        </td>
                        <td>
                            * Documents Expired
                        </td>
                        <td>
                            <img id="Img5" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                        </td>
                        <td>
                            * Documents Expiring in 120 Days
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
