<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantOthers.aspx.cs"
    Inherits="PreSeaNewApplicantOthers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title>PreSea NewApplicant Others</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaOthers" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader">
            <eluc:Title runat="server" ID="ttlAboutUsBy" Text="Applicant Others" ShowMenu="false">
            </eluc:Title>
            <br />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="PreSeaOthersMain" runat="server" OnTabStripCommand="PreSeaOthersMain_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div>
            <table width="90%">
                <tr>
                    <td>
                        First Name
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        Middle Name
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        Last Name
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        Batch
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtBatch" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    Exam Venue
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" GroupingText="First Choice" Width="1000">
                        <asp:RadioButtonList ID="rblExamVenueFirst" runat="server" RepeatDirection="Horizontal" DataTextField="FLDEXAMVENUENAME" DataValueField="FLDEXAMVENUEID"
                            RepeatColumns="5" RepeatLayout="Table">
                        </asp:RadioButtonList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server" GroupingText="Second Choice" Width="1000">
                        <asp:RadioButtonList ID="rblExamVenueSecond" runat="server" RepeatDirection="Horizontal"  DataTextField="FLDEXAMVENUENAME" DataValueField="FLDEXAMVENUEID"
                            RepeatColumns="5" RepeatLayout="Table">
                        </asp:RadioButtonList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    Any other information you want to tell us about yourself?
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtAboutYourselfRemarks" runat="server" TextMode="MultiLine" CssClass="input"
                        Width="400" Height="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel3" runat="server" GroupingText="How did you know about Samundra Institute of Maritime Studies"
                        Width="1000">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkNewspaperMagazine" runat="server" Text="Newspaper/Magazine" />
                                </td>
                                <td>
                                    <eluc:Quick ID="ucNewspaperMagazine" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkFamilyRelativeFriends" runat="server" Text="Family/Relatives/Friends" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkFlyers" runat="server" Text=" Flyers" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkSchoolCollege" runat="server" Text="School/College" />
                                </td>
                                <td>
                                    <eluc:Quick ID="ucSchoolCollage" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkEducationJoFfair" runat="server" Text="Education/Job Fair" />
                                </td>
                                <td>
                                    State:
                                    <eluc:State ID="ucState" CssClass="input" runat="server" AppendDataBoundItems="true"
                                        AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" />
                                </td>
                                <td>
                                    Place:
                                    <eluc:City ID="ddlPlace" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                </td>
                                <td id="country" runat="server">
                                    <eluc:Country runat="server" ID="ucCountry" AutoPostBack="true" AppendDataBoundItems="true"
                                        CssClass="input" OnTextChangedEvent="ucCountry_TextChanged" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkEmailBySims" runat="server" Text="Emails sent by SIMS" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkShiksha" runat="server" Text="www.Shiksha.com" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkInternet" runat="server" Text="Internet" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInternet" runat="server" CssClass="input" Width="300"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkDirectContact" runat="server" Text="Directly Contacted" />
                                </td>
                                <td>
                                    <eluc:Quick ID="ucDirectContact" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkOthers" runat="server" Text="Others,please provide details" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOthers" runat="server" CssClass="input" Width="300"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
