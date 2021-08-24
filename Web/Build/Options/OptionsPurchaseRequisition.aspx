<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsPurchaseRequisition.aspx.cs"
    Inherits="OptionsPurchaseRequisition" %>
    
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOwnersVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCrewList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewList">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>               
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                </div>
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel ID="ddlVessel" runat="server" CssClass="dropdown_mandatory" />
                        </td>                       
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewList" runat="server" OnTabStripCommand="CrewList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvPR" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvPR_ItemDataBound"
                    ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvPR_Sorting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>                       
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblVesselNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"
                                    ForeColor="White">Vessel Name</asp:LinkButton>
                                <img id="FLDVESSELNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblFormNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDFORMNO"
                                    ForeColor="White">Form No</asp:LinkButton>
                                <img id="FLDFORMNO" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>                              
                                <%#((DataRowView)Container.DataItem)["FLDFORMNO"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblTitleHeader" runat="server" CommandName="Sort" CommandArgument="FLDTITLE"
                                    ForeColor="White">Form Title</asp:LinkButton>
                                <img id="FLDTITLE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDTITLE"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblOrderDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDORDEREDDATE"
                                    ForeColor="White">Order Date</asp:LinkButton>
                                <img id="FLDORDEREDDATE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDORDEREDDATE"])%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblStockTypeHeader" runat="server" Text="Type"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDSTOCKTYPE"]%>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblVendorHeader" runat="server" Text="Vendor Name"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDVENDORNAME"]%>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblCurrenyHeader" runat="server" Text="Currency"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCURRENCYCODE"]%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblFinalTotal" runat="server" Text="Final Total"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDFINALTOTAL"]%>
                            </ItemTemplate>
                        </asp:TemplateField>                                          
                    </Columns>
                </asp:GridView>
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
                 <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img1" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblOverdue" runat="server" Text="* Overdue"></asp:Literal>
                        </td>
                        <td>
                            <img id="Img2" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDue" runat="server" Text="* Due"></asp:Literal>
                        </td>                        
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
