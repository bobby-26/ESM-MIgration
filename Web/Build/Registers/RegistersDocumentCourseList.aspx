<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentCourseList.aspx.cs"
    Inherits="RegistersDocumentCourseList" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserGroup" Src="~/UserControls/UserControlUserGroup.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentCategory" Src="~/UserControls/UserControlDocumentCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document Course List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js">
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocumentCourseList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
            <telerik:RadTextBox runat="server" ID="txtLevel" Style="text-align: right;" Width="360px"
                Visible="false">
            </telerik:RadTextBox>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuDoumentCourseList" Title="Course" runat="server" OnTabStripCommand="DoumentCourseList_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtCourse" MaxLength="200" Width="450px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAbbreviation" runat="server" Text="Abbreviation"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAbbreviation" MaxLength="10" Width="250px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCBTCourseNumber" runat="server" Text="CBT Course Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Enabled="false" ID="txtCBTCourse" MaxLength="100" Width="250px"
                            CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCreatedDate" runat="server" Text="Created Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCreatedDate" runat="server" CssClass="readonlytextbox" Width="150px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCreatedBy" runat="server" Text="Created By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCreatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="250px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedDate" runat="server" Text="Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedDate" runat="server" CssClass="readonlytextbox" Width="150px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedBy" runat="server" Text="Updated By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="250px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDocumentType" runat="server" Text="Document Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucDocumentType" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="250px"
                            HardTypeCode="103" AutoPostBack="true" OnTextChangedEvent="EnableCBT" />
                        <%--<eluc:Quick runat="server" ID="ucDocumentType" CssClass="dropdown_mandatory" AppendDataBoundItems="true" QuickTypeCode="2" QuickList='<%# PhoenixRegistersQuick.ListQuick(0, 2) %>' />--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkActiveYesOrNo" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Document Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:DocumentCategory ID="ucCategory" runat="server" OnTextChangedEvent="ucCategory_TextChangedEvent" AutoPostBack="true" Width="350px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExpiryYN" runat="server" Text="Expiry Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkExpiry" AutoPostBack="true" OnCheckedChanged="chkExpiry_CheckedChanged" />
                        <%--</td>
                    <td>--%>
                        <telerik:RadNumericTextBox runat="server" ID="txtExpiryYNText" MaxLength="10" Width="50px" CssClass="input_mandatory">
                        </telerik:RadNumericTextBox>
                        <telerik:RadLabel ID="lblMonths" runat="server" Text="(Months)"></telerik:RadLabel>
                    </td>
                    <%--    <td></td>--%>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStage" runat="server" Text="Stage"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStage" runat="server" AppendDataBoundItems="true" Width="250px" AllowCustomText="true" EmptyMessage="Type to Select">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOffshoreMandatory" runat="server" Text="Mandatory Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkMandatoryYN" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYN_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMandatoryYN" Visible="false" runat="server" Text="Mandatory Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" Visible="false" ID="chkMandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSourceCaption" runat="server" Text="Source"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ddlSource" AppendDataBoundItems="true" Width="250px"
                            HardTypeCode="104" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAdditionalDoc" runat="server" Text='Show in "Additional Documents" on Crew Planner Y/N'></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAdditionalDocYN" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLevel" runat="server" Text="Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucLevel" AppendDataBoundItems="true" Width="250px"
                            HardTypeCode="118" ShortNameFilter="MGM,OPR" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAuthenticationReqYN" runat="server" Text="Requires Authentication Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAuthenticationReqYN" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourseOfficer" runat="server" Text="Course Officer"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCourseOfficer" runat="server" AppendDataBoundItems="true" Width="250px" AllowCustomText="true" EmptyMessage="Type to Select">
                            <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel7" runat="server" Text="Certificate attachment mandatory"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkcertreqyn" runat="server" Text=""></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="Acceptable Institutes"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadComboBox ID="chkAccpInst" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkAccpInst_SelectedIndexChanged"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" EnableCheckAllItemsCheckBox="true" Width="210px">
                        </telerik:RadComboBox>
                        <asp:ImageButton runat="server" ID="imgaccinst" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." Visible="false" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblShowinMasterChecklistYN" runat="server" Text="Show in Master's checklist onboard Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkShowinMasterChecklistYN" runat="server" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Number of days"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucNumberAmount" runat="server" CssClass="input_mandatory" DecimalPlace="0"
                            IsPositive="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPhotocopyAcceptableYN" runat="server" Text="Photocopy acceptable Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkPhotocopyAcceptableYN" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="Training Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="chktrainingtypeList" runat="server"
                            DataSource='<%# PhoenixRegistersQuick.ListQuick(1,192) %>'
                            DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="250px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSingleUse" runat="server" Text="Single use Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkSingleUse" runat="server" />
                    </td>

                </tr>

                <tr>
                    <td></td><td></td>
                    <td>
                        <telerik:RadLabel ID="lblMapInCompetenceSubcategoryYN" runat="server" Text="Map in Competence subcategory Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkMapinCompetencesubcategoryYN" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td></td><td></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel10" runat="server" Text="CRA / Short term acceptable"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkCRAAccept" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFlagEndorsement" runat="server" Text="Flag Endorsement Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkFlagEndorsement" runat="server" AutoPostBack="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel9" runat="server" Text="Flag Endorsement"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgflgendorsment" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lbltobedoneby" runat="server" Text="To be done by" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ToBeDoneBy ID="ucTobedoneby" Width="200px" runat="server" AppendDataBoundItems="true" Visible="false" />
                    </td>
                    <td></td>
                    <td></td>

                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>


                <tr>
                    <td>
                        <telerik:RadLabel ID="lblwaiver" runat="server" Text="Waiver Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkWaiverYN" Enabled="false" runat="server" AutoPostBack="true"
                            OnCheckedChanged="chkWaiverYN_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUserGroup" runat="server" Text="User Group to allow Waiver"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadComboBox ID="chkUserGroup" runat="server"
                            EmptyMessage="Type to select user group" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgusergroup" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                        <%--<eluc:UserGroup runat="server" ID="ucUserGroup" CssClass="input" Enabled="false" AppendDataBoundItems="true" />--%>
                    
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWaivetoNextStage" runat="server" Text="Waive to Next Stage Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkWaivetoNextStage" runat="server" AutoPostBack="true" />
                    </td>
                </tr>


                <tr runat="server" id="trMaker" visible="false">

                    <td>
                        <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddressType runat="server" ID="ucMaker" AddressType="130" Width="250px"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>



                <tr runat="server" id="trCategory" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Training Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubCategory" runat="server" Text="Training SubCategory"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubCategory" runat="server" AppendDataBoundItems="true" Width="250px">
                        </telerik:RadComboBox>
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
                        <%--   <telerik:RadComboBox ID="chkVesselTypeList" runat="server" 
                            EmptyMessage="Type to select vessel type" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgvessseltypelist" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDependentCourse" runat="server" Text="Alternate Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<eluc:Course ID="ucDepCourse" runat="server" AppendDataBoundItems="true" CssClass="input" />--%>

                        <%--  <telerik:RadComboBox ID="cblAlternateCourse" runat="server"
                            EmptyMessage="Type to select alternate course" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="250px">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgalternate" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="PMS Global Component"></telerik:RadLabel>

                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgcomponent" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterer"></telerik:RadLabel>
                    </td>
                    <td>

                        <%-- <telerik:RadComboBox ID="chkChartererList" runat="server" 
                            EmptyMessage="Type to select charter" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgcharterer" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblflag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <%-- <telerik:RadComboBox ID="ddlflag"  runat="server" DataTextField="FLDFLAGNAME" DataValueField="FLDCOUNTRYCODE"
                            EmptyMessage="Type to select flag" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>   --%>
                        <asp:ImageButton runat="server" ID="imgflagpicklist" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Owner"></telerik:RadLabel>

                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgowner" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Managment Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--  <telerik:RadComboBox ID="ddlcompany" runat="server" 
                            EmptyMessage="Type to select management" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                        <asp:ImageButton runat="server" ID="imgcompany" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel8" runat="server" Text="Applicable Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="lmgapplicablevessel" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                        <asp:LinkButton runat="server" ID="lnkvesselmodify" AlternateText="Travel" Width="10PX" Height="10PX"
                            CommandName="NONCOMPLIANCE" ToolTip="Vessel list modified">                                
                                <span class="icon"><i class="fas fa-bell-slash"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeaServiceAcceptedLieu" runat="server" Text="Sea Service Accepted Lieu"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkSeaServiceAcceptedLieu" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeaServiceAcceptedLieu_CheckedChanged" />
                    </td>
                    <%--  <td>
                        <telerik:RadLabel ID="lblSeaServiceDetails" Visible="false" runat="server" Text="Details"></telerik:RadLabel>
                    </td>--%>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSeaServiceDetails" MaxLength="100" Width="250px" TextMode="MultiLine" Rows="2">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                </tr>

                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>


                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblcbt" runat="server" Text="Note: For CBT Course please enter CBT Course Number "
                            CssClass="guideline_text">
                        </telerik:RadLabel>
                        <br />
                        <telerik:RadLabel ID="lblsource" runat="server" Text="Note: Source is mandatory if document type is Value Added"
                            CssClass="guideline_text">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
