<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSignOnOffFlaggedVesselsReport.aspx.cs"
    Inherits="CrewSignOnOffFlaggedVesselsReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew SignOnOff S'pore Flagged Vessels</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
        function resizeFrame() {
            var obj = document.getElementById("ifMoreInfo");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
        }
        </script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewSignOnOff" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAppraisal">
        <ContentTemplate>
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Flagged Vessels" ShowMenu="flase">
                        </eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                </div>
                <table width="80%" cellpadding="2" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <asp:Literal ID="lblPeriodFrom" runat="server" Text="Period From"></asp:Literal>
                        </td>
                        <td valign="top">
                            <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                            &nbsp;&nbsp;&nbsp; To &nbsp;&nbsp;&nbsp;
                            <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                        </td>                        
                        <td>
                            <asp:Literal ID="lblFlag" runat="server" Text="Flag"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Flag ID="ucFlag" runat="server" Width="80%" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                AutoPostBack="true" OnTextChangedEvent="ucFlag_OnTextChangedEvent" />
                        </td>
                        <br />
                        <tr>
                            <td valign="top">
                                <asp:Literal ID="lblvessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <asp:ListBox ID="lstVessel" runat="server" CssClass="input" SelectionMode="Multiple"
                                    Height="100" Width="200" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID">
                                </asp:ListBox>
                            </td>
                            <td valign="top">
                            </td>
                        </tr>
                    </tr>
                </table>
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="MenuCrewSignonoffSporeFlagged" runat="server" OnTabStripCommand="MenuCrewSignonoffSporeFlagged_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvSignOnOff" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvSignOnOff_RowDataBound"
                        EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="From Date">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblOffSignerHeader" runat="server">Off Signer</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOffSignerId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERID")%>'></asp:Label>
                                    <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERNAME")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRankHeader" runat="server">Rank</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERRANK")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselNameHeader" runat="server">Vessel Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSignOffDateHeader" runat="server">SignOff Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERSIGNOFFDATE")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSignOffPortHeader" runat="server">SignOff Port</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERSIGNOFFPORT")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Joiner">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblJoinerHeader" runat="server">Joiner</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOnSignerId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRELIEVERID")%>'></asp:Label>
                                    <asp:LinkButton ID="lnkSignOnName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRELIEVERNAME")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblKinNameHeader" runat="Server">Kin Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDKINNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRelationShipHeader" runat="server">Relationship</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDRELATIONSHIP")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAddressHeader" runat="server">Address</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAddress1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDADDRESS").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDADDRESS").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDADDRESS").ToString() %>'></asp:Label>
                                    <eluc:ToolTip ID="lblAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESS")%>' />
                                    <asp:Label ID="lblAddress2" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDADDRESS")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDOBHeader" runat="server">DOB</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblNationalityHeader" runat="server">Nationality</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDNATIONALITY")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSignOnDateHeader" runat="server">SignOn Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDRELIEVERSIGNONDATE")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSignOnPortHeader" runat="server">SignOn Port</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDRELIEVERPORT")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWagesHeader" runat="server">Wages</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDRELIEVERWAGES")%>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
