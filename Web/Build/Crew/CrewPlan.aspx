<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlan.aspx.cs" Inherits="CrewPlan" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OffSigner" Src="~/UserControls/UserControlCrewOnboard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Query Activity</title>
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
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Plan" ShowMenu="false" />
                        <eluc:Status runat="server" ID="ucStatus" />
                    </div>
                </div>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblEmployeeNumber" runat="server" Text="Employee Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblDateStatus" runat="server" Text="DOA"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDOA" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <br />
                <asp:GridView ID="gvPlan" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowFooter="true" ShowHeader="true" EnableViewState="false"
                     OnRowCommand="gvPlan_RowCommand" OnRowCancelingEdit="gvPlan_RowCancelingEdit" OnRowEditing="gvPlan_RowEditing" 
                    OnRowUpdating="gvPlan_RowUpdating" OnRowDeleting="gvPlan_RowDeleting" OnRowDataBound="gvPlan_RowDataBound">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField HeaderText="Vessel">
                            <ItemTemplate>
                                <asp:Label ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREWPLANID")%>'></asp:Label>
                                 <%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblCrewPlanIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREWPLANID")%>'></asp:Label>
                                <asp:Label ID="lblEmpId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></asp:Label>
                                <eluc:Vessel ID="ddlVesselEdit" runat="server" VesselsOnly="true" AppendDataBoundItems="true" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" SelectedVessel='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELID")%>' 
                                CssClass="input_mandatory" AutoPostBack="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Vessel ID="ddlVesselAdd" runat="server" VesselsOnly="true" AppendDataBoundItems="true" CssClass="input_mandatory" AutoPostBack="true" 
                                VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rank">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Rank ID="ddlRankEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" RankList="<%#PhoenixRegistersRank.ListRank() %>" SelectedRank='<%# DataBinder.Eval(Container, "DataItem.FLDRANKID")%>' 
                                AutoPostBack="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Rank ID="ddlRankAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" AutoPostBack="true" 
                                RankList="<%#PhoenixRegistersRank.ListRank() %>"/>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Off-Signer">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                               <eluc:OffSigner ID="ddlOffSignerEdit" runat="server" CssClass="input" AppendDataBoundItems="true" 
                              OnboardList='<%#PhoenixCrewManagement.ListCrewOnboard(int.Parse(DataBinder.Eval(Container, "DataItem.FLDVESSELID").ToString()), int.Parse(DataBinder.Eval(Container, "DataItem.FLDRANKID").ToString()))%>'
                              SelectedCrew='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERID")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:OffSigner ID="ddlOffSignerAdd" runat="server" CssClass="input" AppendDataBoundItems="true"
                                OnboardList='<%#PhoenixCrewManagement.ListCrewOnboard(null,null)%>' />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exp. Join Date">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtExpJoinDateEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE") %>' />
                            </EditItemTemplate>
                             <FooterTemplate>
                                <eluc:Date ID="txtExpJoinDateAdd" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Off-Signer Remarks">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERREMARKS")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOffSignerRemarkEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERREMARKS")%>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtOffSignerRemarkAdd" runat="server" CssClass="gridinput"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Port">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:SeaPort ID="ddlSeaPortEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    SeaportList="<%#PhoenixRegistersSeaport.ListSeaport() %>"
                                    SelectedSeaport='<%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:SeaPort ID="ddlSeaPortAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    SeaportList="<%#SouthNests.Phoenix.Registers.PhoenixRegistersSeaport.ListSeaport() %>" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reliever Remarks">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRELIEVERREMARKS")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRelieverRemarkEdit" runat="server" CssClass="gridinput" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRELIEVERREMARKS")%>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtRelieverRemarkAdd" runat="server" CssClass="gridinput"></asp:TextBox>
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
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="De-Plan"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
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
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
