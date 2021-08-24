<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaEnquiryQueryAboutCourse.aspx.cs"
    Inherits="PreSeaEnquiryQueryAboutCourse" %>

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
                    <br />
                    <br />
                    Enquiry /Query about courses
                </td>
                <td style="width: 20%">
                    &nbsp;
                </td>
            </tr>
        </table>
        <br style="clear: both;" />
        <hr />
        <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
        <eluc:status runat="server" id="ucStatus" />
        <table>
            <tr>
                <td>
                    <font color="blue"><b>
                        <asp:Literal ID="ltrNote" runat="server" Text="Note: Please read the FAQ before submitting your queries."></asp:Literal></b></font>
                </td>
            </tr>
            <tr>
                <td>
                    <font color="blue"><b>
                        <asp:Literal ID="ltrMsg" runat="server" Text="All enquiries related to Merchant Navy career to be submitted down below to our admission team."></asp:Literal>
                        <br />
                        <asp:Literal ID="ltrMsg1" runat="server" Text="We will be reverting with the answers to your queries soon on your email id or by phone."></asp:Literal></b></font>
                    <br />
                    <br />
                    <b><font color="red">*</font></b> <font color="blue"><b>
                        <asp:Literal ID="ltrMandatory" Text="- Mandatory" runat="server"></asp:Literal></b>
                    </font>
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellpadding="2">
            <tr>
                <td>
                    <asp:Literal ID="lblCourse" runat="server" Text="Course :"></asp:Literal><b><font
                        color="red">*</font></b>
                </td>
                <td>
                    <eluc:preseacourse id="ucCourse" runat="server" cssclass="input" appenddatabounditems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblName" runat="server" Text="Name :"></asp:Literal></asp:Literal><b><font
                        color="red">*</font></b>
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" CssClass="input" Width="180"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblDateofbirth" runat="server" Text="Date of Birth(DOB) :"></asp:Literal></asp:Literal><b><font
                        color="red">*</font></b>
                </td>
                <td>
                    <eluc:date id="ucDOB" runat="server" datepicker="true" cssclass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblContactNo" runat="server" Text="Contact No :"></asp:Literal></asp:Literal><b><font
                        color="red">*</font></b>
                </td>
                <td>
                    <eluc:phonenumber id="txtContact" runat="server" cssclass="input" width="130" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltrMobileNo" runat="server" Text="Mobile Number :"></asp:Literal></asp:Literal><b><font
                        color="red">*</font></b>
                </td>
                <td>
                    <eluc:number id="txtMobileNo" width="180" runat="server" cssclass="input" isinteger="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltrEmail" runat="server" Text="Email Address :"></asp:Literal></asp:Literal><b><font
                        color="red">*</font></b>
                </td>
                <td>
                    <asp:TextBox ID="txtEmailId" runat="server" CssClass="input" Width="180"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltrQuery" runat="server" Text="Questions / Comments :"></asp:Literal></asp:Literal><b><font
                        color="red">*</font></b>
                </td>
                <td>
                    <asp:TextBox ID="txtQueryRemarks" runat="server" CssClass="input" Height="130px"
                        Rows="50" TextMode="MultiLine" Width="180%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="cntxMenuSelect" />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                        CssClass="cntxMenuSelect" />
                    <asp:Button ID="btnHome" runat="server" Text="Home" OnClick="btnHome_Click" CssClass="cntxMenuSelect" />
                </td>
            </tr>
        </table>
        <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
    </div>
    </form>
</body>
</html>
