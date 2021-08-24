<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaTraineeAcademicQualification.aspx.cs" Inherits="PreSeaTraineeAcademicQualification" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Qualificaiton" Src="~/UserControls/UserControlQualification.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PreSea Academics</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

 </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaTraineeAcademic" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaTraineeAcademicEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader">
                    <eluc:Title runat="server" ID="Academic" Text="Trainee Academic Qualification"
                        ShowMenu="false"></eluc:Title>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
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
                            Batch
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtBatch" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <b>Academic Details</b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaAcademic" runat="server" OnTabStripCommand="PreSeaAcademicMenu_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvPreSeaAcademics" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvPreSeaAcademics_RowDataBound"
                        OnRowDeleting="gvPreSeaAcademics_RowDeleting" OnRowEditing="gvPreSeaAcademics_RowEditing"
                        Style="margin-bottom: 0px" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAcademicsHeader" runat="server">Qualification</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAcademicsId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACADEMICID") %>'></asp:Label>
                                    <asp:Label ID="lblAcademictype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACADEMICTYPE") %>'></asp:Label>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:Label ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></asp:Label>
                                    <asp:LinkButton ID="lblAcademicsname" runat="server" CommandArgument='<%#Container.DataItemIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICATIONNAME") %>' CommandName="EDIT"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Place of Institution">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Place of Institution
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPlaceofInstitution" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Percentage">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    Percentage
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPercentage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    Passed Year
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPassYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAROFPASSING") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    Action
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="Attachment" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Attachment"></asp:ImageButton>
                                </ItemTemplate>
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
                <br />
                </table>
                <br />
                &nbsp;<b>Award and Certificate</b>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="PreSeaAwardandCertificate" runat="server" OnTabStripCommand="AwardandCertificateMenu_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divAward" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvAwardAndCertificate" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvAwardAndCertificate_RowDataBound"
                        OnRowDeleting="gvAwardAndCertificate_RowDeleting" OnRowEditing="gvAwardAndCertificate_RowEditing"
                        OnRowCancelingEdit="gvAwardAndCertificate_RowCancelingEdit" OnRowCommand="gvAwardAndCertificate_RowCommand"
                        ShowFooter="true" OnRowUpdating="gvAwardAndCertificate_RowUpdating" Style="margin-bottom: 0px"
                        EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="From Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    Sl no
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSlNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Award/Certificate
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAwardId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWARDID") %>'></asp:Label>
                                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                    <asp:Label ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></asp:Label>
                                    <asp:LinkButton ID="lblCertificate" runat="server" CommandArgument='<%#Container.DataItemIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>' CommandName="EDIT"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlCertificateEdit" runat="server" CssClass="input_mandatory"
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Long Service Award"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Cultural"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Sports"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Technical"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlCertificateAdd" runat="server" CssClass="input_mandatory"
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Long Service Award"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Cultural"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Sports"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Technical"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Issue Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDISSUEDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblAwardIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWARDID") %>'></asp:Label>
                                    <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Remarks
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRemarksEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                        MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtRemarksAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
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
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdXEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdXDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdXAtt"
                                        ToolTip="Attachment"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="div3" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumberCert" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesCert" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsCert" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousCert" runat="server" OnCommand="PagerButtonClickCert"
                                    CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNextCert" OnCommand="PagerButtonClickCert" runat="server"
                                    CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopageCert" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoCert" runat="server" Text="Go" OnClick="cmdGoCert_Click" CssClass="input"
                                    Width="40px"></asp:Button>
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
