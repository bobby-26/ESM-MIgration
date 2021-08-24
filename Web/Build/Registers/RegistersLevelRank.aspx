<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersLevelRank.aspx.cs" Inherits="RegistersLevelRank" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RankList" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rank</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersRank" runat="server">
    <ajaxtoolkit:toolkitscriptmanager combinescripts="false" id="ToolkitScriptManager1"
        runat="server">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlRankEntry">
        <ContentTemplate>
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:title runat="server" id="ucTitle" text="Level Rank" ShowMenu="false"></eluc:title>
                    </div>
                </div>
                <%--<div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuOwnerMapping" runat="server" OnTabStripCommand="OwnerMapping_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                </div>--%>
                <table>
                    <tr>
                        <td><asp:Literal ID="lblPurpose" runat="server" Text="Purpose"></asp:Literal>
                        </td>
                        <td>
                        <eluc:Hard ID="ucHard" CssClass="input" runat="server" AutoPostBack="true" AppendDataBoundItems="true" HardTypeCode="230" />
                        </td>
                    </tr>
                </table> 
                <asp:GridView ID="gvLVB" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvLVB_RowDataBound"
                    OnRowUpdating="gvLVB_RowUpdating" EnableViewState="false"
                    OnRowCancelingEdit="gvLVB_RowCancelingEdit" DataKeyNames="FLDDTKEY"
                    OnRowEditing="gvLVB_RowEditing" ShowHeader="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>                       
                        <asp:TemplateField HeaderText="Level">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                           
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDLEVELNAME") %>
                                <asp:Label ID="lblLevelId" runat="server" Visible=false Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>' ></asp:Label>
                            </ItemTemplate>                                                     
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                           
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRANKLISTNAME").ToString().TrimEnd(',') %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div runat="server" id="dvRank" class="input_mandatory" style="overflow: auto; width: 95%;
                                    height: 80px">
                                    <asp:CheckBoxList runat="server" ID="cblRank" Height="100%" RepeatColumns="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" DataTextField="FLDGROUPRANK" DataValueField="FLDGROUPRANKID" DataSource='<%#PhoenixRegistersLevelRankList.ListGroupRank() %>'>
                                    </asp:CheckBoxList>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Purpose">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                           
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPURPOSENAME")%>
                                <asp:Label ID="lblPurposeId" runat="server" Visible=false Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>' ></asp:Label>
                            </ItemTemplate>                                                     
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%"></HeaderStyle>
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
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>                            
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
