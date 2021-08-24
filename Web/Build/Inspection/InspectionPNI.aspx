<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNI.aspx.cs" Inherits="InspectionPNI" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PNI</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmPNI" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPNI">
        <ContentTemplate>
            <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="Medical Case"
                            ShowMenu="<%# Title1.ShowMenu %>"></eluc:Title>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPNImain" runat="server" OnTabStripCommand="MenuPNImain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <%--<div style="position: relative; overflow: hidden; clear: right;">
                    <iframe runat="server" id="ifMoreInfo" style="min-height: 300px; width: 100%; overflow-x: hidden">
                    </iframe>
                </div>--%>
                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                    <eluc:TabStrip ID="MenuPNI" runat="server" OnTabStripCommand="MenuPNI_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="GVPNI" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnRowDeleting="GVPNI_RowDeleting"
                        Width="100%" CellPadding="3" OnRowDataBound="GVPNI_RowDataBound" ShowHeader="true" OnRowCommand="GVPNI_RowCommand"
                        EnableViewState="false" AllowSorting="true" OnSorting="GVPNI_Sorting" OnSelectedIndexChanging="GVPNI_SelectedIndexChanging"
                        DataKeyNames="FLDINSPECTIONPNIID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCaseNOHeader" runat="server" CommandName="Sort" CommandArgument="FLDCASENUMBER"
                                        ForeColor="White">Case No</asp:LinkButton>
                                    <img id="FLDCASENUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInspectionPNIId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTIONPNIID"] %>'></asp:Label>
                                    <asp:Label ID="lblPNICaseid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDPNICASEID"] %>'></asp:Label>
                                    <asp:Label ID="lblDtkey" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDTKEY"] %>'></asp:Label>
                                    <asp:LinkButton ID="lnkCaseNo" runat="server" CommandName="select"
                                     CommandArgument='<%# Container.DataItemIndex %>'><%# ((DataRowView)Container.DataItem)["FLDCASENUMBER"]%></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkCrewnameHeader" runat="server" CommandName="Sort" CommandArgument="FLDCREWNAME"
                                        ForeColor="White">Crew Name</asp:LinkButton>
                                    <img id="FLDCREWNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                 <%# ((DataRowView)Container.DataItem)["FLDCREWNAME"] %>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblillnessdate" runat="server" CommandName="Sort" CommandArgument="FLDILLNESSINJURYDATE"
                                        ForeColor="White">Illness Date</asp:LinkButton>
                                    <img id="FLDILLNESSINJURYDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDILLNESSINJURYDATE"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblVesselCodeHeader" runat="server"> Vessel Code</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDVESSELCODE"]%>
                                </ItemTemplate>
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
                                    <asp:ImageButton runat="server" AlternateText="PNI Checklist" CommandName="CHECKLIST" ImageUrl="<%$ PhoenixTheme:images/checklist.png %>"
                                        ID="cmdChkList" ToolTip="PNI Checklist"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>                                                       
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>

