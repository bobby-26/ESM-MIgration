<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionQuestionAdd.aspx.cs" Inherits="InspectionQuestionAdd" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>       
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuInspectionQuestions" runat="server" OnTabStripCommand="MenuInspectionQuestions_TabStripCommand"></eluc:TabStrip>
           <table>
               <tr>
                   <td Width="20%">
                       <telerik:RadLabel ID="lblQuestion" runat="server" Text="Question"></telerik:RadLabel>
                   </td>
                   <td rowspan="2" Width="50%" >
                       <telerik:RadTextBox ID="txtQuestion" runat="server" TextMode="MultiLine" Resize="Both" Width="300px" Height="70px"></telerik:RadTextBox>
                   </td>
               </tr>
               <tr>
                   </tr>
               <tr>
                   <td>
                       <telerik:RadLabel ID="lblSortorder" runat="server" Text ="Sort Order"></telerik:RadLabel>
                   </td>
                   <td>
                       <telerik:RadTextBox ID="txtSortorder" runat="server" Width="100px" InputType="Number" MaxLength="2" ></telerik:RadTextBox>
                   </td>
               </tr>
               <tr>
                   <td>
                       <telerik:RadLabel ID="lblActive"  runat="server" Text="ActiveYN"></telerik:RadLabel>
                   </td>
                   <td>
                       <telerik:RadCheckBox ID="cbActive" runat="server" ></telerik:RadCheckBox>
                   </td>
               </tr>
           </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>


