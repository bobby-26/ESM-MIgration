<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionHazardControlMapping.aspx.cs" Inherits="InspectionHazardControlMapping" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Undesirable Event / Worst case Control Mapping</title>
    <div id="InspectionMapping" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </div>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="divFind" DecoratedControls="All" EnableRoundedCorners="true" />
         <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand" Title="Control Mapping"></eluc:TabStrip>  
             <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                <div id="divFind">
                    <table width="100%" cellspacing="2">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblevent" runat="server" Text="Contact Type / Undesirable Event"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtevent" runat="server" ReadOnly="true" Width="360px"
                                    CssClass="readonlytextbox" ></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblhazard" runat="server" Text="Worst Case"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txthazard" runat="server" ReadOnly="true" Width="360px"
                                    CssClass="readonlytextbox" ></telerik:RadTextBox>
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblRiskofEscalation" runat="server" Text="Risk of Escalation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtRiskofEscalation" runat="server" Width="360px"
                                    CssClass="input_mandatory" TextMode="MultiLine" Rows="2" Resize="Both"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblengcontrol" runat="server" Text="Equipment"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickListComponent">
                                    <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input"
                                        Width="90px"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input"
                                       Width="270px"></telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowComponents" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                                    <asp:LinkButton ID="lnkComponentAdd" runat="server" OnClick="lnkComponentAdd_Click" ToolTip ="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                    <div id="divComponents" runat="server" style="height:100px; overflow-y:auto; overflow-x:auto; width:360px; border-width:1px; border-style:solid; border:1px solid #c3cedd ">
                                        <table id="tblcomponents" runat="server">
                                    </table>
                                </div>
                                </span>
                            </td>
                        </tr>                       
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblprocedure" runat="server" Text="Procedures, Forms and Checklists"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickListDocument">
                                    <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="363px" style="font-weight:bold"
                                        CssClass="input"></telerik:RadTextBox>
                                    <asp:LinkButton ID="btnShowDocuments" runat="server" ToolTip="Select Procedures, Forms and Checklists">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                                <asp:LinkButton ID="lnkFormAdd" runat="server" OnClick="lnkFormAdd_Click" ToolTip="Add">
                                    <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                </asp:LinkButton>
                                <br />
                                <div id="divForms" runat="server" style="height:100px; overflow-y:auto; overflow-x:auto; width:360px; border-width:1px; border-style:solid; border:1px solid #c3cedd ">
                                    <table id="tblForms" runat="server">
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPPE" runat="server" Text="PPE"></telerik:RadLabel>
                            </td>
                             <td>
                                <asp:CheckBoxList ID="cblRecomendedPPE" runat="server" RepeatDirection="Vertical" RepeatColumns="3" DataTextField="FLDNAME" DataValueField="FLDMISCELLANEOUSID" CssClass="input" BorderWidth="1px">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>


