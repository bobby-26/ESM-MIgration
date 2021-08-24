<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListRHShipWorkCalendar.aspx.cs" Inherits="CommonPickListRHShipWorkCalendar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ship Calendar</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Literal ID="lblShipCalendar" runat="server" Text="Ship Calendar"></asp:Literal>
        </div>
    </div>               
    <br clear="all" />   
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVesselname" runat="server" CssClass="input" Enabled="false" Width="250px"></asp:TextBox>
                    </td>
                    <td>
                    <asp:Literal ID="lblMonth" runat="server" Text="Month"></asp:Literal>
                    </td>
                     <td>
                            <asp:DropDownList ID="ddlmonth" runat="server" CssClass="dropdown_mandatory" OnSelectedIndexChanged="ddlmonth_OnSelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Jan" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Jun" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Jul" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblYear" runat="server" Text="Year"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdown_mandatory" OnSelectedIndexChanged="ddlmonth_OnSelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>                            
                        </td>
                </tr>
            </table><br />
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <asp:GridView ID="gvShipCalendar" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvShipCalendar_OnRowCommand"
                    ShowHeader="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>  
                            <asp:Label ID="lblCalenderid" runat="server"  Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCALENDARID") %>'></asp:Label>                              
                                <asp:LinkButton ID="lnkDate" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                   Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE","{0:dd/MMM/yyyy}") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:TemplateField HeaderText="IDL W-E/IDL E-W">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblIDLWEIDLEW" runat="server" Text="IDL W-E/IDL E-W"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblClock" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOCKNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                      <asp:TemplateField HeaderText="Hours">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblHours" runat="server" Text="Hours"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblHours" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOURS") %>'></asp:Label>
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
            </div>        
    </form>
</body>
</html>

