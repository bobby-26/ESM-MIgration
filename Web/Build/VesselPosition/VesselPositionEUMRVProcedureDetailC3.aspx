<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVProcedureDetailC3.aspx.cs" Inherits="VesselPositionEUMRVProcedureDetailC3" %>

<%@ Import Namespace="SouthNests.Phoenix.VesselPosition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVPRSLocation" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVPRSLocation" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlVPRSLocation">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="TabProcedure" runat="server" OnTabStripCommand="TabProcedure_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td style="width: 30%">
                        <telerik:RadLabel runat="server" ID="lblprocedure" Text="Procedure"></telerik:RadLabel>
                    </td>
                    <td style="width: 70%">
                        <telerik:RadComboBox runat="server" Visible="false" ID="ddlProcedure" CssClass="dropdown_mandatory" Width="70%"></telerik:RadComboBox>
                        <b>
                            <telerik:RadLabel ID="lblProceduretxt" runat="server"></telerik:RadLabel></b>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblreference" Text="Reference To Existing Procedure"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--  <a id="link" href="#" class="applinks"> <hyperlink id="txtDocumentNameEdit" runat="server"></hyperlink></a> --%>
                        <span id="spnPickListDocument">
                            <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="260px" Enabled="false" CssClass="input" AutoPostBack="true" OnDataBinding="txtDocumentName_OnTextChanged"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" AlternateText=".." ID="btnShowDocuments" ToolTip="DMS Link">
                             <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <telerik:RadTextBox ID="txtReferencetoExisting" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Visible="false"
                            Width="70%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblVersion" Text="Version of Existing Procedure"></telerik:RadLabel>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel runat="server" ID="txtVersion" Text="0"></telerik:RadLabel></b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lbleuprocedure" Text="Description of EUMRV Procedure"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txteuprocedure" runat="server" CssClass="input" Height="70px" TextMode="MultiLine"
                            Width="70%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblpersonreponsible" Text="Name of the Person Responsible"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtxpersonreponsible" CssClass="input" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblDatatasource" Text="Data source"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtDatasource" CssClass="input" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lbllocation" Text="Location where records are kept"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtlocation" CssClass="input" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblSystemUsed" Text="Name of the IT System Used"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSystemUsed" CssClass="input" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
