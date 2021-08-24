<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOfficeAccrualAllotmentRequestDraftViewNotinPB.aspx.cs"
    Inherits="AccountsOfficeAccrualAllotmentRequestDraftViewNotinPB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew BOW</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCTM">
        <ContentTemplate>
            <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="Generate Allotment Request" ShowMenu="false">
                        </eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPB" runat="server" OnTabStripCommand="MenuPB_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                </div>
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFileNo" runat="server" Text="File No"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFileNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblVesselAccount" runat="server" Text="Vessel Account"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselAccount" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank / Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRank" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblComponent" runat="server" Text="Component"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtComponent" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVoucherDate" runat="server" Text="Voucher Date"></asp:Literal>
                        </td>
                        <td colspan="3">
                           <eluc:Date id="txtVoucherDate" runat="server" CssClass="input_mandatory" />
                        </td>                       
                    </tr>
                    <tr>                       
                        <td>
                            <asp:Literal ID="lblVoucherLongDescription" runat="server" Text="Voucher Long Description"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Rows="3" Width="500px"></asp:TextBox>                     
                        </td>
                    </tr>
                </table>                
                <br />
                <asp:GridView ID="gvPB" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvPB_RowDataBound" ShowHeader="true"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>                       
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblHeaderRow" runat="server" Text="Row"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <%# ((DataRowView)Container.DataItem)["FLDROW"] %>
                            </ItemTemplate>
                        </asp:TemplateField>                                                                                                
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblHeaderAccountCode" runat="server" Text="Account Code"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblHeaderBudgetCode" runat="server" Text="Budget Code"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <%# ((DataRowView)Container.DataItem)["FLDSUBACCOUNT"]%>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblHeaderOwnerBudgetCode" runat="server" Text="Owner Budget Code"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <%# ((DataRowView)Container.DataItem)["FLDOWNERBUDGETGROUP"]%>
                            </ItemTemplate>
                        </asp:TemplateField>  
                         <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblHeaderAmount" runat="server" Text="Amount"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                 <%# ((DataRowView)Container.DataItem)["FLDAMOUNT"]%>
                            </ItemTemplate>
                        </asp:TemplateField>  
                                                          
                    </Columns>
                </asp:GridView>                           
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
