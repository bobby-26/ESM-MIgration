<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersDocumentCourseCBTList.aspx.cs" Inherits="RegistersDocumentCourseCBTList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

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
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document CBT List</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDocumentCourseList" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server">
        </telerik:RadScriptManager>

        <div>
            <telerik:RadTextBox runat="server" ID="txtLevel" Style="text-align: right;" Width="360px"
                CssClass="input" Visible="false">
            </telerik:RadTextBox>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuDoumentCourseList" runat="server" OnTabStripCommand="DoumentCourseList_TabStripCommand"></eluc:TabStrip>
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCourse" CssClass="input_mandatory"></telerik:RadTextBox>
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
                        <telerik:RadLabel ID="lblAbbreviation" runat="server" Text="Abbreviation"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAbbreviation" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExpiryYN" runat="server" Text="Expiry Y/N" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkExpiry" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDocumentType" runat="server" Text="Document Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucDocumentType" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            HardTypeCode="103" AutoPostBack="true" OnTextChangedEvent="EnableCBT" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCBTCourseNumber" runat="server" Text="CBT Course Number"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" Enabled="false" ID="txtCBTCourse" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOffshoreMandatory" runat="server" Text="Offshore Mandatory Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkMandatoryYN" runat="server" AutoPostBack="true" OnCheckedChanged="chkMandatoryYN_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMandatoryYN" runat="server" Text="Mandatory Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkMandatory" />
                    </td>
                </tr>
                <tr>
                    <%--<td colspan="2"></td>
                    --%>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Waiver Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="RadCheckBox1" Enabled="true" runat="server" AutoPostBack="true"
                            OnCheckedChanged="chkWaiverYN_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="User Group to allow Waiver"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgusergroup" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                        <%-- <telerik:RadComboBox ID="RadComboBox1" runat="server"
                            EmptyMessage="Type to select user group" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="90%">
                        </telerik:RadComboBox>--%>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblLevel" runat="server" Text="Level" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucLevel" CssClass="input" AppendDataBoundItems="true" Visible="false"
                            HardTypeCode="118" ShortNameFilter="MGM,OPR" />
                    </td>

                </tr>
                <tr>
                    <td>

                        <telerik:RadLabel ID="lblCourseOfficer" runat="server" Text="Course Officer" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCourseOfficer" runat="server" CssClass="input" AppendDataBoundItems="true" Visible="false">
                            <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddressType runat="server" ID="ucMaker" AddressType="130" CssClass="input" Visible="false"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStage" runat="server" Text="Offshore Stage" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStage" runat="server" AppendDataBoundItems="true" CssClass="input" Visible="false">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Document Category" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:DocumentCategory ID="ucCategory" runat="server" CssClass="input" Visible="false" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSourceCaption" runat="server" Text="Source" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ddlSource" CssClass="input" AppendDataBoundItems="true" Visible="false"
                            HardTypeCode="104" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblwaiver" runat="server" Text="Waiver Y/N" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkWaiverYN" Enabled="false" runat="server" AutoPostBack="true" Visible="false"
                            OnCheckedChanged="chkWaiverYN_CheckedChanged" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAdditionalDoc" runat="server" Text='Show in "Additional Documents" on Crew Planner Y/N'></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAdditionalDocYN" runat="server" />
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
                        <telerik:RadLabel ID="lblUserGroup" runat="server" Text="User Group to allow Waiver" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<div style="height: 100px; width: 250px; overflow: auto;" class="input">--%>
                        <asp:CheckBoxList ID="chkUserGroup" RepeatDirection="Vertical" Enabled="false" runat="server" Visible="false">
                        </asp:CheckBoxList>
                        <%-- </div>--%>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCreatedDate" runat="server" Text="Created Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCreatedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCreatedBy" runat="server" Text="Created By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCreatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedDate" runat="server" Text="Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedBy" runat="server" Text="Updated By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>

                <tr runat="server" id="trCategory" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Training Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="input" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubCategory" runat="server" Text="Training SubCategory"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubCategory" runat="server" CssClass="input" AppendDataBoundItems="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgRankPicklist" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                        <%-- <div id="divRank" runat="server" class="input" style="overflow: auto; width: 200px; height: 90px">
                        <asp:CheckBoxList ID="chkRankList" runat="server" Height="100%" RepeatColumns="1"
                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                        </asp:CheckBoxList>
                    </div>--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <%-- <div id="divVesselType" runat="server" class="input" style="overflow: auto; width: 200px; height: 90px">
                        <asp:CheckBoxList ID="chkVesselTypeList" RepeatDirection="Vertical" runat="server">
                        </asp:CheckBoxList>
                    </div>--%>
                        <asp:ImageButton runat="server" ID="imgvessseltypelist" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterer"></telerik:RadLabel>
                    </td>
                    <td>
                        <%-- <div id="divCharterer" runat="server" class="input" style="overflow: auto; width: 200px; height: 90px">
                        <asp:CheckBoxList ID="chkChartererList" RepeatDirection="Vertical" runat="server">
                        </asp:CheckBoxList>
                    </div>    --%>
                        <asp:ImageButton runat="server" ID="imgcharterer" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltobedoneby" runat="server" Text="To be done by"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ToBeDoneBy ID="ucToBeDoneby" Width="200px" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowinMasterChecklistYN" runat="server" Text="Show in Master's checklist onboard Y/N" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkShowinMasterChecklistYN" runat="server" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPhotocopyAcceptableYN" runat="server" Text="Photocopy acceptable Y/N" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkPhotocopyAcceptableYN" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMapInCompetenceSubcategoryYN" runat="server" Text="Map in Competence subcategory Y/N" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkMapinCompetencesubcategoryYN" runat="server" Visible="false" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCreateCBTforCrew" runat="server" Text="Create CBT for existing Crew"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="ChkCreateCBTForCrew" runat="server" AutoPostBack="true" OnCheckedChanged="ChkCreateCBTForCrew_Checkedchanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCBTRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCBTRemarks" runat="server" CssClass="input" Height="50px" Rows="4"
                            TextMode="MultiLine" Width="90%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                  <tr>
                    <td>
                        <telerik:RadLabel ID="lblDependentCourse" runat="server" Text="Alternate Course"></telerik:RadLabel>
                    </td>
                    <td>

                        <%--   <asp:CheckBoxList ID="cblAlternateCourse" RepeatDirection="Vertical" runat="server" Visible="false">
                        </asp:CheckBoxList>--%>
                        <asp:ImageButton runat="server" ID="imgalternate" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblcbt" runat="server" Text="Note: For CBT Course please enter CBT Course Number "
                            CssClass="guideline_text">
                        </telerik:RadLabel>
                        <br />
                        <%--<telerik:RadLabel ID="lblsource" runat="server" Text="Note: Source is mandatory if document type is Value Added"
                        CssClass="guideline_text"></telerik:RadLabel>--%>
                    </td>
                </tr>
              
            </table>
        </div>
    </form>
</body>
</html>
