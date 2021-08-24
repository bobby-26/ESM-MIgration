<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogDeckLogBook.aspx.cs" Inherits="Log_ElectricLogDeckLogBook" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        body {
            font-size: 16px;
        }

        .center {
            margin: 0 auto;
            text-align: center;
        }

        .border-container {
            border: 3px solid black;
            width: 700px;
        }

        .underline {
            text-decoration: underline;
        }

        table {
            padding: 10px 10px;
        }
    </style>
</head>
<body>
  <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <div class="center">
            <h1>
                <telerik:RadLabel runat="server" ID="lblCompanyName"></telerik:RadLabel>
            </h1>
            <div class="border-container center">
                <h2>Deck Log Book</h2>
                OF THE
               <table class="center">
                   <tr>
                       <td>
                           <telerik:RadLabel runat="server" ID="lblVesselTitle" Text="MV/MT : "></telerik:RadLabel>
                       </td>
                       <td colspan="3">
                           <telerik:RadLabel runat="server" ID="lblVesselName" CssClass="underline"></telerik:RadLabel>
                       </td>
                   </tr>
                   <tr>
                       <td>
                           <telerik:RadLabel runat="server" ID="lblPortTitle" Text="P.O.R :"></telerik:RadLabel>
                       </td>
                       <td>
                           <telerik:RadLabel runat="server" ID="lblPortName" CssClass="underline"></telerik:RadLabel>
                       </td>

                   </tr>
                   <tr>
                       <td>
                           <telerik:RadLabel runat="server" ID="lblIMOTitle" Text="IMO NO :"></telerik:RadLabel>
                       </td>
                       <td>
                           <telerik:RadLabel runat="server" ID="lblIMO" CssClass="underline"></telerik:RadLabel>
                       </td>
                   </tr>
                 <%--  <tr>
                       <td>
                           <telerik:RadLabel runat="server" ID="lblMasterTitle" Text="Master's Name:"></telerik:RadLabel>
                       </td>
                       <td colspan="3">
                           <telerik:RadLabel runat="server" ID="lblMasterName" CssClass="underline"></telerik:RadLabel>
                       </td>
                   </tr>--%>
                   <tr>
                       <td>
                           <telerik:RadLabel runat="server" ID="lblWorkingLanguageTitle" Text="Working Language:"></telerik:RadLabel>
                       </td>
                       <td colspan="3">
                           <telerik:RadLabel runat="server" ID="lblWorkingLanguage" CssClass="underline"></telerik:RadLabel>
                       </td>
                   </tr>
                   <tr>
                       <td>
                           <telerik:RadLabel runat="server" ID="lblCommencedDateTitle" Text="COMMENCED DATE:"></telerik:RadLabel>
                       </td>
                       <td>
                           <%--<eluc:Date runat="server" ID="dtCommenced" CssClass="underline" DateTimeFormat="dd-MMM-yyyy"/> --%>
                           <telerik:RadDatePicker ID="dtCommenced" RenderMode="Lightweight" runat="server" CssClass="underline" MaxLength="12">
                               <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                           </telerik:RadDatePicker>
                       </td>
                   </tr>
                   <tr>
                       <td>
                           <telerik:RadLabel runat="server" ID="lblCompletedDateTitle" Text="COMPLETED DATE:"></telerik:RadLabel>
                       </td>
                       <td>
                           <%-- <eluc:Date runat="server" ID="dtCompleted" CssClass="underline" DateTimeFormat="dd-MMM-yyyy"/> --%>
                           <telerik:RadDatePicker ID="dtCompleted" RenderMode="Lightweight" runat="server" CssClass="underline" MaxLength="12">
                               <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                           </telerik:RadDatePicker>

                       </td>
                   </tr>
               </table>
            </div>
        </div>

    </form>
</body>
</html>
