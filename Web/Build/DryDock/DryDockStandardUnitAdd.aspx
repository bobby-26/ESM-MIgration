<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockStandardUnitAdd.aspx.cs" Inherits="DryDockStandardUnitAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Responsibilty" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="rad1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript">
            function fnJobEdit(jobid) {
                location.href = 'DryDockStandardUnit.aspx?StandardUnitID=' + jobid;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStandardUnitAdd" runat="server" autocomplete="off">
    <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server" ></telerik:RadSkinManager>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
              
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
              
              
                <div  style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div  style="font-weight: 600; font-size: 12px;" runat="server">
                  
                        <eluc:TabStrip ID="MenuStandardUnitsSpecification" runat="server" OnTabStripCommand="StandardUnitsSpecification_TabStripCommand">
                        </eluc:TabStrip>
                   
                </div>
                <table width="100%" cellpadding="1" cellspacing="3">             
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtNumber" CssClass="input_mandatory" MaxLength="10" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="100"
                                Width="360px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                           <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>
                            
                        </td>
                        <td colspan="5">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtJobDescription" runat="server" CssClass="input_mandatory" Width="60%"
                                TextMode="MultiLine" Rows="6"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                
                </div>
                 <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:Status ID="ucStatus" runat="server" />
           
 
    </form>
</body>
</html>
