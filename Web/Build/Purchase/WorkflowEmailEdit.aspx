<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowEmailEdit.aspx.cs" Inherits="WorkflowEmailEdit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Email Edit</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
               <eluc:TabStrip ID="MenuWorkEmailEdit" runat="server" OnTabStripCommand="MenuWorkEmailEdit_TabStripCommand" />
              <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <table style="margin-left: 20px" >            
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Activity"  ></telerik:RadLabel>
                    </td>
                    <td>
                       <telerik:RadLabel ID="lblActivity" runat="server" Text="" Width="120px">    </telerik:RadLabel>
                        <telerik:RadLabel ID="lblProcessTransitionActivity" runat="server" Text="" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>

                  <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Description" ></telerik:RadLabel>
                    </td>
                    <td>
                       <telerik:RadTextBox ID="lblDescription" runat="server" Text=""  Width="240px" TextMode="MultiLine" Rows="6">    </telerik:RadTextBox>
                    </td>
                </tr>

                  <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="To" ></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmailId" runat="server" Text="" Visible="false"></telerik:RadLabel>
                       <telerik:RadTextBox ID="lblTo" runat="server" Text=""  Width="180px" >    </telerik:RadTextBox>
                    </td>
                </tr>

                 <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Cc" ></telerik:RadLabel>
                    </td>
                    <td>
                       <telerik:RadTextBox ID="lblCC" runat="server" Text=""  Width="180px" >    </telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="Subject" ></telerik:RadLabel>
                    </td>
                    <td>
                       <telerik:RadTextBox ID="lblSubject" runat="server" Text="" Width="850px" >    </telerik:RadTextBox>
                    </td>
                </tr>

                 
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="Body"></telerik:RadLabel>
                    </td>
                    <td>
                       <telerik:RadTextBox ID="lblBody" runat="server" Text=""  Width="850px" TextMode="MultiLine" Rows="10" Resize="Both" >    </telerik:RadTextBox>
                    </td>
                </tr>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
