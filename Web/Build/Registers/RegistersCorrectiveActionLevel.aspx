<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersCorrectiveActionLevel.aspx.cs"
    Inherits="RegistersCorrectiveActionLevel" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Corrective Action Level</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>


</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersMaritalStatus" autocomplete="off" runat="server">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlMaritalStatusEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="Corrective Action Level" />
                        </div>
                    </div>

                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuRegistersMaritalStatus" runat="server" OnTabStripCommand="RegistersMaritalStatus_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0">
                        <asp:GridView ID="gvMaritalStatus" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvMaritalStatus_RowCommand" OnRowDataBound="gvMaritalStatus_ItemDataBound"
                            OnRowCancelingEdit="gvMaritalStatus_RowCancelingEdit" OnRowDeleting="gvMaritalStatus_RowDeleting"
                            OnRowEditing="gvMaritalStatus_RowEditing" ShowFooter="true" ShowHeader="true"
                            OnSorting="gvMaritalStatus_Sorting" AllowSorting="true" OnRowCreated="gvMaritalStatus_RowCreated"
                            OnSelectedIndexChanging="gvMaritalStatus_SelectedIndexChanging" OnRowUpdating="gvMaritalStatus_RowUpdating">

                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <asp:TemplateField FooterText="New MaritalStatus" FooterStyle-Width="20%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbllvelcode" runat="server" Text='Level'></asp:Label>
                                        <%-- <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDSHORTNAME"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                            CommandArgument="1" />
                                    <asp:LinkButton ID="lblMaritalStatusCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDSHORTNAME" ForeColor="White">Code&nbsp;</asp:LinkButton>
                                    <img id="FLDSHORTNAME" runat="server" visible="false" />--%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMaritalStatusCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCLEVELID") %>'></asp:Label>
                                        <asp:Label ID="lbllvelcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCLEVELCODE") %>'></asp:Label>
                                        <%--<asp:LinkButton ID="lnkMaritalStatusName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></asp:LinkButton>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblMaritalStatusCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCLEVELID") %>'></asp:Label>
                                        <asp:TextBox ID="txtMaritalStatusNameEdit" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="ajxtxtMaritalStatusNameEdit" runat="server" TargetControlID="txtMaritalStatusNameEdit"
                                            Mask="999" MaskType="Number" InputDirection="LeftToRight" AutoComplete="false">
                                        </ajaxToolkit:MaskedEditExtender>

                                        <%--           <asp:TextBox ID="txtMaritalStatusNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCLEVELCODE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>--%>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtMaritalStatusNameAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="ajxMaritalStatusNameAdd" runat="server" TargetControlID="txtMaritalStatusNameAdd"
                                            Mask="999" MaskType="Number" InputDirection="LeftToRight" AutoComplete="false">
                                        </ajaxToolkit:MaskedEditExtender>
                                        <%--<asp:TextBox ID="txtMaritalStatusNameAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="4" ToolTip="Enter Level Code"></asp:TextBox>--%>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblDescriptionHeader" runat="server" CommandName="Sort" CommandArgument="FLDNCLEVELNAME" ForeColor="White">Description&nbsp;</asp:LinkButton>
                                        <img id="FLDNCLEVELNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCLEVELNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCLEVELNAME") %>'
                                            CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtDescriptionAdd" runat="server" CssClass="gridinput_mandatory"
                                            MaxLength="200"></asp:TextBox>
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
                                        <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="UPDATECHEKLICT" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                                            CommandName="RESEND" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdResend"
                                            ToolTip="Resend to vessel"></asp:ImageButton>
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
                    <div id="divPage" style="position: relative;">
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
                                <td width="20px">&nbsp;
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
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="MenuRegistersMaritalStatus" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
