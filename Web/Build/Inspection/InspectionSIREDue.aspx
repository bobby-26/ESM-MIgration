<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSIREDue.aspx.cs"
    Inherits="InspectionSIREDue" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAR</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionSIRE" runat="server">
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionSIRE">
        <ContentTemplate>
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <eluc:status id="ucStatus" runat="server" text="" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="frmTitle" Text="SIRE Next Due" ShowMenu="false"></eluc:Title>
                </div>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input" 
                                VesselsOnly="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                        </td>
                        <td>
                           <asp:Literal ID="lblFleet" runat="server" Text=" Fleet"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Fleet ID="ucFleet" runat="server" AppendDataBoundItems="true" CssClass="input" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:tabstrip id="MenuGeneral" runat="server" ontabstripcommand="MenuGeneral_TabStripCommand">
                    </eluc:tabstrip>
                </div>
                <div id="divSIRE" style="position: relative; z-index: 3; position: static">
                    <asp:GridView ID="gvSIRE" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvSIRE_RowCommand"
                        OnRowDataBound="gvSIRE_ItemDataBound" 
                        OnSorting="gvSIRE_Sorting" AllowSorting="true" 
                        ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDINSPECTIONSIREID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left"/>
                                <HeaderTemplate>
                                    <asp:Label ID="lblvesselHeader" runat="server">Vessel</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>  
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left"/>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCompanyHeader" runat="server">Company</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOMPANYNAME")%>  
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left"/>
                                <HeaderTemplate>
                                    <asp:Label ID="lblInspectionHeader" runat="server">Inspection</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>  
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left"/>
                                <HeaderTemplate>
                                   <asp:Label ID="lblLastDoneHeader" runat="server"> Last Done </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTDONEDATE")) %>  
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left"/>
                                <HeaderTemplate>
                                   <asp:Label ID="lblNextDueHeader" runat="server"> Next Due </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDUEDATE"))%>  
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
