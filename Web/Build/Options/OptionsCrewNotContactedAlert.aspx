<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsCrewNotContactedAlert.aspx.cs" Inherits="OptionsCrewNotContactedAlert" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Not Contacted Alert</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmNotContactedAlert" runat="server">
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>                
                <table width="80%">
                    <tr>
                        <td>
                            <font color="blue"><b><asp:Literal ID="lblNoteOnlyseafarersinPersonnelmasterreflectinthisalert" runat="server" Text="Note: Only seafarers in Personnel master reflect in this alert."></asp:Literal></b></font>
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                            <asp:GridView ID="gvAlertsTask" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowCommand="gvAlertsTask_RowCommand" OnRowDataBound="gvAlertsTask_ItemDataBound"
                                EnableViewState="false" AllowSorting="true" OnSorting="gvAlertsTask_Sorting">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                                ForeColor="White">Name &nbsp;</asp:LinkButton>
                                            <img id="FLDNAME" runat="server" visible="false" />                                            
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaskKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKKEY") %>'></asp:Label>
                                            <asp:Label ID="lblTaskType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>
                                            <asp:Label ID="lblExpression" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPRESSION") %>'></asp:Label>
                                            <asp:LinkButton ID="lblDescriptionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                                CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblFileNo" runat="server" Text="File No"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblZone" runat="server" Text="Zone"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblZone" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANK"
                                                ForeColor="White">Rank &nbsp;</asp:LinkButton>
                                            <img id="FLDRANK" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescriptionRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                   
                                    <asp:TemplateField HeaderText="Last Contacted Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblLastContactDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDLASTCONTACTDATE"
                                                ForeColor="White">Last Contacted Date &nbsp;</asp:LinkButton>
                                            <img id="FLDLASTCONTACTDATE" runat="server" visible="false" />                                            
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastContactDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTCONTACTDATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblViewDateHeader" runat="server">Viewed Date&nbsp;
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVIEWDATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View By">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblViewByHeader" runat="server">Viewed By&nbsp;
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVIEWBY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvAlertsTask" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
