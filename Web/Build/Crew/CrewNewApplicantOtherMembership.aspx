<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantOtherMembership.aspx.cs"
    Inherits="CrewNewApplicantOtherMembership" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
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
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTravelDocument" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlFamilyNok">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader">
                    <eluc:Title runat="server" ID="TravelDocument" Text="CBA/Other Membership" ShowMenu="false">
                    </eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuOtherMembership" TabStrip="true" runat="server" OnTabStripCommand="MenuOtherMembership_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:literal ID="lblFirstName" runat="server" Text="First Name"></asp:literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                        <td>
                             <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeMiddleName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeLastName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAppliedRank" runat="server" Text="Applied Rank"></asp:Literal>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewNewApplicantTravelDocument" runat="server" OnTabStripCommand="CrewNewApplicantTravelDocumentMenu_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvOtherDocument" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvOtherDocument_RowCommand" OnRowDataBound="gvOtherDocument_RowDataBound"
                    OnRowCancelingEdit="gvOtherDocument_RowCancelingEdit" OnRowDeleting="gvOtherDocument_RowDeleting"
                    OnRowUpdating="gvOtherDocument_RowUpdating" OnRowEditing="gvOtherDocument_RowEditing"
                    OnSelectedIndexChanging="gvOtherDocument_SelectedIndexChanging" ShowFooter="True"
                    EnableViewState="false" AllowSorting="true" OnSorting="gvOtherDocument_Sorting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblDocumentTypeHeader" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTNAME"
                                    ForeColor="White">Document Name&nbsp;</asp:LinkButton>
                                <img id="FLDDOCUMENTNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDocumentTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></asp:Label>
                                <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                <asp:Label ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></asp:Label>
                                <asp:Label ID="lblDocumentTypename" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblDocumentTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></asp:Label>
                                <eluc:DocumentType ID="ucDocumentTypeEdit" runat="server" DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString())%>'
                                    SelectedDocumentType='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'
                                    CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged"
                                    AutoPostBack="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:DocumentType ID="ucDocumentTypeAdd" runat="server" DocumentType='<%# ((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString()%>'
                                    DocumentTypeList='<%#PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString())%>'
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" OnTextChangedEvent="ddlDocumentType_TextChanged"
                                    AutoPostBack="true" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField FooterText="Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTNUMBER"
                                    ForeColor="White">Number&nbsp;</asp:LinkButton>
                                <img id="FLDDOCUMENTNUMBER" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbltraveldocumentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></asp:Label>
                                <asp:LinkButton ID="lnkNumber" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbltraveldocumentidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDOCUMENTID") %>'></asp:Label>
                                <asp:TextBox ID="txtNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNUMBER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="30"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtNumberAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="30"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date of Issue">
                            <HeaderTemplate>
                                <asp:Label ID="lblDateofIssueHeader" runat="server">Date of Issue</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDateOfIssue" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDateOfIssueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>'
                                    CssClass="dropdown_mandatory" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucDateOfIssueAdd" runat="server" CssClass="dropdown_mandatory" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblExpiryDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDDATEOFEXPIRY"
                                    ForeColor="White"> Expiry Date&nbsp;</asp:LinkButton>
                                <img id="FLDDATEOFEXPIRY" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDateofExpiry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDateExpiryEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>'
                                    CssClass='<%# DataBinder.Eval(Container,"DataItem.FLDHAVINGEXPIRY").ToString() == "1" ? ("input_mandatory") : ("input") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucDateExpiryAdd" runat="server" CssClass="dropdown_mandatory" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                               <asp:Label ID="lblPlaceofIssueHeader" runat="server"> Place of Issue</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPlaceOfissue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'
                                    CssClass="gridinput" MaxLength="200">
                                </asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtPlaceIssueAdd" runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="200"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Issuing Authority">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITY") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtIssuingAuthorityEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUINGAUTHORITY") %>'
                                    MaxLength="200" ToolTip="Enter Issuing Authority"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtIssuingAuthorityAdd" runat="server" CssClass="gridinput" MaxLength="200"
                                    ToolTip="Enter Issuing Authority"></asp:TextBox>
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
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl='<%$ PhoenixTheme:images/te_edit.png%>'
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img alt="" src="../css/Theme1/images/spacer.png" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="Attachment" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>
                                <img id="Img4" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Archive" ImageUrl="<%$ PhoenixTheme:images/archive.png %>"
                                    CommandName="Archive" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdArchive"
                                    ToolTip="Archive"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                    CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img alt="" src="../css/Theme1/images/spacer.png" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/te_check.png%>'
                                    CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <table width="100%" border="0" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap="nowrap" align="center">
                            <asp:Label ID="lblPagenumber" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblPages" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblRecords" runat="server">
                            </asp:Label>&nbsp;&nbsp;
                        </td>
                        <td nowrap="nowrap" align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">
                            &nbsp;
                        </td>
                        <td nowrap="nowrap" align="right" width="50px">
                            <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                        </td>
                        <td nowrap="nowrap" align="center">
                            <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                </table>
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                        </td>
                        <td>
                           <asp:Literal ID="lblDocumentExpired" runat="server" Text=" * Documents Expired"></asp:Literal>
                        </td>
                        <td>
                            <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDocumentExpiringin120Days" runat="server" Text="* Documents Expiring in 120 Days"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
