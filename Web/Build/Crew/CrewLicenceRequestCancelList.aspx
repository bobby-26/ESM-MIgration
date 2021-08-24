<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestCancelList.aspx.cs"
    Inherits="CrewLicenceRequestCancelList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Licence Request Cancel List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLicReq" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <eluc:Title runat="server" ID="Title3" Text="Licence Request Cancel List" ShowMenu="true">
            </eluc:Title>
        </div>

         <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuLicenceList" runat="server" OnTabStripCommand="MenuLicenceList_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
            <asp:GridView ID="gvLicReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvLicReq_RowDataBound"
                EnableViewState="false"  AllowSorting="true"
                OnSorting="gvLicReq_Sorting" >
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRequestNumber" runat="server" Text="Request Number"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblProcessId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROCESSID")%>'></asp:Label>
                            <asp:Label ID="lblFlagId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGID")%>'></asp:Label>
                            <asp:Label ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFNUMBER") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDFLAG"
                                ForeColor="White">Flag</asp:LinkButton>
                            <img id="FLDFLAG" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                                Text='<%#DataBinder.Eval(Container, "DataItem.FLDFLAGNAME")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vessel Name">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME").ToString().TrimEnd(',') %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblCrewChangeDate" runat="server" Text="Crew Change Date"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEDATE", "{0:dd/MMM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRequestedDate" runat="server" Text="Requested Date"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblCRAReqdDate" runat="server" Text="CRA Reqd Date"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDCRADATE", "{0:dd/MMM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblCancelledDate" runat="server" Text="Cancelled Date"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDCANCELDATE", "{0:dd/MMM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblCancelledBy" runat="server" Text="Cancelled By"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDCANCELLEDBY")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>
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
