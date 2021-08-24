<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerEmailTemplate.aspx.cs"
    Inherits="DefectTracker_DefectTrackerEmailTemplate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Module" Src="~/UserControls/UserControlSEPModuleList.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EmailTemplate</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <div class="subHeader">
        <div id="div1" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucManageTemplate" Text="ManageTemplate"></eluc:Title>
        </div>
        <div style="position: absolute; top: 0px; right: 0px">
        </div>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlEmailTemplate">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <br />
            <table width="75%">
                <tr>
                    <td>
                        <asp:Literal ID="lblSubject" runat="server">Subject</asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubjectSearch" Width="360px" CssClass="input" runat="server">
                        </asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblModule" runat="server">Module</asp:Literal>
                    </td>
                    <td>
                        <eluc:Module ID="ucModuleSearch" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
            <br />
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="AddEmailTemplate" OnTabStripCommand="MenuEmailTemplate_TabStripCommand"
                    runat="server"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0">
                <asp:GridView ID="gvEmailTemplate" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    OnRowCreated="EmailTemplate_RowCreated" Width="100%" CellPadding="3" OnRowDataBound="EmailTemplate_ItemDataBound"
                    OnRowCommand="EmailTemplate_RowCommand" OnRowDeleting="EmailTemplate_RowDeleting"
                    EnableViewState="False">
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" Wrap="False" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:TemplateField HeaderText="From">
                            <ItemStyle HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblIncidentcodeheader" runat="server">Incident Code</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblIncidentId" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDINCIDENTTEMPLATEID").ToString() %>'></asp:Label>
                                <asp:Label ID="lblIncidentCode" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINCIDENTCODE").ToString() %>'> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblModuleNameHeader" runat="server">Module Name</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblModulename" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDMODULENAME").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblDescriptionHeader" runat="server">Description</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Response">
                            <ItemStyle HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblEmailBodyHeader" runat="server">Email Body</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBody" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBODY").ToString().Length > 60 ? DataBinder.Eval(Container, "DataItem.FLDBODY").ToString().Substring(0, 60) + "..." : DataBinder.Eval(Container, "DataItem.FLDBODY").ToString()%>'>></asp:Label>
                                <eluc:Tooltip ID="uclblBody" Width="50%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBODY") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module">
                            <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblSubjectHeader" runat="server">Subject</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDSUBJECT").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                                Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" runat="server" ToolTip=' <%#DataBinder.Eval(Container,"DataItem.FLDFILENAMES").ToString() %>'
                                    Height="14px">
                                </asp:HyperLink>
                                <asp:Label ID="lblFileName" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDFILENAMES").ToString() %>'
                                    Visible="false"></asp:Label>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
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
                    <eluc:Status runat="server" ID="ucStatus" />
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
