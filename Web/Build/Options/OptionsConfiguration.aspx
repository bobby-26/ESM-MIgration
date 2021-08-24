<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsConfiguration.aspx.cs" Inherits="OptionsConfiguration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Configuration</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />  
         <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>     
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
     <table class="loginpagebackground" width="100%" align="center" cellpadding="0" cellspacing="0" height="60px">
        <tr>
            <td align="left" valign="top">
                <font class="application_title"><asp:Literal ID="lbleLog" runat="server" Text="eLog"></asp:Literal></font>&nbsp;&nbsp;
            </td>
            <td align="right" valign="top">
                <font class="loginpage_companyname"><b><asp:Literal ID="lblManagement" runat="server"></asp:Literal></b></font>                
            &nbsp;&nbsp;<img id="Img1" runat="server" style="vertical-align: top" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>" alt="Phoenix" />&nbsp;
            </td>
        </tr>
    </table>
    <div class="subHeader">
        <div class="divFloatLeft">
            <eluc:Title runat="server" ID="Title1" Text="Configuration Settings" ShowMenu="false">
            </eluc:Title>
        </div>
        <div style="position: absolute; right: 0px">
            <eluc:TabStrip ID="MenuConfig" runat="server" OnTabStripCommand="Config_TabStripCommand">
        </eluc:TabStrip>
        </div>
    </div>       
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <br />
    <div id="divPage1"
    <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" Width="100%"
        OnRowCancelingEdit="gvList_RowCancelingEdit" OnRowEditing="gvList_RowEditing" 
        OnRowUpdating="gvList_RowUpdating"
        CellPadding="3" ShowHeader="true" EnableViewState="false">
        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
        <RowStyle Height="10px" />
        <Columns>
            <asp:TemplateField HeaderText="Key">
                 <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                <ItemTemplate>
                    <%# DataBinder.Eval(Container, "DataItem.FLDKEY")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Value">
                 <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                <ItemTemplate>
                    <%# DataBinder.Eval(Container, "DataItem.FLDVALUE")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="lblKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDKEY")%>'></asp:Label>
                    <asp:TextBox ID="txtValue" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVALUE")%>'></asp:TextBox>
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
    <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility:hidden"/>
    </form>
</body>
</html>
