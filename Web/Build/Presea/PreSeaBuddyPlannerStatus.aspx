<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBuddyPlannerStatus.aspx.cs" Inherits="Presea_PreSeaBuddyPlannerStatus" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Buddy Planner Status</title>
     <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
   
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    
     <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaPlannerStatus" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaPlannerStatus">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
            </eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" id="ucTitle" Text="Planner Status">
                        </eluc:Title>
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPreSeaPlannerStatus" runat="server" OnTabStripCommand="MenuPreSeaPlannerStatus_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div>
                    <table id="tblFind" runat="server">
                        <tr>
                            <td>
                                From Date
                            </td>
                            <td>
                                <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                To Date
                            </td>
                            <td>
                                <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                Faculty
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="input"></asp:DropDownList>
                            </td>
                        </tr> 
                    </table>
                </div>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaPlanner" runat="server" OnTabStripCommand="MenuPreSeaPlanner_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPreSeaPlannerStatus" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%"
                        CellPadding="3" 
                        OnRowCreated="gvPreSeaPlannerStatus_RowCreated"
                        OnRowCommand="gvPreSeaPlannerStatus_RowCommand" 
                        OnRowDataBound="gvPreSeaPlannerStatus_RowDataBound"
                        OnRowCancelingEdit="gvPreSeaPlannerStatus_RowCancelingEdit"
                        OnRowUpdating="gvPreSeaPlannerStatus_RowUpdating"
                        OnRowEditing="gvPreSeaPlannerStatus_RowEditing" ShowFooter="true" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true" >
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            
                            <asp:TemplateField>
                                <ItemStyle Width="30%" Wrap="false" />
                                <HeaderTemplate>
                                    Faculty Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBuddyPlannerId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDDYPLANNERID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblFacultyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFACULTYNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Width="10%" Wrap="false" />
                                <HeaderTemplate>
                                    Planned Date
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPlannedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNEDDDATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Width="10%" Wrap="false" />
                                <HeaderTemplate>
                                    Completed Yes/No
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCompletedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETED") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RadioButtonList ID="rblSelection" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" Width="55%" />
                                <HeaderTemplate>
                                    Remarks
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' CssClass="input" 
                                                TextMode="MultiLine" Width="100%" Rows="4"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Width="5%" Wrap="false" />
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
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
