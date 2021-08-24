<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewUserMapping.aspx.cs"
    Inherits="CrewUserMapping" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCode" Src="~/UserControls/UserControlVesselCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Untitled Page</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersUser" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlUserEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Users" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="CrewUserMain" runat="server" OnTabStripCommand="CrewUserMain_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblCrewUserFilter" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Vessel ID="ddlVessel" runat="server" CssClass="dropdown_mandatory" VesselsOnly="true"
                                    OnTextChangedEvent="ddlVesselChanged" AppendDataBoundItems="true" AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblVesselShortCode" runat="server" Text="Vessel Short Code"></asp:Literal>
                            </td>
                            <td>
                                <eluc:VesselCode ID="ddlVesselCode" runat="server" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlVesselCodeChanged" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCrewUsers" runat="server" OnTabStripCommand="MenuCrewUsers_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="dgUser" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="dgUser_RowDataBound" OnRowEditing="dgUser_RowEditing"
                        OnRowCancelingEdit="dgUser_RowCancelingEdit" OnRowCommand="dgUser_RowCommand"
                        ShowHeader="true" AllowSorting="true" OnSorting="dgUser_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUserHeader" runat="server">Department&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUser" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></asp:Label>
                                    <asp:Label ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblUserNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDUSERNAME"
                                        ForeColor="White">Username&nbsp;</asp:LinkButton>
                                    <img id="FLDUSERNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUserCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></asp:Label>
                                    <asp:Label ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblShortCodeHeader" runat="server">Short Code</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActiveYNHeader" runat="server">
                                    Active Y/N
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYNTEXT").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblFirstNameHeader" runat="server">
                                    Name
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFirstName" runat="server" Text='<%#String.Concat(Eval("FLDFIRSTNAME")," ",Eval("FLDMIDDLENAME").ToString()," ",Eval("FLDLASTNAME").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnOnBoardEmployee">
                                        <asp:TextBox ID="txtEmployeenameEdit" runat="server" CssClass="input_mandatory" ReadOnly="true"
                                            Enabled="false" MaxLength="50" Width="50%" Text='<%#String.Concat(Eval("FLDFIRSTNAME")," ",Eval("FLDMIDDLENAME").ToString()," ",Eval("FLDLASTNAME").ToString()) %>'></asp:TextBox>
                                        <asp:TextBox ID="txtEmployeepRankEdit" runat="server" CssClass="input_mandatory"
                                            ReadOnly="true" Enabled="false" MaxLength="50" Width="20%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME")%>'></asp:TextBox>
                                        <asp:Image ID="imgOnBoardEmployeeEdit" runat="server" Style="cursor: pointer; vertical-align: top"
                                            ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" />
                                        <asp:TextBox ID="txtEmployeeIdEdit" runat="server" CssClass="input" Width="1px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEID") %>'></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblEffectiveFromHeader" runat="server">
                                    Effective From
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEFROM", "{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="EffectFromDateEdit" runat="server" CssClass="input_mandatory" />
                                </EditItemTemplate>
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Reset" ImageUrl="<%$ PhoenixTheme:images/unlock.png %>"
                                        CommandName="RESETPASSWORD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdReset"
                                        ToolTip="Reset Password"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;" runat="server">
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
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
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
