<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCostEvaluationAirfareAdd.aspx.cs"
    Inherits="CrewCostEvaluationAirfareAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Cost Airfare Add</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSeafarersList" runat="server">
    <ajaxtoolkit:toolkitscriptmanager combinescripts="false" id="ToolkitScriptManager1"
        runat="server">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlHopList">
        <ContentTemplate>
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <div class="subHeader" style="position: relative">
                <div id="divHeading">
                    <eluc:title runat="server" id="ucTitle" text="Add"></eluc:title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:tabstrip id="MenuCrewAirfare" runat="server" ontabstripcommand="MenuCrewAirfare_TabStripCommand">
                    </eluc:tabstrip>
                </div>
            </div>
            <div id="search">
                <br />
                <table cellpadding="1" cellspacing="3" width="80%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCountry" runat="server" Text="Country"></asp:Literal>
                        </td>
                        <td>
                            <eluc:country id="ddlCountry" runat="server" cssclass="input" appenddatabounditems="true"
                                autopostback="true" ontextchangedevent="ddlCountry_TextChanged" />
                        </td>
                        <td>
                            <asp:Literal ID="lblState" runat="server" Text="State"></asp:Literal>
                        </td>
                        <td>
                            <eluc:state id="ddlState" runat="server" cssclass="input" appenddatabounditems="true"
                                autopostback="true" ontextchangedevent="ddlState_TextChanged" />
                        </td>
                        <td>
                            <asp:Literal ID="lblCity" runat="server" Text="City"></asp:Literal>
                        </td>
                        <td>
                            <eluc:city id="ddlCity" runat="server" cssclass="input" appenddatabounditems="true"
                                autopostback="true" ontextchangedevent="ddlCity_TextChanged" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0">
                <asp:GridView ID="gvCrewAirfare" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvCrewAirfare_RowDataBound" ShowFooter="true"
                    ShowHeader="true" EnableViewState="false" AllowSorting="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Tick">
                            <HeaderTemplate>
                                <asp:Literal ID="lblTick" runat="server" Text="Tick"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCheck" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From City">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblFrom" runat="server" Text="From"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCrewAirfareId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWAIRFAREID") %>'></asp:Label>
                                <asp:Label ID="lblFromCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMCITYNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ToCity Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblTo" runat="server" Text="To"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblToCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOCITYNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Airfare">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAirfare" runat="server" Text="Airfare"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblJoinerAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFARE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From City">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblFrom" runat="server" Text="From"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFromCity1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOCITYNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ToCity Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblTo" runat="server" Text="To"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblToCity1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMCITYNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Airfare">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAirfare" runat="server" Text="Airfare"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblReturnAirfare" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRETURNAIRFARE") %>'></asp:Label>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
