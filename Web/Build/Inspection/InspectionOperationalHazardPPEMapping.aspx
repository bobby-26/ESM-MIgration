<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOperationalHazardPPEMapping.aspx.cs" Inherits="InspectionOperationalHazardPPEMapping" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document and PPE Mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionMapping" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
         <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand" Title="Equipment and PPE"></eluc:TabStrip>  
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status ID="ucStatus" runat="server"></eluc:Status>               
                <div id="divFind">
                    <table width="100%" cellspacing="2">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblelement" runat="server" Font-Bold="true" Text="Element"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtelement" runat="server" ReadOnly="true" Width="360px"
                                    CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAspect" runat="server" Font-Bold="true" Text="Aspect"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAspect" runat="server" ReadOnly="true" Width="360px"
                                    CssClass="readonlytextbox" TextMode="MultiLine" Rows="2" Resize="Both"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblHazard" runat="server" Font-Bold="true" Text="Hazard Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtHazard" runat="server" ReadOnly="true" Width="360px"
                                    CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSubHazard" runat="server" Font-Bold="true" Text="Hazard"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSubHazard" runat="server" Width="360px"
                                    CssClass="input_mandatory" TextMode="MultiLine" Rows="2" Resize="Both"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblengcontrol" runat="server" Font-Bold="true" Text="Equipment"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickListComponent">
                                    <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input"
                                        Width="90px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input"
                                       Width="270px"></telerik:RadTextBox>
                                    <asp:LinkButton ID="imgComponent" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                                    <asp:LinkButton ID="lnkComponentAdd" runat="server" OnClick="lnkComponentAdd_Click" ToolTip ="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                    <div id="divForms" runat="server" style="height:100px; overflow-y:auto; overflow-x:auto; width:360px; border-width:1px; border-style:solid; border:1px solid #c3cedd ">
                                        <table id="tblForms" runat="server">
                                    </table>
                                    </div>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOperational" runat="server" Font-Bold="true" Text="Operational"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtoperational" runat="server" Width="360px"
                                    CssClass="input_mandatory" TextMode="MultiLine" Rows="2" Resize="Both"></telerik:RadTextBox>
                            </td>
                        </tr>                                                           
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPPE" runat="server" Font-Bold="true" Text="PPE"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBoxList ID="cblRecomendedPPE" runat="server" Direction="Vertical" Columns="3" DataBindings-DataTextField="FLDNAME" DataBindings-DataValueField="FLDMISCELLANEOUSID" CssClass="input" BorderWidth="1px">
                                </telerik:RadCheckBoxList>
                            </td>
                        </tr>
                    </table>
                </div>
         </telerik:RadAjaxPanel>
    </form>
</body>
</html>
