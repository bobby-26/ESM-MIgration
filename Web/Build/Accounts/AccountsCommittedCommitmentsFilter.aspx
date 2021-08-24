<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCommittedCommitmentsFilter.aspx.cs" Inherits="AccountsCommittedCommitmentsFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselAccount" Src="~/UserControls/UserControlVesselAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>  <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

      
    </telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    
 
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
        </eluc:TabStrip>
  
  
 <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
   <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <div id="divFind">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselAccountCode" runat="server" Text="Vessel Account Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:VesselAccount ID="ucVesselAccount" runat="server" AppendDataBoundItems="true" CssClass="input" ></eluc:VesselAccount>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPoNumber" runat="server" Text="Po Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPoNumber" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCommittedAsOnDate" runat="server" Text="Committed As On Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucCommittedDate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblHideReversedExcludePOsAsOnCommittedDate" runat="server" Text="Hide Reversed/Exclude POs As On Committed Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkReversed" runat="server"/>
                        </td>
                    </tr>
                </table>
            </div>
  
    </form>
</body>
</html>
