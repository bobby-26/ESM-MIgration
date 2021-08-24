<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerPatchAddEdit.aspx.cs"
    Inherits="DefectTrackerPatchAddEdit" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.DefectTracker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPTeamMembers" Src="~/UserControls/UserControlSEPTeamMembers.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Module" Src="~/UserControls/UserControlSEPModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselList" Src="~/UserControls/UserControlVesselList.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patch Add/Edit</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <div class="subHeader">
            <div id="divHeading" class="divFloatLeft">
                <eluc:Title runat="server" ID="ucTitle" ShowMenu="false" Text="Patch Add/Edit"></eluc:Title>
            </div>
            <div style="position: absolute; top: 0px; right: 0px">
                <eluc:TabStrip ID="MenuPatchAdd" runat="server" TabStrip="true" OnTabStripCommand="MenuPatchAdd_TabStripCommand">
                </eluc:TabStrip>
            </div>
        </div>
        <div class="subHeader">
            <div style="position: absolute; right: 0px">
                <eluc:TabStrip ID="MenuPatchSave" runat="server" OnTabStripCommand="MenuPatchSave_TabStripCommand">
                </eluc:TabStrip>
            </div>
        </div>
        <eluc:Error ID="ucError" runat="server" Visible="false" />
    </div>
    <table width="100%">
        <tr>
            <td colspan="4">
                <font color="blue"><b>Patch Attachments</b>
                    <li>Upload attachment for the patch. Specify the subject. Browse and click on &nbsp;<img
                        id="Img5" runat="server" src="<%$ PhoenixTheme:images/upload.png%>" style="vertical-align: top" />&nbsp;
                        to upload one or more attachments. (OR) Specify, HTTP/FTP Link</li>
                </font>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal runat="server" ID="lblcatalogno" Text="Catalog Number/Title"></asp:Literal>
            </td>
            <td>
                <asp:Label runat="server" ID="lblCatalog" />
                <asp:Label runat="server" ID="lblTitle" Width="70%" />
            </td>
            <td>
                <asp:Literal runat="server" ID="lblcreatedby" Text="Created By"></asp:Literal>
            </td>
            <td>
                <eluc:SEPTeamMembers ID="txtCreatedby" AppendDataBoundItems="true" runat="server"
                    CssClass="input_mandatory"></eluc:SEPTeamMembers>
                <asp:DropDownList runat="server" ID="ddlPatchProject" AppendDataBoundItems="true"
                    CssClass="input" DataTextField="FLDPROJECT" DataValueField="FLDDTKEY">
                    <asp:ListItem Text="--Select--" Value="Dummy"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal runat="server" ID="lblsubject" Text="Subject"></asp:Literal>
            </td>
            <td width="35%">
                <asp:TextBox runat="server" ID="txtSubject" CssClass="input_mandatory" Width="90%" />
            </td>
            <td>
                <asp:Literal runat="server" ID="lblcallno" Text="Call Number / Date "></asp:Literal>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCallNumber" CssClass="input" ReadOnly="true" />
                /
                <eluc:Date runat="server" ID="ucCallDate" CssClass="input" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblinstruction" runat="server" Text="Instruction"></asp:Literal>
            </td>
            <td colspan="3" width="100%">
                <asp:TextBox ID="txtMessage" runat="server" ReadOnly="false" TextMode="MultiLine"
                    Rows="15" Columns="180" Width="80%" Height="100%" CssClass="input"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="4">
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblupload" runat="server" Text="Upload"></asp:Literal>
            </td>
            <td>
                <asp:FileUpload runat="server" ID="filPatchAttachment" CssClass="input" />
                <asp:ImageButton runat="server" ID="cmdUpload" ImageUrl="<%$ PhoenixTheme:images/upload.png %>"
                    ToolTip="Upload" OnClick="cmdUpload_Click" />
            </td>
            <td>
                <asp:Literal ID="lblvessel" runat="server" Text="Vessel"></asp:Literal>
            </td>
            <td rowspan="2">
                <asp:DropDownList ID="ddlCriteria" CssClass="input" runat="server">
                    <asp:ListItem Value="LIKE" Text="Like"></asp:ListItem>
                    <asp:ListItem Value="START" Text="Starts With"></asp:ListItem>
                    <asp:ListItem Value="END" Text="Ends With"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox runat="server" ID="txtFilter" CssClass="input" Width="50px"></asp:TextBox>
                <asp:Button runat="server" ID="cmdSelect" Text="V" CssClass="input" OnClick="cmdSelect_Click" />
                <asp:Button runat="server" ID="cmdClear" Text="X" CssClass="input" OnClick="cmdClear_Click" />
                <font color="blue">(Search and select vessels to send the patch)</font>
                <div id="divVessel" class="input_mandatory" style="height: 90px; width: 400px; overflow-y: auto">
                    <asp:CheckBoxList runat="server" ID="cblVessels" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" />
                    &nbsp;
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblftphttplink" runat="server" Text="FTP/HTTP Link"></asp:Literal>
            </td>
            <td colspan="3">
                <asp:TextBox runat="server" ID="txtLinkFile" CssClass="input" Width="35%" />
                <asp:ImageButton runat="server" ID="cmdUpdateLink" ImageUrl="<%$ PhoenixTheme:images/upload.png %>"
                    ToolTip="Upload" OnClick="cmdUpdateLink_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvPatchAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
        Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvPatchAttachment_RowDataBound"
        OnRowCommand="gvPatchAttachment_RowCommand" OnRowDeleting="gvPatchAttachment_RowDeleting"
        EnableViewState="false">
        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
        <RowStyle Height="10px" />
        <Columns>
            <asp:TemplateField HeaderText="File Name">
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <HeaderTemplate>
                    File Name
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'> </asp:Label>
                    <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'> </asp:Label>
                    <asp:Label ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'> </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Action">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <asp:HyperLink ID="lnkfilename" ImageUrl="<%$ PhoenixTheme:images/download_1.png %>"
                        Target="_blank" Text="View" runat="server" Width="14px" Height="14px" ToolTip="Download File">
                    </asp:HyperLink>&nbsp;
                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                        CommandName="EDITFILE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                        ToolTip="Edit"></asp:ImageButton>
                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                        ToolTip="Delete"></asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>
