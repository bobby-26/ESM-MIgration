<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentLicenceList.aspx.cs"
    Inherits="RegistersDocumentLicenceList" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserGroup" Src="~/UserControls/UserControlUserGroup.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentCategory" Src="~/UserControls/UserControlDocumentCategory.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document Licence List</title>
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
            <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuDoumentLicenceList" runat="server" OnTabStripCommand="DoumentLicenceList_TabStripCommand" Title="Licence"></eluc:TabStrip>

            <table>
                <tr>
                    <td style="color: blue;" colspan="4">
                        <b>
                            <telerik:RadLabel ID="lblNote" runat="server" Text="Note:"></telerik:RadLabel>
                        </b>
                        <telerik:RadLabel ID="lblLevelisusedtoidentifythelicencesforthegivenrank" runat="server"
                            Text="Level is used to identify the licences for the given rank.">
                        </telerik:RadLabel>
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<telerik:RadLabel ID="lbl1stdigitindicatesthetypeofthelicenceEg2Deck3Engine4GMDSS5DCE9Endorsements"
                            runat="server" Text="1st digit indicates the type of the licence(Eg:2-Deck, 3-Engine, 4-GMDSS, 5-DCE, 9-Endorsements).">
                        </telerik:RadLabel>
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<telerik:RadLabel ID="lbl2nddigitisusedforfindingthelevelofthelicenceHighestLicencegetslowernumberEgincaseofDeck0MST1COandsoon"
                            runat="server" Text="2nd digit is used for finding the level of the licence.Highest Licence gets lower number(Eg:incase of Deck:0-MST,1-CO and so on).">
                        </telerik:RadLabel>
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<telerik:RadLabel ID="lbl3rddigitindicatesthevarianceinthelevelEg0full1Limited"
                            runat="server" Text="3rd digit indicates the variance in the level(Eg: 0-full,1-Limited).">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="3" style="height: 100%; width: 100%;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDocumentType" runat="server" Text="Document Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucDocumentType" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="250px"
                            HardTypeCode="80" />
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
                        <telerik:RadLabel ID="lblDocumentCategory" runat="server" Text="Document Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:DocumentCategory ID="ucCategory" runat="server" Width="250px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMandatoryYN" runat="server" Text="Mandatory Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkMandatoryYN" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYN_CheckedChanged"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOffcrew" runat="server" Text="Offcrew"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucOffcrew" AppendDataBoundItems="true" Width="250px"/>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExpiryYN" runat="server" Text="Expiry Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkExpiry" AutoPostBack="true" OnCheckedChanged="chkExpiry_CheckedChanged" />
                        <telerik:RadNumericTextBox runat="server" ID="txtExpiryYNText" MaxLength="10" Width="50px" CssClass="input_mandatory">
                        </telerik:RadNumericTextBox>
                        <telerik:RadLabel ID="lblMonths" runat="server" Text="(Months)"></telerik:RadLabel>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLicence" runat="server" Text="Licence"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLicence" MaxLength="50" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAdditionalDoc" runat="server" Text='Show in "Additional Documents"<br/> on Crew Planner Y/N'></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAdditionalDocYN" runat="server" AutoPostBack="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAbbreviation" runat="server" Text="Abbreviation"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAbbreviation" MaxLength="10" Width="360px" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblShowinMasterChecklistYN" runat="server" Text="Show in Master's checklist onboard Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkShowinMasterChecklistYN" runat="server" AutoPostBack="false" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblocimfname" runat="server" Text="OCIMF Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtOCIMFName" MaxLength="50" Width="360px" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPhotocopyAcceptableYN" runat="server" Text="Photocopy acceptable Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkPhotocopyAcceptableYN" runat="server" AutoPostBack="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStage" runat="server" Text="Stage"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStage" runat="server" AppendDataBoundItems="true" AllowCustomText="true" Width="250px" EmptyMessage="Type to Select">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAuthenticationReqYN" runat="server" Text="Requires Authentication Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAuthenticationReqYN" runat="server" AutoPostBack="false" />
                    </td>
                </tr>
                <tr>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLevel" runat="server" Text="Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLevel" Style="text-align: right;" Width="360px" MaxLength="3"
                            CssClass="input_mandatory">
                        </telerik:RadTextBox>

                        <%--  <ajaxToolkit:MaskedEditExtender ID="mskLevel" runat="server" TargetControlID="txtLevel"
                        Mask="999" MaskType="Number" InputDirection="RightToLeft">
                    </ajaxToolkit:MaskedEditExtender>--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel10" runat="server" Text="CRA / Short term acceptable"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkCRAAccept" runat="server" />
                    </td>


                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGroup" runat="server" Text="Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucGroup" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
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
                        <%-- <telerik:RadComboBox ID="chkUserGroup" runat="server"
                            EmptyMessage="Type to select user group" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%">
                        </telerik:RadComboBox>--%>
                    </td>
                </tr>
                <tr>
                     <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Waiver to next stage Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkwavetonextyn" runat="server" AutoPostBack="false"/>
                    </td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <telerik:RadLabel ID="lblAppliedTo" runat="server" Text="Applied To" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblFlagEndorsement" runat="server" Text="Flag Endorsement Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank runat="server" ID="ucRank" AppendDataBoundItems="true" Visible="false" />
                        <telerik:RadCheckBox ID="chkFlagEndorsement" runat="server" AutoPostBack="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Flag Endorsement"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgflgendorsment" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>

                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    
                </tr>               
               
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadComboBox ID="chkRankList" runat="server"
                            EmptyMessage="Type to select flag" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
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
                        <telerik:RadLabel ID="lblflag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <%-- <telerik:RadComboBox ID="ddlflag" runat="server" DataTextField="FLDFLAGNAME" DataValueField="FLDCOUNTRYCODE"
                            EmptyMessage="Type to select flag" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgflagpicklist" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>                  

                    <td>
                        <telerik:RadLabel ID="lblHigherDocument" runat="server" Text="Higher Document"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterer Standard" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>

                        <%-- <telerik:RadComboBox ID="chkChartererList" runat="server"
                            EmptyMessage="Type to select charter standard" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgalternate" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                        <asp:ImageButton runat="server" ID="imgcharterer" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" Visible="false"
                            ImageAlign="AbsMiddle" Text=".." />

                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Managment Company" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <%-- <telerik:RadComboBox ID="ddlcompany" runat="server"
                            EmptyMessage="Type to select management" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgcompany" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" Visible="false"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Owner" Visible="false"></telerik:RadLabel>

                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgowner" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" Visible="false"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
