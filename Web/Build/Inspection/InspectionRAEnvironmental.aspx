<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAEnvironmental.aspx.cs" Inherits="InspectionRAEnvironmental" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HazardType" Src="~/UserControls/UserControlRAHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Frequency" Src="~/UserControls/UserControlRAFrequency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRAEnvironmental" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlRAEnvironmental">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Risk Analysis" ShowMenu="<%# Title1.ShowMenu %>">
                    </eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuEnvironmental" runat="server" OnTabStripCommand="Environmental_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                <table width="100%" cellpadding="1" cellspacing="3">
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblEnvironmental" runat="server" Text="Environmental"></asp:Literal></b>
                        </td>
                    </tr>
                     <tr>
                         <td>
                             <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                         </td>
                         <td>
                             <eluc:Category ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input"
                                 Enabled="false" />
                         </td>
                         <td>
                             <asp:Literal ID="lblSubCategory" runat="server" Text="Sub Category"></asp:Literal>
                         </td>
                         <td>
                             <asp:TextBox ID="txtSubcategory" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                 Width="220px"></asp:TextBox>
                         </td>
                     </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblActivity" runat="server" Text="Activity"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtActivity" runat="server" CssClass="input_mandatory" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblRisk" runat="server" Text="Risk"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRisk" runat="server" CssClass="input_mandatory" Width="360px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:literal ID="lblImpact" runat="server" Text="Impact"></asp:literal>
                        </td>
                        <td>
                            <eluc:HazardType ID="ucHazardType" runat="server" Type="2" CssClass="dropdown_mandatory"
                                AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblActivityFrequency" runat="server" Text="Activity Frequency"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Frequency ID="ucFrequency" runat="server" Type="2" CssClass="dropdown_mandatory"
                                AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblActivityDuration" runat="server" Text="Activity Duration"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Frequency ID="ucActivityDuration" runat="server" Type="1" CssClass="dropdown_mandatory"
                                AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <asp:literal ID="lblRAControl" runat="server" Text="RA Control"></asp:literal>
                        </td>
                        <td>
                            <eluc:Frequency ID="ucRAControl" runat="server" Type="4" CssClass="dropdown_mandatory"
                                AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblProbabilityofOccurance" runat="server" Text="Probabilty of Occurance"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPOC" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblLikelyhoodofHarm" runat="server" Text="Likelyhood of harm"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLOH" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblLevelofControl" runat="server" Text="Level of Control"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLOC" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:literal ID="lblLevelOfRisk" runat="server" Text="Level of risk"></asp:literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLevelOfRisk" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuGridEnvironmental" runat="server" OnTabStripCommand="GridEnvironmental_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvEnvironmental" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvEnvironmental_RowCommand" OnRowDataBound="gvEnvironmental_ItemDataBound"
                        OnRowDeleting="gvEnvironmental_RowDeleting"
                        AllowSorting="true" OnRowEditing="gvEnvironmental_RowEditing" OnSorting="gvEnvironmental_Sorting"
                        ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDASSESSMENTID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblActivityHeader" runat="server" CommandName="Sort" CommandArgument="FLDACTIVITY"
                                        ForeColor="White">Activity&nbsp;</asp:LinkButton>
                                    <img id="FLDACTIVITY" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAssessmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSESSMENTID") %>'></asp:Label>
                                     <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                  
                                    <asp:LinkButton ID="lnkActivity" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITY") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRiskHeader" runat="server">Risk</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRisk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISK") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblImpactHeader" runat="server" Text="Impact"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblImpact" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Literal ID="lblProbabilityofOccurance" runat="server" Text="Probabilty of occurance"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOC") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblLikelyHoodOfHarm" runat="server" Text="Likelyhood of Harm"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLOH" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLIKELYHOODOFHARM") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblLevelofControl" runat="server" Text="Level of Control"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLevelofControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFCONTROL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblLevelofRiskHeader" runat="server">Level of Risk</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLevelofRisk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELOFRISK") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
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
                        <eluc:Status runat="server" ID="Status1" />
                    </table>
                </div>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
