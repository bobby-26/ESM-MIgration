<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAImpactDocumentPPEMapping.aspx.cs" Inherits="InspectionRAImpactDocumentPPEMapping" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardTypeExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Hazard Level / Impact</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
            <%: Scripts.Render("~/bundles/js") %>
            <%: Styles.Render("~/bundles/css") %>
            <style type="text/css">
            .checkboxstyle tbody tr td {
                width: 550px;
                vertical-align:top;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="tblhazard" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" EnableAJAX="false">
            <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand" Title="Details"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <table width="100%" cellspacing="2" runat="server" id="tblhazard">
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="lblCategory" runat="server"  Text="Category"></telerik:RadLabel>
                    </td>
                    <td width="80%">
                        <eluc:Hazard ID="ucCategory" runat="server" AppendDataBoundItems="true" Width="360px" AutoPostBack="false" />
                    </td>
                </tr>
                <tr id="hazard" runat="server" Visible="false" >
                    <td>
                        <telerik:RadLabel ID="lblHazard" runat="server"  Text="Immediate Cause"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlHazard" runat="server" Width ="360px" OnSelectedIndexChanged="ddlHazard_SelectedIndexChanged1"
                        AutoPostBack="true"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox> &nbsp&nbsp <telerik:RadLabel ID="lbl" runat="server"  Text="* The selected Immediate Cause will copied to Hazard Field, If it is not required leave as blank." BackColor="Yellow"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSubHazard" runat="server"  Text="Hazard"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubHazard" runat="server" Width="360px"
                             TextMode="MultiLine" Rows="2">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblImpact" runat="server"  Text="Impact"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlImpact" runat="server" AppendDataBoundItems="true" Width="360px"
                            EmptyMessage="Type to select " Filter="Contains" CssClass="input_mandatory" MarkFirstMatch="true" 
                            AutoPostBack="false" DataTextField="FLDIMPACTNAME" DataValueField="FLDRISKASSESSMENTIMPACTID" >                            
                        </telerik:RadComboBox>
                        <asp:CheckBoxList ID="chkimpacts" runat="server" RepeatDirection="Vertical" CssClass="checkboxstyle" RepeatColumns="3" DataTextField="FLDIMPACTNAME" DataValueField="FLDRISKASSESSMENTIMPACTID" RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="chkundesirableevent_SelectedIndexChanged" style="width:98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblUndesirable" runat="server"  Text="Contact Type / Undesirable Event"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chkundesirableevent" runat="server" RepeatDirection="Vertical" CssClass="checkboxstyle" RepeatColumns="3" DataTextField="FLDCONTACTTYPE" DataValueField="FLDCONTACTTYPEID" RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="chkundesirableevent_SelectedIndexChanged" style="width:98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblworstCase" runat="server"  Text="Worst Case"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chkworstcase" runat="server" CssClass="checkboxstyle" RepeatDirection="Vertical" RepeatColumns="3" DataTextField="FLDNAME" DataValueField="FLDHAZARDID" RepeatLayout="Table" AutoPostBack="false" style="width:98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPPE" runat="server"  Text="PPE"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="cblRecomendedPPE" runat="server" CssClass="checkboxstyle" RepeatDirection="Vertical" RepeatColumns="3" DataTextField="FLDNAME" DataValueField="FLDMISCELLANEOUSID" RepeatLayout="Table" AutoPostBack="false" style="width:98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

