<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCSubCategoryAdd.aspx.cs"
    Inherits="InspectionMOCSubCategoryAdd" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Questions Add</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:radcodeblock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />

        <telerik:radajaxpanel id="RadAjaxPanel1" runat="server" loadingpanelid="RadAjaxLoadingPanel1" height="100%">
        
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        
            <eluc:TabStrip ID="MenuRegistersSubCategoryAdd" runat="server" OnTabStripCommand="MenuRegistersSubCategoryAdd_TabStripCommand"   TabStrip="true" />
            <br />
        <table runat="server" >
            <tr>
           <td style="padding-right:40px">
               <telerik:RadLabel runat="server" Id="lblcode"  Text="Code"   ></telerik:RadLabel>

           </td>
                
                 <td style="padding-right:40px">

                <telerik:RadTextBox  ID="txtShortCodeEdit" runat="server" MaxLength="3"   Width="270px"   CssClass="gridinput_mandatory" Visible="true"></telerik:RadTextBox>
            
         </td>
                </tr>
                <tr>
           <td style="padding-right:40px">
                <telerik:RadLabel runat="server" Id="lblname"  Text="Name"   ></telerik:RadLabel></td>
                    
                    <td style="padding-right:40px">
                <telerik:RadTextBox ID="txtCategoryEdit" runat="server" TextMode="MultiLine" Resize="Both" Width="270px" Rows="3"   CssClass="gridinput_mandatory"   >         </telerik:RadTextBox>
               </td> 
                
            </tr>    
          
            </table>  
          

        <eluc:Status runat="server" ID="ucStatus" />
    </telerik:radajaxpanel>
    </form>
</body>
</html>
