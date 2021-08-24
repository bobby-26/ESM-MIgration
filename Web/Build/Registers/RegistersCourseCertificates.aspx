<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCourseCertificates.aspx.cs"
    Inherits="RegistersCourseCertificates" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Certificate" Src="~/UserControls/UserControlCertificate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course Certificates</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="certificateslink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersCourseCertificates" runat="server" autocomplete="off" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCourseCertificatesEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Status runat="server" ID="ucStatus" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblCourseCertificates" runat="server" Text="Course Certificates"></asp:Literal>
                    </div>
                </div>
                <div id="divFind" style="position: relative;">
                    <table id="tblConfigureCourseCertificates" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Course ID="ucCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Enabled="false" />
                            </td>
                            <td>
                                <asp:Literal ID="lblCourseType" runat="server" Text="Course Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard runat="server" ID="ucCourseType" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                    Enabled="false" HardTypeCode="103" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="position: relative;">
                    <eluc:TabStrip ID="MenuRegistersCourseCertificates" runat="server" OnTabStripCommand="RegistersCourseCertificates_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1;">
                    <asp:GridView ID="gvCourseCertificates" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCourseCertificates_RowCommand"
                        OnRowDataBound="gvCourseCertificates_ItemDataBound" OnRowCancelingEdit="gvCourseCertificates_RowCancelingEdit"
                        OnRowDeleting="gvCourseCertificates_RowDeleting" OnRowEditing="gvCourseCertificates_RowEditing"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" OnSorting="gvCourseCertificates_Sorting"
                        AllowSorting="true" OnRowCreated="gvCourseCertificates_RowCreated" OnSelectedIndexChanging="gvCourseCertificates_SelectedIndexChanging"
                        OnRowUpdating="gvCourseCertificates_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Certificate">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCertificateNameHeader" runat="server">Certificate
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:Label ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></asp:Label>
                                    <asp:Label ID="lblCertificateId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSECERTIFICATEID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkCertificateName" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATE") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCourseCertificateIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSECERTIFICATEID") %>'></asp:Label>
                                  
                                    <asp:DropDownList ID="ucCertificateEdit" runat="server" DataSource='<%# PhoenixRegistersCourseCertificate.ListCourseCertificate(null) %>'
                                        AppendDataBoundItems="true" CssClass="dropdown_mandatory" DataTextField="FLDNAME" DataValueField="FLDCERTIFICATEID" >
                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                 
                                    <asp:DropDownList ID="ucCertificateAdd" runat="server" AppendDataBoundItems="true"
                                        CssClass="dropdown_mandatory">
                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblCertificateNumberHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDCERTIFICATENO" ForeColor="White">Number&nbsp;</asp:LinkButton>
                                    <img id="FLDCERTIFICATENO" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCertificateNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCertificateNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENO") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtCertificateNumberAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="100" ToolTip="Enter Certificate Number"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Issue Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblDateOfIssueHeader" runat="server" CommandName="Sort" CommandArgument="FLDDATEOFISSUE"
                                        ForeColor="White">Issue Date&nbsp;</asp:LinkButton>
                                    <img id="FLDDATEOFISSUE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date runat="server" ID="txtDateOfIssueEdit" CssClass="gridinput_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date runat="server" ID="txtDateOfIssueAdd" CssClass="gridinput_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Expiry Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblDateOfExpiryHeader" runat="server" CommandName="Sort" CommandArgument="FLDDATEOFEXPIRY"
                                        ForeColor="White">Expiry Date&nbsp;</asp:LinkButton>
                                    <img id="FLDDATEOFEXPIRY" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfExpiry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date runat="server" ID="txtDateOfExpiryEdit" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date runat="server" ID="txtDateOfExpiryAdd" CssClass="input" />
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
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="Attachment" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Attachment"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
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
                            <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="DocumentsExpired" runat="server" Text="* Documents Expired"></asp:Literal>
                        </td>
                        <td>
                            <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="DocumentsExpiringin120Days" runat="server" Text="* Documents Expiring in 120 Days"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
