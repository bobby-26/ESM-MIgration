<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersContractingCompanyAdd.aspx.cs" Inherits="RegistersContractingCompanyAdd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew In-Active</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />        
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuAppraisal" runat="server" OnTabStripCommand="MenuAppraisal_TabStripCommand" Title="Contracting Company"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>Code</td>
                    <td>
                        <telerik:RadTextBox ID="txtShortCode" runat="server" CssClass="gridinput_mandatory"
                            MaxLength="6" Text="" ToolTip="Enter Shortcode" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                    <td>Company
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCompanyName" runat="server" Text=""
                            CssClass="gridinput_mandatory" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Company Header
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtheadercompany" runat="server" Text=""
                            CssClass="input" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                    <td>Company Header  Address 
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtHeaderAddress" runat="server" Text=""
                            CssClass="input" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Contracting Party
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContractingParty" runat="server" Text=""
                            CssClass="input" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                    <td>Contracting Party Address 
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContractingPartyAddress" runat="server" Text=""
                            CssClass="input" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>Agent Description
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAgentDescription" runat="server" Text=""
                            CssClass="input" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                    <td>Place Required
                    </td>
                    <td>
                        <telerik:RadRadioButtonList runat="server" ID="rdbPlaceYn" RepeatDirection="Horizontal" AutoPostBack="false">
                            <Items>
                                <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="No" Value="0"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>

                </tr>
                <tr>
                    <td>Seafarer Signature
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeafarerSignature" runat="server" Text=""
                            CssClass="input" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                    <td>MLC Address</td>
                    <td>
                        <telerik:RadTextBox ID="txtMLCcompany" runat="server" Text=""
                            CssClass="input" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadTextBox ID="txtSeafarerSignature2" runat="server" Text=""
                            CssClass="input" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadTextBox ID="txtMLCcompany1" runat="server" Text=""
                            CssClass="input" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>Version
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtversion" runat="server" Text=""
                            CssClass="input" MaxLength="50" Width="250px">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadTextBox ID="txtMLCcompany2" runat="server" Text=""
                            CssClass="input" MaxLength="200" Width="250px">
                        </telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>Vessel Details Required
                    </td>
                    <td>
                        <telerik:RadRadioButtonList runat="server" ID="rbvesselDetailsRequired" RepeatDirection="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="No" Value="0"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td>Addendum Required
                    </td>
                    <td>
                        <telerik:RadRadioButtonList runat="server" ID="rbAddendumRequired" RepeatDirection="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="No" Value="0"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>Company Signature</td>
                </tr>
                <tr>
                    <td>
                         <telerik:RadEditor ID="txtCompanySignature" runat="server" Width="99%" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            </telerik:RadEditor>
                      <%--  <cc1:Editor ID="txtCompanySignature" runat="server" Width="100%" Height="30px" CssClass="readonlytextbox" />--%>
                        </td>
                </tr>
                <tr>
                    <td colspan="4">Terms &amp; Conditions</td>
                </tr>
                <tr>
                    <td colspan="4" width="100%">
                         <telerik:RadEditor ID="edtBody" runat="server" Width="99%" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            </telerik:RadEditor>
                      <%--  <cc1:Editor ID="edtBody" runat="server" CssClass="readonlytextbox" Height="60px" Width="100%" />--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">Addendum to Contract</td>
                </tr>
                <tr>
                    <td colspan="4" width="100%">
                         <telerik:RadEditor ID="edtAddendum" runat="server" Width="99%" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            </telerik:RadEditor>
                       <%-- <cc1:Editor ID="edtAddendum" runat="server" CssClass="readonlytextbox" Height="60px" Width="100%" />--%>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
