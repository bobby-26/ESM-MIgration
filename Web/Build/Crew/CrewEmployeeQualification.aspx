<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewEmployeeQualification.aspx.cs"
    Inherits="CrewEmployeeQualification" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">    
    <title>Academic Qualification</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">     
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewEmployeeQualification" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlEmployeeQualificationEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader" style="position: relative">
                <div id="divHeading">
                    <eluc:Title runat="server" ID="ucTitle" Text="Academic Qualification" />
                </div>
            </div>
            <div id="divFind" style="position: relative; z-index: 2">
                <table id="tblEmployeeQualification" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblEmployeeNo" runat="server" Text="Employee No"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeNo" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRank" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSurname" runat="server" Text="Surname"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurname" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblFirstname" runat="server" Text="Firstname"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstname" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMiddlename" runat="server" Text="Middlename"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMiddlename" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="navSelect" style="position: relative; width: 115px">
                <eluc:TabStrip ID="MenuCrewEmployeeQualification" runat="server" OnTabStripCommand="CrewEmployeeQualification_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <asp:GridView ID="gdEmployeeQualification" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gdEmployeeQualification_RowCommand"
                    OnRowDataBound="gdEmployeeQualification_ItemDataBound" OnRowCancelingEdit="gdEmployeeQualification_RowCancelingEdit"
                    OnRowDeleting="gdEmployeeQualification_RowDeleting" OnRowEditing="gdEmployeeQualification_RowEditing"
                    ShowFooter="True" Style="margin-bottom: 0px" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:TemplateField HeaderText="Institution Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblInstitutionNameHeader" runat="server">Institute&nbsp;<asp:ImageButton
                                    runat="server" ID="cmdInstitutionNameDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>"
                                    OnClick="cmdSort_Click" CommandName="FLDINSTITUTIONNAME" CommandArgument="1" />
                                    <asp:ImageButton runat="server" ID="cmdInstitutionNameAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                        OnClick="cmdSort_Click" CommandName="FLDINSTITUTIONNAME" CommandArgument="0" />
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblQualificationid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICATIONID") %>'></asp:Label>
                                <asp:LinkButton ID="txtInstitutionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME") %>'
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" MaxLength="100"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblQualificationidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICATIONID") %>'></asp:Label>
                                <asp:TextBox ID="txtInstitutionNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="100px"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInstitutionNameAdd" runat="server" CssClass="gridinput_mandatory"
                                    Width="100px" MaxLength="100"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField FooterText="Place">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblPlaceHeader" runat="server">Place&nbsp;<asp:ImageButton runat="server"
                                    ID="cmdPlaceDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click" CommandName="FLDPLACE"
                                    CommandArgument="1" AlternateText="Place desc" />
                                    <asp:ImageButton runat="server" ID="cmdPlaceAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                        OnClick="cmdSort_Click" CommandName="FLDPLACE" CommandArgument="0" />
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPlace" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACE") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPlaceEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACE") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200" Width="100px"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtPlaceAdd" runat="server" CssClass="gridinput_mandatory" Width="100px"
                                    MaxLength="200"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblCertificateHeader" runat="server">
                                    Certificate
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCertificate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATE") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCertificateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATE") %>'
                                    CssClass="gridinput" MaxLength="200" Width="100px"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtCertificateAdd" runat="server" CssClass="input_mandatory" MaxLength="200"
                                    Width="100px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Percent">
                            <HeaderTemplate>
                                <asp:Literal ID="lblPercent" runat="server" Text="Percent"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPercent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENT") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                               <eluc:Number ID="txtPercentEdit" runat="server" CssClass="input txtNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENT") %>'
                                   MaxLength="5"  />
                            </EditItemTemplate>
                            <FooterTemplate>
                            <eluc:Number ID="txtPercentAdd" runat="server" CssClass="input txtNumber"  MaxLength="5"  />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grade">
                            <HeaderTemplate>
                                <asp:Literal ID="lblGrade" runat="server" Text="Grade"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblGrade" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRADE") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtGradeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRADE") %>'
                                    CssClass="gridinput" MaxLength="5" Width="40px"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtGradeAdd" runat="server" CssClass="input" MaxLength="5" Width="40px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From">
                            <HeaderTemplate>
                                <asp:Literal ID="lblFrom" runat="server" Text="From"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROM") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFromEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROM") %>'
                                    CssClass="gridinput" MaxLength="200" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="extcalFromEdit" runat="server" TargetControlID="txtFromEdit" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtFromAdd" runat="server" CssClass="input" MaxLength="200" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="extcalFrom" runat="server" TargetControlID="txtFromAdd" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To">
                            <HeaderTemplate>
                                <asp:Literal ID="lblTo" runat="server" Text="To"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTO") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtToEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTO") %>'
                                    CssClass="gridinput" MaxLength="200" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="extcalToEdit" runat="server" TargetControlID="txtToEdit" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtToAdd" runat="server" CssClass="input" MaxLength="200" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="extcalTo" runat="server" TargetControlID="txtToAdd" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Duration">
                            <HeaderTemplate>
                                <asp:Literal ID="lblDuration" runat="server" Text="Duration"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDurationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'
                                    ReadOnly="true" CssClass="gridinput" MaxLength="200" Width="60px"></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDurationAdd" runat="server" CssClass="input" MaxLength="200"
                                    ReadOnly="true" Width="60px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PassedDate">
                            <HeaderTemplate>
                                <asp:Literal ID="lblPassedDate" runat="server" Text="PassedDate"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPassedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSEDDATE") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPassedDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSEDDATE") %>'
                                    CssClass="gridinput" MaxLength="200" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="extcalPassedDateEdit" runat="server" TargetControlID="txtPassedDateEdit" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtPassedDateAdd" runat="server" CssClass="input" MaxLength="200"
                                    Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="extcalPassedDate" runat="server" TargetControlID="txtPassedDateAdd" />
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
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divPage" style="position: relative; z-index: -1">
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
