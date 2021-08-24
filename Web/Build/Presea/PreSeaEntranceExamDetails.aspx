<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaEntranceExamDetails.aspx.cs"
    Inherits="PreSeaEntranceExamDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Venue" Src="~/UserControls/UserControlPreSeaExamVenue.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaQualificaiton" Src="~/UserControls/UserControlPreSeaQualification.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Date Of Availabilty</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewDateOfAvailability" runat="server">
          <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader" style="position: relative">
                <div id="divHeading" style="vertical-align: top">
                    <eluc:Title runat="server" ID="ucTitle" Text="Interview" ShowMenu="false" />
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand"></eluc:TabStrip>
            </div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:UpdatePanel runat="server" ID="pnlDOA">
                <ContentTemplate>
                    <br style="clear: both;" />
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>First Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td>Middle Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtMiddleName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td>Last Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Course
                            </td>
                            <td>
                                <asp:TextBox ID="txtCourse" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td>Batch
                            </td>
                            <td>
                                <asp:TextBox ID="txtBatch" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td>Interview Venue
                            </td>
                            <td>
                                <eluc:Venue ID="ucExamVenue" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="150px" />                                
                            </td>
                        </tr>
                        <tr>
                            <td>Exam Date
                            </td>
                            <td>
                                <eluc:Date ID="ucExamdate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>Interviewed By
                            </td>
                            <td>
                                <asp:TextBox ID="txtInterviewBy" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                            </td>
                            <td>Entrance Roll No
                            </td>
                            <td>
                                <asp:TextBox ID="txtRollNo" runat="server" Width="150px" ToolTip="Entrance Roll No"
                                    CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>                       
                        <%--<tr>
                            <td colspan="6">
                                <b>Verify Certificates(Mandatory only for joining course base on eligibility)</b>
                            </td>
                        </tr>--%>
                    </table>
                    <br />

                    <table>
                          <tr style="color: Blue; font-size: small; font-weight: 100;">
                            <td valign="middle">Note :
                            </td>
                            <td colspan="6">Verify Certificates(Mandatory only for joining course based on eligibility)
                         
                        </tr>
                    </table>
                    

                    <asp:GridView ID="gvcertificateverify" runat="server" AutoGenerateColumns="False" Font-Size="11px" ShowFooter="true"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvcertificateverify_RowDataBound"
                        OnRowCommand="gvcertificateverify_RowCommand" OnRowCancelingEdit="gvcertificateverify_RowCancelingEdit" OnRowEditing="gvcertificateverify_RowEditing"
                        OnRowUpdating="gvcertificateverify_RowUpdating" OnRowDeleting="gvcertificateverify_RowDeleting" DataKeyNames="FLDCANDITATECERTIFICATEID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <Columns>

                            <asp:TemplateField HeaderText="Certificate">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblmarksheetHeader" runat="server" Text="Certificate"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCertificate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDQUALIFICATION")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:PreSeaQualificaiton ID="ddlCertificateEdit" runat="server" CssClass="input_mandatory" SelectedQualification='<%# DataBinder.Eval(Container, "DataItem.FLDCERTIFICATE")%>'
                                        AppendDataBoundItems="true" Width="200" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:PreSeaQualificaiton ID="ddlCertificateInsert" runat="server" CssClass="input_mandatory" SelectedQualification='<%# DataBinder.Eval(Container, "DataItem.FLDCERTIFICATE")%>'
                                        AppendDataBoundItems="true" Width="200" />
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Verified">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblverifiedHeader" runat="server" Text="Verified"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVerified" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFICATIONSTATUS")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlverifiedEdit" runat="server" CssClass="input_mandatory">
                                        <asp:ListItem Text="Verified" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Later" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="NA" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlverifiedInsert" runat="server" CssClass="input_mandatory">
                                        <asp:ListItem Text="Verified" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Later" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="NA" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Verified By">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVerifiedByHeader" runat="server" Text="Verified By"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVerifiedBy" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDBY")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="lblVerifiedByEdit" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVERIFIEDBY") %>' runat="server"
                                        Width="150px" CssClass="readonlytextbox"></asp:TextBox>
                                </EditItemTemplate>
                                <%--<FooterTemplate>
                                        <asp:TextBox ID="txtFacultyRoleInsert" runat="server" Width="150px" CssClass="input_mandatory"></asp:TextBox>
                                    </FooterTemplate>--%>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Remarks">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblremarksHeader" runat="server" Text="Remarks"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblremarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" CssClass="input" ID="txtRemarksEdit" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" CssClass="input" ID="txtRemarksInsert"></asp:TextBox>
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
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <img id="Img5" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png%>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <br style="clear: both;" />
                    <h2 id="h1default" runat="server" style="color: Red; text-align: center;">Score Card template is not Configured for this batch.</h2>
                    <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 1000px; width: 100%"></iframe>
                    <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                    <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnSendMail_Click" OKText="Yes"
                        CancelText="No" Visible="false" />
                    <eluc:Confirm ID="ucConfirmDelete" runat="server" OnConfirmMesage="btnConfirmDelete_Click"
                        OKText="Yes" CancelText="No" Visible="false" />
                    </div>
                </ContentTemplate>
                 <Triggers>
                <asp:PostBackTrigger ControlID="gvcertificateverify" />
            </Triggers>
            </asp:UpdatePanel>
            
    </form>
</body>
</html>
