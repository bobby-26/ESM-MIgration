<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagenetTimePicker.aspx.cs" Inherits="DocumentManagement_DocumentManagenetTimePicker" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
           <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
             <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           <table>
           <tr>

           <td>
                Date
           </td>
           <td>
                <eluc:Date ID="ucPublishedDateEdit" runat="server" CssClass="gridinput_mandatory"/>
                                      <%-- Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPUBLISHEDDATE")) %>' />--%>
           </td>
           </tr>
           <tr> <td> &nbsp;
           </td>
           </tr>
         <tr>
           <td>
                <telerik:RadButton ID="btnsubmit" runat="server" Text="Approve" OnClick="Onclick_approve"></telerik:RadButton>
           </td>
          <%-- <td>
              <telerik:RadButton ID="btnno" runat="server" Text="Cancel"  OnClick="Onclick_cancel"></telerik:RadButton>
           </td>--%>
           </tr>
           
           </table>

           </telerik:RadAjaxPanel>
    </form>
</body>
</html>
