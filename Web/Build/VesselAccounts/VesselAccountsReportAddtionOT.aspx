<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReportAddtionOT.aspx.cs" Inherits="VesselAccountsReportAddtionOT" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EntryType" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>D11</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


        <script type="text/javascript">
            function resize() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 55 + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="resize();" onresize="resize();">
    <form id="frmAddtionalOT" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlD11">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="div2" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Earning and Deduction" ShowMenu="True"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <%--<eluc:TabStrip ID="MenuReportD11" runat="server" OnTabStripCommand="MenuReportD11_TabStripCommand"></eluc:TabStrip>--%>
                </div>
                <div>
                    <table width="80%">
                        <tr>
                            <td>
                                <asp:Literal ID="Literal1" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                                    CssClass="input" Width="240px" AssignedVessels="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblReportfortheMonthof" runat="server" Text="Month"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="dropdown_mandatory">
                                    <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="lblYear" runat="server" Text="Year"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdown_mandatory">
                                </asp:DropDownList>
                            </td>

                        </tr>
                        <tr runat="server" id="trErD">
                            <td>
                                <asp:Literal ID="lblEarningDeduction" runat="server" Text="Earning/Deduction"></asp:Literal>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblEarningDeduction" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnTextChanged="ddlEmployee_TextChangedEvent">
                                    <asp:ListItem Selected="True" Value="128" Text="Earning"></asp:ListItem>
                                    <asp:ListItem Value="129" Text="Deduction"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Literal ID="lblEntryType" runat="server" Text="Entry Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:EntryType ID="ddlEntryTypeEarning" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    HardTypeCode="128" ShortNameFilter="OTA,BSH,HRA,AOT,BRF,REM,BSU" AutoPostBack="true"
                                    Visible="true" />
                                <eluc:EntryType ID="ddlEntryTypeDeduction" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    HardTypeCode="129" ShortNameFilter="CRR,OTD,BDD" AutoPostBack="true" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuEarningdeductionList" runat="server" OnTabStripCommand="MenuEarningdeductionList_TabStripCommand"></eluc:TabStrip>

                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvOvertimeHolidays" runat="server" AutoGenerateColumns="False"
                        GridLines="None" Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="false"
                        EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File No.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Entry Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENT") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>
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
                            <td width="20px">&nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <eluc:Number ID="txtnopage" runat="server" CssClass="input" Width="20px" IsInteger="true" />
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
