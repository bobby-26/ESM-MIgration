<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerIssueFlag.aspx.cs"
    Inherits="DefectTrackerIssueFlag" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Module" Src="~/UserControls/UserControlSEPModuleList.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Issue Track</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </div>
    <script type="text/javascript">
        function resizediv() {
            var tbl = document.getElementById("tblComments");
            if (tbl != null) {
                for (var i = 0; i < tbl.rows.length; i++) {
                    tbl.rows[i].cells[0].getElementsByTagName("div")[0].style.width = 3 * (tbl.rows[i].cells[2].offsetWidth) + "px";
                }
            }
        } //script added for fixing Div width for the comments table
    </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
    <div class="subHeader">
        <div id="divHeading" class="divFloatLeft">
            <eluc:Title runat="server" ID="ucTitle" Text="Road Map" ShowMenu="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                Visible="false" />
        </div>
        <div style="position: absolute; right: 0px">
            <eluc:TabStrip ID="MenuIssueTrack" runat="server" OnTabStripCommand="MenuIssueTrack_TabStripCommand">
            </eluc:TabStrip>
        </div>
    </div>
    <div class="subHeader">
        <div style="position: absolute; right: 0px">
            <eluc:TabStrip ID="MenuBugDiscussion" runat="server" OnTabStripCommand="MenuBugDiscussion_TabStripCommand">
            </eluc:TabStrip>
        </div>
    </div>
    <br />
    <div style="position: relative">
        <table width="100%">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblMilestone" Text="Milestone" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlmilestone" runat="server" CssClass="dropdown_mandatory"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlmilestone_SelectedIndexChanged"
                        MaxLength="100" Width="150px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblIssueFlag" Text="Issue Flag" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlissueflag" runat="server" CssClass="input"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlissueflag_SelectedIndexChanged"
                        MaxLength="100" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <td>
        <asp:Label ID="lblcomment" runat="server" Text="Comment"></asp:Label>
        <asp:TextBox ID="txtNotesDescription" runat="server" CssClass="gridinput"
            Height="49px" TextMode="MultiLine"></asp:TextBox>
        <asp:Label ID="lblScript" runat="server" Text="Script Name" Visible="false"></asp:Label>
        <eluc:TabStrip ID="DefectTrackerScriptAdd" runat="server"></eluc:TabStrip>
    </td>
    <br />
    <br />
    <td colspan="6">
        <hr />
    </td>
    <div>
        <table id="tblscriptadd" runat="server" visible="false" border="0" cellpadding="1"
            cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
            width="99%">
            <tr>
                <td>
                    <asp:Literal ID="lblsubject" runat="server" Text="Subject"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtsubject" runat="server" CssClass="input_mandatory" Width="74%"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblcreatedby" runat="server" Text="Created By"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtcreatedby" runat="server" CssClass="input_mandatory"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblmodule" runat="server" Text="Module"></asp:Literal>
                </td>
                <td>
                    <eluc:Module ID="ucModule" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lbldeployon" runat="server" Text="Deploy on"></asp:Literal>
                </td>
                <td>
                    <asp:Panel ID="pnlDeploymentServer" runat="server">
                        <asp:CheckBoxList ID="chklstDeploymentServer" Width="75%" CssClass="input_mandatory"
                            runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnTextChanged="chklstDeploymentServer_TextChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblfilename" runat="server" Text="File Name"></asp:Literal>
                </td>
                <td>
                    <asp:FileUpload runat="server" ID="filePatchAttachment" Width="50%" CssClass="input_mandatory" />
                </td>
            </tr>
        </table>
    </div>
    <br />
    <asp:GridView ID="gvIssueTrack" runat="server" AutoGenerateColumns="False" Font-Size="11px"
        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvIssueTrack_RowDataBound">
        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
        <RowStyle Height="10px" />
        <Columns>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <HeaderTemplate>
                    Milestone
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lbltarget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMILESTONE") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <HeaderTemplate>
                    Issue Flag
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblflag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEFLAG") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <HeaderTemplate>
                    Modified Date
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblmodifieddate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <HeaderTemplate>
                    Comments
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblcomments" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDCOMMENT")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <HeaderTemplate>
                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                        width="3" />
                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAttachment"
                        ToolTip="Script Details"></asp:ImageButton>                    
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </form>
</body>
</html>
