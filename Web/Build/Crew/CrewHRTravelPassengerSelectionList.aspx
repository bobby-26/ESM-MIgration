<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHRTravelPassengerSelectionList.aspx.cs" Inherits="CrewHRTravelPassengerSelectionList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Designation" Src="../UserControls/UserControlDesignation.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OfficeStaff</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmHRTravelPassengerSelection" runat="server" submitdisabledcontrols="true">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlOfficeStaffEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucstatus" runat="server" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="Office Staff" ShowMenu="false"></eluc:Title>
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuHRTravelPassengerSelection" runat="server" OnTabStripCommand="HRTravelPassengerSelection_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div id="divFind" style="position: relative; z-index: 2">
                        <table id="tblConfigureOfficeStaff" width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblEmployeeFileNumber" runat="server" Text="Employee/File Number"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtEmployeeNumber" MaxLength="50" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblFirstName" runat="server" Text="Name"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblPresentRank" runat="server" Text="Designation"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Designation ID="ucDesignation" runat="server" AppendDataBoundItems="true"  CssClass="input" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuRegistersOfficeStaff" runat="server" OnTabStripCommand="RegistersOfficeStaff_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0">
                        <asp:GridView ID="gvOfficeStaff" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvOfficeStaff_RowCommand" OnRowDataBound="gvOfficeStaff_ItemDataBound"
                            OnRowEditing="gvOfficeStaff_RowEditing" ShowHeader="true"
                            EnableViewState="false" AllowSorting="false" DataKeyNames="FLDOFFICESTAFFID">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="Salutation">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblSalutationHeader" runat="server" Text="Salutation"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSalutation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSALUTATION") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FullName">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblNameHeader" runat="server" Text="Name"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEFIRSTNAME") + " " + DataBinder.Eval(Container,"DataItem.FLDOFFICESURNAME") %>'></asp:Label>
                                        <asp:Label ID="lblFirstName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEFIRSTNAME") %>'></asp:Label>
                                        <asp:Label ID="lblMiddleName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME") %>'></asp:Label>
                                        <asp:Label ID="lblLastName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICESURNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EmployeeNumber">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblEmployeeNumberHeader" runat="server" Text="Employee/File Number"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeeNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENUMBER") %>'></asp:Label>
                                        <asp:Label ID="lblOfficeStaffid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICESTAFFID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designation">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblDesignation" runat="server" Text="Designation"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></asp:Label>
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
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="ADDPASSENGER" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                            ToolTip="Add Passenger"></asp:ImageButton>
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
                                <td width="20px">&nbsp;
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
