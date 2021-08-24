<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsWRHOT.aspx.cs"
    Inherits="VesselAccountsWRHOT" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Monthly Overtime</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRestHourCrewList" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlRestHourStart">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <eluc:Title runat="server" ID="ucTitle" Text="Monthly overtime summary" />
                            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuRHGeneral" runat="server" OnTabStripCommand="RHGeneral_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <table width="75%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" VesselsOnly="true" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlvessel_selectedindexchange" />
                                <asp:Label ID="lblAmount" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:Literal ID="lblMonth" runat="server" Text="Month"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlmonth_selectedindexchange">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
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
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlyear_selectedindexchange">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="lblName" runat="server" Text="Employee"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                    AppendDataBoundItems="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>

                        </tr>
                    </table>
                    <br />
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuOPAList" runat="server" OnTabStripCommand="CrewList_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvOPA" runat="server" AutoGenerateColumns="False" Width="100%"
                            GridLines="None" OnRowDataBound="gvOPA_RowDataBound" OnRowCancelingEdit="gvOPA_RowCancelingEdit"
                            OnRowCommand="gvOPA_RowCommand" OnRowEditing="gvOPA_RowEditing" AllowSorting="true"
                            OnRowUpdating="gvOPA_RowUpdating" CellPadding="3" ShowHeader="true"
                            EnableViewState="false" OnRowDeleting="gvOPA_RowDeleting"
                            OnRowCreated="gvOPA_RowCreated">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblFileNo" runat="server" Text="File No"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="lblFileNoitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></asp:Literal>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="lblNameItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></asp:Literal>
                                        <asp:Label ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="lblRankItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblDate" runat="server" Text="Sign On"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateItem" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblGOT" runat="server" Text="Guaranteed(Hrs)"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="lblGOTItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGOT") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblOT" runat="server" Text="Actual(Hrs)"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="lblOTItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAOT") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblestimatedOT" runat="server" Text="Estimated(Hrs)"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="lblestimatedOTItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESTIMATEDOT") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblAOT" runat="server" Text="Additional(Hrs)"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="lblAOTItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALIDOT") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblOTwage" runat="server" Text="Wage per hour(USD)"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="lblOTwageItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTPERHOUR") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblOTtotalwage" runat="server" Text="Total wage(USD)"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOTtotalwageItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTWAGE") %>'></asp:Label>
                                        <asp:Label ID="lblSignonoffid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                    <br />
                    <b>Last month detail</b>
                    <div>
                        <div id="divGrid1" style="position: relative; z-index: 0; width: 100%;">
                            <asp:GridView ID="gvPrevious" runat="server" AutoGenerateColumns="False" Width="100%"
                                GridLines="None" OnRowDataBound="gvPrevious_RowDataBound"
                                OnRowCommand="gvPrevious_RowCommand" AllowSorting="true"
                                CellPadding="3" ShowHeader="true"
                                EnableViewState="false">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="left" Width="100px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblMonth" runat="server" Text="MM/YYYY"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonthYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHYEAR") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblGuaranteed" runat="server" Text="Guaranteed(Hrs)"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="lblGuaranteedItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGOT") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblActualot" runat="server" Text="Actual OT(hr)"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="lblActualOtItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALOTHR") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblEstimatedOT" runat="server" Text="Estimated OT(hr)"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="lblEstimatedOTitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESTIMATEDOTHR") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblvariance" runat="server" Text="Variance(hr)"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="lblvarianceitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblWageperhour" runat="server" Text="Wage per hour(USD)"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Literal ID="lblWageperhouritem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAGEPERHOUR") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblEarningDeduction" runat="server" Text="Earning/Deduction"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEarningDeductionitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEARNINGORDECUTION") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="right" Width="100px"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        <ItemTemplate>
                                            <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                            <asp:ImageButton ID="cmdSync" runat="server" ImageUrl="<%$ PhoenixTheme:images/24.png %>"
                                                CommandName="SEND" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Send OT wage to portage bill" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <eluc:Status ID="ucStatus" runat="server" />
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
