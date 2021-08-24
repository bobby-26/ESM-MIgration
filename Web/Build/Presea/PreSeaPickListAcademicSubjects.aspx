<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaPickListAcademicSubjects.aspx.cs"
    Inherits="PreSeaPickListAcademicSubjects" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaQualificaiton" Src="~/UserControls/UserControlPreSeaQualification.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="fromPreseaPickListAcademic" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucTitle" Text="Academic subject" ShowMenu="false" />
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuAddress" runat="server" OnTabStripCommand="MenuAddress_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="search">
                <table cellpadding="1" cellspacing="1" width="80%">
                    <tr>
                        <td>
                            Qualification
                        </td>
                        <td>
                            <eluc:PreSeaQualificaiton ID="ddlCertificate" runat="server" CssClass="input" AutoPostBack="true"
                                AppendDataBoundItems="true"></eluc:PreSeaQualificaiton>
                        </td>
                        <td>
                            Subject
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" CssClass="input" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divGrid" style="position: relative;">
                <asp:GridView ID="gvPreseaPicklist" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Font-Size="11px" OnRowCommand="gvPreseaPicklist_RowCommand" OnRowDataBound="gvPreseaPicklist_RowDataBound"
                    OnSorting="gvPreseaPicklist_Sorting" DataKeyNames="FLDSUBJECTID" AllowSorting="true"
                    ShowFooter="False" ShowHeader="true" Width="100%" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkQualifyHeader" runat="server" CommandName="Sort" CommandArgument="FLDQUALIFICATION"
                                    ForeColor="White">Qualification&nbsp;</asp:LinkButton>
                                <img id="FLDQUALIFICATION" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblQualify" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICATION") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                Subject
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSubjectId" Text='<%# Bind("FLDSUBJECTID") %>' Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkSubjectName" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divPage" style="position: relative;">
                <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="gvPreseaPicklist" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
