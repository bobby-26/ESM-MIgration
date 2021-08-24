<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSummary.aspx.cs" Inherits="CrewSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Summary</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewSummary" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuCompanySummary" runat="server" Title="Crew Summary"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>            
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td runat="server" id="tdempno">
                        <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td runat="server" id="tdempnum">
                        <telerik:RadTextBox ID="txtEmployeeNumber" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <div id="divFind">
                <table id="tblConfigureDocumentsRequired">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlExperience" GroupingText="Experience">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbCompany" Checked="true" AutoPostBack="true"
                                                            GroupName="rbExperience" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbFull" AutoPostBack="true" GroupName="rbExperience" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblfull" runat="server" Text="Full"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlRankType" runat="server" GroupingText="Rank Exp Type">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rblMonths" Checked="true" AutoPostBack="true"
                                                            GroupName="rblRank" />                                                       
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblExpinMonth" runat="server" Text="Exp in Months"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rblDays" AutoPostBack="true" GroupName="rblRank" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblExpinDecimal" runat="server" Text="Exp in Decimal"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlXX">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbVesselTypeRank" AutoPostBack="true" Checked="true"
                                                            GroupName="rbExperienceType" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblVesselTypeRank" runat="server" Text="Vessel Type - Rank"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbCompanyRank" AutoPostBack="true" GroupName="rbExperienceType" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblCompanyRank" runat="server" Text="Company - Rank"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbEngineTypeRank" AutoPostBack="true" GroupName="rbExperienceType" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblEnginetype" runat="server" Text="Engine Type - Rank"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbPrincipalRank" AutoPostBack="true" GroupName="rbExperienceType" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblPrincipalRank" runat="server" Text="Principal - Rank"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton runat="server" ID="rbOwnerRank" AutoPostBack="true" GroupName="rbExperienceType" />
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="lblOwnerRank" runat="server" Text="Owner - Rank"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <table border="0" width="100%" style="font-size: 11px; width: 99%; border-collapse: collapse;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblExpInMonths" runat="server" Font-Bold="true" Font-Size="Small"
                            Text="*Experience In Months and Days">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" >
                        <telerik:RadLabel ID="ltGrid" runat="server" Text="" Width="100%"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label runat="server" ID="lblSTSSIREHead" Font-Bold="true" Font-Size="Small"
                Text="*STS and SIRE Count"></asp:Label>
            <table width="100%" cellspacing="0" cellpadding="3" rules="all" border="1" style="font-size: 11px; width: 99%; border-collapse: collapse;">
                <thead>
                    <th style="width: 50%">
                        <telerik:RadLabel ID="ltMeasureHeader" runat="server" Text=""></telerik:RadLabel>
                        <th style="width: 50%">
                            <telerik:RadLabel ID="ltCountHeader" runat="server" Text="Count"></telerik:RadLabel>
                        </th>
                </thead>
                <tr>
                    <td>
                        <telerik:RadLabel ID="ltSTSLabel" runat="server" Text="Ship To Ship Transfer"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="ltSTSValue" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="ltSireLable" runat="server" Text="SIRE"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="ltSireValue" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
