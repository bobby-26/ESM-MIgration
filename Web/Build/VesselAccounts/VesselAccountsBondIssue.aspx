<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsBondIssue.aspx.cs" Inherits="VesselAccountsBondIssue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Vessel Sign-On</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="Issue of Phone Card" ShowMenu="false"></eluc:Title>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenPick_Click" />
                    </div>
                    <table width="75%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblOnAccountFor" runat="server" Text="lblOn Account For"></asp:Literal>
                            </td>
                            <td colspan="5">
                                <eluc:VesselCrew ID="ddlEmployee" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    AppendOwnerCharterer="true" AutoPostBack="true" OnTextChangedEvent="ddlEmployee_TextChangedEvent" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblIssueType" runat="server" Text="Issue Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ddlHard" runat="server" CssClass="input_mandatory" HardTypeCode="97"
                                    ShortNameFilter="BND" AutoPostBack="true" OnTextChangedEvent="ddlEmployee_TextChangedEvent"
                                    Enabled="false" />
                            </td>
                            <td>
                                <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                            </td>
                            <td>
                                <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="MenuBondIssue" runat="server" OnTabStripCommand="MenuBondIssue_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvCrewSearch_RowDataBound" OnRowEditing="gvCrewSearch_RowEditing"
                            OnRowCancelingEdit="gvCrewSearch_RowCancelingEdit" OnRowUpdating="gvCrewSearch_RowUpdating"
                            OnRowDeleting="gvCrewSearch_RowDeleting" ShowHeader="true" ShowFooter="true"
                            EnableViewState="false" DataKeyNames="FLDISSUEITEMID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="Item Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDSTORENAME"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDUNITNAME"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="ROB">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDROB"] %>
                                </ItemTemplate>                              
                            </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Sold Quantity">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDQUANTITY"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblRobEdit" Visible="false" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDROB"] %>'></asp:Label>
                                        <asp:Label ID="lblOldQty" Visible="false" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDQUANTITY"]%>'></asp:Label>
                                        <eluc:Number ID="txtQuantityEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                            Text='<%#((DataRowView)Container.DataItem)["FLDQUANTITY"] %>' IsInteger="true"
                                            IsPositive="true" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit Price">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDUNITPRICE"]%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Literal ID="lblTotal" runat="server" Text="Total"></asp:Literal>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDAMOUNT"]%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <%#string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDISSUEDATE"])%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="txtDateEdit" runat="server" CssClass="input_mandatory" Text='<%#((DataRowView)Container.DataItem)["FLDISSUEDATE"] %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%#((DataRowView)Container.DataItem)["FLDREMARKS"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRemarksEdit" runat="server" CssClass="gridinput" Text='<%#((DataRowView)Container.DataItem)["FLDREMARKS"] %>'></asp:TextBox>
                                    </EditItemTemplate>
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
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
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
        </asp:UpdatePanel>
    </form>
</body>
</html>
