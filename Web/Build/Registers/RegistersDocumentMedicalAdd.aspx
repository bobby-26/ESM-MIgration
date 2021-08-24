<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentMedicalAdd.aspx.cs" Inherits="Registers_RegistersDocumentMedicalAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserGroup" Src="~/UserControls/UserControlUserGroup.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentCategory" Src="~/UserControls/UserControlDocumentCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document Medical</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocumentLicenceList" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuDoumentLicenceList" runat="server" OnTabStripCommand="MenuDoumentLicenceList_TabStripCommand" Title="Medical"></eluc:TabStrip>


            <table cellpadding="2" cellspacing="3" style="height: 100%; width: 100%;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLicence" runat="server" Text="Test Name"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtTestname" MaxLength="50" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblDocumentCategory" runat="server" Text="Document Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:DocumentCategory ID="ucCategory" runat="server" OnTextChangedEvent="ucCategory_TextChangedEvent" AutoPostBack="true" Width="250px" />

                    </td>

                    <td>
                        <telerik:RadLabel ID="lblpni" runat="server" Text="P&I/UK P&I"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBoxList ID="cblPIAdd" Width="100px" runat="server" DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE" Direction="Horizontal">
                        </telerik:RadCheckBoxList>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtcode" MaxLength="50" Width="130px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text=" Active Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkActiveYesOrNo" AutoPostBack="false" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucFrequencyAdd" Width="130px" runat="server" HardTypeCode="250" AppendDataBoundItems="true" CssClass="input_mandatory"
                            HardList='<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 250) %>' />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblexpiry" runat="server" Text="Expiry Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkExpiryYNAdd" runat="server" OnCheckedChanged="chkExpiryYNAdd_CheckedChanged" AutoPostBack="true" />

                        <telerik:RadNumericTextBox runat="server" ID="txtExpiryYNText" MaxLength="10" Width="50px" CssClass="input_mandatory">
                        </telerik:RadNumericTextBox>
                        <telerik:RadLabel ID="lblMonths" runat="server" Text="(Months)"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStage" runat="server" Text="Stage"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStage" runat="server" AppendDataBoundItems="true" AllowCustomText="true" EmptyMessage="Type to Select">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblmantory" runat="server" Text="Mandatory Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkMandatoryYNEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYNEdit_CheckedChanged"></telerik:RadCheckBox>

                    </td>
                </tr>


                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Show in Additional Documents on Crew Planner Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAdditionDocYnAdd" runat="server"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Requires Authentication Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAuthReqYnAdd" runat="server"></telerik:RadCheckBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="Show in Master's checklist onboard Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkShowInMasterChecklistYNAdd" runat="server"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="Photocopy Acceptable Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkPhotocopyAcceptableYnAdd" runat="server"></telerik:RadCheckBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Show in Additional Documents on Crew Planner Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="RadCheckBox1" runat="server"></telerik:RadCheckBox>
                    </td>

                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWaiverYN" runat="server" Text="Waiver Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkWaiverYN" runat="server" AutoPostBack="true" Enabled="false"
                            OnCheckedChanged="chkWaiverYN_CheckedChanged" />
                    </td>


                    <td>
                        <telerik:RadLabel ID="lblUserGroup" runat="server" Text="User group to allow waiver"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgusergroup" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                        <%--<telerik:RadComboBox ID="chkUserGroup" runat="server"
                            EmptyMessage="Type to select user group" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%">
                        </telerik:RadComboBox>--%>
                    </td>

                </tr>

                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadComboBox ID="chkRankList" runat="server"
                            EmptyMessage="Type to select rank" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgRankPicklist" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadComboBox ID="chkVesselTypeList" runat="server"
                            EmptyMessage="Type to select vessel type" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgvessseltypelist" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterer Standard"></telerik:RadLabel>
                    </td>
                    <td>

                        <%--  <telerik:RadComboBox ID="chkChartererList" runat="server"
                            EmptyMessage="Type to select charter standard" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgcharterer" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblflag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <%-- <telerik:RadComboBox ID="ddlflag" runat="server" DataTextField="FLDFLAGNAME" DataValueField="FLDCOUNTRYCODE"
                            EmptyMessage="Type to select flag" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgflagpicklist" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmgr" runat="server" Text="Managment Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <%-- <telerik:RadComboBox ID="ddlcompany" runat="server"
                            EmptyMessage="Type to select management" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgcompany" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblowner" runat="server" Text="Owner"></telerik:RadLabel>

                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgowner" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>

                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
