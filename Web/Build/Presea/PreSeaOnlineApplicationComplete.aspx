<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaOnlineApplicationComplete.aspx.cs"
    Inherits="PreSeaOnlineApplicationComplete" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaCourse" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PreSea Enquiry/Query About Course</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <title>
            <%=Application["softwarename"].ToString() %>
            -
            <%=Session["companyname"]%></title>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <div style="overflow: hidden; position: relative">
        <table width="100%" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" style="width: 20%">
                    <img id="img2" runat="server" alt="" src="<%$ PhoenixTheme:images/sims.png %>" />
                </td>
                <td style="font-size: medium; font-weight: bold; width: 60%;" align="center">
                    SAMUNDRA INSTITUTE OF MARITIME STUDIES
                   
                </td>
                <td style="width: 20%">
                    &nbsp;
                </td>
            </tr>
        </table>
        <br style="clear: both;" />
        <hr /> 
        <center>
        <table>
            <tr>
                <td>
                    <font color="blue" style="font-size:14"><b>
                        <asp:Literal ID="ltrNote" runat="server" Text="Your Application has been Successfully Registered. Please find your Application Form in your Mail."></asp:Literal></b></font>
                </td>
            </tr>
        </table>
        </center>
      
    </div>
    </form>
</body>
</html>
