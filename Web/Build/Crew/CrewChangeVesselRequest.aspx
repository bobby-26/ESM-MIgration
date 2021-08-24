<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewChangeVesselRequest.aspx.cs"
    Inherits="CrewChangeVesselRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Change Request</title>
    <style type="text/css">
        .style1
        {
            height: 22px;
        }
    </style>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewChangeRequest" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewChangeRequest">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ucTitle" Text="Crew Change Request" ShowMenu="<%# ucTitle.ShowMenu %>">
                    </eluc:Title>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CCPMenu" runat="server" OnTabStripCommand="CCPMenu_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="CrewChangeRequestMenu" runat="server" TabStrip="false" OnTabStripCommand="CrewChangeRequest_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                <br />
                <table cellspacing="1" cellpadding="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblDateofCrewChange" runat="server" Text="Date of Crew Change"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrewChangeDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCrewChangePort" runat="server" Text="Crew Change Port"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPort" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCityName" runat="server" Text="City"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCrewChangeReason" runat="server" Text="Crew Change Reason"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrewChangeReason" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <div id="divGrid" style="position: relative; z-index: 10; width: 100%;">
                    <asp:GridView ID="gvCCPlan" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvCCPlan_RowDataBound"
                        OnRowCommand="gvCCPlan_OnRowCommand" EnableViewState="false" AllowSorting="true"
                        OnSorting="gvCCPlan_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Select" Visible="false" />
                            <asp:TemplateField HeaderText="On-Signer">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblCrewHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></asp:Label>
                                    <asp:Label ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblDocumentsReq" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTSREQUIRED") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                        CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tick">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblTick" runat="server" Text="Tick"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkOnSigner" />
                                    <asp:Label runat="server" ID="lblOnSignerCrewChange" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERCREWCHANGE") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblOnSignerCrewChangeNotReq" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERCREWCHANGENOTREQUEST") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Off-Signer">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOffSigner" runat="server" Text="Off-Signer"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOffSignerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkOffSigner" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tick">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblTick" runat="server" Text="Tick"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkOffSigner" />
                                    <asp:Label runat="server" ID="lblOffSignerCrewChange" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERCREWCHANGE") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblOffSignerCrewChangeNotReq" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERCREWCHANGENOTREQUEST") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
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
