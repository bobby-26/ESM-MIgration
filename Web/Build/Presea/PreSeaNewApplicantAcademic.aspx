<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantAcademic.aspx.cs"
    Inherits="PreSeaNewApplicantAcademic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonAddress" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Sex" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ECNR" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Relation" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="../UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaQualificaiton" Src="~/UserControls/UserControlPreSeaQualification.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Institution" Src="~/UserControls/UserControlPreSeaMultiColAddress.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>NewApplicant Academic</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaNewApplicantAcademic" runat="server">
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>

        <asp:UpdatePanel runat="server" ID="pnlPreSeaNewApplicantAcademic">
            <ContentTemplate>
                <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="ucStatus" />
                    <div class="subHeader">
                        <eluc:Title runat="server" ID="Academic" Text="NewApplicant Academic Details" ShowMenu="false"></eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuPreSeaAcademic" runat="server" OnTabStripCommand="PreSeaAcademic_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div>
                        <table cellpadding="3" cellspacing="1" width="90%">
                            <tr>
                                <td>Qualification
                                </td>
                                <td>
                                    <eluc:PreSeaQualificaiton ID="ddlCertificate" runat="server" CssClass="input_mandatory"
                                        AppendDataBoundItems="true"  Width="200"/>
                                </td>
                            </tr>
                            <tr>
                                <td>Board
                                </td>
                                <td>
                                    <eluc:Quick ID="ucAcademicBoard" runat="server" CssClass="dropdown_mandatory" QuickTypeCode="101"
                                      Width="200"  AppendDataBoundItems="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>Institution
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlInstitute" runat="server"  Width="200"
                                        CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlInstitute_SelectedIndexChanged"></asp:DropDownList>
                                    <%--<eluc:Institution ID="ucInstitution" runat="server" CssClass="input_mandatory" 
                                        AutoPostBack="true" OnTextChangedEvent="ucInstitution_TextChangedEvent"   />--%>
                                </td>
                            </tr>
                            <tr id="univ" runat="server">
                                <td>University
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUniversity" runat="server" CssClass="input" MaxLength="200" Width="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="rollno" runat="server">
                                <td>Exam Rollno
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExamRollno" runat="server" CssClass="input" MaxLength="200" Width="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Years of Education : from
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYearFrom" runat="server" CssClass="dropdown_mandatory">
                                    </asp:DropDownList>
                                    <asp:Literal ID="lblDash" Text="To" runat="server"></asp:Literal>
                                    <asp:DropDownList ID="ddlYearTo" runat="server" CssClass="dropdown_mandatory">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                               <%-- <td>To
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYearTo" runat="server" CssClass="dropdown_mandatory">
                                    </asp:DropDownList>
                                </td>--%>
                                <td>Year of passing
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYearPass" runat="server" CssClass="dropdown_mandatory">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>First Attempt
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFirstAttemptYN" runat="server" CssClass="input">
                                        <asp:ListItem Text="-select-" Value="Dummy"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>Still awaiting for the examination result :
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkResultYN" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Institution Address</b>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="4" cellspacing="1" width="90%">
                            <tr>
                                <td colspan="3" rowspan="4">
                                    <eluc:CommonAddress ID="ucAddress" runat="server" />
                            </tr>
                        </table>
                        <br />
                        <br />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
XFGDF