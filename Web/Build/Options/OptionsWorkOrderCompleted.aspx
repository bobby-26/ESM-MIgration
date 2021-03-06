<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsWorkOrderCompleted.aspx.cs"
    Inherits="OptionsWorkOrderCompleted" %>
    
<%@ Import Namespace="System.Data" %>
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
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewList" runat="server" OnTabStripCommand="CrewList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvWorkOrderReport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvWorkOrderReport_OnRowDataBound"
                        AllowSorting="true" OnSorting="gvWorkOrderReport_Sorting" 
                        DataKeyNames="FLDWORKORDERID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>                                                                     
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblWorkorderNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDWORKORDERNUMBER"
                                        ForeColor="White">Work Order Number</asp:LinkButton>
                                    <img id="FLDWORKORDERNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkorderNumber" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblWorkorderNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDWORKORDERNAME"
                                        ForeColor="White">Work Order Title</asp:LinkButton>
                                    <img id="FLDWORKORDERNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblComponentNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOMPONENTNUMBER"
                                        ForeColor="White">Component Number</asp:LinkButton>
                                    <img id="FLDCOMPONENTNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentNumber" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblComponentNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDCOMPONENTNAME"
                                        ForeColor="White">Component Name</asp:LinkButton>
                                    <img id="FLDCOMPONENTNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                           
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Literal ID="lblFrequency" runat="server" Text="Frequency"></asp:Literal>                                   
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderTemplate>
                                    <asp:LinkButton ID="lblHeaderDoneDate" runat="server" CommandName="Sort" CommandArgument="FLDWORKDONEDATE"
                                        ForeColor="White">Done Date</asp:LinkButton>
                                    <img id="FLDWORKDONEDATE" runat="server" visible="false" />
                                </HeaderTemplate>                               
                                <ItemTemplate>
                                    <asp:Label ID="lblDoneDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKDONEDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Job Class">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderTemplate>
                                    <asp:LinkButton ID="lblHeaderJobClass" runat="server" CommandName="Sort" CommandArgument="FLDJOBCLASS"
                                        ForeColor="White">Job Class</asp:LinkButton>
                                    <img id="FLDJOBCLASS" runat="server" visible="false" />
                                </HeaderTemplate>   
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDJOBCLASS"]%></ItemTemplate>
                            </asp:TemplateField>                           
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblHeaderStarted" runat="server" CommandName="Sort" CommandArgument="FLDWORKORDERSTARTEDDATE"
                                        ForeColor="White">Started</asp:LinkButton>
                                    <img id="FLDWORKORDERSTARTEDDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDWORKORDERSTARTEDDATE"])%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblHeaderCompleted" runat="server" CommandName="Sort" CommandArgument="FLDWORKORDERCOMPLETEDDATE"
                                        ForeColor="White">Completed</asp:LinkButton>
                                    <img id="FLDWORKORDERCOMPLETEDDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDWORKORDERCOMPLETEDDATE"])%>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
