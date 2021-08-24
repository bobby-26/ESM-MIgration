<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsCrewLicenceAlert.aspx.cs"
    Inherits="OptionsCrewLicenceAlert" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Licence Alert</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmLicenceAlert" runat="server">
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <br clear="all" />
                <table width="80%" style="overflow:scroll">
                    <tr>
                        <td>
                            <font color="blue"><asp:Literal ID="NoteThisalertshowsonboardemployeedocumentswhichareexpiredaftertheirsignondateandgoingtoexpirein45days" runat="server" Text="Note: This alert shows onboard employee documents which are expired (after their sign on date)and going to expire in 45 days."></asp:Literal> </font>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvAlertsTask" runat="server" AutoGenerateColumns="False" Font-Size="10.5px"
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
                                            <%--<asp:LinkButton ID="lblNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                                ForeColor="White">Name &nbsp;</asp:LinkButton>
                                            <img id="FLDNAME" runat="server" visible="false" />--%>
                                            <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaskKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKKEY") %>'></asp:Label>
                                            <asp:Label ID="lblTaskType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>
                                            <asp:Label ID="lblExpression" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPRESSION") %>'></asp:Label>
                                            <asp:Label ID="lblDocType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                            <asp:LinkButton ID="lblDescriptionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                                CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANKNAME"
                                                ForeColor="White">Rank &nbsp;</asp:LinkButton>
                                            <img id="FLDRANKNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescriptionRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"
                                                ForeColor="White">Vessel &nbsp;</asp:LinkButton>
                                            <img id="FLDVESSELNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblLicenceHeader" runat="server" CommandName="Sort" CommandArgument="FLDLICENCE"
                                                ForeColor="White">Licence &nbsp;</asp:LinkButton>
                                            <img id="FLDLICENCE" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLicence" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblExpiryDate" runat="server" Text="Expiry Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                           <asp:Literal ID="lblFlag" runat="server" Text="Flag"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAG") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblViewByHeader" runat="server">View By<br/>Date
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVIEWBY").ToString()+"<br/>"+ DataBinder.Eval(Container,"DataItem.FLDVIEWDATE").ToString() %>'></asp:Label>
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
