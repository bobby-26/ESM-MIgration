<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantRegister.aspx.cs"
    Inherits="PreSeaNewApplicantRegister" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaQualificaiton" Src="~/UserControls/UserControlPreSeaQualification.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaCourse" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaritalStatus" Src="../UserControls/UserControlMaritalStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="../UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Sex" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

   </telerik:RadCodeBlock>
</head>
<body style="margin: 0; padding: 0px; text-align: center;">
    <div style="margin: 0 auto; width: 1024px; text-align: left;">
        <form id="form1" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
            runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Presea New Applicant" ShowMenu="false" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute; float: right;">
            <eluc:TabStrip ID="MenuPreSeaApplication" runat="server" OnTabStripCommand="PreSeaApplication_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
        </div>
        <asp:UpdatePanel ID="pnlOnlineApplication" runat="server">
            <ContentTemplate>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            Course
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCourse" DataTextField="FLDPRESEACOURSENAME" CssClass="dropdown_mandatory"
                                AutoPostBack="true" DataValueField="FLDPRESEACOURSEID" runat="server" 
                                OnSelectedIndexChanged="Course_Changed" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Applied Batch
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" DataTextField="FLDBATCH" DataValueField="FLDBATCHID"
                                CssClass="dropdown_mandatory" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Highest Qualification
                        </td>
                        <td>
                            <eluc:PreSeaQualificaiton ID="ucHighestQualificaiton" runat="server" CssClass="input_mandatory"
                                AppendDataBoundItems="true" Width="150px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            First Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstname" runat="server" CssClass="input_mandatory" MaxLength="200" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            Middle Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtMiddlename" runat="server" CssClass="input" MaxLength="200" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            Last Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastname" runat="server" CssClass="input" MaxLength="200" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date of Birth
                        </td>
                        <td>
                            <eluc:Date ID="txtDateofBirth" runat="server" CssClass="input_mandatory" DatePicker="true" Width="150px"/>
                        </td>
                        <td>
                            Gender
                        </td>
                        <td>
                            <eluc:Sex ID="ucGender" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="150px"
                                HardList='<%# PhoenixRegistersHard.ListHard(1,(int)PhoenixHardTypeCode.SEX)%>' />
                        </td>
                        <td>
                            Nationality
                        </td>
                        <td>
                            <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"  Width="150px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            EMail
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" CssClass="input_mandatory" runat="server" MaxLength="200"
                                Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            Contact No
                        </td>
                        <td>
                            <eluc:PhoneNumber ID="txtContact" runat="server" CssClass="input_mandatory" Width="100px"/>
                        </td>
                        <td>
                            Height(Cms)
                        </td>
                        <td>
                            <eluc:Number ID="txtHeight" runat="server" CssClass="input_mandatory txtNumber" MaxLength="5"
                                IsInteger="true" AutoPostBack="true" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Weight(Kg)
                        </td>
                        <td>
                            <eluc:Number ID="txtWeight" runat="server" CssClass="input_mandatory txtNumber" MaxLength="6"
                                IsInteger="true" AutoPostBack="true"  Width="150px"/>
                        </td>
                        <td>
                            Eye Sight
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEyeSight" runat="server" CssClass="input_mandatory" Width="150px">
                                <asp:ListItem Text="Normal" Value="1"> </asp:ListItem>
                                <asp:ListItem Text="Short-Sighted" Value="2"> </asp:ListItem>
                                <asp:ListItem Text="Long-Sighted" Value="3"> </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Colour Blindness
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlColourBlindness" runat="server" CssClass="input_mandatory" Width="150px">
                                 <asp:ListItem Text="-select-"> </asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                                <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <font color="blue">(Use "," to add more E-Mail Eg : (xx@xx.com,yy@yy.com)</font>
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
        </form>
    </div>
</body>
</html>
